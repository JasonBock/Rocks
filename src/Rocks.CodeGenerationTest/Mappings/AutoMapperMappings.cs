using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Features;
using System.Linq.Expressions;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class AutoMapperMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IMappingExpressionBase<,,>), new()
					{
						{ "TSource", "object" },
						{ "TDestination", "object" },
						{ "TMappingExpression", "global::Rocks.CodeGenerationTest.Mappings.AutoMapper.MappedMappingExpressionBase" },
					}
				},
				{
					typeof(IProjectionExpression<,,>), new()
					{
						{ "TSource", "object" },
						{ "TDestination", "object" },
						{ "TMappingExpression", "global::Rocks.CodeGenerationTest.Mappings.AutoMapper.MappedProjectionExpressionBase" },
					}
				},
				{
					typeof(IProjectionExpressionBase<,,>), new()
					{
						{ "TSource", "object" },
						{ "TDestination", "object" },
						{ "TMappingExpression", "global::Rocks.CodeGenerationTest.Mappings.AutoMapper.MappedProjectionExpressionBase" },
					}
				},
				{
					typeof(MappingExpressionBase<,,>), new()
					{
						{ "TSource", "object" },
						{ "TDestination", "object" },
						{ "TMappingExpression", "global::Rocks.CodeGenerationTest.Mappings.AutoMapper.MappedMappingExpressionBase" },
					}
				},
			};
	}

	namespace AutoMapper
	{
		public sealed class MappedMappingExpressionBase
			: IMappingExpressionBase<object, object, MappedMappingExpressionBase>
		{
			public Features<IMappingFeature> Features => throw new NotImplementedException();

			public List<ValueTransformerConfiguration> ValueTransformers => throw new NotImplementedException();

			public MappedMappingExpressionBase AfterMap(Action<object, object> afterFunction) => throw new NotImplementedException();
			public MappedMappingExpressionBase AfterMap(Action<object, object, ResolutionContext> afterFunction) => throw new NotImplementedException();
			public MappedMappingExpressionBase AfterMap<TMappingAction>() where TMappingAction : IMappingAction<object, object> => throw new NotImplementedException();
			public void As(Type typeOverride) => throw new NotImplementedException();
			public MappedMappingExpressionBase AsProxy() => throw new NotImplementedException();
			public MappedMappingExpressionBase BeforeMap(Action<object, object> beforeFunction) => throw new NotImplementedException();
			public MappedMappingExpressionBase BeforeMap(Action<object, object, ResolutionContext> beforeFunction) => throw new NotImplementedException();
			public MappedMappingExpressionBase BeforeMap<TMappingAction>() where TMappingAction : IMappingAction<object, object> => throw new NotImplementedException();
			public MappedMappingExpressionBase ConstructUsing(Func<object, ResolutionContext, object> ctor) => throw new NotImplementedException();
			public MappedMappingExpressionBase ConstructUsing(Expression<Func<object, object>> ctor) => throw new NotImplementedException();
			public MappedMappingExpressionBase ConstructUsingServiceLocator() => throw new NotImplementedException();
			public void ConvertUsing(Type typeConverterType) => throw new NotImplementedException();
			public void ConvertUsing(Func<object, object, object> mappingFunction) => throw new NotImplementedException();
			public void ConvertUsing(Func<object, object, ResolutionContext, object> mappingFunction) => throw new NotImplementedException();
			public void ConvertUsing(ITypeConverter<object, object> converter) => throw new NotImplementedException();
			public void ConvertUsing<TTypeConverter>() where TTypeConverter : ITypeConverter<object, object> => throw new NotImplementedException();
			public void ConvertUsing(Expression<Func<object, object>> mappingExpression) => throw new NotImplementedException();
			public MappedMappingExpressionBase DisableCtorValidation() => throw new NotImplementedException();
			public MappedMappingExpressionBase ForCtorParam(string ctorParamName, Action<ICtorParamConfigurationExpression<object>> paramOptions) => throw new NotImplementedException();
			public MappedMappingExpressionBase ForSourceMember(string sourceMemberName, Action<ISourceMemberConfigurationExpression> memberOptions) => throw new NotImplementedException();
			public MappedMappingExpressionBase IgnoreAllPropertiesWithAnInaccessibleSetter() => throw new NotImplementedException();
			public MappedMappingExpressionBase IgnoreAllSourcePropertiesWithAnInaccessibleSetter() => throw new NotImplementedException();
			public MappedMappingExpressionBase Include(Type derivedSourceType, Type derivedDestinationType) => throw new NotImplementedException();
			public MappedMappingExpressionBase IncludeAllDerived() => throw new NotImplementedException();
			public MappedMappingExpressionBase IncludeBase(Type sourceBase, Type destinationBase) => throw new NotImplementedException();
			public MappedMappingExpressionBase MaxDepth(int depth) => throw new NotImplementedException();
			public MappedMappingExpressionBase PreserveReferences() => throw new NotImplementedException();
			public MappedMappingExpressionBase ValidateMemberList(MemberList memberList) => throw new NotImplementedException();
		}

		public sealed class MappedProjectionExpressionBase
			: IProjectionExpressionBase<object, object, MappedProjectionExpressionBase>
		{
			public Features<IMappingFeature> Features => throw new NotImplementedException();

			public List<ValueTransformerConfiguration> ValueTransformers => throw new NotImplementedException();

			public MappedProjectionExpressionBase ConstructUsing(Expression<Func<object, object>> ctor) => throw new NotImplementedException();
			public void ConvertUsing(Expression<Func<object, object>> mappingExpression) => throw new NotImplementedException();
			public MappedProjectionExpressionBase ForCtorParam(string ctorParamName, Action<ICtorParamConfigurationExpression<object>> paramOptions) => throw new NotImplementedException();
			public MappedProjectionExpressionBase MaxDepth(int depth) => throw new NotImplementedException();
			public MappedProjectionExpressionBase ValidateMemberList(MemberList memberList) => throw new NotImplementedException();
		}
	}
}