using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.Company;
using YGL.API.Domain;
using YGL.API.Domain.SafeObjects;
using YGL.API.Errors;

namespace YGL.API.Services.Controllers {
public class CompanyService : ICompanyService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public CompanyService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<CompanyResult> GetCompany(int companyId) {
        CompanyResult companyResult = new CompanyResult();

        YGL.Model.Company foundCompany =
            await _yglDataContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId && c.ItemStatus == true);

        if (foundCompany is null) {
            companyResult.IsSuccess = false;
            companyResult.StatusCode = HttpStatusCode.NotFound;
            companyResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CompanyNotFound);
            return companyResult;
        }

        companyResult.Companies = new List<SafeCompany>() { new SafeCompany(foundCompany) };

        companyResult.IsSuccess = true;
        companyResult.StatusCode = HttpStatusCode.OK;
        return companyResult;
    }

    public async Task<CompanyResult> GetCompanies(CompanyFilterQuery companyFilterQuery,
        PaginationFilter paginationFilter) {
        CompanyResult companyResult = new CompanyResult();

        IQueryable<YGL.Model.Company> companyQueryable = _yglDataContext.Companies
            .Where(u => u.ItemStatus == true);

        companyQueryable = AddFiltersOnQueryGetCompanies(companyFilterQuery, companyQueryable);

        List<SafeCompany> safeCompanies =
            (await companyQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take))
            .ConvertAll(c => new SafeCompany(c));

        companyResult.Companies = safeCompanies;

        companyResult.IsSuccess = true;
        companyResult.StatusCode = HttpStatusCode.OK;
        return companyResult;
    }

    private static IQueryable<YGL.Model.Company> AddFiltersOnQueryGetCompanies(CompanyFilterQuery filterQuery,
        IQueryable<YGL.Model.Company> queryable) {
        if (filterQuery is null) return queryable;

        if (!String.IsNullOrEmpty(filterQuery.CompanyName))
            queryable = queryable.Where(c => c.Name.Contains(filterQuery.CompanyName));

        return queryable;
    }
}
}