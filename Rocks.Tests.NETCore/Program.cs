using NUnit.Common;
using NUnitLite;
using System;
using System.Reflection;

namespace Rocks.Tests.NETCore
{
	class Program
	{
		static void Main(string[] args)
		{
			var wrapper = new ExtendedTextWrapper(Console.Out);
			new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args, wrapper, Console.In);

			//new Rocks.Tests.MultipleExpectationsTests().HandleMultiple();
		}
	}
}