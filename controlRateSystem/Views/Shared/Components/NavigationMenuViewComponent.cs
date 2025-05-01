using Microsoft.AspNetCore.Mvc;

using controlRateSystem.Data.Data;

namespace controlRateSystem.Views.Shared.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly RatesDbContext context;

        public NavigationMenuViewComponent(RatesDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            var categories = context.Categories.ToList();

            return View(categories);
        }
    }
}
