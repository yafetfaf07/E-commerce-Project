using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcommerceProject
{
    internal class Buyer
    {
        public int BuyerId { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerCreditCard { get; set; }
        public string BuyerUserName { get; set; }
        public string BuyerPassword { get; set; }
        public string BuyerEmail { get; set; }
    
    public void AddBuyer() 
        {
            try 
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cd = new SqlCommand("sp_signupbuyer", connection);
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.Parameters.AddWithValue("@buyerfirstname", BuyerFirstName);
                    cd.Parameters.AddWithValue("@buyerlastname", BuyerLastName);
                    cd.Parameters.AddWithValue("@buyercreditcard", BuyerCreditCard);
                    cd.Parameters.AddWithValue("@username", BuyerUserName);
                    cd.Parameters.AddWithValue("@password", BuyerPassword);
                    cd.Parameters.AddWithValue("@email", BuyerEmail);

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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void updateBuyer()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("sp_updatebuyer", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fname", BuyerFirstName);
                    cmd.Parameters.AddWithValue("@lname", BuyerLastName);
                    cmd.Parameters.AddWithValue("@creditcard", BuyerCreditCard);
                    cmd.Parameters.AddWithValue("@username", BuyerUserName);
                    cmd.Parameters.AddWithValue("@password", BuyerPassword);
                    cmd.Parameters.AddWithValue("@email", BuyerEmail);
                    cmd.Parameters.AddWithValue("@buyerid", BuyerId);

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

        public void deleteBuyer()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand("delete from buyer where buyerid = @buyerid", connection);
                    cmd.Parameters.AddWithValue("@buyerid", BuyerId);

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
