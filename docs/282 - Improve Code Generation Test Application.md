Idea: Instead of seeing which types are valid for mocking, and which aren't, let's just put a group of them in batches into a `Generate()` call. Filter out any diagnostics that start with `ROCK` as the identifier. In other words, don't do `GetDiscoveredTypes()`, specifically the parallel portion that calls `IsValidTarget()`. Hopefully that removed a lot of pressure we're doing upfront.

Call the new method `GetTargets()`. We can also make the type filter better by
* Excluding structs => `!_.IsValueType`
* Excluding sealed => `!_.IsSealed`
* Is public => `_.IsPublic`
* Not ignored

That's it. Anything else may get too specific, we'll just let `MockModel` do its job.

I think we should partition the target types into sets. Maybe like 200. If the big assemblies are giving us 1000 targets, then we're doing 1002 compilations right now: 1000 for `IsValidTarget()`, and 2 for the create and make. With a partitioned approach, we'd only do 10.

BTW I just noticed that the memory consumption is getting around 22 GB when Stripe.net is hit. 

Current Values (up to Ardalis):

```
Getting target types for System.Private.CoreLib
Type count found for System.Private.CoreLib - 357
Testing System.Private.CoreLib - RockCreateGenerator
Testing System.Private.CoreLib - RockMakeGenerator

Getting target types for System.Net.Http
Type count found for System.Net.Http - 37
Testing System.Net.Http - RockCreateGenerator
Testing System.Net.Http - RockMakeGenerator

Getting target types for System.Collections.Immutable
Type count found for System.Collections.Immutable - 5
Testing System.Collections.Immutable - RockCreateGenerator
Testing System.Collections.Immutable - RockMakeGenerator

Getting target types for System.Text.Json
Type count found for System.Text.Json - 16
Testing System.Text.Json - RockCreateGenerator
Testing System.Text.Json - RockMakeGenerator

Getting target types for System.Threading.Channels
Type count found for System.Threading.Channels - 6
Testing System.Threading.Channels - RockCreateGenerator
Testing System.Threading.Channels - RockMakeGenerator

Getting target types for AngleSharp
Type count found for AngleSharp - 272
Testing AngleSharp - RockCreateGenerator
Testing AngleSharp - RockMakeGenerator

Getting target types for Ardalis.GuardClauses
Type count found for Ardalis.GuardClauses - 1
Testing Ardalis.GuardClauses - RockCreateGenerator
Testing Ardalis.GuardClauses - RockMakeGenerator
```

New approach:

```
Getting target types for System.Private.CoreLib
Type count found for System.Private.CoreLib - 447
Testing System.Private.CoreLib - RockCreateGenerator
Testing System.Private.CoreLib - RockMakeGenerator

Getting target types for System.Net.Http
Type count found for System.Net.Http - 37
Testing System.Net.Http - RockCreateGenerator
Testing System.Net.Http - RockMakeGenerator

Getting target types for System.Collections.Immutable
Type count found for System.Collections.Immutable - 7
Testing System.Collections.Immutable - RockCreateGenerator
Testing System.Collections.Immutable - RockMakeGenerator

Getting target types for System.Text.Json
Type count found for System.Text.Json - 25
Testing System.Text.Json - RockCreateGenerator
Testing System.Text.Json - RockMakeGenerator

Getting target types for System.Threading.Channels
Type count found for System.Threading.Channels - 6
Testing System.Threading.Channels - RockCreateGenerator
Testing System.Threading.Channels - RockMakeGenerator

Getting target types for AngleSharp
Type count found for AngleSharp - 275
Testing AngleSharp - RockCreateGenerator
Testing AngleSharp - RockMakeGenerator

Getting target types for Ardalis.GuardClauses
Type count found for Ardalis.GuardClauses - 3
Testing Ardalis.GuardClauses - RockCreateGenerator
Testing Ardalis.GuardClauses - RockMakeGenerator
```

And it is **much** faster. Let's see if I can get the counts right - let's just do AngleSharp.

OK, I think I got it filtering correctly. Highest I saw memory consumption was 7 GB. If I'm really concerned, I can partition the discovered types into groups of 200. BTW I also noticed that assemblies that have a lot of types (like Stripe and Twilio) actually go really fast. I'm guessing that the number of members on those types is really small.

I tried to get a `Stopwatch`-based number for the "old" way, and I can't even get it to run anymore on my machine. It crashes, locks up the machine, etc. Now, it's just under 2 minutes.