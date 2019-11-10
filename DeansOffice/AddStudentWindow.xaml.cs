using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DeansOffice.DAL;
using DeansOffice.Models;

namespace DeansOffice
{
    /// <summary>
    /// Logika interakcji dla klasy AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private ObservableCollection<Student> ListaStudentow;
        private Student student;

        public AddStudentWindow()
        {
            InitializeComponent();
            PutValueIntoStudiesComboBoxFromDB();
            CancelButton.Click += CancelButton_Click;
            AddButton.Click += AddButton_Click;
        }

        private void PutValueIntoStudiesComboBoxFromDB()
        {
            var ListaStudiow = EfServiceDb.LoadStudiesDataFromDb();

            foreach (Study study in ListaStudiow)
            {
                StudiesComboBox.Items.Add(new ComboBoxItem { Content = study });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.student == null)
            {
                AddStudent();
            }
            else
            {
                ChangeStudent();
            }
        }

        private void ChangeStudent()
        {
            
            var NewStudentLastName = LastNameTextBox.Text;
            var NewStudentFirstName = FirstNameTextBox.Text;
            var NewStudentIndexNumber = IndexNumberTextBox.Text;
            var NewStudentAddress = AddressTextBox.Text;
            var CBI = (ComboBoxItem)StudiesComboBox.SelectedItem;

            if (CBI == null)
            {
                MessageBox.Show("Należy wybrać studia!");
                return;
            }

            var NewStudentStudies = (Study)CBI.Content;
            List<Subject> SubjectsList = new List<Subject>();
            foreach (Subject subject in SubjectsListBox.SelectedItems)
            {
                SubjectsList.Add(subject);
            }

            //   MessageBox.Show(Subjects.Count.ToString());


            if (NewStudentFirstName == null || NewStudentLastName == "" || NewStudentIndexNumber == "" || NewStudentStudies == null || NrIndeksuIsInvalid(NewStudentIndexNumber))
            {
                MessageBox.Show("Niewłaściwe dane");
                return;
            }

            student.FirstName = NewStudentFirstName;
            student.LastName = NewStudentLastName;
            student.Address = NewStudentAddress;
            student.IdStudies = NewStudentStudies.IdStudies;
            student.IndexNumber = NewStudentIndexNumber;
            student.Subjects = SubjectsList;


            ListaStudentow[ListaStudentow.IndexOf(student)] = student;
        }

