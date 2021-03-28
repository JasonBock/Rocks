using Rocks.Exceptions;
using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;

namespace Rocks.Expectations
{
	public class Expectations<T> 
		: IExpectations
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

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		/// <summary>
		/// This method is used by Rocks and is not intented to be used by developers.
		/// </summary>
		public ImmutableDictionary<int, ImmutableArray<HandlerInformation>> CreateHandlers() =>
			this.Handlers.ToImmutableDictionary(pair => pair.Key, kvp => kvp.Value.ToImmutableArray());

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		/// <summary>
		/// This method is used by Rocks and is not intented to be used by developers.
		/// </summary>
		public Expectations<TTarget> To<TTarget>()
			where TTarget : class => new(this.Handlers, this.Mocks);

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		/// <summary>
		/// This property is used by Rocks and is not intented to be used by developers.
		/// </summary>
		public Dictionary<int, List<HandlerInformation>> Handlers { get; } = new();

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		/// <summary>
		/// This method is used by Rocks and is not intented to be used by developers.
		/// </summary>
		public List<IMock> Mocks { get; } = new();
	}
}