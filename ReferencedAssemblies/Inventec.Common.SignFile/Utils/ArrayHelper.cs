using System;

namespace Inventec.Common.SignFile.XmlProcess.Utils
{
	public class ArrayHelper
	{
		public static bool ArrayHasSameLengthAsAny(object[] reference, params object[][] toCompare)
		{
			if (reference == null)
			{
				return false;
			}
			foreach (object[] array in toCompare)
			{
				if (array != null && reference.Length == array.Length)
				{
					return true;
				}
			}
			return false;
		}

		public static void DoWithFirstNotEmpty(Action<object> action, params object[][] arrays)
		{
			object[] array = null;
			foreach (object[] array2 in arrays)
			{
				if (array2.Length != 0)
				{
					array = array2;
					break;
				}
			}
			if (array != null)
			{
				object[] array3 = array;
				foreach (object obj in array3)
				{
					action(obj);
				}
			}
		}

		public static bool ArraysAreEqual(object[] array1, object[] array2)
		{
			if (array1 == null)
			{
				return array2 == null;
			}
			if (array2 == null)
			{
				return false;
			}
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!array1[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool ArraysAreEqual(byte[] array1, byte[] array2)
		{
			if (array1 == null)
			{
				return array2 == null;
			}
			if (array2 == null)
			{
				return false;
			}
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!array1[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
