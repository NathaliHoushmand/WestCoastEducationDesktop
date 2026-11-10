namespace WestCoastEducationDesktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public CoursesViewModel Courses { get; }
    public StudentsViewModel Students { get; }
    public TeachersViewModel Teachers { get; }

    public MainWindowViewModel(CoursesViewModel courses, StudentsViewModel students, TeachersViewModel teachers)
    {
        Courses = courses;
        Students = students;
        Teachers = teachers;
    }
}
