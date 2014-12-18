using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUI_Task.PrintVw6;
using GUI_Task.PrintReport;

namespace GUI_Task
{
    public partial class frmCustList : Form
    {
        public frmCustList()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void customer_List_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void frmCustList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string fRptTitle = this.Text;
            string plstField = "Code, Name, Add1, City, Phone, Mobile";
            string plstType = null; // {"8, 8, 8, 8, 8, 8"};
            string plstValue = null; 
                //this.mskAccCode.Text + "," + StrF01.D2Str(this.dtpFromDate.Value) + "," +
              //  StrF01.D2Str(this.dtpToDate.Value);

            //dsLedgerNew pDs = new dsLedgerNew();

            DataSet pDs = new DataSet();
            CrCustList rpt1 = new CrCustList();

            frmPrintVw6 rptLedger2 = new frmPrintVw6(
               fRptTitle,
               "",
               "",
               //StrF01.D2Str(this.dtpFromDate.Value),
               //StrF01.D2Str(this.dtpToDate.Value),
               "CustList",
               plstField,
               plstType,
               plstValue,
               pDs,
               rpt1,
               "Table"
               );

            //rptLedger2.ShowDialog();
            rptLedger2.Show();
        }
    }
}
