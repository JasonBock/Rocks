using ILGPU;
using ILGPU.Backends.PTX.Analyses;
using ILGPU.IR;
using ILGPU.IR.Analyses;
using ILGPU.IR.Analyses.ControlFlowDirection;
using ILGPU.IR.Analyses.TraversalOrders;
using ILGPU.IR.Construction;
using ILGPU.IR.Intrinsics;
using ILGPU.IR.Rewriting;
using ILGPU.IR.Transformations;
using ILGPU.IR.Types;
using ILGPU.IR.Values;
using ILGPU.Runtime;
using ILGPU.Util;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ILGPUMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(BaseIntrinsicValueMatcher<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
					}
				},
				{
					typeof(BlockFixPointAnalysis<,>), new()
					{
						{ "TData", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(CachedExtensionBase<>), new()
					{
						{ "TExtension", "global::ILGPU.Util.CachedExtension" },
					}
				},
				{
					typeof(CodePlacement<>), new()
					{
						{ "TStrategy", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedPlacementStrategy" },
					}
				},
				{
					typeof(ExtensionBase<>), new()
					{
						{ "TExtension", "global::ILGPU.Util.Extension" },
					}
				},
				{
					typeof(FixPointAnalysis<,,>), new()
					{
						{ "TData", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
						{ "TNode", "global::ILGPU.IR.Node" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(GlobalFixPointAnalysis<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatableStruct" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(GlobalFixPointAnalysis<,,>), new()
					{
						{ "TMethodData", "global::System.Object" },
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatableStruct" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(IAnalysisValueContext<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
					}
				},
				{
					typeof(IAnalysisValueSourceContext<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
					}
				},
				{
					typeof(IArrayView<,>), new()
					{
						{ "T", "int" },
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedGenericIndex" },
					}
				},
				{
					typeof(IArrayView<,,,>), new()
					{
						{ "T", "int" },
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntIndex" },
						{ "TLongIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedLongIndex" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntStride" },
					}
				},
				{
					typeof(IBasicBlockCollection<>), new()
					{
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(ICastableStride<,,>), new()
					{
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntIndex" },
						{ "TLongIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedLongIndex" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedStrideCastableIndex" },
					}
				},
				{
					typeof(IControlFlowAnalysisSource<>), new()
					{
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(IExtensionObject<>), new()
					{
						{ "TExtension", "global::ILGPU.Util.Extension" },
					}
				},
				{
					typeof(IFixPointAnalysisContext<,>), new()
					{
						{ "T", "global::System.Object" },
						{ "TNode", "global::ILGPU.IR.Node" },
					}
				},
				{
					typeof(IGenericIndex<>), new()
					{
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIndex" },
					}
				},
				{
					typeof(IGlobalFixPointAnalysisContext<,>), new()
					{
						{ "TMethodData", "global::System.Object" },
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
					}
				},
				{
					typeof(IIntIndex<,>), new()
					{
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntIndex" },
						{ "TLongIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedLongIndex" },
					}
				},
				{
					typeof(IIntrinsicImplementationTransformer<,>), new()
					{
						{ "TFirst", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
						{ "TSecond", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
					}
				},
				{
					typeof(ILongIndex<,>), new()
					{
						{ "TLongIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedLongIndex" },
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntIndex" },
					}
				},
				{
					typeof(IMemoryBuffer<>), new()
					{
						{ "TView", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedArrayView" },
					}
				},
				{
					typeof(IntrinsicValueMatcher<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
					}
				},
				{
					typeof(IntrinsicValueMatcher<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
						{ "TValueKind", "int" },
					}
				},
				{
					typeof(IRewriterContextProvider<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedRewriterContext" },
						{ "T", "global::System.Object" },
					}
				},
				{
					typeof(IStride<>), new()
					{
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedGenericIndex" },
					}
				},
				{
					typeof(IStride<,>), new()
					{
						{ "TIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntIndex" },
						{ "TLongIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedLongIndex" },
					}
				},
				{
					typeof(IStridedArrayView<,,>), new()
					{
						{ "T", "int" },
						{ "TStrideIndex", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedGenericIndex" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedStride" },
					}
				},
				{
					typeof(ITraversalSuccessorsProvider<>), new()
					{
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(ITypeConverter<>), new()
					{
						{ "TType", "global::ILGPU.IR.Types.TypeNode" },
					}
				},
				{
					typeof(KernelAccelerator<,>), new()
					{
						{ "TCompiledKernel", "global::ILGPU.Backends.CompiledKernel" },
						{ "TKernel", "global::ILGPU.Runtime.Kernel" },
					}
				},
				{
					typeof(LowerTypes<>), new()
					{
						{ "TType", "global::ILGPU.IR.Types.TypeNode" },
					}
				},
				{
					typeof(MemoryBuffer<>), new()
					{
						{ "TView", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedArrayView" },
					}
				},
				{
					typeof(MemoryBuffer1D<,>), new()
					{
						{ "T", "int" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedStride1D" },
					}
				},
				{
					typeof(MemoryBuffer2D<,>), new()
					{
						{ "T", "int" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedStride2D" },
					}
				},
				{
					typeof(MemoryBuffer3D<,>), new()
					{
						{ "T", "int" },
						{ "TStride", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedStride3D" },
					}
				},
				{
					typeof(Movement<>), new()
					{
						{ "TScope", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedMovementScope" },
					}
				},
				{
					typeof(PTXBlockSchedule<,>), new()
					{
						{ "TOrder", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedTraversalOrder" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
				{
					typeof(Rewriter<,,,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedRewriterContext" },
						{ "TContextProvider", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedRewriterContextProvider" },
						{ "TContextData", "global::System.Object" },
						{ "T", "global::System.Object" },
					}
				},
				{
					typeof(SSARewriter<>), new()
					{
						{ "TVariable", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
					}
				},
				{
					typeof(SSARewriter<,>), new()
					{
						{ "TVariable", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatable" },
						{ "T", "global::System.Object" },
					}
				},
				{
					typeof(TypeConverter<>), new()
					{
						{ "TType", "global::ILGPU.IR.Types.TypeNode" },
					}
				},
				{
					typeof(TypedIntrinsicValueMatcher<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedIntrinsicImplementation" },
						{ "TValueKind", "int" },
					}
				},
				{
					typeof(TypeLowering<>), new()
					{
						{ "TType", "global::ILGPU.IR.Types.TypeNode" },
					}
				},
				{
					typeof(ValueBuilder<>), new()
					{
						{ "TBuilder", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedValueBuilder" },
					}
				},
				{
					typeof(ValueFixPointAnalysis<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedEquatableStruct" },
						{ "TDirection", "global::Rocks.CodeGenerationTest.Mappings.ILGPU.MappedControlFlowDirection" },
					}
				},
			};
	}

	namespace ILGPU
	{
		public struct MappedArrayView
			: IArrayView
		{
			public MemoryBuffer Buffer => throw new NotImplementedException();

			public bool IsValid => throw new NotImplementedException();

			public long Length => throw new NotImplementedException();

			public int ElementSize => throw new NotImplementedException();

			public long LengthInBytes => throw new NotImplementedException();
		}

		public struct MappedControlFlowDirection
			 : IControlFlowDirection
		{
			public bool IsForwards => throw new NotImplementedException();

			public BasicBlock GetEntryBlock<TSource, TDirection>(in TSource source)
				where TSource : struct, IControlFlowAnalysisSource<TDirection>
				where TDirection : struct, IControlFlowDirection => throw new NotImplementedException();
		}

		public sealed class MappedEquatable
			 : IEquatable<MappedEquatable>
		{
			public bool Equals(MappedEquatable? other) => throw new NotImplementedException();
		}

		public struct MappedEquatableStruct
			 : IEquatable<MappedEquatableStruct>
		{
			public bool Equals(MappedEquatableStruct other) => throw new NotImplementedException();
		}

		public struct MappedGenericIndex
			: IGenericIndex<MappedGenericIndex>
		{
			public long Size => throw new NotImplementedException();

			public MappedGenericIndex Add(MappedGenericIndex rhs) => throw new NotImplementedException();
			public bool Equals(MappedGenericIndex other) => throw new NotImplementedException();
			public bool InBounds(MappedGenericIndex dimension) => throw new NotImplementedException();
			public bool InBoundsInclusive(MappedGenericIndex dimension) => throw new NotImplementedException();
			public MappedGenericIndex Subtract(MappedGenericIndex rhs) => throw new NotImplementedException();
		}

		public struct MappedIntIndex
			: IIntIndex<MappedIntIndex, MappedLongIndex>
		{
			public int Size => throw new NotImplementedException();

			public IndexType IndexType => throw new NotImplementedException();

			long IIndex.Size => throw new NotImplementedException();

			public MappedIntIndex Add(MappedIntIndex rhs) => throw new NotImplementedException();
			public bool Equals(MappedIntIndex other) => throw new NotImplementedException();
			public bool InBounds(MappedIntIndex dimension) => throw new NotImplementedException();
			public bool InBoundsInclusive(MappedIntIndex dimension) => throw new NotImplementedException();
			public MappedIntIndex Subtract(MappedIntIndex rhs) => throw new NotImplementedException();
			public MappedLongIndex ToLongIndex() => throw new NotImplementedException();
		}

		public struct MappedLongIndex
			 : ILongIndex<MappedLongIndex, MappedIntIndex>
		{
			public IndexType IndexType => throw new NotImplementedException();

			public long Size => throw new NotImplementedException();

			public MappedLongIndex Add(MappedLongIndex rhs) => throw new NotImplementedException();
			public bool Equals(MappedLongIndex other) => throw new NotImplementedException();
			public bool InBounds(MappedLongIndex dimension) => throw new NotImplementedException();
			public bool InBoundsInclusive(MappedLongIndex dimension) => throw new NotImplementedException();
			public MappedLongIndex Subtract(MappedLongIndex rhs) => throw new NotImplementedException();
			public MappedIntIndex ToIntIndex() => throw new NotImplementedException();
		}

		public struct MappedIndex
			  : IIndex
		{
			public long Size => throw new NotImplementedException();
		}

		public sealed class MappedIntrinsicImplementation
			  : IIntrinsicImplementation
		{ }

		public sealed class MappedMovementScope
			: IMovementScope
		{
			public bool TryFindFirstValueOf<T>(BasicBlock basicBlock, Predicate<T> predicate, out (int Index, T Value) entry) where T : Value => throw new NotImplementedException();
		}

		public struct MappedPlacementStrategy
			: CodePlacement.IPlacementStrategy
		{
			public int Count => throw new NotImplementedException();

			public void EnqueueChildren<TMover>(in TMover mover, in CodePlacement.PlacementEntry entry) where TMover : struct, CodePlacement.IMover => throw new NotImplementedException();
			public void Init(int capacity) => throw new NotImplementedException();
			public CodePlacement.PlacementEntry Pop() => throw new NotImplementedException();
			public void Push(in CodePlacement.PlacementEntry entry) => throw new NotImplementedException();
		}

		public struct MappedRewriterContext
			 : IRewriterContext
		{
			public BasicBlock.Builder Builder => throw new NotImplementedException();

			public BasicBlock Block => throw new NotImplementedException();

			public bool IsConverted(Value value) => throw new NotImplementedException();
			public bool MarkConverted(Value value) => throw new NotImplementedException();
			public void Remove(Value value) => throw new NotImplementedException();
			public TValue Replace<TValue>(Value value, TValue newValue) where TValue : Value => throw new NotImplementedException();
			public TValue ReplaceAndRemove<TValue>(Value value, TValue newValue) where TValue : Value => throw new NotImplementedException();
		}

		public struct MappedRewriterContextProvider
			: IRewriterContextProvider<MappedRewriterContext, object>
		{
			public MappedRewriterContext CreateContext(BasicBlock.Builder builder, HashSet<Value> converted, object data) => throw new NotImplementedException();
		}

		public struct MappedTraversalOrder
			: ITraversalOrder
		{
			public TraversalEnumerationState Init<TCollection>(TCollection blocks) where TCollection : IReadOnlyList<BasicBlock> => throw new NotImplementedException();
			public bool MoveNext<TCollection>(TCollection blocks, ref TraversalEnumerationState state) where TCollection : IReadOnlyList<BasicBlock> => throw new NotImplementedException();
			public void Traverse<TVisitor, TSuccessorProvider, TDirection>(BasicBlock entryBlock, ref TVisitor visitor, in TSuccessorProvider successorProvider)
				where TVisitor : struct, ITraversalVisitor
				where TSuccessorProvider : struct, ITraversalSuccessorsProvider<TDirection>
				where TDirection : struct, IControlFlowDirection => throw new NotImplementedException();
		}

		public struct MappedStride
			: IStride<MappedGenericIndex>
		{
			public MappedGenericIndex StrideExtent => throw new NotImplementedException();

			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public struct MappedStride1D
			: IStride1D
		{
			public int XStride => throw new NotImplementedException();

			public Index1D StrideExtent => throw new NotImplementedException();

			public Stride1D.General AsGeneral() => throw new NotImplementedException();
			public int ComputeBufferLength(Index1D extent) => throw new NotImplementedException();
			public long ComputeBufferLength(LongIndex1D extent) => throw new NotImplementedException();
			public int ComputeElementIndex(Index1D index) => throw new NotImplementedException();
			public long ComputeElementIndex(LongIndex1D index) => throw new NotImplementedException();
			public int ComputeElementIndexChecked(Index1D index, Index1D extent) => throw new NotImplementedException();
			public long ComputeElementIndexChecked(LongIndex1D index, LongIndex1D extent) => throw new NotImplementedException();
			public Index1D ReconstructFromElementIndex(int elementIndex) => throw new NotImplementedException();
			public LongIndex1D ReconstructFromElementIndex(long elementIndex) => throw new NotImplementedException();
			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public struct MappedStride2D
			: IStride2D
		{
			public int XStride => throw new NotImplementedException();

			public int YStride => throw new NotImplementedException();

			public Index2D StrideExtent => throw new NotImplementedException();

			public Stride2D.General AsGeneral() => throw new NotImplementedException();
			public int ComputeBufferLength(Index2D extent) => throw new NotImplementedException();
			public long ComputeBufferLength(LongIndex2D extent) => throw new NotImplementedException();
			public int ComputeElementIndex(Index2D index) => throw new NotImplementedException();
			public long ComputeElementIndex(LongIndex2D index) => throw new NotImplementedException();
			public int ComputeElementIndexChecked(Index2D index, Index2D extent) => throw new NotImplementedException();
			public long ComputeElementIndexChecked(LongIndex2D index, LongIndex2D extent) => throw new NotImplementedException();
			public Index2D ReconstructFromElementIndex(int elementIndex) => throw new NotImplementedException();
			public LongIndex2D ReconstructFromElementIndex(long elementIndex) => throw new NotImplementedException();
			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public struct MappedStride3D
			 : IStride3D
		{
			public int XStride => throw new NotImplementedException();

			public int YStride => throw new NotImplementedException();

			public int ZStride => throw new NotImplementedException();

			public Index3D StrideExtent => throw new NotImplementedException();

			public Stride3D.General AsGeneral() => throw new NotImplementedException();
			public int ComputeBufferLength(Index3D extent) => throw new NotImplementedException();
			public long ComputeBufferLength(LongIndex3D extent) => throw new NotImplementedException();
			public int ComputeElementIndex(Index3D index) => throw new NotImplementedException();
			public long ComputeElementIndex(LongIndex3D index) => throw new NotImplementedException();
			public int ComputeElementIndexChecked(Index3D index, Index3D extent) => throw new NotImplementedException();
			public long ComputeElementIndexChecked(LongIndex3D index, LongIndex3D extent) => throw new NotImplementedException();
			public Index3D ReconstructFromElementIndex(int elementIndex) => throw new NotImplementedException();
			public LongIndex3D ReconstructFromElementIndex(long elementIndex) => throw new NotImplementedException();
			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public struct MappedIntStride
			  : IStride<MappedIntIndex>
		{
			public MappedIntIndex StrideExtent => throw new NotImplementedException();

			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public struct MappedStrideCastableIndex
			 : IStride<MappedIntIndex, MappedLongIndex>, ICastableStride<MappedIntIndex, MappedLongIndex, MappedStrideCastableIndex>
		{
			public MappedIntIndex StrideExtent => throw new NotImplementedException();
			public (MappedLongIndex Extent, MappedStrideCastableIndex Stride) Cast<TContext>(in TContext context, in MappedLongIndex extent) where TContext : struct, IStrideCastContext => throw new NotImplementedException();
			public int ComputeBufferLength(MappedIntIndex extent) => throw new NotImplementedException();
			public long ComputeBufferLength(MappedLongIndex extent) => throw new NotImplementedException();
			public int ComputeElementIndex(MappedIntIndex index) => throw new NotImplementedException();
			public long ComputeElementIndex(MappedLongIndex index) => throw new NotImplementedException();
			public int ComputeElementIndexChecked(MappedIntIndex index, MappedIntIndex extent) => throw new NotImplementedException();
			public long ComputeElementIndexChecked(MappedLongIndex index, MappedLongIndex extent) => throw new NotImplementedException();
			public MappedIntIndex ReconstructFromElementIndex(int elementIndex) => throw new NotImplementedException();
			public MappedLongIndex ReconstructFromElementIndex(long elementIndex) => throw new NotImplementedException();
			public Stride1D.General To1DStride() => throw new NotImplementedException();
		}

		public sealed class MappedValueBuilder
			: IValueBuilder
		{
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
			public ValueReference this[FieldAccess access] => throw new NotImplementedException();
#pragma warning restore CA1043 // Use Integral Or String Argument For Indexers

			public IRBuilder IRBuilder => throw new NotImplementedException();

			public Location Location => throw new NotImplementedException();

			public int Count => throw new NotImplementedException();

			public void Add(Value value) => throw new NotImplementedException();
			public ValueReference Seal() => throw new NotImplementedException();
		}
	}
}