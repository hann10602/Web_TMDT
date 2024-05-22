using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Data.SqlClient;

namespace ShoeStore.Pages
{
    public class IndexModel : PageModel
    {
		public List<ProductDTO> productList = new List<ProductDTO>();
		public List<ColorEntity> colorList = new List<ColorEntity>();
		public List<SizeEntity> sizeList = new List<SizeEntity>();
		public void OnGet()
        {
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
                                productDTO.Price = reader.GetInt32(7);
                                productList.Add(productDTO);
							}
						}
					}

					connection.Close();
				}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
        public int Price { get; set; }
        public string Thumbnail { get; set; }
        public int Category { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
    }

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
        public int Price { get; set; }

        public string Thumbnail { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
    public class UserEntity
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

    }
    public class BillEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }

	}
    public class ColorEntity
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
	}
	public class SizeEntity
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
    }
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}