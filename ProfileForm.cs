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
    public partial class ProfileForm : Form
    {

        string userName;
        bool fromBuyer;

        Seller seller = new Seller();
        Buyer buyer = new Buyer();

        public ProfileForm(string userName, bool fromBuyer)
        {
            InitializeComponent();
            this.userName = userName;
            this.fromBuyer = fromBuyer;
        }

        private int getSellerId(string userName)
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand($"select sellerid from seller where username = @username", con);
            cmd.Parameters.AddWithValue("@username", userName);
            con.Open();
            int sellerId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return sellerId;
        }

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

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    if (fromBuyer)
                    {
                        #region Buyer Info From Database
                        SqlCommand cmd = new SqlCommand("select * from buyer where username = @username", connection);
                        cmd.Parameters.AddWithValue("@username", userName);
                        connection.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();

                        while (sdr.Read())
                        {
                            txtFirstName.Text = Convert.ToString(sdr["buyerfirstname"]);
                            txtLastName.Text = Convert.ToString(sdr["buyerlastname"]);
                            txtUserName.Text = Convert.ToString(sdr["username"]);
                            txtCreditCard.Text = Convert.ToString(sdr["buyercreditcard"]);
                            txtPassword.Text = Convert.ToString(sdr["password"]);
                            txtEmail.Text = Convert.ToString(sdr["email"]);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Seller Info From Database
                        SqlCommand cmd = new SqlCommand("select * from seller where username = @username", connection);
                        cmd.Parameters.AddWithValue("@username", userName);
                        connection.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();

                        while (sdr.Read())
                        {
                            txtFirstName.Text = Convert.ToString(sdr["sellerfirstname"]);
                            txtLastName.Text = Convert.ToString(sdr["sellerlastname"]);
                            txtUserName.Text = Convert.ToString(sdr["username"]);
                            txtCreditCard.Text = Convert.ToString(sdr["sellercreditcard"]);
                            txtPassword.Text = Convert.ToString(sdr["password"]);
                            txtEmail.Text = Convert.ToString(sdr["email"]);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (fromBuyer)
            {
                buyer.BuyerFirstName = txtFirstName.Text;
                buyer.BuyerLastName = txtLastName.Text;
                buyer.BuyerCreditCard = txtCreditCard.Text;
                buyer.BuyerUserName = txtUserName.Text;
                buyer.BuyerPassword = txtPassword.Text;
                buyer.BuyerEmail = txtEmail.Text;
                buyer.BuyerId = getBuyerId(userName);
                buyer.updateBuyer();
                new HomePageForm(txtUserName.Text).Show();
                this.Hide();
            }
            else
            {
                seller.SellerFirstName = txtFirstName.Text;
                seller.SellerLastName = txtLastName.Text;
                seller.SellerCreditCard = txtCreditCard.Text;
                seller.SellerUserName = txtUserName.Text;
                seller.SellerPassword = txtPassword.Text;
                seller.SellerEmail = txtEmail.Text;
                seller.SellerId = getSellerId(userName);
                seller.updateSeller();
                new SellerForm(txtUserName.Text).Show();
                this.Hide();
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (fromBuyer)
            {
                buyer.BuyerId = getBuyerId(userName);
                buyer.deleteBuyer();
            }
            else
            {
                seller.SellerId = getSellerId(userName);
                seller.deleteSeller();
            }

            new SignInForm().Show();
            this.Hide();
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                MessageBox.Show("Password Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConfirmPassword.Text = "";
            }
        }

        private void picView1_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '\0';
        }

        private void picView1_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '●';
        }

        private void ProfileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fromBuyer)
                new HomePageForm(userName).Show();
            else
                new SellerForm(userName).Show();
        }
    }
}
