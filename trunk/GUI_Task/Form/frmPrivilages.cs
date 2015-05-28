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
           
            lFieldList += ", UserID";                //  0-    ItemID";       
            lFieldList += ", UserName";            //  1-    ItemCode"; 
            lFieldList += ", FullName"; 
            lFieldList += ", AddNew";
            lFieldList += ", ModifyExisting";
            lFieldList += ", Print";
            lFieldList += ", Delete";
            lFieldList += ", Post/Cancel";
            lFieldList += ", Unpost";
            lFieldList += ", View";



            lHDR += " User ID";             //  0-    ItemID";      
            lHDR += ", User Name";            //  1-    ItemCod     
            lHDR += ", Full Name";
            lHDR += ", Add New";            //  2-    ItemNam 
            lHDR += ", Modify Existing";
            lHDR += ", Print";
            lHDR += ", Delete";
            lHDR += ", Post/Cancel";
            lHDR += ", Unpost";
            lHDR += ", View";

            // Col Visible Width
            lColWidth = "   5";                 //  0-    ItemID";        
            lColWidth += ", 10";                   //  1-    ItemCod";     
            lColWidth += ", 15";
            lColWidth += ", 5";                 //  2-    ItemNam; 
            lColWidth += ", 5";
            lColWidth += ", 5";
            lColWidth += ", 5";
            lColWidth += ", 7";
            lColWidth += ", 5";
            lColWidth += ", 5";


            // Column Input Length/Width
            lColMaxInputLen = "  0";                 //  0-    ItemID";    
            lColMaxInputLen += ", 0";                  //  1-    ItemCod 
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";                  //  2-    ItemNam
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";
            lColMaxInputLen += ", 0";  

            // Column Min Width
            lColMinWidth = "   0";                      //  0-    ItemID";       
            lColMinWidth += ", 0";                        //  1-    ItemCod;   
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";                        //  2-    ItemNam; 
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";
            lColMinWidth += ", 0";


            // Column Format
            lColFormat = "   T";                       //  0-    ItemID";       
            lColFormat += ", T";                         //  1-    ItemCod    
            lColFormat += ", T";
            lColFormat += ", T";                         //  2-    ItemNam 
            lColFormat += ", T";
            lColFormat += ", T";
            lColFormat += ", T";
            lColFormat += ", T";
            lColFormat += ", T";
            lColFormat += ", T";


            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                       //  0-    ItemID";      
            lColReadOnly += ",0";                         //  1-    ItemCod   
            lColReadOnly += ",0";
            lColReadOnly += ",0";                         //  2-    ItemNam
            lColReadOnly += ",0";
            lColReadOnly += ",0";
            lColReadOnly += ",0";
            lColReadOnly += ",0";
            lColReadOnly += ",0";
            lColReadOnly += ",0";

            // For Saving Time
            tColType += "  H";             //  0-    ItemID"; 
            tColType += ", T";               //  1-    ItemCod
            tColType += ", T";
            tColType += ", CH";               //  2-    ItemNam
            tColType += ", CH";
            tColType += ", CH";
            tColType += ", CH";
            tColType += ", CH";
            tColType += ", CH";
            tColType += ", CH";


            tFieldName += "UserID";                //  0-    ItemID";       
            tFieldName += ",UserName";             //  1-    ItemCode";
            tFieldName += ",FullName";
            tFieldName += ",AddNew";             //  2-    ItemName"; 
            tFieldName += ",ModifyExisting";
            tFieldName += ",Print";
            tFieldName += ",Delete";
            tFieldName += ",Post/Cancel";
            tFieldName += ",Unpost";
            tFieldName += ",View";

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
            string lSQL = "";
            lSQL += " select UserID, UserName, FullName from Users Order By UserName ";

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
