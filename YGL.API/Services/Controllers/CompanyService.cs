using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGL.API.Contracts.V1.Requests.Company;
using YGL.API.Domain;
using YGL.API.Errors;
using YGL.API.SafeObjects;
using YGL.API.Services.IControllers;
using YGL.API.Validation;

namespace YGL.API.Services.Controllers {
public class CompanyService : ICompanyService {
    private readonly YGL.Model.YGLDataContext _yglDataContext;

    public CompanyService(YGL.Model.YGLDataContext yglDataContext) {
        _yglDataContext = yglDataContext;
    }

    public async Task<CompanyResult> GetCompanies(string companyIds) {
        CompanyResult companyResult = new CompanyResult();

        if (!ValidationUrl.TryParseInt(companyIds, companyResult, out List<int> ids)) {
            companyResult.IsSuccess = false;
            companyResult.StatusCode = HttpStatusCode.UnprocessableEntity;
            return companyResult;
        }

        List<YGL.Model.Company> foundCompanies = await _yglDataContext.Companies.Where(c => ids.Contains(c.Id)).ToListAsync();

        if (foundCompanies is null || foundCompanies.Count == 0) {
            companyResult.IsSuccess = false;
            companyResult.StatusCode = HttpStatusCode.NotFound;
            companyResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CompanyNotFound);
            return companyResult;
        }

        List<SafeCompany> safeCompanies = foundCompanies.ConvertAll(c => new SafeCompany(c));

        companyResult.Companies = safeCompanies;
        companyResult.IsSuccess = true;
        companyResult.StatusCode = HttpStatusCode.OK;
        return companyResult;
    }

    public async Task<CompanyResult> GetCompaniesFilter(CompanyFilterQuery companyFilterQuery,
        PaginationFilter paginationFilter) {
        CompanyResult companyResult = new CompanyResult();

        IQueryable<YGL.Model.Company> companyQueryable = _yglDataContext.Companies
            .Where(u => u.ItemStatus == true);

        companyQueryable = AddFiltersOnQueryGetCompanies(companyFilterQuery, companyQueryable);

        List<YGL.Model.Company> foundCompanies =
            (await companyQueryable.ToPaginatedListAsync(paginationFilter.Skip, paginationFilter.Take));

        if (foundCompanies is null || foundCompanies.Count == 0) {
            companyResult.IsSuccess = false;
            companyResult.StatusCode = HttpStatusCode.NotFound;
            companyResult.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CompanyNotFound);
            return companyResult;
        }

        companyResult.Companies = foundCompanies.ConvertAll(c => new SafeCompany(c));

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