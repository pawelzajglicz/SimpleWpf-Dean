using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.ModelsOld
{
    class Student
    {

        public static IDictionary<int, Student> ExtensionStudents = new Dictionary<int, Student>();


        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string IndexNumber { get; set; }
        public int IdStudies{ get; set; }
        public Studies StudentStudies { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<int> SubjectsId { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {IndexNumber}";
        }

        public String GetStudiesName()
        {
            return this.StudentStudies.Name;
        }

        public void AddToExtension()
        {
           // MessageBox.Show(this.IdStudent.ToString());
            ExtensionStudents.Add(this.IdStudent, this);
        }

        public void AddSubjectId(int subjectID)
        {
            if (SubjectsId == null)
            {
                SubjectsId = new List<int>();
            }
          //  MessageBox.Show("aa " + this.IdStudent.ToString() + " " + subjectID.ToString());
            SubjectsId.Add(subjectID);
        }
    }

}
