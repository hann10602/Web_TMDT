using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Admin.Product
{
    public class UpdateModel : PageModel
    {
		public ProductEntity productEntity = new ProductEntity();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
		public List<CategoryEntity> categoryList = new List<CategoryEntity>();
		public string errorMessage = "";
		public string color = "";
		public string size = "";
		public string category = "";

		public void OnGet()
		{
			String id = Request.Query["id"];
			try
			{
				using (SqlConnection connection = ShopDBContext.GetSqlConnection())
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

					sql = "SELECT * FROM products WHERE id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{ 
								productEntity.Id = reader.GetInt32(0);
								productEntity.Name = reader.GetString(1);
								productEntity.Quantity = reader.GetInt64(2);
								productEntity.Color = reader.GetInt32(3);
								productEntity.Size = reader.GetInt32(4);
								productEntity.Thumbnail = reader.GetString(5);
								productEntity.Category = reader.GetInt32(6);
								productEntity.Price = reader.GetInt32(7);
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
				productEntity.Id = int.Parse(Request.Form["id"]);
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

				using (SqlConnection connection = ShopDBContext.GetSqlConnection())
				{

					connection.Open();
					string sql = "UPDATE products " +
                    "SET name = @name, quantity = @quantity, thumbnail =  @thumbnail, color_id = @colorId, price = @price, size_id = @sizeId, category_id = @categoryId WHERE id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", productEntity.Id);
						command.Parameters.AddWithValue("@name", productEntity.Name);
						command.Parameters.AddWithValue("@quantity", productEntity.Quantity);
						command.Parameters.AddWithValue("@thumbnail", productEntity.Thumbnail);
                        command.Parameters.AddWithValue("@price", productEntity.Price);
                        command.Parameters.AddWithValue("@categoryId", productEntity.Category);
						command.Parameters.AddWithValue("@colorId", productEntity.Color);
						command.Parameters.AddWithValue("@sizeId", productEntity.Size);
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
