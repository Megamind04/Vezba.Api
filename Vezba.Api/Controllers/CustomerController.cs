using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vezba.Entity;
using System.Web.Http.Results;
using System.Data;
using Vezba.Api.Interfaces;
using Ninject;

namespace Vezba.api.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        ITheRingWhoGonnaRuleThemAll<Customer> _theRingWhoGonnaRuleThemAll;
        
        public CustomerController([Named("Customer")]ITheRingWhoGonnaRuleThemAll<Customer> theRingWhoGonnaRuleThemAll)
            => _theRingWhoGonnaRuleThemAll = theRingWhoGonnaRuleThemAll;

        [Route("GetCustomers")]
        [HttpGet]
        public async Task<IQueryable<Customer>> GetCustomers()
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

        [Route("GetCustomer/{Id:int}")]
        [HttpGet]
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int Id)
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

        [Route("CreateCustomer")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateCustomer(Customer cus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Create(cus))
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


        [Route("EditCustomer/{Id:int}")]
        [HttpPut]
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> EditCustomer(int Id, Customer newCus)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            if (Id != newCus.CustomerID)
            {
                return BadRequest(message: "Id`s don't match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Edit(newCus))
                {
                    return Ok(newCus);
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


        [Route("DeleteCustomer/{Id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteCustomer(int Id)
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
