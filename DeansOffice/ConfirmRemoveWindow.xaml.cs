using DeansOffice.DAL;
using DeansOffice.Models;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeansOffice
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class ConfirmRemoveWindow : Window
    {
        internal ObservableCollection<Student> StudentsList;
        public List<int> SelectedStudentsId { get; set; }

        public List<Student> Students { get; set; }

        public ConfirmRemoveWindow()
        {
            InitializeComponent();
            CancelButton.Click += CancelButton_Click;
            ConfirmButton.Click += ConfirmButton_Click;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var successed = EfServiceDb.RemoveStudents(Students);
            
            if (successed)
            {

                foreach (var Student in Students)
                {
                    StudentsList.Remove(Student);
                }

            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
