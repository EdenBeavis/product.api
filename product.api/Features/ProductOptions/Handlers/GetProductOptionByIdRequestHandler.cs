using Bearded.Monads;
using MediatR;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Infrastructure.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.ProductOptions.Handlers
{
    public class GetProductOptionByIdRequest : IRequest<Option<ProductOption>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductOptionByIdRequestHandler : IRequestHandler<GetProductOptionByIdRequest, Option<ProductOption>>
    {
        private readonly ProductsDbContext _dbContext;

        public GetProductOptionByIdRequestHandler(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Option<ProductOption>> Handle(GetProductOptionByIdRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.ProductOptions.FirstOrNoneAsync(po => po.Id.Equals(request.Id), cancellationToken);
        }
    }
}