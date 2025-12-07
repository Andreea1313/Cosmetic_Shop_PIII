using System.Collections.Generic;

namespace Cosmetic_Shop.Services.Interfaces
{
    public interface ICategoryService
    {
        List<string> GetSubcategories(string mainCategory);
    }
}
