using System.Collections.Generic;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;

namespace WestCoastEducationDesktop.Services.Interfaces;

public interface ITeacherService
{
    Task<List<Teacher>> GetAllAsync();
    Task<Teacher> CreateAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task DeleteAsync(string id);
    Task UpdateCompetencyAreaAsync(string teacherId, string competencyArea);
}
