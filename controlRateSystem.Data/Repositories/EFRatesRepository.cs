using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using controlRateSystem.Data.Models;
using controlRateSystem.Data.Data;

namespace controlRateSystem.Data.Repositories
{
	public class EFRatesRepository : IRatesRepository
	{
		private RatesDbContext context;
		public EFRatesRepository(RatesDbContext ctx)
		{
			context = ctx;
		}
		public IQueryable<Currency> Currencies => context.Currencies;
        public IQueryable<Category> Categories => context.Categories;


        public void CreateCurrency(Currency curr)
        {
            context.Add(curr);
            context.SaveChanges();
        }
        public void DeleteCurrency(Currency curr)
        {
            context.Remove(curr);
            context.SaveChanges();
        }
        public void SaveCurrency(Currency curr)
        {
            var existingCurrency = context.Currencies.FirstOrDefault(c => c.Id == curr.Id);
            if (existingCurrency != null)
            {
                existingCurrency.Name = curr.Name;
                existingCurrency.Code = curr.Code;
                existingCurrency.PriceInDollar = curr.PriceInDollar;
                existingCurrency.CategoryId = curr.CategoryId;

                context.SaveChanges();
            }
        }

        public void CreateCategory(Category cat)
        {
            context.Add(cat);
            context.SaveChanges();
        }
        public void DeleteCategory(Category cat)
        {
            context.Remove(cat);
            context.SaveChanges();
        }
        public void SaveCategory(Category cat)
        {
            var existing = context.Categories.FirstOrDefault(c => c.Id == cat.Id);
            if (existing != null)
            {
                existing.Name = cat.Name;
                context.SaveChanges();
            }
        }
    }
}
