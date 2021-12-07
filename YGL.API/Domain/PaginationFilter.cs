using System;
using YGL.API.Contracts.V1.Requests;

namespace YGL.API.Domain; 

public class PaginationFilter {
    public const int SkipMin = 0;
    public const int SkipMax = Int32.MaxValue;
    public const int SkipDefault = SkipMin;

    public const int TakeMin = 1;
    public const int TakeMax = 100;
    public const int TakeDefaults = TakeMax;
    public int Skip { get; init; }
    public int Take { get; init; }

    public PaginationFilter(int skip, int take) {
        Skip = SetSkip(skip);
        Take = SetTake(take);
    }

    public PaginationFilter(PaginationQuery paginationQuery) {
        if (paginationQuery is null) {
            Skip = SkipDefault;
            Take = TakeDefaults;
        }
        else {
            Skip = SetSkip(paginationQuery.Skip);
            Take = SetTake(paginationQuery.Take);
        }
    }

    public static explicit operator PaginationFilter(PaginationQuery paginationQuery) {
        return paginationQuery is null
            ? new PaginationFilter(SkipDefault, TakeDefaults)
            : new PaginationFilter(paginationQuery.Skip, paginationQuery.Take);
    }

    public static int SetSkip(int skip) {
        return skip is >= SkipMin and <= SkipMax ? skip : SkipDefault;
    }

    public static int SetTake(int take) {
        return take is >= TakeMin and <=TakeMax ? take : TakeDefaults;
    }
}