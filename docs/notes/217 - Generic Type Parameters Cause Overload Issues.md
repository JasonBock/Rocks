I need to put some tests around the MethodModel, ParameterModel, and TypeParameterModel optional parameter I made.

    // Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs(63,47): error CS0111: Type 'CreateExpectationsOfIRequestOfobjectExtensions.RockIRequestOfobject' already defines a member called 'Send' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs", 63, 47, 63, 51).WithArguments("Send", "MockTests.CreateExpectationsOfIRequestOfobjectExtensions.RockIRequestOfobject"),
    // Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs(97,210): error CS0111: Type 'MethodExpectationsOfIRequestOfobjectExtensions' already defines a member called 'Send' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs", 97, 210, 97, 214).WithArguments("Send", "MockTests.MethodExpectationsOfIRequestOfobjectExtensions"),

So, we end up with two methods that only differ by return type.

If I can detect that, I can mark one as being implemented explicitly. 