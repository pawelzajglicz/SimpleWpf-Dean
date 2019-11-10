namespace DeansOffice.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("apbd.Student")]
    public partial class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStudent { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string IndexNumber { get; set; }

        public int IdStudies { get; set; }

        [ForeignKey("IdStudies")]
        public virtual Study Study { get; set; }

        public virtual ICollection<Student_Subject> Student_Subjects { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        /* public Student()
         {
             this.Subjects = new HashSet<Subject>();
         }*/
    }
}
