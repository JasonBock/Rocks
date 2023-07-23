Ugh! I never look to see if the member (constructor, method, etc.) is obsolete itself.

    // /0/Test0.cs(11,8): error ROCK10: The member GetPixelOptions is obsolete
    DiagnosticResult.CompilerError("ROCK10").WithSpan(11, 8, 11, 23),
    // /0/Test0.cs(14,8): error ROCK10: The member Values is obsolete
    DiagnosticResult.CompilerError("ROCK10").WithSpan(14, 8, 14, 14),
    // /0/Test0.cs(17,22): error ROCK10: The member ShadingOccurred is obsolete
    DiagnosticResult.CompilerError("ROCK10").WithSpan(17, 22, 17, 37),