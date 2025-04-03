using Assignment.DTOs;
using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Assignment.DALs
{
    public class FoodRepository
    {
        private readonly CDAC_AssignmentEntities2 db;

        public FoodRepository()
        {
            db = new CDAC_AssignmentEntities2();
        }

        public List<FoodItem> GetAllFoodItems()
        {
            var foodItmes= db.FoodItems.ToList();
            return foodItmes;
        }

        public FoodItem GetFoodById(int id)
        {
            return db.FoodItems.Find(id);
        }

        
        public void AddFoodItem(FoodItem foodItem)
        {
            db.FoodItems.Add(foodItem);
            db.SaveChanges();
        }


        public void UpdateFoodItem(FoodItem food)
        {
            var existingFood = db.FoodItems.Find(food.Id);
            if (existingFood != null)
            {
                existingFood.Name = food.Name;
                existingFood.Price = food.Price;
                existingFood.Description = food.Description;
                existingFood.ImagePath = food.ImagePath;
                existingFood.CategoryId = food.CategoryId;
                existingFood.MobileNo = food.MobileNo;
                existingFood.Email = food.Email;
                existingFood.IsVegetarian = food.IsVegetarian;
                existingFood.Calories = food.Calories;
                existingFood.Ratings = food.Ratings;

                db.Entry(existingFood).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            try
            {
                var foodItem = db.FoodItems.FirstOrDefault(f => f.Id == id);
                if (foodItem != null)
                {
                    db.FoodItems.Remove(foodItem);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("Food item not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting food item: " + ex.Message);
            }
        }

        public IQueryable<FoodItem> GetFoodItemsQuery()
        {
            return db.FoodItems.Include(food => food.FoodCategory);
        }

    }
}