        private void AddStudent()
        {

            var NewStudentLastName = LastNameTextBox.Text;
            var NewStudentFirstName = FirstNameTextBox.Text;
            var NewStudentIndexNumber = IndexNumberTextBox.Text;
            var NewStudentAddress = AddressTextBox.Text;
            var CBI = (ComboBoxItem)StudiesComboBox.SelectedItem;

            if (CBI == null)
            {
                MessageBox.Show("Należy wybrać studia!");
                return;
            }

            var NewStudentStudies = (Study)CBI.Content;
            //List<Subject> SubjectsList = new List<Subject>();
            var SubjectsList = new HashSet<Subject>();

            foreach (Subject subject in SubjectsListBox.SelectedItems)
            {
                SubjectsList.Add(subject);
            }

            //   MessageBox.Show(Subjects.Count.ToString());


            if (NewStudentFirstName == null || NewStudentLastName == "" || NewStudentIndexNumber == "" || NewStudentStudies == null || NrIndeksuIsInvalid(NewStudentIndexNumber))
            {
                MessageBox.Show("Niewłaściwe dane");
                return;
            }

            var NewStudent = new Student
            {
                FirstName = NewStudentFirstName,
                LastName = NewStudentLastName,
                Address = NewStudentAddress,
             //   IdStudies = NewStudentStudies.IdStudies,
                IndexNumber = NewStudentIndexNumber,
                Study = NewStudentStudies,
                Subjects = SubjectsList
            };

            ListaStudentow.Add(NewStudent);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void Open(ObservableCollection<Student> listaStudentow, Student student)
        {
            this.ListaStudentow = listaStudentow;
            this.student = student;
            var SubjectsList = EfServiceDb.LoadSubjectsDataFromDb();
            SubjectsListBox.ItemsSource = SubjectsList;


            if (student == null)
            {
                ActionLabel.Content = "Dodaj Studenta";
                AddButton.Content = "Dodaj";
            }
            else
            {
                ActionLabel.Content = "Edytuj Studenta";
                AddButton.Content = "Zmień";
                LastNameTextBox.Text = student.LastName;
                FirstNameTextBox.Text = student.FirstName;
                IndexNumberTextBox.Text = student.IndexNumber;
                AddressTextBox.Text = student.Address;

                foreach (ComboBoxItem item in StudiesComboBox.Items)
                {
                    Study studies = (Study)item.Content;
                    if (studies.IdStudies == student.IdStudies)
                    {
                        StudiesComboBox.SelectedItem = item;
                    }
                }

             /*   if (student.Subjects.FirstOrDefault() != null)
                {
                    foreach (var Subject in student.Subjects)
                    {
                        //  MessageBox.Show(SubjectId.ToString());
                        //  SubjectsListBox.SelectedValue = student.Subjects;

                       // Subject subject = Subject.Extension[SubjectId];
                        SubjectsListBox.SelectedItem = Subject;


                        /* foreach (var item in SubjectsListBox.Items)
                         {
                             ListBoxItem items = (ListBoxItem) item;
                             Subject subject = (Subject)items.Content;

                             if (subject.IdSubject == SubjectId)
                                 items.IsSelected = true;
                         }*/
          //          }
         //       }

            }

            this.Show();
        }


        private bool NrIndeksuIsInvalid(String NrIndeksu)
        {
            if (NrIndeksu == null)
                return true;

            if (NrIndeksu.Length == 0)
                return true;

            if (NrIndeksu[0] != 's')
                return true;

            {
                var NrIndeksuNumber = NrIndeksu.Substring(1);
                foreach (char c in NrIndeksuNumber)
                {
                    if (c < '0' || c > '9')
                        return true;
                }
            }
            return false;
        }
    }
}








