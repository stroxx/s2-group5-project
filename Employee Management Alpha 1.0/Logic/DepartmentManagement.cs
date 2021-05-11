﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace Employee_Management_Alpha_1._0
{
    static class DepartmentManagement
    {
        public static void CreateDepartment(string name, string headOfDepartment, string address, int phone, string email, string language)
        {
            DepartmentDAL.CreateDepartment(name,headOfDepartment,address,phone,email,language);
        }
        public static Department GetDepartmentByID(int id)
        {
            return DepartmentDAL.GetDepartmentByID(id);
        }
        public static List<Department> GetAllDepartments()
        {
            return DepartmentDAL.GetAllDepartments();
        }
        public static List<Department> GetAllActiveDepartments()
        {
            return DepartmentDAL.GetAllActiveDepartments();
        }
        public static void UpdateDepartment(int id, string name, string headOfDepartment, string address, int phone, string email, string language)
        {
        }
        public static void DeleteDepartment(int id)
        {
            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_insert = $@"DELETE FROM department WHERE ID = @ID;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_insert, conn);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", id); 
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void AssignEmployee(Employee e, Department d)
        {
            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_insert = $@"INSERT INTO depemp (Dep, EmpID) 
                                VALUES (@DepID, @EmpID);";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_insert, conn);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@DepID", d.Id);
                    cmd.Parameters.AddWithValue("@EmpID", e.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void UnassignEmployee(Employee e)
        {
            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_insert = $@"DELETE FROM depemp WHERE EmpID = @EmpID;"; 

            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_insert, conn);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@EmpID", e.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static List<Employee> GetAllAvailableEmployeesForDepartment(Department d)
        {
            EmployeeManagement employee_Management = new EmployeeManagement();
            List<Employee> employees = employee_Management.GetAllActiveEmployees();
            List<int> IDsOfEmployees = new List<int>();

            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_select = $@"SELECT EmpID FROM depemp WHERE Dep = @ID;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_select, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID", d.Id);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        IDsOfEmployees.Add(Convert.ToInt32(dr["EmpID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            for (int i = 0; i < employees.Count(); i++)
            {//check to see if indexes roll back with 1 when u remove item and adjust by i-- when removed
                if (IDsOfEmployees.Contains(employees[i].Id))
                {
                    employees.Remove(employees[i]);
                    i--;
                }
            }

            return employees;
        }
        public static List<Employee> GetAllAssignedEmployeesForDepartment(Department d)
        {
            EmployeeManagement employee_Management = new EmployeeManagement();
            List<Employee> employees = employee_Management.GetAllActiveEmployees();
            List<int> IDsOfEmployees = new List<int>();

            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_select = $@"SELECT EmpID FROM depemp WHERE Dep = @ID;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_select, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID", d.Id);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        IDsOfEmployees.Add(Convert.ToInt32(dr["EmpID"]));
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            for (int i = 0; i < employees.Count(); i++)
            {//check to see if indexes roll back with 1 when u remove item and adjust by i-- when removed
                if (!IDsOfEmployees.Contains(employees[i].Id))
                {
                    employees.Remove(employees[i]);
                    i--;
                }
            }

            return employees;
        }
        public static void ChangeStatus(Department d)
        {
            string sql_connection = "server=studmysql01.fhict.local;database=dbi456096;uid=dbi456096;password=logixtic;";
            string sql_insert = $@"UPDATE department SET Status = @Status WHERE ID = @ID;";

            int changedStatus;
            if (d.Status == 0)
                changedStatus = 1;
            else
                changedStatus = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(sql_connection))
                {
                    MySqlCommand cmd = new MySqlCommand(sql_insert, conn);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", d.Id);
                    cmd.Parameters.AddWithValue("@Status", changedStatus);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
