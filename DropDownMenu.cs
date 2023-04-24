using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcommerceProject
{
    public partial class DropDownMenu : UserControl
    {
        public DropDownMenu()
        {
            InitializeComponent();
        }

        HomePageForm HomePageParentForm;
        SellerForm SellerParentForm;
        bool fromBuyer;
        public DropDownMenu(object parentForm, bool fromBuyer)
        {
            InitializeComponent();
            this.fromBuyer = fromBuyer;
            if(fromBuyer)
                HomePageParentForm = (HomePageForm)parentForm;
            else
                SellerParentForm = (SellerForm)parentForm;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
                return;
            else
                Hide();
        }

        #region Mouse Click Events
        private void lblMyProfile_Click(object sender, EventArgs e)
        {
            Hide();
            if (fromBuyer)
            {
                new ProfileForm(HomePageParentForm.userName, fromBuyer).Show();
                HomePageParentForm.Hide();
            }
            else
            {
                new ProfileForm(SellerParentForm.userName, fromBuyer).Show();
                SellerParentForm.Hide();
            }
        }

        private void lblCart_Click(object sender, EventArgs e)
        {
            Hide();
            if (fromBuyer)
            {
                HomePageParentForm.showCart();
            }
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
            Hide();
            if (fromBuyer)
            {
                new SignInForm().Show();
                HomePageParentForm.Hide();
            }
            else
            {
                new SignInForm().Show();
                SellerParentForm.Hide();
            }
        }
        #endregion

        #region Mouse Hover Events
        private void lblMyProfile_MouseHover(object sender, EventArgs e)
        {
            lblMyProfile.Font = new Font("Arial", 12F, FontStyle.Bold);
        }

        private void lblMyProfile_MouseLeave(object sender, EventArgs e)
        {
            lblMyProfile.Font = new Font("Arial", 12F, FontStyle.Regular);
        }

        private void lblCart_MouseHover(object sender, EventArgs e)
        {
            lblCart.Font = new Font("Arial", 12F, FontStyle.Bold);
        }

        private void lblCart_MouseLeave(object sender, EventArgs e)
        {
            lblCart.Font = new Font("Arial", 12F, FontStyle.Regular);
        }

        private void lblLogOut_MouseHover(object sender, EventArgs e)
        {
            lblLogOut.Font = new Font("Arial", 12F, FontStyle.Bold);
        }

        private void lblLogOut_MouseLeave(object sender, EventArgs e)
        {
            lblLogOut.Font = new Font("Arial", 12F, FontStyle.Regular);
        } 
        #endregion
    }
}
