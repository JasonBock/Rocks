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
		internal AssemblyBinder()
		{
			this.Assemblies = new List<Assembly>();
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			return (from assembly in this.Assemblies
					  let type = assembly.GetType(typeName)
					  where type != null
					  select type).FirstOrDefault();
		}

		internal List<Assembly> Assemblies { get; set; }
	}
}
