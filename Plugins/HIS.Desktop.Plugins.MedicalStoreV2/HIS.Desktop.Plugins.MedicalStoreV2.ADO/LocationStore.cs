using System.Collections.Generic;

namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class LocationStore
	{
		public long ID { get; set; }

		public string StoreCode { get; set; }

		public string StoreName { get; set; }

		public long? ParentID { get; set; }

		public List<LocationStore> Children { get; set; }

		public LocationStore()
		{
			Children = new List<LocationStore>();
		}

		public void AddChild(LocationStore child)
		{
			if (child != null)
			{
				Children.Add(child);
			}
		}

		public bool IsLeaf()
		{
			return Children.Count == 0;
		}
	}
}
