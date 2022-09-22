using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Rocks;
using Rocks.CodeGenerationTest;

TestWithCode();
//TestWithType();
//TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Microsoft.CodeAnalysis;
		using Rocks;
		using System;

		public static class Test
		{
			public static void Go()
			{
				var expectations = Rock.Create<Diagnostic>();
			}
		}
		""");
}

static void TestWithType() =>
	 TestGenerator.Generate(new RockCreateGenerator(), 
	 new[] 
	 { 
		 typeof(Microsoft.CodeAnalysis.Diagnostic)
	 });

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{ 
		// Core .NET types
		//typeof(object), typeof(Dictionary<,>),
		//typeof(System.Collections.Immutable.ImmutableArray), typeof(HttpMessageHandler),

		// ComputeSharp
		//typeof(ComputeSharp.AutoConstructorAttribute),
	
		// ComputeSharp.D2D1
		// ID2D1TransformMapperFactory will fail because it needs a struct 
		// that can be unmanaged and implement ID2D1PixelShader. If that's
		// done, then it works just fine.
		//typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),
	
		// CSLA
		//typeof(Csla.DataPortal<>),

		// Moq
		//typeof(Moq.Mock<>),

		// ImageSharp
		//typeof(SixLabors.ImageSharp.GraphicsOptions),

		// System.Text.Json
		//typeof(System.Text.Json.JsonDocument),

		// Microsoft.Extensions.DependencyInjection
		//typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),

		// Microsoft.CodeAnalysis.CSharp
		typeof(Microsoft.CodeAnalysis.SyntaxTree),

		// TerraFX.Interop.D3D12MemoryAllocator
		//typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),

		// TerraFX.Interop.Windows
		//typeof(TerraFX.Interop.INativeGuid),

		// Microsoft.Extensions.Logging
		//typeof(Microsoft.Extensions.Logging.LogDefineOptions),

		// Castle.Core
		//typeof(Castle.DynamicProxy.ProxyGenerationOptions),

		// Mono.Cecil
		//typeof(Mono.Cecil.FixedSysStringMarshalInfo),

		// AutoMapper
		//typeof(AutoMapper.AutoMapAttribute),

		// NUnit
		//typeof(NUnit.Framework.TestCaseAttribute),

		// System.Threading.Channels
		//typeof(System.Threading.Channels.BoundedChannelFullMode),

		// Serilog
		//typeof(Serilog.Core.IDestructuringPolicy),

		// Polly
		//typeof(Polly.AdvancedCircuitBreakerSyntax),

		// EntityFramework
		//typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),

		// FluentAssertions
		//typeof(FluentAssertions.AggregateExceptionExtractor),

		// StackExchange.Redis
		//typeof(StackExchange.Redis.Aggregate),

		// RestSharp
		//typeof(RestSharp.BodyParameter),

		// IdentityModel
		//typeof(IdentityModel.Base64Url),

		// Google.Protobuf
		//typeof(Google.Protobuf.ByteString),

		// CsvHelper
		//typeof(CsvHelper.ArrayHelper),

		// TODO: Azure.Identity, Antlr, SharpZipLib, MediatR, System.Reactive, 
		// NSubstitute, AWSSDK.Core, AngleSharp, MassTransit, Bogus, SkiaSharp,
		// ClangSharp, LLVMSharp, Silk.NET, System.Reflection.Metadata
	}.Select(_ => _.Assembly).ToHashSet();

	Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
	TestGenerator.Generate(new RockCreateGenerator(), targetAssemblies);
	Console.WriteLine();

	Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
	TestGenerator.Generate(new RockMakeGenerator(), targetAssemblies);
	Console.WriteLine();

	Console.WriteLine("Generator testing complete");
}

public class MyDiagnostic : Microsoft.CodeAnalysis.Operations.OperationWalker
{
	public override void DefaultVisit(Microsoft.CodeAnalysis.IOperation operation) => base.DefaultVisit(operation);
	public override bool Equals(object? obj) => base.Equals(obj);
	public override int GetHashCode() => base.GetHashCode();
	public override string? ToString() => base.ToString();
	public override void Visit(Microsoft.CodeAnalysis.IOperation? operation) => base.Visit(operation);
	public override void VisitAddressOf(IAddressOfOperation operation) => base.VisitAddressOf(operation);
	public override void VisitAnonymousFunction(IAnonymousFunctionOperation operation) => base.VisitAnonymousFunction(operation);
	public override void VisitAnonymousObjectCreation(IAnonymousObjectCreationOperation operation) => base.VisitAnonymousObjectCreation(operation);
	public override void VisitArgument(IArgumentOperation operation) => base.VisitArgument(operation);
	public override void VisitArrayCreation(IArrayCreationOperation operation) => base.VisitArrayCreation(operation);
	public override void VisitArrayElementReference(IArrayElementReferenceOperation operation) => base.VisitArrayElementReference(operation);
	public override void VisitArrayInitializer(IArrayInitializerOperation operation) => base.VisitArrayInitializer(operation);
	public override void VisitAwait(IAwaitOperation operation) => base.VisitAwait(operation);
	public override void VisitBinaryOperator(IBinaryOperation operation) => base.VisitBinaryOperator(operation);
	public override void VisitBinaryPattern(IBinaryPatternOperation operation) => base.VisitBinaryPattern(operation);
	public override void VisitBlock(IBlockOperation operation) => base.VisitBlock(operation);
	public override void VisitBranch(IBranchOperation operation) => base.VisitBranch(operation);
	public override void VisitCatchClause(ICatchClauseOperation operation) => base.VisitCatchClause(operation);
	public override void VisitCaughtException(ICaughtExceptionOperation operation) => base.VisitCaughtException(operation);
	public override void VisitCoalesce(ICoalesceOperation operation) => base.VisitCoalesce(operation);
	public override void VisitCoalesceAssignment(ICoalesceAssignmentOperation operation) => base.VisitCoalesceAssignment(operation);
	public override void VisitCollectionElementInitializer(ICollectionElementInitializerOperation operation) => base.VisitCollectionElementInitializer(operation);
	public override void VisitCompoundAssignment(ICompoundAssignmentOperation operation) => base.VisitCompoundAssignment(operation);
	public override void VisitConditional(IConditionalOperation operation) => base.VisitConditional(operation);
	public override void VisitConditionalAccess(IConditionalAccessOperation operation) => base.VisitConditionalAccess(operation);
	public override void VisitConditionalAccessInstance(IConditionalAccessInstanceOperation operation) => base.VisitConditionalAccessInstance(operation);
	public override void VisitConstantPattern(IConstantPatternOperation operation) => base.VisitConstantPattern(operation);
	public override void VisitConstructorBodyOperation(IConstructorBodyOperation operation) => base.VisitConstructorBodyOperation(operation);
	public override void VisitConversion(IConversionOperation operation) => base.VisitConversion(operation);
	public override void VisitDeclarationExpression(IDeclarationExpressionOperation operation) => base.VisitDeclarationExpression(operation);
	public override void VisitDeclarationPattern(IDeclarationPatternOperation operation) => base.VisitDeclarationPattern(operation);
	public override void VisitDeconstructionAssignment(IDeconstructionAssignmentOperation operation) => base.VisitDeconstructionAssignment(operation);
	public override void VisitDefaultCaseClause(IDefaultCaseClauseOperation operation) => base.VisitDefaultCaseClause(operation);
	public override void VisitDefaultValue(IDefaultValueOperation operation) => base.VisitDefaultValue(operation);
	public override void VisitDelegateCreation(IDelegateCreationOperation operation) => base.VisitDelegateCreation(operation);
	public override void VisitDiscardOperation(IDiscardOperation operation) => base.VisitDiscardOperation(operation);
	public override void VisitDiscardPattern(IDiscardPatternOperation operation) => base.VisitDiscardPattern(operation);
	public override void VisitDynamicIndexerAccess(IDynamicIndexerAccessOperation operation) => base.VisitDynamicIndexerAccess(operation);
	public override void VisitDynamicInvocation(IDynamicInvocationOperation operation) => base.VisitDynamicInvocation(operation);
	public override void VisitDynamicMemberReference(IDynamicMemberReferenceOperation operation) => base.VisitDynamicMemberReference(operation);
	public override void VisitDynamicObjectCreation(IDynamicObjectCreationOperation operation) => base.VisitDynamicObjectCreation(operation);
	public override void VisitEmpty(IEmptyOperation operation) => base.VisitEmpty(operation);
	public override void VisitEnd(IEndOperation operation) => base.VisitEnd(operation);
	public override void VisitEventAssignment(IEventAssignmentOperation operation) => base.VisitEventAssignment(operation);
	public override void VisitEventReference(IEventReferenceOperation operation) => base.VisitEventReference(operation);
	public override void VisitExpressionStatement(IExpressionStatementOperation operation) => base.VisitExpressionStatement(operation);
	public override void VisitFieldInitializer(IFieldInitializerOperation operation) => base.VisitFieldInitializer(operation);
	public override void VisitFieldReference(IFieldReferenceOperation operation) => base.VisitFieldReference(operation);
	public override void VisitFlowAnonymousFunction(IFlowAnonymousFunctionOperation operation) => base.VisitFlowAnonymousFunction(operation);
	public override void VisitFlowCapture(IFlowCaptureOperation operation) => base.VisitFlowCapture(operation);
	public override void VisitFlowCaptureReference(IFlowCaptureReferenceOperation operation) => base.VisitFlowCaptureReference(operation);
	public override void VisitForEachLoop(IForEachLoopOperation operation) => base.VisitForEachLoop(operation);
	public override void VisitForLoop(IForLoopOperation operation) => base.VisitForLoop(operation);
	public override void VisitForToLoop(IForToLoopOperation operation) => base.VisitForToLoop(operation);
	public override void VisitFunctionPointerInvocation(IFunctionPointerInvocationOperation operation) => base.VisitFunctionPointerInvocation(operation);
	public override void VisitImplicitIndexerReference(IImplicitIndexerReferenceOperation operation) => base.VisitImplicitIndexerReference(operation);
	public override void VisitIncrementOrDecrement(IIncrementOrDecrementOperation operation) => base.VisitIncrementOrDecrement(operation);
	public override void VisitInstanceReference(IInstanceReferenceOperation operation) => base.VisitInstanceReference(operation);
	public override void VisitInterpolatedString(IInterpolatedStringOperation operation) => base.VisitInterpolatedString(operation);
	public override void VisitInterpolatedStringAddition(IInterpolatedStringAdditionOperation operation) => base.VisitInterpolatedStringAddition(operation);
	public override void VisitInterpolatedStringAppend(IInterpolatedStringAppendOperation operation) => base.VisitInterpolatedStringAppend(operation);
	public override void VisitInterpolatedStringHandlerArgumentPlaceholder(IInterpolatedStringHandlerArgumentPlaceholderOperation operation) => base.VisitInterpolatedStringHandlerArgumentPlaceholder(operation);
	public override void VisitInterpolatedStringHandlerCreation(IInterpolatedStringHandlerCreationOperation operation) => base.VisitInterpolatedStringHandlerCreation(operation);
	public override void VisitInterpolatedStringText(IInterpolatedStringTextOperation operation) => base.VisitInterpolatedStringText(operation);
	public override void VisitInterpolation(IInterpolationOperation operation) => base.VisitInterpolation(operation);
	public override void VisitInvalid(IInvalidOperation operation) => base.VisitInvalid(operation);
	public override void VisitInvocation(IInvocationOperation operation) => base.VisitInvocation(operation);
	public override void VisitIsNull(IIsNullOperation operation) => base.VisitIsNull(operation);
	public override void VisitIsPattern(IIsPatternOperation operation) => base.VisitIsPattern(operation);
	public override void VisitIsType(IIsTypeOperation operation) => base.VisitIsType(operation);
	public override void VisitLabeled(ILabeledOperation operation) => base.VisitLabeled(operation);
	public override void VisitListPattern(IListPatternOperation operation) => base.VisitListPattern(operation);
	public override void VisitLiteral(ILiteralOperation operation) => base.VisitLiteral(operation);
	public override void VisitLocalFunction(ILocalFunctionOperation operation) => base.VisitLocalFunction(operation);
	public override void VisitLocalReference(ILocalReferenceOperation operation) => base.VisitLocalReference(operation);
	public override void VisitLock(ILockOperation operation) => base.VisitLock(operation);
	public override void VisitMemberInitializer(IMemberInitializerOperation operation) => base.VisitMemberInitializer(operation);
	public override void VisitMethodBodyOperation(IMethodBodyOperation operation) => base.VisitMethodBodyOperation(operation);
	public override void VisitMethodReference(IMethodReferenceOperation operation) => base.VisitMethodReference(operation);
	public override void VisitNameOf(INameOfOperation operation) => base.VisitNameOf(operation);
	public override void VisitNegatedPattern(INegatedPatternOperation operation) => base.VisitNegatedPattern(operation);
	public override void VisitObjectCreation(IObjectCreationOperation operation) => base.VisitObjectCreation(operation);
	public override void VisitObjectOrCollectionInitializer(IObjectOrCollectionInitializerOperation operation) => base.VisitObjectOrCollectionInitializer(operation);
	public override void VisitOmittedArgument(IOmittedArgumentOperation operation) => base.VisitOmittedArgument(operation);
	public override void VisitParameterInitializer(IParameterInitializerOperation operation) => base.VisitParameterInitializer(operation);
	public override void VisitParameterReference(IParameterReferenceOperation operation) => base.VisitParameterReference(operation);
	public override void VisitParenthesized(IParenthesizedOperation operation) => base.VisitParenthesized(operation);
	public override void VisitPatternCaseClause(IPatternCaseClauseOperation operation) => base.VisitPatternCaseClause(operation);
	public override void VisitPropertyInitializer(IPropertyInitializerOperation operation) => base.VisitPropertyInitializer(operation);
	public override void VisitPropertyReference(IPropertyReferenceOperation operation) => base.VisitPropertyReference(operation);
	public override void VisitPropertySubpattern(IPropertySubpatternOperation operation) => base.VisitPropertySubpattern(operation);
	public override void VisitRaiseEvent(IRaiseEventOperation operation) => base.VisitRaiseEvent(operation);
	public override void VisitRangeCaseClause(IRangeCaseClauseOperation operation) => base.VisitRangeCaseClause(operation);
	public override void VisitRangeOperation(IRangeOperation operation) => base.VisitRangeOperation(operation);
	public override void VisitRecursivePattern(IRecursivePatternOperation operation) => base.VisitRecursivePattern(operation);
	public override void VisitReDim(IReDimOperation operation) => base.VisitReDim(operation);
	public override void VisitReDimClause(IReDimClauseOperation operation) => base.VisitReDimClause(operation);
	public override void VisitRelationalCaseClause(IRelationalCaseClauseOperation operation) => base.VisitRelationalCaseClause(operation);
	public override void VisitRelationalPattern(IRelationalPatternOperation operation) => base.VisitRelationalPattern(operation);
	public override void VisitReturn(IReturnOperation operation) => base.VisitReturn(operation);
	public override void VisitSimpleAssignment(ISimpleAssignmentOperation operation) => base.VisitSimpleAssignment(operation);
	public override void VisitSingleValueCaseClause(ISingleValueCaseClauseOperation operation) => base.VisitSingleValueCaseClause(operation);
	public override void VisitSizeOf(ISizeOfOperation operation) => base.VisitSizeOf(operation);
	public override void VisitSlicePattern(ISlicePatternOperation operation) => base.VisitSlicePattern(operation);
	public override void VisitStaticLocalInitializationSemaphore(IStaticLocalInitializationSemaphoreOperation operation) => base.VisitStaticLocalInitializationSemaphore(operation);
	public override void VisitStop(IStopOperation operation) => base.VisitStop(operation);
	public override void VisitSwitch(ISwitchOperation operation) => base.VisitSwitch(operation);
	public override void VisitSwitchCase(ISwitchCaseOperation operation) => base.VisitSwitchCase(operation);
	public override void VisitSwitchExpression(ISwitchExpressionOperation operation) => base.VisitSwitchExpression(operation);
	public override void VisitSwitchExpressionArm(ISwitchExpressionArmOperation operation) => base.VisitSwitchExpressionArm(operation);
	public override void VisitThrow(IThrowOperation operation) => base.VisitThrow(operation);
	public override void VisitTranslatedQuery(ITranslatedQueryOperation operation) => base.VisitTranslatedQuery(operation);
	public override void VisitTry(ITryOperation operation) => base.VisitTry(operation);
	public override void VisitTuple(ITupleOperation operation) => base.VisitTuple(operation);
	public override void VisitTupleBinaryOperator(ITupleBinaryOperation operation) => base.VisitTupleBinaryOperator(operation);
	public override void VisitTypeOf(ITypeOfOperation operation) => base.VisitTypeOf(operation);
	public override void VisitTypeParameterObjectCreation(ITypeParameterObjectCreationOperation operation) => base.VisitTypeParameterObjectCreation(operation);
	public override void VisitTypePattern(ITypePatternOperation operation) => base.VisitTypePattern(operation);
	public override void VisitUnaryOperator(IUnaryOperation operation) => base.VisitUnaryOperator(operation);
	public override void VisitUsing(IUsingOperation operation) => base.VisitUsing(operation);
	public override void VisitUsingDeclaration(IUsingDeclarationOperation operation) => base.VisitUsingDeclaration(operation);
	public override void VisitUtf8String(IUtf8StringOperation operation) => base.VisitUtf8String(operation);
	public override void VisitVariableDeclaration(IVariableDeclarationOperation operation) => base.VisitVariableDeclaration(operation);
	public override void VisitVariableDeclarationGroup(IVariableDeclarationGroupOperation operation) => base.VisitVariableDeclarationGroup(operation);
	public override void VisitVariableDeclarator(IVariableDeclaratorOperation operation) => base.VisitVariableDeclarator(operation);
	public override void VisitVariableInitializer(IVariableInitializerOperation operation) => base.VisitVariableInitializer(operation);
	public override void VisitWhileLoop(IWhileLoopOperation operation) => base.VisitWhileLoop(operation);
	public override void VisitWith(IWithOperation operation) => base.VisitWith(operation);
}
#pragma warning restore CS8321 // Local function is declared but never used