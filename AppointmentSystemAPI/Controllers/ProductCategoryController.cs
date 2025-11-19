using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
       private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _productCategoryService.GetList();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _productCategoryService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_ProductCategory productCategory)
        {
           _productCategoryService.Add(productCategory);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_ProductCategory productCategory)
        {
           _productCategoryService.Update(productCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _productCategoryService.GetById(id);
            if (user == null)
                return NotFound();

            await _productCategoryService.Delete(user);
            return Ok();
        }
    }
}
