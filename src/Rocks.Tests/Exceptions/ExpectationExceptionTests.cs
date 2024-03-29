﻿using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests.Exceptions;

public static class ExpectationExceptionTests
{
   [Test]
   public static void ThrowIfFalse() => 
		Assert.That(() => ExpectationException.ThrowIf(false), Throws.Nothing);

   [Test]
   public static void ThrowIfTrue() => 
		Assert.That(() => ExpectationException.ThrowIf(true), Throws.TypeOf<ExpectationException>());
}