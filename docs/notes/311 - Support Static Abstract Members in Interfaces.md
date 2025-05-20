https://github.com/JasonBock/Rocks/issues/311

So what are some things to look at

A decent amount of "generic math" interfaces use the Curiously Recurring Generic Pattern, or CRGP. I have in the docs that the way to support this is to make an intermediary type definition, which is literally a one-liner. So this may not be a big deal, and interfaces like `IParseable` don't do this. It's not a requirement :).