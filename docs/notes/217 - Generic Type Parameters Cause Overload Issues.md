I need to put some tests around the MethodModel, ParameterModel, and TypeParameterModel optional parameter I made.

    // Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs(63,47): error CS0111: Type 'CreateExpectationsOfIRequestOfobjectExtensions.RockIRequestOfobject' already defines a member called 'Send' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs", 63, 47, 63, 51).WithArguments("Send", "MockTests.CreateExpectationsOfIRequestOfobjectExtensions.RockIRequestOfobject"),
    // Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs(97,210): error CS0111: Type 'MethodExpectationsOfIRequestOfobjectExtensions' already defines a member called 'Send' with the same parameter types
    DiagnosticResult.CompilerError("CS0111").WithSpan("Rocks\Rocks.RockCreateGenerator\IRequestOfobject_Rock_Create.g.cs", 97, 210, 97, 214).WithArguments("Send", "MockTests.MethodExpectationsOfIRequestOfobjectExtensions"),

So, we end up with two methods that only differ by return type.

If I can detect that, I can mark one as being implemented explicitly. 

Testing RockCreateGenerator
Number of types found: 3847
Does generator compilation have any warning or error diagnostics? True
Was emit successful? False
6 errors, 0 warnings

Error:

ID: CS0311
Description: (625,54): error CS0311: The type 'object' cannot be used as type parameter 'TSaga' in the generic type or method 'RequestSettings<TSaga, TRequest, TResponse>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r615 = Rock.Create<MassTransit.RequestSettings<object, object, object>>();

Error:

ID: CS0311
Description: (1645,60): error CS0311: The type 'object' cannot be used as type parameter 'TInstance' in the generic type or method 'IRequestConfigurator<TInstance, TRequest, TResponse, TResponse2, TResponse3>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r1635 = Rock.Create<MassTransit.IRequestConfigurator<object, object, object, object, object>>();

Error:

ID: CS0311
Description: (1699,55): error CS0311: The type 'object' cannot be used as type parameter 'TSaga' in the generic type or method 'RequestSettings<TSaga, TRequest, TResponse, TResponse2>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r1689 = Rock.Create<MassTransit.RequestSettings<object, object, object, object>>();

Error:

ID: CS0311
Description: (2163,60): error CS0311: The type 'object' cannot be used as type parameter 'TInstance' in the generic type or method 'IRequestConfigurator<TInstance, TRequest, TResponse, TResponse2>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r2153 = Rock.Create<MassTransit.IRequestConfigurator<object, object, object, object>>();

Error:

ID: CS0311
Description: (3455,60): error CS0311: The type 'object' cannot be used as type parameter 'TInstance' in the generic type or method 'IRequestConfigurator<TInstance, TRequest, TResponse>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r3445 = Rock.Create<MassTransit.IRequestConfigurator<object, object, object>>();

Error:

ID: CS0311
Description: (3572,55): error CS0311: The type 'object' cannot be used as type parameter 'TSaga' in the generic type or method 'RequestSettings<TSaga, TRequest, TResponse, TResponse2, TResponse3>'. There is no implicit reference conversion from 'object' to 'MassTransit.SagaStateMachineInstance'.
Code:
                var r3562 = Rock.Create<MassTransit.RequestSettings<object, object, object, object, object>>();


Number of types found: 729
Does generator compilation have any warning or error diagnostics? False
Was emit successful? False
22 errors, 0 warnings

Error:

ID: CS0473
Description: Rocks\Rocks.RockCreateGenerator\IRequestClientOfobject_Rock_Create.g.cs(67,89): error CS0473: Explicit interface implementation 'CreateExpectationsOfIRequestClientOfobjectExtensions.RockIRequestClientOfobject.IRequestClient<object>.Create(object, CancellationToken, RequestTimeout)' matches more than one interface member. Which interface member is actually chosen is implementation-dependent. Consider using a non-explicit implementation instead.

"Rocks\\Rocks.RockCreateGenerator\\IRequestClientOfobject_Rock_Create.g.cs(67,155): error CS1066: The default value specified for parameter 'cancellationToken' will have no effect because it applies to a member that is used in contexts that do not allow optional arguments"

"Rocks\\Rocks.RockCreateGenerator\\IMessagePublishTopologyConfigurator_Rock_Create.g.cs(41,6): error CS0535: 'CreateExpectationsOfIMessagePublishTopologyConfiguratorExtensions.RockIMessagePublishTopologyConfigurator' does not implement interface member 'IMessagePublishTopology.Exclude.get'"

Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
   at Rocks.CodeGenerationTest.TestGenerator.Generate(IIncrementalGenerator generator, Type[] targetTypes, Type[] typesToLoadAssembliesFrom, Dictionary`2 genericTypeMappings) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 116
   at Rocks.CodeGenerationTest.TestGenerator.Generate(IIncrementalGenerator generator, HashSet`1 targetAssemblies, Type[] typesToLoadAssembliesFrom, Dictionary`2 genericTypeMappings) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\TestGenerator.cs:line 39
   at Program.<<Main>$>g__TestWithTypes|0_3() in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 315
   at Program.<Main>$(String[] args) in C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\Program.cs:line 13