Create (typeof(object)):
  Time: 00:00:00.5600766, Type: Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid
  Time: 00:00:00.5131633, Type: System.Type

Create (typeof(AngleSharp.BrowsingContext)):
  Time: 00:00:01.7321634, Type: AngleSharp.Html.Dom.IHtmlInputElement
  Time: 00:00:01.6925403, Type: AngleSharp.Dom.IDocument
  Time: 00:00:01.4734223, Type: AngleSharp.Html.Dom.IHtmlVideoElement
  Time: 00:00:01.4095603, Type: AngleSharp.Html.Dom.IHtmlMediaElement
  Time: 00:00:01.3817389, Type: AngleSharp.Html.Dom.IHtmlAudioElement
  Time: 00:00:01.3520646, Type: AngleSharp.Html.Dom.IHtmlAreaElement
  Time: 00:00:01.3273438, Type: AngleSharp.Html.Dom.IHtmlTextAreaElement
  Time: 00:00:01.2268048, Type: AngleSharp.Html.Dom.IHtmlInlineFrameElement

Make (typeof(AngleSharp.BrowsingContext)):
  Time: 00:00:00.4359708, Type: AngleSharp.Configuration
  Time: 00:00:00.1556189, Type: AngleSharp.Svg.Dom.ISvgCircleElement
  Time: 00:00:00.1464230, Type: AngleSharp.Svg.Dom.ISvgDescriptionElement

Create with no event extension methods made (typeof(AngleSharp.BrowsingContext)):
  Time: 00:00:00.5625743, Type: AngleSharp.Configuration
  Time: 00:00:00.3376438, Type: AngleSharp.Svg.Dom.ISvgElement
  Time: 00:00:00.3372373, Type: AngleSharp.Svg.Dom.ISvgSvgElement
  Time: 00:00:00.3349859, Type: AngleSharp.Svg.Dom.ISvgForeignObjectElement
  Time: 00:00:00.3096635, Type: AngleSharp.Svg.Dom.ISvgTitleElement
  Time: 00:00:00.3055880, Type: AngleSharp.Svg.Dom.ISvgCircleElement

One thing I saw is that those "element" interfaces all derive from `AngleSharp.Dom.Events.IGlobalEventHandlers`, and that has approximately 50+ events. For each member on the type, I'm generating 50+ extension methods. That's a lot. As soon as I commented out the code in `MockEventExtensionsBuilder.Build()`, those types disappeared from the top of the list. So, I have to think of a way to generate those extension methods "faster" or with more "elegance". I could also consider giving an options switch where the user could choose to turn that off, but my immediate reaction is that I don't like that approach.

Maybe create a `IAdornment` interface with one method: `AddRaiseEvent()`, that both adornment classes derive from:

```c#
public interface IAdornments<TAdornments>
    where TAdornments : IAdornments<TAdornments>
{
    TAdornments AddRaiseEvent(RaiseEventInformation raiseEvent);
    // We could put ExpectedCallCount() and CallCount() on this interface as well.
}
```

Then, I generate one extension method for each event, and the `self` is typed as `IAdornment`. This has a big of a danger in that these would work for **any** adornment, even if that adornment type doesn't have events. I could generate a custom adornment type for each member, but then I have to come up with names for each one, and that kind of sucks :). It would be nice if I could somehow constrain it so the extensions are only for the the adornment types needed, but...that's kind of what I'm doing by generating one for each adornment type. 

Hmmmm....maybe generating custom adornment types won't be that bad. I can use the handlers name:

```c#
internal sealed class AdornmentsForHandler0
    : global::Rocks.Adornments<AdornmentsForHandler0, global::AbstractClassMethodReturnWithEventsCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> { }
```

I'd only have to do if events are in play, but it might be easier just to do this regardless. Then, I can also generated a custom interface that only these adornment types derive from, and then gen a set of event extension methods just for that custom interface:

```c#
internal interface IAdornmentsForAbstractClassMethodReturnWithEventsCreate<TAdornments>
    : IAdornments<TAdornments>
    // Not sure I can change the constraint like this...
    where TAdornments : IAdornmentsForAbstractClassMethodReturnWithEventsCreate<TAdornments> { }

internal sealed class AdornmentsForHandler0
    : global::Rocks.Adornments<AdornmentsForHandler0, global::AbstractClassMethodReturnWithEventsCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, 
    global::AbstractClassMethodReturnWithEventsCreateExpectations.IAdornmentsForAbstractClassMethodReturnWithEventsCreate<AdornmentsForHandler0> { }

internal static class AbstractClassMethodReturnWithEventsAdornmentsEventExtensions
{
    internal static TAdornments RaiseMyEvent<TAdornments>(this TAdornments self, global::System.EventArgs args)
        where TAdornments : global::AbstractClassMethodReturnWithEventsCreateExpectations.IAdornmentsForAbstractClassMethodReturnWithEventsCreate<TAdornments> => 
        self.AddRaiseEvent(new("MyEvent", args));
}
```

