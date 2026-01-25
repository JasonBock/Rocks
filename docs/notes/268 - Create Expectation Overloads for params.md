We need to do a couple of things:

* During expectations generation for a method and indexers
    * If the `params` is **not** ref-like, then we do another generation pass. In other words, change this: `needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue || parameter.IsParams;` to this: `needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue || (parameter.IsParams && !parameter.Type.IsRefLikeType)`;
    * If a 2nd pass is needed (e.g. because there's an optional parameter), then if the `params` argument **is** ref-like, we still generate with `RefStructArgument` as the parameter, and simple pass that over to the overload.