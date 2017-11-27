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
    [RoutePrefix("Category")]
    public class CategoryController : ApiController
    {
        ITheRingWhoGonnaRuleThemAll<Category> _theRingWhoGonnaRuleThemAll;

        public CategoryController([Named("Category")]ITheRingWhoGonnaRuleThemAll<Category> theRingWhoGonnaRuleThemAll)
            => _theRingWhoGonnaRuleThemAll = theRingWhoGonnaRuleThemAll;

        [Route("GetCategories")]
        [HttpGet]
        public async Task<IQueryable<Category>> GetCategories()
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

        [Route("GetCategory/{Id:int}")]
        [HttpGet]
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int Id)
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

        [Route("CreateCategory")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateCategory(Category cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Create(cat))
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


        [Route("EditCategory/{Id:int}")]
        [HttpPut]
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> EditCategory(int Id, Category newCat)
        {
            if (await _theRingWhoGonnaRuleThemAll.GetById(Id) == null)
            {
                return NotFound();
            }
            if (Id != newCat.CategoryID)
            {
                return BadRequest(message: "Id`s don't match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (await _theRingWhoGonnaRuleThemAll.Edit(newCat))
                {
                    return Ok(newCat);
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


        [Route("DeleteCategory/{Id:int}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteCategory(int Id)
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
