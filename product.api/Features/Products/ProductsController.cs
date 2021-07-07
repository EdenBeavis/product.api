using AutoMapper;
using Bearded.Monads;
using Bolt.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using product.api.Features.Products.Handlers;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace product.api.Features.Products
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IMediator mediator,
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var products = await _mediator.Send(new GetProductsByNameRequest { Name = name ?? string.Empty });

                if (products.HasItem())
                    return Ok(CreateProductsResponse(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when getting all products");
            }

            return NotFound("No products found.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");

            try
            {
                var productOptional = await _mediator.Send(new GetProductByIdRequest { Id = id });

                if (productOptional)
                    return Ok(_mapper.Map<ProductDto>(productOptional.ElseNew()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with id: {id}");
            }

            return NotFound($"No product found with Id: {id}.");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            if (!productDto.Id.Equals(Guid.Empty)) return BadRequest("New products must have an empty Id.");
            if (!ModelState.IsValid) return BadRequest(GetModelError());

            try
            {
                var productOptional = await _mediator.Send(new CreateNewProductRequest { ProductDto = productDto });

                if (productOptional)
                    return CreatedAtAction(nameof(Get), new { id = productOptional.ElseNew().Id }, productOptional.ElseNew().Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating new product with name: {productDto.Name}");
            }

            return BadRequest($"Could not create product with name: {productDto.Name}");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto productDto)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");
            if (!ModelState.IsValid) return BadRequest(GetModelError());

            try
            {
                var productOptional = await _mediator.Send(new UpdateProductRequest { Id = id, ProductDto = productDto });

                if (productOptional)
                    return Ok(productOptional.ElseNew().Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with id: {id}");
            }

            return NotFound("Product does not exist.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");

            try
            {
                var completed = await _mediator.Send(new DeleteProductRequest { Id = id });

                if (completed)
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with id: {id}");
            }

            return NotFound("Product does not exist.");
        }

        private string GetModelError() =>
            ModelState.Values.Where(v => v.ValidationState.Equals(ModelValidationState.Invalid)).Select(v => v.Errors.FirstOrDefault().ErrorMessage).FirstOrDefault();

        private ProductsDto CreateProductsResponse(IEnumerable<Product> products) => new ProductsDto
        {
            Items = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products).ToList()
        };
    }
}