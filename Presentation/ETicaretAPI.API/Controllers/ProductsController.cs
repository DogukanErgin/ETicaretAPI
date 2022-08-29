using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Services;
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

        //  private readonly IProductService _productService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IStorageService _storageService;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileService fileService, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IStorageService storageService) //IProductService productService,
        {
            //  _productService = productService;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductsGet([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(pagination.Size * pagination.Page).Take(pagination.Size).ToList();
            return Ok(new
            {
                products,
                totalCount
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductGet(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id, false);
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> ProductsPost(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }




        [HttpPut]
        public async Task<IActionResult> ProductsPut(VM_Update_Product model) //parametre entity ile karşılanmaz test amaçlı, ilerde döneceğim
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductsDelete(string id)
        {

            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();

            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {

            var datas = await _storageService.UploadAsync("resource/files", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.path,
                Storage = _storageService.StorageName,
                Products=new List<Product>() { product}

            })
                .ToList());

            // var datas= await _fileService.UploadAsync("resource/product-images",Request.Form.Files);

            //await  _productImageFileWriteRepository.AddRangeAsync(datas.Select(d=> new ProductImageFile() { FileName=d.fileName,Path=d.path}).ToList());
            //  await _productImageFileWriteRepository.SaveAsync();


            return Ok();
        }
    }
}
