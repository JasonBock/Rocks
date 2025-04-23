Projects:

* Rocks.Runtime - This targets .NET 9, and contains types that are used at compile time and run time. The emphasis is on "run time". Ideally, these types should not be needed by the generator.
* Rocks.Runtime.Tests - This targets .NET 9, tests for Rocks.Runtime
* Rocks.Analysis - This targets NS 2.0, and contains the generator, suppressor, analyzer, and diagnostic. It also contains the models and generator types used to create the mock code.
* Rocks.Analysis.Tests - This targets .NET 9, tests for Rocks.Analysis. Should also have a reference to Rocks.Runtime, and a reference to that project when code is generated.
* Rocks.Analysis.IntegrationTests - This targets .NET 9, integration tests for Rocks.CompilerExtensions (basically what Rocks.IntegrationTests are right now)
* Rocks.Completions - This targets NS 2.0, and contains code fixes (which I don't think I have, but they could go here) and refactorings.
* Rocks.Completions.Tests - This targets .NET 9, tests for Rocks.Completions. Should also have a reference to Rocks.Runtime, and a reference to that project when code is generated.
* Rocks - This turns into the metapackage that bundles all of the projects into one package.


* Code creation
    * Should be `using Rocks.Runtime;`
    * What about projections? Instead of `Rocks.Projections`, change to `Rocks.Runtime.Projections`
    * In CodeGenTests, what to do about BuildType?

So how should the Rocks.csproj metapackage look?

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<Description>A mocking library based on the Compiler APIs (Roslyn + Mocks)</Description>
		<GeneratePackageOnBuild>true</GeneratePackageOnBuild>
		<NoWarn>$(NoWarn),RS1036</NoWarn>
		<PackageIcon>Icon.png</PackageIcon>
		<PackageId>Rocks</PackageId>
		<PackageLicenseFile>LICENSE</PackageLicenseFile>
		<PackageProjectUrl>https://github.com/jasonbock/rocks</PackageProjectUrl>
		<PackageReadmeFile>README.md</PackageReadmeFile>
		<PackageReleaseNotes>A changelog is available at https://github.com/JasonBock/Rocks/blob/main/changelog.md</PackageReleaseNotes>
		<PackageRequireLicenseAcceptance>false</PackageRequireLicenseAcceptance>
		<PackageTags>Mocking C# .NET</PackageTags>
		<PackageVersion>$(Version)</PackageVersion>
		<PublishRepositoryUrl>true</PublishRepositoryUrl>
		<RepositoryType>git</RepositoryType>
		<RepositoryUrl>https://github.com/jasonbock/rocks</RepositoryUrl>
	</PropertyGroup>
	<ItemGroup>
		<PackageReference Include="Microsoft.CodeAnalysis.Analyzers" PrivateAssets="all" />
		<PackageReference Include="Microsoft.CodeAnalysis.CSharp" PrivateAssets="all" />
		<PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers">
			<PrivateAssets>all</PrivateAssets>
			<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
		</PackageReference>
	</ItemGroup>
	<ItemGroup>
		<None Include="README.md" Pack="true" PackagePath="" />
		<None Include="../../LICENSE" Pack="true" PackagePath="" />
		<None Include="../Images/Icon.png" Pack="true" PackagePath="" Visible="false" />
		<None Include="../Rocks.Analysis/bin/$(Configuration)/netstandard2.0/Rocks.Analysis.dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
        <TfmSpecificPackageFile
            Include="../Rocks.Runtime/bin/$(Configuration)/$(TargetFramework)/Rocks.Runtime.dll"
            Pack="true"
            PackagePath="lib/$(TargetFramework)" />        
        <TfmSpecificPackageFile
            Include="../Rocks.Runtime/bin/$(Configuration)/$(TargetFramework)/Rocks.Runtime.xml"
            Pack="true"
            PackagePath="lib/$(TargetFramework)" />        
	</ItemGroup>
    <ItemGroup>
        <ProjectReference Include="../Rocks.Analysis/Rocks.Analysis.csproj" ReferenceOutputAssembly="false" />
        <ProjectReference Include="../Rocks.Runtime/Rocks.Runtime.csproj" ReferenceOutputAssembly="false" />
    </ItemGroup>    
</Project>
```