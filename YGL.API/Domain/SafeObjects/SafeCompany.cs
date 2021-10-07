namespace YGL.API.Domain.SafeObjects {
public class SafeCompany {
    public string Description { get; set; }
    public string Name { get; set; }
    public short Country { get; set; }

    public SafeCompany(YGL.Model.Company company) {
        this.Description = company.Description;
        this.Name = company.Name;
        this.Country = company.Country;
    }
}
}