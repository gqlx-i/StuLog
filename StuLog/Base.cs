using StuLog.Common;
using StuLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog
{
    public class Base
    {
        #region 懒汉式单例
        private static volatile Base _containerInstance;
        private static object locker = new Object();
        private Base()
        {

        }
        public static Base GetContainer()
        {
            if (_containerInstance == null)
            {
                lock (locker)
                {
                    if (_containerInstance == null)
                    {
                        _containerInstance = new Base();
                    }
                }
            }
            return _containerInstance;
        }
        #endregion

        #region 工厂单例
        private volatile static Dictionary<object, object> instanceDic = new Dictionary<object, object>();
        private volatile static object obj = null;
        private static readonly object objLock = new object();

        //public static T GetSingleton<T>() where T : class
        //{
        //    var x = typeof(T);
        //    try
        //    {
        //        if (obj == null)
        //        {
        //            lock (objLock)
        //            {
        //                if (obj == null)
        //                {
        //                    obj = Activator.CreateInstance(typeof(T));
        //                }
        //            }
        //        }
        //        return (T)obj;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static T GetSingleton<T>() where T : class
        {
            var item = typeof(T).GetHashCode();
            try
            {
                if (!instanceDic.TryGetValue(item, out obj))
                {
                    lock (objLock)
                    {
                        if (!instanceDic.TryGetValue(item, out obj))
                        {
                            obj = Activator.CreateInstance(typeof(T));
                            instanceDic.Add(item, obj); 
                        }
                    }
                }
                return (T)obj;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public static T GetParameter<T>() where T : IParameter
       {
            T t = (T)((Object)Activator.CreateInstance(typeof(T)));
            t.Read();
            return t;
            //IParameter parameter = this.fileMap.Find((IParameter w) => w.GetType().FullName == typeof(T).FullName);
            //bool flag = parameter != null;
            //T result;
            //if (flag)
            //{
            //    T t = (T)((object)Activator.CreateInstance(typeof(T)));
            //    t.Copy(parameter);
            //    t.UpdateCopy = new UpdateCopy(this.Handle);
            //    result = t;
            //}
            //else
            //{
            //    result = default(T);
            //}
            //return result;
        }
    }
}
