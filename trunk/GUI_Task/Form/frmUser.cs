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
    enum GColUser
    {
       UserID = 0,
       Day = 1,       
       StartingT = 2, 
       EndingT = 3,
       Enabled = 4   
    }

    public partial class frmUser : Form
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
        //******* Grid1 Variable Setting -- End ******

        string fHDR2 = string.Empty;                       // Column Header
        string fColWidth2 = string.Empty;                  // Column Width (Input)
        string fColMinWidth2 = string.Empty;               // Column Minimum Width
        string fColMaxInputLen2 = string.Empty;            // Column Visible Length/Width 
        string fColFormat2 = string.Empty;                 // Column Format  
        string fColReadOnly2 = string.Empty;               // Column ReadOnly 1 = ReadOnly, 0 = Read-Write  
        string fFieldList2 = string.Empty;

        string fColType2 = string.Empty;
        string fFieldName2 = string.Empty;
        //******* Grid2 Variable Setting -- End ******

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


        public frmUser()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void user_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            PopulateRecords();
            AtFormLoad();
            LoadGridData();
            AtFormLoad2();
            LoadGridData2();

            //lstTimeRestriction.View = View.Details;
            //lstTimeRestriction.Columns.Add("Day", 50, HorizontalAlignment.Left);
            //lstTimeRestriction.Columns.Add("Start Time", 70, HorizontalAlignment.Left);
            //lstTimeRestriction.Columns.Add("End Time", 70, HorizontalAlignment.Left);
            //lstTimeRestriction.Columns.Add("Enabled", 70, HorizontalAlignment.Left);
            //lstTimeRestriction.CheckBoxes = true;
            //ListViewItem item1 = new ListViewItem("Day",1);
            //item1.SubItems.Add("1");

        }

        private void AtFormLoad()
        {
            SettingGridVariable();
            LoadInitialControls();

            //string lSQL = string.Empty;

            //lSQL = " SELECT UserID, Day FROM RestrictedAccess";

            //clsFillCombo.FillCombo(cboDay, clsGVar.ConString1, "RestrictedAccess" + "," + "UserID" + "," + "False", lSQL);
            //fcboDefaultValue = Convert.ToInt16(cboDay.SelectedValue);
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
            lFieldList = "UserID";
            lFieldList += ", Day";                //  0-    ItemID";       
            lFieldList += ", StartingT";            //  1-    ItemCode";     
            lFieldList += ", EndingT";            //  2-    ItemName"; 
            lFieldList += ", Enabled";           // 3- Description
          

            lHDR += "User ID";             //  0-    ItemID";      
            lHDR += ", Day";            //  1-    ItemCod     
            lHDR += ", Start Time";            //  2-    ItemNam 
            lHDR += ", End Time";            // 3- Descripti
            lHDR += ", Enabled";                 //  4-    SizeNam     
            
            // Col Visible Width
            lColWidth = "   5";                 //  0-    ItemID";        
            lColWidth += ", 15";                   //  1-    ItemCod";     
            lColWidth += ", 10";                 //  2-    ItemNam; 
            lColWidth += ", 10";                     // 3- Descripti
            lColWidth += ", 5";
     

            // Column Input Length/Width
            lColMaxInputLen = "  0";                 //  0-    ItemID";    
            lColMaxInputLen += ", 0";                  //  1-    ItemCod   
            lColMaxInputLen += ", 0";                  //  2-    ItemNam
            lColMaxInputLen += ", 0";                    // 3- Descripti
            lColMaxInputLen += ", 0";
      
            // Column Min Width
            lColMinWidth = "   0";                      //  0-    ItemID";       
            lColMinWidth += ", 0";                        //  1-    ItemCod;     
            lColMinWidth += ", 0";                        //  2-    ItemNam; 
            lColMinWidth += ", 0";                          // 3- Descripti
            lColMinWidth += ", 0";                        //  4-    SizeNam      
           

            // Column Format
            lColFormat = "   H";                       //  0-    ItemID";       
            lColFormat += ", T";                         //  1-    ItemCod     
            lColFormat += ", T";                         //  2-    ItemNam 
            lColFormat += ", T";                           // 3- Descripti
            lColFormat += ", CH";                         //  4-    SizeNam     
            

            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                       //  0-    ItemID";      
            lColReadOnly += ",0";                         //  1-    ItemCod     
            lColReadOnly += ",0";                         //  2-    ItemNam
            lColReadOnly += ",0";                           // 3- Descripti
            lColReadOnly += ",0";                         //  4-    SizeNam     
    
            // For Saving Time
            tColType += "  H";             //  0-    ItemID"; 
            tColType += ", T";               //  1-    ItemCod
            tColType += ", T";               //  2-    ItemNam
            tColType += ", T";                  // 3- Descripti
            tColType += ", T";               //  4-    SizeNam
           

            tFieldName += "UserID";                //  0-    ItemID";       
            tFieldName += ",Day";             //  1-    ItemCode";     
            tFieldName += ",StartingT";             //  2-    ItemName"; 
            tFieldName += ",EndingT";            // 3- Descripti
            tFieldName += ",Enabled";             //  4-    SizeNamD";       
          
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
                5,
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
            lSQL += " select * from RestrictedAccess ";
            lSQL += " where UserID = " + txtUserID.Text.ToString() + "; ";
            //lSQL += " ORDER BY Day";

            clsDbManager.FillDataGrid(
                grd,
                lSQL,
                fFieldList,
                fColFormat);
        }

        private void AtFormLoad2()
        {
            SettingGridVariable2();
            LoadInitialControls2();
        }

        private void SettingGridVariable2()
        {
            string lHDR2 = "";                       // Column Header
            string lColWidth2 = "";                  // Column Width (Input)
            string lColMinWidth2 = "";               // Column Minimum Width
            string lColMaxInputLen2 = "";            // Column Visible Length/Width 
            string lColFormat2 = "";                 // Column Format  
            string lColReadOnly2 = "";               // Column ReadOnly 1 = ReadOnly, 0 = Read-Write  
            string lFieldList2 = "";
            string tColType2 = "";
            string tFieldName2 = "";

            //
            lFieldList2 = "UserID";
            lFieldList2 += ", UserName";                //  0-    ItemID";       
            lFieldList2 += ", Description";            //  1-    ItemCode";     


            lHDR2 += "User ID";             //  0-    ItemID";      
            lHDR2 += ", Name";            //  1-    ItemCod     
            lHDR2 += ", Description";            //  2-    ItemNam 

            // Col Visible Width
            lColWidth2 = "   5";                 //  0-    ItemID";        
            lColWidth2 += ", 10";                   //  1-    ItemCod";     
            lColWidth2 += ", 20";                 //  2-    ItemNam; 


            // Column Input Length/Width
            lColMaxInputLen2 = "  0";                 //  0-    ItemID";    
            lColMaxInputLen2 += ", 0";                  //  1-    ItemCod   
            lColMaxInputLen2 += ", 0";                  //  2-    ItemNam

            // Column Min Width
            lColMinWidth2 = "   0";                      //  0-    ItemID";       
            lColMinWidth2 += ", 0";                        //  1-    ItemCod;     
            lColMinWidth2 += ", 0";                        //  2-    ItemNam; 


            // Column Format
            lColFormat2 = "   H";                       //  0-    ItemID";       
            lColFormat2 += ", T";                         //  1-    ItemCod     
            lColFormat2 += ", T";                         //  2-    ItemNam 


            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly2 = "  0";                       //  0-    ItemID";      
            lColReadOnly2 += ",0";                         //  1-    ItemCod     
            lColReadOnly2 += ",0";                         //  2-    ItemNam

            // For Saving Time
            tColType2 += "  H";             //  0-    ItemID"; 
            tColType2 += ", T";               //  1-    ItemCod
            tColType2 += ", T";               //  2-    ItemNam


            tFieldName2 += "UserID";                //  0-    ItemID";       
            tFieldName2 += ",UserName";             //  1-    ItemCode";     
            tFieldName2 += ",Description";             //  2-    ItemName"; 

            fHDR2 = lHDR2;
            fColWidth2 = lColWidth2;
            fColMaxInputLen2 = lColMaxInputLen2;
            fColMinWidth2 = lColMinWidth2;
            fColFormat2 = lColFormat2;
            fColReadOnly2 = lColReadOnly2;
            fFieldList2 = lFieldList2;

            fColType2 = tColType2;
            fFieldName2 = tFieldName2;

        }

        private void LoadInitialControls2()
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
            grd2.Rows.Clear();
            grd2.Columns.Clear();

            List<string> lMask2 = null; //new List<string>;
            List<string> lCboFillType2 = null; //new List<string>;
            List<string> lCboTableKeyField2 = null; //new List<string>;
            List<string> lCboQry2 = null; //new List<string>;

            clsDbManager.SetGridHeaderCmb(
                grd2,
                3,
                fHDR2,
                fColWidth2,
                fColMaxInputLen2,
                fColMinWidth2,
                fColFormat2,
                fColReadOnly2,
                "DATA",
                lMask2,
                lCboFillType2,
                lCboTableKeyField2,
                lCboQry2,
                false,
                2);

        }

        private void LoadGridData2()
        {
            string lSQL2 = "";
            lSQL2 += " select * from Users ";
            //lSQL2 += " where UserID = " + txtUserID.Text.ToString() + "; ";

            clsDbManager.FillDataGrid(
                grd2,
                lSQL2,
                fFieldList2,
                fColFormat2);
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            frmGrpUser frm = new frmGrpUser();
            frm.Show();
        }

        private void frmUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            else if (e.KeyCode == Keys.Insert)
            {
                //pnlVocTran.Visible = true;
            }
        }

        private void PopulateRecords()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  

            tSQL = " select UserID, UserName, Password, FullName, Description, Lock, PwdChngNext ";
            tSQL += " from Users "; 
            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Users");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    txtUserID.Text = (ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserID"].ToString());
                    txtUsername.Text = (ds.Tables[0].Rows[0]["UserName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserName"].ToString());
                    txtPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                    txtConfirmPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                    txtFullName.Text = (ds.Tables[0].Rows[0]["FullName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["FullName"].ToString());
                    txtDescription.Text = (ds.Tables[0].Rows[0]["Description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Description"].ToString());
                    

                    if (ds.Tables[0].Rows[0]["Lock"].Equals(1))
                    {
                        chkLock.Checked = true;
                    }
                    else if (ds.Tables[0].Rows[0]["Lock"].Equals(0))
                    {
                        chkLock.Checked = false;
                    }

                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["PwdChngNext"].Equals(1)))
                    {
                        chkChangePassword.Checked = true;
                    }
                    else if (ds.Tables[0].Rows[0]["PwdChngNext"].Equals(0))
                    {
                        chkChangePassword.Checked = false;
                    }

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

        int nextRec = 1;

        private void btnNext_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin 

            if (txtUserID.Text.ToString().Trim(' ', '-') == "")
            {
                nextRec = 1;
            }

            else
            {
                tSQL = "select top 1 UserID from Users where UserID > " + txtUserID.Text.ToString() + " ORDER BY CAST(UserID AS INT)";
                ds = clsDbManager.GetData_Set(tSQL, "Users");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    nextRec = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "1" : ds.Tables[0].Rows[0]["UserID"].ToString());
                }
            }

                tSQL = " select UserID, UserName, Password, FullName, Description, Lock, PwdChngNext ";
                tSQL += " from Users ";
                tSQL += " WHERE UserID = " + nextRec.ToString();
                try
                {
                    ds = clsDbManager.GetData_Set(tSQL, "Users");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dRow = ds.Tables[0].Rows[0];
                        txtUserID.Text = (ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserID"].ToString());
                        txtUsername.Text = (ds.Tables[0].Rows[0]["UserName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserName"].ToString());
                        txtPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                        txtConfirmPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                        txtFullName.Text = (ds.Tables[0].Rows[0]["FullName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["FullName"].ToString());
                        txtDescription.Text = (ds.Tables[0].Rows[0]["Description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Description"].ToString());

                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Lock"].Equals(1)))
                        {
                            chkLock.Checked = true;
                        }
                        else if (ds.Tables[0].Rows[0]["Lock"].Equals(0))
                        {
                            chkLock.Checked = false;
                        }

                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["PwdChngNext"].Equals(1)))
                        {
                            chkChangePassword.Checked = true;
                        }
                        else if (ds.Tables[0].Rows[0]["PwdChngNext"].Equals(0))
                        {
                            chkChangePassword.Checked = false;
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Clear();
                        }
                        LoadGridData();
                        //SumVoc();
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
                }

                nextRec++;
            }
        

        int prevRec = 0;

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin 

            if (txtUserID.Text.ToString().Trim(' ', '-') == "")
            {
                MessageBox.Show("Unable to Navigate");
            }

           else
            {
                tSQL = "select top 1 UserID from Users where UserID < " + txtUserID.Text.ToString() + " ORDER BY CAST(UserID AS INT) DESC";
                ds = clsDbManager.GetData_Set(tSQL, "Users");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    prevRec = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "1" : ds.Tables[0].Rows[0]["UserID"].ToString());
                }
            }
            tSQL = " select UserID, UserName, Password, FullName, Description, Lock, PwdChngNext ";
                tSQL += " from Users ";
                tSQL += " WHERE UserID = " + prevRec.ToString();
                try
                {
                    ds = clsDbManager.GetData_Set(tSQL, "Users");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dRow = ds.Tables[0].Rows[0];
                        txtUserID.Text = (ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserID"].ToString());
                        txtUsername.Text = (ds.Tables[0].Rows[0]["UserName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserName"].ToString());
                        txtPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                        txtConfirmPassword.Text = (ds.Tables[0].Rows[0]["Password"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Password"].ToString());
                        txtFullName.Text = (ds.Tables[0].Rows[0]["FullName"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["FullName"].ToString());
                        txtDescription.Text = (ds.Tables[0].Rows[0]["Description"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Description"].ToString());

                        if (ds.Tables[0].Rows[0]["Lock"].Equals(1))
                        {
                            chkLock.Checked = true;
                        }
                        else if (ds.Tables[0].Rows[0]["Lock"].Equals(0))
                        {
                            chkLock.Checked = false;
                        }

                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["PwdChngNext"].Equals(1)))
                        {
                            chkChangePassword.Checked = true;
                        }
                        else if (ds.Tables[0].Rows[0]["PwdChngNext"].Equals(0))
                        {
                            chkChangePassword.Checked = false;
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Clear();
                        }
                         LoadGridData();
                        //SumVoc();
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
                }

                prevRec--;
            }

        private void btnModify_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
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

        private int GridTNOT(DataGridView pdGv)
        {
            int rtnValue = 0;
            try
            {
                //
                for (int dGVRow = 0; dGVRow < pdGv.Rows.Count; dGVRow++)
                {
                    if (pdGv.Rows[dGVRow].Cells[(int)GColUser.UserID].Value == null)
                    {
                        if (dGVRow == fLastRow)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if ((pdGv.Rows[dGVRow].Cells[(int)GColUser.UserID].Value.ToString()).Trim(' ', '-') == "")
                        {
                            if (dGVRow == fLastRow)
                            {
                                break;
                            }
                        }
                    }

                    rtnValue++;
                } // End For loopo
                return rtnValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Grid TNOT: " + ex.Message, this.Text.ToString());
                return rtnValue;
            }
        }

        private bool SaveData()
        {
            // Code Before Grid Saving 
            //    bool rtnValue = true;
            //    fTErr = 0;
            //    if (fManySQL != null)
            //    {
            //        if (fManySQL.Count() > 0)
            //        {
            //            fManySQL.Clear();
            //        }
            //    }
            //    string lSQL = string.Empty;
            //    DateTime lNow = DateTime.Now;
            //    try
            //    {
            //        ErrrMsg = "";
            //        if (!FormValidation())
            //        {
            //            textAlert.Text = "Form Validation Error: Not Saved." + "  " + lNow.ToString();
            //            MessageBox.Show(ErrrMsg, "Save: " + this.Text.ToString());
            //            return false;
            //        }

            //        fManySQL = new List<string>();

            //        // Prepare Master Doc Query List

            //        if (!PrepareDocMaster())
            //        {
            //            textAlert.Text = "DocMaster: Modifying Doc/Voucher not available for updation.'  ...." + "  " + lNow.ToString();
            //            //tabMDtl.SelectedTab = tabError;
            //            return false;
            //        }
            //        //
            //        DateTime now = DateTime.Now;
            //        textAlert.Text = "selected Box Empty... " + now.ToString("T");
            //        // pending return false;

            //        // Execute Query
            //        if (fManySQL.Count > 0)
            //        {
            //            if (!clsDbManager.ExeMany(fManySQL))
            //            {
            //                MessageBox.Show("Not Saved see log...", this.Text.ToString());
            //                return false;
            //            }
            //            else
            //            {
            //                fLastID = txtUserID.Text.ToString();
            //                ClearThisForm();
            //                return true;
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Data Preparation list empty, Not Saved...", this.Text.ToString());
            //            return false;
            //        } // End Execute Query
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Exception Processing Save: " + ex.Message, "Save Data: " + this.Text.ToString());
            //        return false;
            //    }

            //} // End Save

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
                if (grd.Rows.Count < 1)
                {
                    fTErr++;
                    MessageBox.Show("No transaction in grid to Save", "Save: " + this.Text.ToString());
                    return false;
                }
                fLastRow = grd.Rows.Count - 1;
                if (!FormValidation())
                {
                    textAlert.Text = "Form Validation Error: Not Saved." + "  " + lNow.ToString();
                    MessageBox.Show(ErrrMsg, "Save: " + this.Text.ToString());
                    return false;
                }

                fManySQL = new List<string>();

                // Prepare Master Doc Query List
                fTNOT = GridTNOT(grd);
                if (!PrepareDocMaster())
                {
                    textAlert.Text = "DocMaster: Modifying Doc/Voucher not available for updation.'  ...." + "  " + lNow.ToString();
                    //tabMDtl.SelectedTab = tabError;
                    return false;
                }
                //
                if (grd.Rows.Count > 0)
                {
                    // Prepare Detail Doc Query List
                    if (!PrepareDocDetail())
                    {
                        return false;
                    }
                }
                else
                {
                    DateTime now = DateTime.Now;
                    textAlert.Text = "selected Box Empty... " + now.ToString("T");
                    // pending return false;
                }
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
                        fLastID = txtUserID.Text.ToString();
                        ClearThisForm();
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Data Preparation list empty, Not Saved...", this.Text.ToString());
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
                if (txtUserID.Text.ToString().Trim(' ', '-') == "")
                {
                    strDocId = 0;
                    fDocAlreadyExists = false;
                    //fDocID = fDocID + 1;
                    txtUserID.Text = fDocID.ToString();
                    //fDocID = clsDbManager.GetNextValDocID("Users", "UserID", fDocWhere, "");
                    //fDocID = fDocID + 1;

                    lSQL = "insert into Users (";
                    //lSQL += " UserID ";
                    lSQL += " UserName ";
                    lSQL += ", Password ";
                    lSQL += ", FullName ";
                    lSQL += ", Description ";
                    lSQL += ", Lock ";
                    lSQL += ", Hold";
                    lSQL += ", FailedAttempts";
                    lSQL += ", PwdChngNext";
                    lSQL += " ) values (";
                    //                                                       
                    //lSQL += "" + fDocID.ToString() + "";
                    lSQL += " '" + txtUsername.Text.ToString() + "'";
                    lSQL += ",'" + txtPassword.Text.ToString() + "'";
                    lSQL += ",'" + txtFullName.Text.ToString() + "'";
                    lSQL += ",'" + txtDescription.Text.ToString() + "'";
                    lSQL += ", " + (chkLock.Checked == true ? 1 : 0);
                    lSQL += ", 0";
                    lSQL += ", 0";
                    lSQL += ", " + (chkChangePassword.Checked == true ? 1 : 0);
                    lSQL += ") SELECT @@IDENTITY AS NewIdentity ";


                    dsIdentity = clsDbManager.GetData_Set(lSQL, "Users");
                   // intIdentity = Convert.ToInt16(dsIdentity.Tables[0].Rows[0].ToString());
                    intIdentity = Convert.ToInt16(dsIdentity.Tables[0].Rows[0]["NewIdentity"] == DBNull.Value ? "1" : dsIdentity.Tables[0].Rows[0]["NewIdentity"].ToString());

                    fDocID = intIdentity;

                    //MessageBox.Show("Identity: " + intIdentity.ToString());
                    
                    //fManySQL.Add(lSQL);
                }
                else
                {
                    fDocWhere = " UserID = " + txtUserID.Text.ToString() + "";
                    if (clsDbManager.IDAlreadyExistWw("Users", "UserID", fDocWhere))
                    {
                        fDocAlreadyExists = true;


                        lSQL = "delete from RestrictedAccess ";
                        lSQL += " where " + fDocWhere;

                        fManySQL.Add(lSQL);
                    }

                    lSQL = "update Users set";

                    lSQL += "  UserName = '" + txtUsername.Text.ToString() + "'";
                    lSQL += ", Password = '" + txtPassword.Text.ToString() + "'";
                    lSQL += ", FullName = '" + txtFullName.Text.ToString() + "'";
                    lSQL += ", Description = '" + txtDescription.Text.ToString() + "'";
                    lSQL += ", Lock = " + (chkLock.Checked == true ? 1 : 0);
                    lSQL += ", PwdChngNext = " + (chkChangePassword.Checked == true ? 1 : 0);
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

        //private bool PrepareDocDetail()
        //{
        //    bool rtnValue = true;
        //    string lSQL = "";
        //    try
        //    {
        //        for (int dGVRow = 0; dGVRow < grd.Rows.Count; dGVRow++)
        //        {
        //            if (grd.Rows[dGVRow].Cells[(int)GColUser.UserID].Value == null)
        //            {
        //                if (dGVRow == fLastRow)
        //                {
        //                    continue;
        //                }
        //            }
        //            else
        //            {
        //                if ((grd.Rows[dGVRow].Cells[(int)GColUser.UserID].Value.ToString()).Trim(' ', '-') == "")
        //                {
        //                    //lBlank = true;
        //                    if (dGVRow == fLastRow)
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }

        //            lSQL = "INSERT INTO RestrictedAccess( UserID ";
        //            lSQL += ",Day,StartingT,EndingT,Enabled)";
        //            lSQL += " VALUES (";
        //            lSQL += "" + fDocID + "";
        //            //lSQL += ", '" + txtUserID.Text.ToString() + "'";
        //            //lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColUser.UserID].Value.ToString() + "";
        //            lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColUser.Day].Value.ToString() + "";
        //            lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColUser.StartingT].Value.ToString() + "";
        //            lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColUser.EndingT].Value.ToString() + "";
        //            lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColUser.Enabled].Value.ToString() + "";
        //            lSQL += ")";
        //            fManySQL.Add(lSQL);
        //        } // End For loopo
        //        return rtnValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Save Detail Doc: " + ex.Message, this.Text.ToString());
        //        return false;
        //    }

        //}


        private bool PrepareDocDetail()
        {
            bool rtnValue = true;
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
                    tColType += "  N0";  // UserID = 0,
                    tColType += ", T";  // Day = 1,      
                    tColType += ", T";  // StartingT = 2,
                    tColType += ", T"; // EndingT = 3,
                    tColType += ", CH";  // Enabled = 4   
                    
                    //

                    tFieldName += "  UserID";        // UserID = 0,
                    tFieldName += ", Day";          // Day = 1,      
                    tFieldName += ", StartingT";     // StartingT = 2,
                    tFieldName += ", EndingT";      // EndingT = 3,
                    tFieldName += ", Enabled";       // Enabled = 4   
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
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            SaveData();
            textAlert.Text = "Data Saved Successfully";
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
                        chkLock.Checked = false;
                    }
                    else
                        func(control.Controls);
            };

            func(Controls);
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

        private void btnFirst_Click(object sender, EventArgs e)
        {

        }

        private void btnLast_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            LoadGridData2();
            //grd2.Refresh();
            //MessageBox.Show("View Tab Clicked");
        }
    }
}