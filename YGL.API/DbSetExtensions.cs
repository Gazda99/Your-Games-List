using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YGL.API; 

public static class DbSetExtensions {
    //maybe use offset instead of skip
    public static async Task<List<TSource>> ToPaginatedListAsync<TSource>(
        [NotNull] this IQueryable<TSource> source, int skip, int pageSize,
        CancellationToken cancellationToken = default) {
        return await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
    }
}