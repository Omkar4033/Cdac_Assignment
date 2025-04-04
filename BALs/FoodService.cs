using Assignment.DTOs;
using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Assignment.DALs
{
    public class FoodService
    {
        private readonly FoodRepository foodRepository;

        public FoodService()
        {
            foodRepository = new FoodRepository();
        }

        public List<FoodItemDTO> GetAllFoodItems()
        {
            try
            {
                return foodRepository.GetAllFoodItems()
                    .Select(food => new FoodItemDTO
                    {
                        Id = food.Id,
                        Name = food.Name,
                        Price = food.Price,
                        Description = food.Description ?? " ",
                        ImageUrl = food.ImagePath,
                        CategoryId = food.CategoryId ?? 0,
                        CategoryName = food.FoodCategory != null ? food.FoodCategory.Name : "Unknown",
                        MobileNo = food.MobileNo,
                        Email = food.Email,
                        IsVegetarian = food.IsVegetarian,
                        Calories = food.Calories ?? 0,
                        Rating = food.Ratings ?? 1,
                        CreatedAt = food.CreatedAt ?? DateTime.Now
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all food items.", ex);
            }
        }

        public FoodItemDTO GetFoodItemById(int id)
        {
            try
            {
                var food = foodRepository.GetFoodById(id);
                if (food == null)
                    throw new KeyNotFoundException("Food item not found.");

                return new FoodItemDTO
                {
                    Id = food.Id,
                    Name = food.Name,
                    Price = food.Price,
                    Description = food.Description,
                    ImageUrl = food.ImagePath,
                    CategoryId = food.CategoryId ?? 0,
                    CategoryName = food.FoodCategory != null ? food.FoodCategory.Name : "Unknown",
                    MobileNo = food.MobileNo,
                    Email = food.Email,
                    IsVegetarian = food.IsVegetarian,
                    Calories = food.Calories ?? 0,
                    Rating = food.Ratings ?? 0,
                    CreatedAt = food.CreatedAt ?? DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching food item by ID.", ex);
            }
        }

        public void AddFoodItem(FoodItemDTO foodItemDto, HttpPostedFileBase ImageFile)
        {
            try
            {
                var foodItem = new FoodItem
                {
                    Name = foodItemDto.Name,
                    Price = foodItemDto.Price,
                    Description = foodItemDto.Description ?? " ",
                    CategoryId = foodItemDto.CategoryId,
                    MobileNo = foodItemDto.MobileNo,
                    Email = foodItemDto.Email,
                    Calories = foodItemDto.Calories ?? 0,
                    Ratings = foodItemDto.Rating ?? 1,
                    IsVegetarian = foodItemDto.IsVegetarian
                };

                if (ImageFile != null)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine("C:\\Users\\91883\\Downloads\\CDAC WORK\\FoodImages", fileName);


                    ImageFile.SaveAs(filePath);
                    foodItem.ImagePath = "http://localhost/FoodImages/" + fileName;
                }

                foodRepository.AddFoodItem(foodItem);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding food item.", ex);
            }
        }

        public void UpdateFoodItem(FoodItemDTO foodItemDto, HttpPostedFileBase ImageFile)
        {
            try
            {
                var foodItem = foodRepository.GetFoodById(foodItemDto.Id);
                if (foodItem == null)
                    throw new KeyNotFoundException("Food item not found.");

                foodItem.Name = foodItemDto.Name;
                foodItem.Price = foodItemDto.Price;
                foodItem.Description = foodItemDto.Description;
                foodItem.CategoryId = foodItemDto.CategoryId;
                foodItem.MobileNo = foodItemDto.MobileNo;
                foodItem.Email = foodItemDto.Email;
                foodItem.IsVegetarian = foodItemDto.IsVegetarian;
                foodItem.Calories = foodItemDto.Calories;
                foodItem.Ratings = foodItemDto.Rating;

                string newFile = Path.GetFileName(ImageFile.FileName);
                string prevFile = foodItem.ImagePath;
               
                string uploadPath = System.Configuration.ConfigurationManager.AppSettings["ImageFolder"].ToString();
                if (prevFile != null)
                {




                    if (uploadPath != null && prevFile != null)
                    {

                        string[] parts = prevFile.Split('/');
                        prevFile = parts[parts.Length - 1];
                        string filePath = Path.Combine(uploadPath, prevFile);


                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }

                }

                string newFilePath = Path.Combine(uploadPath, newFile);

                ImageFile.SaveAs(newFilePath);
                foodItem.ImagePath = "http://localhost/FoodImages/" + newFile;






                foodRepository.UpdateFoodItem(foodItem);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating food item.", ex);
            }
        }



        public void DeleteFoodItem(int id)
        {
            try
            {
                var foodItem = foodRepository.GetFoodById(id);
                if (foodItem == null)
                    throw new KeyNotFoundException("Food item not found.");


                string uploadPath = "C:\\Users\\91883\\Downloads\\CDAC WORK\\FoodImages";
                string fileName = Path.GetFileName(foodItem.ImagePath);

                if (uploadPath != null && fileName != null)
                {


                    string filePath = Path.Combine(uploadPath, fileName);


                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }




                foodRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting food item.", ex);
            }
        }




        public (List<FoodItemDTO> foodItems, int totalRecords, int totalPages) GetPaginatedFoodItems(int currentPage, int pageSize)
        {
            try
            {
                var query = foodRepository.GetFoodItemsQuery();
                int totalRecords = query.Count();
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var foodItems = query
                    .OrderByDescending(f => f.CreatedAt)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(food => new FoodItemDTO
                    {
                        Id = food.Id,
                        Name = food.Name,
                        Price = food.Price,
                        Description = food.Description,
                        ImageUrl = food.ImagePath,
                        CategoryId = food.CategoryId ?? 0,
                        CategoryName = food.FoodCategory != null ? food.FoodCategory.Name : "Unknown",
                        MobileNo = food.MobileNo,
                        Email = food.Email,
                        IsVegetarian = food.IsVegetarian,
                        Calories = food.Calories ?? 0,
                        Rating = food.Ratings ?? 0,
                        CreatedAt = food.CreatedAt ?? DateTime.Now
                    }).ToList();

                return (foodItems, totalRecords, totalPages);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching paginated food items.", ex);
            }
        }
    }
}
