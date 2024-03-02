So, one of the problems with changing it in the methods is that the `"` isn't handled correctly. But...I just had a revelation:

* For **all** member descriptions, make the string a raw string literal. Don't worry about doing any escaping
* For the `throw new global::Rocks.Exceptions.ExpectationException` calls, also make that a raw string literal.