using DeansOffice.DAL;
using DeansOffice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeansOffice
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Student> ListaStudentow;

        public MainWindow()
        {
            InitializeComponent();
            LoadIntoGridBoxDataFromDB();
            StudentsDataGrid.SelectedCellsChanged += StudentsDataGrid_SelectedCellsChanged;
            RemoveButton.Click += RemoveButton_Click;
            AddButton.Click += AddButton_Click;
            ChangeButton.Click += ChangeButton_Click;
            ListaStudentow.CollectionChanged += ListaStudentow_CollectionChanged;
        }

        private void ListaStudentow_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var NewStudent = (Student) e.NewItems[0];
                try
                {
                    EfServiceDb.AddStudent(NewStudent);
                } catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    MessageBox.Show("Nie udało się dodać nowego studenta");
                }
                    
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                var ModifiedStudent = (Student)e.NewItems[0];
                try
                {
                    EfServiceDb.UpdateStudent(ModifiedStudent);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    MessageBox.Show("Nie udało się zedytować studenta");
                }
                StudentsDataGrid.Items.Refresh();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var Window = new AddStudentWindow();
            Window.Open(ListaStudentow, null);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Nie wybrano studentów do usunięcia.");
                return;
            }

            var SelectedStudents = StudentsDataGrid.SelectedItems;

            List<int> SelectedStudentsId = new List<int>();
            List<Student> Students = new List<Student>();

            foreach (var SelectedStudent in SelectedStudents)
            {
                Students.Add((Student)SelectedStudent);
            }

            var window = new ConfirmRemoveWindow();

            window.Students = Students;
            window.StudentsList = (ObservableCollection<Student>)StudentsDataGrid.ItemsSource;

            window.Show();
        }

        private void StudentsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int SelectedRowsNumber = StudentsDataGrid.SelectedCells.Count / StudentsDataGrid.Columns.Count;

            string Ending;

            if (SelectedRowsNumber == 1)
                Ending = "a.";
            else
                Ending = "ów.";

            SelectedCounterLabel.Content = "Wybrałeś " + SelectedRowsNumber + " student" + Ending;
        }

        private void LoadIntoGridBoxDataFromDB()
        {
            ListaStudentow = EfServiceDb.LoadStudentsDataFromDb();
            
            StudentsDataGrid.ItemsSource = ListaStudentow;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var Window = new AddStudentWindow();
            Student SelectedStudent = null;
            var SelectedStudents = StudentsDataGrid.SelectedItems;

            if (SelectedStudents.Count == 0)
            {
                MessageBox.Show("Nie wybrano żadnego studenta!");
                return;
            } else
            if (SelectedStudents.Count > 1)
            {
                MessageBox.Show("Wybrano zbyt wielu studentów!");
                return;
            }
            else
            {
                SelectedStudent = (Student)StudentsDataGrid.SelectedItem;
            }

            Window.Open(ListaStudentow, SelectedStudent);
        }
    }
}

















/*


    using DeansOffice.DAL;
using DeansOffice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeansOffice
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Student> ListaStudentow;

        public MainWindow()
        {
            InitializeComponent();
            LoadIntoGridBoxDataFromDB();
            StudentsDataGrid.SelectedCellsChanged += StudentsDataGrid_SelectedCellsChanged;
            RemoveButton.Click += RemoveButton_Click;
            AddButton.Click += AddButton_Click;
            ChangeButton.Click += ChangeButton_Click;
            ListaStudentow.CollectionChanged += ListaStudentow_CollectionChanged;
        }

        private void ListaStudentow_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var NewStudent = (Student) e.NewItems[0];
                try
                {
                    StudentsDbService.AddStudent(NewStudent);
                } catch (Exception exc)
                {
                    //    ListaStudentow.Remove(NewStudent);
                    MessageBox.Show(exc.Message);
                    MessageBox.Show("Nie udało się dodać nowego studenta");
                }
                    
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                var ModifiedStudent = (Student)e.NewItems[0];
                try
                {
                    StudentsDbService.UpdateStudent(ModifiedStudent);
                }
                catch (Exception exc)
                {
                    //    ListaStudentow.Remove(NewStudent);
                    MessageBox.Show(exc.Message);
                    MessageBox.Show("Nie udało się zedytować studenta");
                }
                StudentsDataGrid.Items.Refresh();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var Window = new AddStudentWindow();
            Window.Open(ListaStudentow, null);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedCells.Count == 0)
            {
                MessageBox.Show("Nie wybrano studentów do usunięcia.");
                return;
            }

            var SelectedStudents = StudentsDataGrid.SelectedItems;

            //var SelectedStudents = StudentsDataGrid.SelectedItems.Cast<Student>.ToList();;

            List<int> SelectedStudentsId = new List<int>();

            foreach (var SelectedStudent in SelectedStudents)
            {
                SelectedStudentsId.Add(((Student)SelectedStudent).IdStudent);
            }

            var window = new ConfirmRemoveWindow();
            window.SelectedStudentsId = SelectedStudentsId;
            window.StudentsList = (ObservableCollection<Student>)StudentsDataGrid.ItemsSource;
            window.Show();
        }

        private void StudentsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int SelectedRowsNumber = StudentsDataGrid.SelectedCells.Count / StudentsDataGrid.Columns.Count;

            string Ending;

            if (SelectedRowsNumber == 1)
                Ending = "a.";
            else
                Ending = "ów.";

            SelectedCounterLabel.Content = "Wybrałeś " + SelectedRowsNumber + " student" + Ending;
        }

        private void LoadIntoGridBoxDataFromDB()
        {
            ListaStudentow = StudentsDbService.LoadStudentsDataFromDb();
            StudentsDataGrid.ItemsSource = ListaStudentow;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var Window = new AddStudentWindow();
            Student SelectedStudent = null;
            var SelectedStudents = StudentsDataGrid.SelectedItems;

            if (SelectedStudents.Count == 0)
            {
                MessageBox.Show("Nie wybrano żadnego studenta!");
                return;
            } else
            if (SelectedStudents.Count > 1)
            {
                MessageBox.Show("Wybrano zbyt wielu studentów!");
                return;
            }
            else
            {
                SelectedStudent = (Student)StudentsDataGrid.SelectedItem;
            }

            Window.Open(ListaStudentow, SelectedStudent);
        }
    }
}


*/
