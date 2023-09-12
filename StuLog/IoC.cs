using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog
{
    public class IoC
    {
        #region IoC搭建
        private static Dictionary<Type, Type> relations = new Dictionary<Type, Type>();

        public IoC()
        {

        }
        public void Register<TService>()
        {
            relations[typeof(TService)] = typeof(TService);
        }
        public void Register<ITService, TService>()
        {
            relations[typeof(ITService)] = typeof(TService);
        }
        public TService Get<TService>()
        {
            if (relations.TryGetValue(typeof(TService), out var service))
            {
                return (TService)CreateInstance(service);
            }
            return default(TService);
        }
        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
        #endregion
    }
}
