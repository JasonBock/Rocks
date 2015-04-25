#pragma warning disable CS0618
#pragma warning disable CS0672
using System;
using System.IO;

public class x : FileStream
{
	public x(String path, FileMode mode) : base (path, mode)
   { }

	public override IntPtr Handle
	{
		get
		{
			return base.Handle;
		}
	}
}
#pragma warning restore CS0672
#pragma warning restore CS0618