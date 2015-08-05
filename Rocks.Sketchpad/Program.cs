using Moq;
using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var f = new Func<System.Threading.Tasks.Task<int>>(async () => await System.Threading.Tasks.Task.FromResult<int>(44));
			var q = f().Result;
			var m = new Mock<Async>();
			m.Setup(_ => _.Go()).Returns(async () => await System.Threading.Tasks.Task.FromResult<int>(44));
		}
	}
}
