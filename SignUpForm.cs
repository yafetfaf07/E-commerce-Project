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
    public partial class SignUpForm : Form
    {
        Buyer buyer = new Buyer();
        Seller seller = new Seller();
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void btnRegisterAsBuyer_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFirstName.Text) == false && String.IsNullOrWhiteSpace(txtLastName.Text) == false &&
                String.IsNullOrWhiteSpace(txtUserName.Text) == false && String.IsNullOrWhiteSpace(txtCreditCard.Text) == false &&
                String.IsNullOrWhiteSpace(txtEmail.Text) == false && String.IsNullOrWhiteSpace(txtPassword.Text) == false)
            {
                if (cmbRegisterAs.Text == "buyer" && userNameAvailable())
                {
                    buyer.BuyerFirstName = txtFirstName.Text;
                    buyer.BuyerLastName = txtLastName.Text;
                    buyer.BuyerUserName = txtUserName.Text;
                    buyer.BuyerCreditCard = txtCreditCard.Text;
                    buyer.BuyerEmail = txtEmail.Text;
                    buyer.BuyerPassword = txtPassword.Text;
                    buyer.AddBuyer();
                    new HomePageForm(txtUserName.Text).Show();
                    this.Close();
                }
                else if (cmbRegisterAs.Text == "seller" && userNameAvailable())
                {
                    seller.SellerFirstName = txtFirstName.Text;
                    seller.SellerLastName = txtLastName.Text;
                    seller.SellerUserName = txtUserName.Text;
                    seller.SellerCreditCard = txtCreditCard.Text;
                    seller.SellerEmail = txtEmail.Text;
                    seller.SellerPassword = txtPassword.Text;
                    seller.AddSeller();
                    new SellerForm(txtUserName.Text).Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill in every form");
            }

        }

        private bool userNameAvailable()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cd1 = new SqlCommand("select username from buyer;", connection);
                    SqlCommand cd2 = new SqlCommand("select username from seller;", connection);
                    connection.Open();
                    SqlDataReader dr = cd1.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr["username"].ToString() == txtUserName.Text)
                        {
                            MessageBox.Show("UserName is already Taken!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUserName.Text = "";
                            return false;
                        }
                    }

                    dr.Close();

                    dr = cd2.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr["username"].ToString() == txtUserName.Text)
                        {
                            MessageBox.Show("UserName is already Taken!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUserName.Text = "";
                            return false;
                        }
                    }
                    dr.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
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

        private void picView_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '\0';
        }

        private void picView_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '●';
        }

        private void picView2_MouseDown(object sender, MouseEventArgs e)
        {
            txtConfirmPassword.PasswordChar = '\0';
        }

        private void picView2_MouseUp(object sender, MouseEventArgs e)
        {
            txtConfirmPassword.PasswordChar = '●';
        }

    }
}
