using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using ShoeStore.DB;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Product
{
    public class IndexModel : PageModel
    {
        public ProductDTO productDTO = new ProductDTO();
        public List<ColorEntity> colorList = new List<ColorEntity>();
        public List<SizeEntity> sizeList = new List<SizeEntity>();
        public CategoryEntity categoryEntity = new CategoryEntity();
		public int isLogin;
		public void OnGet()
		{
			String productId = Request.Query["id"];
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
								isLogin = reader.GetInt32(2);
							}
						}
					}
					if (isLogin == 0)
					{

					}
					else
					{
						sql = "SELECT * FROM color";
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

						sql = "SELECT * FROM products WHERE id = " + productId;
						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									productDTO.Id = reader.GetInt32(0);
									productDTO.Name = reader.GetString(1);
									productDTO.Quantity = reader.GetInt64(2);
									foreach (ColorEntity colorEntity in colorList)
									{
										int colorId = reader.GetInt32(3);
										if (colorEntity.Id == colorId)
										{
											productDTO.Color = colorEntity.Name;
										}
									}
									foreach (SizeEntity sizeEntity in sizeList)
									{
										int sizeId = reader.GetInt32(4);
										if (sizeEntity.Id == sizeId)
										{
											productDTO.Size = sizeEntity.Name;
										}
									}
									productDTO.Thumbnail = reader.GetString(5);
                                    productDTO.Price = reader.GetInt32(7);
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
