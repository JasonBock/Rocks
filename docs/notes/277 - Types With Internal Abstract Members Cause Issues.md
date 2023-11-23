I'm already checking for CanBeReferencedByName. I think the real problem is that, if there's a member that's abstract and doesn't have a name that can be referenced, then, I'll filter them out, but then I get the error:

```
error CS0534: 'CreateExpectationsOfMapiContactPropertySetExtensions.RockMapiContactPropertySet' does not implement inherited abstract member 'MapiContactPropertySet.4gbnwbnbspeclmzqvzf8egt7ef2ytrtdMCa(MapiMessageItemBase)'
```

So...how can I handle this?

I could remove that filter, and then, before I return from the "GetMockableXYZ()" methods, see if there are any members that cannot be referenced **and** that are `abstract`. If so, then create a diagnostic and return. That'll stop the creation of any models and code gen.

"Cannot create a mock for a type that has abstract members with invalid identifiers." Or something like that.

If there are none of these, return with the members filtered with referenceable names.

I'm not sure what will break by me doing this :).


Remember to remove Aspose.Email before I commit because I still need to fix another related issue with it.

* DONE - Run code gen tests for Aspose.Email
* DONE - Run all code gen tests
* Run all unit + integration tests
* DONE - Add test cases for properties
* Profit

The code gen app seems to be taking more time to run. The last two runs, I had memory issues, specifically when I get to the Stripe.net package. I think what I should do is go back to my original approach, which is to filter out unreferenceable members, but then, before the return, look for any accessible unreferenceable abstract members, and if any exist, flip that flag. I think what's happening is that some assemblies (like Stripe) may make use of obfuscation, which I was removing before because I wasn't including them in my original LINQ expressions. But now, I am, and that's causing a major spike in memory allocation. Again, just a theory, not sure if that's the cause.


self.GetMembers().OfType<TSymbol>().Select(_ => !_.IsStatic && _.IsAbstract && !_.CanBeReferencedByName && _.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))

So, I just realized the problem was not stated correctly. What I need to do is find any members that are `abstract` and `internal`. That's a type Rocks can't mock.

And wouldn't you know it, I don't test for this condition in `NonPublicMembersGeneratorTests`. Ugh.

I think I remove the "InaccessibleAbstractMembers" properties from `MockableMethods`, `MockableProperties`, and `MockableEvents`, because all I do is look at the length. A `bool` would be much cheaper.

So I need to look for inaccessible, instance, abstract properties or methods (not indexers) and add that to the `bool` checks in the previous paragraph.

I'll need to add in Mono.Cecil again. Yay.

HasInaccessibleMembersWithInvalidIdentifiers()

type.Kind == TypeKind.Interface ?
    /* type + type.AllInterfaces 


type.GetMembers().Any(
				 _ => (_.Kind == SymbolKind.Method || _.Kind == SymbolKind.Property /* but not indexers */) &&
					!_.IsStatic && _.IsAbstract && _.CanBeReferencedByName && !SyntaxFacts.IsValidIdentifier(_.Name) &&
					_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol));

