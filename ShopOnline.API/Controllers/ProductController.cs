﻿using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _repository.GetItems();
                var productCategories = await _repository.GetCategories();

                if (products == null || productCategories == null) 
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _repository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await _repository.GetCategory(product.CategoryId);

                    var productDto = product.ConvertToDto(productCategory);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }
    }
}
