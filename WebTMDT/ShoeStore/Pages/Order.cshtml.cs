using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace ShoeStore.Pages
{
    public class OrderModel : PageModel
    {
        public List<BillEntity> billList = new List<BillEntity>();
        public string fullname;
        public int userId;
        public int isLogin;
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = ShopDBContext.GetSqlConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM login WHERE id = 1";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userId = reader.GetInt32(1);
                                isLogin = reader.GetInt32(2);
                            }
                        }
                    }
                    if (isLogin == 0)
                    {

                    }
                    else
                    {
                        sql = "SELECT * FROM users WHERE id = " + userId;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    fullname = reader.GetString(1);
                                }
                            }
                        }

                        sql = "SELECT b.id, p.name, b.status, b.quantity FROM bill b LEFT JOIN products p ON b.product_id = p.id WHERE b.user_id = " + userId;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BillEntity billEntity = new BillEntity();
                                    billEntity.Id = reader.GetInt32(0);
                                    billEntity.ProductName = reader.GetString(1);
									billEntity.Status = reader.GetInt32(2);
                                    billEntity.Quantity = reader.GetInt32(3);
									billList.Add(billEntity);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            if (isLogin == 0)
            {
                Response.Redirect("/auth/login");
            }
        }
    }
}
