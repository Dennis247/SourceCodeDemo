using BlazorMvvMDemo.Models;

namespace BlazorMvvMDemo.ViewModels
{
    public class StudentViewModels
    {
        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public Student CurrentStudent { get; set; } = new Student();

        public List<Student> AllSudents { get; set; } = new List<Student>();

        public void SetSetlectedStudent(Student selectedStudent)
        {
        CurrentStudent = selectedStudent;
        }

        public void SaveStudent()
        {
            if(CurrentStudent.Id == Guid.Empty)
            {
                AllSudents.Add(CurrentStudent);
                SuccessMessage = "Added student sucessfuly";
            }
            else
            {
                var existingSTudents = AllSudents.FirstOrDefault(x=>x.Id == CurrentStudent.Id);
                var existingStudentIndex = AllSudents.IndexOf(CurrentStudent);
                AllSudents.RemoveAt(existingStudentIndex);
                AllSudents.Insert(existingStudentIndex, CurrentStudent);
                SuccessMessage = "Updated student sucessfuly";
            }
          
        }

  

        public void DeleteStudent(Guid id)
        {
            var existingStudent = AllSudents.FirstOrDefault(s => s.Id == id);
            if(existingStudent != null)
            {
                AllSudents.Remove(existingStudent);
            }
            else
            {
                ErrorMessage = "Student profile does not exist";
            }
        }
    }
}
