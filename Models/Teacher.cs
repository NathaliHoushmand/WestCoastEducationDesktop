using System.Text.Json.Serialization;

namespace WestCoastEducationDesktop.Models;

public class Teacher
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PersonalNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string CompetencyArea { get; set; } = string.Empty;

    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}";

    [JsonIgnore]
    public string DisplayText => $"{FullName} | Competency: {CompetencyArea} | Tel: {Phone}";
}
