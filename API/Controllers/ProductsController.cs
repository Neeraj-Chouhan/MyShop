using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.DTO;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductBrands()
        {

            var productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductTypes()
        {

            var productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery]ProductSpecParam ProductParams)
        {
            var spec = new ProductswithBrandsAndTypes(ProductParams);

            var CountSpec = new ProductWithFilterCountSpecification(ProductParams);

            var totalItems = _productRepo.CountAsync(CountSpec);

            var products = await _productRepo.ListAsync(spec);

            var Data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDTO>>(products);
            return Ok( new Pagination<ProductDTO>(ProductParams.pageSize,ProductParams.pageIndex,totalItems.Result,Data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProducts(int id)
        {
            var spec = new ProductswithBrandsAndTypes(id);
            var product = await _productRepo.GetEntityBeSpec(spec);
            
            return _mapper.Map<Product,ProductDTO>(product);
        
        }
    }
}