/*


    using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DeansOffice.DAL;
using DeansOffice.Models;

namespace DeansOffice
{
    /// <summary>
    /// Logika interakcji dla klasy AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private ObservableCollection<Student> ListaStudentow;
        private Student student;

        public AddStudentWindow()
        {
            InitializeComponent();
            PutValueIntoStudiesComboBoxFromDB();
            CancelButton.Click += CancelButton_Click;
            AddButton.Click += AddButton_Click;
        }

        private void PutValueIntoStudiesComboBoxFromDB()
        {
            StudentsDbService.LoadStudies();
            var StudiesExtention = Studies.Extension;

            foreach (DictionaryEntry x in StudiesExtention)
            {
                StudiesComboBox.Items.Add(new ComboBoxItem { Content = x.Value });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.student == null)
            {
                AddStudent();
            }
            else
            {
                ChangeStudent();
            }
        }

        private void ChangeStudent()
        {
            
            var NewStudentLastName = LastNameTextBox.Text;
            var NewStudentFirstName = FirstNameTextBox.Text;
            var NewStudentIndexNumber = IndexNumberTextBox.Text;
            var NewStudentAddress = AddressTextBox.Text;
            var CBI = (ComboBoxItem)StudiesComboBox.SelectedItem;

            if (CBI == null)
            {
                MessageBox.Show("Należy wybrać studia!");
                return;
            }

            var NewStudentStudies = (Studies)CBI.Content;
            List<Subject> SubjectsList = new List<Subject>();
            foreach (Subject subject in SubjectsListBox.SelectedItems)
            {
                SubjectsList.Add(subject);
            }

            //   MessageBox.Show(Subjects.Count.ToString());


            if (NewStudentFirstName == null || NewStudentLastName == "" || NewStudentIndexNumber == "" || NewStudentStudies == null || NrIndeksuIsInvalid(NewStudentIndexNumber))
            {
                MessageBox.Show("Niewłaściwe dane");
                return;
            }

            student.FirstName = NewStudentFirstName;
            student.LastName = NewStudentLastName;
            student.Address = NewStudentAddress;
            student.IdStudies = NewStudentStudies.IdStudies;
            student.IndexNumber = NewStudentIndexNumber;
            student.StudentStudies = NewStudentStudies;
            student.Subjects = SubjectsList;

            ListaStudentow[ListaStudentow.IndexOf(student)] = student;
        }

        private void AddStudent()
        {

            var NewStudentLastName = LastNameTextBox.Text;
            var NewStudentFirstName = FirstNameTextBox.Text;
            var NewStudentIndexNumber = IndexNumberTextBox.Text;
            var NewStudentAddress = AddressTextBox.Text;
            var CBI = (ComboBoxItem)StudiesComboBox.SelectedItem;

            if (CBI == null)
            {
                MessageBox.Show("Należy wybrać studia!");
                return;
            }

            var NewStudentStudies = (Studies)CBI.Content;
            List<Subject> SubjectsList = new List<Subject>();
            foreach (Subject subject in SubjectsListBox.SelectedItems)
            {
                SubjectsList.Add(subject);
            }

            //   MessageBox.Show(Subjects.Count.ToString());


            if (NewStudentFirstName == null || NewStudentLastName == "" || NewStudentIndexNumber == "" || NewStudentStudies == null || NrIndeksuIsInvalid(NewStudentIndexNumber))
            {
                MessageBox.Show("Niewłaściwe dane");
                return;
            }

            var NewStudent = new Student
            {
                FirstName = NewStudentFirstName,
                LastName = NewStudentLastName,
                Address = NewStudentAddress,
                IdStudies = NewStudentStudies.IdStudies,
                IndexNumber = NewStudentIndexNumber,
                StudentStudies = NewStudentStudies,
                Subjects = SubjectsList
            };

            ListaStudentow.Add(NewStudent);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void Open(ObservableCollection<Student> listaStudentow, Student student)
        {
            this.ListaStudentow = listaStudentow;
            this.student = student;
            var SubjectsList = StudentsDbService.getSubjectsList();
            SubjectsListBox.ItemsSource = SubjectsList;


            if (student == null)
            {
                ActionLabel.Content = "Dodaj Studenta";
                AddButton.Content = "Dodaj";
            }
            else
            {
                ActionLabel.Content = "Edytuj Studenta";
                AddButton.Content = "Zmień";
                LastNameTextBox.Text = student.LastName;
                FirstNameTextBox.Text = student.FirstName;
                IndexNumberTextBox.Text = student.IndexNumber;
                AddressTextBox.Text = student.Address;

                foreach (ComboBoxItem item in StudiesComboBox.Items)
                {
                    Studies studies = (Studies)item.Content;
                    if (studies.IdStudies == student.IdStudies)
                    {
                        StudiesComboBox.SelectedItem = item;
                    }
                }

                if (student.SubjectsId != null)
                {
                    foreach (int SubjectId in student.SubjectsId)
                    {
                        //  MessageBox.Show(SubjectId.ToString());
                        //  SubjectsListBox.SelectedValue = student.Subjects;

                        Subject subject = Subject.Extension[SubjectId];
                        SubjectsListBox.SelectedItem = subject;


                        /* foreach (var item in SubjectsListBox.Items)
                         {
                             ListBoxItem items = (ListBoxItem) item;
                             Subject subject = (Subject)items.Content;

                             if (subject.IdSubject == SubjectId)
                                 items.IsSelected = true;
                         }
                    }
                }

            }

            this.Show();
        }


private bool NrIndeksuIsInvalid(String NrIndeksu)
{
    if (NrIndeksu == null)
        return true;

    if (NrIndeksu.Length == 0)
        return true;

    if (NrIndeksu[0] != 's')
        return true;

    {
        var NrIndeksuNumber = NrIndeksu.Substring(1);
        foreach (char c in NrIndeksuNumber)
        {
            if (c < '0' || c > '9')
                return true;
        }
    }
    return false;
}
    }
}



*/