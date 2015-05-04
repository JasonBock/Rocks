#pragma warning disable CS8019
using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
#pragma warning restore CS8019

namespace System.Runtime.InteropServices
{
	public sealed class RockInvalidComObjectException : InvalidComObjectException, IMock
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;
		public RockInvalidComObjectException(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers) : base()
		{
			this.handlers = handlers;
		}

		public RockInvalidComObjectException(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers, String message) : base(message)
		{
			this.handlers = handlers;
		}

		public RockInvalidComObjectException(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers, String message, Exception inner) : base(message, inner)
		{
			this.handlers = handlers;
		}

		public RockInvalidComObjectException(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers, SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.handlers = handlers;
		}

		public override Exception GetBaseException()
		{
			ReadOnlyCollection<HandlerInformation> methodHandlers = null;
			if (this.handlers.TryGetValue(100664506, out methodHandlers))
			{
				var methodHandler = methodHandlers[0];
				var result = methodHandler.Method != null ? (methodHandler.Method as Func<Exception>)() as Exception : (methodHandler as HandlerInformation<Exception>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override String ToString()
		{
			ReadOnlyCollection<HandlerInformation> methodHandlers = null;
			if (this.handlers.TryGetValue(100664519, out methodHandlers))
			{
				var methodHandler = methodHandlers[0];
				var result = methodHandler.Method != null ? (methodHandler.Method as Func<String>)() as String : (methodHandler as HandlerInformation<String>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			ReadOnlyCollection<HandlerInformation> methodHandlers = null;
			if (this.handlers.TryGetValue(100664525, out methodHandlers))
			{
				var foundMatch = false;
				foreach (var methodHandler in methodHandlers)
				{
					if ((methodHandler.Expectations["info"] as ArgumentExpectation<SerializationInfo>).IsValid(info, "info") && (methodHandler.Expectations["context"] as ArgumentExpectation<StreamingContext>).IsValid(context, "context"))
					{
						foundMatch = true;
						if (methodHandler.Method != null)
						{
							(methodHandler.Method as Action<SerializationInfo, StreamingContext>)(info, context);
						}

						methodHandler.RaiseEvents(this);
						methodHandler.IncrementCallCount();
						break;
					}
				}

				if (!foundMatch)
				{
					throw new ExpectationException($"No handlers were found for GetObjectData({info}, {context})");
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override Boolean Equals(Object obj)
		{
			ReadOnlyCollection<HandlerInformation> methodHandlers = null;
			if (this.handlers.TryGetValue(100663827, out methodHandlers))
			{
				foreach (var methodHandler in methodHandlers)
				{
					if ((methodHandler.Expectations["obj"] as ArgumentExpectation<Object>).IsValid(obj, "obj"))
					{
						var result = methodHandler.Method != null ? (Boolean)(methodHandler.Method as Func<Object, Boolean>)(obj) : (methodHandler as HandlerInformation<Boolean>).ReturnValue;
						methodHandler.RaiseEvents(this);
						methodHandler.IncrementCallCount();
						return result;
					}
				}

				throw new ExpectationException($"No handlers were found for Equals({obj})");
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override Int32 GetHashCode()
		{
			ReadOnlyCollection<HandlerInformation> methodHandlers = null;
			if (this.handlers.TryGetValue(100663830, out methodHandlers))
			{
				var methodHandler = methodHandlers[0];
				var result = methodHandler.Method != null ? (Int32)(methodHandler.Method as Func<Int32>)() : (methodHandler as HandlerInformation<Int32>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override String Message
		{
			get
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664500, out methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method != null ? (methodHandler.Method as Func<String>)() as String : (methodHandler as HandlerInformation<String>).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public override IDictionary Data
		{
			get
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664501, out methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method != null ? (methodHandler.Method as Func<IDictionary>)() as IDictionary : (methodHandler as HandlerInformation<IDictionary>).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public override String StackTrace
		{
			get
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664512, out methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method != null ? (methodHandler.Method as Func<String>)() as String : (methodHandler as HandlerInformation<String>).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public override String HelpLink
		{
			get
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664515, out methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method != null ? (methodHandler.Method as Func<String>)() as String : (methodHandler as HandlerInformation<String>).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result;
				}
				else
				{
					throw new NotImplementedException();
				}
			}

			set
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664516, out methodHandlers))
				{
					var foundMatch = false;
					foreach (var methodHandler in methodHandlers)
					{
						if ((methodHandler.Expectations["value"] as ArgumentExpectation<String>).IsValid(value, "value"))
						{
							foundMatch = true;
							if (methodHandler.Method != null)
							{
								(methodHandler.Method as Action<String>)(value);
							}

							methodHandler.RaiseEvents(this);
							methodHandler.IncrementCallCount();
							break;
						}
					}

					if (!foundMatch)
					{
						throw new ExpectationException($"No handlers were found for set_HelpLink({value})");
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public override String Source
		{
			get
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664517, out methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method != null ? (methodHandler.Method as Func<String>)() as String : (methodHandler as HandlerInformation<String>).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result;
				}
				else
				{
					throw new NotImplementedException();
				}
			}

			set
			{
				ReadOnlyCollection<HandlerInformation> methodHandlers = null;
				if (this.handlers.TryGetValue(100664518, out methodHandlers))
				{
					var foundMatch = false;
					foreach (var methodHandler in methodHandlers)
					{
						if ((methodHandler.Expectations["value"] as ArgumentExpectation<String>).IsValid(value, "value"))
						{
							foundMatch = true;
							if (methodHandler.Method != null)
							{
								(methodHandler.Method as Action<String>)(value);
							}

							methodHandler.RaiseEvents(this);
							methodHandler.IncrementCallCount();
							break;
						}
					}

					if (!foundMatch)
					{
						throw new ExpectationException($"No handlers were found for set_Source({value})");
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

#pragma warning disable CS0067
#pragma warning restore CS0067
		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> IMock.Handlers
		{
			get
			{
				return this.handlers;
			}
		}

		void IMock.Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();
			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[]
					{
						  this, args
					}

					);
				}
			}
		}
	}
}