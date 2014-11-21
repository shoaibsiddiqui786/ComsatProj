﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUI_Task.Class;
using GUI_Task.StringFun01;

namespace GUI_Task
{

    enum GColGRNRet
    {
        ItemID = 0,
        ItemCode = 1,
        ItemName = 2,
        VenderName = 3,
        SizeName = 4,
        ColorName = 5,
        UnitName = 6,
        GodownName = 7,
        Stock = 8,
        GRNQty = 9,
        RTNQty = 10,
        NewStock = 11,
        Rate = 12,
        Amount = 13,
        SizeID = 14,
        ColorID = 15,
        UOMID = 16,
        GodownID = 17
    }
    
    
    public partial class frmGRNReturn : Form
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
        bool fDocAlreadyExists = false;             // Check if Doc/voucher already exists (Edit Mode = True, New Mode = false)

        bool blnFormLoad = true;
        int fcboDefaultValue = 0;
        public frmGRNReturn()
        {
            InitializeComponent();
        }

        private void frmGdRecNoteReturn_Load(object sender, EventArgs e)
        {
            AtFormLoad();
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
                18,
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

        private void AtFormLoad()
        {
            string lSQL = string.Empty;
            //SettingGridVariable();
            LoadInitialControls();
            this.KeyPreview = true;

            //Size Combo Fill
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmSize);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cbo_I_Size, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cbo_I_Size.SelectedValue);

            DataSet ds = new DataSet();
            ds = clsDbManager.GetData_Set(lSQL, "CatDtl");
            SizeColumn.DataSource = ds.Tables[0];
            SizeColumn.ValueMember = ds.Tables[0].Columns[0].ToString();
            SizeColumn.DisplayMember = ds.Tables[0].Columns[1].ToString();
            ds.Clear();

