using System.Collections.Generic;
using System.Linq;

namespace CodeSharp.Parser.Tests
{
	class TypeScriptLoader
	{
		
	}

	class ModuleDefinition : BaseDefinition
	{
		public string Id { get; set; }

		public List<BaseDefinition> Definitions { get; set; } = new List<BaseDefinition> ();
	}

	class InterfaceDefinition : BaseDefinition
	{
		List<BaseDefinition> Definitions { get; set; } = new List<BaseDefinition> ();
		public List<TypeDefinition> Properties => Definitions.OfType<TypeDefinition> ().ToList ();
		public List<MethodDefinition> Methods => Definitions.OfType<MethodDefinition> ().ToList ();
	}

	abstract class BaseDefinition
	{
		public string Comment { get; set; }
	}

	class EnumFieldDefinition : BaseDefinition
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	class MethodDefinition : BaseDefinition
	{
		public string Name { get; set; }

		public bool IsNulleable { get; set; }
		public bool IsReadOnly { get; set; }
	}

	class TypeDefinition : BaseDefinition
	{
		public string Name { get; set; }
		public string FieldType { get; set; }
		public bool IsNulleable { get; set; }
		public bool IsReadOnly { get; set; }
	}

	class PropertyTypeDefinition : TypeDefinition
	{
		public string Value { get; set; } 
	}

}
