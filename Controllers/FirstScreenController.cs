using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExampleToken.Controllers
{
    [Route("[controller]/[action]")]
    public class FirstScreenController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public FirstScreenController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.category = await _unitOfWork.category.GetByCategory("Active", true);
            return View(mymodel);
        }
        public async Task<IActionResult> ProductPartial(Guid categoryId)
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.product = await _unitOfWork.product.GetByCategoryId(categoryId, "Active", true);
            return PartialView("_ProductPartial", mymodel);
        }
    }
}
