using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Interface
{
    public interface IParameter
    {
        void Read();
        void Write();
        //为什么要实现Copy
        void Copy(IParameter source);
    }
}
