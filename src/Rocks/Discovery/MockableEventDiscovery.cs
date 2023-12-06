using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableEventDiscovery
{
   internal MockableEventDiscovery(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol) =>
		this.Events =
		   mockType.TypeKind == TypeKind.Interface ?
			   GetEventsForInterface(mockType, containingAssemblyOfInvocationSymbol) :
			   GetEventsForClass(mockType, containingAssemblyOfInvocationSymbol);

   private static MockableEvents GetEventsForClass(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol)
   {
	  var events = ImmutableArray.CreateBuilder<MockableEventResult>();
	  var inaccessibleAbstractMembers = false;

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

	  return new(events.ToImmutable(), inaccessibleAbstractMembers);
   }

   private static MockableEvents GetEventsForInterface(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol)
   {
	  var events = ImmutableArray.CreateBuilder<MockableEventResult>();
	  var inaccessibleAbstractMembers = false;

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

	  return new(events.ToImmutable(), inaccessibleAbstractMembers);
   }

   internal MockableEvents Events { get; }
}