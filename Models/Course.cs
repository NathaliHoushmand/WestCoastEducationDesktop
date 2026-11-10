using System;
using System.Text.Json.Serialization;

namespace WestCoastEducationDesktop.Models;

public class Course
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    public string CourseNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int LengthInWeeks { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today;
    public bool IsClassroom { get; set; } = true;
    public string Location { get; set; } = string.Empty;
    public string TeacherId { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;

    [JsonIgnore]
    public string CourseTypeLabel => IsClassroom ? "Classroom" : "Remote";

    [JsonIgnore]
    public string DisplayText =>
        $"{CourseNumber} — {Title} ({CourseTypeLabel}) | {StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd} | " +
        $"Teacher: {(string.IsNullOrWhiteSpace(TeacherName) ? "Unassigned" : TeacherName)}";
}
