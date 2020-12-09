using System;
using System.Collections.Generic;

namespace Rocks
{
	public sealed class RockRepository
		: IDisposable
	{
		private readonly List<IExpectations> rocks = new List<IExpectations>();

		public Expectations<T> Manage<T>(Func<Expectations<T>> creator)
			where T : class
		{
			var rock = creator();
			this.rocks.Add(rock);
			return rock;
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