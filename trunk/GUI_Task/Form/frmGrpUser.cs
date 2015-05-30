using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUI_Task.StringFun01;

namespace GUI_Task
{
    enum GColGrpUser
    {
        GroupID = 0,
        Checked = 1,
        UserID = 2
    }

    public partial class frmGrpUser : Form
    {
        //******* Grid Variable Setting -- Begin ******
        string fHDR = string.Empty;                       // Column Header
        string fColWidth = string.Empty;                  // Column Width (Input)
        string fColMinWidth = string.Empty;               // Column Minimum Width
        string fColMaxInputLen = string.Empty;            // Column Visible Length/Width 
        string fColFormat = string.Empty;                 // Column Format  
        string fColReadOnly = string.Empty;               // Column ReadOnly 1 = ReadOnly, 0 = Read-Write  
        string fFieldList = string.Empty;

        string fColType = string.Empty;
        string fFieldName = string.Empty;
        //******* Grid Variable Setting -- End ******

        string[] a_Color = new string[0];
        int[] a_ColorInt = new int[0];

        string[] a_Size = new string[0];
        int[] a_SizeInt = new int[0];
        List<string> fManySQL = null;                      // List string for storing Multiple Queri
        string fRptTitle = string.Empty;
        //bool fFormClosing = false;

        bool ftTIsBalloon = true;
        bool fEditMod = false;
        int fEditRow = 0;
        //bool fFrmLoading = true;                    // Form is Loading Controls (to accomodate Load event so that first time loading requirement is done)
        int fTErr = 0;                              // Total Errors while Saving or other operation.
        string ErrrMsg = string.Empty;              // To display error message if any.
        string fLastID = string.Empty;              // Last Voucher/Doc ID (Saved new or modified)
        //
        //int fDocTypeID = 1;                         // Voucher/Doc Type ID
        int fDocFiscal = 1;                         // Accounting / Fiscal Period    
        //int fTNOA = 0;                              // Total Number of Attachments.    
        int fTNOT = 0;                              // Total Number of Grid Transactions.
        decimal fDocAmt = 0;                        // Document Amount Debit or Credit for DocMaster Field.
        string fDocWhere = string.Empty;            // Where string to build where clause for Voucher level
        int fLastRow = 0;                           // Last row number of the grid.
        Int64 fDocID = 1;
        bool fGridControl = false;                  // To overcome Grid's tabing

        bool fSingleEntryAllowed = true;            // for the time being later set to false.
        bool fDocAlreadyExists = false;             // Check if Doc/voucher already exists (Edit Mode = True, New Mode = false)
        bool fIDConfirmed = false;                  // Account ID is valid and existance in Table is confirmed.
        bool fCellEditMode = false;                 // Cell Edit Mode

        bool blnFormLoad = true;
        int fcboDefaultValue = 0;

        string strDocId = string.Empty;

        public frmGrpUser()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGrpUser_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void frmGrpUser_Load(object sender, EventArgs e)
        {
            AtFormLoad();
            LoadGridData();
            blnFormLoad = false;
        }

        private void AtFormLoad()
        {
            string lSQL = string.Empty;

            SettingGridVariable();
            LoadInitialControls();

            this.KeyPreview = true;


            //Users Combo Fill
            lSQL = " select UserID, UserName from Users ";
            lSQL += " ORDER BY UserName";

            clsFillCombo.FillCombo(cboUsers, clsGVar.ConString1, "Users" + "," + "UserID" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboUsers.SelectedValue);
        }

