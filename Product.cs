using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcommerceProject
{
    internal class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; }
        public Image ProductImage { get; set; }
        public int ProductQuantity { get; set; }
        public int SellerId { get; set; }

        public void AddProduct()
        {
            MemoryStream stream = new MemoryStream();
            ProductImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] pic = stream.ToArray();
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cd = new SqlCommand("sp_addproduct", connection);
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.Parameters.AddWithValue("@productimage", pic);
                    cd.Parameters.AddWithValue("@productname", ProductName);
                    cd.Parameters.AddWithValue("@productcategory", ProductCategory);
                    cd.Parameters.AddWithValue("@productprice", ProductPrice);
                    cd.Parameters.AddWithValue("@productquantity", ProductQuantity);
                    cd.Parameters.AddWithValue("@sellerid", SellerId);

                    connection.Open();
                    cd.ExecuteNonQuery();
                    MessageBox.Show("Product has been added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void updateProduct()
        {
            MemoryStream stream = new MemoryStream();
            ProductImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] pic = stream.ToArray();
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("sp_updateproduct", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productname", ProductName);
                    cmd.Parameters.AddWithValue("@category", ProductCategory);
                    cmd.Parameters.AddWithValue("@price", ProductPrice);
                    cmd.Parameters.AddWithValue("@image", pic);
                    cmd.Parameters.AddWithValue("@quantity", ProductQuantity);
                    cmd.Parameters.AddWithValue("@prodid", ProductId);

                    connection.Open();
                    int rowAffected = cmd.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Update Successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void deleteProduct()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("delete from product where productid = @prodid", connection);
                    cmd.Parameters.AddWithValue("@prodid", ProductId);

                    connection.Open();
                    int rowAffected = cmd.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Deletion Successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
