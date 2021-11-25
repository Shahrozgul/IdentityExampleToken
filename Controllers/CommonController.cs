using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityExampleToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public CommonController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region organization
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("organization/add")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> add(organization model)
        {
            model.createdByUserName = this.User.Identity.Name;
            model.createdDate = DateTime.Now;
            model.status = "Active";
            await _unitOfWork.organization.Add(model);
            IEnumerable<cmsKeyValueTypeDefaultAttribute> cmsKeyvaluetypedefaultattribute = await _unitOfWork.cmsKeyValueTypeDefaultAttribute.GetAll();
            foreach (var item in cmsKeyvaluetypedefaultattribute)
            {
                cmsKeyValueTypeAttribute cmsKeyvaluetypeattribute = new cmsKeyValueTypeAttribute();
                cmsKeyvaluetypeattribute.organizationId = model.Id;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeId = item.cmsKeyValueTypeId;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeName = item.cmsKeyValueTypeAttributeName;
                cmsKeyvaluetypeattribute.isMandatory = item.isMandatory;
                cmsKeyvaluetypeattribute.dataTypeName = item.dataTypeName;
                cmsKeyvaluetypeattribute.createdByUserName = this.User.Identity.Name;
                cmsKeyvaluetypeattribute.createdDate = DateTime.Now;
                cmsKeyvaluetypeattribute.status = item.status;
                cmsKeyvaluetypeattribute.isSystemGenerated = true;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeSortingNumber = item.cmsKeyValueTypeAttributeSortingNumber;
                await _unitOfWork.cmsKeyValueTypeAttribute.Add(cmsKeyvaluetypeattribute);
            }
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("organization/update")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> update(organization model)
        {
            organization organization = await _unitOfWork.organization.Get(model.Id);
            if (model.organizationName != null)
                organization.organizationName = model.organizationName;
            if (model.status != null)
                organization.status = model.status;
            organization.modifiedByUserName = this.User.Identity.Name;
            organization.modifiedDate = DateTime.Now;
            _unitOfWork.organization.Update(organization);
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("organization/get")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<organization>> organization_get()
        {
            return await _unitOfWork.organization.GetAll();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("organization/getbyid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<organization> getbyid(organization model)
        {
            return await _unitOfWork.organization.Get(model.Id);
        }
        #endregion

        #region cmsKey Value Type Default Attribute
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetypedefaultattribute/add")]
        [Consumes("application/json")]
        [Authorize(Roles = "superadmin,cmsKeyvaluetypedefaultattributeadd")]
        public async Task<IActionResult> add(cmsKeyValueTypeDefaultAttribute model)
        {
            model.createdByUserName = this.User.Identity.Name;
            model.createdDate = DateTime.Now;
            model.status = "Active";
            await _unitOfWork.cmsKeyValueTypeDefaultAttribute.Add(model);
            IEnumerable<organization> organization = await _unitOfWork.organization.GetAll();
            foreach (var item in organization)
            {
                cmsKeyValueTypeAttribute cmsKeyvaluetypeattribute = new cmsKeyValueTypeAttribute();
                cmsKeyvaluetypeattribute.organizationId = item.Id;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeId = model.cmsKeyValueTypeId;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeName = model.cmsKeyValueTypeAttributeName;
                cmsKeyvaluetypeattribute.isMandatory = model.isMandatory;
                cmsKeyvaluetypeattribute.dataTypeName = model.dataTypeName;
                cmsKeyvaluetypeattribute.createdByUserName = this.User.Identity.Name;
                cmsKeyvaluetypeattribute.createdDate = DateTime.Now;
                cmsKeyvaluetypeattribute.status = item.status;
                cmsKeyvaluetypeattribute.isSystemGenerated = true;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeSortingNumber = model.cmsKeyValueTypeAttributeSortingNumber;
                await _unitOfWork.cmsKeyValueTypeAttribute.Add(cmsKeyvaluetypeattribute);
            }
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetypedefaultattribute/update")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> update(cmsKeyValueTypeDefaultAttribute model)
        {
            cmsKeyValueTypeDefaultAttribute cmsKeyvaluetypedefaultattribute = await _unitOfWork.cmsKeyValueTypeDefaultAttribute.Get(model.Id);
            if (model.cmsKeyValueTypeAttributeName != null)
                cmsKeyvaluetypedefaultattribute.cmsKeyValueTypeAttributeName = model.cmsKeyValueTypeAttributeName;
            if (model.isMandatory != null)
                cmsKeyvaluetypedefaultattribute.isMandatory = model.isMandatory;
            if (model.dataTypeName != null)
                cmsKeyvaluetypedefaultattribute.dataTypeName = model.dataTypeName;
            if (model.status != null)
                cmsKeyvaluetypedefaultattribute.status = model.status;
            if (model.cmsKeyValueTypeAttributeSortingNumber != null)
                cmsKeyvaluetypedefaultattribute.cmsKeyValueTypeAttributeSortingNumber = model.cmsKeyValueTypeAttributeSortingNumber;
            cmsKeyvaluetypedefaultattribute.modifiedByUserName = this.User.Identity.Name;
            cmsKeyvaluetypedefaultattribute.modifiedDate = DateTime.Now;
            _unitOfWork.cmsKeyValueTypeDefaultAttribute.Update(cmsKeyvaluetypedefaultattribute);
            IEnumerable<cmsKeyValueTypeAttribute> cmsKeyvaluetypeattribute = await _unitOfWork.cmsKeyValueTypeAttribute.GetcmsKeyValueTypeAttributesBycmsKeyValueTypeId(cmsKeyvaluetypedefaultattribute.cmsKeyValueTypeId);
            foreach (var item in cmsKeyvaluetypeattribute)
            {
                if (model.cmsKeyValueTypeAttributeName != null)
                    item.cmsKeyValueTypeAttributeName = model.cmsKeyValueTypeAttributeName;
                if (model.isMandatory != null)
                    item.isMandatory = model.isMandatory;
                if (model.dataTypeName != null)
                    item.dataTypeName = model.dataTypeName;
                if (model.status != null)
                    item.status = model.status;
                if (model.cmsKeyValueTypeAttributeSortingNumber != null)
                    item.cmsKeyValueTypeAttributeSortingNumber = model.cmsKeyValueTypeAttributeSortingNumber;
                item.modifiedByUserName = this.User.Identity.Name;
                item.modifiedDate = DateTime.Now;
                _unitOfWork.cmsKeyValueTypeAttribute.Update(item);
            }
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypedefaultattribute/get")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<cmsKeyValueTypeDefaultAttribute>> cmsKeyvaluetypedefaultattribute_get()
        {
            return await _unitOfWork.cmsKeyValueTypeDefaultAttribute.GetAll();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypedefaultattribute/getbyid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<cmsKeyValueTypeDefaultAttribute> getbyid(cmsKeyValueTypeDefaultAttribute model)
        {
            return await _unitOfWork.cmsKeyValueTypeDefaultAttribute.Get(model.Id);
        }
        #endregion

        #region cmsKey Value Type Attribute
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetypeattribute/add")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> add(cmsKeyValueTypeAttribute model)
        {
            model.createdByUserName = this.User.Identity.Name;
            model.createdDate = DateTime.Now;
            model.status = "Active";
            await _unitOfWork.cmsKeyValueTypeAttribute.Add(model);
            IEnumerable<organization> organization = await _unitOfWork.organization.GetAll();
            foreach (var item in organization)
            {
                cmsKeyValueTypeAttribute cmsKeyvaluetypeattribute = new cmsKeyValueTypeAttribute();
                cmsKeyvaluetypeattribute.organizationId = item.Id;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeId = model.cmsKeyValueTypeId;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeName = model.cmsKeyValueTypeAttributeName;
                cmsKeyvaluetypeattribute.isMandatory = model.isMandatory;
                cmsKeyvaluetypeattribute.dataTypeName = model.dataTypeName;
                cmsKeyvaluetypeattribute.createdByUserName = this.User.Identity.Name;
                cmsKeyvaluetypeattribute.createdDate = DateTime.Now;
                cmsKeyvaluetypeattribute.status = item.status;
                cmsKeyvaluetypeattribute.isSystemGenerated = true;
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeSortingNumber = model.cmsKeyValueTypeAttributeSortingNumber;
                await _unitOfWork.cmsKeyValueTypeAttribute.Add(cmsKeyvaluetypeattribute);
            }
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetypeattribute/update")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> update(cmsKeyValueTypeAttribute model)
        {
            cmsKeyValueTypeAttribute cmsKeyvaluetypeattribute = await _unitOfWork.cmsKeyValueTypeAttribute.Get(model.Id);
            if (model.cmsKeyValueTypeAttributeName != null)
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeName = model.cmsKeyValueTypeAttributeName;
            if (model.isMandatory != null)
                cmsKeyvaluetypeattribute.isMandatory = model.isMandatory;
            if (model.dataTypeName != null)
                cmsKeyvaluetypeattribute.dataTypeName = model.dataTypeName;
            if (model.status != null)
                cmsKeyvaluetypeattribute.status = model.status;
            if (model.cmsKeyValueTypeAttributeSortingNumber != null)
                cmsKeyvaluetypeattribute.cmsKeyValueTypeAttributeSortingNumber = model.cmsKeyValueTypeAttributeSortingNumber;
            cmsKeyvaluetypeattribute.modifiedByUserName = this.User.Identity.Name;
            cmsKeyvaluetypeattribute.modifiedDate = DateTime.Now;
            _unitOfWork.cmsKeyValueTypeAttribute.Update(cmsKeyvaluetypeattribute);
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypeattribute/get")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<cmsKeyValueTypeAttribute>> cmsKeyvaluetypeattribute_get()
        {
            return await _unitOfWork.cmsKeyValueTypeAttribute.GetAll();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypeattribute/getbyid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<cmsKeyValueTypeAttribute> getbyid(cmsKeyValueTypeAttribute model)
        {
            return await _unitOfWork.cmsKeyValueTypeAttribute.Get(model.Id);
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypeattribute/getcmsKeyvaluetypeattributesbycmsKeyvaluetypeid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<cmsKeyValueTypeAttribute>> getcmsKeyvaluetypeattributesbycmsKeyvaluetypeid(cmsKeyValueTypeAttribute model)
        {
            return await _unitOfWork.cmsKeyValueTypeAttribute.GetcmsKeyValueTypeAttributesBycmsKeyValueTypeId(model.cmsKeyValueTypeId);
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetypeattribute/getcmsKeyvaluetypeattributesbycmsKeyvaluetypeId_organisationid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<cmsKeyValueTypeAttribute>> getcmsKeyvaluetypeattributesbycmsKeyvaluetypeId_organisationid(cmsKeyValueTypeAttribute model)
        {
            return await _unitOfWork.cmsKeyValueTypeAttribute.GetcmsKeyValueTypeAttributesBycmsKeyValueTypeId_OrganisationId(model.cmsKeyValueTypeId, model.organizationId);
        }
        #endregion

        #region cmsKey Value Type
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetype/add")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> add(cmsKeyValueType model)
        {
            model.createdByUserName = this.User.Identity.Name;
            model.createdDate = DateTime.Now;
            model.status = "Active";
            await _unitOfWork.cmsKeyValueType.Add(model);
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsKeyvaluetype/update")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> update(cmsKeyValueType model)
        {
            cmsKeyValueType cmsKeyvaluetype = await _unitOfWork.cmsKeyValueType.Get(model.Id);
            if (model.cmsKeyValueTypeName != null)
                cmsKeyvaluetype.cmsKeyValueTypeName = model.cmsKeyValueTypeName;
            if (model.status != null)
                cmsKeyvaluetype.status = model.status;
            if (model.cmsKeyValueTypeSortingNumber != null)
                cmsKeyvaluetype.cmsKeyValueTypeSortingNumber = model.cmsKeyValueTypeSortingNumber;
            cmsKeyvaluetype.modifiedByUserName = this.User.Identity.Name;
            cmsKeyvaluetype.modifiedDate = DateTime.Now;
            _unitOfWork.cmsKeyValueType.Update(cmsKeyvaluetype);
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetype/get")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IEnumerable<cmsKeyValueType>> cmsKeyvaluetype_get()
        {
            return await _unitOfWork.cmsKeyValueType.GetAll();
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("cmsKeyvaluetype/getbyid")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<cmsKeyValueType> getbyid(cmsKeyValueType model)
        {
            return await _unitOfWork.cmsKeyValueType.Get(model.Id);
        }
        #endregion

        #region cmsKey Transactions
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsTransactions/add")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> add(List<cmsKeyValueTypeAttributeTransactionDetail> listCmsKeyValueTypeAttributeTransactionDetail)
        {
            cmsKeyValueTypeAttributeTransactionMaster cmskeyvaluetypeattributetransactionmaster = new cmsKeyValueTypeAttributeTransactionMaster();
            cmskeyvaluetypeattributetransactionmaster.createdByUserName = this.User.Identity.Name;
            cmskeyvaluetypeattributetransactionmaster.createdDate = DateTime.Now;
            cmskeyvaluetypeattributetransactionmaster.status = "Active";
            await _unitOfWork.cmsKeyValueTypeAttributeTransactionMaster.Add(cmskeyvaluetypeattributetransactionmaster);
            foreach (var item in listCmsKeyValueTypeAttributeTransactionDetail)
            {
                cmsKeyValueTypeAttributeTransactionDetail cmskeyvaluetypeattributetransactiondetail = new cmsKeyValueTypeAttributeTransactionDetail();
                cmskeyvaluetypeattributetransactiondetail.cmsKeyValueTypeAttributeId = item.cmsKeyValueTypeAttributeId;
                cmskeyvaluetypeattributetransactiondetail.cmsKeyValueTypeAttributeTransactionMasterId = item.cmsKeyValueTypeAttributeTransactionMasterId;
                cmskeyvaluetypeattributetransactiondetail.cmsKeyValueTypeAttributeValue = item.cmsKeyValueTypeAttributeValue;
                cmskeyvaluetypeattributetransactiondetail.createdByUserName = this.User.Identity.Name;
                cmskeyvaluetypeattributetransactiondetail.createdDate = DateTime.Now;
                cmskeyvaluetypeattributetransactiondetail.status = "Active";
                await _unitOfWork.cmsKeyValueTypeAttributeTransactionDetail.Add(cmskeyvaluetypeattributetransactiondetail);
            }
            _unitOfWork.Complete();
            return Ok();
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("cmsTransactions/getbyorganizationcmskeyvaluetype")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> getbyorganizationcmskeyvaluetype(Guid OrganisationId, Guid cmsKeyValueTypeId)
        {


            return Ok();
        }






        #endregion


        



    }
}
