using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HttpAPIConsumer.Models
{
    public class Faculty
    {
        // [Key]
        public string Facultyid { get; set; } = Guid.NewGuid().ToString();// { get; set; }
        public string Facultyname{ get; set; }

        //public IEnumerable<Department> Depts { get; set; }
        //public IEnumerable<Student> Students { get; set; }
    }
}
