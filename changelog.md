# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [7.0.0-alpha.3] - Not Yet Released

### Added
- Indexers with an `init` can now be set up with `ConstructorProperties`, and properties and indexers with an `init` can be set up with expectations (issue [#197](https://github.com/JasonBock/Rocks/issues/197))

### Changed
- All direct casts now use `Unsafe.As<>()` (issue [#198](https://github.com/JasonBock/Rocks/issues/198))

### Fixed
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