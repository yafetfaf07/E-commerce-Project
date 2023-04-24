using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcommerceProject
{
    public partial class ProductInstance : UserControl
    {
        public Image PbImage { get; set; }
        public string PName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int ProdId { get; set; }
        public int sellerid { get; set; }

       

        int BuyerId;
        public ProductInstance(int BuyerId)
        {
            InitializeComponent();
            this.BuyerId = BuyerId;
        }

        public void AddProductInstance()
        {
            pictureBox1.Image = PbImage;
            lblProductName.Text = PName;
            lblPrice.Text = $"${Price}";
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    connection.Open();
                   // Quantity=Quantity - 1;
                    SqlCommand cd = new SqlCommand("select sellerrevenue from seller where sellerid = @sellerid", connection);
                    cd.Parameters.AddWithValue("@sellerid",sellerid);
                    double sellerrevenue = Convert.ToDouble(cd.ExecuteScalar());
                    sellerrevenue += Price;

                    cd = new SqlCommand("buy_update_product", connection);
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.Parameters.AddWithValue("@productId", ProdId);
                    //cd.Parameters.AddWithValue("@quantity", Quantity);
                    cd.Parameters.AddWithValue("@revenue", sellerrevenue);
                    cd.Parameters.AddWithValue("@sellerid", sellerid);
                    //SqlCommand cd = new SqlCommand("update product set productquantity = @quantity where productid = @prodid", connection);
                    //cd.Parameters.AddWithValue("@quantity", Quantity);
                    //cd.Parameters.AddWithValue("@prodid", ProdId);
                    //connection.Open();
                    int affectedRowsTblProduct = cd.ExecuteNonQuery();

                    cd = new SqlCommand("insert into bought values( @buyerid, @productid)", connection);
                    cd.Parameters.AddWithValue("@buyerid", BuyerId);
                    cd.Parameters.AddWithValue("@productid", ProdId);
                    int affectedRowsTblBought = cd.ExecuteNonQuery();

                  
                  

                    //cd = new SqlCommand("select sellerrevenue from seller where sellerid = @sellerid", connection);
                    //cd.Parameters.AddWithValue("@sellerid", sellerid);
                    //double sellerrevenue = Convert.ToDouble(cd.ExecuteScalar());
                    //sellerrevenue += Price;

                    //cd = new SqlCommand("update seller set sellerrevenue = @revenue where sellerid = @sellerid", connection);
                    //cd.Parameters.AddWithValue("@revenue", sellerrevenue);
                    //cd.Parameters.AddWithValue("@sellerid", sellerid);
                    int affectedRowsTblSeller = cd.ExecuteNonQuery();


                    if (affectedRowsTblProduct > 0 && affectedRowsTblBought > 0 && affectedRowsTblSeller > 0)
                    {
                        MessageBox.Show("Purchase Successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    cd = new SqlCommand("select productquantity from product where productid=@productid", connection);
                    cd.Parameters.AddWithValue("@productid", ProdId);
                    int s = Convert.ToInt32(cd.ExecuteScalar());

                    if (s < 0)
                    {
                        this.Hide();
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    if (btnAddToCart.Text == "ADD TO CART")
                    {
                        SqlCommand cd = new SqlCommand("insert into cart values( @buyerid, @productid)", connection);
                        cd.Parameters.AddWithValue("@buyerid", BuyerId);
                        cd.Parameters.AddWithValue("@productid", ProdId);

                        connection.Open();
                        int affectedRows = cd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Product Added to Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnAddToCart.Text = "REMOVE";
                        }
                    }
                    else if (btnAddToCart.Text == "REMOVE")
                    {
                        SqlCommand cd = new SqlCommand("delete cart where buyerid = @buyerid and productid = @productid", connection);
                        cd.Parameters.AddWithValue("@buyerid", BuyerId);
                        cd.Parameters.AddWithValue("@productid", ProdId);

                        connection.Open();
                        int affectedRows = cd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Product Removed from Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnAddToCart.Text = "ADD TO CART";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void checkCart(int prodId)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("select productid from cart where buyerid = @buyerid", connection);
                    cmd.Parameters.AddWithValue("@buyerid", BuyerId);

                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        if (prodId == Convert.ToInt32(sdr["productid"]))
                        {
                            btnAddToCart.Text = "REMOVE";
                        }
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
