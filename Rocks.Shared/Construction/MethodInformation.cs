namespace Rocks.Construction
{
	internal class MethodInformation
	{
		internal MethodInformation(bool containsDelegateConditions, string delegateCast,
			string description, string descriptionWithOverride)
		{
			this.ContainsDelegateConditions = containsDelegateConditions;
			this.DelegateCast = delegateCast;
			this.Description = description;
			this.DescriptionWithOverride = descriptionWithOverride;
		}

		internal bool ContainsDelegateConditions { get; }
		internal string DelegateCast { get; }
		internal string Description { get; }
		internal string DescriptionWithOverride { get; }
	}
}
