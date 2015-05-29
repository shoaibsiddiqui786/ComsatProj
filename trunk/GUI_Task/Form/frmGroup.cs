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
    public partial class frmGroup : Form
    {
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

        // Check if Form is edited or not
        bool isEdited = false;

        int strDocId = 0;

        public frmGroup()
        {
            InitializeComponent();
        }

        private void Group_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGrpUser frm = new frmGrpUser();
            frm.Show();
        }

        private void frmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            PopulateRecords();
            AtFormLoad();
            LoadGridData();
        }

        private void AtFormLoad()
        {
            SettingGridVariable();
            LoadInitialControls();
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
            lFieldList = " groupid";
            lFieldList += ", GroupName";                //  0-    ItemID";       
            lFieldList += ", description";            //  1-    ItemCode";     


            lHDR += " Group ID";             //  0-    ItemID";      
            lHDR += ", Name";            //  1-    ItemCod     
            lHDR += ", Description";            //  2-    ItemNam 

            // Col Visible Width
            lColWidth = "   5";                 //  0-    ItemID";        
            lColWidth += ", 10";                   //  1-    ItemCod";     
            lColWidth += ", 15";                 //  2-    ItemNam; 

            // Column Input Length/Width
            lColMaxInputLen = "  0";                 //  0-    ItemID";    
            lColMaxInputLen += ", 0";                  //  1-    ItemCod   
            lColMaxInputLen += ", 0";                  //  2-    ItemNam

            // Column Min Width
            lColMinWidth = "   0";                      //  0-    ItemID";       
            lColMinWidth += ", 0";                        //  1-    ItemCod;     
            lColMinWidth += ", 0";                        //  2-    ItemNam; 


            // Column Format
            lColFormat = "   H";                       //  0-    ItemID";       
            lColFormat += ", T";                         //  1-    ItemCod     
            lColFormat += ", T";                         //  2-    ItemNam 


            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                       //  0-    ItemID";      
            lColReadOnly += ",0";                         //  1-    ItemCod     
            lColReadOnly += ",0";                         //  2-    ItemNam

            // For Saving Time
            tColType += "  H";             //  0-    ItemID"; 
            tColType += ", T";               //  1-    ItemCod
            tColType += ", T";               //  2-    ItemNam


            tFieldName += "groupid";                //  0-    ItemID";       
            tFieldName += ",GroupName";             //  1-    ItemCode";     
            tFieldName += ",description";             //  2-    ItemName"; 

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
            lSQL += " select groupid, GroupName, description from Groups ";

            clsDbManager.FillDataGrid(
                grd,
                lSQL,
                fFieldList,
                fColFormat);
        }

        private void PopulateRecords()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  


            tSQL = " select groupid, groupcode, GroupName, description ";
            tSQL += " from Groups ";
           
            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Groups");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    txtGroupId.Text = (ds.Tables[0].Rows[0]["groupid"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupid"].ToString());
                    txtGroupCode.Text = (ds.Tables[0].Rows[0]["groupcode"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupcode"].ToString());
                    txtGroupName.Text = (ds.Tables[0].Rows[0]["GroupName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["GroupName"].ToString());
                    txtDescription.Text = (ds.Tables[0].Rows[0]["description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["description"].ToString());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
                    }
                    //LoadGridData();
                    //SumVoc();
                }
            }
            catch
            {
                MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
            }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        int prevRec = 0;

        private void btnPrevious_Click_1(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin 

            if (txtGroupCode.Text.ToString().Trim(' ', '-') == "")
            {
                MessageBox.Show("Unable to Navigate");
            }

            else
            {
                tSQL = "select top 1 groupid from Groups where groupid < " + txtGroupId.Text.ToString() + " ORDER BY CAST(groupid AS INT) DESC";
                ds = clsDbManager.GetData_Set(tSQL, "Groups");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    prevRec = Convert.ToInt32(ds.Tables[0].Rows[0]["groupid"] == DBNull.Value ? "1" : ds.Tables[0].Rows[0]["groupid"].ToString());
                }
            }

            tSQL = " select groupid, groupcode, GroupName, description ";
            tSQL += " from Groups ";
            tSQL += " WHERE groupid = " + prevRec.ToString();

            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Groups");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    txtGroupId.Text = (ds.Tables[0].Rows[0]["groupid"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupid"].ToString());
                    txtGroupCode.Text = (ds.Tables[0].Rows[0]["groupcode"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupcode"].ToString());
                    txtGroupName.Text = (ds.Tables[0].Rows[0]["GroupName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["GroupName"].ToString());
                    txtDescription.Text = (ds.Tables[0].Rows[0]["description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["description"].ToString());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
                    }
                    //LoadGridData();
                    //SumVoc();
                }
            }
            catch
            {
                MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
            }
            
            prevRec--;
        }

        int nextRec = 1;

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin 

            if (txtGroupCode.Text.ToString().Trim(' ', '-') == "")
            {
                nextRec = 1;
            }

            else
            {
                tSQL = "select top 1 groupid from Groups where groupid > " + txtGroupId.Text.ToString() + " ORDER BY CAST(groupid AS INT)";
                ds = clsDbManager.GetData_Set(tSQL, "Groups");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    nextRec = Convert.ToInt32(ds.Tables[0].Rows[0]["groupid"] == DBNull.Value ? "1" : ds.Tables[0].Rows[0]["groupid"].ToString());
                }
            }


            tSQL = " select groupid, groupcode, GroupName, description ";
            tSQL += " from Groups ";
            tSQL += " WHERE groupid = " + nextRec.ToString();
            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Groups");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    txtGroupId.Text = (ds.Tables[0].Rows[0]["groupid"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupid"].ToString());
                    txtGroupCode.Text = (ds.Tables[0].Rows[0]["groupcode"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["groupcode"].ToString());
                    txtGroupName.Text = (ds.Tables[0].Rows[0]["GroupName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["GroupName"].ToString());
                    txtDescription.Text = (ds.Tables[0].Rows[0]["description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["description"].ToString());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
                    }
                    //LoadGridData();
                    //SumVoc();
                }
            }
            catch
            {
                MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
            }
            nextRec++;
        }

        private void btnModify_Click_1(object sender, EventArgs e)
        {
            btnAddNew.Enabled = false;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnModify.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            Action<Control.ControlCollection> func1 = null;

            func1 = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                    {
                        (control as TextBox).Enabled = true;
                        (control as TextBox).ReadOnly = false;
                    }
                    else if (control is CheckBox)
                        (control as CheckBox).Enabled = true;
                    else if (control is ComboBox)
                        (control as ComboBox).Enabled = true;
                    else if (control is DateTimePicker)
                        (control as DateTimePicker).Enabled = true;
                    else
                        func1(control.Controls);
            };

            func1(Controls);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            btnAddNew.Enabled = false;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnModify.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = true;

            ClearTextBoxes();

            Action<Control.ControlCollection> func1 = null;

            func1 = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                    {
                        (control as TextBox).Enabled = true;
                        (control as TextBox).ReadOnly = false;
                    }
                    else if (control is CheckBox)
                        (control as CheckBox).Enabled = true;
                    else if (control is ComboBox)
                        (control as ComboBox).Enabled = true;
                    else if (control is DateTimePicker)
                        (control as DateTimePicker).Enabled = true;
                    else
                        func1(control.Controls);
            };

            func1(Controls);
        }
            private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                    {
                        (control as TextBox).Clear();
                        //lblTime.Text = "";
                        //lblTotal.Text = "";
                    }
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           SaveData();
           textAlert.Text = "Data Saved Successfully";
        }

        private bool SaveData()
        {
            bool rtnValue = true;
            fTErr = 0;
            if (fManySQL != null)
            {
                if (fManySQL.Count() > 0)
                {
                    fManySQL.Clear();
                }
            }
            string lSQL = string.Empty;
            DateTime lNow = DateTime.Now;
            try
            {
                ErrrMsg = "";
                if (!FormValidation())
                {
                    textAlert.Text = "Form Validation Error: Not Saved." + "  " + lNow.ToString();
                    MessageBox.Show(ErrrMsg, "Save: " + this.Text.ToString());
                    return false;
                }

                fManySQL = new List<string>();

                // Prepare Master Doc Query List

                if (!PrepareDocMaster())
                {
                    textAlert.Text = "DocMaster: Modifying Doc/Voucher not available for updation.'  ...." + "  " + lNow.ToString();
                    //tabMDtl.SelectedTab = tabError;
                    return false;
                }
                //
                DateTime now = DateTime.Now;
                textAlert.Text = "selected Box Empty... " + now.ToString("T");
                // pending return false;

                // Execute Query
                if (fManySQL.Count > 0)
                {
                    if (!clsDbManager.ExeMany(fManySQL))
                    {
                        MessageBox.Show("Not Saved see log...", this.Text.ToString());
                        return false;
                    }
                    else
                    {
                        fLastID = txtGroupId.Text.ToString();
                        ClearThisForm();
                        return true;
                    }
                }
                else
                {
                   // MessageBox.Show("Data Preparation list empty, Not Saved...", this.Text.ToString());
                    return false;
                } // End Execute Query
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Processing Save: " + ex.Message, "Save Data: " + this.Text.ToString());
                return false;
            }

        } // End Save

        private bool PrepareDocMaster()
        {
            bool rtnValue = true;
            string lSQL = string.Empty;
            int intIdentity = 0;
            DataSet dsIdentity = new DataSet();

            try
            {
                if (txtGroupId.Text.ToString().Trim(' ', '-') == "")
                {
                    strDocId = 0;
                    fDocAlreadyExists = false;
                    //fDocID = fDocID + 1;
                    txtGroupId.Text = fDocID.ToString();
                    fDocID = clsDbManager.GetNextValDocID("Groups", "groupid", fDocWhere, "");
                    //fDocID = fDocID + 1;

                    lSQL = "insert into Groups (";
                    //lSQL += " groupid ";
                    lSQL += " groupcode ";
                    lSQL += ", GroupName ";
                    lSQL += ", Description ";
                    lSQL += ", OrgID ";
                    lSQL += " ) values (";
                    //                                                       
                    //lSQL += "" + fDocID.ToString() + "";
                    lSQL += " '" + txtGroupCode.Text.ToString() + "'";
                    lSQL += ",'" + txtGroupName.Text.ToString() + "'";
                    lSQL += ",'" + txtDescription.Text.ToString() + "'";
                    lSQL += ", 1";
                    lSQL += ") SELECT @@IDENTITY AS NewIdentity ";


                    dsIdentity = clsDbManager.GetData_Set(lSQL, "Groups");
                    // intIdentity = Convert.ToInt16(dsIdentity.Tables[0].Rows[0].ToString());
                    intIdentity = Convert.ToInt16(dsIdentity.Tables[0].Rows[0]["NewIdentity"] == DBNull.Value ? "1" : dsIdentity.Tables[0].Rows[0]["NewIdentity"].ToString());

                    //MessageBox.Show("Identity: " + intIdentity.ToString());

                    fDocID = intIdentity;

                    //fManySQL.Add(lSQL);
                }
                else
                {
                    fDocWhere = " groupid = " + txtGroupId.Text.ToString() + "";
                    if (clsDbManager.IDAlreadyExistWw("Groups", "groupid", fDocWhere))
                    {
                        fDocAlreadyExists = true;
                        //fManySQL.Add(lSQL);
                    }

                    lSQL = "update Groups set";

                    lSQL += "  groupcode = '" + txtGroupCode.Text.ToString() + "'";
                    lSQL += ", GroupName = '" + txtGroupName.Text.ToString() + "'";
                    lSQL += ", Description = '" + txtDescription.Text.ToString() + "'";
                    lSQL += " where ";
                    lSQL += fDocWhere;

                    fManySQL.Add(lSQL);
                }

                return rtnValue;
            }
            catch (Exception ex)
            {
                rtnValue = false;
                MessageBox.Show("Save Master Doc: " + ex.Message, this.Text.ToString());
                return false;
            }
        }

            private void btnCancel_Click_1(object sender, EventArgs e)
            {
                btnCancel.Enabled = false;
                btnSave.Enabled = false;
                btnAddNew.Enabled = true;
                btnFirst.Enabled = true;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                btnModify.Enabled = true;

                Action<Control.ControlCollection> func1 = null;

                func1 = (controls) =>
                {
                    foreach (Control control in controls)
                        if (control is TextBox)
                        {
                            (control as TextBox).Enabled = false;
                            (control as TextBox).ReadOnly = true;
                        }
                        else if (control is CheckBox)
                            (control as CheckBox).Enabled = false;
                        else if (control is ComboBox)
                            (control as ComboBox).Enabled = false;
                        else if (control is DateTimePicker)
                            (control as DateTimePicker).Enabled = false;
                        else
                            func1(control.Controls);
                };

                func1(Controls);
            }

            private void btnUsers_Click(object sender, EventArgs e)
            {
                frmGrpUser frm = new frmGrpUser();
                frm.Show();
            }
   }
}
