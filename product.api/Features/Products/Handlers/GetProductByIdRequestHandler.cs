using Bearded.Monads;
using MediatR;
using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Infrastructure.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.Products.Handlers
{
    public class GetProductByIdRequest : IRequest<Option<Product>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, Option<Product>>
    {
        private readonly ProductsDbContext _dbContext;

        public GetProductByIdRequestHandler(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Option<Product>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.Include(p => p.Options)
                .FirstOrNoneAsync(product => product.Id.Equals(request.Id), cancellationToken);
        }
    }
}