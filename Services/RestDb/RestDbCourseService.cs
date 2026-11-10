using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.Services.RestDb;

/// <summary>
/// Talks to the "courses" collection on RestDB.io over plain HTTP.
/// The interface (ICourseService) is what the rest of the app depends
/// on, so this class could be swapped for a different backend without
/// touching any ViewModel.
/// </summary>
public class RestDbCourseService : ICourseService
{
    private const string CollectionPath = "courses";
    private readonly HttpClient _httpClient;

    public RestDbCourseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Course>>(CollectionPath);
        return result ?? new List<Course>();
    }

    public async Task<Course> CreateAsync(Course course)
    {
        var response = await _httpClient.PostAsJsonAsync(CollectionPath, course);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<Course>();
        return created ?? course;
    }

    public async Task UpdateAsync(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Id))
        {
            throw new InvalidOperationException("Course must have an Id to be updated.");
        }

        var response = await _httpClient.PutAsJsonAsync($"{CollectionPath}/{course.Id}", course);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"{CollectionPath}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
