using StuLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace StuLog.Common
{
	public class EventAggregator : IEventAggregator
	{
		public void Subscribe(IHandle handler, params string[] channels)
		{
			object obj = this.handlersLock;
			lock (obj)
			{
				EventAggregator.Handler handler2 = this.handlers.FirstOrDefault((EventAggregator.Handler x) => x.IsHandlerForInstance(handler));
				bool flag2 = handler2 == null;
				if (flag2)
				{
					this.handlers.Add(new EventAggregator.Handler(handler, channels));
				}
				else
				{
					handler2.SubscribeToChannels(channels);
				}
			}
		}

		public void Unsubscribe(IHandle handler, params string[] channels)
		{
			object obj = this.handlersLock;
			lock (obj)
			{
				EventAggregator.Handler handler2 = this.handlers.FirstOrDefault((EventAggregator.Handler x) => x.IsHandlerForInstance(handler));
				bool flag2 = handler2 != null && handler2.UnsubscribeFromChannels(channels);
				if (flag2)
				{
					this.handlers.Remove(handler2);
				}
			}
		}

		public void PublishWithDispatcher(object message, Action<Action> dispatcher, params string[] channels)
		{
			object obj = this.handlersLock;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				this.handlers.RemoveAll((EventAggregator.Handler x) => !x.IsAlive);
				Type messageType = message.GetType();
				EventAggregator.HandlerInvoker[] array = this.handlers.SelectMany((EventAggregator.Handler x) => x.GetInvokers(messageType, channels)).ToArray<EventAggregator.HandlerInvoker>();
				EventAggregator.HandlerInvoker[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					EventAggregator.HandlerInvoker invoker = array2[i];
					dispatcher(delegate
					{
						invoker.Invoke(message);
					});
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		public static readonly string DefaultChannel = "DefaultChannel";

		private readonly List<EventAggregator.Handler> handlers = new List<EventAggregator.Handler>();

		private readonly object handlersLock = new object();

		private class Handler
		{
			public bool IsAlive
			{
				get
				{
					return this.target.IsAlive;
				}
			}

			public Handler(object handler, string[] channels)
			{
				Type type = handler.GetType();
				this.target = new WeakReference(handler);
				foreach (Type type2 in from x in handler.GetType().GetInterfaces()
									   where x.IsGenericType && typeof(IHandle).IsAssignableFrom(x)
									   select x)
				{
					Type messageType = type2.GetGenericArguments()[0];
					this.invokers.Add(new EventAggregator.HandlerInvoker(this.target, type, messageType, type2.GetMethod("Handle")));
				}
				bool flag = channels.Length == 0;
				if (flag)
				{
					channels = EventAggregator.Handler.DefaultChannelArray;
				}
				this.SubscribeToChannels(channels);
			}

			public bool IsHandlerForInstance(object subscriber)
			{
				return this.target.Target == subscriber;
			}

			public void SubscribeToChannels(string[] channels)
			{
				this.channels.UnionWith(channels);
			}

			public bool UnsubscribeFromChannels(string[] channels)
			{
				bool flag = channels.Length == 0;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					this.channels.ExceptWith(channels);
					result = (this.channels.Count == 0);
				}
				return result;
			}

			public IEnumerable<EventAggregator.HandlerInvoker> GetInvokers(Type messageType, string[] channels)
			{
				bool flag = !this.IsAlive;
				IEnumerable<EventAggregator.HandlerInvoker> result;
				if (flag)
				{
					result = Enumerable.Empty<EventAggregator.HandlerInvoker>();
				}
				else
				{
					bool flag2 = channels.Length == 0;
					if (flag2)
					{
						channels = EventAggregator.Handler.DefaultChannelArray;
					}
					bool flag3 = !channels.Any((string x) => this.channels.Contains(x));
					if (flag3)
					{
						result = Enumerable.Empty<EventAggregator.HandlerInvoker>();
					}
					else
					{
						result = from x in this.invokers
								 where x.CanInvoke(messageType)
								 select x;
					}
				}
				return result;
			}

			private static readonly string[] DefaultChannelArray = new string[]
			{
				EventAggregator.DefaultChannel
			};

			private readonly WeakReference target;

			private readonly List<EventAggregator.HandlerInvoker> invokers = new List<EventAggregator.HandlerInvoker>();

			private readonly HashSet<string> channels = new HashSet<string>();
		}

		private class HandlerInvoker
		{
			public HandlerInvoker(WeakReference target, Type targetType, Type messageType, MethodInfo invocationMethod)
			{
				this.target = target;
				this.messageType = messageType;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "target");
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "message");
				UnaryExpression instance = Expression.Convert(parameterExpression, targetType);
				UnaryExpression unaryExpression = Expression.Convert(parameterExpression2, messageType);
				MethodCallExpression body = Expression.Call(instance, invocationMethod, new Expression[]
				{
					unaryExpression
				});
				this.invoker = Expression.Lambda<Action<object, object>>(body, new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile();
			}

			public bool CanInvoke(Type messageType)
			{
				return this.messageType.IsAssignableFrom(messageType);
            }

            public void Invoke(object message)
            {
                object obj = this.target.Target;
                bool flag = obj != null;
				if (flag)
                {
					this.invoker(obj, message);
				}
			}

			private readonly WeakReference target;

			private readonly Type messageType;

			private readonly Action<object, object> invoker;
		}
	}
}
