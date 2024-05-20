using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Admin.Product
{
    public class CreateModel : PageModel
    {
        public ProductEntity productEntity = new ProductEntity();
        public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
        public List<CategoryEntity> categoryList = new List<CategoryEntity>();
        public string errorMessage = "";
        public string color = "";
        public string size = "";

        public void OnGet()
        {
			try
			{
				string connectionString = @"Data Source=DESKTOP-J61PUVN;Initial Catalog=WebTMDT;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM color";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								ColorEntity colorEntity = new ColorEntity();
								colorEntity.Id = reader.GetInt32(0);
								colorEntity.Name = reader.GetString(1);
								colorEntity.Code = reader.GetString(2);
								colorList.Add(colorEntity);
							}
						}
					}

					sql = "SELECT * FROM size";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								SizeEntity sizeEntity = new SizeEntity();
								sizeEntity.Id = reader.GetInt32(0);
								sizeEntity.Name = reader.GetString(1);
								sizeEntity.Code = reader.GetString(2);
								sizeList.Add(sizeEntity);
							}
						}
                    }

                    sql = "SELECT * FROM category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryEntity categoryEntity = new CategoryEntity();
                                categoryEntity.Id = reader.GetInt32(0);
                                categoryEntity.Name = reader.GetString(1);
                                categoryEntity.Code = reader.GetString(2);
                                categoryList.Add(categoryEntity);
                            }
                        }
                    }
                }
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}

        public void OnPost()
        {
			try
			{
                productEntity.Name = Request.Form["name"];
                productEntity.Quantity = int.Parse(Request.Form["quantity"]);
                productEntity.Thumbnail = Request.Form["thumbnail"];
                productEntity.Category = int.Parse(Request.Form["category"]);
                productEntity.Color = int.Parse(Request.Form["color"]);
                productEntity.Size = int.Parse(Request.Form["size"]);
                productEntity.Price = int.Parse(Request.Form["price"]);

				if (string.IsNullOrEmpty(productEntity.Name) || productEntity.Quantity == 0
				|| string.IsNullOrEmpty(productEntity.Thumbnail)
				|| productEntity.Size == 0 || productEntity.Color == 0)
				{
					errorMessage = "All fields are required!!!";
					return;
				}

				string connectionString = @"Data Source=ALLIGATOR\ALLIGATOR;Initial Catalog=WebTMDT;Persist Security Info=True;User ID=sa; Password=123456;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{

					connection.Open();
					string sql = "INSERT INTO products" +
                    "(id, name, quantity, thumbnail, color_id, size_id, category_id, price) VALUES" +
					"(@id, @name, @quantity, @thumbnail, @colorId, @sizeId, @categoryId, @price);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						Guid originalGuid = Guid.NewGuid();
						byte[] bytes = originalGuid.ToByteArray();
						int id = BitConverter.ToInt32(bytes, 0);
						command.Parameters.AddWithValue("@id", id);
						command.Parameters.AddWithValue("@name", productEntity.Name);
						command.Parameters.AddWithValue("@quantity", productEntity.Quantity);
						command.Parameters.AddWithValue("@thumbnail", productEntity.Thumbnail);
                        command.Parameters.AddWithValue("@categoryId", productEntity.Category);
                        command.Parameters.AddWithValue("@colorId", productEntity.Color);
						command.Parameters.AddWithValue("@sizeId", productEntity.Size);
						command.Parameters.AddWithValue("@price", productEntity.Price);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

            productEntity.Name = "";
            productEntity.Quantity = 0;
            productEntity.Thumbnail = "";
            productEntity.Category = 0;
            productEntity.Color = 0;
            productEntity.Size = 0;

            Response.Redirect("/admin/product");
        }
    }
}
