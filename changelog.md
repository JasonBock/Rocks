# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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