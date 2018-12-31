namespace Rocks.Construction
{
	internal class MethodInformation
	{
		internal MethodInformation(bool containsDelegateConditions, string delegateCast,
			string description, string descriptionWithOverride, bool isSpanLike) => 
			(this.ContainsDelegateConditions, this.DelegateCast, this.Description, this.DescriptionWithOverride, this.IsSpanLike) =
				(containsDelegateConditions, delegateCast, description, descriptionWithOverride, isSpanLike);

		internal bool ContainsDelegateConditions { get; }
		internal string DelegateCast { get; }
		internal string Description { get; }
		internal string DescriptionWithOverride { get; }
		internal bool IsSpanLike { get; }
	}
}
