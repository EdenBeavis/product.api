using MediatR;
using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.ProductOptions.Handlers
{
    public class GetProductOptionsByProductIdRequest : IRequest<IEnumerable<ProductOption>>
    {
        public Guid ProductId { get; set; }
    }

    public class GetProductOptionsByProductIdRequestHandler : IRequestHandler<GetProductOptionsByProductIdRequest, IEnumerable<ProductOption>>
    {
        private readonly ProductsDbContext _dbContext;

        public GetProductOptionsByProductIdRequestHandler(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductOption>> Handle(GetProductOptionsByProductIdRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.ProductOptions.Where(po => po.ProductId.Equals(request.ProductId)).ToListAsync(cancellationToken);
        }
    }
}