using BenchmarkDotNet.Attributes;
using Rocks.Options;

namespace Rocks.Sketchpad
{
	[MemoryDiagnoser]
	public class MetadataReferenceCacheBenchmark
	{
		private readonly IRock<IA> iaRock;
		private readonly IRock<IB> ibRock;
		private readonly IRock<IC> icRock;
		private readonly IRock<ID> idRock;

		public MetadataReferenceCacheBenchmark()
		{
			var options = new RockOptions(caching: CachingOption.GenerateNewVersion);
			this.iaRock = Rock.Create<IA>(options);
			this.iaRock.Handle(_ => _.Foo());
			this.ibRock = Rock.Create<IB>(options);
			this.ibRock.Handle(_ => _.Foo());
			this.icRock = Rock.Create<IC>(options);
			this.icRock.Handle(_ => _.Foo());
			this.idRock = Rock.Create<ID>(options);
			this.idRock.Handle(_ => _.Foo());
		}

		[Benchmark]
		public IA CreateOneMock() =>
			this.iaRock.Make();

		[Benchmark]
		public IA CreateFourMocks()
		{
			this.ibRock.Make();
			this.icRock.Make();
			this.idRock.Make();
			return this.iaRock.Make();
		}
	}

	public interface IA
	{
		void Foo();
	}

	public interface IB
	{
		void Foo();
	}

	public interface IC
	{
		void Foo();
	}

	public interface ID
	{
		void Foo();
	}
}
