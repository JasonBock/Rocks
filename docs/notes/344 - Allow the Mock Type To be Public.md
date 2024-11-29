* DONE - Make comment about the `final` CodeAnalysis package versions still in play
* DONE - Need to create `feature/9.1.0`
* DONE - Add `MockTypeVisibility` enum: `Public`, `Internal`, `Private`, can debate the first two.
* DONE - Create an optional `mockTypeVisibility` parameter with `RockAttribute` and `RockPartialAttribute`, default is `MockTypeVisibility.Private`
* DONE - That gets passed into the builder infrastructure.
* DONE - When the mock type is built, use that value to determine the visibility of the type
* Write a test to show CRGP in action
* Update docs
* Update changelog

Well...this didn't go as expected. One way out of this is for the developer to create an intermediary type:

```c#
public interface IProcessor<TProcessor>
	where TProcessor : IProcessor<TProcessor>
{
	void Process();
}

public abstract class Processor
    : IProcessor<Processor>
{
    public abstract void Process();
}

[assembly: Rock(typeof(Processor), BuildType.Create)]
```

I **think** this would work. Sort of similar when you have two or more interfaces to mock. Other mocking frameworks have a way to "combine" them, but Rocks takes the road of "you define the intermediate yourself":

```c#
public interface IA { }

public interface IB { }

public interface IC
    : IA, IB { }
```

`IC` is what you would use to mock stuff out with.