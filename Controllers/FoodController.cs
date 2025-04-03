using Assignment.DALs;
using Assignment.DTOs;
using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Assignment.Utils;

namespace Assignment.Controllers
{
    public class FoodController : Controller
    {
        private readonly FoodService foodService;
        private readonly CategoryService categoryService;

        public FoodController()
        {
            foodService = new FoodService();
            categoryService = new CategoryService();
        }

        public ActionResult Index()
        {
            try
            {
               
                var foodItems = foodService.GetAllFoodItems();
                return View(foodItems);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex); 
                ModelState.AddModelError("", "Error fetching food items.");
                return View(new List<FoodItemDTO>());
            }
        }

        public ActionResult AddFoodItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFoodItem(FoodItemDTO foodItemDto, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogException(new Exception("FoodItem is not stored in correct dto format"));
                return View(foodItemDto);
            }


            try
            {
                foodService.AddFoodItem(foodItemDto, ImageFile);
                TempData["Success"] = "Food item added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                ModelState.AddModelError("", "Error adding food item.");
                return View(foodItemDto);
            }
        }

        [HttpGet]
        public ActionResult UpdateFoodItem(int id)
        {
            try
            {
                var foodItem = foodService.GetFoodItemById(id);
                if (foodItem == null)
                    return HttpNotFound();

                return View(foodItem);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                ModelState.AddModelError("", "Error fetching food item.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateFoodItem(FoodItemDTO foodItemDto, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
                return View(foodItemDto);

            try
            {
                foodService.UpdateFoodItem(foodItemDto, ImageFile);
                TempData["Success"] = "Food item updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                ModelState.AddModelError("", "Error updating food item.");
                return View(foodItemDto);
            }
        }

        [HttpGet]
        public JsonResult GetFoodCategories()
        {
            try
            {
                var categories = categoryService.GetAllCategories();
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = false, message = "Error fetching categories." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ConfirmDeleteFoodItem(int id)
        {
            try
            {
                var foodItem = foodService.GetFoodItemById(id);
                if (foodItem == null)
                {
                    return Json(new { success = false, message = "Food item not found." });
                }

                foodService.DeleteFoodItem(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = false, message = "Error deleting food item." });
            }
        }

        public JsonResult GetFoodItems(int page = 1, int rows = 10)
        {
            try
            {
                var (foodItems, totalRecords, totalPages) = foodService.GetPaginatedFoodItems(page, rows);

                return Json(new
                {
                    rows = foodItems,
                    currentPage = page,
                    total = totalPages,
                    records = totalRecords
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = false, message = "Error fetching food items." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