Now the extension methods will only work for those adornments within that expectations class that houses all of the custom types. This just might work... 

Modified statistics/times:

```
Slowest Generation Times
Generation Time: 00:00:00.1422370, Emit Time: 00:00:00.3431724, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0834348, Emit Time: 00:00:00.8624710, Type: AngleSharp.Html.Dom.IHtmlOptionsGroupElement
Generation Time: 00:00:00.0789988, Emit Time: 00:00:01.0008188, Type: AngleSharp.Html.Dom.IHtmlBodyElement
Generation Time: 00:00:00.0765985, Emit Time: 00:00:00.8784135, Type: AngleSharp.Html.Dom.IHtmlTableDataCellElement
Generation Time: 00:00:00.0763967, Emit Time: 00:00:00.8258181, Type: AngleSharp.Html.Dom.IHtmlHeadingElement
Generation Time: 00:00:00.0752168, Emit Time: 00:00:00.8969815, Type: AngleSharp.Html.Dom.IHtmlFieldSetElement
Generation Time: 00:00:00.0744640, Emit Time: 00:00:00.8484363, Type: AngleSharp.Html.Dom.IHtmlTableCellElement
Generation Time: 00:00:00.0742130, Emit Time: 00:00:01.4644373, Type: AngleSharp.Html.Dom.IHtmlInputElement
Generation Time: 00:00:00.0708833, Emit Time: 00:00:00.9671056, Type: AngleSharp.Html.Dom.IHtmlInlineFrameElement
Generation Time: 00:00:00.0635855, Emit Time: 00:00:01.2324526, Type: AngleSharp.Html.Dom.IHtmlMediaElement

Slowest Emit Times
Generation Time: 00:00:00.0612721, Emit Time: 00:00:01.7592233, Type: AngleSharp.Dom.IDocument
Generation Time: 00:00:00.0742130, Emit Time: 00:00:01.4644373, Type: AngleSharp.Html.Dom.IHtmlInputElement
Generation Time: 00:00:00.0589239, Emit Time: 00:00:01.2872871, Type: AngleSharp.Html.Dom.IHtmlVideoElement
Generation Time: 00:00:00.0635855, Emit Time: 00:00:01.2324526, Type: AngleSharp.Html.Dom.IHtmlMediaElement
Generation Time: 00:00:00.0439750, Emit Time: 00:00:01.2155606, Type: AngleSharp.Html.Dom.IHtmlTextAreaElement
Generation Time: 00:00:00.0622886, Emit Time: 00:00:01.2004067, Type: AngleSharp.Html.Dom.IHtmlAudioElement
Generation Time: 00:00:00.0512945, Emit Time: 00:00:01.1522267, Type: AngleSharp.Html.Dom.IHtmlAreaElement
Generation Time: 00:00:00.0483516, Emit Time: 00:00:01.0867410, Type: AngleSharp.Html.Dom.IHtmlSelectElement
Generation Time: 00:00:00.0460070, Emit Time: 00:00:01.0778344, Type: AngleSharp.Html.Dom.IHtmlButtonElement
Generation Time: 00:00:00.0580725, Emit Time: 00:00:01.0484765, Type: AngleSharp.Html.Dom.IHtmlFormElement

Fasted Generation Times
Generation Time: 00:00:00.0087038, Emit Time: 00:00:00.0041275, Type: AngleSharp.Browser.ICommandProvider
Generation Time: 00:00:00.0087508, Emit Time: 00:00:00.0047279, Type: AngleSharp.Common.ICancellable
Generation Time: 00:00:00.0088153, Emit Time: 00:00:00.0181166, Type: AngleSharp.Browser.INavigationHandler
Generation Time: 00:00:00.0089017, Emit Time: 00:00:00.0045036, Type: AngleSharp.Html.Forms.Submitters.IHtmlEncoder
Generation Time: 00:00:00.0089117, Emit Time: 00:00:00.0043210, Type: AngleSharp.Css.Parser.ICssSelectorParser
Generation Time: 00:00:00.0089609, Emit Time: 00:00:00.0056040, Type: AngleSharp.Html.Forms.IFormSubmitter
Generation Time: 00:00:00.0089621, Emit Time: 00:00:00.0075778, Type: AngleSharp.Css.IStylingService
Generation Time: 00:00:00.0089627, Emit Time: 00:00:00.0045324, Type: AngleSharp.Css.IAttributeSelectorFactory
Generation Time: 00:00:00.0090039, Emit Time: 00:00:00.0062419, Type: AngleSharp.Browser.EncodingMetaHandler
Generation Time: 00:00:00.0090388, Emit Time: 00:00:00.0057040, Type: AngleSharp.Html.Dom.Events.ITouchList

Fasted Emit Times
Generation Time: 00:00:00.0196178, Emit Time: 00:00:00.0040173, Type: AngleSharp.Browser.Dom.INavigatorOnline
Generation Time: 00:00:00.0093462, Emit Time: 00:00:00.0040695, Type: AngleSharp.Browser.Dom.INavigatorStorageUtilities
Generation Time: 00:00:00.0087038, Emit Time: 00:00:00.0041275, Type: AngleSharp.Browser.ICommandProvider
Generation Time: 00:00:00.0200156, Emit Time: 00:00:00.0041478, Type: AngleSharp.Browser.IEncodingProvider
Generation Time: 00:00:00.0090894, Emit Time: 00:00:00.0042735, Type: AngleSharp.Dom.Events.IEventFactory
Generation Time: 00:00:00.0089117, Emit Time: 00:00:00.0043210, Type: AngleSharp.Css.Parser.ICssSelectorParser
Generation Time: 00:00:00.0224474, Emit Time: 00:00:00.0044088, Type: AngleSharp.Css.IPseudoClassSelectorFactory
Generation Time: 00:00:00.0090898, Emit Time: 00:00:00.0044835, Type: AngleSharp.Css.IPseudoElementSelectorFactory
Generation Time: 00:00:00.0089017, Emit Time: 00:00:00.0045036, Type: AngleSharp.Html.Forms.Submitters.IHtmlEncoder
Generation Time: 00:00:00.0089627, Emit Time: 00:00:00.0045324, Type: AngleSharp.Css.IAttributeSelectorFactory
```

