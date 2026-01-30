# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [10.1.0] - Not Yet Released

### Added

- Added expectation overloads for `params` methods and indexers (issue [#268](https://github.com/JasonBock/Rocks/issues/268))

## [10.0.0] - 2025-12-01

### Changed

- Pushed the final 10.0.0 release

## [10.0.0-alpha.2] - 2025-11-04

### Changed

- Updated the generated expectations API (issue [#394](https://github.com/JasonBock/Rocks/issues/394))

## [10.0.0-alpha.1] - 2025-09-29

### Changed

- Removed `DisableVerification` flag from `RockContext` (issue [#393](https://github.com/JasonBock/Rocks/issues/393))

### Fixed

- Fixed generated code for property setter expectation logic (issue [#392](https://github.com/JasonBock/Rocks/issues/392))
- Made all explicit implementations in shim members cast to the type of the explicit implementation (issue [#389](https://github.com/JasonBock/Rocks/issues/389) and issue [#390](https://github.com/JasonBock/Rocks/issues/390))
- Mocks no longer includes `[UnscopedRef]` (issue [#391](https://github.com/JasonBock/Rocks/issues/391))
- No longer generating `[MemberNotNullWhen]` on mock types (issue [#395](https://github.com/JasonBock/Rocks/issues/395))

## [9.3.0] - 2025-09-14

### Changed

- Made `Verify()` throw an exception if expectations were set and an instance was never made (issue [#382](https://github.com/JasonBock/Rocks/issues/382) and issue [#367](https://github.com/JasonBock/Rocks/issues/367))
- Refined symbol search for refactoring (issue [#384](https://github.com/JasonBock/Rocks/issues/384))

## [9.2.0] - 2025-07-07

### Added

- Added more syntax node support for `AddRockAttributeRefactoring` (issue [#378](https://github.com/JasonBock/Rocks/issues/378))
- Added `DisableVerification` flag to `RockContext` to allow developers to temporarily disable verification (issue [#377](https://github.com/JasonBock/Rocks/issues/377))

### Fixed

- Changed `Expectations` to not call `Verify()` if the mock threw a code generated exception (issue [#377](https://github.com/JasonBock/Rocks/issues/377))

## [9.1.0] - 2025-05-11

### Added

- Created a refactoring to simplify `[Rock]` creation (issue [#361](https://github.com/JasonBock/Rocks/issues/361) and issue [#369](https://github.com/JasonBock/Rocks/issues/369))
- Added a suppressor for `CA2012` when a `ValueTask` is passed to `ReturnValue` (issue [#354](https://github.com/JasonBock/Rocks/issues/354))
- Added `RockContext` to group expectation instances together and automatically call `Verify()` (issue [#365](https://github.com/JasonBock/Rocks/issues/365))

### Changed

- Made a number of minor internal refactorings that made compile-time code generation consume slightly less memory (issue [#358](https://github.com/JasonBock/Rocks/issues/358) and issue [#367](https://github.com/JasonBock/Rocks/issues/367))
- Restructured package to separate analyzers and runtime types (issue [#339](https://github.com/JasonBock/Rocks/issues/339))

### Fixed

- Interfaces with non-public members that have a default implementation are no longer considered for mocking (issue [#364](https://github.com/JasonBock/Rocks/issues/364))
- Generated constructors calling a base constructor attributed with `[SetsRequiredMembers]` will now have that attribute as well (issue [#368](https://github.com/JasonBock/Rocks/issues/368))
- Shim types now include generic type parameter names (issue [#374](https://github.com/JasonBock/Rocks/issues/374))
- Shim types with an `init` property now work with no issues (issue [#373](https://github.com/JasonBock/Rocks/issues/373))

## [9.0.2] - 2025-03-17

### Fixed

- Handlers for methods with `ref` or `out` parameters and parameters with default values are now generated correctly (issue [#350](https://github.com/JasonBock/Rocks/issues/350))
- Events in interfaces that contain members with default implementations are now handled correctly in shim types (issue [#349](https://github.com/JasonBock/Rocks/issues/349))
- Members using generic types with different type parameters are now generated correctly (issue [#351](https://github.com/JasonBock/Rocks/issues/351))
- Hiding members that are different only by type parameter constraints will now work (issue [#352](https://github.com/JasonBock/Rocks/issues/352))

## [9.0.1] - 2024-12-15

### Fixed

- Non-public types used with parameters or return types now work correctly (issue [#345](https://github.com/JasonBock/Rocks/issues/345))

## [9.0.0] - 2024-12-4

### Added

- `[RockPartial]` allows the user to define the name of the expectations type (issue [#342](https://github.com/JasonBock/Rocks/issues/342))

## [9.0.0-alpha.1] - 2024-10-22

### Added

- Type parameters constrained with `allows ref struct` are now supported (issue [#331](https://github.com/JasonBock/Rocks/issues/331))
- Added more parameter information when an `ExpectationException` occurs (issue [#338](https://github.com/JasonBock/Rocks/issues/338))

### Changed

- Closed generics are no longer valid types to mock (issue [#318](https://github.com/JasonBock/Rocks/issues/318))
- The following attributes were removed: `RockCreateAttribute`, `RockCreateAttribute<>`, `RockMakeAttribute`, and `RockMakeAttribute<>` (issue [#324](https://github.com/JasonBock/Rocks/issues/324))
- Improved handling of `ref struct` types (issue [#170](https://github.com/JasonBock/Rocks/issues/170))
- Improved handling of pointer and special types (issue [#330](https://github.com/JasonBock/Rocks/issues/330))

### Fixed

- `scoped` is no longer added to a `params` argument (issue [#328](https://github.com/JasonBock/Rocks/issues/328))
- Parameters for base member invocations now make all names literal (issue [#334](https://github.com/JasonBock/Rocks/issues/334))
- "Caller" attributes are no longer emitted with parameters that have optional values and the method is explicitly implemented (issue [#332](https://github.com/JasonBock/Rocks/issues/332))
- Static virtual members on interfaces will not be processed (issue [#337](https://github.com/JasonBock/Rocks/issues/3327))
- Special types (`ArgIterator`, `RuntimeArgumentHandle`, `TypedReference`) used as parameter or return types are now handled (issue [#329](https://github.com/JasonBock/Rocks/issues/329))
- Nested generic types are now handled correctly (issue [#333](https://github.com/JasonBock/Rocks/issues/333))
- Members that hide other members are now handled correctly (issue [#336](https://github.com/JasonBock/Rocks/issues/336))

## [8.3.1] - 2024-09-30

### Fixed

- Addressed a rare naming issue with projects referencing Rocks and each other (issue [#340](https://github.com/JasonBock/Rocks/issues/340))

## [8.3.0] - 2024-07-24

### Added

- Added a diagnostic to warn about mocking closed generic types (issue [#326](https://github.com/JasonBock/Rocks/issues/326))
- Added collection expression support for `EquatableArray<>` (issue [#327](https://github.com/JasonBock/Rocks/issues/327))
- Created a new attribute, `RockAttribute`, and obsoleted the existing `RockCreateAttribute` and `RockMakeAttribute` as a warning (issue [#323](https://github.com/JasonBock/Rocks/issues/323))

### Changed

- Minor project changes: removed GitHub source linking, embedded debug information, and added NuGet auditing (issue [#321](https://github.com/JasonBock/Rocks/issues/321))

### Fixed

- Nullable types within type definitions are now constrained correctly (issue [#320](https://github.com/JasonBock/Rocks/issues/320))

## [8.2.0] - 2024-07-17

### Changed

- Rare naming collisions with the generated expectations class and property names are resolved (issue [#288](https://github.com/JasonBock/Rocks/issues/288))
- Generated code is now excluded from code coverage (issue [#312](https://github.com/JasonBock/Rocks/issues/312))

### Fixed

- Non-public member descriptions are now found (issue [#317](https://github.com/JasonBock/Rocks/issues/317))

## [8.1.0] - 2024-04-16

### Changed

- Open generics are now supported (e.g. `[RockCreate(typeof(IService<>))]`) (issue [#287](https://github.com/JasonBock/Rocks/issues/287))
- Changed how handlers were managed in a mock (issue [#302](https://github.com/JasonBock/Rocks/issues/302))
- Made minor updates to improve performance (issue [#298](https://github.com/JasonBock/Rocks/issues/298))
- Added support for `scoped` parameters (issue [#304](https://github.com/JasonBock/Rocks/issues/304))
- Removed the member descriptions in generated code, and replaced with run-time discovery (issue [#307](https://github.com/JasonBock/Rocks/issues/307))

## [8.0.0] - 2024-01-22

### Changed

- Final release for 8.0.0

## [8.0.0-alpha.3] - 2024-01-21

### Changed

- Moved diagnostic reporting to an analyzer (issue [#289](https://github.com/JasonBock/Rocks/issues/289))
- Prevented expectations from being set after a mock was created (issue [#294](https://github.com/JasonBock/Rocks/issues/294))

### Fixed

- Mock types with explicit and non-explicit properties/indexers now generate correct code (issue [#299](https://github.com/JasonBock/Rocks/issues/299))
- Ignored a warning when a make was generated and `[MemberNotNullWhen]` existed (issue [#297](https://github.com/JasonBock/Rocks/issues/297))
- Added generics to event extension methods when needed (issue [#295](https://github.com/JasonBock/Rocks/issues/295))
- Fixed a variable naming issue in the member expectation handlers (issue [#296](https://github.com/JasonBock/Rocks/issues/296))

## [8.0.0-alpha.2] - 2024-01-14

### Changed

- Updated handler list generation to reduce allocations (issue [#292](https://github.com/JasonBock/Rocks/issues/292) and issue [#293](https://github.com/JasonBock/Rocks/issues/293))

## [8.0.0-alpha.1] - 2024-01-13

### Changed

- Made internal refactors for member discovery (issue [#235](https://github.com/JasonBock/Rocks/issues/235))
- Changed mock creation to eliminate casts and use attributes to initiate mock code generation (issue [#283](https://github.com/JasonBock/Rocks/issues/283))

### Fixed

- `sealed` members on interfaces are no longer included in the mock type (issue [#284](https://github.com/JasonBock/Rocks/issues/284))

## [7.3.0] - 2023-11-25

### Changed

- Updated projects to use .NET 8.

### Fixed

- No longer emitting `[Nullable]` or `[NullableContext]` attributes (issue [#273](https://github.com/JasonBock/Rocks/issues/273))
- Using a type with nullable types will now create a valid file name (issue [#275](https://github.com/JasonBock/Rocks/issues/275))
- Fixed an issue with parameters marked with `[AllowNull]` and had `default` as the default value (issue [#279](https://github.com/JasonBock/Rocks/issues/279))
- Overriding methods that change the name of generic parameters are now handled correctly (issue [#280](https://github.com/JasonBock/Rocks/issues/280))
- Non-virtual static members in interfaces are no longer included in the mock (issue [#276](https://github.com/JasonBock/Rocks/issues/276))
- Calling base members in the mock will use the parameter name to ensure the correct base member is invoked (issue [#278](https://github.com/JasonBock/Rocks/issues/278))
- Internal abstract members with unreferenceable names are now handled correctly (issue [#277](https://github.com/JasonBock/Rocks/issues/277))
- References that have aliases will now be used in Rocks during mock generation (issue [#281](https://github.com/JasonBock/Rocks/issues/281))

## [7.2.0] - 2023-11-10

### Added

- Created overloads for member expectation setup when the parameters have optional values (issue [#264](https://github.com/JasonBock/Rocks/issues/264))

### Changed

- Changed diagnostics so their location is on the `Rock.Create()` or `Rock.Make()` invocation (issue [#270](https://github.com/JasonBock/Rocks/issues/270))
- Altered diagnostic creation for obsolete type usage (issue [#267](https://github.com/JasonBock/Rocks/issues/267))
- Removed diagnostic docs that were no longer relevant (issue [#272](https://github.com/JasonBock/Rocks/issues/272))
- Made all package references in Rocks private (issue [#266](https://github.com/JasonBock/Rocks/issues/266))

### Fixed

- Corrected how generic methods are handled in the mock (issue [#269](https://github.com/JasonBock/Rocks/issues/269))
- Fixed how methods are matches with nullability differences in arrays (issue [#271](https://github.com/JasonBock/Rocks/issues/271))

## [7.1.4] - 2023-10-7

### Fixed

- Ensured members that have `[DoesNotReturn]` generate code with no returns (issue [#175](https://github.com/JasonBock/Rocks/issues/175))

### Added

- Put the "auto-generated" comment in the generated code (issue [#260](https://github.com/JasonBock/Rocks/issues/260))
- Adding separate checks for obsolete usage (issue [#249](https://github.com/JasonBock/Rocks/issues/249))

## [7.1.3] - 2023-07-30

### Fixed

- Duplicate constructors will prevent a type from being mocked (issue [#256](https://github.com/JasonBock/Rocks/issues/256))

## [7.1.2] - 2023-07-26

### Fixed

- Fixed a type name when an explicit property setter extension method is made (issue [#229](https://github.com/JasonBock/Rocks/issues/229))
- Fixed a type name when an explicit event is declared in the mock (issue [#238](https://github.com/JasonBock/Rocks/issues/238))
- Added unmanaged constraints to type parameters for projected pointer types (issue [#242](https://github.com/JasonBock/Rocks/issues/242))
- Constraints on type parameters that are inaccessible will prevent a mock from being created (issue [#237](https://github.com/JasonBock/Rocks/issues/237))
- Mockable members that are considered obsolete will prevent a mock from being created (issue [#240](https://github.com/JasonBock/Rocks/issues/240))
- Methods that hide mockable methods are no longer generated (issue [#239](https://github.com/JasonBock/Rocks/issues/239))
- Addressed when explicit interface implementations should occur in shims (issue [#231](https://github.com/JasonBock/Rocks/issues/231))
- Correctly detecting when duplicate method signatures exist on the same type (issue [#232](https://github.com/JasonBock/Rocks/issues/232))
- Abstract members that match an existing non-virtual member will prevent a mock from being made (issue [#247](https://github.com/JasonBock/Rocks/issues/247))
- Added the null forgiving operator for shim members (issue [#246](https://github.com/JasonBock/Rocks/issues/246))
- Updated type generation for `Task` types in makes to use a FQN (issue [#248](https://github.com/JasonBock/Rocks/issues/248))
- Expanded the search for obsolete type usage (issue [#250](https://github.com/JasonBock/Rocks/issues/250))
- Handled another issue with duplicate members on a type (issue [#251](https://github.com/JasonBock/Rocks/issues/251))

## [7.1.1] - 2023-07-23

### Fixed

- Changed the file name to reduce collisions (issue [#219](https://github.com/JasonBock/Rocks/issues/219))
- Members with more than 16 parameters are now handled correctly (issue [#234](https://github.com/JasonBock/Rocks/issues/234))
- `ConditionalAttribute` instances are no longer emitted (issue [#233](https://github.com/JasonBock/Rocks/issues/233))
- Attributes with a `return:` target are now emmitted correctly (issue [#230](https://github.com/JasonBock/Rocks/issues/230))
- Non-accessible types are now considered to determine a member's visibility (issue [#213](https://github.com/JasonBock/Rocks/issues/213))
- Base properties that are hid and have different accessors are new explicitly implemented (issue [#227](https://github.com/JasonBock/Rocks/issues/227))
- Default values for optional parameters are no longer being emitted for explicit implementations (issue [#225](https://github.com/JasonBock/Rocks/issues/225))
- Exact matches for methods on an interface are no longer duplicated (issue [#226](https://github.com/JasonBock/Rocks/issues/226))
- Handled a case where a type parameter value led to matching members (issue [#217](https://github.com/JasonBock/Rocks/issues/217))
- Fixed an issue with pointer projected types based on a type parameter (issue [#215](https://github.com/JasonBock/Rocks/issues/215))
- Addressed an issue when a `notnull` constraint was not being emitted (issue [#204](https://github.com/JasonBock/Rocks/issues/204))

## [7.1.0] - 2023-07-03

- Change the internals of Rocks to use read-only models for mock generation (issue [#212](https://github.com/JasonBock/Rocks/issues/212))

## [7.0.1] - 2023-03-30

### Fixed
- If an exception is thrown in `Callback()`, the `Verify()` call now works as expected (issue [#220](https://github.com/JasonBock/Rocks/issues/220))
- When `Verify()` fails, the resulting exception message now contains the correct member information (issue [#221](https://github.com/JasonBock/Rocks/issues/221))
- Added XML documentation and changed the visibility on some members that were not used in generated code (issue [#138](https://github.com/JasonBock/Rocks/issues/138))
- Changed the name of the exception `MockException` to `NoReturnValueException` (issue [#223](https://github.com/JasonBock/Rocks/issues/223))
- Enforced dispose rules on `RockRepository` (issue [#222](https://github.com/JasonBock/Rocks/issues/222))

## [7.0.0] - 2022-11-19

### Added
- Created the final release (issue [#218](https://github.com/JasonBock/Rocks/issues/218))

## [7.0.0-alpha.3] - 2022-11-08

### Added
- Indexers with an `init` can now be set up with `ConstructorProperties`, and properties and indexers with an `init` can be set up with expectations (issue [#197](https://github.com/JasonBock/Rocks/issues/197))

### Changed
- All direct casts now use `Unsafe.As<>()` (issue [#198](https://github.com/JasonBock/Rocks/issues/198))

### Fixed
- `protected internal` properties and indexers with different visibilities on the `get`, `set`, or `init` now works correctly (issue [#216](https://github.com/JasonBock/Rocks/issues/216))
- Named tuple elements no longer cause an issue in the mock (issue [#214](https://github.com/JasonBock/Rocks/issues/214))
- Projected types using an open generic type are now generated correctly (issue [#211](https://github.com/JasonBock/Rocks/issues/211))
- Members that are hidden by accessible static members are now removed (issue [#206](https://github.com/JasonBock/Rocks/issues/206))
- Methods on classes with generics that have `class` or `struct` constraints are now emitted on the mock (issue [#210](https://github.com/JasonBock/Rocks/issues/210))
- Projected delegates now have constraints added (issue [#202](https://github.com/JasonBock/Rocks/issues/202))
- Parameters with generic types and a `default` optional value are now generated correctly (issue [#200](https://github.com/JasonBock/Rocks/issues/200))
- Enum value references in optional arguments are handled correctly (issue [#209](https://github.com/JasonBock/Rocks/issues/209))
- Visibility differences in properties are handled correctly (issue [#203](https://github.com/JasonBock/Rocks/issues/203) and issue [#207](https://github.com/JasonBock/Rocks/issues/207))
- Default values for optional parameters like `double.PositiveInfinity` will no longer print out the infinity symbol (issue [#208](https://github.com/JasonBock/Rocks/issues/208))
- Internal members that are not visible will not be included in the mock (issue [#199](https://github.com/JasonBock/Rocks/issues/199))

## [7.0.0-alpha.2] - 2022-10-10

### Fixed
- Generated shim types to handle DIM invocations have been updated to remove a couple of errors (issue [#188](https://github.com/JasonBock/Rocks/issues/188))
- If Rocks encounters the `AsyncIteratorStateMachineAttribute`, it will not generate its description in the mock (issue [#185](https://github.com/JasonBock/Rocks/issues/185))
- Async iterators are now handled correctly (issue [#191](https://github.com/JasonBock/Rocks/issues/191))
- Members that use obsolete types will flag the type to mock as unmockable (issue [#182](https://github.com/JasonBock/Rocks/issues/182))
- Constraints are generated correctly, based on the type to mock (issue [#186](https://github.com/JasonBock/Rocks/issues/186))
- Support for types with hidden members have been improved (issue [#187](https://github.com/JasonBock/Rocks/issues/187))
- Types that have `internal abstract` members are now flagged as being unmockable (issue [#181](https://github.com/JasonBock/Rocks/issues/181))
- Parameters that do not have nullable annotation and have a `null` optional value are now handled correctly (issue [#192](https://github.com/JasonBock/Rocks/issues/192))
- Only one of these constraints is now added to the generated mock: `unmanaged`, `notnull`, `class`, or `struct` (issue [#180](https://github.com/JasonBock/Rocks/issues/180))
- Generated delegates for projected types now use SHA to create the hash code (issue [#194](https://github.com/JasonBock/Rocks/issues/194))
- Types that use C# keywords for parameters (e.g. `string @namespace`) are now handed by Rocks (issue [#184](https://github.com/JasonBock/Rocks/issues/184))
- Parameter names will no longer collide with variables created by Rocks (issue [#183](https://github.com/JasonBock/Rocks/issues/183))
- Constraints on generic parameters from methods are now carried to the extension methods used for setting expectations (issue [#179](https://github.com/JasonBock/Rocks/issues/179))
- For makes, any `Task<>` or `ValueTask<>` return types are marked with the null-forgiving operator when the return type is not nullable. (issue [#174](https://github.com/JasonBock/Rocks/issues/174))
- For makes, `[AllowNull]` now shows up on properties when the base property has it defined. (issue [#174](https://github.com/JasonBock/Rocks/issues/174))
- Constraint ordering has been fixed in certain cases (it was failing when there were `struct` and interface constraints) (issue [#174](https://github.com/JasonBock/Rocks/issues/174))
- Method matching now correctly handles cases where the either parameters or return types use generic type parameters. (issue [#174](https://github.com/JasonBock/Rocks/issues/174))
- If an interface has static abstract members, Rocks will raise the `InterfaceHasStaticAbstractMembersDiagnostic` (`ROCK7`). (issue [#174](https://github.com/JasonBock/Rocks/issues/174))

## [7.0.0-alpha.1] - 2022-09-10

### Added

- While this package technically targets .NET 6, this version is intended to target .NET 7.0 and C# 11-related features.
- `required` and `init` properties can be set in the `Instance()` method via a `ConstructorProperties` type (issue [#162](https://github.com/JasonBock/Rocks/issues/162))

## [6.4.5] - 2022-06-29

### Added

- Added the README.md file to the package (issue [#168](https://github.com/JasonBock/Rocks/issues/168)).

### Changed

- Methods that have the `[DoesNotReturn]` attribute are now mocked such that they will throw `DoesNotReturnException`. This addresses the `CS8763` warning (issue [#169](https://github.com/JasonBock/Rocks/issues/169)). 
- If the mock type is an interface and it contains methods that match accessible methods from the `object` type (e.g. `Equals(T? other)` from `IEquatable<T>`), the methods will be implemented in the mock explicitly. This addresses the `CS0114` and `CS0108` warnings (issue [#169](https://github.com/JasonBock/Rocks/issues/169)).
- Properties with the `[AllowNull]` attribute are now handled correctly. This addresses the `CS8765` warning (issue [#169](https://github.com/JasonBock/Rocks/issues/169)).
- If a property expectation was not met, calling `Verify()` was causing `InvalidOperationException` to be thrown instead of `VerificationException`. This is fixed (issue [#171](https://github.com/JasonBock/Rocks/issues/171)).

## [6.4.4] - 2022-06-27

### Changed

- Fixed issue with ref-like types (e.g. `Span<T>`) being used for the return type of a method, or a getter of a property.
- Fixed issue mocking a class with an accessible constructor that had `ref`, `out`, and/or `params` parameter.
- Fixed issue referencing types anywhere within the mock that was a nested type.
- Fixed issue with explicit implementation from a generic type was not emitted correctly.
- Fixed issue with types with members that "shadow" base members (think `IEnumerable<T>`) - now one is being explicitly implemented.
- Fixed issue where Rocks was allowing special types (e.g. `Delegate`, `ValueType`, `Enum`) to be mocked, and they can't be.
- Fixed issue When a base method is called because it is `virtual` and no expectations were set, if method has a `params` parameter, it will now be called correctly.
- Fixed issue with enums with negative values causing issues in attribute data.
- Fixed issue adding attributes to members when they were not able to be "seen" by the assembly.
- Fixed issue where methods had certain attributes that should not be included in the mock (`IteratorStateMachineAttribute`, `CompilerGeneratedAttribute`, and `AsyncStateMachine`).
- Fixed issue when projecting types for pointers, ref-like types, etc., from types to mock that were in the same namespace.
