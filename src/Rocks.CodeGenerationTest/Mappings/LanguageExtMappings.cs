using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using LanguageExt.TypeClasses;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class LanguageExtMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(Client<,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "REQ", "object" },
						{ "RES", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Consumer<,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "IN", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(ConsumerLift<,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "IN", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Effect<,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "A", "object" },
					}
				},
				{
					typeof(FloatType<,,>), new()
					{
						{ "SELF", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedFloatType" },
						{ "FLOATING", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedFloating" },
						{ "A", "object" },
					}
				},
				{
					typeof(FloatType<,,,>), new()
					{
						{ "SELF", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedFloatTypePred" },
						{ "FLOATING", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedFloating" },
						{ "A", "object" },
						{ "PRED", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPred" },
					}
				},
				{
					typeof(HasCancel<>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
					}
				},
				{
					typeof(Lift<,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "A", "object" },
					}
				},
				{
					typeof(Lst<,>), new()
					{
						{ "PRED", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPredList" },
						{ "A", "object" },
					}
				},
				{
					typeof(Lst<,,>), new()
					{
						{ "PredList", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPredList" },
						{ "PredItem", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPredItem" },
						{ "A", "object" },
					}
				},
				{
					typeof(M<,,,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "UOut", "object" },
						{ "UIn", "object" },
						{ "DIn", "object" },
						{ "DOut", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadRWS<,,,,>), new()
					{
						{ "MonoidW", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonoid" },
						{ "R", "object" },
						{ "W", "object" },
						{ "S", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadTrans<,,,,>), new()
					{
						{ "OuterMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonad" },
						{ "OuterType", "object" },
						{ "InnerMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonad" },
						{ "InnerType", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadTransAsync<,,,,>), new()
					{
						{ "OuterMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonadAsync" },
						{ "OuterType", "object" },
						{ "InnerMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonadAsync" },
						{ "InnerType", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadTransAsyncSync<,,,,>), new()
					{
						{ "OuterMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonadAsync" },
						{ "OuterType", "object" },
						{ "InnerMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonad" },
						{ "InnerType", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadTransSyncAsync<,,,,>), new()
					{
						{ "OuterMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonad" },
						{ "OuterType", "object" },
						{ "InnerMonad", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonadAsync" },
						{ "InnerType", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(MonadWriter<,,>), new()
					{
						{ "MonoidW", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonoid" },
						{ "W", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(NewType<,>), new()
					{
						{ "NEWTYPE", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNewType" },
						{ "A", "object" },
					}
				},
				{
					typeof(NewType<,,>), new()
					{
						{ "NEWTYPE", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNewTypePred" },
						{ "A", "object" },
						{ "PRED", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPred" },
					}
				},
				{
					typeof(NewType<,,,>), new()
					{
						{ "NEWTYPE", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNewTypePredOrd" },
						{ "A", "object" },
						{ "PRED", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPred" },
						{ "ORD", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedOrd" },
					}
				},
				{
					typeof(NumType<,,>), new()
					{
						{ "SELF", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNumType" },
						{ "NUM", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNum" },
						{ "A", "object" },
					}
				},
				{
					typeof(NumType<,,,>), new()
					{
						{ "SELF", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNumTypePred" },
						{ "NUM", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNum" },
						{ "A", "object" },
						{ "PRED", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedPred" },
					}
				},
				{
					typeof(Pipe<,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "IN", "object" },
						{ "OUT", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Producer<,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "OUT", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(ProducerLift<,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "OUT", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Proxy<,,,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "UOut", "object" },
						{ "UIn", "object" },
						{ "DIn", "object" },
						{ "DOut", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Pure<,,,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "UOut", "object" },
						{ "UIn", "object" },
						{ "DIn", "object" },
						{ "DOut", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Record<>), new()
					{
						{ "RECORDTYPE", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedRecord" },
					}
				},
				{
					typeof(Range<,,>), new()
					{
						{ "SELF", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedRange" },
						{ "MonoidOrdA", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonoidOrdA" },
						{ "A", "object" },
					}
				},
				{
					typeof(Request<,,,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "UOut", "object" },
						{ "UIn", "object" },
						{ "DIn", "object" },
						{ "DOut", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Respond<,,,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "UOut", "object" },
						{ "UIn", "object" },
						{ "DIn", "object" },
						{ "DOut", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(Server<,,,>), new()
					{
						{ "RT", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedHasCancel" },
						{ "REQ", "object" },
						{ "RES", "object" },
						{ "A", "object" },
					}
				},
				{
					typeof(ValidationData<,,>), new()
					{
						{ "MonoidFail", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedMonoidFail" },
						{ "FAIL", "object" },
						{ "SUCCESS", "object" },
					}
				},
				{
					typeof(VectorClock<>), new()
					{
						{ "A", "int" },
					}
				},
				{
					typeof(VectorClock<,,,>), new()
					{
						{ "OrdA", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedOrd" },
						{ "NumB", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNum" },
						{ "A", "object" },
						{ "B", "object" },
					}
				},
				{
					typeof(VersionVector<,,>), new()
					{
						{ "ConflictA", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedConflict" },
						{ "Actor", "int" },
						{ "A", "object" },
					}
				},
				{
					typeof(VersionVector<,,,,,>), new()
					{
						{ "ConflictA", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedConflict" },
						{ "OrdActor", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedOrd" },
						{ "NumClock", "global::Rocks.CodeGenerationTest.Mappings.LanguageExt.MappedNum" },
						{ "Actor", "object" },
						{ "Clock", "object" },
						{ "A", "object" },
					}
				},
			};
	}

	namespace LanguageExt
	{
		public struct MappedConflict
			: Conflict<object>
		{
			public (long TimeStamp, Option<object> Value) Resolve((long TimeStamp, Option<object> Value) Current, (long TimeStamp, Option<object> Value) Proposed) => throw new NotImplementedException();
		}

		public struct MappedHasCancel
			 : HasCancel<MappedHasCancel>
		{
			public MappedHasCancel LocalCancel => throw new NotImplementedException();

			public CancellationToken CancellationToken => throw new NotImplementedException();

			public CancellationTokenSource CancellationTokenSource => throw new NotImplementedException();
		}

#pragma warning disable CA1715 // Identifiers should have correct prefix
		public struct MappedFloating
			: Floating<object>
		{
			public object Abs(object x) => throw new NotImplementedException();
			public object Acos(object x) => throw new NotImplementedException();
			public object Acosh(object x) => throw new NotImplementedException();
			public object Append(object x, object y) => throw new NotImplementedException();
			public object Asin(object x) => throw new NotImplementedException();
			public object Asinh(object x) => throw new NotImplementedException();
			public object Atan(object x) => throw new NotImplementedException();
			public object Atanh(object x) => throw new NotImplementedException();
			public int Compare(object x, object y) => throw new NotImplementedException();
			public Task<int> CompareAsync(object x, object y) => throw new NotImplementedException();
			public object Cos(object x) => throw new NotImplementedException();
			public object Cosh(object x) => throw new NotImplementedException();
			public object Divide(object x, object y) => throw new NotImplementedException();
			public object Empty() => throw new NotImplementedException();
			public new bool Equals(object x, object y) => throw new NotImplementedException();
			public Task<bool> EqualsAsync(object x, object y) => throw new NotImplementedException();
			public object Exp(object x) => throw new NotImplementedException();
			public object FromDecimal(decimal x) => throw new NotImplementedException();
			public object FromDouble(double x) => throw new NotImplementedException();
			public object FromFloat(float x) => throw new NotImplementedException();
			public object FromInteger(int x) => throw new NotImplementedException();
			public object FromRational(Ratio<int> x) => throw new NotImplementedException();
			public int GetHashCode(object x) => throw new NotImplementedException();
			public Task<int> GetHashCodeAsync(object x) => throw new NotImplementedException();
			public object Log(object x) => throw new NotImplementedException();
			public object LogBase(object x, object y) => throw new NotImplementedException();
			public object Negate(object x) => throw new NotImplementedException();
			public object Pi() => throw new NotImplementedException();
			public object Plus(object x, object y) => throw new NotImplementedException();
			public object Pow(object x, object y) => throw new NotImplementedException();
			public object Product(object x, object y) => throw new NotImplementedException();
			public object Signum(object x) => throw new NotImplementedException();
			public object Sin(object x) => throw new NotImplementedException();
			public object Sinh(object x) => throw new NotImplementedException();
			public object Sqrt(object x) => throw new NotImplementedException();
			public object Subtract(object x, object y) => throw new NotImplementedException();
			public object Tan(object x) => throw new NotImplementedException();
			public object Tanh(object x) => throw new NotImplementedException();
		}

		public struct MappedMonadAsync
			: MonadAsync<object, object>
		{
			public object Apply(Func<object, object, object> f, object ma, object mb) => throw new NotImplementedException();
			public MB Bind<MONADB, MB, B>(object ma, Func<object, MB> f) where MONADB : struct, MonadAsync<Unit, Unit, MB, B> => throw new NotImplementedException();
			public MB BindAsync<MONADB, MB, B>(object ma, Func<object, Task<MB>> f) where MONADB : struct, MonadAsync<Unit, Unit, MB, B> => throw new NotImplementedException();
			public object BindReturn(Unit outputma, object mb) => throw new NotImplementedException();
			public Func<Unit, Task<int>> Count(object fa) => throw new NotImplementedException();
			public object Fail(object err = null!) => throw new NotImplementedException();
			public Func<Unit, Task<S>> Fold<S>(object fa, S state, Func<S, object, S> f) => throw new NotImplementedException();
			public Func<Unit, Task<S>> FoldAsync<S>(object fa, S state, Func<S, object, Task<S>> f) => throw new NotImplementedException();
			public Func<Unit, Task<S>> FoldBack<S>(object fa, S state, Func<S, object, S> f) => throw new NotImplementedException();
			public Func<Unit, Task<S>> FoldBackAsync<S>(object fa, S state, Func<S, object, Task<S>> f) => throw new NotImplementedException();
			public object Plus(object ma, object mb) => throw new NotImplementedException();
			public object ReturnAsync(Task<object> x) => throw new NotImplementedException();
			public object ReturnAsync(Func<Unit, Task<object>> f) => throw new NotImplementedException();
			public object RunAsync(Func<Unit, Task<object>> ma) => throw new NotImplementedException();
			public object Zero() => throw new NotImplementedException();
		}

		public struct MappedMonad
			 : Monad<object, object>
		{
			public object Apply(Func<object, object, object> f, object ma, object mb) => throw new NotImplementedException();
			public MB Bind<MONADB, MB, B>(object ma, Func<object, MB> f) where MONADB : struct, Monad<Unit, Unit, MB, B> => throw new NotImplementedException();
			public MB BindAsync<MonadB, MB, B>(object ma, Func<object, MB> f) where MonadB : struct, MonadAsync<Unit, Unit, MB, B> => throw new NotImplementedException();
			public object BindReturn(Unit outputma, object mb) => throw new NotImplementedException();
			public Func<Unit, int> Count(object fa) => throw new NotImplementedException();
			public object Fail(object err = null!) => throw new NotImplementedException();
			public Func<Unit, S> Fold<S>(object fa, S state, Func<S, object, S> f) => throw new NotImplementedException();
			public Func<Unit, S> FoldBack<S>(object fa, S state, Func<S, object, S> f) => throw new NotImplementedException();
			public object Plus(object ma, object mb) => throw new NotImplementedException();
			public object Return(object x) => throw new NotImplementedException();
			public object Return(Func<Unit, object> f) => throw new NotImplementedException();
			public object Run(Func<Unit, object> ma) => throw new NotImplementedException();
			public object Zero() => throw new NotImplementedException();
		}

		public struct MappedMonoid
			 : Monoid<object>
		{
			public object Append(object x, object y) => throw new NotImplementedException();
			public object Empty() => throw new NotImplementedException();
		}

		public struct MappedMonoidFail
			: Monoid<object>, Eq<object>
		{
			public object Append(object x, object y) => throw new NotImplementedException();
			public object Empty() => throw new NotImplementedException();
			public new bool Equals(object x, object y) => throw new NotImplementedException();
			public Task<bool> EqualsAsync(object x, object y) => throw new NotImplementedException();
			public int GetHashCode(object x) => throw new NotImplementedException();
			public Task<int> GetHashCodeAsync(object x) => throw new NotImplementedException();
		}

		public sealed class MappedNewTypePredOrd
			: NewType<MappedNewTypePredOrd, object, MappedPred, MappedOrd>
		{
			public MappedNewTypePredOrd()
				: base(new object()) { }
		}

		public sealed class MappedNewType
			: NewType<MappedNewType, object>
		{
			public MappedNewType()
				: base(new object()) { }
		}

		public sealed class MappedNewTypePred
			: NewType<MappedNewTypePred, object, MappedPred>
		{
			public MappedNewTypePred()
				: base(new object()) { }
		}

		public sealed class MappedNumType
			: NumType<MappedNumType, MappedNum, object>
		{
			public MappedNumType()
				: base(new object()) { }
		}

		public sealed class MappedNumTypePred
			: NumType<MappedNumTypePred, MappedNum, object, MappedPred>
		{
			public MappedNumTypePred()
				: base(new object()) { }
		}

		public sealed class MappedFloatType
			: FloatType<MappedFloatType, MappedFloating, object>
		{
			public MappedFloatType()
				: base(new object()) { }
		}

		public sealed class MappedFloatTypePred
			: FloatType<MappedFloatTypePred, MappedFloating, object, MappedPred>
		{
			public MappedFloatTypePred()
				: base(new object()) { }
		}

		public struct MappedNum
			: Num<object>
		{
			public object Abs(object x) => throw new NotImplementedException();
			public object Append(object x, object y) => throw new NotImplementedException();
			public int Compare(object x, object y) => throw new NotImplementedException();
			public Task<int> CompareAsync(object x, object y) => throw new NotImplementedException();
			public object Divide(object x, object y) => throw new NotImplementedException();
			public object Empty() => throw new NotImplementedException();
			public new bool Equals(object x, object y) => throw new NotImplementedException();
			public Task<bool> EqualsAsync(object x, object y) => throw new NotImplementedException();
			public object FromDecimal(decimal x) => throw new NotImplementedException();
			public object FromDouble(double x) => throw new NotImplementedException();
			public object FromFloat(float x) => throw new NotImplementedException();
			public object FromInteger(int x) => throw new NotImplementedException();
			public int GetHashCode(object x) => throw new NotImplementedException();
			public Task<int> GetHashCodeAsync(object x) => throw new NotImplementedException();
			public object Negate(object x) => throw new NotImplementedException();
			public object Plus(object x, object y) => throw new NotImplementedException();
			public object Product(object x, object y) => throw new NotImplementedException();
			public object Signum(object x) => throw new NotImplementedException();
			public object Subtract(object x, object y) => throw new NotImplementedException();
		}

		public struct MappedOrd
			  : Ord<object>
		{
			public int Compare(object x, object y) => throw new NotImplementedException();
			public Task<int> CompareAsync(object x, object y) => throw new NotImplementedException();
			public new bool Equals(object x, object y) => throw new NotImplementedException();
			public Task<bool> EqualsAsync(object x, object y) => throw new NotImplementedException();
			public int GetHashCode(object x) => throw new NotImplementedException();
			public Task<int> GetHashCodeAsync(object x) => throw new NotImplementedException();
		}

		public struct MappedMonoidOrdA
			: Monoid<object>, Ord<object>, Arithmetic<object>
		{
			public object Append(object x, object y) => throw new NotImplementedException();
			public int Compare(object x, object y) => throw new NotImplementedException();
			public Task<int> CompareAsync(object x, object y) => throw new NotImplementedException();
			public object Empty() => throw new NotImplementedException();
			public new bool Equals(object x, object y) => throw new NotImplementedException();
			public Task<bool> EqualsAsync(object x, object y) => throw new NotImplementedException();
			public int GetHashCode(object x) => throw new NotImplementedException();
			public Task<int> GetHashCodeAsync(object x) => throw new NotImplementedException();
			public object Negate(object x) => throw new NotImplementedException();
			public object Plus(object x, object y) => throw new NotImplementedException();
			public object Product(object x, object y) => throw new NotImplementedException();
			public object Subtract(object x, object y) => throw new NotImplementedException();
		}

		public struct MappedPred
			 : Pred<object>
		{
			public bool True(object value) => throw new NotImplementedException();
		}

		public struct MappedPredItem
			: Pred<object>
		{
			public bool True(object value) => throw new NotImplementedException();
		}

		public struct MappedPredList
			 : Pred<ListInfo>
		{
			public bool True(ListInfo value) => throw new NotImplementedException();
		}

		public sealed class MappedRange
			: Range<MappedRange, MappedMonoidOrdA, object>
		{
			public MappedRange()
				: base(new(), new(), new()) { }
		}

		public sealed class MappedRecord
			: Record<MappedRecord>
		{ }
#pragma warning restore CA1715 // Identifiers should have correct prefix
	}
}