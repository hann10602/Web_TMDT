using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeStore.DB;
using System.Data.SqlClient;

namespace ShoeStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        public List<ProductDTO> productList = new List<ProductDTO>();
        public List<ColorEntity> colorList = new List<ColorEntity>();
        public List<SizeEntity> sizeList = new List<SizeEntity>();
        public CategoryEntity categoryEntity = new CategoryEntity();
        public void OnGet()
        {
            String category = Request.Query["category"];
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

                    sql = "SELECT * FROM category WHERE code = '" + category + "'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryEntity.Id = reader.GetInt32(0);
                            }
                        }
                    }

                    sql = "SELECT * FROM products WHERE category_id = " + categoryEntity.Id;
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
                                productDTO.Price = reader.GetInt32(6);
                                productList.Add(productDTO);
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
        }
    }
}
