using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

using controlRateSystem.Data.Models;
using controlRateSystem.Data.Data;

namespace controlRateSystem.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            RatesDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RatesDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Categories.Any())
            {
                var europeanCategory = new Category { Name = "Європейські" };
                var americanCategory = new Category { Name = "Американські" };
                var asianCategory = new Category { Name = "Азіатські" };
                var africanCategory = new Category { Name = "Африканські" };

                context.Categories.AddRange(
                    europeanCategory,
                    americanCategory,
                    asianCategory,
                    africanCategory
                );

                context.SaveChanges();
            }

            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(
                    new Currency
                    {
                        Name = "Гривня",
                        Code = "UAH",
                        PriceInDollar = 41.53M,
                        CategoryId = context.Categories.First(c => c.Name == "Європейські").Id
                    },
                    new Currency
                    {
                        Name = "Євро",
                        Code = "EUR",
                        PriceInDollar = 1.12M,
                        CategoryId = context.Categories.First(c => c.Name == "Європейські").Id
                    },
                    new Currency
                    {
                        Name = "Долар США",
                        Code = "USD",
                        PriceInDollar = 1.00M,
                        CategoryId = context.Categories.First(c => c.Name == "Американські").Id
                    },
                    new Currency
                    {
                        Name = "Британський фунт",
                        Code = "GBP",
                        PriceInDollar = 0.81M,
                        CategoryId = context.Categories.First(c => c.Name == "Європейські").Id
                    },
                    new Currency
                    {
                        Name = "Японська єна",
                        Code = "JPY",
                        PriceInDollar = 135.25M,
                        CategoryId = context.Categories.First(c => c.Name == "Азіатські").Id
                    },
                    new Currency
                    {
                        Name = "Канадський долар",
                        Code = "CAD",
                        PriceInDollar = 1.34M,
                        CategoryId = context.Categories.First(c => c.Name == "Американські").Id
                    },
                    new Currency
                    {
                        Name = "Швейцарський франк",
                        Code = "CHF",
                        PriceInDollar = 0.92M,
                        CategoryId = context.Categories.First(c => c.Name == "Європейські").Id
                    },
                    new Currency
                    {
                        Name = "Австралійський долар",
                        Code = "AUD",
                        PriceInDollar = 1.49M,
                        CategoryId = context.Categories.First(c => c.Name == "Азіатські").Id
                    }
                );

                context.SaveChanges();
            }
        }
    }
}

