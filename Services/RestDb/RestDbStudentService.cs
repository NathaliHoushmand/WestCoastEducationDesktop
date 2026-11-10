using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.Services.RestDb;

public class RestDbStudentService : IStudentService
{
    private const string CollectionPath = "students";
    private readonly HttpClient _httpClient;

    public RestDbStudentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Student>>(CollectionPath);
        return result ?? new List<Student>();
    }

    public async Task<Student> CreateAsync(Student student)
    {
        var response = await _httpClient.PostAsJsonAsync(CollectionPath, student);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<Student>();
        return created ?? student;
    }

    public async Task UpdateAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Id))
        {
            throw new InvalidOperationException("Student must have an Id to be updated.");
        }

        var response = await _httpClient.PutAsJsonAsync($"{CollectionPath}/{student.Id}", student);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"{CollectionPath}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
