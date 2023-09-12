using StuLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuLog.Common
{
    public static class EventAggregatorExtensions
    {
        //public static void PublishOnUIThread(this IEventAggregator eventAggregator, object message, params string[] channels)
        //{
        //	eventAggregator.PublishWithDispatcher(message, new Action<Action>(Execute.OnUIThread), channels);
        //}

        public static void Publish(this IEventAggregator eventAggregator, object message, params string[] channels)
        {
            eventAggregator.PublishWithDispatcher(message, delegate (Action a)
            {
                a();
            }, channels);
        }
    }
}
