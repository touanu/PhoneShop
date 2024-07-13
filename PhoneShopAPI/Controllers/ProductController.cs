﻿using Microsoft.AspNetCore.Mvc;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.UnitOfWork;

namespace PhoneShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ProductController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var listProducts = new List<Product>();
            try
            {
                listProducts = await _unitOfWork._productServices.GetProducts();
                return Ok(listProducts);
            }
            catch (Exception ex)
            {
                return Ok(listProducts);
            }
        }
    }
}