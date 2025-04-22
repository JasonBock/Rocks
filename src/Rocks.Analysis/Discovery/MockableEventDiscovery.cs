using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableEventDiscovery
{
	internal MockableEventDiscovery(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		Compilation compilation) =>
		this.Events =
			mockType.TypeKind == TypeKind.Interface ?
				GetEventsForInterface(mockType, containingAssemblyOfInvocationSymbol, compilation) :
				GetEventsForClass(mockType, containingAssemblyOfInvocationSymbol, compilation);

	private static MockableEvents GetEventsForClass(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		Compilation compilation)
	{
		var events = new List<MockableEventResult>();
		var inaccessibleAbstractMembers = false;

		foreach (var selfEvent in mockType.GetMembers().OfType<IEventSymbol>()
			.Where(_ => !_.IsStatic && _.CanBeReferencedByName &&
				(_.IsAbstract || _.IsVirtual)))
		{
			var canBeSeen = selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation);

			if (!canBeSeen && selfEvent.IsAbstract)
			{
				inaccessibleAbstractMembers = true;
			}
			else if (canBeSeen)
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
			}
		}

		return new([.. events], inaccessibleAbstractMembers);
	}

	private static MockableEvents GetEventsForInterface(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		Compilation compilation)
	{
		static bool IsEventToExamine(IEventSymbol @event) =>
			!@event.IsStatic && 
			(@event.IsAbstract || @event.IsVirtual) &&
			@event.CanBeReferencedByName;

		var events = new List<MockableEventResult>();
		var inaccessibleAbstractMembers = false;

		foreach (var selfEvent in mockType.GetMembers().OfType<IEventSymbol>()
			.Where(IsEventToExamine))
		{
			if (!selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation))
			{
				inaccessibleAbstractMembers = true;
			}
			else
			{
				events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
			}
		}

		var baseInterfaceEventsGroups = new List<List<IEventSymbol>>();

		foreach (var selfBaseInterface in mockType.AllInterfaces)
		{
			foreach (var selfBaseEvent in selfBaseInterface.GetMembers().OfType<IEventSymbol>()
				.Where(IsEventToExamine))
			{
				if (!selfBaseEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					if (!events.Any(_ => _.Value.Name == selfBaseEvent.Name))
					{
						var foundMatch = false;

						foreach (var baseInterfaceEventGroup in baseInterfaceEventsGroups)
						{
							if (baseInterfaceEventGroup.Any(_ => _.Name == selfBaseEvent.Name))
							{
								baseInterfaceEventGroup.Add(selfBaseEvent);
								foundMatch = true;
								break;
							}
						}

						if (!foundMatch)
						{
							baseInterfaceEventsGroups.Add([selfBaseEvent]);
						}
					}
				}
			}
		}

		foreach (var baseInterfaceEventGroup in baseInterfaceEventsGroups)
		{
			if (baseInterfaceEventGroup.Count == 1)
			{
				events.Add(new(baseInterfaceEventGroup[0],
					RequiresExplicitInterfaceImplementation.No, RequiresOverride.No));
			}
			else
			{
				foreach (var baseInterfaceEvent in baseInterfaceEventGroup)
				{
					events.Add(new(baseInterfaceEvent,
						RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No));
				}
			}
		}

		return new([.. events], inaccessibleAbstractMembers);
	}

	internal MockableEvents Events { get; }
}