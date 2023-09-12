using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Common
{
    public static class CollectionsHelper
    {
        public static T Find<T>(this ObservableCollection<T> source, Predicate<T> match)
        {
            if (match == null)
            {
                return default;
            }
            for (int i = 0; i < source.Count; i++)
            {
                if (match(source[i]))
                {
                    return source[i];
                }
            }
            return default;
        }
    }
}
