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
    public partial class SignInForm : Form
    {
        public SignInForm()
        {
            InitializeComponent();
            MaximizeBox = false;
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using(SqlConnection connection=new SqlConnection(ConString))
                {
                    SqlCommand cd = new SqlCommand("select username,password from buyer;", connection);
                    connection.Open();
                    SqlDataReader dr = cd.ExecuteReader();

                    while(dr.Read())
                    {
                        if(dr["username"].ToString() == txtUserName.Text
                            && dr["password"].ToString() == txtPassword.Text )
                        {
                            new HomePageForm(txtUserName.Text).Show();
                            this.Hide();
                        }
                    }

                    dr.Close();
                    cd = new SqlCommand("select username,password from seller;", connection);
                    dr = cd.ExecuteReader();

                    while(dr.Read())
                    {
                        if (dr["username"].ToString() == txtUserName.Text
                            && dr["password"].ToString() == txtPassword.Text)
                        {
                            new SellerForm(txtUserName.Text).Show();
                            this.Hide();
                        }
                    }
                    errorProvider1.SetError(txtPassword, null);
                    errorProvider1.SetError(txtPassword, "User Name or Password is Incorrect");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            new SignUpForm().Show();
            this.Hide();
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {

        }

        private void picAccount_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '\0';
        }

        private void picAccount_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '●';
        }

        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
