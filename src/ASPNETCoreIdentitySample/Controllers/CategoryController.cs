using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCoreIdentitySample.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreIdentitySample.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        public IActionResult Index()
        {
            var model = _categoryServices.Queryable();
            return View();
        }
    }
}