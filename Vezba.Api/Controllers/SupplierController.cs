using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vezba.Api.Interfaces;
using Vezba.Entity;

namespace Vezba.Api.Controllers
{
    [RoutePrefix("Supplier")]
    public class SupplierController : ApiController
    {
        ITheRingWhoGonnaRuleThemAll<Supplier> _theRingWhoGonnaRuleThemAll;

        public SupplierController([Named("Supplier")]ITheRingWhoGonnaRuleThemAll<Supplier> theRingWhoGonnaRuleThemAll)
            => _theRingWhoGonnaRuleThemAll = theRingWhoGonnaRuleThemAll;

        [Route("GetSuppliers")]
        [HttpGet]
        public async Task<IQueryable<Supplier>> GetSuppliers()
        {
            try
            {
                return await _theRingWhoGonnaRuleThemAll.GetAll() /*as IQueryable<Customer>*/;
            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode: HttpStatusCode.NotImplemented);
            }

        }

        [Route("GetSupplier/{Id:int}")]
        [HttpGet]
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> GetSupplier(int Id)
        {
            try
            {
                return Ok(await _theRingWhoGonnaRuleThemAll.GetById(Id));
            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode: HttpStatusCode.NotFound);
            }
        }

        [Route("CreateSupplier")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateSupplier(Supplier sup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Create(sup))
                {
                    return Ok("Successfully Created");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode: HttpStatusCode.NotImplemented);
            }
        }


        [Route("EditSupplier/{Id:int}")]
        [HttpPut]
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> EditSupplier(int Id, Supplier newSup)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            if (Id != newSup.SupplierID)
            {
                return BadRequest(message: "Id`s don't match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Edit(newSup))
                {
                    return Ok(newSup);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode: HttpStatusCode.NotImplemented);
            }
        }


        [Route("DeleteSupplier/{Id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteSupplier(int Id)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Delete(Id))
                {
                    return Ok("Successfully Deleted");
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode: HttpStatusCode.NotImplemented);
            }
        }
    }
}
