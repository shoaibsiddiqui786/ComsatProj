using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace GUI_Task
{
    public partial class frmPassword : Form
    {
        public frmPassword()
        {
            InitializeComponent();
        }

        private void Password_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNewPassword.Text) | string.IsNullOrEmpty(this.txtOldPassword.Text) | string.IsNullOrEmpty(this.txtConfirmPassword.Text))
            {
                MessageBox.Show("Invalid Credentials");
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source= (Local); Initial Catalog=GUI_Task; User ID=sa; Password=smc786";
            conn.Open();

            string Password = txtOldPassword.Text;

            SqlCommand cmd = new SqlCommand("select * from Users WHERE UserName = 'Usama' AND Password = '" + txtOldPassword.Text + "'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            System.Data.SqlClient.SqlDataReader dr = null;
            dr = cmd.ExecuteReader();

            if (txtNewPassword.Text == txtConfirmPassword.Text)
            {

                if (dr.Read())
                {
                   // SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
                   // con.ConnectionString = "Data Source= (Local); Initial Catalog=GUI_Task; User ID=sa; Password=smc786";
                   // con.Open();

                   // SqlCommand cmd1 = new SqlCommand("UPDATE Users SET Password = '" + txtNewPassword.Text + "'" + "WHERE UserName = 'Usama'", conn);
                   // MessageBox.Show("Password Changed Successfully !");
                   // SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                   // DataTable dt1 = new DataTable();
                   //// da.Fill(dt1);

                   // System.Data.SqlClient.SqlDataReader dr1 = null;
                   // dr1 = cmd.ExecuteReader();
                   // this.Close();

                    string lSQL = "";

                    DataSet ds = new DataSet();
                    ds = clsDbManager.GetData_Set(lSQL, "CatDtl");
  
                }
            }
            else
            {
                MessageBox.Show("Passwords Does not Match !");
                this.Close();
            }
        }
    }
}
