using BenchmarkDotNet.Attributes;

namespace Rocks.Performance;

/*

To test:

var handlers = new Handlers<HandlerInt>(new HandlerInt { input = 0 });
handlers.Add(new HandlerInt { input = 1 });

foreach (var handler in handlers)
{
	Console.WriteLine(handler);
}

*/

[MemoryDiagnoser]
public class HandlerListsEnumeration
{
	[Benchmark]
	[ArgumentsSource(nameof(ListData))]
	public int EnumerateList(List<HandlerInt> handlers)
	{
		var i = 0;

#pragma warning disable CA1062 // Validate arguments of public methods
		foreach (var handler in handlers)
		{
			i++;
		}
#pragma warning restore CA1062 // Validate arguments of public methods

		return i;
	}

	[Benchmark]
	[ArgumentsSource(nameof(LinkedListData))]
	public int EnumerateLinkedList(LinkedList<HandlerInt> handlers)
	{
		var i = 0;

#pragma warning disable CA1062 // Validate arguments of public methods
		foreach (var handler in handlers)
		{
			i++;
		}
#pragma warning restore CA1062 // Validate arguments of public methods

		return i;
	}

	[Benchmark]
	[ArgumentsSource(nameof(HandlerData))]
	public int EnumerateHandlers(Handlers<HandlerInt> handlers)
	{
		var i = 0;

#pragma warning disable CA1062 // Validate arguments of public methods
		foreach (var handler in handlers)
		{
			i++;
		}
#pragma warning restore CA1062 // Validate arguments of public methods

		return i;
	}

	public static IEnumerable<List<HandlerInt>> ListData()
	{
		static List<HandlerInt> Generate(int size)
		{
			var handlers = new List<HandlerInt>(size);

			for (var i = 0; i < size; i++)
			{
				handlers.Add(new HandlerInt { input = i });
			}

			return handlers;
		}

		yield return Generate(1);
		yield return Generate(2);
		yield return Generate(5);
		yield return Generate(10);
	}

	public static IEnumerable<LinkedList<HandlerInt>> LinkedListData()
	{
		static LinkedList<HandlerInt> Generate(int size)
		{
			var handlers = new LinkedList<HandlerInt>();

			for (var i = 0; i < size; i++)
			{
				_ = handlers.AddLast(new HandlerInt { input = i });
			}

			return handlers;
		}

		yield return Generate(1);
		yield return Generate(2);
		yield return Generate(5);
		yield return Generate(10);
	}

	public static IEnumerable<Handlers<HandlerInt>> HandlerData()
	{
		static Handlers<HandlerInt> Generate(int size)
		{
			var handlers = new Handlers<HandlerInt>(new HandlerInt { input = 0 });

			for (var i = 1; i < size; i++)
			{
				handlers.Add(new HandlerInt { input = i });
			}

			return handlers;
		}

		yield return Generate(1);
		yield return Generate(2);
		yield return Generate(5);
		yield return Generate(10);
	}
}