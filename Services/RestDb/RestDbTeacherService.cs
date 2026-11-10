using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.Services.RestDb;

public class RestDbTeacherService : ITeacherService
{
    private const string CollectionPath = "teachers";
    private readonly HttpClient _httpClient;

    public RestDbTeacherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Teacher>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Teacher>>(CollectionPath);
        return result ?? new List<Teacher>();
    }

    public async Task<Teacher> CreateAsync(Teacher teacher)
    {
        var response = await _httpClient.PostAsJsonAsync(CollectionPath, teacher);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<Teacher>();
        return created ?? teacher;
    }

    public async Task UpdateAsync(Teacher teacher)
    {
        if (string.IsNullOrWhiteSpace(teacher.Id))
        {
            throw new InvalidOperationException("Teacher must have an Id to be updated.");
        }

        var response = await _httpClient.PutAsJsonAsync($"{CollectionPath}/{teacher.Id}", teacher);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"{CollectionPath}/{id}");
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Dedicated method for the G-requirement of specifically managing
    /// a teacher's competency area (separate from a full record update).
    /// RestDB merges partial documents on PUT, so only this one field
    /// is sent.
    /// </summary>
    public async Task UpdateCompetencyAreaAsync(string teacherId, string competencyArea)
    {
        var payload = new { CompetencyArea = competencyArea };
        var response = await _httpClient.PutAsJsonAsync($"{CollectionPath}/{teacherId}", payload);
        response.EnsureSuccessStatusCode();
    }
}
