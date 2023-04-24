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
    public partial class SellerForm : Form
    {
        Product product = new Product();

        public string userName;
        DropDownMenu dropDownMenu;
        public SellerForm(string sellerUserName)
        {
            InitializeComponent();
            dropDownMenu = new DropDownMenu(this, false);
            userName = sellerUserName;
            showProducts();
        }

        private void SellerForm_Load(object sender, EventArgs e)
        {
            lblUserName.Text = userName;
        }

        private int getSellerId(string userName)
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand($"select sellerid from seller where username = '{userName}'", con);
            con.Open();
            int sellerId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return sellerId;
        }

        private void showProducts()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("select productid, productname, productcategory," +
                        $" productprice, productquantity from product where sellerid = {getSellerId(userName)}", connection);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dgvProductList.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtProductName.Text) == false && String.IsNullOrWhiteSpace(txtPrice.Text) == false &&
                 String.IsNullOrWhiteSpace(txtQuantity.Text) == false)
            {
                MemoryStream stream = new MemoryStream();
                pbImage.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] pic = stream.ToArray();
                product.ProductImage = Image.FromStream(stream);
                product.ProductName = txtProductName.Text;
                product.ProductCategory = CmbCategory.Text;
                product.ProductPrice = float.Parse(txtPrice.Text);
                product.ProductQuantity = int.Parse(txtQuantity.Text);
                product.SellerId = getSellerId(userName);
                product.AddProduct();
                showProducts();

                txtProductName.Text = "";
                txtPrice.Text = "";
                txtQuantity.Text = "";
            }
            else
            {
                MessageBox.Show("Please fill in every form");
            }
        }

        int productId = 0;
        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int rowIndex = dgvProductList.CurrentCell.RowIndex;

            txtProductName.Text = dgvProductList.Rows[rowIndex].Cells[1].Value.ToString();
            CmbCategory.Text = dgvProductList.Rows[rowIndex].Cells[2].Value.ToString();
            txtPrice.Text = dgvProductList.Rows[rowIndex].Cells[3].Value.ToString();
            txtQuantity.Text = dgvProductList.Rows[rowIndex].Cells[4].Value.ToString();

            productId = Convert.ToInt32(dgvProductList.Rows[rowIndex].Cells[0].Value.ToString());
            getProductImage(productId);

        }

        private void getProductImage(int productId)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(ConString);
                SqlCommand cmd = new SqlCommand($"select productimage from product where productid = {productId}", con);
                con.Open();
                byte[] picbyte = cmd.ExecuteScalar() as byte[] ?? null;
                MemoryStream mstream = new MemoryStream(picbyte);
                pbImage.Image = Image.FromStream(mstream);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtProductName.Text) == false && String.IsNullOrWhiteSpace(txtPrice.Text) == false &&
                String.IsNullOrWhiteSpace(txtQuantity.Text) == false)
            {
                MemoryStream stream = new MemoryStream();
                pbImage.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] pic = stream.ToArray();
                product.ProductName = txtProductName.Text;
                product.ProductCategory = CmbCategory.Text;
                product.ProductPrice = Convert.ToDouble(txtPrice.Text);
                product.ProductImage = Image.FromStream(stream);
                product.ProductQuantity = Convert.ToInt32(txtQuantity.Text);
                product.ProductId = productId;
                product.updateProduct();
                showProducts();

                txtProductName.Text = "";
                txtPrice.Text = "";
                txtQuantity.Text = "";
            }
            else
            {
                MessageBox.Show("Please fill in every form");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            product.ProductId = productId;
            product.deleteProduct();
            showProducts();

            txtProductName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image File(*.jpe; *.jpeg; *jpeg; *.bmp) | *.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        string FileName = openFileDialog.FileName;
                        string imagepathtext = FileName;
                        pbImage.Load(FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void picAccount_MouseHover(object sender, EventArgs e)
        {
            Controls.Add(dropDownMenu);
            dropDownMenu.Location = new Point(848, 76);
            dropDownMenu.Margin = new Padding(0);
            dropDownMenu.Size = new Size(82, 102);
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

        private void SellerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
