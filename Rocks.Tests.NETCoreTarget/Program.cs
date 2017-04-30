using NUnit.Common;
using NUnitLite;
using System;
using System.Reflection;

namespace Rocks.Tests.NETCoreTarget
{
	class Program
	{
		static void Main(string[] args)
		{
			var wrapper = new ExtendedTextWrapper(Console.Out);
			new AutoRun(typeof(AsyncTests).GetTypeInfo().Assembly).Execute(args, wrapper, Console.In);
		}
	}
}