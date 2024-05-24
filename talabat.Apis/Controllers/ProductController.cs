using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.Apis.Dtos;
using talabat.Apis.Errors;
using talabat.Apis.Helpers;
using talabat.core;
using talabat.core.Entites;
using talabat.core.Repositories;
using talabat.core.Specifications;
using talabat.Repository;

namespace talabat.Apis.Controllers
{

    public class ProductController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        ///private readonly iGenericRepository<Product> _productRepo;
        ///private readonly iGenericRepository<ProductType> _typsRypo;
        ///private readonly iGenericRepository<ProductBrand> _breandRepo;
        private readonly IMapper _mapper;

        public ProductController(///iGenericRepository<Product> productRepo,
                                 ///iGenericRepository<ProductType> TypRsypo,
                                 ///iGenericRepository<ProductBrand> BreandRepo
                                 IUnitOfWork unitOfWork,
                                 IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            ///_productRepo = productRepo;
            ///_typsRypo = TypRsypo;
            ///_breandRepo = BreandRepo;
        }


        // baseURL / API / Product (Get)
        [ChachedAtrribute(600)]
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> Getproducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var products = await _unitOfWork.repository<Product>().Getallwithspecasync(spec);
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            //OkObjectResult result = new OkObjectResult(products);
            //return result;

            var CountSpec = new ProductWithFilterationForCount(specParams);
            var Count = await _unitOfWork.repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, Count, Data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDto>> GetproductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _unitOfWork.repository<Product>().GetEntitywithspecasync(spec);
            if (product is null) return NotFound(new ApiResponse(404));
            var mapproduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(mapproduct);
        }


        [HttpGet("Brands")] //Get : /Api/Products/Brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitOfWork.repository<ProductBrand>().Getallasync();
            return Ok(Brands);
        }

        [HttpGet("Types")] //Get : /Api/Products/Types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.repository<ProductType>().Getallasync();
            return Ok(Types);
        }






    }
}
