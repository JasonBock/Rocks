using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Exceptions;
using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks
{
	public class Expectations<T> 
		where T : class
	{
		internal Expectations() { }

		internal Expectations(Dictionary<int, List<HandlerInformation>> handlers, List<IMock> mocks) =>
			(this.Handlers, this.Mocks) = (handlers, mocks);

		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.Mocks)
			{
				failures.AddRange(rock.GetVerificationFailures());
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}

		public ImmutableDictionary<int, ImmutableArray<HandlerInformation>> CreateHandlers() =>
			this.Handlers.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.ToImmutableArray());

		public Expectations<TTarget> To<TTarget>()
			where TTarget : class => new Expectations<TTarget>(this.Handlers, this.Mocks);

		internal Dictionary<int, List<HandlerInformation>> Handlers { get; } = new();
		public List<IMock> Mocks { get; } = new();
	}
}