* DONE - We need projected adornment types to inherit from `Adornments<TAdornments, TCallback>`
* DONE - Change where we're returning a specific adornment type to a `AdornmentsForHandler{memberIdentifier}` (Keep how we figure out all the generics)
* DONE - Generate all of the custom adornment types, plus the specific intermediate interface that all of them will derive from. Remember that the adornments must be open generics if there are type arguments.
* DONE - If there are events, change the event generation to only do one pass.
* DONE - Need to make the first type argument for the custom adornment the adornment itself.
* DONE - Need to add constructor.
* DONE - Need to put the event extensions class outside and not nest

Now how is it?

```
Slowest Generation Times
Generation Time: 00:00:00.1713458, Emit Time: 00:00:00.4152455, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0592587, Emit Time: 00:00:00.2631907, Type: AngleSharp.Svg.Dom.ISvgCircleElement
Generation Time: 00:00:00.0590370, Emit Time: 00:00:00.1064556, Type: AngleSharp.Html.Dom.IHtmlDataElement
Generation Time: 00:00:00.0588117, Emit Time: 00:00:00.1238957, Type: AngleSharp.Html.Dom.IHtmlFieldSetElement
Generation Time: 00:00:00.0563467, Emit Time: 00:00:00.0825243, Type: AngleSharp.Text.ITextSource
Generation Time: 00:00:00.0543712, Emit Time: 00:00:00.2594210, Type: AngleSharp.Svg.Dom.ISvgDescriptionElement
Generation Time: 00:00:00.0523122, Emit Time: 00:00:00.0054726, Type: AngleSharp.Html.Dom.Events.TrackEvent
Generation Time: 00:00:00.0509475, Emit Time: 00:00:00.1268762, Type: AngleSharp.Html.Dom.IHtmlImageElement
Generation Time: 00:00:00.0506331, Emit Time: 00:00:00.1455637, Type: AngleSharp.Html.Dom.IHtmlInlineFrameElement
Generation Time: 00:00:00.0504286, Emit Time: 00:00:00.0494961, Type: AngleSharp.IStyleFormatter

Slowest Emit Times
Generation Time: 00:00:00.1713458, Emit Time: 00:00:00.4152455, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0592587, Emit Time: 00:00:00.2631907, Type: AngleSharp.Svg.Dom.ISvgCircleElement
Generation Time: 00:00:00.0543712, Emit Time: 00:00:00.2594210, Type: AngleSharp.Svg.Dom.ISvgDescriptionElement
Generation Time: 00:00:00.0384693, Emit Time: 00:00:00.2422831, Type: AngleSharp.Svg.Dom.ISvgElement
Generation Time: 00:00:00.0279991, Emit Time: 00:00:00.2261296, Type: AngleSharp.Svg.Dom.ISvgTitleElement
Generation Time: 00:00:00.0145439, Emit Time: 00:00:00.2201423, Type: AngleSharp.Dom.IDocument
Generation Time: 00:00:00.0332539, Emit Time: 00:00:00.2137894, Type: AngleSharp.Svg.Dom.ISvgForeignObjectElement
Generation Time: 00:00:00.0303182, Emit Time: 00:00:00.2080073, Type: AngleSharp.Svg.Dom.ISvgSvgElement
Generation Time: 00:00:00.0149213, Emit Time: 00:00:00.2035910, Type: AngleSharp.Html.Dom.IHtmlInputElement
Generation Time: 00:00:00.0309135, Emit Time: 00:00:00.1866118, Type: AngleSharp.Html.Dom.IHtmlMediaElement

Fasted Generation Times
Generation Time: 00:00:00.0086285, Emit Time: 00:00:00.0149066, Type: AngleSharp.Common.ICancellable`1
Generation Time: 00:00:00.0086702, Emit Time: 00:00:00.0085729, Type: AngleSharp.Html.Forms.IFormSubmitter
Generation Time: 00:00:00.0088247, Emit Time: 00:00:00.0060064, Type: AngleSharp.Html.Dom.Events.ITouchList
Generation Time: 00:00:00.0088348, Emit Time: 00:00:00.0044489, Type: AngleSharp.Css.Parser.ICssSelectorParser
Generation Time: 00:00:00.0088634, Emit Time: 00:00:00.0165666, Type: AngleSharp.Browser.INavigationHandler
Generation Time: 00:00:00.0088643, Emit Time: 00:00:00.0050693, Type: AngleSharp.Common.ICancellable
Generation Time: 00:00:00.0088808, Emit Time: 00:00:00.0073057, Type: AngleSharp.Css.Dom.IMediaFeature
Generation Time: 00:00:00.0088900, Emit Time: 00:00:00.0050621, Type: AngleSharp.Html.IInputTypeFactory
Generation Time: 00:00:00.0088935, Emit Time: 00:00:00.0039523, Type: AngleSharp.Browser.Dom.INavigatorOnline
Generation Time: 00:00:00.0089103, Emit Time: 00:00:00.0059584, Type: AngleSharp.Html.Construction.IConstructableNamedNodeMap

