namespace YGL.API.SafeObjects; 

public class SafeCompany {
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public short Country { get; set; }

    public SafeCompany(YGL.Model.Company company) {
        this.Id = company.Id;
        this.Description = company.Description;
        this.Name = company.Name;
        this.Country = company.Country;
    }
}