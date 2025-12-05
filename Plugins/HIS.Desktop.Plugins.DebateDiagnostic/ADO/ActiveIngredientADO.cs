using AutoMapper;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	internal class ActiveIngredientADO : HIS_ACTIVE_INGREDIENT
	{
		public bool IsChecked { get; set; }

		public string ACTIVE_INGREDIENT_NAME__UNSIGN { get; set; }

		public ActiveIngredientADO(HIS_ACTIVE_INGREDIENT item)
		{
			Mapper.CreateMap<HIS_ACTIVE_INGREDIENT, ActiveIngredientADO>();
			Mapper.Map<HIS_ACTIVE_INGREDIENT, ActiveIngredientADO>(item, this);
			ACTIVE_INGREDIENT_NAME__UNSIGN = Convert.UnSignVNese(((HIS_ACTIVE_INGREDIENT)this).ACTIVE_INGREDIENT_NAME);
		}
	}
}
