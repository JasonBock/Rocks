using BenchmarkDotNet.Attributes;

namespace Rocks.Performance;

/*

To test:

var lists = new HandlerLists();
Console.WriteLine(lists.AddDefault(4));
Console.WriteLine(lists.AddWithOneInitialization(4));
Console.WriteLine(lists.AddHandlers(4));

*/

[MemoryDiagnoser]
public class HandlerLists
{
	[Benchmark(Baseline = true)]
	[ArgumentsSource(nameof(Sizes))]
	public int AddDefault(int count)
	{
		List<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListDefault ??= [];
			if (handlers is null) { handlers = new(); }
			handlers.Add(@handler);
		}

		return count;
	}

	[Benchmark]
	[ArgumentsSource(nameof(Sizes))]
	public int AddLinkedList(int count)
	{
		LinkedList<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListDefault ??= [];
			if (handlers is null) { handlers = new(); }
			handlers.AddLast(@handler);
		}

		return count;
	}

	[Benchmark]
	[ArgumentsSource(nameof(Sizes))]
	public int AddHandlers(int count)
	{
		Handlers<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListDefault ??= [];

#pragma warning disable CA2000 // Dispose objects before losing scope
			if (handlers is null) { handlers = new(handler); }
#pragma warning restore CA2000 // Dispose objects before losing scope
			else { handlers.Add(@handler); }
		}

		return count;
	}

	[Benchmark]
	[ArgumentsSource(nameof(Sizes))]
	public int AddOneCapacity(int count)
	{
		List<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListDefault ??= new(1);
			if (handlers is null) { handlers = new(1); }
			handlers.Add(@handler);
		}

		return handlers!.Count;
	}

	[Benchmark]
	[ArgumentsSource(nameof(Sizes))]
	public int AddTwoCapacity(int count)
	{
		List<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListDefault ??= new(2);
			if (handlers is null) { handlers = new(2); }
			handlers.Add(@handler);
		}

		return handlers!.Count;
	}

	[Benchmark]
	[ArgumentsSource(nameof(Sizes))]
	public int AddWithOneInitialization(int count)
	{
		List<HandlerInt>? handlers = null;

		for (var i = 0; i < count; i++)
		{
			var handler = new HandlerInt { input = i };
			// TODO: the new hotness:
			// this.handlerListOneInitialization ??= new([handler]);
			if (handlers is null) { handlers = new([handler]); }
			else { handlers.Add(@handler); }
		}

		return handlers!.Count;
	}

	public static IEnumerable<object[]> Sizes()
	{
		yield return new object[] { 1 };
		yield return new object[] { 2 };
		yield return new object[] { 5 };
		yield return new object[] { 10 };
	}
}