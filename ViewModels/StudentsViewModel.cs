using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.ViewModels;

public partial class StudentsViewModel : ViewModelBase
{
    private readonly IStudentService _studentService;

    public ObservableCollection<Student> Students { get; } = new();

    [ObservableProperty] private string _newFirstName = string.Empty;
    [ObservableProperty] private string _newLastName = string.Empty;
    [ObservableProperty] private string _newPhone = string.Empty;
    [ObservableProperty] private string _newPersonalNumber = string.Empty;
    [ObservableProperty] private string _newAddress = string.Empty;
    [ObservableProperty] private string _newPostalCode = string.Empty;
    [ObservableProperty] private string _newCity = string.Empty;

    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;

    public StudentsViewModel(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            var students = await _studentService.GetAllAsync();
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not load students: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NewFirstName) || string.IsNullOrWhiteSpace(NewLastName))
        {
            StatusMessage = "First and last name are required.";
            return;
        }

        var student = new Student
        {
            FirstName = NewFirstName,
            LastName = NewLastName,
            Phone = NewPhone,
            PersonalNumber = NewPersonalNumber,
            Address = NewAddress,
            PostalCode = NewPostalCode,
            City = NewCity
        };

        try
        {
            IsBusy = true;
            await _studentService.CreateAsync(student);
            ClearForm();
            await LoadAsync();
            StatusMessage = $"Student {student.FirstName} {student.LastName} added.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not add student: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ClearForm()
    {
        NewFirstName = string.Empty;
        NewLastName = string.Empty;
        NewPhone = string.Empty;
        NewPersonalNumber = string.Empty;
        NewAddress = string.Empty;
        NewPostalCode = string.Empty;
        NewCity = string.Empty;
    }
}
