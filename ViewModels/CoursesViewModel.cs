using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducationDesktop.Models;
using WestCoastEducationDesktop.Services.Interfaces;

namespace WestCoastEducationDesktop.ViewModels;

public partial class CoursesViewModel : ViewModelBase
{
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;

    public ObservableCollection<Course> Courses { get; } = new();
    public ObservableCollection<Teacher> Teachers { get; } = new();

    [ObservableProperty]
    private Course? _selectedCourse;

    // --- Add-course form ---
    [ObservableProperty] private string _newCourseNumber = string.Empty;
    [ObservableProperty] private string _newTitle = string.Empty;
    [ObservableProperty] private int _newLengthInWeeks = 1;
    [ObservableProperty] private DateTimeOffset _newStartDate = DateTimeOffset.Now;
    [ObservableProperty] private DateTimeOffset _newEndDate = DateTimeOffset.Now.AddDays(7);
    [ObservableProperty] private bool _newIsClassroom = true;
    [ObservableProperty] private string _newLocation = string.Empty;
    [ObservableProperty] private Teacher? _newTeacher;

    // --- Edit selected course form (VG: update a course) ---
    [ObservableProperty] private string _editTitle = string.Empty;
    [ObservableProperty] private int _editLengthInWeeks = 1;
    [ObservableProperty] private DateTimeOffset _editStartDate = DateTimeOffset.Now;
    [ObservableProperty] private DateTimeOffset _editEndDate = DateTimeOffset.Now.AddDays(7);
    [ObservableProperty] private bool _editIsClassroom = true;
    [ObservableProperty] private string _editLocation = string.Empty;
    [ObservableProperty] private Teacher? _editTeacher;

    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;

    public CoursesViewModel(ICourseService courseService, ITeacherService teacherService)
    {
        _courseService = courseService;
        _teacherService = teacherService;
    }

    partial void OnSelectedCourseChanged(Course? value)
    {
        if (value is null)
        {
            EditTitle = string.Empty;
            EditLengthInWeeks = 1;
            EditStartDate = DateTimeOffset.Now;
            EditEndDate = DateTimeOffset.Now.AddDays(7);
            EditIsClassroom = true;
            EditLocation = string.Empty;
            EditTeacher = null;
            return;
        }

        EditTitle = value.Title;
        EditLengthInWeeks = value.LengthInWeeks;
        EditStartDate = value.StartDate;
        EditEndDate = value.EndDate;
        EditIsClassroom = value.IsClassroom;
        EditLocation = value.Location;
        EditTeacher = FindTeacherById(value.TeacherId);
    }

    private Teacher? FindTeacherById(string teacherId)
    {
        foreach (var teacher in Teachers)
        {
            if (teacher.Id == teacherId)
            {
                return teacher;
            }
        }

        return null;
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

            var courses = await _courseService.GetAllAsync();
            Courses.Clear();
            foreach (var course in courses)
            {
                Courses.Add(course);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not load courses: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NewCourseNumber) || string.IsNullOrWhiteSpace(NewTitle))
        {
            StatusMessage = "Course number and title are required.";
            return;
        }

        var course = new Course
        {
            CourseNumber = NewCourseNumber,
            Title = NewTitle,
            LengthInWeeks = NewLengthInWeeks,
            StartDate = NewStartDate.Date,
            EndDate = NewEndDate.Date,
            IsClassroom = NewIsClassroom,
            Location = NewLocation,
            TeacherId = NewTeacher?.Id ?? string.Empty,
            TeacherName = NewTeacher?.FullName ?? string.Empty
        };

        try
        {
            IsBusy = true;
            await _courseService.CreateAsync(course);
            ClearAddForm();
            await LoadAsync();
            StatusMessage = $"Course {course.CourseNumber} added.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not add course: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UpdateAsync()
    {
        if (SelectedCourse is null || string.IsNullOrWhiteSpace(SelectedCourse.Id))
        {
            StatusMessage = "Select a course to update first.";
            return;
        }

        var updated = new Course
        {
            Id = SelectedCourse.Id,
            CourseNumber = SelectedCourse.CourseNumber,
            Title = EditTitle,
            LengthInWeeks = EditLengthInWeeks,
            StartDate = EditStartDate.Date,
            EndDate = EditEndDate.Date,
            IsClassroom = EditIsClassroom,
            Location = EditLocation,
            TeacherId = EditTeacher?.Id ?? string.Empty,
            TeacherName = EditTeacher?.FullName ?? string.Empty
        };

        try
        {
            IsBusy = true;
            await _courseService.UpdateAsync(updated);
            await LoadAsync();
            StatusMessage = $"Course {updated.CourseNumber} updated.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not update course: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedCourse is null || string.IsNullOrWhiteSpace(SelectedCourse.Id))
        {
            StatusMessage = "Select a course to remove first.";
            return;
        }

        try
        {
            IsBusy = true;
            await _courseService.DeleteAsync(SelectedCourse.Id);
            SelectedCourse = null;
            await LoadAsync();
            StatusMessage = "Course removed.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not remove course: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ClearAddForm()
    {
        NewCourseNumber = string.Empty;
        NewTitle = string.Empty;
        NewLengthInWeeks = 1;
        NewStartDate = DateTimeOffset.Now;
        NewEndDate = DateTimeOffset.Now.AddDays(7);
        NewIsClassroom = true;
        NewLocation = string.Empty;
        NewTeacher = null;
    }
}
