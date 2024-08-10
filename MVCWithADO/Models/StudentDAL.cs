using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MVCWithADO.Models
{
    public class StudentDAL
    {
        SqlConnection con; //declaring/defineing
        SqlCommand cmd; //declaring/defining

        public StudentDAL()  //create constructor
        {
            string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString; //it will read the connectionstring
            con = new SqlConnection(ConStr); //it will create the connection object
            cmd = new SqlCommand(); //it will create command object 
            cmd.Connection = con; // binds connection and command with each other
            cmd.CommandType = CommandType.StoredProcedure; //tells the command -need to call a stored procedure makes the command ready
            //cmd.CommandText = "";
        }
        public List<Student> SelectStudents(int? Sid, bool? Status)
        {
            List<Student> students = new List<Student>();
            try
            {
                cmd.CommandText = "Student_Select";
                cmd.Parameters.Clear();
                if (Sid != null && Status != null)
                {
                    cmd.Parameters.AddWithValue("@Sid", Sid); //calling status parameters
                    cmd.Parameters.AddWithValue("@Status", Status); //calling status parameters
                }
                else if (Sid != null && Status == null)
                    cmd.Parameters.AddWithValue("@Sid", Sid);//calling status parameters
                else if (Sid == null && Status != null)
                    cmd.Parameters.AddWithValue("@Status", Status);//calling status parameters
                con.Open(); //need to open the connection
                SqlDataReader dr = cmd.ExecuteReader();// executerreader will executing storedprocedure and load the data in sqldatareader

                while (dr.Read()) //read each data and send by while loop for multiple records
                {
                    Student student = new Student //create new empty student instance for student
                    {
                        Sid = Convert.ToInt32(dr["Sid"]), //this is object, so convert it into data type accordingly 
                        Name = dr["Name"].ToString(),
                        Class = Convert.ToInt32(dr["Class"]),
                        Fees = Convert.ToDecimal(dr["Fees"]),
                        Photo = dr["Photo"].ToString()
                     };
                    students.Add(student);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return students;
        }
        public int InsertStudent(Student student)
        {
            int Count = 0;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "Student_Insert";
                cmd.Parameters.AddWithValue("@Sid", student.Sid);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                if(student.Photo != null && student.Photo.Length!=0) 
                cmd.Parameters.AddWithValue("@Photo", student.Photo);
                con.Open();
               Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close() ;
            }

            return Count;
        }
        public int UpdateStudent(Student student)
        {
            int Count = 0;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "Student_Update";
                cmd.Parameters.AddWithValue("@Sid", student.Sid);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                cmd.Parameters.AddWithValue("@Photo", student.Photo);
                con.Open();
                Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return Count;
        }
        public int DeleteStudent(int Sid)
        {
            int Count = 0;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "Student_Delete";
                cmd.Parameters.AddWithValue("@Sid", Sid);
                con.Open();
                Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return Count;
        }
    }
}