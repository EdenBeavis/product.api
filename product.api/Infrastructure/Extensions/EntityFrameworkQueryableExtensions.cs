using Bearded.Monads;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Infrastructure.Extensions
{
    public static class EntityFrameworkQueryableExtensions
    {
        public static async Task<Option<TSource>> FirstOrNoneAsync<TSource>([NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return await source.FirstOrDefaultAsync(cancellationToken) ?? Option<TSource>.None;
        }

        public static async Task<Option<TSource>> FirstOrNoneAsync<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await source.FirstOrDefaultAsync(predicate, cancellationToken) ?? Option<TSource>.None;
        }
    }
}