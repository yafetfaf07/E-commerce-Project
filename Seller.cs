using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcommerceProject
{
    internal class Seller
    {
        public int SellerId { get; set; }
        public string SellerFirstName { get; set; }
        public string SellerLastName { get; set; }
        public string SellerCreditCard { get; set; }
        public float SellerRevenue { get; set; }
        public string SellerUserName { get; set; }
        public string SellerPassword { get; set; }
        public string SellerEmail { get; set; }

        public void AddSeller()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            try
            { 
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cd = new SqlCommand("sp_signupseller", connection);
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.Parameters.AddWithValue("@sellerfirstname", SellerFirstName);
                    cd.Parameters.AddWithValue("@sellerlastname", SellerLastName);
                    cd.Parameters.AddWithValue("@sellercreditcard", SellerCreditCard);
                    cd.Parameters.AddWithValue("@username", SellerUserName);
                    cd.Parameters.AddWithValue("@password", SellerPassword);
                    cd.Parameters.AddWithValue("@email", SellerEmail);

                    connection.Open();
                    int rowsAffected = cd.ExecuteNonQuery();
                   if(rowsAffected > 0)
                   {
                      MessageBox.Show("Account has been added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void updateSeller()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("sp_updateseller", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fname", SellerFirstName);
                    cmd.Parameters.AddWithValue("@lname", SellerLastName);
                    cmd.Parameters.AddWithValue("@creditcard", SellerCreditCard);
                    cmd.Parameters.AddWithValue("@username", SellerUserName);
                    cmd.Parameters.AddWithValue("@password", SellerPassword);
                    cmd.Parameters.AddWithValue("@email", SellerEmail);
                    cmd.Parameters.AddWithValue("@sellerid", SellerId);

                    connection.Open();
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
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

        public void deleteSeller()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("delete from seller where sellerid = @sellerid", connection);
                    cmd.Parameters.AddWithValue("@sellerid", SellerId);

                    connection.Open();
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
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
