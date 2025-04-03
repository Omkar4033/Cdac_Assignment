using Assignment.DALs;
using Assignment.DTOs;
using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.Controllers
{
    public class CategoryService
    {
        private readonly CategoryRepository categoryRepository;

        public CategoryService()
        {
            categoryRepository = new CategoryRepository();
        }

        public List<FoodCategoryDTO> GetAllCategories()     
        {
            return categoryRepository.GetAllCategories().Select(category => new FoodCategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
        }
    }
}