            //Color Combo Fill
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmColor);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cbo_I_Color, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cbo_I_Color.SelectedValue);

            ds = clsDbManager.GetData_Set(lSQL, "CatDtl");
            ColorColumn.DataSource = ds.Tables[0];
            ColorColumn.ValueMember = ds.Tables[0].Columns[0].ToString();
            ColorColumn.DisplayMember = ds.Tables[0].Columns[1].ToString();
            ds.Clear();

            //UOM Combo Fill
            lSQL = "select UOMID, UnitName from IMS_UOM order by UnitName";

            clsFillCombo.FillCombo(cbo_I_UOM, clsGVar.ConString1, "IMS_UOM" + "," + "UnitName" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cbo_I_UOM.SelectedValue);

            ds = clsDbManager.GetData_Set(lSQL, "IMS_UOM");
            UnitColumn.DataSource = ds.Tables[0];
            UnitColumn.ValueMember = ds.Tables[0].Columns[0].ToString();
            UnitColumn.DisplayMember = ds.Tables[0].Columns[1].ToString();
            ds.Clear();



           

            //Emp Code Combo Fill
            lSQL = "SELECT employeeid, first_name +' '+ last_name AS EmpName FROM PR_Employee ";
            //lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cboEmpCode, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboEmpCode.SelectedValue);

            //Type combo
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmType);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cboType, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboType.SelectedValue);


            ////LC cOMBO
            //lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmLC);
            //lSQL += " order by cgdDesc";

            //clsFillCombo.FillCombo(cboLC, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            //fcboDefaultValue = Convert.ToInt16(cboLC.SelectedValue);

            //Item group cOMBO
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmItemGroup);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cboItemGroup, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboItemGroup.SelectedValue);

            //Godown cOMBO
            lSQL = "select cgdCode, cgdDesc from catdtl where cgcode=" + Convert.ToString((int)Category.enmGodown);
            lSQL += " order by cgdDesc";

            clsFillCombo.FillCombo(cboGodown, clsGVar.ConString1, "CatDtl" + "," + "cgdCode" + "," + "False", lSQL);
            fcboDefaultValue = Convert.ToInt16(cboGodown.SelectedValue);

            ds = clsDbManager.GetData_Set(lSQL, "IMS_UOM");
            GodownColumn.DataSource = ds.Tables[0];
            GodownColumn.ValueMember = ds.Tables[0].Columns[0].ToString();
            GodownColumn.DisplayMember = ds.Tables[0].Columns[1].ToString();
            ds.Clear();

        }

        private void LookUp_Voc()
        {
            frmLookUp sForm = new frmLookUp(
                    "g.GRNId",
                    "g.TypeId, g.LCId, cd.cgdDesc AS ItemGroupName, gd.Qty ",
                    " GRN g INNER JOIN CatDtl cd on g.ItemGroupID = cd.cgdCode AND cd.cgCode = 6 INNER JOIN GRNDetail gd on gd.GRNId = g.GRNId ",
                    this.Text.ToString(),
                    1,
                    " GRN ID, GateInward No, Date, Item Group, Gate",
                    "8,12,8,12,8",
                    " T, T, T, T, T",
                    true,
                    "",
                    "",
                    "TextBox"
                   
                    );

            txtGR.Text = string.Empty;
            sForm.lupassControl = new frmLookUp.LUPassControl(PassDataVocID);
            sForm.ShowDialog();

            if (txtGR.Text != null)
            {
                if (txtGR.Text != null)
                {
                    if (txtGR.Text.ToString() == "" || txtGR.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (txtGR.Text.ToString().Trim().Length > 0)
                    {
                        PopulateRecords();
                    }

                }

            }
        }
        //#endregion

        private void PassDataVocID(object sender)
        {
            txtGR.Text = ((TextBox)sender).Text;
        }


        private void PopulateRecords()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  

            tSQL = "select g.Date, g.GateInward, g.GRNId, cd.cgdDesc AS ItemGroupName, gd.Qty";
            tSQL += " from GRN f INNER JOIN CatDtl cd on g.ItemGroupID = cd.cgdCode AND cd.cgCode = 6 ";
            tSQL += " INNER JOIN GRNDetail gd on gd.GRNId = g.GRNId";
            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Heads");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dRow = ds.Tables[0].Rows[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Clear();
                    }
                    LoadGridData();
                }
            }
            catch
            {
                MessageBox.Show("Unable to Get Account Code...", this.Text.ToString());
            }
        }



        private void LoadGridData()
        {
            string lSQL = "";

            lSQL = " SELECT i.ItemId AS Code, i.ItemCode, i.Name AS ItemName, sz.cgdDesc AS SizeName, clr.cgdDesc AS ColorName, ";
            lSQL += " u.UnitName, grt.cgdDesc AS GodownName, 0 AS Stock, grt.GRNQty,grt.RTNQty, dd.SizeId, dd.ColorId, u.UOMID, dd.GodownId ";
            lSQL += " FROM GRNDetail grt ";
            lSQL += " INNER JOIN Item i on i.ItemId = grt.ItemId ";
            lSQL += " INNER JOIN CatDtl sz on grt.SizeId = sz.cgdCode AND sz.cgCode = 5 ";
            lSQL += " INNER JOIN CatDtl clr on grt.ColorId = clr.cgdCode AND clr.cgCode = 3 ";
            lSQL += " INNER JOIN IMS_UOM u on u.UOMID = u.UOMID ";
            lSQL += " INNER JOIN CatDtl gdw on grt.GodownId = gdw.cgdCode AND gdw.cgCode = 2 ";
            lSQL += " where grt.GRNId ='" + txtGR.Text.ToString() + "'; ";

            clsDbManager.FillDataGrid(
                grd,
                lSQL,
                fFieldList,
                fColFormat);
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
            lFieldList = "Code";                    //  0-    ItemID";       
            lFieldList += ",ItemCode";              //  1-    ItemCode";     
            lFieldList += ",ItemName";              //  2-    ItemName";
            lFieldList += ",Desription";                   //  8-    Vender Name
            lFieldList += ",SizeName";              //  3-    SizeName";       
            lFieldList += ",ColorName";             //  4-    ColorName";      
            lFieldList += ",UnitName";              //  5-    UOMName";
            lFieldList += ",GodownName";            //  6- GodownName
            lFieldList += ",Stock";                 //  7- Stock
            lFieldList += ",GRnQty";                   //  8- Qty
            lFieldList += ",RTNQty";                   //  8- Qty
            lFieldList += ",New Stock";                   //  8- Qty
            lFieldList += ",Rate";                   //  8- Qty
            lFieldList += ",Amount";                   //  8- Qty

            lFieldList += ",SizeID";                //  9    SizeID";     
            lFieldList += ",ColorID";               //  10    ColorID";     
            lFieldList += ",UOMID";                 //  11    UOMID"; 
            lFieldList += ",GodownId";              //  12- GodownID


            lHDR += "Item ID";            //  0-    ItemID";       
            lHDR += ",Item Code";         //  1-    ItemCode";     
            lHDR += ",Item Name";         //  2-    ItemName";
            lHDR += ",Description";         //  2-    ItemName";
            lHDR += ",Size";       //  3-    SizeName";      
            lHDR += ",Color";              //  4-    ColorName";     
            lHDR += ",UOM Name";             //  5-    UOMName";
            lHDR += ",Godown";          //  6- GodownName
            lHDR += ",Stock";            //  7- Stock
            lHDR += ",GRNQty";               //  8- Qty
            lHDR += ",RTNQty";               //  8- Qty
            lHDR += ",New Stock";               //  8- Qty
            lHDR += ",Rate";               //  8- Qty
            lHDR += ",Amount";               //  8- Qty
            lHDR += ",SizeID";            //  9    SizeID";     
            lHDR += ",ColorID";           //  10    ColorID";     
            lHDR += ",UOMID";             //  11    UOMID"; 
            lHDR += ",GodownId";          //  12- GodownID

            // Col Visible Width
            lColWidth = "   5";                 //  0-    ItemID";       
            lColWidth += ",12";                 //  1-    ItemCode";     
            lColWidth += ",20";                 //  2-    ItemName";
            lColWidth += ",20";                 //  2-    VenderName";
            lColWidth += ",10";                 //  3-    SizeName";      
            lColWidth += ", 7";                 //  4-    ColorName";     
            lColWidth += ", 7";                 //  5-    UOMName";
            lColWidth += ", 7";                 //  6- GodownName
            lColWidth += ", 7";                 //  7- Stock
            lColWidth += ", 20";                 //  8- Qty
            lColWidth += ", 20";                 //  8- Qty
            lColWidth += ", 5";                 //  8- New Stock
            lColWidth += ", 5";                 //  8- Rate
            lColWidth += ", 5";                 //  8- Amount
            lColWidth += ", 5";                 //  9    SizeID";     
            lColWidth += ", 5";                 //  10    ColorID";     
            lColWidth += ", 5";                 //  11    UOMID"; 
            lColWidth += ", 5";                 //  12- GodownID

            // Column Input Length/Width
            lColMaxInputLen = "  0";                 //  0-    ItemID";       
            lColMaxInputLen += ", 0";                //  1-    ItemCode";     
            lColMaxInputLen += ", 0";                //  2-    ItemName";
            lColMaxInputLen += ", 0";                //  2-    VenderName";
            lColMaxInputLen += ", 0";                //  3-    SizeName";      
            lColMaxInputLen += ", 0";                //  4-    ColorName";     
            lColMaxInputLen += ", 0";                //  5-    UOMName";
            lColMaxInputLen += ", 0";                //  6- GodownName
            lColMaxInputLen += ", 0";                //  7- Stock
            lColMaxInputLen += ", 0";                //  8- Qty
            lColMaxInputLen += ", 0";                //  8- Qty
            lColMaxInputLen += ", 0";                //  2-    NewStock";
            lColMaxInputLen += ", 0";                //  2-    Rate";
            lColMaxInputLen += ", 0";                //  2-    Amount";
            lColMaxInputLen += ", 0";                //  9    SizeID";     
            lColMaxInputLen += ", 0";                //  10    ColorID";     
            lColMaxInputLen += ", 0";                //  11    UOMID"; 
            lColMaxInputLen += ", 0";                //  12- GodownID

            // Column Min Width
            lColMinWidth = "   0";                      //  0-    ItemID";           
            lColMinWidth += ", 0";                      //  1-    ItemCode";         
            lColMinWidth += ", 0";                      //  2-    ItemName";
            lColMinWidth += ", 0";                      //  2-    VenderName";
            lColMinWidth += ", 0";                      //  3-    SizeName";      
            lColMinWidth += ", 0";                      //  4-    ColorName";         
            lColMinWidth += ", 0";                      //  5-    UOMName";    
            lColMinWidth += ", 0";                      //  6- GodownName
            lColMinWidth += ", 0";                      //  7- Stock
            lColMinWidth += ", 0";                      //  8- Qty
            lColMinWidth += ", 0";                      //  8- Qty
            lColMinWidth += ", 0";                      //  2-    New Stock";
            lColMinWidth += ", 0";                      //  2-    Rate";
            lColMinWidth += ", 0";                      //  2-    Amount";
            lColMinWidth += ", 0";                      //  9    SizeID";       
            lColMinWidth += ", 0";                      //  10    ColorID";        
            lColMinWidth += ", 0";                      //  11    UOMID"; 
            lColMinWidth += ", 0";                      //  12- GodownID

            // Column Format
            lColFormat = "   T";                       //  0-    ItemID";            
            lColFormat += ", T";                       //  1-    ItemCode";         
            lColFormat += ", T";                       //  2-    ItemName";
            lColFormat += ", T";                       //  2-    VenderName";
            lColFormat += ", T";                       //  3-    SizeName";      
            lColFormat += ", T";                       //  4-    ColorName";         
            lColFormat += ", T";                       //  5-    UOMName";    
            lColFormat += ", T";                       //  6- GodownName
            lColFormat += ", T";                       //  7- Stock
            lColFormat += ", T";                       //  8- Qty
            lColFormat += ", T";                       //  8- Qty
            lColFormat += ", T";                       //  2-    New Stock";
            lColFormat += ", T";                       //  2-    Rate";
            lColFormat += ", T";                       //  2-    Amount";

            lColFormat += ", H";                       //  9    SizeID";       
            lColFormat += ", H";                       //  10    ColorID";        
            lColFormat += ", H";                       //  11    UOMID"; 
            lColFormat += ", H";                       //  12- GodownID

            // Column ReadOnly 1= readonly, 0 = read-write
            lColReadOnly = "  0";                      //  0-    ItemID";       
            lColReadOnly += ",1";                      //  1-    ItemCode";     
            lColReadOnly += ",1";                      //  2-    ItemName";
            lColReadOnly += ",0";                      //  2-    VenderName";
            lColReadOnly += ",0";                      //  3-    SizeName";      
            lColReadOnly += ",0";                      //  4-    ColorName";     
            lColReadOnly += ",0";                      //  5-    UOMName";
            lColReadOnly += ",0";                      //  6- GodownName
            lColReadOnly += ",0";                      //  7- Stock
            lColReadOnly += ",0";                      //  8- Qty
            lColReadOnly += ",0";                      //  8- Qty
            lColReadOnly += ",0";                      //  2-    New Stock";
            lColReadOnly += ",0";                      //  2-    Rate";
            lColReadOnly += ",0";                      //  2-    Amount";
            lColReadOnly += ",1";                      //  9    SizeID";     
            lColReadOnly += ",1";                      //  10    ColorID";     
            lColReadOnly += ",1";                      //  11    UOMID"; 
            lColReadOnly += ",1";                      //  12- GodownID

            // For Saving Time
            tColType += "  N0";             //  0-    ItemID";       
            tColType += ",SKP";             //  1-    ItemCode";     
            tColType += ",SKP";             //  2-    ItemName";
            tColType += ",SKP";             //  2-    ItemName";
            tColType += ",SKP";              //  3-    SizeName";      
            tColType += ",SKP";             //  4-    ColorName";     
            tColType += ",SKP";             //  5-    UOMName";
            tColType += ",SKP";             //  6- GodownName
            tColType += ", N0";             //  7- Stock
            tColType += ", N0";             //  8- Qty
            tColType += ", N0";             //  8- Qty
            tColType += ",N0";             //  2-    NewStock";
            tColType += ",N0";             //  2-    Rate";
            tColType += ",N0";             //  2-    Amount";
            tColType += ", N0";             //  9    SizeID";     
            tColType += ", N0";             //  10    ColorID";     
            tColType += ", N0";             //  11    UOMID"; 
            tColType += ", N0";             //  12- GodownID

            tFieldName += "Code";               //  0-    ItemID";        
            tFieldName += ",ItemCode";          //  1-    ItemCode";        
            tFieldName += ",ItemName";          //  2-    ItemName";
            tFieldName += ",VDescription";          //  2-    ItemName";
            tFieldName += ",SizeName";       //  3-    SizeName";      
            tFieldName += ",ColorName";          //  4-    ColorName";          
            tFieldName += ",UnitName";         //  5-    UOMName";     
            tFieldName += ",GodownName";          //  6- GodownName
            tFieldName += ",Stock";        //  7- Stock
            tFieldName += ",GRNQty";               //  8- Qty
            tFieldName += ",RTNQty";               //  8- Qty
            tFieldName += ",NewStock";          //  2-    New Stock";
            tFieldName += ",Rate";          //  2-    Rate";
            tFieldName += ",Amount";          //  2-    Amount";
            tFieldName += ",SizeID";            //  9    SizeID";     
            tFieldName += ",ColorID";           //  10    ColorID";      
            tFieldName += ",UOMID";             //  11    UOMID"; 
            tFieldName += ",GodownID";          //  12- GodownID

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
                        fLastID = txtGR.Text.ToString();
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

        private int GridTNOT(DataGridView pdGv)
        {
            int rtnValue = 0;
            try
            {
                //
                for (int dGVRow = 0; dGVRow < pdGv.Rows.Count; dGVRow++)
                {
                    if (pdGv.Rows[dGVRow].Cells[(int)GCol.ItemID].Value == null)
                    {
                        if (dGVRow == fLastRow)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if ((pdGv.Rows[dGVRow].Cells[(int)GCol.ItemID].Value.ToString()).Trim(' ', '-') == "")
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
        private void ClearThisForm()
        {
            if (grd.Rows.Count > 0)
            {
                grd.Rows.Clear();
            }
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

        private bool PrepareDocMaster()
        {
            bool rtnValue = true;
            string lSQL = string.Empty;

            try
            {
                if (txtGR.Text.ToString().Trim(' ', '-') == "")
                {
                    fDocAlreadyExists = false;
                    fDocID = clsDbManager.GetNextValDocID("GRNRet", "GRNREtId", fDocWhere, "");

                    lSQL = "insert into GRNRet (";
                    lSQL += "  GRNRetId ";                              //  0-    ItemID";   
                    lSQL += ", Date ";
                    lSQL += "  GRNId ";   //  1-    ItemCod  
                    lSQL += ", GRNDate ";
                    lSQL += "  VendorId ";
                    lSQL += "  PurchaserId ";
                    lSQL += "  TypeId ";
                    //lSQL += "  LCId ";
                    lSQL += ", ItemGroupID ";                                        //  2-    ItemNam 
                   // lSQL += ", GateID ";                                      // 3- Descripti
                    //  4-    SizeNam  
                    lSQL += ",Note ";
                    lSQL += ", OrgId ";
                    lSQL += ", UserId ";
                    lSQL += ", Status ";
                    lSQL += ", BranchId ";
                    //lSQL += ",EmployeeID ";
                    //lSQL += ",DeptID ";
                    //lSQL += ",RecPerName ";
                    //lSQL += ",Discount ";
                    lSQL += " ) values (";
                    //                                                       
                    lSQL += "'" + fDocID.ToString() + "'";
                    lSQL += ",'" + txtGR.Text.ToString() + "";
                    lSQL += ", " + StrF01.D2Str(dtpGRNRet) + "";
                    lSQL += ", 1";
                    lSQL += ", " + StrF01.D2Str(dtpGRN) + "";
                    lSQL += ", 1";
                    lSQL += ", 1";
                    lSQL += ", 1";
                    lSQL += ", 1";
                    //lSQL += ", 1";
                    //lSQL += ", 1";
                    lSQL += ", " + cboItemGroup.SelectedValue.ToString() + "";
                   // lSQL += ", " + cboGate.SelectedValue.ToString() + "";
                    lSQL += ", 1";
                    lSQL += ",'" + txtNote + "'";
                    lSQL += ", " + cboGodown.SelectedValue.ToString() + "";
                    lSQL += ", 1";
                  //  lSQL += ", " + txtRecPerName.Text.ToString() + "";
                   // lSQL += ", " + txtDiscount.Text.ToString() + "";
                    lSQL += ")";
                }
                else
                {
                    fDocWhere = " GRNRetId = '" + txtGR.Text.ToString() + "'";
                    if (clsDbManager.IDAlreadyExistWw("GRNRet", "GRNRetId", fDocWhere))
                    {

                        fDocAlreadyExists = true;
                        lSQL = "delete from GRNRetDetail ";
                        lSQL += " where " + fDocWhere;

                        fManySQL.Add(lSQL);
                        //
                    }

                    lSQL = "update GRNRet set";
                    lSQL += "  Date = '" + StrF01.D2Str(dtpGRNRet.Value) + "'";
                    lSQL += "  GRNDate = '" + StrF01.D2Str(dtpGRN.Value) + "'";
                    //lSQL += ", Vendorid = " + mskVenderCode.SelectedValue.ToString() + "";
                    //lSQL += ", Purchaserid = " + mskPurchaseCode.SelectedValue.ToString() + "";
                    lSQL += ", Typeid = " + cboType.SelectedValue.ToString() + "";
                    //lSQL += ", LCid = " + cboLC.SelectedValue.ToString() + "";
                    lSQL += ", ItemGroupID = " + cboItemGroup.SelectedValue.ToString() + "";
                    //lSQL += ", GateID = " + cboGate.SelectedValue.ToString() + "";
                    lSQL += ", Note = '" + txtNote.Text.ToString() + "'";
                    lSQL += ", OrgID = " + cboItemGroup.SelectedValue.ToString() + "";
                    //lSQL += ", Category = '" + cboCategory.SelectedValue.ToString() + "'";
                    lSQL += ", UserID = 1";
                    lSQL += ",Status = 0";
                    lSQL += ", BranchID = 1";
                    //lSQL += ", EmployeeID = " + cboEmpCode.SelectedValue.ToString() + "";
                    //lSQL += ", DeptID = 1";
                    //lSQL += ", ChargesTmplID = '" + txtChargesTempNo.Text.ToString() + "'";
                    //lSQL += ", ChargesTmplDes = '" + txtChargesDes.Text.ToString() + "'";
                    lSQL += " where ";
                    lSQL += fDocWhere;

                }
                fManySQL.Add(lSQL);


                return rtnValue;
            }
            catch (Exception ex)
            {
                rtnValue = false;
                MessageBox.Show("Save Master Doc: " + ex.Message, this.Text.ToString());
                return false;
            }
        }

        private bool PrepareDocDetail()
        {
            bool rtnValue = true;
            string lSQL = "";
            try
            {
                for (int dGVRow = 0; dGVRow < grd.Rows.Count; dGVRow++)
                {
                    if (grd.Rows[dGVRow].Cells[(int)GCol.ItemID].Value == null)
                    {
                        if (dGVRow == fLastRow)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ((grd.Rows[dGVRow].Cells[(int)GCol.ItemID].Value.ToString()).Trim(' ', '-') == "")
                        {
                            //lBlank = true;
                            if (dGVRow == fLastRow)
                            {
                                continue;
                            }
                        }
                    }

                    lSQL = "INSERT INTO GRNRetDetail (GRNRetId";
                    lSQL += ",ItemId,SizeId,ColorID,GodownId,GRNQty,RTNQty,Rate,Amount)";
                    lSQL += " VALUES (";
                    lSQL += "'" + txtGR.Text.ToString() + "'";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.ItemID].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.SizeID].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.ColorID].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.GodownID].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.GRNQty].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.RTNQty].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.Rate].Value.ToString() + "";
                    lSQL += ", " + grd.Rows[dGVRow].Cells[(int)GColGRNRet.Amount].Value.ToString() + "";
                    lSQL += ")";
                    fManySQL.Add(lSQL);
                } // End For loopo
                return rtnValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Detail Doc: " + ex.Message, this.Text.ToString());
                return false;
            }

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
                //SumVoc();

                return lRtnValue;

            }
            catch (Exception ex)
            {
                fTErr++;
                ErrrMsg = StrF01.BuildErrMsg(ErrrMsg, "Exception: FormValidation -> " + ex.Message.ToString());
                return false;
            }
        }



        private void mskVenderCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LookUp_GL();
        }

        private void LookUp_GL()
        {
            //SELECT Code, Name FROM Heads WHERE TYPE = 'A'
            frmLookUp sForm = new frmLookUp(
                    "Code",
                    "Name",
                    "Vendor",
                    this.Text.ToString(),
                    1,
                    "Code,Name",
                    "16,40",
                    " T, T",
                    true,
                    "",
                    " Type='A'",
                    "TextBox"
                    );

            mskVenderCode.Mask = "";
            mskVenderCode.Text = string.Empty;
            mskVenderCode.Mask = clsGVar.maskGLCode;

            sForm.lupassControl = new frmLookUp.LUPassControl(PassData);
            sForm.ShowDialog();
            if (mskVenderCode.Text != null)
            {
                if (mskVenderCode.Text != null)
                {
                    if (mskVenderCode.Text.ToString() == "" || mskVenderCode.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (mskVenderCode.Text.ToString().Trim().Length > 0)
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
            mskVenderCode.Mask = "";
            mskVenderCode.Text = ((TextBox)sender).Text;
            mskVenderCode.Mask = clsGVar.maskGLCode;
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

            tSQL = "SELECT Name ";
            tSQL += " from Vendor ";
            tSQL += " where Code ='" + mskVenderCode.Text.ToString() + "';";

            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Vendor");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //fAlreadyExists = true;
                    dRow = ds.Tables[0].Rows[0];
                    // Starting title as 0
                    lblNameUp.Text = (ds.Tables[0].Rows[0]["Name"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Name"].ToString());
                    
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

        private void mskVenderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                LookUp_GL();
            }
        }

        private void mskPurchaseCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        
            LookUp_GL1();
        
        }

        private void mskPurchaseCode_KeyDown(object sender, KeyEventArgs e)
          {
            LookUp_GL1();
        }

        private void LookUp_GL1()
        {
            //SELECT Code, Name FROM Heads WHERE TYPE = 'A'
            frmLookUp sForm = new frmLookUp(
                    "Code",
                    "Name",
                    "Customer",
                    this.Text.ToString(),
                    1,
                    " Code, Name",
                    "16,40",
                    " T, T",
                    true,
                    "",
                    " Type='A'",
                    "TextBox"
                    );

            mskPurchaseCode.Mask = "";
            mskPurchaseCode.Text = string.Empty;
            mskPurchaseCode.Mask = clsGVar.maskGLCode;

            sForm.lupassControl = new frmLookUp.LUPassControl(PassData1);
            sForm.ShowDialog();
            if (mskPurchaseCode.Text != null)
            {
                if (mskPurchaseCode.Text != null)
                {
                    if (mskPurchaseCode.Text.ToString() == "" || mskPurchaseCode.Text.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (mskPurchaseCode.Text.ToString().Trim().Length > 0)
                    {
                        PopulateRecordsGL1();
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

        private void PassData1(object sender)
        {
            mskPurchaseCode.Mask = "";
            mskPurchaseCode.Text = ((TextBox)sender).Text;
            mskPurchaseCode.Mask = clsGVar.maskGLCode;
            //mskCustomerCode.Text = ((MaskedTextBox)sender).Text;
            //mskCustomerCode.Mask = clsGVar.maskGLCode;

        }

        //Populate Recordset 
        private void PopulateRecordsGL1()
        {
            DataSet ds = new DataSet();
            DataRow dRow;
            string tSQL = string.Empty;

            // Fields 0,1,2,3 are Begin  

            tSQL = "SELECT Name ";
            tSQL += " from Customer ";
            tSQL += " where Code ='" + mskPurchaseCode.Text.ToString() + "';";

            try
            {
                ds = clsDbManager.GetData_Set(tSQL, "Customer");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //fAlreadyExists = true;
                    dRow = ds.Tables[0].Rows[0];
                    // Starting title as 0
                    lblNameBottom.Text = (ds.Tables[0].Rows[0]["Name"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["Name"].ToString());
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

        private void frmGdRecNoteReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            grd.Rows.Add(txt_I_ItemID.Text.ToString(),
               lbl_I_ItemCode.Text.ToString(),
               lbl_I_ItemName.Text.ToString(),
               txtDescription.Text.ToString(),
               cbo_I_Size.Text.ToString(),
               cbo_I_Color.Text.ToString(),
               cbo_I_UOM.Text.ToString(),
               cboGodown.Text.ToString(),
               txtStock.Text.ToString(),
               txtGRN.Text.ToString(),
               txtRTN.Text.ToString(),
               txtNewStock.Text.ToString(),
               txtRate.Text.ToString(),
               txtAmount.Text.ToString(),
                cbo_I_Size.SelectedValue.ToString(),
               cbo_I_Color.SelectedValue.ToString(),
               cbo_I_UOM.SelectedValue.ToString());
            cboGodown.SelectedValue.ToString();

           // SumVoc();
        }

        private void txt_I_ItemID_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmItemsHelpAuto frm = new frmItemsHelpAuto();
            frm.ShowDialog();

            txt_I_ItemID.Text = frm.Ret_ItemID.ToString();
            lbl_I_ItemCode.Text = frm.Ret_ItemCode.ToString();
            lbl_I_ItemName.Text = frm.Ret_ItemName.ToString();
        }

        private void txtGR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LookUp_Voc();
        }

            


    }
}


        
     
        