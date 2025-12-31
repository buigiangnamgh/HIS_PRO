using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Dsl
{
	public class BatchSignDSL
	{
		private string[] _inputPaths;

		private SignDSL _signDsl;

		private XmlDocument[] _inputXmls;

		private string[] _outputPaths;

		public void InputPaths(string[] inputPaths)
		{
			_inputPaths = inputPaths;
			_inputXmls = new XmlDocument[0];
			_signDsl = new SignDSL();
		}

		public void InputXmls(XmlDocument[] inputXmls)
		{
			_inputXmls = inputXmls;
			_inputPaths = new string[0];
			_signDsl = new SignDSL();
		}

		public BatchSignDSL Using(X509Certificate2 certificate)
		{
			_signDsl.Using(certificate);
			return this;
		}

		public BatchSignDSL UsingFormat(XmlDsigSignatureFormat format)
		{
			_signDsl.UsingFormat(format);
			return this;
		}

		public BatchSignDSL IncludingCertificateInSignature()
		{
			_signDsl.IncludingCertificateInSignature();
			return this;
		}

		public BatchSignDSL Enveloping()
		{
			_signDsl.Enveloping();
			return this;
		}

		public BatchSignDSL Enveloped()
		{
			_signDsl.Enveloped();
			return this;
		}

		public BatchSignDSL Detached()
		{
			_signDsl.Detached();
			return this;
		}

		public BatchSignDSL DoNotIncludeCertificateInSignature()
		{
			_signDsl.DoNotIncludeCertificateInSignature();
			return this;
		}

		public BatchSignDSL IncludeTimestamp(bool includeTimestamp)
		{
			_signDsl.IncludeTimestamp(includeTimestamp);
			return this;
		}

		public BatchSignDSL WithProperty(string propertyName, string propertyValue)
		{
			_signDsl.WithProperty(propertyName, propertyValue);
			return this;
		}

		public BatchSignDSL WithProperty(string propertyName, string propertyValue, string propertyNameSpace)
		{
			_signDsl.WithProperty(propertyName, propertyValue, propertyNameSpace);
			return this;
		}

		public BatchSignDSL WithPropertyBuiltFromDoc(Converter<XmlDocument, XmlElement> howToCreatePropertyNodeFromDoc)
		{
			_signDsl.WithPropertyBuiltFromDoc(howToCreatePropertyNodeFromDoc);
			return this;
		}

		public void SignToFiles(params string[] outputPaths)
		{
			_outputPaths = outputPaths;
			if (!CheckInputOutputIntegrity())
			{
				throw new InvalidParameterException("Check input and output paths!");
			}
			if (_inputPaths.Length != 0)
			{
				for (int i = 0; i < _outputPaths.Length; i++)
				{
					_signDsl.InputPath(_inputPaths[i]);
					_signDsl.SignToFile(_outputPaths[i]);
				}
			}
			else
			{
				for (int j = 0; j < _outputPaths.Length; j++)
				{
					_signDsl.InputXml(_inputXmls[j]);
					_signDsl.SignToFile(_outputPaths[j]);
				}
			}
		}

		private bool CheckInputOutputIntegrity()
		{
			object[] outputPaths = _outputPaths;
			object[] reference = outputPaths;
			object[][] array = new object[2][];
			outputPaths = _inputXmls;
			array[0] = outputPaths;
			outputPaths = _inputPaths;
			array[1] = outputPaths;
			return ArrayHelper.ArrayHasSameLengthAsAny(reference, array);
		}

		public XmlDocument[] SignAndGetXmls()
		{
			List<XmlDocument> result = new List<XmlDocument>();
			Action<object> action = delegate(object item)
			{
				if (item is string)
				{
					_signDsl.InputPath((string)item);
				}
				else
				{
					_signDsl.InputXml((XmlDocument)item);
				}
				result.Add(_signDsl.SignAndGetXml());
			};
			object[][] array = new object[2][];
			object[] inputPaths = _inputPaths;
			array[0] = inputPaths;
			inputPaths = _inputXmls;
			array[1] = inputPaths;
			ArrayHelper.DoWithFirstNotEmpty(action, array);
			return result.ToArray();
		}
	}
}
