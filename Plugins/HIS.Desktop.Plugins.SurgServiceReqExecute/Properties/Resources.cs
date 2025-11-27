using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace HIS.Desktop.Plugins.SurgServiceReqExecute.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
					ResourceManager resourceManager = new ResourceManager("HIS.Desktop.Plugins.SurgServiceReqExecute.Properties.Resources", typeof(Resources).Assembly);
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

		internal static Bitmap demostration
		{
			get
			{
				object obj = ResourceManager.GetObject("demostration", resourceCulture);
				return (Bitmap)obj;
			}
		}

		internal static Bitmap sound
		{
			get
			{
				object obj = ResourceManager.GetObject("sound", resourceCulture);
				return (Bitmap)obj;
			}
		}

		internal Resources()
		{
		}
	}
}
