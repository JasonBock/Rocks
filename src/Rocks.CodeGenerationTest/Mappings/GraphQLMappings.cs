using GraphQL;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using GraphQL.Validation;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class GraphQLMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(Connection<,>), new()
					{
						{ "TNode", "object" },
						{ "TEdge", "global::GraphQL.Types.Relay.DataObjects.Edge<object>" },
					}
				},
				{
					typeof(ConnectionType<>), new()
					{
						{ "TNodeType", "global::GraphQL.Types.IGraphType" },
					}
				},
				{
					typeof(ConnectionType<,>), new()
					{
						{ "TNodeType", "global::GraphQL.Types.IGraphType" },
						{ "TEdgeType", "global::GraphQL.Types.Relay.EdgeType<global::GraphQL.Types.IGraphType>" },
					}
				},
				{
					typeof(EdgeType<>), new()
					{
						{ "TNodeType", "global::GraphQL.Types.IGraphType" },
					}
				},
				{
					typeof(EnumerationGraphType<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.GraphQL.MappedEnum" },
					}
				},
				{
					typeof(EnumValues<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.GraphQL.MappedEnum" },
					}
				},
				{
					typeof(IDocumentExecuter<>), new()
					{
						{ "TSchema", "global::GraphQL.Types.ISchema" },
					}
				},
				{
					typeof(InputTypeAttribute<>), new()
					{
						{ "TGraphType", "global::GraphQL.Types.IGraphType" },
					}
				},
				{
					typeof(MatchingNodeVisitor<>), new()
					{
						{ "TNode", "global::GraphQL.Parser.AST.ASTNode" },
					}
				},
				{
					typeof(MatchingNodeVisitor<,>), new()
					{
						{ "TNode", "global::GraphQL.Parser.AST.ASTNode" },
						{ "TState", "object" },
					}
				},
				{
					typeof(QueryArgument<>), new()
					{
						{ "TType", "global::GraphQL.Types.IGraphType" },
					}
				},
				{
					typeof(OutputTypeAttribute<>), new()
					{
						{ "TGraphType", "global::GraphQL.Types.IGraphType" },
					}
				},
			};
	}

	namespace GraphQL
	{
		public enum MappedEnum { }
	}
}