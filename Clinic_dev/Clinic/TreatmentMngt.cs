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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Clinic.Report;
using DevExpress.XtraReports.UI;

namespace Clinic
{
    public partial class TreatmentMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Layanan> listTipe = new List<Layanan>();
        List<Stat> listGrpLaya = new List<Stat>();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Status> listStat2 = new List<Status>();
        List<Status> listStat3 = new List<Status>();
        List<Status> listStat4 = new List<Status>();
        DataSet dsBillRj = new DataSet();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string pub_head_id = "", pub_rm = "", pub_que = "", pub_date = "", pub_pasno = "", pub_tl = "", pub_adj_disc="";
        string pub_insu = "";
        //string today = "2019-11-27";

        public TreatmentMngt()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void TreatmentMngt_Load(object sender, EventArgs e)
        {
            InitData();
            LoadDataHead();
        }

        private void InitData()
        {
            dStartDt.Text = today;
            dEndDt.Text = today;

            string sql_tipe = " select treat_type_id, treat_type_name from cs_treatment_type  order by treat_type_id ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_tipe, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listTipe.Clear();

            listTipe.Add(new Layanan() { layananCode = "", layananName = "Pilih" });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listTipe.Add(new Layanan() { layananCode = dt.Rows[i]["treat_type_id"].ToString(), layananName = dt.Rows[i]["treat_type_name"].ToString() });

            }

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "A", statusName = "Tambah" });
            listStat2.Add(new Status() { statusCode = "D", statusName = "Hapus" });

            listStat3.Clear();
            listStat3.Add(new Status() { statusCode = "OPN", statusName = "Belum Bayar" });
            listStat3.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat3.Add(new Status() { statusCode = "ADJ", statusName = "Adjusment" });
            listStat3.Add(new Status() { statusCode = "CAN", statusName = "Batal" });

            listStat4.Clear();
            listStat4.Add(new Status() { statusCode = "", statusName = "All" });
            listStat4.Add(new Status() { statusCode = "B", statusName = "BPJS" });
            listStat4.Add(new Status() { statusCode = "U", statusName = "Umum" });
            listStat4.Add(new Status() { statusCode = "P", statusName = "Perusahaan" });

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            //SQL = SQL + Environment.NewLine + "and (treat_type_id <> 'TRT01' or treat_type_id is null) ";
            //SQL = SQL + Environment.NewLine + "and treat_group_id not in ('TRG02','TRG03','TRG05') ";

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            string sql_grplay = " select treat_group_id, initcap(treat_group_name) treat_group_name from cs_treatment_group  ";
            OleDbConnection oraConnectg = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrag = new OleDbDataAdapter(sql_grplay, oraConnectg);
            DataTable dtg = new DataTable();
            adOrag.Fill(dtg);
            listGrpLaya.Clear();
            for (int i = 0; i < dtg.Rows.Count; i++)
            {
                listGrpLaya.Add(new Stat() { statCode = dtg.Rows[i]["treat_group_id"].ToString(), statName = dtg.Rows[i]["treat_group_name"].ToString() });
            }
        }

        private void LoadDataHead()
        {
            string SQL, p_type = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select head_id, a.rm_no, a.patient_no, a.visit_no, to_char(a.visit_date,'yyyy-mm-dd') visit_date, ";
            SQL = SQL + Environment.NewLine + "b.name, b.insu_no bpjs, b.insu_no2, b.insu_nm2, ";
            SQL = SQL + Environment.NewLine + "insu_flag, treat_type_id,  pay_status, to_char(a.close_dt,'yyyy-mm-dd') close_dt, adj_flag status_adj, ";
            SQL = SQL + Environment.NewLine + "total_covered, total_bill, total_pay, remain_pay, disc, ";
            SQL = SQL + Environment.NewLine + "adj_covered, adj_bill, adj_pay, adj_remain_pay, nvl(adj_disc,0) adj_disc, 'S' action, pay_status, ";
            SQL = SQL + Environment.NewLine + "total_trt, total_med, adj_trt, adj_med, nvl(adj_disc,0) adj_disc_calc";
            SQL = SQL + Environment.NewLine + "from cs_treatment_head a ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and visit_date between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDt.Text + "','yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "order by 4, 6";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.FixedLineWidth = 8;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[6].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[7].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = true;
                gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "ID";
                gridView1.Columns[1].Caption = "No RM";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "Antrian";
                gridView1.Columns[4].Caption = "Tanggal";
                gridView1.Columns[5].Caption = "Nama";
                gridView1.Columns[6].Caption = "No BPJS";
                gridView1.Columns[7].Caption = "No Asuransi";
                gridView1.Columns[8].Caption = "Nama Asuransi";
                gridView1.Columns[9].Caption = "Asuransi";
                gridView1.Columns[10].Caption = "Tipe Layanan";
                gridView1.Columns[11].Caption = "Status Bayar";
                gridView1.Columns[12].Caption = "Tgl Bayar";
                gridView1.Columns[13].Caption = "Status Adj";
                gridView1.Columns[14].Caption = "Total Cover";
                gridView1.Columns[15].Caption = "Total Tagihan";
                gridView1.Columns[16].Caption = "Total Bayar";
                gridView1.Columns[17].Caption = "Sisa Bayar";
                gridView1.Columns[18].Caption = "Diskon";
                gridView1.Columns[19].Caption = "Adj Cover";
                gridView1.Columns[20].Caption = "Adj Tagihan";
                gridView1.Columns[21].Caption = "Adj Bayar";
                gridView1.Columns[22].Caption = "Adj Sisa Bayar";
                gridView1.Columns[23].Caption = "Adj Diskon";
                gridView1.Columns[24].Caption = "Action";
                gridView1.Columns[25].Caption = "Adj";
                gridView1.Columns[26].Caption = "Total Layanan";
                gridView1.Columns[27].Caption = "Total Obat";
                gridView1.Columns[28].Caption = "Adj Layanan";
                gridView1.Columns[29].Caption = "Adj Obat";
                gridView1.Columns[30].Caption = "Adj Disc Calc";

                RepositoryItemLookUpEdit statusLookup4 = new RepositoryItemLookUpEdit();
                statusLookup4.DataSource = listStat4;
                statusLookup4.ValueMember = "statusCode";
                statusLookup4.DisplayMember = "statusName";

                statusLookup4.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup4.DropDownRows = listStat4.Count;
                statusLookup4.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup4.AutoSearchColumnIndex = 1;
                statusLookup4.NullText = "";
                gridView1.Columns[9].ColumnEdit = statusLookup4;

                RepositoryItemLookUpEdit tLookup = new RepositoryItemLookUpEdit();
                tLookup.DataSource = listTipe;
                tLookup.ValueMember = "layananCode";
                tLookup.DisplayMember = "layananName";

                tLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                tLookup.DropDownRows = listTipe.Count;
                tLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                tLookup.AutoSearchColumnIndex = 1;
                tLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = tLookup;

                RepositoryItemLookUpEdit statusLookup2 = new RepositoryItemLookUpEdit();
                statusLookup2.DataSource = listStat3;
                statusLookup2.ValueMember = "statusCode";
                statusLookup2.DisplayMember = "statusName";

                statusLookup2.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup2.DropDownRows = listStat3.Count;
                statusLookup2.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup2.AutoSearchColumnIndex = 1;
                statusLookup2.NullText = "";
                gridView1.Columns[11].ColumnEdit = statusLookup2;

                //gridView1.Columns[3].Width = 80;

                gridView1.Columns[0].Visible = false;
                //gridView1.Columns[2].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[24].Visible = false;
                gridView1.Columns[25].Visible = false;
                gridView1.Columns[30].Visible = false;
                gridView1.Columns[26].VisibleIndex = 13;
                gridView1.Columns[27].VisibleIndex = 14;
                gridView1.Columns[28].VisibleIndex = 20;
                gridView1.Columns[29].VisibleIndex = 21;

                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = false;
                gridView1.Columns[4].OptionsColumn.AllowEdit = false;
                gridView1.Columns[5].OptionsColumn.AllowEdit = false;
                gridView1.Columns[6].OptionsColumn.AllowEdit = false;
                gridView1.Columns[7].OptionsColumn.AllowEdit = false;
                gridView1.Columns[8].OptionsColumn.AllowEdit = false;
                gridView1.Columns[9].OptionsColumn.AllowEdit = false;
                gridView1.Columns[10].OptionsColumn.AllowEdit = false;
                gridView1.Columns[12].OptionsColumn.AllowEdit = false;
                gridView1.Columns[13].OptionsColumn.AllowEdit = false;
                gridView1.Columns[14].OptionsColumn.AllowEdit = false;
                gridView1.Columns[15].OptionsColumn.AllowEdit = false;
                gridView1.Columns[16].OptionsColumn.AllowEdit = false;
                gridView1.Columns[17].OptionsColumn.AllowEdit = false;
                gridView1.Columns[18].OptionsColumn.AllowEdit = false;
                gridView1.Columns[19].OptionsColumn.AllowEdit = false;
                gridView1.Columns[20].OptionsColumn.AllowEdit = false;
                gridView1.Columns[21].OptionsColumn.AllowEdit = false;
                gridView1.Columns[22].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[23].OptionsColumn.AllowEdit = false;
                gridView1.Columns[26].OptionsColumn.AllowEdit = false;
                gridView1.Columns[27].OptionsColumn.AllowEdit = false;
                gridView1.Columns[28].OptionsColumn.AllowEdit = false;
                gridView1.Columns[29].OptionsColumn.AllowEdit = false;

                gridView1.BestFitColumns();

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadDataHead();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "layanan_mngt.xls",
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

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSave.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Status Bayar" || e.Column.Caption == "Adj Diskon")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[24]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[24], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[24], "U");
                }
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {

            GridView View = sender as GridView;

            if (e.Column.Caption == "Tipe Layanan")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                if (kk == "Rawat Jalan")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Rawat Inap")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "Status Bayar")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);
                if (kk == "Belum Bayar")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Batal")
                {
                    
                }
                else if (kk == "Adjusment")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.OldLace;
                    e.Appearance.ForeColor = Color.Black;
                }
                
            }

            if (e.Column.Caption == "Adj Diskon")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_head = "", s_rm="", s_que="", s_date="", s_adj="", s_pasno = "", s_tl = "", s_adj_disc="", s_insu="";

            s_head = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_pasno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
            s_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            s_insu = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            s_adj = View.GetRowCellDisplayText(e.RowHandle, View.Columns[13]);
            s_tl = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_adj_disc = View.GetRowCellDisplayText(e.RowHandle, View.Columns[30]);

            pub_head_id = s_head;
            pub_rm = s_rm;
            pub_que = s_que;
            pub_date = s_date;
            pub_pasno = s_pasno;
            pub_tl = s_tl;
            pub_adj_disc = s_adj_disc;
            pub_insu = s_insu;

            if (s_adj == "Y")
            {
                btnCalc.Enabled = true;
                btnCanc.Enabled = true;
                btnAddAdj.Enabled = true;
                btnSaveAdj.Enabled = true;
                btnDelAdj.Enabled = true;
            }
            else
            {
                btnCalc.Enabled = false;
                btnCanc.Enabled = false;
                btnAddAdj.Enabled = false;
                btnSaveAdj.Enabled = false;
                btnDelAdj.Enabled = false;
            }

            LoadDataDetail();
            LoadDataAdj();
        }

        private void LoadDataDetail()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + " select a.treat_item_id, a.treat_group_id, a.treat_item_name, ";
            SQL = SQL + Environment.NewLine + " to_char(treat_date,'yyyy-mm-dd') treat_date, treat_qty, ";
            SQL = SQL + Environment.NewLine + " b.total_price, remarks ";
            SQL = SQL + Environment.NewLine + " from cs_treatment_item a ";
            SQL = SQL + Environment.NewLine + " join cs_treatment_detail b on (a.treat_item_id=b.treat_item_id) ";
            SQL = SQL + Environment.NewLine + " where head_id='" + pub_head_id + "' ";
            SQL = SQL + Environment.NewLine + " union all";
            SQL = SQL + Environment.NewLine + " select 0 treat_item_id, 'TRG05' treat_group_id, initcap(med_name) med_name, ";
            SQL = SQL + Environment.NewLine + " to_char(a.insp_date,'yyyy-mm-dd') insp_date,  med_qty,  ";
            if (pub_insu == "Umum")
            {
                SQL = SQL + Environment.NewLine + " price, ";
            }
            else
            {
                SQL = SQL + Environment.NewLine + " price * insu_cover price, ";
            }
            SQL = SQL + Environment.NewLine + " confirm  remarks ";
            SQL = SQL + Environment.NewLine + " from cs_receipt a  ";
            SQL = SQL + Environment.NewLine + " join cs_patient b on (a.rm_no = b.rm_no)  ";
            SQL = SQL + Environment.NewLine + " join cs_medicine c on(a.med_cd = c.med_cd)  ";
            SQL = SQL + Environment.NewLine + " join cs_medicine_trans d on(a.receipt_id = d.receipt_id)  ";
            if (pub_tl == "Rawat Jalan")
            {
                
            }
            else
            {
                SQL = SQL + Environment.NewLine + " join cs_inpatient e on (a.rm_no=e.rm_no and a.visit_dt=e.reg_date)   ";
            }
            SQL = SQL + Environment.NewLine + " where b.status = 'A'  ";
            SQL = SQL + Environment.NewLine + " and c.status = 'A'  ";
            SQL = SQL + Environment.NewLine + " and b.patient_no = '" + pub_pasno + "'  ";
            if (pub_tl == "Rawat Jalan")
            {
                SQL = SQL + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + pub_date + "'  ";
            }

            SQL = SQL + Environment.NewLine + " and visit_no = '" + pub_que + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView2.IndicatorWidth = 40;
            gridView2.OptionsBehavior.Editable = false;
            gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Item ID";
            gridView2.Columns[1].Caption = "Group";
            gridView2.Columns[2].Caption = "Layanan";
            gridView2.Columns[3].Caption = "Tanggal";
            gridView2.Columns[4].Caption = "Jumlah";
            gridView2.Columns[5].Caption = "Harga";
            gridView2.Columns[6].Caption = "Remarks";

            gridView2.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView2.Columns[3].VisibleIndex = 2;

            gridView2.Columns[0].Visible = false;
            gridView2.Columns[6].Visible = false;

            RepositoryItemLookUpEdit grpLookup = new RepositoryItemLookUpEdit();
            grpLookup.DataSource = listGrpLaya;
            grpLookup.ValueMember = "statCode";
            grpLookup.DisplayMember = "statName";

            grpLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            grpLookup.DropDownRows = listGrpLaya.Count;
            grpLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            grpLookup.AutoSearchColumnIndex = 1;
            grpLookup.NullText = "";
            gridView2.Columns[1].ColumnEdit = grpLookup;

            //RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            //glLaya.DataSource = listLaya2;
            //glLaya.ValueMember = "layananCode";
            //glLaya.DisplayMember = "layananName";

            //glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            //glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //glLaya.ImmediatePopup = true;
            //glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            //glLaya.NullText = "";
            //gridView2.Columns[2].ColumnEdit = glLaya;

            gridView2.BestFitColumns();
        }

        private void LoadDataAdj()
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select b.detail_adj_id, b.adj_type, b.treat_item_id, b.treat_qty, b.treat_item_price, ";
            SQL = SQL + Environment.NewLine + "b.remarks, 'S' action, a.head_id, to_char(b.treat_date,'yyyy-mm-dd') treat_date, a.pay_status, detail_id ";
            SQL = SQL + Environment.NewLine + "from cs_treatment_head a ";
            SQL = SQL + Environment.NewLine + "join cs_treatment_detail_adj b on (a.head_id=b.head_id) ";
            SQL = SQL + Environment.NewLine + "join cs_treatment_item c on (b.treat_item_id=c.treat_item_id) ";
            SQL = SQL + Environment.NewLine + "where a.head_id='" + pub_head_id + "'  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView3.IndicatorWidth = 40;
            gridView3.OptionsBehavior.Editable = true;
            gridView3.BestFitColumns();

            gridView3.Columns[0].Caption = "ID";
            gridView3.Columns[1].Caption = "Tipe Adj";
            gridView3.Columns[2].Caption = "Nama Tindakan";
            gridView3.Columns[3].Caption = "Jumlah";
            gridView3.Columns[4].Caption = "Harga";
            gridView3.Columns[5].Caption = "Remark";
            gridView3.Columns[6].Caption = "Action";
            gridView3.Columns[7].Caption = "Head ID";
            gridView3.Columns[8].Caption = "Tanggal";
            gridView3.Columns[9].Caption = "Status Bayar";
            gridView3.Columns[10].Caption = "Det ID";

            gridView3.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            gridView3.Columns[8].VisibleIndex = 2;

            gridView3.Columns[0].Visible = false;
            gridView3.Columns[5].Visible = false;
            gridView3.Columns[6].Visible = false;
            gridView3.Columns[7].Visible = false;
            //gridView3.Columns[8].Visible = false;
            gridView3.Columns[9].Visible = false;
            gridView3.Columns[10].Visible = false;

            //gridView3.Columns[1].OptionsColumn.ReadOnly = true;
            gridView3.Columns[3].OptionsColumn.ReadOnly = true;
            gridView3.Columns[4].OptionsColumn.ReadOnly = true;
            gridView3.Columns[6].OptionsColumn.ReadOnly = true;

            RepositoryItemLookUpEdit statusLookup2 = new RepositoryItemLookUpEdit();
            statusLookup2.DataSource = listStat2;
            statusLookup2.ValueMember = "statusCode";
            statusLookup2.DisplayMember = "statusName";

            statusLookup2.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            statusLookup2.DropDownRows = listStat2.Count;
            statusLookup2.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            statusLookup2.AutoSearchColumnIndex = 1;
            statusLookup2.NullText = "";
            gridView3.Columns[1].ColumnEdit = statusLookup2;

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya2;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView3.Columns[2].ColumnEdit = glLaya;

            gridView3.BestFitColumns();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql_update = "", s_head = "", s_status = "", action = "", s_adj = "", s_adj_disc="", s_flag_adj="";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                s_head = pub_head_id;
                s_status = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[24]).ToString();
                s_adj = gridView1.GetRowCellValue(i, gridView1.Columns[25]).ToString();
                s_adj_disc = gridView1.GetRowCellValue(i, gridView1.Columns[23]).ToString();
                s_flag_adj = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();

                if (action == "U" && s_adj == "CAN")
                {
                    MessageBox.Show("Data tidak dapat dirubah");
                }
                else if (action == "U" && s_adj == "OPN")
                {
                    MessageBox.Show("Silahkan lakukan pembayaran");
                }
                else
                {
                    if (action == "I")
                    {

                    }
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_treatment_head " +
                                                  " set pay_status = '" + s_status + "', status = '" + s_status + "', adj_disc='" + s_adj_disc + "', ";
                        if (s_status == "ADJ" && s_flag_adj == "")
                        {
                            sql_update = sql_update + " adj_flag='Y', ";
                        }
                        else if (s_status == "CLS" && s_flag_adj == "Y")
                        {

                        }
                        else
                        {
                            sql_update = sql_update + " adj_flag=null, ";
                        }
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where head_id = '" + s_head + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            LoadDataHead();
        }

        private void btnAddAdj_Click(object sender, EventArgs e)
        {
            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView3.AddNewRow();
        }

        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
            //gridView6.Columns[3].OptionsColumn.ReadOnly = false;
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[8], today);
            //view.SetRowCellValue(e.RowHandle, view.Columns[6], "TRT02");
            //btnAddTindakan.Enabled = false;
        }

        private void btnSaveAdj_Click(object sender, EventArgs e)
        {
            string date = "", rm_no = "", que = "", nama_laya = "", head = "", detail = "", ldate = "", qty = "", price = "", remarks = "", action = "", stbyr = "";
            string sql_cnt = "", diag_cnt = "", sql_update = "", tipe_adj ="", det_id="";

            date = pub_date;
            que = pub_que;
            rm_no = pub_rm;

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                tipe_adj = gridView3.GetRowCellValue(i, gridView3.Columns[1]).ToString();
                detail = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                head = gridView3.GetRowCellValue(i, gridView3.Columns[7]).ToString();
                nama_laya = gridView3.GetRowCellValue(i, gridView3.Columns[2]).ToString();
                ldate = gridView3.GetRowCellValue(i, gridView3.Columns[8]).ToString();
                qty = gridView3.GetRowCellValue(i, gridView3.Columns[3]).ToString();
                price = gridView3.GetRowCellValue(i, gridView3.Columns[4]).ToString();
                remarks = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();
                action = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                stbyr = gridView3.GetRowCellValue(i, gridView3.Columns[9]).ToString();
                det_id = gridView3.GetRowCellValue(i, gridView3.Columns[10]).ToString();

                if (nama_laya == "")
                {
                    MessageBox.Show("Nama Layanan harus diisi");
                }
                else if (tipe_adj=="")
                {
                    MessageBox.Show("Tipe harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from cs_treatment_detail_adj where head_id = '" + head + "' and to_char(treat_date,'yyyy-mm-dd') = '" + ldate + "' and treat_item_id = '" + nama_laya + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        diag_cnt = dt.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(diag_cnt) > 0)
                        {
                            //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        }
                        else
                        {
                            string sql_seq = "", seq_val = "", sql_tmp = "";
                            sql_seq = " select CS_TREATMENT_DETAIL_ADJ_SEQ.nextval seq from dual ";
                            OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                            DataTable dts = new DataTable();
                            adOras.Fill(dts);
                            seq_val = dts.Rows[0]["seq"].ToString();

                            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                            OleDbCommand command = new OleDbCommand();
                            OleDbTransaction trans = null;

                            command.Connection = oraConnectTrans;
                            oraConnectTrans.Open();

                            try
                            {
                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into cs_treatment_detail_adj (detail_adj_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, remarks, adj_type, detail_id, ins_date, ins_emp) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'yyyy-mm-dd'), " + qty + ", " + price + ", '" + remarks + "', '" + tipe_adj + "', '" + det_id + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                if (tipe_adj == "A")
                                {
                                    command.CommandText = " insert into cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, detail_adj_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + rm_no + "', to_date('" + ldate + "', 'yyyy-mm-dd'), to_date('" + date + "', 'yyyy-mm-dd'), '" + que + "', '" + det_id + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    command.CommandText = " update cs_action set detail_adj_id = '" + seq_val + "', upd_emp = '" + DB.vUserId + "', upd_date = sysdate where detail_id = '" + det_id + "' ";
                                    command.ExecuteNonQuery();
                                }
                                
                                trans.Commit();
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql_insert);
                                MessageBox.Show("Data Berhasil disimpan.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                MessageBox.Show("ERROR: " + ex.Message);
                            }

                            oraConnectTrans.Close();
                        }
                    }
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_treatment_detail_adj " +
                                                  " set remarks = '" + remarks + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where detail_adj_id = '" + detail + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }


                }
            }
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "";

            date = pub_date;
            que = pub_que;
            rm_no = pub_rm;

            a = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
            tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();

            if (e.Column.Caption == "Nama Tindakan" && a != "")
            {
                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "", sql_det = "", det_id ="" ;
                sql_ = " select treat_group_id, treat_item_price from cs_treatment_item where treat_item_id = " + a + " ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["treat_group_id"].ToString();
                    price = dt0.Rows[0]["treat_item_price"].ToString();
                }

                sql_head = " select head_id, pay_status from cs_treatment_head where rm_no = '" + rm_no + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' ";

                OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                DataTable dt1 = new DataTable();
                adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["head_id"].ToString();
                    stbyr = dt1.Rows[0]["pay_status"].ToString();
                }

                sql_det = " select detail_id from cs_treatment_detail where head_id = '" + head_id + "' and treat_item_id = '" + a + "' ";

                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_det, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    det_id = dt2.Rows[0]["detail_id"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], head_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], stbyr);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], det_id);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }

            }

            if (e.Column.Caption == "Remark")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }
            }
        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Tindakan" || e.Column.Caption == "Remark")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelAdj_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "", payst = "", det_id="", tipe_adj="";

                if (gridView3.RowCount <= 0)
                {
                    return;
                }

                id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[0]).ToString();
                tipe_adj = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[1]).ToString();
                payst = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[9]).ToString();
                det_id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[10]).ToString();

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    if (payst == "ADJ")
                    {
                        trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Connection = oraConnectTrans;
                        command.Transaction = trans;


                        command.CommandText = " delete cs_treatment_detail_adj where detail_adj_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        if (tipe_adj == "A")
                        {
                            command.CommandText = " delete cs_action where detail_adj_id = '" + id + "' ";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText = " update cs_action set detail_adj_id = null where detail_id = '" + det_id + "'  ";
                            command.ExecuteNonQuery();
                        }
                    

                        trans.Commit();
                        //MessageBox.Show(sql_insert);
                        //MessageBox.Show("Query Exec : " + sql_insert);
                        gridView3.DeleteRow(gridView3.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil didelete.");
                    }
                    else
                    {
                        //
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                oraConnectTrans.Close();
                LoadDataDetail();
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            string sql_cek_amt_laya = "", tmp_amt_laya = "";
            string sql_cek_amt_med = "", tmp_amt_med = "";
            int adj_bill = 0, adj_disc = 0, adj_pay=0, tmp=0;

            adj_disc = Convert.ToInt32(pub_adj_disc);

            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " select sum (amt) amt_laya from ( ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " select sum(total_price) amt ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " from  cs_treatment_detail ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " where head_id='"+pub_head_id+"' ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " union ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " select sum(treat_item_price) amt ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " from  cs_treatment_detail_adj_v ";
            sql_cek_amt_laya = sql_cek_amt_laya + Environment.NewLine + " where head_id='" + pub_head_id + "') ";

            OleDbConnection sqlConnectlaya = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqllaya = new OleDbDataAdapter(sql_cek_amt_laya, sqlConnectlaya);
            DataTable dtlaya = new DataTable();
            adSqllaya.Fill(dtlaya);
            if (dtlaya.Rows.Count > 0)
            {
                tmp_amt_laya = dtlaya.Rows[0]["amt_laya"].ToString();
            }
            else
            {
                tmp_amt_laya = "0";
            }

            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " select nvl(sum(price),0) amt_med ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " from cs_receipt a  ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join cs_patient b on (a.rm_no = b.rm_no)  ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join cs_medicine c on(a.med_cd = c.med_cd)  ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join cs_medicine_trans d on(a.receipt_id = d.receipt_id)  ";
            if (pub_tl == "Rawat Jalan")
            {

            }
            else
            {
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " join cs_inpatient e on (a.rm_no=e.rm_no and a.visit_dt=e.reg_date)   ";
            }
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " where b.status = 'A'  ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and c.status = 'A'  ";
            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and b.patient_no = '" + pub_pasno + "'  ";

            if (pub_tl == "Rawat Jalan")
            {
                sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + pub_date + "'  ";
            }

            sql_cek_amt_med = sql_cek_amt_med + Environment.NewLine + " and visit_no = '" + pub_que + "' ";

            OleDbConnection sqlConnectmed = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlmed = new OleDbDataAdapter(sql_cek_amt_med, sqlConnectmed);
            DataTable dtmed = new DataTable();
            adSqlmed.Fill(dtmed);
            if (dtmed.Rows.Count > 0)
            {
                tmp_amt_med = dtmed.Rows[0]["amt_med"].ToString();
            }
            else
            {
                tmp_amt_med = "0";
            }

            adj_bill = Convert.ToInt32(tmp_amt_laya) + Convert.ToInt32(tmp_amt_med);
            tmp = Convert.ToInt32(adj_disc) * adj_bill / 100;
            adj_pay = adj_bill - tmp;

            string sql_update = "";

            sql_update = "";

            sql_update = sql_update + " update cs_treatment_head " +
                                      " set adj_trt = " + tmp_amt_laya + ", adj_med = " + tmp_amt_med + ", adj_bill = " + adj_bill.ToString() + ", adj_pay = " + adj_pay.ToString() + ", ";
            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
            sql_update = sql_update + " where head_id = '" + pub_head_id + "' and pay_status = 'ADJ' ";

            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                oraConnect.Open();
                cm.ExecuteNonQuery();
                oraConnect.Close();
                cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);

                MessageBox.Show("Data Berhasil diupdate");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            LoadDataHead();
        }

        private void btnCanc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan membatalkan adjusment?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                if (gridView3.RowCount > 0)
                {
                    string sql_update = "", head = "", tipe_adj = "", detail = "", det_id = "";

                    for (int i = 0; i < gridView3.DataRowCount; i++)
                    {
                        head = pub_head_id;
                        tipe_adj = gridView3.GetRowCellValue(i, gridView3.Columns[1]).ToString();
                        detail = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                        det_id = gridView3.GetRowCellValue(i, gridView3.Columns[10]).ToString();

                        sql_update = "";
                        if (tipe_adj == "D")
                        {
                            // update cs_action
                            sql_update = sql_update + " update cs_action " +
                                                      " set detail_adj_id = null, ";
                            sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                            sql_update = sql_update + " where detail_id = '" + det_id + "' ";
                        }
                        else
                        {
                            // delete cs_action
                            sql_update = sql_update + " delete from cs_action ";
                            sql_update = sql_update + " where detail_adj_id = '" + detail + "' ";
                        }


                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }

                    }

                }

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;

                    command.CommandText = " delete from cs_treatment_detail_adj where head_id = '"+ pub_head_id + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = " update cs_treatment_head set status = 'CLS', pay_status = 'CLS', adj_flag = null, adj_trt = null, adj_med = null, adj_bill = null, adj_pay = null, adj_disc = null, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where head_id = '" + pub_head_id + "' ";
                    command.ExecuteNonQuery();

                    trans.Commit();
                    //MessageBox.Show(sql_insert);
                    //MessageBox.Show("Query Exec : " + sql_insert);
                    MessageBox.Show("Data Adjusment berhasil dibatalkan.");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                oraConnectTrans.Close();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string SQL = "", SQL2 = "", limit = "", s_head = "", s_pasno = "", s_rmno = "", s_date = "", s_que;
            string p_name = "", p_age = "", p_phone = "", p_address = "", p_rm = "", p_date = "", p_tipe = "", tot = "";
            string s_adj_type = "", s_tl="", s_st_bayar="";

            //tot = String.Format(CultureInfo.GetCultureInfo("id-ID"), "{0:C2}", totPay);

            s_st_bayar = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[25]).ToString();
            if (s_st_bayar == "ADJ")
            {
                MessageBox.Show("Silahkan selesaikan status pembayaran.");
                return;
            }

            s_head = pub_head_id;
            s_pasno = pub_pasno;
            s_rmno = pub_rm;
            s_date = pub_date;
            s_que = pub_que;
            s_adj_type = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            s_tl = pub_tl;

            if (s_adj_type == "Y")
            {
                tot = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[21]).ToString();
            }
            else
            {
                tot = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[16]).ToString();
            }

            SQL = "";
            SQL = SQL + Environment.NewLine + " select name, round((sysdate-birth_date)/30/12) age, phone,  ";
            SQL = SQL + Environment.NewLine + " address, b.rm_no, TO_CHAR(c.visit_date, 'fmdd Month yyyy', 'nls_date_language = INDONESIAN') tgl,   ";
            SQL = SQL + Environment.NewLine + " decode (insu_flag,'B','BPJS','P','Perusahaan','Umum') insu_flag  ";
            SQL = SQL + Environment.NewLine + " from cs_patient_info a  ";
            SQL = SQL + Environment.NewLine + " join cs_patient b on (a.patient_no=b.patient_no)  ";
            SQL = SQL + Environment.NewLine + " join cs_visit c on (a.patient_no=c.patient_no)  ";
            SQL = SQL + Environment.NewLine + " join cs_treatment_head d on (b.rm_no=d.rm_no and trunc(c.visit_date)=d.visit_date and c.que01=d.visit_no)  ";
            SQL = SQL + Environment.NewLine + " where 1=1  ";
            //SQL = SQL + Environment.NewLine + " and b.group_patient='COMM'  ";
            SQL = SQL + Environment.NewLine + " and to_char(c.visit_date,'yyyy-mm-dd')='" + s_date + "'  ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                p_name = dt.Rows[0]["name"].ToString();
                p_age = dt.Rows[0]["age"].ToString();
                p_phone = dt.Rows[0]["phone"].ToString();
                p_address = dt.Rows[0]["address"].ToString();
                p_rm = dt.Rows[0]["rm_no"].ToString();
                p_date = dt.Rows[0]["tgl"].ToString();
                p_tipe = dt.Rows[0]["insu_flag"].ToString();
            }

            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + " select name, age, phone, address, rm, tgl, tipe,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_group_order, treat_group_name, a, b, c, ord from (   ";
            SQL2 = SQL2 + Environment.NewLine + " select '" + p_name + "' name, '" + p_age + "' age, '" + p_phone + "' phone, '" + p_address + "' address, '" + p_rm + "' rm, '" + p_date + "' tgl, '" + p_tipe + "' tipe,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_group_order, treat_group_name, a, b, a*b c, ord from (   ";
            SQL2 = SQL2 + Environment.NewLine + " select treat_group_order, treat_group_name, 0 a, 0 b, treat_group_order * 10 ord  from cs_treatment_group  ";
            SQL2 = SQL2 + Environment.NewLine + " where treat_group_id in (select c.treat_group_id  ";
            SQL2 = SQL2 + Environment.NewLine + " from cs_treatment_item a   ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_treatment_detail b on (a.treat_item_id=b.treat_item_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_treatment_group c on (a.treat_group_id=c.treat_group_id)  ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "'   ";
            SQL2 = SQL2 + Environment.NewLine + " and b.treat_item_id not in (";
            SQL2 = SQL2 + Environment.NewLine + " select treat_item_id from cs_treatment_detail_adj ";
            SQL2 = SQL2 + Environment.NewLine + " where adj_type='D') ";
            SQL2 = SQL2 + Environment.NewLine + " ) ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            SQL2 = SQL2 + Environment.NewLine + " select i, treat_item_name, sum(treat_qty) treat_qty,  ";
            SQL2 = SQL2 + Environment.NewLine + " treat_item_price, ord  from ( ";
            SQL2 = SQL2 + Environment.NewLine + " select null i, a.treat_item_name, treat_qty,   ";
            SQL2 = SQL2 + Environment.NewLine + " a.treat_item_price, (c.treat_group_order * 10) + 1 ord  ";
            SQL2 = SQL2 + Environment.NewLine + " from cs_treatment_item a   ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_treatment_detail b on (a.treat_item_id=b.treat_item_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_treatment_group c on (a.treat_group_id=c.treat_group_id)  ";
            SQL2 = SQL2 + Environment.NewLine + " where head_id='" + s_head + "'   ";
            SQL2 = SQL2 + Environment.NewLine + " and b.treat_item_id not in (";
            SQL2 = SQL2 + Environment.NewLine + " select treat_item_id from cs_treatment_detail_adj ";
            SQL2 = SQL2 + Environment.NewLine + " where adj_type='D') ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            SQL2 = SQL2 + Environment.NewLine + " select 5 i, 'Obat-obatan dan Alkes' a, 0 b, 0 c, 50 d from dual  ";
            SQL2 = SQL2 + Environment.NewLine + " union all ";
            SQL2 = SQL2 + Environment.NewLine + " select null i, initcap(med_name) med_name, 1 med_qty, price,   ";
            SQL2 = SQL2 + Environment.NewLine + " 50 + 1  remarks   ";
            SQL2 = SQL2 + Environment.NewLine + " from cs_receipt a    ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_patient b on (a.rm_no = b.rm_no)   ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_medicine c on(a.med_cd = c.med_cd)    ";
            SQL2 = SQL2 + Environment.NewLine + " join cs_medicine_trans d on(a.receipt_id = d.receipt_id)   ";
            SQL2 = SQL2 + Environment.NewLine + " where b.status = 'A'    ";
            SQL2 = SQL2 + Environment.NewLine + " and c.status = 'A'    ";
            SQL2 = SQL2 + Environment.NewLine + " and b.patient_no = '" + s_pasno + "'    ";
            if (s_tl == "Rawat Jalan")
            {
                SQL2 = SQL2 + Environment.NewLine + " and to_char(insp_date, 'yyyy-mm-dd') = '" + s_date + "'    ";
            }
            else
            {
                SQL2 = SQL2 + Environment.NewLine + " and to_char(visit_dt, 'yyyy-mm-dd') = '" + s_date + "'    ";
            }

            SQL2 = SQL2 + Environment.NewLine + " and visit_no = '" + s_que + "' )  ";
            SQL2 = SQL2 + Environment.NewLine + " group by i, treat_item_name, treat_item_price, ord) ";
            SQL2 = SQL2 + Environment.NewLine + " where 1=1  ";
            SQL2 = SQL2 + Environment.NewLine + " union ";
            SQL2 = SQL2 + Environment.NewLine + " select null a1, null a2, null a3, null a4, null a5, '" + p_date + "' a6, null a7, ";
            SQL2 = SQL2 + Environment.NewLine + " null aa, 'Jumlah biaya yang harus dibayar' bb, ";
            SQL2 = SQL2 + Environment.NewLine + " null a, null b, " + tot + " c, 999 ord from dual ";
            SQL2 = SQL2 + Environment.NewLine + " ) ";
            SQL2 = SQL2 + Environment.NewLine + " where 1=1  ";
            SQL2 = SQL2 + Environment.NewLine + " order by ord asc  ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL2, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            dsBillRj.Tables.Clear();
            dsBillRj.Tables.Add(dt2);

            ReportBill report = new ReportBill(dsBillRj);
            report.ShowPreviewDialog();
        }
    }
}