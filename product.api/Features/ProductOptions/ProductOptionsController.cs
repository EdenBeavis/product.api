using AutoMapper;
using Bearded.Monads;
using Bolt.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using product.api.Features.ProductOptions.Handlers;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.ProductOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace product.api.Features.ProductOptions
{
    [Route("api/products")]
    public class ProductOptionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductOptionsController> _logger;

        public ProductOptionsController(
            IMediator mediator,
            IMapper mapper,
            ILogger<ProductOptionsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptions(Guid productId)
        {
            if (productId.Equals(Guid.Empty)) return BadRequest("Invalid product Id.");

            try
            {
                var productOptions = await _mediator.Send(new GetProductOptionsByProductIdRequest { ProductId = productId });

                if (productOptions.HasItem())
                    return Ok(CreateProductOptionsResponse(productOptions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product options with product id: {productId}");
            }

            return NotFound($"No product options found for product Id: {productId}");
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");

            try
            {
                var productOption = await _mediator.Send(new GetProductOptionByIdRequest { Id = id });

                if (productOption)
                    return Ok(_mapper.Map<ProductOptionDto>(productOption.ElseNew()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product option with id: {id}");
            }

            return NotFound($"No product option found with Id: {id}.");
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOption(Guid productId, [FromBody] ProductOptionDto optionDto)
        {
            if (!optionDto.Id.Equals(Guid.Empty)) return BadRequest("New product options must have an empty Id.");
            if (productId.Equals(Guid.Empty)) return BadRequest("Product id must have a value.");
            if (!ModelState.IsValid) return BadRequest(GetModelError());

            try
            {
                var productOption = await _mediator.Send(new CreateNewProductOptionRequest { ProductId = productId, ProductOptionDto = optionDto });

                if (productOption)
                    return CreatedAtAction(nameof(GetOption),
                        new { productId = productId, id = productOption.ElseNew().Id }, productOption.ElseNew().Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating new product option for productId: {productId}");
            }

            return BadRequest($"Could not create product option with product id: {productId}");
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<IActionResult> UpdateOption(Guid id, [FromBody] ProductOptionDto optionDto)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");
            if (!ModelState.IsValid) return BadRequest(GetModelError());

            try
            {
                var productOption = await _mediator.Send(new UpdateProductOptionRequest { Id = id, ProductOptionDto = optionDto });

                if (productOption)
                    return Ok(productOption.ElseNew().Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product option with id: {id}");
            }

            return NotFound("Product option does not exist.");
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<IActionResult> DeleteOption(Guid id)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("Invalid Id.");

            try
            {
                var completed = await _mediator.Send(new DeleteProductOptionRequest { Id = id });

                if (completed)
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product option with id: {id}");
            }

            return NotFound("Product option does not exist.");
        }

        private string GetModelError() =>
            ModelState.Values.Where(v => v.ValidationState.Equals(ModelValidationState.Invalid)).Select(v => v.Errors.FirstOrDefault().ErrorMessage).FirstOrDefault();

        private ProductOptionsDto CreateProductOptionsResponse(IEnumerable<ProductOption> productOptions) => new ProductOptionsDto
        {
            Items = _mapper.Map<IEnumerable<ProductOption>, IEnumerable<ProductOptionDto>>(productOptions).ToList()
        };
    }
}