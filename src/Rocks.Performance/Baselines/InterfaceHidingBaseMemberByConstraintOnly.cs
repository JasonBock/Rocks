using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class InterfaceHidingBaseMemberByConstraintOnly
	: BaselineTest
{
	public InterfaceHidingBaseMemberByConstraintOnly()
		: base(
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IServiceDerived), BuildType.Create | BuildType.Make)]
			
			public interface IOutBase { }
			
			public interface IOutDerived : IOutBase { }
			
			public interface IServiceBase
			{
				void TryGet<T>(out T value) where T : IOutBase;
			}
			
			public interface IServiceDerived : IServiceBase
			{
				new void TryGet<T>(out T value) where T : IOutDerived;
			}
			""")
	{ }
}