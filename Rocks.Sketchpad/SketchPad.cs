using System;

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
}