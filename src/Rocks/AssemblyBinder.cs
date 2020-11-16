using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rocks
{
	internal sealed class AssemblyBinder
		: SerializationBinder
	{
		internal AssemblyBinder() => this.Assemblies = new HashSet<Assembly>();

		public override Type BindToType(string assemblyName, string typeName) =>
			(from assembly in this.Assemblies
			 where assembly.FullName == assemblyName
			 let type = assembly.GetType(typeName)
			 where type != null
			 select type).FirstOrDefault();

		internal HashSet<Assembly> Assemblies { get; set; }
	}
}