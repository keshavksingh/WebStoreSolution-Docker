using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreWebAPIApplication.CustomActionFilters;
using StoreWebAPIApplication.Data;
using StoreWebAPIApplication.DataTransferObjects;
using StoreWebAPIApplication.DomainModels;
using StoreWebAPIApplication.Repositories;

namespace StoreWebAPIApplication.Controllers
{
    
    [ApiController]
    [Authorize]
    //[Authorize(Roles = "webstoreapi.RW,webstoreapi.Reader,Admin")]
    [ApiVersion(1.0)]
    [Route("api/version={version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductStoreDbContext dbProductContext;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductsController(ProductStoreDbContext dbProductContext,IProductRepository productRepository, IMapper mapper)
        {
            this.dbProductContext = dbProductContext;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,webstoreapi.Reader,Admin")]
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetAll([FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=1000) 
        {
            //var whiteList = new List<string>() { "7213687e-488e-4a09-8188-e3abd4523539" };
            //var appId = HttpContext.User.Claims.First(c=>c.Type=="appid").Value;
            //if (!whiteList.Contains(appId))
            //{ return new ForbidResult(); }
            //else
            //{
            //Get Data From Domain Model
            var ProductsDomain = await productRepository.GetAllAsync(sortBy, isAscending, pageNumber, pageSize);
            //Convert Domain Model and Map to DTOs
            //var ProductsDto = new List<ProductDto>();
            //foreach (var product in ProductsDomain) 
            //{ 
            //    ProductsDto.Add(new ProductDto 
            //    {  ProductId = product.ProductId,
            //        ProductName=product.ProductName,
            //        ProductPrice=product.ProductPrice,
            //        DateModified = product.DateModified
            //    });
            //}
            var ProductsDto = mapper.Map<List<ProductDto>>(ProductsDomain);

            return Ok(ProductsDto);
            //}

        }
        [HttpGet]
        [Route("{ProductId:guid}")]
        //[Authorize]
        //[TypeFilter(typeof(CustomAuthorizationAttribute))]
        //[Authorize(Roles = "webstoreapi.RW,Admin")]
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetById([FromRoute]Guid ProductId) 
        {
            var ProductsDomain = await productRepository.GetByProductIdAsync(ProductId);
            //var ProductsDomain = await dbProductContext.Product.FindAsync(ProductId);
            //var Products = dbProductContext.Product.FirstOrDefault(x => x.ProductId == ProductId);
            //var Products = dbProductContext.Product.Where(x => x.ProductId == ProductId);
            if (ProductsDomain == null)
            {
                return NotFound();
            }

            //var ProductsDto = new ProductDto{
            //    ProductId = ProductsDomain.ProductId,
            //    ProductName = ProductsDomain.ProductName,
            //    ProductPrice = ProductsDomain.ProductPrice,
            //    DateModified = ProductsDomain.DateModified
            //};
            var ProductsDto = mapper.Map<ProductDto>(ProductsDomain);
            return Ok(ProductsDto);
        }
        [HttpPost]
        [ValidateModel]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,Admin")]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> Create([FromBody] AddProductDto addProductDto) 
        {

            //Map DTO to Domain Model
            var productDomainModel = mapper.Map<Product>(addProductDto);
            //var productDomainModel = new Product
            //{
            //    ProductName = addProductDto.ProductName,
            //    ProductPrice = addProductDto.ProductPrice,
            //    DateModified = DateTime.UtcNow
            //};

            //use Domain Model To Create Product
            productDomainModel = await productRepository.CreateAsync(productDomainModel);
            //await dbProductContext.Product.AddAsync(productDomainModel);
            //await dbProductContext.SaveChangesAsync();
            var ProductDto = mapper.Map<ProductDto>(productDomainModel);
            //var ProductDto = new ProductDto { 
            //ProductId= productDomainModel.ProductId,
            //ProductName= productDomainModel.ProductName,
            //ProductPrice= productDomainModel.ProductPrice,
            //DateModified = productDomainModel.DateModified
            //};
            return CreatedAtAction(nameof(GetById), new { ProductId = ProductDto.ProductId }, ProductDto);


        }
        //Update Product
        [HttpPut]
        [Route("{ProductId:guid}")]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,Admin")]
        [Authorize(Policy = "WritePolicy")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid ProductId, [FromBody] UpdateProductRequestDto updateProdReqDto)
        {

            //convert Dto to Domain Model
            var productDomainModel = mapper.Map<Product>(updateProdReqDto);
            //var productDomainModel = new Product {
            //    ProductPrice = updateProdReqDto.ProductPrice
            //};

            var existingProduct = await productRepository.UpdateAsync(ProductId, productDomainModel);
            //var productDomainModel = await dbProductContext.Product.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }
            //productDomainModel.ProductPrice = updateProdReqDto.ProductPrice;
            //await dbProductContext.SaveChangesAsync();
            var productDto = mapper.Map<ProductDto>(existingProduct);
            //var productDto = new ProductDto
            //{
            //    ProductId = existingProduct.ProductId,
            //    ProductName = existingProduct.ProductName,
            //    ProductPrice = existingProduct.ProductPrice,
            //    DateModified = existingProduct.DateModified
            //};
            return Ok(productDto);

        }
        //Delete Product
        [HttpDelete]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,Admin")]
        [Authorize(Policy = "WritePolicy")]
        [Route("{ProductId:guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid ProductId) 
        {
            var productDomainModel = await productRepository.DeleteAsync(ProductId);
            //var productDomainModel = await dbProductContext.Product.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (productDomainModel == null)
            {
                return NotFound();
            }
            //dbProductContext.Product.Remove(productDomainModel);//Does not have an Async Method For Remove.
            //await dbProductContext.SaveChangesAsync();
            //Return The Delete Product Back
            var productDto=mapper.Map<ProductDto>(productDomainModel);
            //var productDto = new ProductDto
            //{
            //    ProductId = productDomainModel.ProductId,
            //    ProductName = productDomainModel.ProductName,
            //    ProductPrice = productDomainModel.ProductPrice,
            //    DateModified = productDomainModel.DateModified
            //};

            return Ok(productDto);
            
        }
    }
}
