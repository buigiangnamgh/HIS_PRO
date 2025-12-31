using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Inventec.Common.SignFile.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("Inventec.Common.SignFile.Properties.Resources", typeof(Resources).Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string XAdES_1_3_2
		{
			get
			{
				return ResourceManager.GetString("XAdES_1_3_2", resourceCulture);
			}
		}

		internal static string xmldsig_core_schema
		{
			get
			{
				return ResourceManager.GetString("xmldsig_core_schema", resourceCulture);
			}
		}

		internal static string XMLSchema
		{
			get
			{
				return ResourceManager.GetString("XMLSchema", resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
