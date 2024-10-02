It seems like the regression came in with 8.2.0, and it looks like it may be related to [this fix](https://github.com/JasonBock/Rocks/issues/288). The workaround with `InternalsVisibleTo` doesn't seem to workaround the issue.

Try a similar thing with RegEx generation, are there class name

TODO:
* If I'm generating a type, like I'm doing in `9.0.0` with `RefStructArgument`, will this bring in a similar issue where type names may collide. I may need to make sure that these gen'd types are `internal`, **and**, if someone does a `InternalsVisibleTo`, then I may not need to generate them at all.
* `[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Text.RegularExpressions.Generator", "8.0.10.36612")]` - probably need to polyfill this, but interesting nonetheless