Fasted Emit Times
Generation Time: 00:00:00.0093139, Emit Time: 00:00:00.0039019, Type: AngleSharp.Html.Dom.ILabelabelElement
Generation Time: 00:00:00.0088935, Emit Time: 00:00:00.0039523, Type: AngleSharp.Browser.Dom.INavigatorOnline
Generation Time: 00:00:00.0189157, Emit Time: 00:00:00.0040861, Type: AngleSharp.Browser.IEncodingProvider
Generation Time: 00:00:00.0088348, Emit Time: 00:00:00.0044489, Type: AngleSharp.Css.Parser.ICssSelectorParser
Generation Time: 00:00:00.0092785, Emit Time: 00:00:00.0045116, Type: AngleSharp.Css.IPseudoElementSelectorFactory
Generation Time: 00:00:00.0388917, Emit Time: 00:00:00.0045914, Type: AngleSharp.Dom.Events.IEventFactory
Generation Time: 00:00:00.0089401, Emit Time: 00:00:00.0046404, Type: AngleSharp.Html.Forms.Submitters.IHtmlEncoder
Generation Time: 00:00:00.0180809, Emit Time: 00:00:00.0046823, Type: AngleSharp.Dom.IReverseEntityProvider
Generation Time: 00:00:00.0094325, Emit Time: 00:00:00.0047093, Type: AngleSharp.Browser.ICommandProvider
Generation Time: 00:00:00.0091157, Emit Time: 00:00:00.0047508, Type: AngleSharp.Browser.IMetaHandler
Total time: 00:00:36.0116360
```

Well, that's much better, but does it actually compile? Nope, I have 12 errors, looks like they're related to open generics (maybe):

```
Error Counts
        Code: CS0305, Count: 9
        Code: CS1503, Count: 3
Total Error Count: 12

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(781,32): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Adornments.AdornmentsForHandler0<T>' requires 1 type arguments
Code:
AdornmentsForHandler0

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(781,183): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Adornments.AdornmentsForHandler0<T>' requires 1 type arguments
Code:
AdornmentsForHandler0

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(783,88): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Handler0<T>' requires 1 type arguments
Code:
Handler0

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(787,32): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Adornments.AdornmentsForHandler1<T>' requires 1 type arguments
Code:
AdornmentsForHandler1

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(787,277): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Adornments.AdornmentsForHandler1<T>' requires 1 type arguments
Code:
AdornmentsForHandler1

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(789,88): error CS0305: Using the generic type 'IBrowsingContextCreateExpectations.Handler1<T>' requires 1 type arguments
Code:
Handler1

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.Html.Parser.IHtmlParser_Rock_Create.g.cs(1124,32): error CS0305: Using the generic type 'IHtmlParserCreateExpectations.Adornments.AdornmentsForHandler14<TDocument, TElement>' requires 2 type arguments
Code:
AdornmentsForHandler14

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.Html.Parser.IHtmlParser_Rock_Create.g.cs(1124,308): error CS0305: Using the generic type 'IHtmlParserCreateExpectations.Adornments.AdornmentsForHandler14<TDocument, TElement>' requires 2 type arguments
Code:
AdornmentsForHandler14

