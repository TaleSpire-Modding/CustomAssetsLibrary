using System;
using System.Reflection;

namespace CustomAssetsLibrary.ReflecExt
{
    public static class SysReflec
    {
        public static object call(this object o, string methodName, params object[] args)
        {
            var mi = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null) return mi.Invoke(o, args);
            return null;
        }

        public static object call<T>(this object o, string methodName, params object[] args)
        {
            var mi = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,  new []{typeof(T)}, null);
            if (mi != null) return mi.Invoke(o, args);
            return null;
        }

        public static void call<T>(string methodName, params object[] args)
        {
            var myClassType = Assembly.GetExecutingAssembly().GetType(typeof(T).Namespace); 
            myClassType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);
        }

        public static I GetValue<T,I>(string methodName)
        {
            var mi = typeof(T).GetField(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return (I) mi?.GetValue(null);
        }

        public static void SetValue(this object o, string methodName, object value)
        {
            var mi = o.GetType().GetField(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null) mi.SetValue(o, value);
        }

        public static Action<T1> GetMethod<T1>(this object o, string methodName)
        {
            var mi = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null) return (Action<T1>) Delegate.CreateDelegate(typeof(Action<T1>), mi);
            return null;
        }
    }
}