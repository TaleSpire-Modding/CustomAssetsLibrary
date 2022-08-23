using System;
using System.Collections.Generic;
using Bounce.BlobAssets;
using Bounce.ManagedCollections;
using Bounce.Singletons;
using Bounce.TaleSpire.AssetManagement;
using Bounce.Unmanaged;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CustomAssetsKind.Singleton
{
    public class CustomAssetDb : SimpleSingletonBehaviour<CustomAssetDb>
    {
        private static readonly Dictionary<NGuid, AssetPack> _assetPacks = new Dictionary<NGuid, AssetPack>(8);

        private static NativeHashMap<BoardAssetGuid, BlobView<CreatureData>> _creatures;
        private static readonly BList<DbGroupImpl> _creatureGroups = new BList<DbGroupImpl>(128);
        private static readonly TagCollection _creatureTags = new TagCollection(128);


        private static readonly List<Texture2D> _atlases = new List<Texture2D>(10);
        private static readonly Dictionary<BoardAssetGuid, Sprite> _icons = new Dictionary<BoardAssetGuid, Sprite>(512);


        public enum AssetPackOrigin
        {
            BuiltIn,
            External,
        }

        private class AssetPack
        {
            public readonly NGuid Id;
            public readonly string OptionalName;
            public readonly AssetPackOrigin Origin;
            public readonly BlobView<AssetPackIndex> AssetPackIndexView;
            public readonly HashSet<string> TileTags = new HashSet<string>();
            public readonly HashSet<string> PropTags = new HashSet<string>();
            public readonly HashSet<string> CreatureTags = new HashSet<string>();
            public readonly BList<DbGroupImpl> Groups = new BList<DbGroupImpl>();

            public AssetPack(
              NGuid id,
              string name,
              AssetPackOrigin origin,
              BlobView<AssetPackIndex> assetPackIndexView)
            {
                Id = id;
                Origin = origin;
                AssetPackIndexView = assetPackIndexView;
            }

            public DbEntryImpl MakeDbEntry(ref CustomData custom, Sprite icon)
            {
                return null;
            }
            
        }

        internal class DbEntryImpl : DbEntry
        {
            public readonly DbGroupImpl GroupImpl;

            public override DbGroup Group => (DbGroup)GroupImpl;

            public DbEntryImpl(
              BoardAssetGuid id,
              NGuid assetPackId,
              AssetPackOrigin packOrigin,
              string kind,
              string name,
              string description,
              DbGroupImpl group,
              string groupTagName,
              int groupTagOrder,
              ref BlobArray<BlobString> tags,
              bool isDeprecated,
              Sprite icon)
              : base(id, assetPackId, packOrigin, kind, name, description, groupTagName, groupTagOrder, ref tags, isDeprecated, icon)
            {
                GroupImpl = group;
            }
        }

        public abstract class DbEntry
        {
            public static readonly string[] EntryKindNames = Enum.GetNames(typeof(string));
            public readonly BoardAssetGuid Id;
            public readonly NGuid AssetPackId;
            public readonly AssetPackOrigin Origin;
            public readonly string Kind;
            public readonly string Name;
            public readonly string Description;
            public readonly string[] Tags;
            public readonly string GroupTagName;
            public readonly int GroupTagOrder;
            public readonly bool IsDeprecated;
            public readonly Sprite Icon;

            public abstract DbGroup Group { get; }

            public bool IsBuiltInAsset => Origin == CustomAssetDb.AssetPackOrigin.BuiltIn;

            protected DbEntry(
              BoardAssetGuid id,
              NGuid assetPackId,
              AssetPackOrigin packPackOrigin,
              string kind,
              string name,
              string description,
              string groupTagName,
              int groupTagOrder,
              ref BlobArray<BlobString> tags,
              bool isDeprecated,
              Sprite icon)
            {
                Id = id;
                AssetPackId = assetPackId;
                Origin = packPackOrigin;
                Kind = kind;
                Name = name;
                Description = description;
                GroupTagName = groupTagName;
                GroupTagOrder = groupTagOrder;
                IsDeprecated = isDeprecated;
                Icon = icon;
                Tags = new string[tags.Length];
                for (int index = 0; index < tags.Length; ++index)
                    Tags[index] = tags[index].ToString();
            }
        }

        public interface IDbCollection
        {
            string Name { get; }

            bool IsEmpty { get; }

            Enumerator GetEnumerator();

            ref struct Enumerator
            {
                private BList.ReadOnlyView<DbEntry> _secondList;
                private BList.ReadOnlyView<DbEntry> _currentList;
                private int _index;

                internal Enumerator(
                  BList<DbEntryImpl> firstEntries,
                  BList<DbEntryImpl> otherEntries)
                {
                    _currentList = firstEntries.Reinterpret<DbEntryImpl, DbEntry>();
                    _secondList = otherEntries.Reinterpret<DbEntryImpl, DbEntry>();
                    _index = -1;
                }

                public Enumerator(BList<DbEntry> entries)
                {
                    _secondList = new BList.ReadOnlyView<DbEntry>();
                    _currentList = entries.TakeReadonlyView();
                    _index = -1;
                }

                public DbEntry Current => _currentList[_index];

                public bool MoveNext()
                {
                    for (; !_currentList.IsCreated || _index + 1 >= _currentList.Length; _index = -1)
                    {
                        if (!_secondList.IsCreated)
                            return false;
                        _currentList = _secondList;
                        _secondList = new BList.ReadOnlyView<DbEntry>();
                    }
                    ++_index;
                    return true;
                }
            }
        }

        public abstract class DbGroup : IDbCollection
        {
            public string Name { get; }

            public abstract bool IsEmpty { get; }

            protected DbGroup(string name) => Name = name;

            public abstract IDbCollection.Enumerator GetEnumerator();
        }

        internal class DbGroupImpl : DbGroup
        {
            private readonly BList<DbEntryImpl> _builtInEntries = new BList<DbEntryImpl>();
            private readonly BList<DbEntryImpl> _externalEntries = new BList<DbEntryImpl>();

            public override bool IsEmpty => _builtInEntries.IsEmpty && _externalEntries.IsEmpty;

            public DbGroupImpl(string name)
              : base(name)
            {
            }

            public void Add(DbEntryImpl dbEntry)
            {
                if (dbEntry.IsBuiltInAsset)
                    _builtInEntries.Add(dbEntry);
                else
                    _externalEntries.Add(dbEntry);
            }

            public bool Remove(DbEntryImpl dbEntry) => _externalEntries.RemoveSwapBack(dbEntry);

            public override IDbCollection.Enumerator GetEnumerator() => new IDbCollection.Enumerator(_builtInEntries, _externalEntries);
        }

        private class TagCollection
        {
            private readonly BList<string> _tags;
            private readonly Dictionary<string, RefPair> _lookup;
            private readonly Dictionary<NGuid, HashSet<string>> _registered = new Dictionary<NGuid, HashSet<string>>();

            public TagCollection(int initSize)
            {
                _lookup = new Dictionary<string, RefPair>(initSize);
                _tags = new BList<string>(initSize);
            }

            public void SetAssetPackTags(NGuid assetPackId, HashSet<string> tags)
            {
                RemoveAssetPack(assetPackId);
                foreach (string tag in tags)
                {
                    RefPair refPair;
                    if (_lookup.TryGetValue(tag, out refPair))
                    {
                        _lookup[tag] = refPair.IncrementRefCount();
                    }
                    else
                    {
                        _lookup[tag] = new RefPair(_tags.Length);
                        _tags.Add(tag);
                    }
                }
                _registered[assetPackId] = tags;
            }

            public bool RemoveAssetPack(NGuid assetPackId)
            {
                HashSet<string> stringSet;
                if (!_registered.TryGetValue(assetPackId, out stringSet))
                    return false;
                _registered.Remove(assetPackId);
                foreach (string key in stringSet)
                {
                    RefPair refPair;
                    if (_lookup.TryGetValue(key, out refPair))
                    {
                        if (refPair.RefCount == 1)
                        {
                            _lookup.Remove(key);
                            _tags.RemoveAtSwapBack(refPair.Index);
                            if (refPair.Index != _tags.Length)
                            {
                                string tag = _tags[refPair.Index];
                                _lookup[tag] = _lookup[tag].Relocate(refPair.Index);
                            }
                        }
                        else
                            _lookup[key] = refPair.DecrementRefCount();
                    }
                }
                return true;
            }

            public (string, int) Search(string partialTag, int maxScore) => TagUtils.FindPotentialTagAndScore((IReadOnlyList<string>)_tags, partialTag, maxScore);

            private readonly struct RefPair
            {
                public readonly int Index;
                public readonly int RefCount;

                public RefPair(int index)
                  : this(index, 1)
                {
                }

                private RefPair(int index, int refCount)
                {
                    Index = index;
                    RefCount = refCount;
                }

                public RefPair IncrementRefCount() => new RefPair(Index, RefCount + 1);

                public RefPair DecrementRefCount() => new RefPair(Index, RefCount - 1);

                public RefPair Relocate(int index) => new RefPair(index, RefCount);
            }
        }
    }
}