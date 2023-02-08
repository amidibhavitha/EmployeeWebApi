using EmployeeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeWebApi.Controllers;
using System.Xml;
using System.Drawing;

namespace EmployeeWebApi.Controllers
{
    //[Route("api/Employee")]
    public class EmployeeController : ApiController
    {
        public EmployeeController()
        {
           loadDepartmentDropdown();
            loadOfficeLocationDropdown();

        }
       
        string conn = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public List<Department> loadDepartmentDropdown()
         {
            SqlDataReader dr = null;

            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
               cmd.CommandType= CommandType.Text;
            cmd.CommandText = "Select  DepId,DepName From Department";
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            Department dep = null;
            List<Department> deplist = new List<Department>();
            while (dr.Read())
            {
                dep = new Department();
                dep.DepId = Convert.ToInt32(dr["DepId"]);
                dep.DepName = dr["DepName"].ToString();
                deplist.Add(dep);
            }
           
            con.Close();
            return deplist;
           
        }
       
        public List<OfficeLocation> loadOfficeLocationDropdown() 
        {
            SqlDataReader dr = null;

            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
              cmd.CommandType= CommandType.Text;
            cmd.CommandText = "Select LocId, LocName from OfficeLocation";
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            OfficeLocation office = null;
            List<OfficeLocation> locationlist = new List<OfficeLocation>();
               while (dr.Read())
            {
                office = new OfficeLocation();
                office.LocId = Convert.ToInt32(dr["LocId"]);
                office.LocName = dr["LocName"].ToString();
                locationlist.Add(office);
            }
            con.Close();
            return locationlist;
        }


        [HttpGet]
        [Route("api/Employee/GetAllEmployees")]
        public List<Employee> GetAllEmployees()
        {
          
           SqlDataReader dr = null;

            SqlConnection con = new SqlConnection(conn);
            // cmd.CommandText = "Select  EmpId,Name,PnoneNumber,Gender,DepId,DepName,LocId,LocName From Employee";
            SqlCommand cmd = new SqlCommand("spGetAllEmployees", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            Employee emp = null;
            List<Employee> emplist = new List<Employee>();
            while (dr.Read())
            {
                emp = new Employee();
                emp.EmpId = Convert.ToInt32(dr["EmpId"]);
                emp.Name = dr["Name"].ToString();
                emp.PnoneNumber = dr["PnoneNumber"].ToString();
                emp.Gender = dr["Gender"].ToString();
                emp.DepName = dr["DepName"].ToString();

                emp.LocName = dr["LocName"].ToString() ;


                emplist.Add(emp);
            }

            con.Close();
            return emplist;
        }
          
              

      [HttpPost]
        [Route("api/Employee/AddEmployee")]
        public string AddEmployee(Employee emp)
        {
            string result;
           
            SqlConnection con = new SqlConnection(conn);
           // SqlCommand sqlCmd = new SqlCommand();
          //sqlCmd.CommandText = "INSERT INTO Employee (Name,PnoneNumber,Gender,DepId,LocId,) Values(@Name,@PnoneNumber,@Gender,@DepId,LocId,)";
            SqlCommand cmd = new SqlCommand("spAddNewEmpDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
           // sqlCmd.Connection = con;
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@PnoneNumber", emp.PnoneNumber);
            cmd.Parameters.AddWithValue("@Gender", emp.Gender);
            cmd.Parameters.AddWithValue("@DepId", emp.DepId);

            cmd.Parameters.AddWithValue("@LocId", emp.LocId);
            
            con.Open();
            int rowInserted = cmd.ExecuteNonQuery();
            con.Close();
            if (rowInserted == 1)
            {
                result = "Success";
            }
            else
            {
                result = "Failure";
            }
            return result;

        }
        [Route("api/Employee/GetEmployeeById/{id}")]
        [HttpGet]
        public Employee GetEmployeeById(int id)
        {
            SqlDataReader dr = null;
          SqlConnection con = new SqlConnection(conn);
         SqlCommand cmd = new SqlCommand("GetEmployeeById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpId", id);
            // cmd.CommandText = "Select EmpId,Name,Pnonenumber,Gender,DepId,DepName,LocId ,LocName from Employee where EmpId=" + id + "";
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            Employee emp = null;
            while (dr.Read())
            {
                emp= new Employee();
                emp.EmpId = Convert.ToInt32(dr["EmpId"]);
                emp.Name = dr["Name"].ToString();
                emp.PnoneNumber = dr["Pnonenumber"].ToString();
                emp.Gender = dr["Gender"].ToString();
               emp.DepId = Convert.ToInt32(dr["DepId"]);
                emp.DepName = dr["DepName"].ToString();
               emp.LocId = Convert.ToInt32(dr["LocId"]);
                emp.LocName = dr["LocName"].ToString();

            }
            return emp;

        }

        [Route("api/Employee/deletebyID/{id}")]
        [HttpDelete]
        public string DeleteByID(int id)
        {
            string result;
         SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand("DeleteEmployeeById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            // cmd.CommandText = "delete from Employee where EmpId=" + id + "";
            cmd.Parameters.AddWithValue("@EmpId", id);
            // cmd.Connection = con;
            con.Open();
            int rowDeleted = cmd.ExecuteNonQuery();
            con.Close();
            if (rowDeleted == 1)
            {
                result = "Deleted Successfully";
            }
            else
            {
                result = "failed";
            }
            return result;


        }
        [HttpPost]
        [Route("api/Employee/UpdateEmployeeDetails")]
       
        public string UpdateEmployeeDetails(Employee emp)
        {
            string result;
           SqlConnection con = new SqlConnection(conn);
           // SqlCommand sqlCmd = new SqlCommand();
            SqlCommand cmd = new SqlCommand("spUpdateEmpDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            // sqlCmd.CommandText = "Update Employee set Name=@Name,PnoneNumber=@PnoneNumber,Gender=@Gender,DepId=DepId,LocId=LocId";
           // cmd.Connection = con;
            cmd.Parameters.AddWithValue("@EmpId",emp.EmpId);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@PnoneNumber", emp.PnoneNumber);
            cmd.Parameters.AddWithValue("@Gender", emp.Gender);
            cmd.Parameters.AddWithValue("@DepId", emp.DepId);
            cmd.Parameters.AddWithValue("@LocId", emp.LocId);
            con.Open();
            int rowInserted = cmd.ExecuteNonQuery();
            con.Close();
            if (rowInserted == 1)
            {
                result = "Updated Successfully";
            }
            else
            {
                result = "failure";
            }
            return result;

        }

    }
}
