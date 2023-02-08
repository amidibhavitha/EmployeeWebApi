using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Models
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string PnoneNumber { get; set; }
        public string Gender { get; set; }
        public int DepId { get; set; }
        public string DepName { get; set; }
        public int LocId { get; set; }
        public string LocName { get; set; }
    }
}