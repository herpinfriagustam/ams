using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace Clinic
{
    public partial class TransMedList : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listOut = new List<Status>();
        List<Status> listTrans = new List<Status>();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";

        public TransMedList()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            InitData();
            //LoadData();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitData()
        {
            listOut.Clear();
            listOut.Add(new Status() { statusCode = "", statusName = "All" });
            listOut.Add(new Status() { statusCode = "RTN", statusName = "Return" });
            listOut.Add(new Status() { statusCode = "EXP", statusName = "Expire" });
            listOut.Add(new Status() { statusCode = "MAN", statusName = "Manual" });
            listOut.Add(new Status() { statusCode = "ADJ", statusName = "Adjust" });

            luOut.Properties.DataSource = listOut;
            luOut.Properties.ValueMember = "statusCode";
            luOut.Properties.DisplayMember = "statusCode";

            luOut.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luOut.Properties.DropDownRows = listOut.Count;
            luOut.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luOut.Properties.AutoSearchColumnIndex = 1;
            luOut.Properties.NullText = "";
            luOut.ItemIndex = 0;

            listTrans.Clear();
            listTrans.Add(new Status() { statusCode = "", statusName = "All" });
            listTrans.Add(new Status() { statusCode = "IN", statusName = "In" });
            listTrans.Add(new Status() { statusCode = "OUT", statusName = "Out" });

            luType.Properties.DataSource = listTrans;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusCode";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listTrans.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0;

            dResDate.Text = today;
        }

        private void LoadData()
        {
            string sql_select = "";


            sql_select = "";
            sql_select = sql_select + Environment.NewLine + "select to_char(trans_date, 'yyyy-mm-dd') trans_date, trans_type,  ";
            sql_select = sql_select + Environment.NewLine + "med_name, trans_qty, batch_no, to_char(expire_date, 'yyyy-mm-dd') expire_date, ";
            sql_select = sql_select + Environment.NewLine + "trans_cd, trans_remark, c.patient_no, c.name, null dept ";
            sql_select = sql_select + Environment.NewLine + "from cs_trans_med_v a ";
            sql_select = sql_select + Environment.NewLine + "left join cs_patient b on (a.rm_no=b.rm_no) ";
            sql_select = sql_select + Environment.NewLine + "left join cs_patient_info c on (b.patient_no=c.patient_no) ";
            sql_select = sql_select + Environment.NewLine + "where to_char(trans_date, 'yyyy-mm-dd')='" + dResDate.Text + "' ";

            if (luType.Text != "")
            {
                sql_select = sql_select + Environment.NewLine + "and trans_type = '" + luType.Text + "' ";
            }
            if (luOut.Text != "")
            {
                sql_select = sql_select + Environment.NewLine + "and trans_cd = '" + luOut.Text + "' ";
            }
            
            sql_select = sql_select + Environment.NewLine + "order by 2,3 ";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_select, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();
                //gridView1.FixedLineWidth = 5;
                //gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Trans Type";
                gridView1.Columns[2].Caption = "Nama Obat";
                gridView1.Columns[3].Caption = "Jumlah";
                gridView1.Columns[4].Caption = "Batch No";
                gridView1.Columns[5].Caption = "Expire Date";
                gridView1.Columns[6].Caption = "Kode Out";
                gridView1.Columns[7].Caption = "Remark";
                gridView1.Columns[8].Caption = "Pasien No";
                gridView1.Columns[9].Caption = "Nama";
                gridView1.Columns[10].Caption = "Department";

                RepositoryItemLookUpEdit kodeLookup = new RepositoryItemLookUpEdit();
                kodeLookup.DataSource = listOut;
                kodeLookup.ValueMember = "statusCode";
                kodeLookup.DisplayMember = "statusName";

                kodeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kodeLookup.DropDownRows = listOut.Count;
                kodeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kodeLookup.AutoSearchColumnIndex = 1;
                kodeLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = kodeLookup;

                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[9].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[10].Visible = false;

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Trans Type")
            {
                string type = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
                if (type == "IN")
                {
                    //e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    //e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);

                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.Crimson;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //    if (stat == "Over")
            //    {
            //        e.Appearance.BackColor = Color.IndianRed;
            //        e.Appearance.BackColor2 = Color.Firebrick;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            //GridView View = sender as GridView;
            //string s_status = "", sql_chk = "";

            //s_status = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //if (s_status == "Over")
            //{
            //    btnObsCls.Enabled = true;
            //}
            //else
            //{
            //    btnObsCls.Enabled = false;
            //}
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "trans_med_list.xls",
                    RestoreDirectory = true,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    OverwritePrompt = true,
                    DereferenceLinks = true,
                    ValidateNames = true,
                    AddExtension = false,
                    FilterIndex = 1
                };
                saveDialog.InitialDirectory = "C:\\";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }
    }
}