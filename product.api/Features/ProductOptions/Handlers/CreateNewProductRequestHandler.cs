using AutoMapper;
using Bearded.Monads;
using MediatR;
using product.api.Features.Products.Handlers;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.ProductOptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.ProductOptions.Handlers
{
    public class CreateNewProductOptionRequest : IRequest<Option<ProductOption>>
    {
        public Guid ProductId { get; set; }
        public ProductOptionDto ProductOptionDto { get; set; }
    }

    public class CreateNewProductOptionRequestHandler : IRequestHandler<CreateNewProductOptionRequest, Option<ProductOption>>
    {
        private readonly IMediator _mediator;
        private readonly ProductsDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateNewProductOptionRequestHandler(
            ProductsDbContext dbContext,
            IMapper mapper,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Option<ProductOption>> Handle(CreateNewProductOptionRequest request, CancellationToken cancellationToken)
        {
            var productToAddOption = await _mediator.Send(new GetProductByIdRequest { Id = request.ProductId });

            if (!productToAddOption)
                return Option<ProductOption>.None;

            var productOption = _mapper.Map<ProductOption>(request.ProductOptionDto);

            _dbContext.ProductOptions.Add(UpdateIds(productOption, request.ProductId));
            await _dbContext.SaveChangesAsync(cancellationToken);

            return productOption;
        }

        private ProductOption UpdateIds(ProductOption productOption, Guid productId)
        {
            productOption.Id = Guid.NewGuid();
            productOption.ProductId = productId;

            return productOption;
        }
    }
}