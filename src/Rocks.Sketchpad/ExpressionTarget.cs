using System;

namespace Rocks.Sketchpad
{
	internal static partial class ExpressionEvaluation
	{
		public class ExpressionTarget
		{
			public void TargetWithNothing() { }
			public void Target(int a, string b) { }
			public void TargetWithNonLiteralValue(int a, string b, Guid c) { }
		}
	}
}