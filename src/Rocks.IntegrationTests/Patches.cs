// These are only here until I reference .NET 7.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class CompilerFeatureRequiredAttribute 
	: Attribute
{
	public CompilerFeatureRequiredAttribute(string featureName) => 
		this.FeatureName = featureName;

	public string FeatureName { get; }

	public bool IsOptional { get; init; }

	public const string RefStructs = nameof(RefStructs);

	public const string RequiredMembers = nameof(RequiredMembers);
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class RequiredMemberAttribute 
	: Attribute { }