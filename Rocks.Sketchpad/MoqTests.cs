using Moq;
using System;

namespace Rocks.Sketchpad
{
	public static class MoqTests
	{
		public static void Test()
		{
			var mock = new Mock<IHaveEvent>();

			var mock1 = mock.Object;
			var mock2 = mock.Object;

         var uses1 = new UsesEvent(mock1);
			var uses2 = new UsesEvent(mock2);
			mock.Raise(_ => _.Use += null, new EventArgs());

			Console.Out.WriteLine($"uses1.Data != null is {uses1.Data != null}");
			Console.Out.WriteLine($"uses2.Data != null is {uses2.Data != null}");
			Console.Out.WriteLine($"object.ReferenceEquals(mock1, mock2) is {object.ReferenceEquals(mock1, mock2)}");
		}
	}

	public interface IHaveEvent
	{
		event EventHandler Use;
	}

	public class UsesEvent
	{
		private IHaveEvent haveEvent;

		public UsesEvent(IHaveEvent haveEvent)
		{
			this.haveEvent = haveEvent;
			this.haveEvent.Use += (s, e) => this.Data = e;
		}

		public EventArgs Data { get; set; }
	}
}
