using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.ViewModels;

public partial class TeachersViewModel : ViewModelBase
{
    private readonly ITeacherService _teacherService;

    public ObservableCollection<Teacher> Teachers { get; } = new();

    // --- Add-teacher form ---
    [ObservableProperty] private string _newFirstName = string.Empty;
    [ObservableProperty] private string _newLastName = string.Empty;
    [ObservableProperty] private string _newPhone = string.Empty;
    [ObservableProperty] private string _newPersonalNumber = string.Empty;
    [ObservableProperty] private string _newAddress = string.Empty;
    [ObservableProperty] private string _newPostalCode = string.Empty;
    [ObservableProperty] private string _newCity = string.Empty;
    [ObservableProperty] private string _newCompetencyArea = string.Empty;

    [ObservableProperty]
    private Teacher? _selectedTeacher;

    // --- Edit selected teacher form ---
    [ObservableProperty] private string _editFirstName = string.Empty;
    [ObservableProperty] private string _editLastName = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;
    [ObservableProperty] private string _editPersonalNumber = string.Empty;
    [ObservableProperty] private string _editAddress = string.Empty;
    [ObservableProperty] private string _editPostalCode = string.Empty;
    [ObservableProperty] private string _editCity = string.Empty;
    [ObservableProperty] private string _editCompetencyArea = string.Empty;

    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;

    public TeachersViewModel(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    partial void OnSelectedTeacherChanged(Teacher? value)
    {
        if (value is null)
        {
            EditFirstName = string.Empty;
            EditLastName = string.Empty;
            EditPhone = string.Empty;
            EditPersonalNumber = string.Empty;
            EditAddress = string.Empty;
            EditPostalCode = string.Empty;
            EditCity = string.Empty;
            EditCompetencyArea = string.Empty;
            return;
        }

        EditFirstName = value.FirstName;
        EditLastName = value.LastName;
        EditPhone = value.Phone;
        EditPersonalNumber = value.PersonalNumber;
        EditAddress = value.Address;
        EditPostalCode = value.PostalCode;
        EditCity = value.City;
        EditCompetencyArea = value.CompetencyArea;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            var teachers = await _teacherService.GetAllAsync();
            Teachers.Clear();
            foreach (var teacher in teachers)
            {
                Teachers.Add(teacher);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not load teachers: {ex.Message}";
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

        var teacher = new Teacher
        {
            FirstName = NewFirstName,
            LastName = NewLastName,
            Phone = NewPhone,
            PersonalNumber = NewPersonalNumber,
            Address = NewAddress,
            PostalCode = NewPostalCode,
            City = NewCity,
            CompetencyArea = NewCompetencyArea
        };

        try
        {
            IsBusy = true;
            await _teacherService.CreateAsync(teacher);
            ClearAddForm();
            await LoadAsync();
            StatusMessage = $"Teacher {teacher.FirstName} {teacher.LastName} added.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not add teacher: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// G-requirement: manage a teacher's competency area specifically.
    /// </summary>
    [RelayCommand]
    private async Task SaveCompetencyAreaAsync()
    {
        if (SelectedTeacher is null || string.IsNullOrWhiteSpace(SelectedTeacher.Id))
        {
            StatusMessage = "Select a teacher first.";
            return;
        }

        try
        {
            IsBusy = true;
            await _teacherService.UpdateCompetencyAreaAsync(SelectedTeacher.Id, EditCompetencyArea);
            await LoadAsync();
            StatusMessage = "Competency area updated.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not update competency area: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// VG-requirement: update a teacher's full record.
    /// </summary>
    [RelayCommand]
    private async Task UpdateTeacherAsync()
    {
        if (SelectedTeacher is null || string.IsNullOrWhiteSpace(SelectedTeacher.Id))
        {
            StatusMessage = "Select a teacher first.";
            return;
        }

        var updated = new Teacher
        {
            Id = SelectedTeacher.Id,
            FirstName = EditFirstName,
            LastName = EditLastName,
            Phone = EditPhone,
            PersonalNumber = EditPersonalNumber,
            Address = EditAddress,
            PostalCode = EditPostalCode,
            City = EditCity,
            CompetencyArea = EditCompetencyArea
        };

        try
        {
            IsBusy = true;
            await _teacherService.UpdateAsync(updated);
            await LoadAsync();
            StatusMessage = $"Teacher {updated.FirstName} {updated.LastName} updated.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not update teacher: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ClearAddForm()
    {
        NewFirstName = string.Empty;
        NewLastName = string.Empty;
        NewPhone = string.Empty;
        NewPersonalNumber = string.Empty;
        NewAddress = string.Empty;
        NewPostalCode = string.Empty;
        NewCity = string.Empty;
        NewCompetencyArea = string.Empty;
    }
}
