using Rocks.Options;
using System;
using System.Collections.Generic;

namespace Rocks
{
	public sealed class RockRepository
		: IDisposable
	{
		private readonly List<IRock> rocks = new List<IRock>();

		public RockRepository()
			: this(new RockOptions()) { }

		public RockRepository(RockOptions options) =>
			this.Options = options ?? throw new ArgumentNullException(nameof(options));

		public IRock<T> Create<T>()
			where T : class
		{
			var rock = Rock.Create<T>(this.Options);
			this.rocks.Add(rock);
			return rock;
		}

		public (bool isSuccessful, IRock<T>? rock) TryCreate<T>()
			where T : class
		{
			var (isSuccessful, result) = Rock.TryCreate<T>(this.Options);

			if(isSuccessful)
			{
				var rock = result!;
				this.rocks.Add(rock);
				return (true, rock);
			}

			return (false, null);
		}

		public void Dispose()
		{
			foreach (var chunk in this.rocks)
			{
				chunk.Verify();
			}
		}

		public RockOptions Options { get; }
	}
}