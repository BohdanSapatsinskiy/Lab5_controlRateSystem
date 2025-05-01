using controlRateSystem.Data.Models;

namespace controlRateSystem.Models.ViewModels
{
	public class RatesListViewModel
	{
		public IEnumerable<Currency> Currencies { get; set; }
		public PagingInfo PagingInfo { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