Error:

ID: CS0305
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.Html.Parser.IHtmlParser_Rock_Create.g.cs(1126,96): error CS0305: Using the generic type 'IHtmlParserCreateExpectations.Handler14<TDocument, TElement>' requires 2 type arguments
Code:
Handler14

Error:

ID: CS1503
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(529,16): error CS1503: Argument 1: cannot convert from 'AngleSharp.IBrowsingContextCreateExpectations.Handler0<T>' to 'AngleSharp.IBrowsingContextCreateExpectations.Handler0'
Code:
handler

Error:

ID: CS1503
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.IBrowsingContext_Rock_Create.g.cs(538,16): error CS1503: Argument 1: cannot convert from 'AngleSharp.IBrowsingContextCreateExpectations.Handler1<T>' to 'AngleSharp.IBrowsingContextCreateExpectations.Handler1'
Code:
handler

Error:

ID: CS1503
Description: Rocks\Rocks.RockAttributeGenerator\AngleSharp.Html.Parser.IHtmlParser_Rock_Create.g.cs(932,16): error CS1503: Argument 1: cannot convert from 'AngleSharp.Html.Parser.IHtmlParserCreateExpectations.Handler14<TDocument, TElement>' to 'AngleSharp.Html.Parser.IHtmlParserCreateExpectations.Handler14'
Code:
@handler
```

OK, fixed all of those, now how does it perform?

```
Slowest Generation Times
Generation Time: 00:00:00.1355856, Emit Time: 00:00:00.3623837, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0739877, Emit Time: 00:00:00.3738890, Type: AngleSharp.Svg.Dom.ISvgCircleElement
Generation Time: 00:00:00.0684805, Emit Time: 00:00:00.2732399, Type: AngleSharp.Svg.Dom.ISvgTitleElement
Generation Time: 00:00:00.0549821, Emit Time: 00:00:00.1747747, Type: AngleSharp.Html.Dom.IHtmlVideoElement
Generation Time: 00:00:00.0539748, Emit Time: 00:00:00.1335722, Type: AngleSharp.Html.Dom.IHtmlAreaElement
Generation Time: 00:00:00.0517812, Emit Time: 00:00:00.1677820, Type: AngleSharp.Html.Dom.IHtmlAudioElement
Generation Time: 00:00:00.0507392, Emit Time: 00:00:00.0154742, Type: AngleSharp.Dom.IUrlUtilities
Generation Time: 00:00:00.0501347, Emit Time: 00:00:00.1467009, Type: AngleSharp.Html.Dom.IHtmlFormElement
Generation Time: 00:00:00.0499873, Emit Time: 00:00:00.1059505, Type: AngleSharp.Html.Dom.IHtmlLabelElement
Generation Time: 00:00:00.0496310, Emit Time: 00:00:00.0735324, Type: AngleSharp.Text.ITextSource

Slowest Emit Times
Generation Time: 00:00:00.0739877, Emit Time: 00:00:00.3738890, Type: AngleSharp.Svg.Dom.ISvgCircleElement
Generation Time: 00:00:00.1355856, Emit Time: 00:00:00.3623837, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0308667, Emit Time: 00:00:00.2823139, Type: AngleSharp.Svg.Dom.ISvgElement
Generation Time: 00:00:00.0684805, Emit Time: 00:00:00.2732399, Type: AngleSharp.Svg.Dom.ISvgTitleElement
Generation Time: 00:00:00.0342899, Emit Time: 00:00:00.2596722, Type: AngleSharp.Svg.Dom.ISvgDescriptionElement
Generation Time: 00:00:00.0446943, Emit Time: 00:00:00.2575087, Type: AngleSharp.Svg.Dom.ISvgForeignObjectElement
Generation Time: 00:00:00.0382526, Emit Time: 00:00:00.2249952, Type: AngleSharp.Svg.Dom.ISvgSvgElement
Generation Time: 00:00:00.0395533, Emit Time: 00:00:00.2111915, Type: AngleSharp.Html.Dom.IHtmlTableHeaderCellElement
Generation Time: 00:00:00.0134917, Emit Time: 00:00:00.2052802, Type: AngleSharp.Html.Dom.IHtmlInputElement
Generation Time: 00:00:00.0207316, Emit Time: 00:00:00.2049162, Type: AngleSharp.Dom.IDocument

