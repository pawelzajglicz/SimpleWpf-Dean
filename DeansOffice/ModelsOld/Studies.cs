using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeansOffice.ModelsOld
{
    class Studies
    {
        public int IdStudies { get; set; }
        public string Name{ get; set; }
        public static IDictionary Extension = new Dictionary<int, Studies>();


        public Studies(int dBIdStudies, string dBStudiesName)
        {
            this.IdStudies = dBIdStudies;
            this.Name = dBStudiesName;

            Extension[IdStudies] = this;
        }

        public static Studies GetStudies(int id)
        {
            return (Studies)Extension[id];
        }



        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
