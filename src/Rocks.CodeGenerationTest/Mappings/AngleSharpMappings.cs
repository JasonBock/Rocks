using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Media;
using AngleSharp.Text;
using System.Net;

namespace Rocks.CodeGenerationTest.Mappings;

internal static class AngleSharpMappings
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new()
		{
			{
				typeof(IResourceService<>), new()
				{
					{ "TResource", "global::Rocks.CodeGenerationTest.Mappings.MappedResourceInfo" },
				}
			},
			{
				typeof(IHtmlCollection<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedElement" },
				}
			},
			{
				typeof(IElementFactory<,>), new()
				{
					{ "TDocument", "global::Rocks.CodeGenerationTest.Mappings.MappedDocument" },
					{ "TElement", "global::Rocks.CodeGenerationTest.Mappings.MappedElement" },
				}
			},
		};
}

public sealed class MappedDocument
	: IDocument
{
   public IHtmlAllCollection All => throw new NotImplementedException();

   public IHtmlCollection<IHtmlAnchorElement> Anchors => throw new NotImplementedException();

   public IImplementation Implementation => throw new NotImplementedException();

   public string DesignMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string? Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public string DocumentUri => throw new NotImplementedException();

   public string CharacterSet => throw new NotImplementedException();

   public string CompatMode => throw new NotImplementedException();

   public string Url => throw new NotImplementedException();

   public string ContentType => throw new NotImplementedException();

   public IDocumentType Doctype => throw new NotImplementedException();

   public IElement DocumentElement => throw new NotImplementedException();

   public string? LastModified => throw new NotImplementedException();

   public DocumentReadyState ReadyState => throw new NotImplementedException();

   public ILocation Location => throw new NotImplementedException();

   public IHtmlCollection<IHtmlFormElement> Forms => throw new NotImplementedException();

   public IHtmlCollection<IHtmlImageElement> Images => throw new NotImplementedException();

   public IHtmlCollection<IHtmlScriptElement> Scripts => throw new NotImplementedException();

   public IHtmlCollection<IHtmlEmbedElement> Plugins => throw new NotImplementedException();

   public IHtmlCollection<IElement> Commands => throw new NotImplementedException();

   public IHtmlCollection<IElement> Links => throw new NotImplementedException();

   public string? Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public IHtmlHeadElement? Head => throw new NotImplementedException();

   public IHtmlElement? Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string Cookie { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public string? Origin => throw new NotImplementedException();

   public string Domain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public string? Referrer => throw new NotImplementedException();

   public IElement? ActiveElement => throw new NotImplementedException();

   public IHtmlScriptElement? CurrentScript => throw new NotImplementedException();

   public IWindow? DefaultView => throw new NotImplementedException();

   public IBrowsingContext Context => throw new NotImplementedException();

   public IDocument? ImportAncestor => throw new NotImplementedException();

   public TextSource Source => throw new NotImplementedException();

   public HttpStatusCode StatusCode => throw new NotImplementedException();

   public IEntityProvider Entities => throw new NotImplementedException();

   public string BaseUri => throw new NotImplementedException();

   public Url? BaseUrl => throw new NotImplementedException();

   public string NodeName => throw new NotImplementedException();

   public INodeList ChildNodes => throw new NotImplementedException();

   public IDocument? Owner => throw new NotImplementedException();

   public IElement? ParentElement => throw new NotImplementedException();

   public INode? Parent => throw new NotImplementedException();

   public INode? FirstChild => throw new NotImplementedException();

   public INode? LastChild => throw new NotImplementedException();

   public INode? NextSibling => throw new NotImplementedException();

   public INode? PreviousSibling => throw new NotImplementedException();

   public NodeType NodeType => throw new NotImplementedException();

   public string NodeValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string TextContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public bool HasChildNodes => throw new NotImplementedException();

   public NodeFlags Flags => throw new NotImplementedException();

   public IHtmlCollection<IElement> Children => throw new NotImplementedException();

   public IElement? FirstElementChild => throw new NotImplementedException();

   public IElement? LastElementChild => throw new NotImplementedException();

   public int ChildElementCount => throw new NotImplementedException();

   public IStyleSheetList StyleSheets => throw new NotImplementedException();

   public string? SelectedStyleSheetSet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public string? LastStyleSheetSet => throw new NotImplementedException();

   public string? PreferredStyleSheetSet => throw new NotImplementedException();

   public IStringList StyleSheetSets => throw new NotImplementedException();

#pragma warning disable CS0067
	public event DomEventHandler? ReadyStateChanged;
   public event DomEventHandler? Aborted;
   public event DomEventHandler? Blurred;
   public event DomEventHandler? Cancelled;
   public event DomEventHandler? CanPlay;
   public event DomEventHandler? CanPlayThrough;
   public event DomEventHandler? Changed;
   public event DomEventHandler? Clicked;
   public event DomEventHandler? CueChanged;
   public event DomEventHandler? DoubleClick;
   public event DomEventHandler? Drag;
   public event DomEventHandler? DragEnd;
   public event DomEventHandler? DragEnter;
   public event DomEventHandler? DragExit;
   public event DomEventHandler? DragLeave;
   public event DomEventHandler? DragOver;
   public event DomEventHandler? DragStart;
   public event DomEventHandler? Dropped;
   public event DomEventHandler? DurationChanged;
   public event DomEventHandler? Emptied;
   public event DomEventHandler? Ended;
   public event DomEventHandler? Error;
   public event DomEventHandler? Focused;
   public event DomEventHandler? Input;
   public event DomEventHandler? Invalid;
   public event DomEventHandler? KeyDown;
   public event DomEventHandler? KeyPress;
   public event DomEventHandler? KeyUp;
   public event DomEventHandler? Loaded;
   public event DomEventHandler? LoadedData;
   public event DomEventHandler? LoadedMetadata;
   public event DomEventHandler? Loading;
   public event DomEventHandler? MouseDown;
   public event DomEventHandler? MouseEnter;
   public event DomEventHandler? MouseLeave;
   public event DomEventHandler? MouseMove;
   public event DomEventHandler? MouseOut;
   public event DomEventHandler? MouseOver;
   public event DomEventHandler? MouseUp;
   public event DomEventHandler? MouseWheel;
   public event DomEventHandler? Paused;
   public event DomEventHandler? Played;
   public event DomEventHandler? Playing;
   public event DomEventHandler? Progress;
   public event DomEventHandler? RateChanged;
   public event DomEventHandler? Resetted;
   public event DomEventHandler? Resized;
   public event DomEventHandler? Scrolled;
   public event DomEventHandler? Seeked;
   public event DomEventHandler? Seeking;
   public event DomEventHandler? Selected;
   public event DomEventHandler? Shown;
   public event DomEventHandler? Stalled;
   public event DomEventHandler? Submitted;
   public event DomEventHandler? Suspended;
   public event DomEventHandler? TimeUpdated;
   public event DomEventHandler? Toggled;
   public event DomEventHandler? VolumeChanged;
   public event DomEventHandler? Waiting;
#pragma warning restore CS0067

	public void AddEventListener(string type, DomEventHandler? callback = null, bool capture = false) => throw new NotImplementedException();
   public bool AddImportUrl(Uri uri) => throw new NotImplementedException();
   public INode Adopt(INode externalNode) => throw new NotImplementedException();
   public void Append(params INode[] nodes) => throw new NotImplementedException();
   public INode AppendChild(INode child) => throw new NotImplementedException();
   public INode Clone(bool deep = true) => throw new NotImplementedException();
   public void Close() => throw new NotImplementedException();
   public DocumentPositions CompareDocumentPosition(INode otherNode) => throw new NotImplementedException();
   public bool Contains(INode otherNode) => throw new NotImplementedException();
   public IAttr CreateAttribute(string name) => throw new NotImplementedException();
   public IAttr CreateAttribute(string? namespaceUri, string name) => throw new NotImplementedException();
   public IComment CreateComment(string data) => throw new NotImplementedException();
   public IDocumentFragment CreateDocumentFragment() => throw new NotImplementedException();
   public IElement CreateElement(string name) => throw new NotImplementedException();
   public IElement CreateElement(string? namespaceUri, string name) => throw new NotImplementedException();
   public Event CreateEvent(string type) => throw new NotImplementedException();
   public INodeIterator CreateNodeIterator(INode root, FilterSettings settings = FilterSettings.All, NodeFilter? filter = null) => throw new NotImplementedException();
   public IProcessingInstruction CreateProcessingInstruction(string target, string data) => throw new NotImplementedException();
   public IRange CreateRange() => throw new NotImplementedException();
   public IText CreateTextNode(string data) => throw new NotImplementedException();
   public ITreeWalker CreateTreeWalker(INode root, FilterSettings settings = FilterSettings.All, NodeFilter? filter = null) => throw new NotImplementedException();
   public bool Dispatch(Event ev) => throw new NotImplementedException();
   public void Dispose() => throw new NotImplementedException();
   public void EnableStyleSheetsForSet(string name) => throw new NotImplementedException();
   public bool Equals(INode otherNode) => throw new NotImplementedException();
   public bool ExecuteCommand(string commandId, bool showUserInterface = false, string value = "") => throw new NotImplementedException();
   public string? GetCommandValue(string commandId) => throw new NotImplementedException();
   public IElement? GetElementById(string elementId) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByClassName(string classNames) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByName(string name) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByTagName(string tagName) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByTagName(string? namespaceUri, string tagName) => throw new NotImplementedException();
   public bool HasFocus() => throw new NotImplementedException();
   public bool HasImported(Uri uri) => throw new NotImplementedException();
   public INode Import(INode externalNode, bool deep = true) => throw new NotImplementedException();
   public INode InsertBefore(INode newElement, INode? referenceElement) => throw new NotImplementedException();
   public void InvokeEventListener(Event ev) => throw new NotImplementedException();
   public bool IsCommandEnabled(string commandId) => throw new NotImplementedException();
   public bool IsCommandExecuted(string commandId) => throw new NotImplementedException();
   public bool IsCommandIndeterminate(string commandId) => throw new NotImplementedException();
   public bool IsCommandSupported(string commandId) => throw new NotImplementedException();
   public bool IsDefaultNamespace(string namespaceUri) => throw new NotImplementedException();
   public void Load(string url) => throw new NotImplementedException();
   public string? LookupNamespaceUri(string prefix) => throw new NotImplementedException();
   public string? LookupPrefix(string? namespaceUri) => throw new NotImplementedException();
   public void Normalize() => throw new NotImplementedException();
   public IDocument Open(string type = "text/html", string? replace = null) => throw new NotImplementedException();
   public void Prepend(params INode[] nodes) => throw new NotImplementedException();
   public IElement? QuerySelector(string selectors) => throw new NotImplementedException();
   public IHtmlCollection<IElement> QuerySelectorAll(string selectors) => throw new NotImplementedException();
   public INode RemoveChild(INode child) => throw new NotImplementedException();
   public void RemoveEventListener(string type, DomEventHandler? callback = null, bool capture = false) => throw new NotImplementedException();
   public INode ReplaceChild(INode newChild, INode oldChild) => throw new NotImplementedException();
   public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => throw new NotImplementedException();
   public void Write(string content) => throw new NotImplementedException();
   public void WriteLine(string content) => throw new NotImplementedException();
}

public sealed class MappedElement
	: IElement
{
   public string? Prefix => throw new NotImplementedException();

   public string LocalName => throw new NotImplementedException();

   public string? NamespaceUri => throw new NotImplementedException();

   public INamedNodeMap Attributes => throw new NotImplementedException();

   public ITokenList ClassList => throw new NotImplementedException();

   public string? ClassName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string? Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string InnerHtml { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string OuterHtml { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public string TagName => throw new NotImplementedException();

   public IElement? AssignedSlot => throw new NotImplementedException();

   public string? Slot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public IShadowRoot? ShadowRoot => throw new NotImplementedException();

   public bool IsFocused => throw new NotImplementedException();

   public ISourceReference? SourceReference => throw new NotImplementedException();

   public string BaseUri => throw new NotImplementedException();

   public Url? BaseUrl => throw new NotImplementedException();

   public string NodeName => throw new NotImplementedException();

   public INodeList ChildNodes => throw new NotImplementedException();

   public IDocument? Owner => throw new NotImplementedException();

   public IElement? ParentElement => throw new NotImplementedException();

   public INode? Parent => throw new NotImplementedException();

   public INode? FirstChild => throw new NotImplementedException();

   public INode? LastChild => throw new NotImplementedException();

   public INode? NextSibling => throw new NotImplementedException();

   public INode? PreviousSibling => throw new NotImplementedException();

   public NodeType NodeType => throw new NotImplementedException();

   public string NodeValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public string TextContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   public bool HasChildNodes => throw new NotImplementedException();

   public NodeFlags Flags => throw new NotImplementedException();

   public IHtmlCollection<IElement> Children => throw new NotImplementedException();

   public IElement? FirstElementChild => throw new NotImplementedException();

   public IElement? LastElementChild => throw new NotImplementedException();

   public int ChildElementCount => throw new NotImplementedException();

   public IElement? NextElementSibling => throw new NotImplementedException();

   public IElement? PreviousElementSibling => throw new NotImplementedException();

   public void AddEventListener(string type, DomEventHandler? callback = null, bool capture = false) => throw new NotImplementedException();
   public void After(params INode[] nodes) => throw new NotImplementedException();
   public void Append(params INode[] nodes) => throw new NotImplementedException();
   public INode AppendChild(INode child) => throw new NotImplementedException();
   public IShadowRoot AttachShadow(ShadowRootMode mode = ShadowRootMode.Open) => throw new NotImplementedException();
   public void Before(params INode[] nodes) => throw new NotImplementedException();
   public INode Clone(bool deep = true) => throw new NotImplementedException();
   public IElement? Closest(string selectors) => throw new NotImplementedException();
   public DocumentPositions CompareDocumentPosition(INode otherNode) => throw new NotImplementedException();
   public bool Contains(INode otherNode) => throw new NotImplementedException();
   public bool Dispatch(Event ev) => throw new NotImplementedException();
   public bool Equals(INode otherNode) => throw new NotImplementedException();
   public string? GetAttribute(string name) => throw new NotImplementedException();
   public string? GetAttribute(string? namespaceUri, string localName) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByClassName(string classNames) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByTagName(string tagName) => throw new NotImplementedException();
   public IHtmlCollection<IElement> GetElementsByTagNameNS(string? namespaceUri, string tagName) => throw new NotImplementedException();
   public bool HasAttribute(string name) => throw new NotImplementedException();
   public bool HasAttribute(string? namespaceUri, string localName) => throw new NotImplementedException();
   public void Insert(AdjacentPosition position, string html) => throw new NotImplementedException();
   public INode InsertBefore(INode newElement, INode? referenceElement) => throw new NotImplementedException();
   public void InvokeEventListener(Event ev) => throw new NotImplementedException();
   public bool IsDefaultNamespace(string namespaceUri) => throw new NotImplementedException();
   public string? LookupNamespaceUri(string prefix) => throw new NotImplementedException();
   public string? LookupPrefix(string? namespaceUri) => throw new NotImplementedException();
   public bool Matches(string selectors) => throw new NotImplementedException();
   public void Normalize() => throw new NotImplementedException();
   public void Prepend(params INode[] nodes) => throw new NotImplementedException();
   public IElement? QuerySelector(string selectors) => throw new NotImplementedException();
   public IHtmlCollection<IElement> QuerySelectorAll(string selectors) => throw new NotImplementedException();
   public void Remove() => throw new NotImplementedException();
   public bool RemoveAttribute(string name) => throw new NotImplementedException();
   public bool RemoveAttribute(string? namespaceUri, string localName) => throw new NotImplementedException();
   public INode RemoveChild(INode child) => throw new NotImplementedException();
   public void RemoveEventListener(string type, DomEventHandler? callback = null, bool capture = false) => throw new NotImplementedException();
   public void Replace(params INode[] nodes) => throw new NotImplementedException();
   public INode ReplaceChild(INode newChild, INode oldChild) => throw new NotImplementedException();
   public void SetAttribute(string name, string value) => throw new NotImplementedException();
   public void SetAttribute(string? namespaceUri, string name, string value) => throw new NotImplementedException();
   public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => throw new NotImplementedException();
}

public sealed class MappedResourceInfo
	: IResourceInfo
{
   public Url Source { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}