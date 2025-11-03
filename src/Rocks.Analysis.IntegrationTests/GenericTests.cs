using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.GenericTestTypes;

public class ReferenceTypeOne { }
public class ReferenceTypeTwo { }

public class ReferencedContainer<T> { }

public class GenericContainer
{
	// Add docs for multiple return values and using Callback()
	// public virtual Container<T> Set<T>() where T : class
	// public DbSet<TEntity> Set<TEntity() where TEntity : class
	public virtual ReferencedContainer<T> SetThings<T>() where T : class => new();
	public virtual TReturn Run<TReturn>() where TReturn : new() => new();
}

public interface IGenericContainer
{
	void Sprint<TReturn>();
	TReturn Run<TReturn>() where TReturn : new();
	TReturn Run<TInput, TReturn>(TInput input) where TReturn : new();
}

public interface ICacheUpdater<TObject, TKey>
  where TObject : notnull
  where TKey : notnull
{
	void Refresh(IEnumerable<TKey> keys);
}

public interface ISourceUpdater<TObject, TKey>
  : ICacheUpdater<TObject, TKey>
  where TObject : notnull
  where TKey : notnull
{
	void AddOrUpdate(TObject item);
	void Refresh(IEnumerable<TObject> items);
}

public static class GenericTests
{
	[Test]
	public static void CreateWithOverloadUsingDifferentTypeParameter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ISourceUpdaterCreateExpectations<string, Guid>>();
		expectations.Setups.Refresh(Arg.Any<IEnumerable<string>>());
		expectations.Setups.Refresh(Arg.Any<IEnumerable<Guid>>());

		var mock = expectations.Instance();
		mock.Refresh(["a"]);
		mock.Refresh([Guid.NewGuid()]);
	}

	[Test]
	public static void CreateWithMultipleExpectationsOfDifferentTypesForReturnThatUsesGenericFromClass()
	{
		var referencedContainerOne = new ReferencedContainer<ReferenceTypeOne>();
		var referencedContainerTwo = new ReferencedContainer<ReferenceTypeTwo>();

		using var context = new RockContext(); 
		var expectations = context.Create<GenericContainerCreateExpectations>();
		expectations.Setups.SetThings<ReferenceTypeOne>().ReturnValue(referencedContainerOne);
		expectations.Setups.SetThings<ReferenceTypeTwo>().ReturnValue(referencedContainerTwo);

		var mock = expectations.Instance();
	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(mock.SetThings<ReferenceTypeOne>(), Is.SameAs(referencedContainerOne));
			Assert.That(mock.SetThings<ReferenceTypeTwo>(), Is.EqualTo(referencedContainerTwo));
		}
	}

	[Test]
	public static void CreateWithMultipleExpectationsOfDifferentTypesForReturnFromClass()
	{
		var guidReturn = Guid.NewGuid();

		using var context = new RockContext(); 
		var expectations = context.Create<GenericContainerCreateExpectations>();
		expectations.Setups.Run<int>().ReturnValue(4);
		expectations.Setups.Run<Guid>().ReturnValue(guidReturn);

		var mock = expectations.Instance();
	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(mock.Run<int>(), Is.EqualTo(4));
			Assert.That(mock.Run<Guid>(), Is.EqualTo(guidReturn));
		}
	}

	[Test]
	public static void CreateWithMultipleExpectationsOfDifferentTypesForReturnFromInterface()
	{
		var guidReturn = Guid.NewGuid();

		using var context = new RockContext(); 
		var expectations = context.Create<IGenericContainerCreateExpectations>();
		expectations.Setups.Run<int>().ReturnValue(4);
		expectations.Setups.Run<Guid>().ReturnValue(guidReturn);

		var mock = expectations.Instance();
	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(mock.Run<int>(), Is.EqualTo(4));
			Assert.That(mock.Run<Guid>(), Is.EqualTo(guidReturn));
		}
	}

	[Test]
	public static void CreateWithMultipleExpectationsOfDifferentTypesForParameterAndReturnFromInterface()
	{
		var guidReturn = Guid.NewGuid();
		var guidArgument = Guid.NewGuid();

		using var context = new RockContext(); 
		var expectations = context.Create<IGenericContainerCreateExpectations>();
		expectations.Setups.Run<int, Guid>(4).ReturnValue(guidReturn);
		expectations.Setups.Run<Guid, int>(guidArgument).ReturnValue(5);

		var mock = expectations.Instance();
	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(mock.Run<int, Guid>(4), Is.EqualTo(guidReturn));
			Assert.That(mock.Run<Guid, int>(guidArgument), Is.EqualTo(5));
		}
	}
}