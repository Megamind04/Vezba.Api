using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vezba.Entity;
using System.Text;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.ComponentModel;
using Vezba.Api.Interfaces;
using Ninject;

namespace Vezba.Api.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeeController : ApiController
    {
        ITheRingWhoGonnaRuleThemAll<Employee> _theRingWhoGonnaRuleThemAll;

        public EmployeeController([Named("Employee")]ITheRingWhoGonnaRuleThemAll<Employee> theRingWhoGonnaRuleThemAll)
            => _theRingWhoGonnaRuleThemAll = theRingWhoGonnaRuleThemAll;

        [Route("GetEmployees")]
        [HttpGet]
        public async Task<IQueryable<Employee>> GetEmployees()
        {
            try
            {
                return await _theRingWhoGonnaRuleThemAll.GetAll() /*as IQueryable<Employee>*/;
            }
            catch(Exception)
            {
                throw new HttpResponseException(statusCode:HttpStatusCode.NotImplemented);
            }
            
        }
        
        [Route("GetEmployee/{Id:int}")]
        [HttpGet]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> GetEmployee(int Id)
        {
            try
            {
                return Ok(await _theRingWhoGonnaRuleThemAll.GetById(Id));
            }
            catch (Exception)
            {
                throw new HttpResponseException(statusCode:HttpStatusCode.NotFound);
            }
        }

        [Route("CreateEmployee")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if(await _theRingWhoGonnaRuleThemAll.Create(emp))
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


        [Route("EditEmployee/{Id:int}")]
        [HttpPut]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> EditEmployee(int Id, Employee newEmp)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            if (Id != newEmp.EmployeeID)
            {
                return BadRequest(message: "Id`s don't match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Edit(newEmp)) 
                {
                    return Ok(newEmp);
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


        [Route("DeleteEmployee/{Id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteEmployee(int Id)
        {
            if(await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
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
