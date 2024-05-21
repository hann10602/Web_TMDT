using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public List<UserEntity> userList = new List<UserEntity>();
        public UserEntity userEntity = new UserEntity();
        public string errorMessage = "";
        public void OnPost()
        {
            try
            {
                userEntity.Fullname = Request.Form["fullname"];
                userEntity.Username = Request.Form["username"];
                userEntity.Password = Request.Form["password"];

                if (string.IsNullOrEmpty(userEntity.Fullname) || string.IsNullOrEmpty(userEntity.Username)
                || string.IsNullOrEmpty(userEntity.Password))
                {
                    errorMessage = "All fields are required!!!";
                    return;
                }

                using (SqlConnection connection = ShopDBContext.GetSqlConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM users WHERE username = '" + userEntity.Username + "'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userEntity.Id = reader.GetInt32(0);
                                userList.Add(userEntity);
                            }
                        }
                    }
                    
                    if(userList.Count == 0)
                    {
                        sql = "INSERT INTO users" +
                        "(id, fullname, username, password, role_id) VALUES" +
                        "(@id, @fullname, @username, @password, 1);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            Guid originalGuid = Guid.NewGuid();
                            byte[] bytes = originalGuid.ToByteArray();
                            int id = BitConverter.ToInt32(bytes, 0);
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@fullname", userEntity.Fullname);
                            command.Parameters.AddWithValue("@username", userEntity.Username);
                            command.Parameters.AddWithValue("@password", userEntity.Password);
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


            userEntity.Fullname = "";
            userEntity.Username = "";
            userEntity.Password = "";

            if (userList.Count == 0)
            {
                Response.Redirect("/auth/login");
            }
            else
            {
                Response.Redirect("/auth/register");
            }
        }
    }
}