        private void SettingGridVariable()
        {
            string lHDR = "";                       // Column Header
            string lColWidth = "";                  // Column Width (Input)
            string lColMinWidth = "";               // Column Minimum Width
            string lColMaxInputLen = "";            // Column Visible Length/Width 
            string lColFormat = "";                 // Column Format  
            string lColReadOnly = "";               // Column ReadOnly 1 = ReadOnly, 0 = Read-Write  
            string lFieldList = "";
            string tColType = "";
            string tFieldName = "";

            //
            lFieldList = "groupid";
            lFieldList += ", Checked";
            lFieldList += ", GroupName";


            lHDR = "Group ID";
            lHDR += ", Check ";
            lHDR += ", Groups";           
           

            // Col Visible Width
            lColWidth = "   5";                 
            lColWidth += ", 5";                
            lColWidth += ", 10";                


            // Column Input Length/Width
            lColMaxInputLen = "  0";            
            lColMaxInputLen += ", 0";           
            lColMaxInputLen += ", 0";           

            // Column Min Width
            lColMinWidth = "   0";                      //  0-    ItemID";       
            lColMinWidth += ", 0";                        //  1-    ItemCod;     
            lColMinWidth += ", 0";                        //  2-    ItemNam; 


            // Column Format
            lColFormat = "   H";                       //  0-    ItemID";       
            lColFormat += ", CH";                         //  1-    ItemCod     
            lColFormat += ", T";                         //  4-    SizeNam     


            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                       //  0-    ItemID";      
            lColReadOnly += ",0";                         //  1-    ItemCod     
            lColReadOnly += ",0";                         //  2-    ItemNam

            // For Saving Time
            tColType += "  H";             //  0-    ItemID"; 
            tColType += ", T";               //  1-    ItemCod
            tColType += ", T";               //  2-    ItemNam


            tFieldName += "groupid";                //  0-    ItemID";       
            tFieldName += ", Checked";             //  1-    ItemCode";     
            tFieldName += ", GroupName";             //  2-    ItemName"; 

            fHDR = lHDR;
            fColWidth = lColWidth;
            fColMaxInputLen = lColMaxInputLen;
            fColMinWidth = lColMinWidth;
            fColFormat = lColFormat;
            fColReadOnly = lColReadOnly;
            fFieldList = lFieldList;

            fColType = tColType;
            fFieldName = tFieldName;

        }

        private void LoadInitialControls()
        {
            // 1 = dGV Grid Control
            // 2 = Column Total (Total number of Columns for cross verification with other parameters like width, format)
            // 3 = Column Header
            // 4 = Column Width to be displayed on Grid
            // 5 = Column MaxInputLen   // 0 = unlimited, 
            // 6 = Column Format        // T = Text, N = Numeric, H = Hiden
            // 7 = Column ReadOnly      // 1 = ReadOnly, 0 = Not ReadOnly
            // 8 = Grid Color Scheme    // Default = 1
            // RO 
            grd.Rows.Clear();
            grd.Columns.Clear();

            List<string> lMask = null; //new List<string>;
            List<string> lCboFillType = null; //new List<string>;
            List<string> lCboTableKeyField = null; //new List<string>;
            List<string> lCboQry = null; //new List<string>;

            clsDbManager.SetGridHeaderCmb(
                grd,
                3,
                fHDR,
                fColWidth,
                fColMaxInputLen,
                fColMinWidth,
                fColFormat,
                fColReadOnly,
                "DATA",
                lMask,
                lCboFillType,
                lCboTableKeyField,
                lCboQry,
                false,
                2);

        }

        private void LoadGridData()
        {
            string lSQL = "";

            //// ************** OLD Query *************
            ////lSQL =  " select groupid, Disable, GroupName ";
            //lSQL = " SELECT * ";
            //lSQL += " from Groups ";

            //lSQL += " ORDER BY GroupName ";


            //// **************  New Query ***************
            //lSQL = " select g.groupid, g.Disable, g.GroupName ";
            //lSQL += " from Groups g INNER JOIN GroupUsers gu ON g.groupid = gu.GroupID ";
            //lSQL += " WHERE gu.UserID = " + cboUsers.SelectedIndex.ToString() + "";
            //lSQL += " ORDER BY GroupName ";

            //clsDbManager.SP_DataSet("sp_GroupUsers", "1");


            SPDataGet();



            //clsDbManager.FillDataGrid(
            //    grd,
            //    lSQL,
            //    fFieldList,
            //    fColFormat);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnFirst.Enabled = true;
            btnLast.Enabled = true;
            btnPrevious.Enabled = true;
            btnNext.Enabled = true;
            btnModify.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(cboUsers.SelectedIndex < cboUsers.Items.Count)
            cboUsers.SelectedIndex += 1;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedIndex > 0)
            {
                cboUsers.SelectedIndex -= 1;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            cboUsers.SelectedIndex = 0;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            cboUsers.SelectedIndex = cboUsers.Items.Count - 1;
        }

        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridData();

            //SPDataGet();
        }

        private void SPDataGet()
        {
            string plstField = "@UserID";
            string plstType = "8"; // {"8, 8, 8, 8, 8, 8"};
            string plstValue = string.Empty;
            
            string cboUsersValue = string.Empty;

            if (cboUsers.SelectedIndex.ToString() == "0")
            {
                plstValue = "1";
            }
            else
            {
                plstValue = cboUsers.SelectedIndex.ToString();
            }

            //string plstValue = "1";

            //DataSet dsUsers = new DataSet();
            //dsUsers = clsDbManager.SP_Exe("sp_GroupUsers", plstField, plstType, plstValue);

            clsDbManager.FillData_SP(
                grd,
                "sp_GroupUsers",
                plstField,
                plstType,
                plstValue,
                fFieldList,
                fColFormat);

            //DataGridView pDGV,
            //string pSP,
            //string plstField,
            //string plstType, 
            //string plstValue,
            //string pFieldList,
            //string pFieldFormat,
            //DataSet pDS = null,
            //int pTableId = 0,
            //bool pReadOnly = false,
            //string pCon = clsGVar.ConString1
        }

