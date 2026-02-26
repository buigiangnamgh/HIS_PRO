using HIS.Desktop.Plugins.Library.ElectronicBill.Data;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public delegate bool DelegateSignAndRelease(SignDelegate data, ref string errorMessage);
}
