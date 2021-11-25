using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExampleToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstScreenApiController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public FirstScreenApiController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("pos/submitbill")]
        [Consumes("application/json")]
        public async Task<IActionResult> SubmitBill(List<billDetail> listBillDetails)
        {
            if (listBillDetails != null && listBillDetails.Count != 0)
            {
                var billHeader = new billHeader();
                billHeader.billNumber = _unitOfWork.billHeader.GetCurrentToken() + 1;
                billHeader.areaAllocated = "Pending";
                billHeader.createdByUserName = "admin@gmail.com";
                billHeader.modifiedByUserName = "admin@gmail.com";
                billHeader.status = "Pending";
                billHeader.billDetails = listBillDetails;
                await _unitOfWork.billHeader.Add(billHeader);
                _unitOfWork.Complete();
                var list = await _unitOfWork.billHeader.GetByBillHeaderId(billHeader.Id);
                dynamic mymodel = new ExpandoObject();
                mymodel.billHeader = list;
                return Ok(mymodel);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("pos/GetBill")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetBill(int billNumber)
        {
            if (billNumber == 0)
                billNumber = _unitOfWork.billHeader.GetCurrentToken();
            var list = await _unitOfWork.billHeader.GetByBillNumber(billNumber);
            dynamic mymodel = new ExpandoObject();
            mymodel.billHeader = list;
            return Ok(mymodel);
        }
    }
}
