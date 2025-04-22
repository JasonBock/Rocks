// Lifted from:
// https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/IsExternalInit.cs,7c39b2fb54deae81
// Polyfill is needed because this attribute was introduced for the "init" property,
// and is not available in NS 2.0

using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
internal class IsExternalInit { }