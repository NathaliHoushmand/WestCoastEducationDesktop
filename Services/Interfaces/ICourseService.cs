using System.Collections.Generic;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;

namespace WestCoastEducationDesktop.Services.Interfaces;

public interface ICourseService
{
    Task<List<Course>> GetAllAsync();
    Task<Course> CreateAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(string id);
}
