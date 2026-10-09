namespace Stuff;

public abstract class InternalTargets { public abstract void VisibleWork(); protected abstract void Work(string value); }

public abstract class IntermediateInternalTargets
	: InternalTargets
{
	protected override sealed void Work(string value) => throw new NotImplementedException();
}