        private void cboUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            SPDataGet();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            textAlert.Text = "Data Saved Successfully";
        }

        private void ClearThisForm()
        {
            ResetFields();
        }

        private void ResetFields()
        {
            // Reset Form Level Variables/Fields
            fEditMod = false;
            fTNOT = 0;
            fDocAmt = 0;
            fDocWhere = string.Empty;
            fLastRow = 0;
        }

        private bool FormValidation()
        {
            bool lRtnValue = true;
            DateTime lNow = DateTime.Now;
            decimal lDebit = 0;
            decimal lCredit = 0;
            fDocAmt = 0;
            try
            {
                return lRtnValue;
            }
            catch (Exception ex)
            {
                fTErr++;
                ErrrMsg = StrF01.BuildErrMsg(ErrrMsg, "Exception: FormValidation -> " + ex.Message.ToString());
                return false;
            }
        }

        string lSQL = string.Empty;

        private bool SaveData()
        {
            bool rtnValue = true;
            fTErr = 0;
            fManySQL = new List<string>();

            if (fManySQL != null)
            {
                if (fManySQL.Count() > 0)
                {
                    fManySQL.Clear();
                }
            }

            //string lSQL = string.Empty;

            fDocWhere = " UserID = " + cboUsers.SelectedValue.ToString();

            lSQL = "delete from GroupUsers ";
            lSQL += " where " + fDocWhere;

            fManySQL.Add(lSQL);

            clsDbManager.ExeMany(fManySQL);

            rtnValue = true;
            try
            {

                // Grid Voucher
                if (grd.Rows.Count > 0)
                {
                    // Prepare Detail Doc Query List
                    string tTableName = "RestrictedAccess";
                    string tFieldName = "";
                    string tColType = "";
                    //
                    tColType += "  N0"; // groupid";  
                    tColType += ", CH";  // Checked";
                    tColType += ", T";  // GroupName

                    //

                    tFieldName += "  groupid";       // groupid";  
                    tFieldName += ", Checked";          // Checked";
                    tFieldName += ", GroupName";    // GroupName
                    // 
                    //string tAddFieldName = "Doc_vt_id, Doc_fiscal_ID, Doc_ID";
                    //string tAddValue = fDocTypeID.ToString() + ", " + fDocFiscal.ToString() + ", " + fDocID.ToString();

                    string tAddFieldName = string.Empty;
                    string tAddValue = string.Empty;

                    if (clsDbManager.PrepareGridSQL(grd, tTableName, tFieldName, tColType, fManySQL, tAddFieldName, tAddValue) != "OK")
                    {
                        return false;
                    }
                }
                return rtnValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Detail Doc: " + ex.Message, this.Text.ToString());
                return false;
            }

            //try
            //{
            //    for (int dGVRow = 0; dGVRow < grd.Rows.Count; dGVRow++)
            //    {
            //        if (grd.Rows[dGVRow].Cells[(int)GColGrpUser.GroupID].Value == null)
            //        {
            //            if (dGVRow == fLastRow)
            //            {
            //                continue;
            //            }
            //        }
            //        else
            //        {
            //            if ((grd.Rows[dGVRow].Cells[(int)GColGrpUser.GroupID].Value.ToString()).Trim(' ', '-') == "")
            //            {
            //                //lBlank = true;
            //                if (dGVRow == fLastRow)
            //                {
            //                    continue;
            //                }
            //            }
            //        }

            //        //if (grd.Rows[dGVRow].Cells[(int)GColGrpUser.Checked].Value.ToString() == "true")
            //        //{
            //            lSQL = " INSERT INTO GroupUsers( GroupID ";
            //            lSQL += ", UserID )";
            //            lSQL += " VALUES (";
            //            lSQL += "" + grd.Rows[dGVRow].Cells[(int)GColGrpUser.GroupID].Value.ToString() + "";
            //            lSQL += ", " + grd.Rows[dGVRow].Cells[(int)cboUsers.SelectedIndex].Value.ToString() + "";
            //            lSQL += ")";

            //            fManySQL.Add(lSQL);
            //        //}
                    
            //    } // End For loopo

            //    clsDbManager.ExeMany(fManySQL);
            //    //return rtnValue;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Save Detail Doc: " + ex.Message, this.Text.ToString());
            //    //return false;
            //}
            
        } // End Save
    }
}
