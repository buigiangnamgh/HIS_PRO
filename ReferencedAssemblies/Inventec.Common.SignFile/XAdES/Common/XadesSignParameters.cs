using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common;

namespace Inventec.Common.SignFile.XmlProcess.Xades.Common
{
	public class XadesSignParameters
	{
		private List<XmlPropertyDescriptor> _properties = new List<XmlPropertyDescriptor>();

		private List<Converter<XmlDocument, XmlElement>> _propertyBuilders = new List<Converter<XmlDocument, XmlElement>>();

		public string InputPath { get; set; }

		public XmlDocument InputXml { get; set; }

		public string OutputPath { get; set; }

		public bool IncludeCertificateInSignature { get; set; }

		public X509Certificate2 SignatureCertificate { get; set; }

		public List<XmlPropertyDescriptor> Properties
		{
			get
			{
				return _properties;
			}
			set
			{
				_properties = value;
			}
		}

		public List<Converter<XmlDocument, XmlElement>> PropertyBuilders
		{
			get
			{
				return _propertyBuilders;
			}
			set
			{
				_propertyBuilders = value;
			}
		}
	}
}
