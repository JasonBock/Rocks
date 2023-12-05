using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed class MockableEventDiscovery
{
	internal MockableEventDiscovery(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		var events = ImmutableArray.CreateBuilder<EventMockableResult>();
		var inaccessibleAbstractMembers = false;

		if (mockType.TypeKind == TypeKind.Interface)
		{
			foreach (var selfEvent in mockType.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeReferencedByName))
			{
				if (!selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
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
					.Where(_ => _.CanBeReferencedByName))
				{
					if (!selfBaseEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
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
								baseInterfaceEventsGroups.Add(new List<IEventSymbol>(1) { selfBaseEvent });
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
		}
		else
		{
			foreach (var selfEvent in mockType.GetMembers().OfType<IEventSymbol>()
				.Where(_ => !_.IsStatic && _.CanBeReferencedByName &&
					(_.IsAbstract || _.IsVirtual)))
			{
				var canBeSeen = selfEvent.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

				if (!canBeSeen && selfEvent.IsAbstract)
				{
					inaccessibleAbstractMembers = true;
				}
				else if (canBeSeen)
				{
					events.Add(new(selfEvent, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes));
				}
			}
		}

		this.Events = new(events.ToImmutable(), inaccessibleAbstractMembers);
	}

	internal MockableEvents Events { get; }
}