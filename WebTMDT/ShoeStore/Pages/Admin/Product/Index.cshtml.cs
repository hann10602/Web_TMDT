using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Drawing;

namespace ShoeStore.Pages.Admin.Product
{
    public class IndexModel : PageModel
	{
		public List<ProductDTO> productList = new List<ProductDTO>();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
        public List<CategoryEntity> categoryList = new List<CategoryEntity>();
		public int userId;
        public int role;
        public void OnGet()
        {
			try
			{
				string connectionString = @"Data Source=ALLIGATOR\ALLIGATOR;Initial Catalog=WebTMDT;Persist Security Info=True;User ID=sa; Password=123456;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM login";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
                                userId = reader.GetInt32(1);
							}
						}
					}

                    sql = "SELECT * FROM users WHERE id = " + userId;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                role = reader.GetInt32(5);
                            }
                        }
                    }

                    if (role == 2)
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

                        sql = "SELECT * FROM products";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ProductDTO productDTO = new ProductDTO();
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

                                    foreach (CategoryEntity categoryEntity in categoryList)
                                    {
                                        int sizeId = reader.GetInt32(6);
                                        if (categoryEntity.Id == sizeId)
                                        {
											productDTO.Category = categoryEntity.Name;
                                        }
                                    }
									productDTO.Price = reader.GetInt32(7);
                                    productList.Add(productDTO);
                                }
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

            if(role != 2)
            {
                Response.Redirect("/");
            }
		}
    }
}
