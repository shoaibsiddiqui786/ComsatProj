using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_Task
{
    public partial class frmPrivilages : Form
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

        public frmPrivilages()
        {
            InitializeComponent();
        }

        private void form_Privilages_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            AtFormLoad();
        }

        private void AtFormLoad()
        {
            string lSQL = string.Empty;

            lSQL = " select groupid, GroupName from Groups ";
            lSQL += " ORDER BY GroupName ";

            clsFillCombo.FillCombo(cboGroup, clsGVar.ConString1, "Groups" + "," + "groupid" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboGroup.SelectedValue);

            SettingGridVariable();
            LoadInitialControls();
            LoadGridData();
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
           
            lFieldList += " UserID";                //  0-    ItemID";       
            lFieldList += ", UserName";            //  1-    ItemCode"; 
            lFieldList += ", FullName";
            lFieldList += ", AllowAddNew";
            lFieldList += ", AllowModify";
            lFieldList += ", AllowPrint";
            lFieldList += ", AllowDelete";
            lFieldList += ", AllowPost";
            lFieldList += ", AllowUnPost";
            lFieldList += ", AllowView";



            lHDR += " User ID";           // UserID";          
            lHDR += ", User Name";        // UserName";        
            lHDR += ", Full Name";        // FullName"; 
            lHDR += ", Add New";          // AddNew";
            lHDR += ", Modify Existing";  // ModifyExisting";
            lHDR += ", Print";            // Print";
            lHDR += ", Delete";           // Delete";
            lHDR += ", Post/Cancel";      // PostCancel";
            lHDR += ", Unpost";           // Unpost";
            lHDR += ", View";             // View";

            // Col Visible Width
            lColWidth = "   5";                 // UserID";           
            lColWidth += ", 10";                // UserName";          
            lColWidth += ", 15";                // FullName"; 
            lColWidth += ", 5";                 // AddNew";
            lColWidth += ", 5";                 // ModifyExisting";
            lColWidth += ", 5";                 // Print";
            lColWidth += ", 5";                 // Delete";
            lColWidth += ", 7";                 // PostCancel";
            lColWidth += ", 5";                 // Unpost";
            lColWidth += ", 5";                 // View";


            // Column Input Length/Width
            lColMaxInputLen = "  0";                // UserID";           
            lColMaxInputLen += ", 0";               // UserName";        
            lColMaxInputLen += ", 0";               // FullName"; 
            lColMaxInputLen += ", 0";               // AddNew";
            lColMaxInputLen += ", 0";               // ModifyExisting";
            lColMaxInputLen += ", 0";               // Print";
            lColMaxInputLen += ", 0";               // Delete";
            lColMaxInputLen += ", 0";               // PostCancel";
            lColMaxInputLen += ", 0";               // Unpost";
            lColMaxInputLen += ", 0";               // View";

            // Column Min Width
            lColMinWidth = "   0";                     // UserID";             
            lColMinWidth += ", 0";                     // UserName";         
            lColMinWidth += ", 0";                     // FullName"; 
            lColMinWidth += ", 0";                     // AddNew";
            lColMinWidth += ", 0";                     // ModifyExisting";
            lColMinWidth += ", 0";                     // Print";
            lColMinWidth += ", 0";                     // Delete";
            lColMinWidth += ", 0";                     // PostCancel";
            lColMinWidth += ", 0";                     // Unpost";
            lColMinWidth += ", 0";                     // View";


            // Column Format
            lColFormat = "   H";                      // UserID";            
            lColFormat += ", T";                      // UserName";        
            lColFormat += ", T";                      // FullName"; 
            lColFormat += ", CH";                      // AddNew";
            lColFormat += ", CH";                      // ModifyExisting";
            lColFormat += ", CH";                      // Print";
            lColFormat += ", CH";                      // Delete";
            lColFormat += ", CH";                      // PostCancel";
            lColFormat += ", CH";                      // Unpost";
            lColFormat += ", CH";                      // View";


            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                      // UserID";           
            lColReadOnly += ",0";                      // UserName";        
            lColReadOnly += ",0";                      // FullName"; 
            lColReadOnly += ",0";                      // AddNew";
            lColReadOnly += ",0";                      // ModifyExisting";
            lColReadOnly += ",0";                      // Print";
            lColReadOnly += ",0";                      // Delete";
            lColReadOnly += ",0";                      // PostCancel";
            lColReadOnly += ",0";                      // Unpost";
            lColReadOnly += ",0";                      // View";

            // For Saving Time
            tColType += "  H";            // UserID";          
            tColType += ", T";            // UserName";        
            tColType += ", T";            // FullName"; 
            tColType += ", T";           // AddNew";
            tColType += ", T";           // ModifyExisting";
            tColType += ", T";           // Print";
            tColType += ", T";           // Delete";
            tColType += ", T";           // PostCancel";
            tColType += ", T";           // Unpost";
            tColType += ", T";           // View";


            tFieldName += "UserID";             // UserID";             
            tFieldName += ",UserName";          // UserName";        
            tFieldName += ",FullName";          // FullName"; 
            tFieldName += ",AllowAddNew";            // AddNew";
            tFieldName += ",AllowModify";    // ModifyExisting";
            tFieldName += ",AllowPrint";             // Print";
            tFieldName += ",AllowDelete";            // Delete";
            tFieldName += ",AllowPost";       // PostCancel";
            tFieldName += ",AllowUnpost";            // Unpost";
            tFieldName += ",AllowView";              // View";

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
                10,
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
            string lSQL = string.Empty;
            lSQL = " SELECT f.UserID, u.UserName, u.FullName, f.AllowAddNew, f.AllowModify, ";
            lSQL += " f.AllowPrint, f.AllowDelete, f.AllowPost, f.AllowUnPost, f.AllowView ";
            lSQL += " FROM FormPrivileges f INNER JOIN Users u ON u.UserID = f.UserID ";

            clsDbManager.FillDataGrid(
                grd,
                lSQL,
                fFieldList,
                fColFormat);
        }


        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrivilages_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
