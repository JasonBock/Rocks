using System;
using System.Linq;

namespace Rocks.Sketchpad
{
	public interface IMembers
	{
		void Method();
		int Property { get; set; }
		int ReadOnly { get; }
		int WriteOnly { set; }
		string this[int index] { get; set; }
		string this[string key] { get; set; }
		event EventHandler Event;
	}

	public class MembersImpl : IMembers
	{
		public string this[string key]
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public string this[int index]
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public int Property
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public int ReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public int WriteOnly
		{
			set
			{
				throw new NotImplementedException();
			}
		}

		public event EventHandler Event;

		public void Method()
		{
			throw new NotImplementedException();
		}
	}
	public static class Members
	{
		public static void Read()
		{
			var memberType = typeof(IMembers);

			foreach (var method in memberType.GetMethods().Where(_ => !_.IsSpecialName))
			{
				Console.Out.WriteLine($"Method: {method.Name}");
			}

			foreach (var property in memberType.GetProperties())
			{
				Console.Out.WriteLine($"Property: {property.Name}, {property.CanRead} - {property.CanWrite}");

				if(property.Name == "Item")
				{
					Console.Out.WriteLine($"Property {property.Name} is an indexer.");
            }
			}

			foreach (var @event in memberType.GetEvents())
			{
				Console.Out.WriteLine($"Event: {@event.Name}");
			}
		}
	}
}
