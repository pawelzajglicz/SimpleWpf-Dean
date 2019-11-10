using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.ModelsOld
{
    class Subject
    {
        public int IdSubject { get; set; }
        public string Name{ get; set; }
        public static IDictionary<int, Subject> Extension = new Dictionary<int, Subject>();


        public Subject(int dBIdSubject, string dBSubjectName)
        {
            this.IdSubject = dBIdSubject;
            this.Name = dBSubjectName;

            Extension[IdSubject] = this;
        }

        public static Subject GetSubject(int id)
        {
            return (Subject)Extension[id];
        }



        public override string ToString()
        {
            return $"{Name}";
        }

        public void AddToExtension()
        {
            Extension.Add(this.IdSubject, this);
        }
    }
}
