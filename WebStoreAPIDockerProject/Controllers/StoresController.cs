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
using System.Runtime.CompilerServices;

namespace StoreWebAPIApplication.Controllers
{
    [ApiController]
    [Authorize]
    //[Authorize(Roles = "webstoreapi.RW,webstoreapi.Reader,Admin")]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/version={version:apiVersion}/[controller]")]
    
    public class StoresController:ControllerBase
    {
        private readonly ProductStoreDbContext dbProductContext;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public StoresController(ProductStoreDbContext dbProductContext, IProductRepository productRepository, IMapper mapper)
        {
            this.dbProductContext = dbProductContext;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,webstoreapi.Reader,Admin")]
        [Authorize(Policy = "ReadPolicy")]
        [ApiVersion(1.0)]
        public async Task<IActionResult> GetAllProductStore([FromQuery] string? filterOn, [FromQuery] string?filterQuery)
        {
            //var whiteList = new List<string>() { "7213687e-488e-4a09-8188-e3abd4523539" };
            //var appId = HttpContext.User.Claims.First(c => c.Type == "appid").Value;
            //if (!whiteList.Contains(appId))
            //{ return new ForbidResult(); }
            //else
            //{
                //Get Data From Domain Model
            var StoreDomain = await productRepository.GetAllProductStoreAsync(filterOn, filterQuery);
            var ProductsDomain = await productRepository.GetAllAsync();
            var query = from store in StoreDomain
                        join product in ProductsDomain on store.ProductId equals product.ProductId
                        select new ProductStoreDto
                        {
                            StoreName = store.StoreName,
                            ProductName = product.ProductName,
                            ProductPrice = product.ProductPrice
                        };
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
            //var ProductsDto = mapper.Map<List<ProductDto>>(ProductsDomain);
            return Ok(query.ToList());
            //}

        }
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "webstoreapi.RW,webstoreapi.Reader,Admin")]
        [Authorize(Policy = "ReadPolicy")]
        [ApiVersion(2.0)]
        public async Task<IActionResult> GetAllProductStore()
        {
            //Get Data From Domain Model
            var StoreDomain = await productRepository.GetAllProductStoreAsync();
            var ProductsDomain = await productRepository.GetAllAsync();
            var query = from store in StoreDomain
                        join product in ProductsDomain on store.ProductId equals product.ProductId
                        select new ProductStoreDto
                        {
                            StoreName = store.StoreName,
                            ProductName = product.ProductName,
                            ProductPrice = product.ProductPrice
                        };
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
            //var ProductsDto = mapper.Map<List<ProductDto>>(ProductsDomain);
            return Ok(query.ToList());

        }
    }
}
