using System.Threading.Tasks;

namespace His.UC.UCHein.Init
{
	internal interface IInitAsync
	{
		Task<object> Run();
	}
}
