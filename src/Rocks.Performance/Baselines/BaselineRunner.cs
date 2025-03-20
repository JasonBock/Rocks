using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Reflection;

namespace Rocks.Performance.Baselines;

public static class BaselineRunner
{
	public static void Run(bool runBenchmarking = true)
	{
		// Get all the types in this assembly
		// that derive from BaselineTest
		foreach (var baselineTestType in
			typeof(BaselineTest).Assembly.GetTypes()
				.Where(_ => _ != typeof(BaselineTest) && _.IsAssignableTo(typeof(BaselineTest))))
		{
			try
			{
				if (runBenchmarking)
				{
					Console.WriteLine($"Running benchmark for {baselineTestType} ...");
					Console.WriteLine();

					// Execute "BenchmarkRunner.Run<baselineTestType>()"
					var runMethod = typeof(BenchmarkRunner).GetMethod(
						"Run", BindingFlags.Public | BindingFlags.Static, [typeof(IConfig), typeof(string[])])!;
					runMethod.MakeGenericMethod([baselineTestType]).Invoke(null, [null, null]);

					Console.WriteLine($"Benchmark complete for {baselineTestType}");
					Console.WriteLine();
				}
				else
				{
					Console.WriteLine($"Running test code for {baselineTestType} ...");
					Console.WriteLine();

					// Create an instance of the type and call
					// RunGenerator() on it.
					var baselineTest = (BaselineTest)Activator.CreateInstance(baselineTestType)!;
					_ = baselineTest.RunGenerator();

					Console.WriteLine($"Test code complete for {baselineTestType}");
					Console.WriteLine();
				}
			}
			catch (DiagnosticException e)
			{
				foreach (var diagnostic in e.Diagnostics)
				{
					Console.WriteLine($"{baselineTestType.Name} - {diagnostic}");
				}

				Console.WriteLine();
				throw;
			}
			catch (Exception e) when (e.InnerException is DiagnosticException de)
			{
				foreach (var diagnostic in de.Diagnostics)
				{
					Console.WriteLine($"{baselineTestType.Name} - {diagnostic}");
				}

				Console.WriteLine();
				throw;
			}
		}

		var grabbedSpecs = false;

		if (runBenchmarking)
		{
			var allResults = new List<string>
			{
				"# All Results",
				string.Empty
			};

			foreach (var resultFile in Directory.EnumerateFiles(
				Path.Combine(Environment.CurrentDirectory, @"BenchmarkDotNet.Artifacts\results"), "*-report-github.md"))
			{
				var results = File.ReadAllLines(resultFile).ToList();

				var headerLine = results.FindIndex(_ => _.StartsWith("| Method", StringComparison.CurrentCulture));

				if (!grabbedSpecs)
				{
					allResults.AddRange(results[..headerLine]);
					allResults.Add(string.Empty);
					grabbedSpecs = true;
				}

				allResults.Add($"## {Path.GetFileNameWithoutExtension(resultFile).Replace("-report-github", string.Empty, StringComparison.CurrentCulture)}");
				allResults.AddRange(results[headerLine..]);
				allResults.Add(string.Empty);
			}

			const string allResultsFile = "All Results.md";

			if (File.Exists(allResultsFile))
			{
				File.Delete(allResultsFile);
			}

			File.WriteAllLines(allResultsFile, allResults);
		}
	}
}