using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.DALs
{
    public class CategoryRepository
    {
        private readonly CDAC_AssignmentEntities2 db;

        public CategoryRepository()
        {
            db = new CDAC_AssignmentEntities2();
        }

        public List<FoodCategory> GetAllCategories()
        {
            return db.FoodCategories.ToList();
        }
    }
}