Fasted Generation Times
Generation Time: 00:00:00.0085122, Emit Time: 00:00:00.0058730, Type: AngleSharp.Html.Construction.IConstructableNamedNodeMap
Generation Time: 00:00:00.0085717, Emit Time: 00:00:00.0051777, Type: AngleSharp.Common.ICancellable
Generation Time: 00:00:00.0085937, Emit Time: 00:00:00.0152106, Type: AngleSharp.Browser.IEventLoop
Generation Time: 00:00:00.0086297, Emit Time: 00:00:00.0062940, Type: AngleSharp.Html.Forms.IFormSubmitter
Generation Time: 00:00:00.0086733, Emit Time: 00:00:00.0158924, Type: AngleSharp.Browser.Dom.INavigatorId
Generation Time: 00:00:00.0087185, Emit Time: 00:00:00.0213582, Type: AngleSharp.Browser.Dom.IHistory
Generation Time: 00:00:00.0087260, Emit Time: 00:00:00.0046964, Type: AngleSharp.Browser.IMetaHandler
Generation Time: 00:00:00.0087272, Emit Time: 00:00:00.0099795, Type: AngleSharp.Browser.IParser
Generation Time: 00:00:00.0087831, Emit Time: 00:00:00.0103227, Type: AngleSharp.Dom.ITreeWalker
Generation Time: 00:00:00.0087879, Emit Time: 00:00:00.0231869, Type: AngleSharp.Browser.Dom.INavigatorStorageUtilities

Fasted Emit Times
Generation Time: 00:00:00.0090147, Emit Time: 00:00:00.0039415, Type: AngleSharp.Html.Dom.ILabelabelElement
Generation Time: 00:00:00.0089697, Emit Time: 00:00:00.0041725, Type: AngleSharp.Browser.ICommandProvider
Generation Time: 00:00:00.0113353, Emit Time: 00:00:00.0042547, Type: AngleSharp.Html.Forms.Submitters.IHtmlEncoder
Generation Time: 00:00:00.0088050, Emit Time: 00:00:00.0043371, Type: AngleSharp.Browser.Dom.INavigatorOnline
Generation Time: 00:00:00.0459355, Emit Time: 00:00:00.0043428, Type: AngleSharp.Browser.IEncodingProvider
Generation Time: 00:00:00.0327442, Emit Time: 00:00:00.0043724, Type: AngleSharp.Css.IPseudoElementSelectorFactory
Generation Time: 00:00:00.0096060, Emit Time: 00:00:00.0044729, Type: AngleSharp.Css.IPseudoClassSelectorFactory
Generation Time: 00:00:00.0093898, Emit Time: 00:00:00.0044732, Type: AngleSharp.Css.Parser.ICssSelectorParser
Generation Time: 00:00:00.0087260, Emit Time: 00:00:00.0046964, Type: AngleSharp.Browser.IMetaHandler
Generation Time: 00:00:00.0187006, Emit Time: 00:00:00.0047373, Type: AngleSharp.Css.IAttributeSelectorFactory
Total time: 00:00:34.0061344
```

Looking much better. Need to run integration tests...

Some issues with pointers:
* DONE - When generating the projected adornment
  * DONE - Have it derive from `global::Rocks.Adornments<TAdornments, HandlerFordelegatePointerOfint__void<TCallback>, TCallback>`
  * DONE - Do not need the private handler field anymore.
* DONE - When creating the custom adornment, the first type argument needs to be the custom adornment type itself. 

Good, this fix is in. Now, we need to go deeper. I included EF in this run:

```
Slowest Generation Times
Generation Time: 00:00:00.1437481, Emit Time: 00:00:00.3523858, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0642545, Emit Time: 00:00:00.0112495, Type: Microsoft.EntityFrameworkCore.Design.MethodCallCodeFragment
Generation Time: 00:00:00.0627278, Emit Time: 00:00:00.2533899, Type: AngleSharp.Svg.Dom.ISvgSvgElement
Generation Time: 00:00:00.0611927, Emit Time: 00:00:00.1267060, Type: AngleSharp.Html.Dom.IHtmlInlineFrameElement
Generation Time: 00:00:00.0596588, Emit Time: 00:00:00.0562600, Type: AngleSharp.Svg.Dom.SvgElement
Generation Time: 00:00:00.0590639, Emit Time: 00:00:00.1139853, Type: AngleSharp.Html.Dom.IHtmlMenuElement
Generation Time: 00:00:00.0578760, Emit Time: 00:00:00.0090947, Type: Microsoft.EntityFrameworkCore.ChangeTracking.Internal.EntryCurrentProviderValueComparer
Generation Time: 00:00:00.0575626, Emit Time: 00:00:00.0115759, Type: Microsoft.EntityFrameworkCore.Infrastructure.Internal.InternalServiceCollectionMap
Generation Time: 00:00:00.0556610, Emit Time: 00:00:00.2586513, Type: Microsoft.EntityFrameworkCore.Metadata.IConventionEntityType
Generation Time: 00:00:00.0555075, Emit Time: 00:00:00.0083533, Type: Microsoft.EntityFrameworkCore.Diagnostics.QueryExpressionEventData

