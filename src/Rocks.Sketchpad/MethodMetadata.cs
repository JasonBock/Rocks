using System;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Rocks.Sketchpad
{
	public class MethodMetadata
	{
		public string Data { get; set; }

		public static void Run()
		{
			var action = new Action<MethodMetadata>(_ => _.Data = "44");
		}
	}
}
