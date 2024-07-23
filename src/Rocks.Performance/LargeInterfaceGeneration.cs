using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Rocks.Performance;

/*
https://github.com/moq/moq/issues/1350
I was curious to see how fast Rocks would
generate a mock from an interface with a lot of members.

Grabbed the interface from:
https://gist.github.com/rauhs/4cbe672e26dd6727e84f7b96c68dcf1f

The results:

|       Method |     Mean |    Error |   StdDev |      Gen0 |      Gen1 |     Gen2 | Allocated |
|------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| RunGenerator | 14.89 ms | 0.291 ms | 0.399 ms | 1750.0000 | 1218.7500 | 218.7500 |   9.62 MB |

This means it takes Rocks 15 ms to generate the mock code for the interface.
The memory allocation is admittedly not good. Something to look at
in the future.
*/

[MemoryDiagnoser]
public class LargeInterfaceGeneration
{
	private readonly CSharpCompilation compilation;
	private readonly CSharpGeneratorDriver driver;

	public LargeInterfaceGeneration()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: RockCreate<IHaveLotsOfMembers>]

			public interface IHaveLotsOfMembers
			{
				int SomeProp { get; }
				int SomeProp1 { get; }
				int SomeProp2 { get; }
				int SomeProp3 { get; }
				int SomeProp4 { get; }
				int SomeProp5 { get; }
				int SomeProp6 { get; }
				int SomeProp7 { get; }
				int SomeProp8 { get; }
				int SomeProp9 { get; }
				int SomeProp10 { get; }
				int SomeProp11 { get; }
				int SomeProp12 { get; }
				int SomeProp13 { get; }
				int SomeProp14 { get; }
				int SomeProp15 { get; }
				int SomeProp16 { get; }
				int SomeProp17 { get; }
				int SomeProp18 { get; }
				int SomeProp19 { get; }
				int SomeProp20 { get; }
				int SomeProp21 { get; }
				int SomeProp22 { get; }
				int SomeProp23 { get; }
				int SomeProp24 { get; }
				int SomeProp25 { get; }
				int SomeProp26 { get; }
				int SomeProp27 { get; }
				int SomeProp28 { get; }
				int SomeProp29 { get; }
				int SomeProp30 { get; }
				int SomeProp31 { get; }
				int SomeProp32 { get; }
				int SomeProp33 { get; }
				int SomeProp34 { get; }
				int SomeProp35 { get; }
				int SomeProp36 { get; }
				int SomeProp37 { get; }
				int SomeProp38 { get; }
				int SomeProp39 { get; }
				int SomeProp40 { get; }
				int SomeProp41 { get; }
				int SomeProp42 { get; }
				int SomeProp43 { get; }
				int SomeProp44 { get; }
				int SomeProp45 { get; }
				int SomeProp46 { get; }
				int SomeProp47 { get; }
				int SomeProp48 { get; }
				int SomeProp49 { get; }
				int SomeProp50 { get; }
				int SomeProp51 { get; }
				int SomeProp52 { get; }
				int SomeProp53 { get; }
				int SomeProp54 { get; }
				int SomeProp55 { get; }
				int SomeProp56 { get; }
				int SomeProp57 { get; }
				int SomeProp58 { get; }
				int SomeProp59 { get; }
				int SomeProp60 { get; }
				int SomeProp61 { get; }
				int SomeProp62 { get; }
				int SomeProp63 { get; }
				int SomeProp64 { get; }
				int SomeProp65 { get; }
				int SomeProp66 { get; }
				int SomeProp67 { get; }
				int SomeProp68 { get; }
				int SomeProp69 { get; }
				int SomeProp70 { get; }
				int SomeProp71 { get; }
				int SomeProp72 { get; }
				int SomeProp73 { get; }
				int SomeProp74 { get; }
				int SomeProp75 { get; }
				int SomeProp76 { get; }
				int SomeProp77 { get; }
				int SomeProp78 { get; }
				int SomeProp79 { get; }
				int SomeProp80 { get; }
				int SomeProp81 { get; }
				int SomeProp82 { get; }
				int SomeProp83 { get; }
				int SomeProp84 { get; }
				int SomeProp85 { get; }
				int SomeProp86 { get; }
				int SomeProp87 { get; }
				int SomeProp88 { get; }
				int SomeProp89 { get; }
				int SomeProp90 { get; }
				int SomeProp91 { get; }
				int SomeProp92 { get; }
				int SomeProp93 { get; }
				int SomeProp94 { get; }
				int SomeProp95 { get; }
				int SomeProp96 { get; }
				int SomeProp97 { get; }
				int SomeProp98 { get; }
				int SomeProp99 { get; }
				int SomeProp100 { get; }
				int SomeProp101 { get; }
				int SomeProp102 { get; }
				int SomeProp103 { get; }
				int SomeProp104 { get; }
				int SomeProp105 { get; }
				int SomeProp106 { get; }
				int SomeProp107 { get; }
				int SomeProp108 { get; }
				int SomeProp109 { get; }
				int SomeProp110 { get; }
				int SomeProp111 { get; }
				int SomeProp112 { get; }
				int SomeProp113 { get; }
				int SomeProp114 { get; }
				int SomeProp115 { get; }
				int SomeProp116 { get; }
				int SomeProp117 { get; }
				int SomeProp118 { get; }
				int SomeProp119 { get; }
				int SomeProp120 { get; }
				int SomeProp121 { get; }
				int SomeProp122 { get; }
				int SomeProp123 { get; }
				int SomeProp124 { get; }
				int SomeProp125 { get; }
				int SomeProp126 { get; }
				int SomeProp127 { get; }
				int SomeProp128 { get; }
				int SomeProp129 { get; }
				int SomeProp130 { get; }
				int SomeProp131 { get; }
				int SomeProp132 { get; }
				int SomeProp133 { get; }
				int SomeProp134 { get; }
				int SomeProp135 { get; }
				int SomeProp136 { get; }
				int SomeProp137 { get; }
				int SomeProp138 { get; }
				int SomeProp139 { get; }
				int SomeProp140 { get; }
				int SomeProp141 { get; }
				int SomeProp142 { get; }
				int SomeProp143 { get; }
				int SomeProp144 { get; }
				int SomeProp145 { get; }
				int SomeProp146 { get; }
				int SomeProp147 { get; }
				int SomeProp148 { get; }
				int SomeProp149 { get; }
				int SomeProp150 { get; }
				int SomeProp151 { get; }
				int SomeProp152 { get; }
				int SomeProp153 { get; }
				int SomeProp154 { get; }
				int SomeProp155 { get; }
				int SomeProp156 { get; }
				int SomeProp157 { get; }
				int SomeProp158 { get; }
				int SomeProp159 { get; }
				int SomeProp160 { get; }
				int SomeProp161 { get; }
				int SomeProp162 { get; }
				int SomeProp163 { get; }
				int SomeProp164 { get; }
				int SomeProp165 { get; }
				int SomeProp166 { get; }
				int SomeProp167 { get; }
				int SomeProp168 { get; }
				int SomeProp169 { get; }
				int SomeProp170 { get; }
				int SomeProp171 { get; }
				int SomeProp172 { get; }
				int SomeProp173 { get; }
				int SomeProp174 { get; }
				int SomeProp175 { get; }
				int SomeProp176 { get; }
				int SomeProp177 { get; }
				int SomeProp178 { get; }
				int SomeProp179 { get; }
				int SomeProp180 { get; }
				int SomeProp181 { get; }
				int SomeProp182 { get; }
				int SomeProp183 { get; }
				int SomeProp184 { get; }
				int SomeProp185 { get; }
				int SomeProp186 { get; }
				int SomeProp187 { get; }
				int SomeProp188 { get; }
				int SomeProp189 { get; }
				int SomeProp190 { get; }
				int SomeProp191 { get; }
				int SomeProp192 { get; }
				int SomeProp193 { get; }
				int SomeProp194 { get; }
				int SomeProp195 { get; }
				int SomeProp196 { get; }
				int SomeProp197 { get; }
				int SomeProp198 { get; }
				int SomeProp199 { get; }
				int SomeProp200 { get; }
				int SomeProp201 { get; }
				int SomeProp202 { get; }
				int SomeProp203 { get; }
				int SomeProp204 { get; }
				int SomeProp205 { get; }
				int SomeProp206 { get; }
				int SomeProp207 { get; }
				int SomeProp208 { get; }
				int SomeProp209 { get; }
				int SomeProp210 { get; }
				int SomeProp211 { get; }
				int SomeProp212 { get; }
				int SomeProp213 { get; }
				int SomeProp214 { get; }
				int SomeProp215 { get; }
				int SomeProp216 { get; }
				int SomeProp217 { get; }
				int SomeProp218 { get; }
				int SomeProp219 { get; }
				int SomeProp220 { get; }
				int SomeProp221 { get; }
				int SomeProp222 { get; }
				int SomeProp223 { get; }
				int SomeProp224 { get; }
				int SomeProp225 { get; }
				int SomeProp226 { get; }
				int SomeProp227 { get; }
				int SomeProp228 { get; }
				int SomeProp229 { get; }
				int SomeProp230 { get; }
				int SomeProp231 { get; }
				int SomeProp232 { get; }
				int SomeProp233 { get; }
				int SomeProp234 { get; }
				int SomeProp235 { get; }
				int SomeProp236 { get; }
				int SomeProp237 { get; }
				int SomeProp238 { get; }
				int SomeProp239 { get; }
				int SomeProp240 { get; }
				int SomeProp241 { get; }
				int SomeProp242 { get; }
				int SomeProp243 { get; }
				int SomeProp244 { get; }
				int SomeProp245 { get; }
				int SomeProp246 { get; }
				int SomeProp247 { get; }
				int SomeProp248 { get; }
				int SomeProp249 { get; }
				int SomeProp250 { get; }
				int SomeProp251 { get; }
				int SomeProp252 { get; }
				int SomeProp253 { get; }
				int SomeProp254 { get; }
				int SomeProp255 { get; }
				int SomeProp256 { get; }
				int SomeProp257 { get; }
				int SomeProp258 { get; }
				int SomeProp259 { get; }
				int SomeProp260 { get; }
				int SomeProp261 { get; }
				int SomeProp262 { get; }
				int SomeProp263 { get; }
				int SomeProp264 { get; }
				int SomeProp265 { get; }
				int SomeProp266 { get; }
				int SomeProp267 { get; }
				int SomeProp268 { get; }
				int SomeProp269 { get; }
				int SomeProp270 { get; }
				int SomeProp271 { get; }
				int SomeProp272 { get; }
				int SomeProp273 { get; }
				int SomeProp274 { get; }
				int SomeProp275 { get; }
				int SomeProp276 { get; }
				int SomeProp277 { get; }
				int SomeProp278 { get; }
				int SomeProp279 { get; }
				int SomeProp280 { get; }
				int SomeProp281 { get; }
				int SomeProp282 { get; }
				int SomeProp283 { get; }
				int SomeProp284 { get; }
				int SomeProp285 { get; }
				int SomeProp286 { get; }
				int SomeProp287 { get; }
				int SomeProp288 { get; }
				int SomeProp289 { get; }
				int SomeProp290 { get; }
				int SomeProp291 { get; }
				int SomeProp292 { get; }
				int SomeProp293 { get; }
				int SomeProp294 { get; }
				int SomeProp295 { get; }
				int SomeProp296 { get; }
				int SomeProp297 { get; }
				int SomeProp298 { get; }
				int SomeProp299 { get; }
				int SomeProp300 { get; }
				int SomeProp301 { get; }
				int SomeProp302 { get; }
				int SomeProp303 { get; }
				int SomeProp304 { get; }
				int SomeProp305 { get; }
				int SomeProp306 { get; }
				int SomeProp307 { get; }
				int SomeProp308 { get; }
				int SomeProp309 { get; }
				int SomeProp310 { get; }
				int SomeProp311 { get; }
				int SomeProp312 { get; }
				int SomeProp313 { get; }
				int SomeProp314 { get; }
				int SomeProp315 { get; }
				int SomeProp316 { get; }
				int SomeProp317 { get; }
				int SomeProp318 { get; }
				int SomeProp319 { get; }
				int SomeProp320 { get; }
				int SomeProp321 { get; }
				int SomeProp322 { get; }
				int SomeProp323 { get; }
				int SomeProp324 { get; }
				int SomeProp325 { get; }
				int SomeProp326 { get; }
				int SomeProp327 { get; }
				int SomeProp328 { get; }
				int SomeProp329 { get; }
				int SomeProp330 { get; }
				int SomeProp331 { get; }
				int SomeProp332 { get; }
				int SomeProp333 { get; }
				int SomeProp334 { get; }
				int SomeProp335 { get; }
				int SomeProp336 { get; }
				int SomeProp337 { get; }
				int SomeProp338 { get; }
				int SomeProp339 { get; }
				int SomeProp340 { get; }
				int SomeProp341 { get; }
				int SomeProp342 { get; }
				int SomeProp343 { get; }
				int SomeProp344 { get; }
				int SomeProp345 { get; }
				int SomeProp346 { get; }
				int SomeProp347 { get; }
				int SomeProp348 { get; }
				int SomeProp349 { get; }
				int SomeProp350 { get; }
				int SomeProp351 { get; }
				int SomeProp352 { get; }
				int SomeProp353 { get; }
				int SomeProp354 { get; }
				int SomeProp355 { get; }
				int SomeProp356 { get; }
				int SomeProp357 { get; }
				int SomeProp358 { get; }
				int SomeProp359 { get; }
				int SomeProp360 { get; }
				int SomeProp361 { get; }
				int SomeProp362 { get; }
				int SomeProp363 { get; }
				int SomeProp364 { get; }
				int SomeProp365 { get; }
				int SomeProp366 { get; }
				int SomeProp367 { get; }
				int SomeProp368 { get; }
				int SomeProp369 { get; }
				int SomeProp370 { get; }
				int SomeProp371 { get; }
				int SomeProp372 { get; }
				int SomeProp373 { get; }
				int SomeProp374 { get; }
				int SomeProp375 { get; }
				int SomeProp376 { get; }
				int SomeProp377 { get; }
				int SomeProp378 { get; }
				int SomeProp379 { get; }
				int SomeProp380 { get; }
				int SomeProp381 { get; }
				int SomeMethod();
			}
			""";
		var tree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(
			[
				MetadataReference.CreateFromFile(typeof(RockGenerator).Assembly.Location),
			]);
		this.compilation = CSharpCompilation.Create("generator", [tree],
			references, new(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		this.driver = CSharpGeneratorDriver.Create(new RockGenerator());
	}

	[Benchmark]
	public Compilation RunGenerator()
	{
		this.driver.RunGeneratorsAndUpdateCompilation(this.compilation, out var outputCompilation, out var _);
		return outputCompilation;
	}
}