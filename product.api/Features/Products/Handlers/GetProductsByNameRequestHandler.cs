using MediatR;
using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.Products.Handlers
{
    public class GetProductsByNameRequest : IRequest<IEnumerable<Product>>
    {
        public string Name { get; set; }
    }

    public class GetProductsByNameRequestHandler : IRequestHandler<GetProductsByNameRequest, IEnumerable<Product>>
    {
        private readonly ProductsDbContext _dbContext;

        public GetProductsByNameRequestHandler(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsByNameRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.Where(product => product.Name.ToLower().Contains(request.Name.ToLower())).ToListAsync(cancellationToken);
        }
    }
}