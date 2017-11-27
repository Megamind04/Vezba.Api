using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Vezba.Api.Interfaces;
using Vezba.Entity;

namespace Vezba.Api.Controllers
{
    [RoutePrefix("Shipper")]
    public class ShipperController : ApiController
    {
        ITheRingWhoGonnaRuleThemAll<Shipper> _theRingWhoGonnaRuleThemAll;

        public ShipperController([Named("Shipper")]ITheRingWhoGonnaRuleThemAll<Shipper> theRingWhoGonnaRuleThemAll)
            => _theRingWhoGonnaRuleThemAll = theRingWhoGonnaRuleThemAll;

        [Route("GetShippers")]
        [HttpGet]
        public async Task<IQueryable<Shipper>> GetShippers()
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

        [Route("GetShipper/{Id:int}")]
        [HttpGet]
        [ResponseType(typeof(Shipper))]
        public async Task<IHttpActionResult> GetShipper(int Id)
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

        [Route("CreateShipper")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateShipper(Shipper shi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Create(shi))
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


        [Route("EditShipper/{Id:int}")]
        [HttpPut]
        [ResponseType(typeof(Shipper))]
        public async Task<IHttpActionResult> EditShipper(int Id, Shipper newShi)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            if (Id != newShi.ShipperID)
            {
                return BadRequest(message: "Id`s don't match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Edit(newShi))
                {
                    return Ok(newShi);
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


        [Route("DeleteShipper/{Id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteShipper(int Id)
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
