using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeansOffice.Models;
using Student = DeansOffice.Models.Student;

namespace DeansOffice.DAL
{

    class EfServiceDb
    {
        private static PjatkDb context = new PjatkDb();

        internal static ObservableCollection<Student> LoadStudentsDataFromDb()
        {
           // var context = new PjatkDb();
            var res = context.Students.ToList();

            ObservableCollection<Student> ListaStudentow = new ObservableCollection<Student>(res);
            return ListaStudentow;
        }

        internal static ObservableCollection<Study> LoadStudiesDataFromDb()
        {
           // var context = new PjatkDb();
            var res = context.Studies.ToList();

            ObservableCollection<Study> ListaStudiow = new ObservableCollection<Study>(res);
            return ListaStudiow;
        }

        internal static ObservableCollection<Subject> LoadSubjectsDataFromDb()
        {
           // var context = new PjatkDb();
            var res = context.Subjects.ToList();

            ObservableCollection<Subject> ListaStudiow = new ObservableCollection<Subject>(res);
            return ListaStudiow;
        }

        internal static void AddStudent(Student newStudent)
        {
            //   var context = new PjatkDb();

            context.Students.Add(newStudent);

            context.SaveChanges();

            /* var subs = newStudent.Subjects;
             newStudent.Subjects = null;


             foreach (Subject sub in subs)
             {
                 var studsub = new Student_Subject()
                 {
                     IdStudent = newStudent.IdStudent,
                     IdSubject = sub.IdSubject
                 };

                 context.Student_Subject.Add(studsub);
             }
             */

            /* var newStudies = new Study()
             {
                 IdStudies = newStudent.IdStudies
             };
             //context.Studies.Attach(newStudies);*/


            //var subs = newStudent.Subjects;
            /* newStudent.Subjects = null;

              context.SaveChanges();

              newStudent.Subjects = subs; */

            /*  foreach (Subject sub in newStudent.Subjects)
              {
                  sub.Student_Subjects.Add(newStudent);
              }*/

        }



        internal static bool RemoveStudents(List<int> SelectedStudentsId)
        {
            try
            {
         //       var context = new PjatkDb();

                foreach (var id in SelectedStudentsId)
                {

                    var delStudent = new Student()
                    {
                        IdStudent = id
                    };

                    context.Students.Attach(delStudent);
                    context.Students.Remove(delStudent);

                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }


        internal static bool RemoveStudents(List<Student> SelectedStudents)
        {
            try
            {
                //       var context = new PjatkDb();

                foreach (var Student in SelectedStudents)
                {

                  /*  var delStudent = new Student()
                    {
                        IdStudent = id
                    };

                    context.Students.Attach(delStudent);*/
                    context.Students.Remove(Student);  
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }


        internal static bool UpdateStudent(Student ChangedStudent)
        {
            try
            {
                context.SaveChanges();

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }


    }
}

