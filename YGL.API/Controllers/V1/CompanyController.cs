using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGL.API.Attributes;
using YGL.API.Contracts.V1.Requests;
using YGL.API.Contracts.V1.Requests.Company;
using YGL.API.Contracts.V1.Responses;
using YGL.API.Contracts.V1.Responses.Company;
using YGL.API.Domain;
using YGL.API.Services.IControllers;

namespace YGL.API.Controllers.V1; 

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CompanyController : ControllerBase {
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService) {
        _companyService = companyService;
    }
    
    [HttpGet(Routes.Company.GetCompany)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetCompanies(string companyIds) {
        IResponse res;

        CompanyResult companyResult = await _companyService.GetCompanies(companyIds);

        if (companyResult.IsSuccess) {
            res = companyResult.Companies
                .Select(c => new CompanyGetSuccRes() { Company = c })
                .ToResponse(companyResult.Companies.Count);

            return this.ReturnResult(companyResult.StatusCode, res);
        }

        res = new CompanyGetFailRes() {
            ErrorCodes = companyResult.ErrorCodes,
            ErrorMessages = companyResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(companyResult.StatusCode, res);
    }


    [HttpGet(Routes.Company.GetCompanies)]
    [RedisCached(3600)]
    public async Task<IActionResult> GetCompaniesFilter(
        [FromQuery] CompanyFilterQuery companyFilterQuery, [FromQuery] PaginationQuery paginationQuery) {
        IResponse res;

        CompanyResult companyResult = await _companyService.GetCompaniesFilter(companyFilterQuery, (PaginationFilter)paginationQuery);

        if (companyResult.IsSuccess) {
            res = companyResult.Companies
                .Select(c => new CompanyGetSuccRes() { Company = c })
                .ToPagedResponse(companyResult.Companies.Count, (PaginationFilter)paginationQuery);

            return this.ReturnResult(companyResult.StatusCode, res);
        }

        res = new CompanyGetFailRes() {
            ErrorCodes = companyResult.ErrorCodes,
            ErrorMessages = companyResult.ErrorMessages
        }.ToResponseWithErrors();

        return this.ReturnResult(companyResult.StatusCode, res);
    }
}