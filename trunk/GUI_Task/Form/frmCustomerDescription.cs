﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_Task
{
    public partial class frmCustomerDescription : Form
    {
        public frmCustomerDescription()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mskCustomerCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                LookUp_GL();
            }

        }

        private void mskCustomerCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LookUp_GL();
        }




        private void LookUp_GL()
        {
            //SELECT Code, Name FROM Heads WHERE TYPE = 'A'
            frmLookUp sForm = new frmLookUp(
                    "Code",
                    "Name",
                    " Heads",
                    this.Text.ToString(),
                    1,
                    "Code,Name",
                    "16,40",
                    " T, T",
                    true,
                    "",
                      "",
                //" Type='A'",
                    "TextBox"
                    );

            mskCustomerCode.Mask = "";
            mskCustomerCode.Text = string.Empty;
            mskCustomerCode.Mask = clsGVar.maskGLCode;

            sForm.lupassControl = new frmLookUp.LUPassControl(PassData);
            sForm.ShowDialog();
            if (mskCustomerCode.Text != null)
            {
                if (mskCustomerCode.Text != null)
                {
                    if (mskCustomerCode.Text.ToString() == "" || mskCustomerCode.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (mskCustomerCode.Text.ToString().Trim().Length > 0)
                    {
                        PopulateRecordsGL();
                        //LoadSampleData();
                        //SumVoc();
                    }

                    //txtOrderNo.Text = txtPassDataVocID.Text.ToString();
                    //grdVoucher[pCol, pRow].Value = tmtext.Text.ToString();
                    //System.Windows.Forms.SendKeys.Send("{TAB}");
                }

                //if (msk_AccountID.Text.ToString() == "" || msk_AccountID.Text.ToString() == string.Empty)
                //{
                //    return;
                //}
                //msk_AccountID.Text = sForm.lupassControl.ToString();
                ////grdVoucher[pCol, pRow].Value = msk_AccountID.Text.ToString();
                //System.Windows.Forms.SendKeys.Send("{TAB}");
            }
        }

        private void PassData(object sender)
        {
            mskCustomerCode.Mask = "";
            mskCustomerCode.Text = ((TextBox)sender).Text;
            mskCustomerCode.Mask = clsGVar.maskGLCode;
            //mskCustomerCode.Text = ((MaskedTextBox)sender).Text;
            //mskCustomerCode.Mask = clsGVar.maskGLCode;

        }

        //Populate Recordset 
        private void PopulateRecordsGL()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  
            tSQL = " SELECT h.Code,h.Name, c.Add1,c.Add2,c.City,c.Contact,c.Phone,c.Mobile,c.Fax,c.Email,c.Limit,c.CreditDays from Customer c ";
            tSQL += " INNER JOIN Heads h ON c.Name = h.Name ";
            tSQL += " where h.Code ='" + mskCustomerCode.Text.ToString() + "';";

            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Heads");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    txtName.Text = (ds.Tables[0].Rows[0]["Name"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Name"].ToString());
                    txtContact.Text = (ds.Tables[0].Rows[0]["Contact"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Contact"].ToString());
                    txtAdd1.Text = (ds.Tables[0].Rows[0]["Add1"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Add1"].ToString());
                    txtAdd2.Text = (ds.Tables[0].Rows[0]["Add2"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Add2"].ToString());
                    txtCity.Text = (ds.Tables[0].Rows[0]["City"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["City"].ToString());
                    txtPhone.Text = (ds.Tables[0].Rows[0]["Phone"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Phone"].ToString());
                    txtMobile.Text = (ds.Tables[0].Rows[0]["Mobile"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Mobile"].ToString());
                    txtFax.Text = (ds.Tables[0].Rows[0]["Fax"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Fax"].ToString());
                    txtEmail.Text = (ds.Tables[0].Rows[0]["Email"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["EMail"].ToString());
                    txtCrLimit.Text = (ds.Tables[0].Rows[0]["Limit"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Limit"].ToString());
                    txtCrDays.Text = (ds.Tables[0].Rows[0]["CreditDays"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Creditdays"].ToString());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
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

        private void frmCustomerDescription_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            LookUp_GL();
        }


    }
}
