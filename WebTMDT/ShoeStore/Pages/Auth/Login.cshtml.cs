using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public string errorMessage = "";
        public int userId = 0;
        public string username;
        public string password;
        public void OnPost()
        {
            try
            {
                username = Request.Form["username"];
                password = Request.Form["password"];

                if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
                {
                    errorMessage = "All fields are required!!!";
                    return;
                }

                string connectionString = "Data Source=DESKTOP-J61PUVN;Initial Catalog=WebTMDT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    string sql = "SELECT  * FROM users WHERE username = '" + username +"' AND password = '" + password + "'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                            }
                        }
                    }

                    if(userId != 0)
                    {
                        sql = "UPDATE login SET user_id = @userId, active = 1 WHERE id = 1";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@userId", userId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            if (userId == 0)
            {
                Response.Redirect("/auth/login");
            }
            else
            {
                Response.Redirect("/");
            }
        }
    }
}
