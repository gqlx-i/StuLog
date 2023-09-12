using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Interface
{
    public interface IHandle
    {
    }
    public interface IHandle<in TMessageType> : IHandle
    {
        void Handle(TMessageType message);
    }
}
