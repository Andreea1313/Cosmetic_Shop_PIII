using Cosmetic_Shop.Services.Interfaces;
using System.Collections.Generic;

namespace Cosmetic_Shop.Services
{
    public class CategoryService : ICategoryService
    {
        public List<string> GetSubcategories(string category)
        {
            category = category.ToLower();

            return category switch
            {
                "hair" => new List<string> { "hair" },
                "skincare" => new List<string> { "skincare" },
                "makeup" => new List<string> { "makeup" },
                "bodycare" => new List<string> { "bodycare" },
                "nails" => new List<string> { "nails" },
                _ => new List<string>()
            };
        }
    }
}
