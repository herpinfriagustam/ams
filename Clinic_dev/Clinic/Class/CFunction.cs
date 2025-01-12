using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using DevExpress.XtraEditors;
using System.Data;
using Microsoft.VisualBasic;
using DevExpress.XtraEditors.Repository;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;

namespace Clinic
{
    class CFunction
    {
        public class UserLogin
        {
            public String _mUserName;
            public String _mPassword;
            public String _mFullName;
            public String _mDeptnm;
            public String _mAdmin;
            public string reguserna;
        }

        public static DataTable DataSizeHungkul = null;

        string firstName;
        string secondName;
        string comments;

        DataTable datuk = new DataTable();
        ConnectDb Cn = new ConnectDb();
        //private readonly string _sDisplay;
        //private readonly string _sValue;

        public CFunction(string firstName, string secondName)
        {
            this.firstName = firstName;
            this.secondName = secondName;
        }
        public CFunction(string firstName, string secondName, string comments)
            : this(firstName, secondName)
        {
            this.comments = comments;
        }

        public CFunction()
        {
            // TODO: Complete member initialization
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string SecondName
        {
            get { return secondName; }
            set { secondName = value; }
        }
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public string GetValue()
        {
            return FirstName;
        }
        public string GetValue2()
        {
            return SecondName;
        }
        public class C_UOM
        {
            public string UomCode { get; set; }
            public string UomName { get; set; }
        }
        public override string ToString()
        {
            return firstName;
        }
        public static void Export_excel(DevExpress.XtraGrid.Views.Grid.GridView gridControl)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;
                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridControl.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridControl.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gridControl.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                }
            }
        }


        public static void SetFont(Control ctrl)
        {

            ctrl.Font = new Font("Arial", 10, FontStyle.Regular); ;

            foreach (Control ctrlChild in ctrl.Controls)
            {
                SetFont(ctrlChild);
            }
        }

        public static void Arange_Grid(DevExpress.XtraGrid.Views.Grid.GridView grid01)
        {
            grid01.IndicatorWidth = 40;
            grid01.OptionsSelection.UseIndicatorForSelection = false;
            grid01.OptionsSelection.MultiSelect = true;
            grid01.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grid01.OptionsView.ColumnAutoWidth = false;
            grid01.BestFitColumns();
            grid01.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
        }
        public static void SetDataSource(DevExpress.XtraEditors.ComboBoxEdit comb, DataTable dt, string value, string display, int selectedIndex = 0)
        {
            if (dt == null)
                return;

            comb.Properties.Items.Clear();
            comb.Properties.Items.Add(new CFunction(null, null));

            foreach (DataRow dr in dt.Rows)
            {
                comb.Properties.Items.Add(new CFunction(dr[display].ToString(), dr[value].ToString()));
            }
            comb.SelectedIndex = selectedIndex;
        }

        public static void SetEditLook(object sender, ProcessNewValueEventArgs e, LookUpEdit look, DataTable data, string kolom)
        {
            DataRow Row;
            RepositoryItemLookUpEdit Edit;
            Edit = ((LookUpEdit)sender).Properties;
            if (e.DisplayValue == null || Edit.NullText.Equals(e.DisplayValue) || string.Empty.Equals(e.DisplayValue))
                return;

            Row = data.NewRow();
            Row[kolom] = e.DisplayValue;
            data.Rows.Add(Row);
            e.Handled = true;
        }

        class Record
        {
            public string KDITEM { get; set; }
            public string ITEM { get; set; }
        }
        public static void SetDataLook(DevExpress.XtraEditors.LookUpEdit comb, DataTable dt, string value, string display, int selectedIndex = 0)
        {
            if (dt == null)
                return;

            comb.Properties.DataSource = null;

            List<Record> list = new List<Record>();
            //for (int i = 0; i < 1; i++)
            //{
            //    list.Add(new Record() { ITEM = "ALL" });
            //}
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Record() { KDITEM = dr[value].ToString(), ITEM = dr[display].ToString() });
            }
            var recs = from rec in list select rec;
            if (recs.Count() > 0)
                comb.Properties.DataSource = recs.ToList();
            comb.Properties.DisplayMember = "KODE";
            comb.Properties.ValueMember = "SATUAN";


            //comb.Properties.DataSource =null ;
            ////(comb.Properties.DataSource as List<Record>).Add(new Record() { KDITEM = null, ITEM = null  });

            //foreach (DataRow dr in dt.Rows)
            //{
            //    (comb.Properties.DataSource as List<Record>).Add(new Record() { KDITEM = dr[value].ToString(), ITEM = dr[display].ToString() });
            //}
            //comb.ItemIndex  = selectedIndex;
        }

        class Recordna
        {
            public string NAMA { get; set; }
        }
        public static void SetDataLookna(DevExpress.XtraEditors.LookUpEdit comb, DataTable dt, string value, string display, int selectedIndex = 0)
        {
            if (dt == null)
                return;

            comb.Properties.DataSource = null;

            List<Recordna> list = new List<Recordna>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Recordna() { NAMA = dr[display].ToString() });
            }
            var recs = from rec in list select rec;
            if (recs.Count() > 0)
                comb.Properties.DataSource = recs.ToList();
            comb.Properties.DisplayMember = "NAMA";
            comb.Properties.ValueMember = "NAMA";


        }
        public static object GetDataLook(DevExpress.XtraEditors.LookUpEdit Look, string sname, string value)
        {
            //LookUpEdit lookna;
            //return  
            return Look.Properties.GetDataSourceValue(Look.Properties.ValueMember, Look.Properties.GetDataSourceRowIndex(sname, value));
            //return lookna;
        }
       
        public static void SetDataRepo(DevExpress.XtraEditors.Repository.RepositoryItemComboBox comb, DataTable dt, string value, string display, int selectedIndex = 1)
        {
            if (dt == null)
                return;

            comb.Items.Clear();
            comb.Items.Add(new CFunction(null, null));

            foreach (DataRow dr in dt.Rows)
            {
                comb.Items.Add(new CFunction(dr[value].ToString(), dr[display].ToString()));
            }
            //comb.Items.IndexOf[selectedIndex];

        }
        public static void NoBanded(RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                int RowCount = e.RowHandle + 1;
                e.Info.DisplayText = RowCount.ToString();
                e.Info.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
        }
        public static void RepoEditBandCombo(ConvertEditValueEventArgs e)
        {
            if (e.Value != null)
            {
                if (Convert.ToString(e.Value) != null && !e.Value.ToString().Equals(""))
                {
                    e.Value = e.Value.ToString();
                    e.Handled = true;
                }
            }
            else
            {
                e.Value = Convert.ToString(e.Value);
                e.Handled = true;
            }
        }
        public static void repoAdd_Button(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView grid01, ButtonPressedEventArgs e, string nmkolom, string tempdata)
        {
            ButtonEdit ed = grid01.ActiveEditor as ButtonEdit;
            if (ed == null) return;
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)
            {
                int newIndex = grid01.FocusedRowHandle;
                int groupColumnCount = grid01.GroupedColumns.Count;
                grid01.AddNewRow();
                int nextIndex = grid01.FocusedRowHandle;
                grid01.SetRowCellValue(nextIndex, nmkolom, tempdata);
            }
        }
        public static void SetRepoStatus(DevExpress.XtraEditors.Repository.RepositoryItemComboBox comb)
        {
            comb.Items.Clear();
            comb.Items.Add(new CFunction("Aktif", "Aktif"));
            comb.Items.Add(new CFunction("Non Aktif", "Non Aktif"));
        }
        public static string GetValueItem(ComboBoxEdit comb)
        {
            if (comb.SelectedItem == null || comb.SelectedIndex == -1)
            {
                return null;
            }
            return ((CFunction)comb.SelectedItem).GetValue();
        }

        public static string GetKodeItem(ComboBoxEdit comb)
        {
            if (comb.SelectedItem == null || comb.SelectedIndex == -1)
            {
                return null;
            }
            return ((CFunction)comb.SelectedItem).GetValue2();
        }
        public string gGetCode(ComboBoxEdit gGCode, int v_flag = 0)
        {
            string functionReturnValue = null;
            string v_strText = gGCode.Text;

            int iStart = 0;
            int iEnd = 0;

            iStart = Strings.InStr(v_strText, "[");
            iEnd = Strings.InStr(v_strText, "]");

            if (v_flag == 0)
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iStart + 1, iEnd - iStart - 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            else
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iEnd + 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            return functionReturnValue;
        }

        public static string gGetCodeGlobal(string gGCode, int v_flag = 0)
        {
            string functionReturnValue = null;
            string v_strText = gGCode;

            int iStart = 0;
            int iEnd = 0;

            iStart = Strings.InStr(v_strText, "[");
            iEnd = Strings.InStr(v_strText, "]");

            if (v_flag == 0)
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iStart + 1, iEnd - iStart - 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = "0";
                }
            }
            else
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iEnd + 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            return functionReturnValue;
        }

        public static string gGetCodeDesc(string gGCode, int v_flag = 0)
        {
            string functionReturnValue = null;
            string v_strText = gGCode;

            int iStart = 0;
            int iEnd = 0;
            int imin = 0;

            iStart = Strings.InStr(v_strText, "]");
            iEnd = Strings.Len(v_strText);

            if (iStart == 0)
            {
                iStart = 1;
                imin = 0;
            }
            else
            {
                iStart = iStart;
                imin = 1;
            }


            if (v_flag == 0)
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iStart + imin, (iEnd - iStart) + 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            else
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iEnd + 1);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            return functionReturnValue;
        }
        public static string gGetCodeData(string gGCode, int v_flag = 0)
        {
            string functionReturnValue = null;
            string v_strText = gGCode;

            int iStart = 0;
            int iEnd = 0;

            iStart = 1;
            iEnd = Strings.InStr(v_strText, "[");

            if (v_flag == 0)
            {
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iStart, iEnd - iStart);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            else
            {
                iStart = Strings.InStr(v_strText, "-") + 1;
                iEnd = Strings.InStr(v_strText, "]");
                if (iStart != 0 & iEnd != 0 & iEnd > iStart)
                {
                    if (Strings.Len(v_strText) >= iEnd)
                    {
                        functionReturnValue = Strings.Mid(v_strText, iStart, iEnd - iStart);
                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                }
                else
                {
                    functionReturnValue = v_strText;
                }
            }
            return functionReturnValue;
        }


        public void GGetLine(ComboBoxEdit CboLine, int i)
        {
            //'*---------------------------------------------------------------------------------------------*
            //'*   Function  Name : GGetLine                                                                 *
            //'*   Access         : Public                                                                   *
            //'*   Parameter      : cboLine - Line combobox                                                  *
            //'*                  : intFlag  - flag                                                          *
            //'*   Return         : None                                                                     *
            //'*   Description    : set cboLine combobox when a form loads                                   *
            //'*   Create Date    : 2016.03.18            Creator : Herpin Friagustam                        *
            //'*   Update Date    :                       Updator :                                          *
            //'*---------------------------------------------------------------------------------------------*

            string SQL = "select '[ALL]' LINE from dual union SELECT    '[' || c_comcode || ']' ||n_comname LINE" +
                 "FROM      trtb_m_common " +
                 "WHERE     c_group = 'M04' " +
                 "And       t_buffer = '81' " +
                 "ORDER BY  1 ";

            //datuk = Cn.Data_Table_MES (SQL);

            CFunction.SetDataSource(CboLine, datuk, "LINE", "LINE", 0);

        }


        public static string GetDisplayItem(ComboBoxEdit comb)
        {
            if (comb.SelectedItem == null)
            {
                return null;
            }
            return ((CFunction)comb.SelectedItem).firstName;
        }

        public static void SelectedItem(ComboBoxEdit comb, string svalue)
        {
            foreach (object item in comb.Properties.Items)
            {
                if (((CFunction)item).GetValue() == svalue)
                {
                    comb.SelectedItem = item;
                }
            }
        }

        public static void CheckItem(LookUpEdit comb, string svalue)
        {
            foreach (object item in comb.Properties.ValueMember)
            {
                if (((CFunction)item).GetValue() == svalue)
                {
                    comb.EditValue = item;

                }
            }
        }

        public static void GetLook(LookUpEdit comb, string svalue, string nvalue)
        {
            foreach (object item in comb.Properties.ValueMember)
            {
                if (((CFunction)item).GetValue() == svalue)
                {
                    comb.EditValue = item;
                    return;
                }
            }
        }

        public static bool CheckSelectedNull(ComboBoxEdit comb)
        {
            return comb.SelectedItem == null;
        }

        public static string Get_Day()
        {
            var str = DateTime.Now.ToString().Trim();
            str = str.Substring(0, 2);
            return str;
        }
        public static string Get_Month()
        {
            var str = DateTime.Now.ToString().Trim();
            str = str.Substring(3, 2);
            return str;
        }
        public static string Get_Year()
        {
            var str = DateTime.Now.ToString().Trim();
            str = str.Substring(6, 4);
            return str;
        }

        public static string Get_YearMD()
        {
            var str = DateTime.Now.ToString().Trim();
            str = str.Substring(0, 4) + "-" + str.Substring(5, 2) + "-" + str.Substring(8, 2);
            return str;
        }
        public static DataSet ImportExcelXLS(string FileName, bool hasHeaders)
        {
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";

            DataSet output = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();

                DataTable schemaTable = conn.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheet = schemaRow["TABLE_NAME"].ToString();

                    if (!sheet.EndsWith("_"))
                    {
                        try
                        {
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                            cmd.CommandType = CommandType.Text;

                            DataTable outputTable = new DataTable(sheet);
                            output.Tables.Add(outputTable);
                            new OleDbDataAdapter(cmd).Fill(outputTable);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheet, FileName), ex);
                        }
                    }
                }
            }
            return output;
        }
        public static void Export(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedna)
        {
            if (bandedna.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel (.xlsx)|*.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bandedna.OptionsPrint.UsePrintStyles = false;
                    bandedna.ExportToXlsx(saveDialog.FileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx() { ExportType = DevExpress.Export.ExportType.WYSIWYG }); ;
                    DialogResult dialogResult = MessageBox.Show("Apakah anda ingin membuka file ini..?", "KONFIRMASI", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            else
            {
                MessageBox.Show("Maaf data yang anda Export tidak ada..?", "KONFIRMASI", MessageBoxButtons.OK);
            }
        }
        public static void GridSetting(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedna)
        {
            bandedna.IndicatorWidth = 35;
            bandedna.OptionsSelection.UseIndicatorForSelection = false;
            bandedna.OptionsSelection.MultiSelect = true;
            bandedna.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            for (int i = 0; i < bandedna.Columns.Count; i++)
            {
                bandedna.Columns[i].OptionsColumn.AllowEdit = true;
                bandedna.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
