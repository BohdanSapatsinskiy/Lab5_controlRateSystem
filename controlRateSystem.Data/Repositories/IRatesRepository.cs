using controlRateSystem.Data.Models;

namespace controlRateSystem.Data.Repositories
{
	public interface IRatesRepository
	{
		IQueryable<Currency> Currencies { get; }
        IQueryable<Category> Categories { get; }

        void CreateCurrency(Currency currency);
        void SaveCurrency(Currency currency);
        void DeleteCurrency(Currency currency);


        void CreateCategory(Category category);
        void SaveCategory(Category category);
        void DeleteCategory(Category category);
    }
}
