using Bearded.Monads;
using MediatR;
using product.api.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Void = product.api.Infrastructure.Void;

namespace product.api.Features.Products.Handlers
{
    public class DeleteProductRequest : IRequest<Option<Void>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, Option<Void>>
    {
        private readonly IMediator _mediator;
        private readonly ProductsDbContext _dbContext;

        public DeleteProductRequestHandler(IMediator mediator, ProductsDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Option<Void>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var productToDeleteOptional = await _mediator.Send(new GetProductByIdRequest { Id = request.Id });

            if (!productToDeleteOptional)
                return Option<Void>.None;

            _dbContext.ProductOptions.RemoveRange(productToDeleteOptional.ElseNew().Options);
            _dbContext.Products.Remove(productToDeleteOptional.ElseNew());

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Void.Instance;
        }
    }
}