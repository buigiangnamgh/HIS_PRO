using HIS.Desktop.Plugins.Library.ElectronicBill.Template;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public interface IRun
	{
		ElectronicBillResult Run(ElectronicBillType.ENUM electronicBillType, TemplateEnum.TYPE _templateType);
	}
}