Slowest Emit Times
Generation Time: 00:00:00.1437481, Emit Time: 00:00:00.3523858, Type: AngleSharp.Configuration
Generation Time: 00:00:00.0430725, Emit Time: 00:00:00.3133185, Type: AngleSharp.Svg.Dom.ISvgDescriptionElement
Generation Time: 00:00:00.0498920, Emit Time: 00:00:00.2852354, Type: AngleSharp.Svg.Dom.ISvgTitleElement
Generation Time: 00:00:00.0387759, Emit Time: 00:00:00.2799565, Type: AngleSharp.Svg.Dom.ISvgElement
Generation Time: 00:00:00.0278223, Emit Time: 00:00:00.2789496, Type: AngleSharp.Svg.Dom.ISvgForeignObjectElement
Generation Time: 00:00:00.0457056, Emit Time: 00:00:00.2719956, Type: AngleSharp.Svg.Dom.ISvgCircleElement
Generation Time: 00:00:00.0279008, Emit Time: 00:00:00.2654656, Type: Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType
Generation Time: 00:00:00.0556610, Emit Time: 00:00:00.2586513, Type: Microsoft.EntityFrameworkCore.Metadata.IConventionEntityType
Generation Time: 00:00:00.0627278, Emit Time: 00:00:00.2533899, Type: AngleSharp.Svg.Dom.ISvgSvgElement
Generation Time: 00:00:00.0307393, Emit Time: 00:00:00.2411238, Type: Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType

Fasted Generation Times
Generation Time: 00:00:00.0085619, Emit Time: 00:00:00.0059903, Type: AngleSharp.Html.Construction.IConstructableNamedNodeMap
Generation Time: 00:00:00.0085668, Emit Time: 00:00:00.0039917, Type: AngleSharp.Browser.Dom.INavigatorStorageUtilities
Generation Time: 00:00:00.0086362, Emit Time: 00:00:00.0044282, Type: Microsoft.EntityFrameworkCore.Metadata.IClrPropertySetter
Generation Time: 00:00:00.0086502, Emit Time: 00:00:00.0197531, Type: Microsoft.EntityFrameworkCore.Query.IPrintableExpression
Generation Time: 00:00:00.0086506, Emit Time: 00:00:00.0043116, Type: Microsoft.EntityFrameworkCore.Query.IQueryTranslationPostprocessorFactory
Generation Time: 00:00:00.0087031, Emit Time: 00:00:00.0053418, Type: Microsoft.EntityFrameworkCore.Infrastructure.IInternalServiceCollectionMap
Generation Time: 00:00:00.0087092, Emit Time: 00:00:00.0146929, Type: Microsoft.EntityFrameworkCore.Storage.IDatabaseFacadeDependenciesAccessor
Generation Time: 00:00:00.0087157, Emit Time: 00:00:00.0044853, Type: Microsoft.EntityFrameworkCore.Query.IEvaluatableExpressionFilterPlugin
Generation Time: 00:00:00.0087225, Emit Time: 00:00:00.0046950, Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.INavigationRemovedConvention
Generation Time: 00:00:00.0087233, Emit Time: 00:00:00.0151511, Type: AngleSharp.Css.Dom.IMultiSelector

Fasted Emit Times
Generation Time: 00:00:00.0087471, Emit Time: 00:00:00.0039254, Type: Microsoft.EntityFrameworkCore.Storage.IExecutionStrategyFactory
Generation Time: 00:00:00.0207964, Emit Time: 00:00:00.0039409, Type: Microsoft.EntityFrameworkCore.Diagnostics.INavigationBaseEventData
Generation Time: 00:00:00.0090168, Emit Time: 00:00:00.0039674, Type: Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IChangeTrackerFactory
Generation Time: 00:00:00.0085668, Emit Time: 00:00:00.0039917, Type: AngleSharp.Browser.Dom.INavigatorStorageUtilities
Generation Time: 00:00:00.0090445, Emit Time: 00:00:00.0040015, Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure.IConventionSetBuilder
Generation Time: 00:00:00.0089159, Emit Time: 00:00:00.0040510, Type: Microsoft.EntityFrameworkCore.Query.IQueryContextFactory
Generation Time: 00:00:00.0223922, Emit Time: 00:00:00.0040572, Type: AngleSharp.Html.Dom.ILabelabelElement
Generation Time: 00:00:00.0202904, Emit Time: 00:00:00.0040911, Type: Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure`1
Generation Time: 00:00:00.0088035, Emit Time: 00:00:00.0040973, Type: Microsoft.EntityFrameworkCore.Internal.IRegisteredServices
Generation Time: 00:00:00.0093235, Emit Time: 00:00:00.0041019, Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure.IProviderConventionSetBuilder
Total time: 00:01:36.3588776
```

