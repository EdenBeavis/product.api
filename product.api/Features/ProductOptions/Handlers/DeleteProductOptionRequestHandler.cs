using Bearded.Monads;
using MediatR;
using product.api.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Void = product.api.Infrastructure.Void;

namespace product.api.Features.ProductOptions.Handlers
{
    public class DeleteProductOptionRequest : IRequest<Option<Void>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductOptionRequestHandler : IRequestHandler<DeleteProductOptionRequest, Option<Void>>
    {
        private readonly IMediator _mediator;
        private readonly ProductsDbContext _dbContext;

        public DeleteProductOptionRequestHandler(IMediator mediator, ProductsDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Option<Void>> Handle(DeleteProductOptionRequest request, CancellationToken cancellationToken)
        {
            var productOptionToDelete = await _mediator.Send(new GetProductOptionByIdRequest { Id = request.Id });

            if (!productOptionToDelete)
                return Option<Void>.None;

            _dbContext.ProductOptions.Remove(productOptionToDelete.ElseNew());
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Void.Instance;
        }
    }
}