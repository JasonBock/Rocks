using System;
using System.Collections.Generic;

namespace Rocks
{
	public sealed class RockRepository
		: IDisposable
	{
		private readonly List<IExpectations> rocks = new List<IExpectations>();

		public Expectations<T> Create<T>()
			where T : class
		{
			var expectations = new Expectations<T>();
			this.rocks.Add(expectations);
			return expectations;
		}

		public void Dispose()
		{
			foreach (var chunk in this.rocks)
			{
				chunk.Verify();
			}
		}
	}
}