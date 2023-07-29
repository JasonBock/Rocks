using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Data;
using FluentAssertions.Equivalency;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using FluentAssertions.Streams;
using FluentAssertions.Types;
using FluentAssertions.Xml;
using System.Reflection;
using System.Xml;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class FluentAssertionsMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AsyncFunctionAssertions<,>), new()
					{
						{ "TTask", "global::System.Threading.Tasks.Task" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedAsyncFunctionAssertions" },
					}
				},
				{
					typeof(BooleanAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedBooleanAssertions" },
					}
				},
				{
					typeof(BufferedStreamAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedBufferedStreamAssertions" },
					}
				},
				{
					typeof(ComparableTypeAssertions<,>), new()
					{
						{ "T", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedComparableTypeAssertions" },
					}
				},
				{
					typeof(DelegateAssertions<,>), new()
					{
						{ "TDelegate", "global::System.Delegate" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDelegateAssertions" },
					}
				},
				{
					typeof(DataRowAssertions<>), new()
					{
						{ "TDataRow", "global::System.Data.DataRow" },
					}
				},
				{
					typeof(DataSetAssertions<>), new()
					{
						{ "TDataSet", "global::System.Data.DataSet" },
					}
				},
				{
					typeof(DataTableAssertions<>), new()
					{
						{ "TDataTable", "global::System.Data.DataTable" },
					}
				},
				{
					typeof(DateOnlyAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDateOnlyAssertions" },
					}
				},
				{
					typeof(DateTimeAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDateTimeAssertions" },
					}
				},
				{
					typeof(DateTimeOffsetAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDateTimeOffsetAssertions" },
					}
				},
				{
					typeof(DateTimeOffsetRangeAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDateTimeOffsetAssertions" },
					}
				},
				{
					typeof(DateTimeRangeAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedDateTimeAssertions" },
					}
				},
				{
					typeof(EnumAssertions<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedEnum" },
					}
				},
				{
					typeof(EnumAssertions<,>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedEnum" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedEnumAssertions" },
					}
				},
				{
					typeof(ExceptionAssertions<>), new()
					{
						{ "TException", "global::System.Exception" },
					}
				},
				{
					typeof(GenericCollectionAssertions<,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<object>" },
						{ "T", "object" },
					}
				},
				{
					typeof(GenericCollectionAssertions<,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<object>" },
						{ "T", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedGenericCollectionAssertions" },
					}
				},
				{
					typeof(GenericDictionaryAssertions<,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.Dictionary<object, object>" },
						{ "TKey", "object" },
						{ "TValue", "object" },
					}
				},
				{
					typeof(GenericDictionaryAssertions<,,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.Dictionary<object, object>" },
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedGenericDictionaryAssertions" },
					}
				},
				{
					typeof(GuidAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedGuidAssertions" },
					}
				},
				{
					typeof(HttpResponseMessageAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedHttpResponseMessageAssertions" },
					}
				},
				{
					typeof(ICollectionWrapper<>), new()
					{
						{ "TCollection", "global::System.Collections.ICollection" },
					}
				},
				{
					typeof(MemberInfoAssertions<,>), new()
					{
						{ "TSubject", "global::System.Reflection.MemberInfo" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedMemberInfoAssertions" },
					}
				},
				{
					typeof(MethodBaseAssertions<,>), new()
					{
						{ "TSubject", "global::System.Reflection.MethodBase" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedMethodBaseAssertions" },
					}
				},
				{
					typeof(NullableBooleanAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableBooleanAssertions" },
					}
				},
				{
					typeof(NullableDateOnlyAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableDateOnlyAssertions" },
					}
				},
				{
					typeof(NullableDateTimeAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableDateTimeAssertions" },
					}
				},
				{
					typeof(NullableDateTimeOffsetAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableDateTimeOffsetAssertions" },
					}
				},
				{
					typeof(NullableEnumAssertions<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedEnum" },
					}
				},
				{
					typeof(NullableEnumAssertions<,>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedEnum" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableEnumAssertions" },
					}
				},
				{
					typeof(NullableGuidAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableGuidAssertions" },
					}
				},
				{
					typeof(NullableNumericAssertions<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedComparableAssertion" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableNumericAssertions" },
					}
				},
				{
					typeof(NullableSimpleTimeSpanAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableSimpleTimeSpanAssertions" },
					}
				},
				{
					typeof(NullableTimeOnlyAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNullableTimeOnlyAssertions" },
					}
				},
				{
					typeof(NumericAssertions<,>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedComparableAssertion" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedNumericAssertions" },
					}
				},
				{
					typeof(ObjectAssertions<,>), new()
					{
						{ "TSubject", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedObjectAssertions" },
					}
				},
				{
					typeof(ReferenceTypeAssertions<,>), new()
					{
						{ "TSubject", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedReferenceTypeAssertions" },
					}
				},
				{
					typeof(SelfReferenceEquivalencyAssertionOptions<>), new()
					{
						{ "TSelf", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedSelfReferenceEquivalencyAssertionOptions" },
					}
				},
				{
					typeof(SimpleTimeSpanAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedSimpleTimeSpanAssertions" },
					}
				},
				{
					typeof(StreamAssertions<,>), new()
					{
						{ "TSubject", "global::System.IO.Stream" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedStreamAssertions" },
					}
				},
				{
					typeof(StringAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedStringAssertions" },
					}
				},
				{
					typeof(StringCollectionAssertions<>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<global::System.String>" },
					}
				},
				{
					typeof(StringCollectionAssertions<,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<global::System.String>" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedStringCollectionAssertions" },
					}
				},
				{
					typeof(SubsequentOrderingGenericCollectionAssertions<,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<object>" },
						{ "T", "object" },
					}
				},
				{
					typeof(SubsequentOrderingGenericCollectionAssertions<,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.List<object>" },
						{ "T", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedSubsequentOrderingGenericCollectionAssertions" },
					}
				},
				{
					typeof(TimeOnlyAssertions<>), new()
					{
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedTimeOnlyAssertions" },
					}
				},
				{
					typeof(WhoseValueConstraint<,,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.Dictionary<object, object>" },
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedGenericDictionaryAssertions" },
					}
				},
				{
					typeof(XmlNodeAssertions<,>), new()
					{
						{ "TSubject", "global::System.Xml.XmlNode" },
						{ "TAssertions", "global::Rocks.CodeGenerationTest.Mappings.FluentAssertions.MappedXmlNodeAssertions" },
					}
				},
			};
	}

	namespace FluentAssertions
	{
		public sealed class MappedSimpleTimeSpanAssertions
			: SimpleTimeSpanAssertions<MappedSimpleTimeSpanAssertions>
		{
			public MappedSimpleTimeSpanAssertions(TimeSpan? value) : base(value)
			{
			}
		}

		public sealed class MappedNullableGuidAssertions
			: NullableGuidAssertions<MappedNullableGuidAssertions>
		{
			public MappedNullableGuidAssertions(Guid? value) : base(value)
			{
			}
		}

		public sealed class MappedSubsequentOrderingGenericCollectionAssertions
			: SubsequentOrderingGenericCollectionAssertions<List<object>, object, MappedSubsequentOrderingGenericCollectionAssertions>
		{
			public MappedSubsequentOrderingGenericCollectionAssertions(List<object> actualValue, IOrderedEnumerable<object> previousOrderedEnumerable) : base(actualValue, previousOrderedEnumerable)
			{
			}
		}

		public sealed class MappedGenericDictionaryAssertions
			: GenericDictionaryAssertions<Dictionary<object, object>, object, object, MappedGenericDictionaryAssertions>
		{
			public MappedGenericDictionaryAssertions(Dictionary<object, object> keyValuePairs) : base(keyValuePairs)
			{
			}
		}

		public sealed class MappedBooleanAssertions
			: BooleanAssertions<MappedBooleanAssertions>
		{
			public MappedBooleanAssertions(bool? value) : base(value)
			{
			}
		}

		public sealed class MappedDateTimeOffsetAssertions
			: DateTimeOffsetAssertions<MappedDateTimeOffsetAssertions>
		{
			public MappedDateTimeOffsetAssertions(DateTimeOffset? value) : base(value)
			{
			}
		}

		public sealed class MappedDateTimeOffsetRangeAssertions
			: DateTimeOffsetRangeAssertions<MappedDateTimeOffsetAssertions>
		{
			internal MappedDateTimeOffsetRangeAssertions(MappedDateTimeOffsetAssertions parentAssertions, DateTimeOffset? subject, TimeSpanCondition condition, TimeSpan timeSpan) : base(parentAssertions, subject, condition, timeSpan)
			{
			}
		}

		public sealed class MappedNullableDateOnlyAssertions
			: NullableDateOnlyAssertions<MappedNullableDateOnlyAssertions>
		{
			public MappedNullableDateOnlyAssertions(DateOnly? value) : base(value)
			{
			}
		}

		public sealed class MappedObjectAssertions
			: ObjectAssertions<object, MappedObjectAssertions>
		{
			public MappedObjectAssertions(object value) : base(value)
			{
			}
		}

		public sealed class MappedDateTimeAssertions
			: DateTimeAssertions<MappedDateTimeAssertions>
		{
			public MappedDateTimeAssertions(DateTime? value) : base(value)
			{
			}
		}

		public sealed class MappedSelfReferenceEquivalencyAssertionOptions
			: SelfReferenceEquivalencyAssertionOptions<MappedSelfReferenceEquivalencyAssertionOptions>
		{
			public MappedSelfReferenceEquivalencyAssertionOptions(IEquivalencyAssertionOptions defaults) : base(defaults)
			{
			}
		}

		public sealed class MappedAsyncFunctionAssertions
			: AsyncFunctionAssertions<Task, MappedAsyncFunctionAssertions>
		{
#pragma warning disable CS0618 // Type or member is obsolete
			public MappedAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor, IClock clock) : base(subject, extractor, clock)
#pragma warning restore CS0618 // Type or member is obsolete
			{
			}
		}

		public sealed class MappedGuidAssertions
			: GuidAssertions<MappedGuidAssertions>
		{
			public MappedGuidAssertions(Guid? value) : base(value)
			{
			}
		}

		public sealed class MappedDateTimeRangeAssertions
			: DateTimeRangeAssertions<MappedDateTimeAssertions>
		{
			internal MappedDateTimeRangeAssertions(MappedDateTimeAssertions parentAssertions, DateTime? subject, TimeSpanCondition condition, TimeSpan timeSpan) : base(parentAssertions, subject, condition, timeSpan)
			{
			}
		}

		public sealed class MappedNullableBooleanAssertions
			: NullableBooleanAssertions<MappedNullableBooleanAssertions>
		{
			public MappedNullableBooleanAssertions(bool? value) : base(value)
			{
			}
		}

		public sealed class MappedReferenceTypeAssertions
			: ReferenceTypeAssertions<object, MappedReferenceTypeAssertions>
		{
			public MappedReferenceTypeAssertions(object subject) : base(subject)
			{
			}

			protected override string Identifier => throw new NotImplementedException();
		}

		public sealed class MappedDateOnlyAssertions
			: DateOnlyAssertions<MappedDateOnlyAssertions>
		{
			public MappedDateOnlyAssertions(DateOnly? value) : base(value)
			{
			}
		}

		public sealed class MappedNullableDateTimeOffsetAssertions
			: NullableDateTimeOffsetAssertions<MappedNullableDateTimeOffsetAssertions>
		{
			public MappedNullableDateTimeOffsetAssertions(DateTimeOffset? expected) : base(expected)
			{
			}
		}

		public sealed class MappedStringAssertions
			: StringAssertions<MappedStringAssertions>
		{
			public MappedStringAssertions(string value) : base(value)
			{
			}
		}

		public sealed class MappedXmlNodeAssertions
			: XmlNodeAssertions<XmlNode, MappedXmlNodeAssertions>
		{
			public MappedXmlNodeAssertions(XmlNode xmlNode) : base(xmlNode)
			{
			}
		}

		public sealed class MappedTimeOnlyAssertions
			: TimeOnlyAssertions<MappedTimeOnlyAssertions>
		{
			public MappedTimeOnlyAssertions(TimeOnly? value) : base(value)
			{
			}
		}

		public sealed class MappedNullableTimeOnlyAssertions
			: NullableTimeOnlyAssertions<MappedNullableTimeOnlyAssertions>
		{
			public MappedNullableTimeOnlyAssertions(TimeOnly? value) : base(value)
			{
			}
		}

		public sealed class MappedBufferedStreamAssertions
			: BufferedStreamAssertions<MappedBufferedStreamAssertions>
		{
			public MappedBufferedStreamAssertions(BufferedStream stream) : base(stream)
			{
			}
		}

		public sealed class MappedEnumAssertions
			: EnumAssertions<MappedEnum, MappedEnumAssertions>
		{
			public MappedEnumAssertions(MappedEnum subject) : base(subject)
			{
			}
		}

		public sealed class MappedNullableSimpleTimeSpanAssertions
			: NullableSimpleTimeSpanAssertions<MappedNullableSimpleTimeSpanAssertions>
		{
			public MappedNullableSimpleTimeSpanAssertions(TimeSpan? value) : base(value)
			{
			}
		}

		public enum MappedEnum { }

		public sealed class MappedNullableEnumAssertions
			: NullableEnumAssertions<MappedEnum, MappedNullableEnumAssertions>
		{
			public MappedNullableEnumAssertions(MappedEnum? subject) : base(subject)
			{
			}
		}

		public sealed class MappedNullableDateTimeAssertions
			: NullableDateTimeAssertions<MappedNullableDateTimeAssertions>
		{
			public MappedNullableDateTimeAssertions(DateTime? expected) : base(expected)
			{
			}
		}

		public sealed class MappedHttpResponseMessageAssertions
			: HttpResponseMessageAssertions<MappedHttpResponseMessageAssertions>
		{
			public MappedHttpResponseMessageAssertions(HttpResponseMessage value) : base(value)
			{
			}
		}

		public sealed class MappedStringCollectionAssertions
			: StringCollectionAssertions<List<string>, MappedStringCollectionAssertions>
		{
			public MappedStringCollectionAssertions(List<string> actualValue) : base(actualValue)
			{
			}
		}

		public sealed class MappedStreamAssertions
			: StreamAssertions<Stream, MappedStreamAssertions>
		{
			public MappedStreamAssertions(Stream stream) : base(stream)
			{
			}
		}

		public sealed class MappedGenericCollectionAssertions
			: GenericCollectionAssertions<List<object>, object, MappedGenericCollectionAssertions>
		{
			public MappedGenericCollectionAssertions(List<object> actualValue) : base(actualValue)
			{
			}
		}

		public sealed class MappedDelegateAssertions
			: DelegateAssertions<Delegate, MappedDelegateAssertions>
		{
			public MappedDelegateAssertions(Delegate @delegate, IExtractExceptions extractor) : base(@delegate, extractor)
			{
			}

			protected override string Identifier => throw new NotImplementedException();

			protected override void InvokeSubject() => throw new NotImplementedException();
		}

		public sealed class MappedComparableTypeAssertions
			: ComparableTypeAssertions<object, MappedComparableTypeAssertions>
		{
			public MappedComparableTypeAssertions(IComparable<object> value) : base(value)
			{
			}
		}

		public sealed class MappedNumericAssertions
			: NumericAssertions<MappedComparableAssertion, MappedNumericAssertions>
		{
			public MappedNumericAssertions(MappedComparableAssertion value) : base(value)
			{
			}
		}

		public sealed class MappedNullableNumericAssertions
			: NullableNumericAssertions<MappedComparableAssertion, MappedNullableNumericAssertions>
		{
			public MappedNullableNumericAssertions(MappedComparableAssertion? value) : base(value)
			{
			}
		}

		public struct MappedComparableAssertion
			: IComparable<MappedComparableAssertion>
		{
			public int CompareTo(MappedComparableAssertion other) => throw new NotImplementedException();
		}

		public sealed class MappedMethodBaseAssertions
			: MethodBaseAssertions<MethodBase, MappedMethodBaseAssertions>
		{
			public MappedMethodBaseAssertions(MethodBase subject)
				: base(subject) { }
		}

		public sealed class MappedMemberInfoAssertions
			: MemberInfoAssertions<MemberInfo, MappedMemberInfoAssertions>
		{
			public MappedMemberInfoAssertions(MemberInfo subject) : base(subject)
			{
			}
		}
	}
}