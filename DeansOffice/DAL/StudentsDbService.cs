using DeansOffice.ModelsOld;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.DAL
{
    class StudentsDbService
    {
        private static string connectionString = "Data Source=db-mssql;Initial Catalog=s14941;Integrated Security=True";

        public static ObservableCollection<Student> LoadStudentsDataFromDb()
        {
            ObservableCollection<Student> ListaStudentow = new ObservableCollection<Student>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("Select Student.IdStudent, Student.FirstName, Student.LastName, Student.Address, Student.IndexNumber, Student.IdStudies, Studies.Name " +
                        "From apbd.Student, apbd.Studies WHERE Student.IdStudies = Studies.IdStudies;", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var DBIdStudent = (int)reader["IdStudent"];
                            var DBFirstName = reader["firstName"].ToString();
                            var DBLastName = reader["lastName"].ToString();
                            var DBAddress = reader["Address"].ToString();
                            var DBIndexNumber = reader["IndexNumber"].ToString();
                            var DBIdStudies = (int)reader["IdStudies"];
                            var newSubjectsId = new List<int>();

                            var StudiesObj = Studies.GetStudies(DBIdStudies);

                            if (StudiesObj == null)
                            {
                                var DBStudiesName = reader["Name"].ToString();
                                StudiesObj = new Studies(DBIdStudies, DBStudiesName);
                            }

                            Student newStudent = new Student
                            { IdStudent = DBIdStudent, FirstName = DBFirstName, LastName = DBLastName, Address = DBAddress, IdStudies = DBIdStudies, IndexNumber = DBIndexNumber, StudentStudies = StudiesObj , SubjectsId = newSubjectsId};

                            ListaStudentow.Add(newStudent);
                            newStudent.AddToExtension();
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.StackTrace);
                }

                try
                {
                    using (SqlCommand command = new SqlCommand("select IdStudent, IdSubject from apbd.Student_Subject;", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var DBIdStudent = (int)reader["IdStudent"];
                            var DBIdSubject = (int)reader["IdSubject"];

                            //MessageBox.Show(DBIdStudent.ToString());
                            var student = Student.ExtensionStudents[DBIdStudent];
                            student.AddSubjectId(DBIdSubject);
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.StackTrace);
                }

            }


            return ListaStudentow;
        }

        internal static void UpdateStudent(Student modifiedStudent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();
                
                command.CommandText = "UPDATE apbd.Student " +
                   "SET FirstName = @FirstName, LastName = @LastName, Address = @Address, IndexNumber = @IndexNumber, IdStudies = @IdStudies WHERE IdStudent = @ModifiedStudentId";


                command.Parameters.AddWithValue("FirstName", modifiedStudent.FirstName);
                command.Parameters.AddWithValue("LastName", modifiedStudent.LastName);
                command.Parameters.AddWithValue("Address", modifiedStudent.Address);
                command.Parameters.AddWithValue("IndexNumber", modifiedStudent.IndexNumber);
                command.Parameters.AddWithValue("IdStudies", modifiedStudent.IdStudies.ToString());
                command.Parameters.AddWithValue("ModifiedStudentId", modifiedStudent.IdStudent.ToString());



                command.Connection = con;
                var tran = con.BeginTransaction();
                command.Transaction = tran;
                command.ExecuteNonQuery();


                var SubjectsIdList = new List<int>();

                command.CommandText = "SELECT IdStudentSubject, IdSubject FROM apbd.Student_Subject WHERE IdStudent = @StudentId;";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("StudentId", modifiedStudent.IdStudent);
                command.Connection = con;

                IDictionary<int, int> CurrentSubjcetsIDSInDB = new Dictionary<int, int>();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var StudentSubjectId = (int)reader["IdStudentSubject"];
                        var SubjectId = (int)reader["IdSubject"];

                        CurrentSubjcetsIDSInDB[StudentSubjectId] = SubjectId;
                    }
                }

                List<int> IdSubjectsToAdd = new List<int>();

                foreach (Subject subject in modifiedStudent.Subjects)
                {
                    if (!CurrentSubjcetsIDSInDB.Values.Contains(subject.IdSubject))
                    {
                        IdSubjectsToAdd.Add(subject.IdSubject);
                    }
                }

                if (IdSubjectsToAdd.Count > 0)
                {
                    foreach (int idSubject in IdSubjectsToAdd)
                    {
                        command.CommandText = "INSERT INTO apbd.Student_Subject(IdStudent, IdSubject, CreatedAt) " +
                                                              "VALUES(@IdStudent, @IdSubject, @Date);";

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("IdStudent", modifiedStudent.IdStudent);
                        command.Parameters.AddWithValue("IdSubject", idSubject);
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        command.Parameters.AddWithValue("Date", sqlFormattedDate);


                        command.ExecuteNonQuery();
                    }
                }

                List<int> IdStudentSubjectsToRemove = new List<int>();
                bool remove;

                foreach (KeyValuePair<int, int> item in CurrentSubjcetsIDSInDB)
                {
                    remove = true;

                    foreach (Subject subject in modifiedStudent.Subjects)
                    {
                        if (subject.IdSubject == item.Value)
                        {
                            remove = false;
                            break;
                        }
                    }

                    if (remove)
                    {
                        IdStudentSubjectsToRemove.Add(item.Key);
                    }
                }


                if (IdStudentSubjectsToRemove.Count > 0)
                {
                    for (int i = 0; i < IdStudentSubjectsToRemove.Count(); i++)
                    {
                        command.CommandText = "DELETE FROM apbd.Student_Subject WHERE apbd.Student_Subject.IdStudentSubject IN(@IDToRemove); ";

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("IDToRemove", IdStudentSubjectsToRemove[i].ToString());
                        command.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
        }


        internal static void AddStudent(Student newStudent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO apbd.Student(FirstName, LastName, Address, IndexNumber, IdStudies) " +
                    "VALUES(@FirstName, @LastName, @Address, @IndexNumber, @IdStudies);";

                command.Parameters.AddWithValue("FirstName", newStudent.FirstName);
                command.Parameters.AddWithValue("LastName", newStudent.LastName);
                command.Parameters.AddWithValue("Address", newStudent.Address);
                command.Parameters.AddWithValue("IndexNumber", newStudent.IndexNumber);
                command.Parameters.AddWithValue("IdStudies", newStudent.IdStudies.ToString());

                command.Connection = con;
                var tran = con.BeginTransaction();
                command.Transaction = tran;
                command.ExecuteNonQuery();
                command.ExecuteNonQuery();

                command.CommandText = "select MAX(apbd.Student.IdStudent) from apbd.Student";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var DBLastStudentId = (int)reader[""];
                        newStudent.IdStudent = DBLastStudentId;
                    }
                }

                var SubjectsIdList = new List<int>();
                foreach (Subject subject in newStudent.Subjects)
                {
                    command.CommandText = "SELECT IdSubject FROM apbd.Subject WHERE Name = @SubjectName;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("SubjectName", subject.Name);
                    command.Connection = con;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var SubjectId = (int)reader["IdSubject"];
                            SubjectsIdList.Add(SubjectId);
                        }
                    }
                }

                foreach (int idSubject in SubjectsIdList)
                {
                    command.CommandText = "INSERT INTO apbd.Student_Subject(IdStudent, IdSubject, CreatedAt) " +
                                                          "VALUES(@IdStudent, @IdSubject, @Date);";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("IdStudent", newStudent.IdStudent);
                    command.Parameters.AddWithValue("IdSubject", idSubject);
                    DateTime myDateTime = DateTime.Now;
                    string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    command.Parameters.AddWithValue("Date", sqlFormattedDate);


                    command.ExecuteNonQuery();
                }

                tran.Commit();
            }
        }

        internal static List<Subject> getSubjectsList()
        {
            var SubjectsList = new List<Subject>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("Select IdSubject, Name From apbd.Subject;", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var DBIdSubject = (int)reader["IdSubject"];
                        var DBName = reader["Name"].ToString();

                        SubjectsList.Add(new Subject(DBIdSubject, DBName));
                    }
                }
            }

            return SubjectsList;
        }
        //   }

        internal static void LoadStudies()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("Select IdStudies, Name From apbd.Studies;", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var DBIdStudies = (int)reader["IdStudies"];
                        var DBName = reader["Name"].ToString();

                        new Studies(DBIdStudies, DBName);
                    }
                }
            }
        }

        internal static bool RemoveStudents(List<int> SelectedStudentsId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (var com = new SqlCommand())
                {
                    con.Open();
                    var tran = con.BeginTransaction();
                    com.Connection = con;
                    com.Transaction = tran;

                    foreach (var id in SelectedStudentsId)
                    {
                        com.CommandText = "delete from apbd.Student where IdStudent = @studentno;" +
                                           "delete from apbd.Student_Subject where IdStudent = @studentno;";
                        com.Parameters.AddWithValue("studentno", id.ToString());
                        com.ExecuteNonQuery();
                        com.Parameters.Clear();
                    };

                    tran.Commit();
                    return true;
                }
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
