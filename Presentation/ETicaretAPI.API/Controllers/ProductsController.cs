using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        public ProductsController(IProductService productService, IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productService = productService;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async Task<IActionResult>  ProductsGet()
        {
            var products = _productReadRepository.GetAll(false);
            return  Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductGet(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id,false);
            return Ok(product);
        }
        [HttpPost]
          public async Task<IActionResult> ProductsPost(VM_Create_Product model) 
        {
          await  _productWriteRepository.AddAsync(new (){
                Name = model.Name,
                Price=model.Price,
                Stock=model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }




        [HttpPut]
        public async Task<IActionResult> ProductsPut(VM_Update_Product model) //parametre entity ile karşılanmaz test amaçlı, ilerde döneceğim
        {
            Product product= await _productReadRepository.GetByIdAsync(model.Id);
            product.Name=model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductsDelete(string id)
        {

            Product product = await _productReadRepository.GetByIdAsync(id);
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            if (product == null)
            {
                return Ok("ilgili id bulunamadı çoktan silinmiş olabilir");
            }

            return StatusCode((int)HttpStatusCode.Gone);
        }
    }
}
