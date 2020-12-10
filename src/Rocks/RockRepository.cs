using System;
using System.Collections.Generic;

namespace Rocks
{
	public sealed class RockRepository
		: IDisposable
	{
		private readonly List<IExpectations> rocks = new List<IExpectations>();

		public Expectations<T> Add<T>(Expectations<T> expectations)
			where T : class
		{
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