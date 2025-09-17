What are some things to fix?

* FIXED - Suppressed tests, trying to find `UseValueTasksCorrectlyAnalyzer` doesn't work. I think it has something to do with `<Reference Include="$(PkgMicrosoft_CodeAnalysis_NetAnalyzers)\analyzers\dotnet\cs\Microsoft.CodeAnalysis.NetAnalyzers.dll" />`

All issues now have GitHub issues.

Errors:

```
Code: CS0200, Count: 2
Code: CS0266, Count: 2
Code: CS9101, Count: 16


ID: CS0200
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\Microsoft.EntityFrameworkCore.Metadata.IMutableComplexType_Partial_Rock_Create.g.cs(4104,13): error CS0200: Property or indexer 'IMutableComplexType.BaseType' cannot be assigned to -- it is read only
Code:
((global::Microsoft.EntityFrameworkCore.Metadata.IMutableComplexType)this.mock).BaseType

Error:

ID: CS0200
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\Microsoft.EntityFrameworkCore.Metadata.IMutableComplexType_Rock_Create.g.cs(4104,13): error CS0200: Property or indexer 'IMutableComplexType.BaseType' cannot be assigned to -- it is read only
Code:
((global::Microsoft.EntityFrameworkCore.Metadata.IMutableComplexType)this.mock).BaseType

Error:

ID: CS0266
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType_Rock_Create.g.cs(8620,103): error CS0266: Cannot implicitly convert type 'Microsoft.EntityFrameworkCore.Metadata.IMutableTypeBase' to 'Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType'. An explicit conversion exists (are you missing a cast?)
Code:
value

Error:

ID: CS0266
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType_Partial_Rock_Create.g.cs(8620,103): error CS0266: Cannot implicitly convert type 'Microsoft.EntityFrameworkCore.Metadata.IMutableTypeBase' to 'Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType'. An explicit conversion exists (are you missing a cast?)
Code:
value

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Partial_Rock_Create.g.cs(204,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Partial_Rock_Create.g.cs(247,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Partial_Rock_Create.g.cs(316,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Partial_Rock_Create.g.cs(359,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Rock_Create.g.cs(204,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Rock_Create.g.cs(247,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Rock_Create.g.cs(316,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Rock_Create.g.cs(359,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Partial_Rock_Make.g.cs(57,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Partial_Rock_Make.g.cs(66,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Rock_Make.g.cs(47,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Rock_Make.g.cs(56,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Rock_Make.g.cs(57,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.ITensor_Rock_Make.g.cs(66,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Partial_Rock_Make.g.cs(47,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute

Error:

ID: CS9101
Description: Rocks.Analysis\Rocks.Analysis.RockGenerator\System.Numerics.Tensors.IReadOnlyTensor_Partial_Rock_Make.g.cs(56,5): error CS9101: UnscopedRefAttribute can only be applied to struct or virtual interface instance methods and properties, and cannot be applied to constructors or init-only members.
Code:
global::System.Diagnostics.CodeAnalysis.UnscopedRefAttribute
```