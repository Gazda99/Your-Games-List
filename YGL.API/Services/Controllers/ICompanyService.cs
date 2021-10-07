using System.Threading.Tasks;
using YGL.API.Contracts.V1.Requests.Company;
using YGL.API.Domain;

namespace YGL.API.Services.Controllers {
public interface ICompanyService {
    public Task<CompanyResult> GetCompany(int companyId);
    public Task<CompanyResult> GetCompanies(CompanyFilterQuery companyFilterQuery, PaginationFilter paginationFilter);
}
}