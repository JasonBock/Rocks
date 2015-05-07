using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rocks.Sketchpad
{
	public static class OutParameters
	{
		public static void Test()
		{
			var outModifierParameter = typeof(IOutModifier).GetMethod(nameof(IOutModifier.OutModifier)).GetParameters()[0];
			var outAttributeModifierParameters = typeof(IOutAttributeModifier).GetMethod(nameof(IOutAttributeModifier.OutAttributeModifier)).GetParameters()[0];
			var outAndOutAttributeModifierParameters = typeof(IOutAndOutAttributeModifier).GetMethod(nameof(IOutAndOutAttributeModifier.OutAndOutAttributeModifier)).GetParameters()[0];

			Console.Out.WriteLine($"{outModifierParameter} {outModifierParameter.IsOut} {outModifierParameter.IsIn} {outModifierParameter.GetCustomAttribute<OutAttribute>() != null} {outModifierParameter.ParameterType.IsByRef} ");
			Console.Out.WriteLine($"{outAttributeModifierParameters} {outAttributeModifierParameters.IsOut} {outAttributeModifierParameters.IsIn} {outAttributeModifierParameters.GetCustomAttribute<OutAttribute>() != null} {outAttributeModifierParameters.ParameterType.IsByRef}");
			Console.Out.WriteLine($"{outAndOutAttributeModifierParameters} {outAndOutAttributeModifierParameters.IsOut} {outAndOutAttributeModifierParameters.IsIn} {outAndOutAttributeModifierParameters.GetCustomAttribute<OutAttribute>() != null} {outAndOutAttributeModifierParameters.ParameterType.IsByRef}");
		}
	}

	public interface IOutModifier
	{
		void OutModifier(out int a);
	}

	public class IOM : IOutModifier
	{
		public void OutModifier(out int a)
		{
			throw new NotImplementedException();
		}
	}

	public interface IOutAttributeModifier
	{
		void OutAttributeModifier([Out] int a);
	}

	public class IOAM : IOutAttributeModifier
	{
		public void OutAttributeModifier([Out] int a)
		{
			throw new NotImplementedException();
		}
	}

	public interface IOutAndOutAttributeModifier
	{
		void OutAndOutAttributeModifier([Out] out int a);
	}

	public class IOOAM : IOutAndOutAttributeModifier
	{
		public void OutAndOutAttributeModifier([Out]out int a)
		{
			throw new NotImplementedException();
		}
	}
}
