using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class HomePageForm : Form
    {

        public string userName;
        DropDownMenu dropDownMenu;

        public HomePageForm(string BuyerUserName)
        {
            InitializeComponent();
            MaximizeBox = false;
            dropDownMenu = new DropDownMenu(this, true);
            this.userName = BuyerUserName;
        }


        private void picAccount_MouseHover(object sender, EventArgs e)
        {
            Controls.Add(dropDownMenu);
            dropDownMenu.Location = new Point(930, 73);
            dropDownMenu.Margin = new Padding(0);
            dropDownMenu.Size = new Size(112, 133);
            dropDownMenu.BringToFront();
            dropDownMenu.Show();
        }

        private void picAccount_MouseLeave(object sender, EventArgs e)
        {
            if (ClientRectangle.Contains(dropDownMenu.PointToClient(MousePosition)))
                return;
            else
                Controls.Remove(dropDownMenu);
        }

        ProductInstance productInstance;


        private int getBuyerId(string userName)
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("select buyerid from buyer where username = @username", con);
            cmd.Parameters.AddWithValue("@username", userName);
            con.Open();
            int buyerId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return buyerId;
        }

        private void showproducts(string Query)
        {
            List<Product> products = new List<Product>();
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand(Query, connection);
                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        byte[] picbyte = sdr["productimage"] as byte[] ?? null;
                        MemoryStream mstream = new MemoryStream(picbyte);
                        products.Add(new Product()
                        {
                            ProductImage = Image.FromStream(mstream),
                            ProductId = Convert.ToInt32(sdr["productid"]),
                            ProductName = Convert.ToString(sdr["productname"]),
                            ProductPrice = Convert.ToDouble(sdr["productprice"]),
                            ProductQuantity = Convert.ToInt32(sdr["productquantity"]),
                            SellerId = Convert.ToInt32(sdr["sellerid"])
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (Product product in products)
            {
                productInstance = new ProductInstance(getBuyerId(userName));
                productInstance.PbImage = product.ProductImage;
                productInstance.ProdId = product.ProductId;
                productInstance.PName = product.ProductName;
                productInstance.Price = product.ProductPrice;
                productInstance.Quantity = product.ProductQuantity;
                productInstance.sellerid = product.SellerId;
                if (product.ProductQuantity > 0)
                {
                    productInstance.AddProductInstance();
                    productInstance.checkCart(product.ProductId);
                    flowLayoutPanel1.Controls.Add(productInstance);
                }
            }

        }

        private void HomePageForm_Load(object sender, EventArgs e)
        {
            lblUserName.Text = userName;
            showproducts("select * from product");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            showproducts("select * from productDescription");
        }

        private void btnClothing_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            showproducts(" select * from productcategoryClothing ");
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            showproducts("select * from productcategoryBooks");
        }

        private void btnElectronics_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            showproducts("select * from productcategoryElectronics");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Product> products = new List<Product>();
            flowLayoutPanel1.Controls.Clear();

            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("select * from [productDescription]", connection);
                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        byte[] picbyte = sdr["productimage"] as byte[] ?? null;
                        MemoryStream mstream = new MemoryStream(picbyte);
                        products.Add(new Product()
                        {
                            ProductImage = Image.FromStream(mstream),
                            ProductName = Convert.ToString(sdr["productname"]),
                            ProductPrice = Convert.ToDouble(sdr["productprice"]),
                            ProductQuantity = Convert.ToInt32(sdr["productquantity"])
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (Product product in products)
            {
                if (product.ProductName.Contains(txtSearch.Text))
                {
                    productInstance = new ProductInstance(getBuyerId(userName));
                    productInstance.PbImage = product.ProductImage;
                    productInstance.PName = product.ProductName;
                    productInstance.Price = product.ProductPrice;
                    productInstance.Quantity = product.ProductQuantity;
                    if (product.ProductQuantity > 0)
                    {
                        productInstance.AddProductInstance();
                        productInstance.checkCart(product.ProductId);
                        flowLayoutPanel1.Controls.Add(productInstance);
                    }
                }
            }
        }

        public void showCart()
        {
            int buyerId = getBuyerId(userName);
            string query = "select * from product where productid = 0";
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("select productid from cart where buyerid = @buyerid", connection);
                    cmd.Parameters.AddWithValue("@buyerid", buyerId);

                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        query = query + $" or productid = {Convert.ToInt32(sdr["productid"])}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            flowLayoutPanel1.Controls.Clear();
            Label lblCartItems = new Label();
            lblCartItems.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblCartItems.Location = new Point(3, 0);
            lblCartItems.Padding = new Padding(0, 10, 700, 0);
            lblCartItems.Size = new Size(792, 34);
            lblCartItems.Text = "Cart Items";
            flowLayoutPanel1.Controls.Add(lblCartItems);
            showproducts(query);
        }

        private void HomePageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
