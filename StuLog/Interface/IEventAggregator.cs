using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Interface
{
    public interface IEventAggregator
    {
        void Subscribe(IHandle handler, params string[] channels);

        void Unsubscribe(IHandle handler, params string[] channels);

        void PublishWithDispatcher(object message, Action<Action> dispatcher, params string[] channels);
    }
}
