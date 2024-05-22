using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int userId = 0;
        public string username;
        public string password;

        public void OnPost()
        {
            try
            {
                username = Request.Form["username"];
                password = Request.Form["password"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ErrorMessage = "All fields are required!!!";
                    return;
                }

                using (SqlConnection connection = ShopDBContext.GetSqlConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM users WHERE username = @username AND password = @password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                            }
                        }
                    }

                    if (userId != 0)
                    {
                        sql = "UPDATE login SET user_id = @userId, active = 1 WHERE id = 1";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@userId", userId);
                            command.ExecuteNonQuery();
                        }
                        SuccessMessage = "Login successful!";
                        Response.Redirect("/");
                    }
                    else
                    {
                        ErrorMessage = "Invalid username or password.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your request.";
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