I'm going to focus on `AngleSharp.Configuration` first. It only derives from one interface, `IConfiguration`, which has one property, `IEnumerable<Object> Services { get; }`. So, I doubt that's the culpirt. It only has one custom constructor, which takes one parameter. Offhand...it's kind of baffling why it takes so long for this one to get generated.

I ran the generator on it 10 times, and the results were interesting:

```
Generation Time: 00:00:00.3102103, Emit Time: 00:00:00.3918673
Generation Time: 00:00:00.0108122, Emit Time: 00:00:00.0161312
Generation Time: 00:00:00.0130127, Emit Time: 00:00:00.0157916
Generation Time: 00:00:00.0135036, Emit Time: 00:00:00.0139804
Generation Time: 00:00:00.0142126, Emit Time: 00:00:00.0140517
Generation Time: 00:00:00.0087490, Emit Time: 00:00:00.0141340
Generation Time: 00:00:00.0189066, Emit Time: 00:00:00.0143159
Generation Time: 00:00:00.0087779, Emit Time: 00:00:00.0144407
Generation Time: 00:00:00.0088000, Emit Time: 00:00:00.0143849
Generation Time: 00:00:00.0099315, Emit Time: 00:00:00.0271314
```

Seems like it's a case of the "first-time" blues. Seems like it only takes about 10 ms to generate on average. But, it's worth going through with some profiling to see if anything shows up. I think a different target type is needed, like `AngleSharp.Svg.Dom.ISvgSvgElement`:

```
Generation Time: 00:00:00.3380347, Emit Time: 00:00:00.6281806
Generation Time: 00:00:00.0180301, Emit Time: 00:00:00.2338810
Generation Time: 00:00:00.0360199, Emit Time: 00:00:00.3062508
Generation Time: 00:00:00.0171241, Emit Time: 00:00:00.2213145
Generation Time: 00:00:00.0382549, Emit Time: 00:00:00.2130229
Generation Time: 00:00:00.0414213, Emit Time: 00:00:00.2072570
Generation Time: 00:00:00.0279943, Emit Time: 00:00:00.2071172
Generation Time: 00:00:00.0461695, Emit Time: 00:00:00.2063899
Generation Time: 00:00:00.0324327, Emit Time: 00:00:00.2026871
Generation Time: 00:00:00.0565957, Emit Time: 00:00:00.2199905
```

In this case, the generation times are a bit higher, around 35 ms. The compilation time also goes up, though, if it needs to compile a fairly large type...there may not be much I can do about that.

Remaining work:

* DONE - See if profiling brings up anything in Rocks that may be worth changing
* DONE - Add tests that have event handlers with type parameters and constraints, along with members that have constraints (e.g. `TService Get<TService>()`)
* DONE - Probably want to go back and add:
``` c#
/// <remarks>
/// This type is designed to be used from code generated by Rocks,
/// and is not intended for general use. Please refrain from
/// referencing it, as its implementation may change.
/// </remarks>
```

What are some areas to try and reduce memory allocation and improve performance?

* DONE - `AttributeDataExtensions.GetDescription(this AttributeData self, Compilation compilation)` - I think I can improve this with a different strategy around `argumentParts`.
* Look for any places where lists/collections are made without an initial capacity
  * DONE - `methodAdornmentsFQNNames` in `MethodExpectationsBuilder.Build()` - actually, it can be removed as it's no longer needed (I think)
  * DONE - `adornments` and `expectationMappings` in `MockBuilder.Build()` - we can make a good guess as to how many will be needed based on the member + property count (though if I change how properties are managed, the count may be a bit tricky)
  * `propertyMappings` in `PropertyExpectationsBuilder.Build()`
  * All of the `propertyProperties` in `PropertyExpectationsBuilder`
* Look for dead code
  * `IPropertySymbolExtensions.GetNamespaces(this IPropertySymbol self)`, which would mean `GetNamespaces(this AttributeData self)` may be removed as well. Not entirely sure that these members aren't being used in other places.
* DONE - I'm wondering if my usage for immutable data structure is overkill. For the models, yes, it's worth it. But when I'm building stuff, it may be better to use lists, dictionaries, etc.
  * DONE - `IEventSymbolExtensions.GetObsoleteDiagnostics()`
  * DONE - `IMethodSymbolExtensions.GetObsoleteDiagnostics()`
  * DONE - `IPropertySymbolExtensions.GetObsoleteDiagnostics()`
  * DONE - ...and other `GetObsoleteDiagnostics()`
* In the constructor for `TypeMockModel`, I'm wondering if we can create a `List<>` for each member list of the size given, and then do `ToImmutableArray()` on those. Would that be better?


3 methods, 4 properties, 2 of which are get/set, and 2 that are get.

memberIdentifier would be 8 at the end
get the methodCount, which would be 3, and then (8 - 3) + 1 = 6