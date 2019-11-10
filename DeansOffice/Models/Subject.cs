namespace DeansOffice.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("apbd.Subject")]
    public partial class Subject
    {
        [Key]
        public int IdSubject { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Student_Subject> Student_Subjects { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        /* public Subject()
         {
             this.Students = new HashSet<Student>();
         }*/


        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
