using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Company;
using YGL.API.Domain;

namespace YGL.API.Services.IControllers {
public interface ICompanyService {
    public Task<CompanyResult> GetCompanies(string companyIds);
    public Task<CompanyResult> GetCompaniesFilter(CompanyFilterQuery companyFilterQuery, PaginationFilter paginationFilter);
}
}