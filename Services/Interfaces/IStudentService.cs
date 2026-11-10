using System.Collections.Generic;
using System.Threading.Tasks;
using WestCoastEducationDesktop.Models;

namespace WestCoastEducationDesktop.Services.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync();
    Task<Student> CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(string id);
}
