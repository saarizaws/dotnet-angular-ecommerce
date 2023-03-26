using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
	public CompanyController(IUnitOfWork unitOfWork)
        {
		    _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    //Upsert GET
    public IActionResult Upsert(int? id)
    {
        Company company = new();
		if (id == null || id == 0)
            {
            //    ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            return View(company);
        }
    }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
            if (obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company updated successfully";
            }
            return RedirectToAction("Index");
            }
            return View(obj);
        }
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList= _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
	}
	[HttpDelete]
	public IActionResult Delete(int? id)
	{
        var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }
		_unitOfWork.Company.Remove(obj);
		_unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });
	}
	#endregion
}
