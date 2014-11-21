﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUI_Task.Class;

namespace GUI_Task
{
    public partial class frmSalesReturn : Form
    {
        int fcboDefaultValue = 0;
        public frmSalesReturn()
        {
            InitializeComponent();
        }

        private void sales_Sales_Return_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            AtFormLoad();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AtFormLoad()
        {
            string lSQL = string.Empty;

            this.KeyPreview = true;

            //Item Group Combo Fill
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmItemGroup);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cboItemGrp, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboItemGrp.SelectedValue);

        }

        #region Lookup_GL
        private void LookUp_GL()
        {
            //SELECT Code, Name FROM Heads WHERE TYPE = 'A'
            frmLookUp sForm = new frmLookUp(
                    "Code",
                    "Name",
                    "Heads",
                    this.Text.ToString(),
                    1,
                    "Account Code, Account Name",
                    "16,40",
                    " T, T",
                    true,
                    "",
                    " Type='A'",
                    "TextBox"
                    );

            mskCustCode.Mask = "";
            mskCustCode.Text = string.Empty;
            mskCustCode.Mask = clsGVar.maskGLCode;

            sForm.lupassControl = new frmLookUp.LUPassControl(PassData);
            sForm.ShowDialog();
            if (mskCustCode.Text != null)
            {

                if (mskCustCode.Text != null)
                {
                    if (mskCustCode.Text.ToString() == "" || mskCustCode.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (mskCustCode.Text.ToString().Trim().Length > 0)
                    {
                        PopulateRecordsGL();
                        btnOK.Enabled = true;
                        //LoadSampleData();
                        //SumVoc();
                    }

                    //        if (blnFormLoad == true)
                    //        {
                    //            return;
                    //        }
                    //txtOrderNo.Text = txtPassDataVocID.Text.ToString();
                    //grdVoucher[pCol, pRow].Value = tmtext.Text.ToString();
                    //System.Windows.Forms.SendKeys.Send("{TAB}");
                }
                //        string lSQL = string.Empty;

                //if (msk_AccountID.Text.ToString() == "" || msk_AccountID.Text.ToString() == string.Empty)
                //{
                //    return;
                //}
                //msk_AccountID.Text = sForm.lupassControl.ToString();
                ////grdVoucher[pCol, pRow].Value = msk_AccountID.Text.ToString();
                //System.Windows.Forms.SendKeys.Send("{TAB}");
            }
        }
        #endregion
        private void PassData(object sender)
        {
            mskCustCode.Mask = "";
            mskCustCode.Text = ((TextBox)sender).Text;
            mskCustCode.Mask = clsGVar.maskGLCode;
            //mskCustomerCode.Text = ((MaskedTextBox)sender).Text;
            //mskCustomerCode.Mask = clsGVar.maskGLCode;



            //        clsFillCombo.FillCombo(cboItemGroup, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            //        fcboDefaultValue = Convert.ToInt16(cboItemGroup.SelectedValue);

        }

        #region Populate GL Record
        private void PopulateRecordsGL()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  

            tSQL = "SELECT Name ";
            tSQL += " from Heads ";
            tSQL += " where Code ='" + mskCustCode.Text.ToString() + "';";

            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Heads");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool blnValidGLCode;
                    //fAlreadyExists = true;
                    dRow = ds.Tables[0].Rows[0];
                    // Starting title as 0
                    lblCustomerName.Text = (ds.Tables[0].Rows[0]["Name"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Name"].ToString());
                    blnValidGLCode = true;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
                        //btn_EnableDisable(true);
                    }
                    //LoadGridData();
                    //txtManualDoc.Enabled = false;
                }
            }

            catch
            {
                MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
            }
        }
        #endregion



        private void frmSalesReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                LookUp_GL();   
            }

        }

        private void mskCustCode_DoubleClick(object sender, EventArgs e)
        {
            LookUp_GL();
        }

     

    }
}
