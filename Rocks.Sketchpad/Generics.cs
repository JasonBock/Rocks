using System;
using System.Reflection;

namespace Rocks.Sketchpad
{
	public interface IBaseConstraint { }

	public interface IGeneric<T>
		where T : class
	{
		void MethodThatUsesT(T a, int b, T c);
		void MethodThatUsesItsOwnGeneric<U>(T a, U b);
		void MethodThatUsesItsOwnGenericWithNonTypeConstraints<U>(T a, U b)
			where U : struct;
		void MethodThatUsesItsOwnGenericWithTypeConstraints<U>(T a, U b)
			where U : IBaseConstraint;
	}

	public class Generic : IGeneric<string>
	{
		public void MethodThatUsesItsOwnGeneric<U>(string a, U b)
		{
			var x = new object();
			(x as Action<string, U>)(a, b);
			throw new NotImplementedException();
		}

		public void MethodThatUsesItsOwnGenericWithNonTypeConstraints<U>(string a, U b)
			where U : struct
		{
			throw new NotImplementedException();
		}

		public void MethodThatUsesItsOwnGenericWithTypeConstraints<U>(string a, U b)
			where U : IBaseConstraint
		{
			throw new NotImplementedException();
		}

		public void MethodThatUsesT(string a, int b, string c)
		{
			throw new NotImplementedException();
		}
	}

	public static class Generics
	{
		private static void TestGenerics()
		{
			var genericType = typeof(IGeneric<string>);

			var methodThatUsesTName = nameof(IGeneric<string>.MethodThatUsesT);
			Console.Out.WriteLine(methodThatUsesTName);
			var methodThatUsesT = genericType.GetMethod(methodThatUsesTName);
			Console.Out.WriteLine($"{nameof(methodThatUsesT.IsGenericMethod)}: {methodThatUsesT.IsGenericMethod}");
         Console.Out.WriteLine($"{nameof(methodThatUsesT.IsGenericMethodDefinition)}: {methodThatUsesT.IsGenericMethodDefinition}");
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericName = nameof(IGeneric<string>.MethodThatUsesItsOwnGeneric);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericName);
			var methodThatUsesItsOwnGeneric = genericType.GetMethod(methodThatUsesItsOwnGenericName);
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGeneric.IsGenericMethod)}: {methodThatUsesItsOwnGeneric.IsGenericMethod}");
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGeneric.IsGenericMethodDefinition)}: {methodThatUsesItsOwnGeneric.IsGenericMethodDefinition}");
			var methodThatUsesItsOwnGenericConstructed = methodThatUsesItsOwnGeneric.MakeGenericMethod(typeof(int));
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericConstructed.IsGenericMethod)}: {methodThatUsesItsOwnGenericConstructed.IsGenericMethod}");
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericConstructed.IsGenericMethodDefinition)}: {methodThatUsesItsOwnGenericConstructed.IsGenericMethodDefinition}");
			Generics.ShowGenericAttributes(methodThatUsesItsOwnGeneric);
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericWithNonTypeConstraintsName = nameof(IGeneric<string>.MethodThatUsesItsOwnGenericWithNonTypeConstraints);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraintsName);
			var methodThatUsesItsOwnGenericWithNonTypeConstraints = genericType.GetMethod(methodThatUsesItsOwnGenericWithNonTypeConstraintsName);
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethod)}: { methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethod}");
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethodDefinition)}: {methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethodDefinition}");
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraints.GetParameters()[0].ParameterType.Name);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraints.GetParameters()[1].ParameterType.Name);
			Generics.ShowGenericAttributes(methodThatUsesItsOwnGenericWithNonTypeConstraints);
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericWithTypeConstraintsName = nameof(IGeneric<string>.MethodThatUsesItsOwnGenericWithTypeConstraints);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithTypeConstraintsName);
			var methodThatUsesItsOwnGenericWithTypeConstraints = genericType.GetMethod(methodThatUsesItsOwnGenericWithTypeConstraintsName);
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethod)}: {methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethod}");
			Console.Out.WriteLine($"{nameof(methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethodDefinition)}: {methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethodDefinition}");
			Generics.ShowGenericAttributes(methodThatUsesItsOwnGenericWithTypeConstraints);
			Console.Out.WriteLine();
		}

		private static void ShowGenericAttributes(MethodInfo method)
		{
			foreach (var genericArgument in method.GetGenericArguments())
			{
				Console.Out.WriteLine($"{nameof(genericArgument.IsInterface)}: {genericArgument.IsInterface}");
				Console.Out.WriteLine($"{nameof(genericArgument.IsClass)}: {genericArgument.IsClass}");
				Console.Out.WriteLine($"{nameof(genericArgument.IsValueType)}: {genericArgument.IsValueType}");
				Console.Out.WriteLine(Generics.ListGenericParameterAttributes(genericArgument));
				Console.Out.WriteLine(genericArgument.Name);
				foreach (var constraint in genericArgument.GetGenericParameterConstraints())
				{
					Console.Out.WriteLine(constraint);
				}
			}
		}

		private static string ListGenericParameterAttributes(Type t)
		{
			var attributes = t.GenericParameterAttributes;
			var variance = attributes & GenericParameterAttributes.VarianceMask;
			var info = variance == GenericParameterAttributes.None ?
				"No variance flag;" :
				(variance & GenericParameterAttributes.Covariant) != 0 ?
					"Covariant;" : "Contravariant;";

			var constraints = attributes & GenericParameterAttributes.SpecialConstraintMask;

			if (constraints == GenericParameterAttributes.None)
			{
				info += " No special constraints";
			}
			else
			{
				if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
				{
					info += " ReferenceTypeConstraint";
				}
				if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
				{
					info += " NotNullableValueTypeConstraint";
				}
				if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
				{
					info += " DefaultConstructorConstraint";
				}
			}

			return info;
		}
	}
}
