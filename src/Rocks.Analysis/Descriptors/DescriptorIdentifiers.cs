namespace Rocks.Analysis.Descriptors;

internal static class DescriptorIdentifiers
{
	internal const string CannotMockSealedTypeId = "ROCK1";
	internal const string CannotMockObsoleteTypeId = "ROCK2";
	internal const string TypeHasNoMockableMembersId = "ROCK3";
	internal const string TypeHasNoAccessibleConstructorsId = "ROCK4";
	internal const string CannotMockSpecialTypesId = "ROCK6";
	internal const string InterfaceHasStaticAbstractMembersId = "ROCK7";
	internal const string TypeHasInaccessibleAbstractMembersId = "ROCK8";
	internal const string MemberUsesObsoleteTypeId = "ROCK9";
	internal const string TypeIsClosedGenericId = "ROCK14";
	internal const string ValueTaskInReturnValueId = "ROCK15";
	internal const string IDisposableInstancesIntoTasksId = "ROCK16";
	internal const string DisposableInstancesFromExpectationsId = "ROCK17";
	internal const string InterfaceAllowsRefStructAsRefOrRefReadonlyReturnId = "ROCK19";
}