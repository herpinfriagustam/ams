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
using DevExpress.XtraEditors.Repository;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic
{
    public partial class InpatientMngt : DevExpress.XtraEditors.XtraForm
    {

        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Medicine> listMedicine = new List<Medicine>();
        DataTable dtGlMed = new DataTable();

        List<PatientType> listPatientType = new List<PatientType>();
        List<Status> listStat = new List<Status>();
        List<Stat> statIn = new List<Stat>();
        List<Stat> statFrom = new List<Stat>();
        List<Stat> statOut = new List<Stat>();
        List<Stat> statPasien = new List<Stat>();

        public string v_empid = "", v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public InpatientMngt()
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

        private void InpatientMngt_Load(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void initData()
        {
            dStartDt.Text = today;
            dEndDt.Text = today;

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "B", patientTypeName = "BPJS" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });
            listPatientType.Add(new PatientType() { patientTypeCode = "P", patientTypeName = "Perusahaan" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "REG", statusName = "Registrasi" });
            listStat.Add(new Status() { statusCode = "OPN", statusName = "Proses" });
            listStat.Add(new Status() { statusCode = "CLS", statusName = "Selesai" });
            listStat.Add(new Status() { statusCode = "CAN", statusName = "Batal" });

            statIn.Clear();
            statIn.Add(new Stat() { statCode = "DSN", statName = "Datang Sendiri" });
            statIn.Add(new Stat() { statCode = "POL", statName = "Poli Klinik" });
            statIn.Add(new Stat() { statCode = "UGD", statName = "UGD" });

            statFrom.Clear();
            statFrom.Add(new Stat() { statCode = "BDN", statName = "Bidan Desa" });
            statFrom.Add(new Stat() { statCode = "DKT", statName = "Dokter Praktek" });
            statFrom.Add(new Stat() { statCode = "PLS", statName = "Kasus Polisi" });

            statOut.Clear();
            statOut.Add(new Stat() { statCode = "STJ", statName = "Persetujuan" });
            statOut.Add(new Stat() { statCode = "PLG", statName = "Pulang Paksa" });
            statOut.Add(new Stat() { statCode = "OUT", statName = "Melarikan Diri" });
            statOut.Add(new Stat() { statCode = "RJK", statName = "Dirujuk" });

            statPasien.Clear();
            statPasien.Add(new Stat() { statCode = "SMB", statName = "Sembuh" });
            statPasien.Add(new Stat() { statCode = "PRB", statName = "Perbaikan" });
            statPasien.Add(new Stat() { statCode = "SKT", statName = "Tidak Sembuh" });
            statPasien.Add(new Stat() { statCode = "M01", statName = "Meninggal < 24 Jam" });
            statPasien.Add(new Stat() { statCode = "M02", statName = "Meninggal > 24 Jam" });
        }

        private void btnLoadRanap_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'U' action, inpatient_id, a.rm_no, to_char(reg_date, 'yyyy-mm-dd') reg_date , ";
            sql_search = sql_search + Environment.NewLine + "c.name, d.name penjamin, room_id,";
            sql_search = sql_search + Environment.NewLine + "to_char(date_in, 'yyyy-mm-dd hh24:mi:ss') date_in, ";
            sql_search = sql_search + Environment.NewLine + "to_char(date_out, 'yyyy-mm-dd hh24:mi:ss') date_out, ";
            sql_search = sql_search + Environment.NewLine + "a.status, rs_in, came_from, came_remark, rs_out, patient_stat ";
            sql_search = sql_search + Environment.NewLine + "from KLINIK.cs_inpatient a ";
            sql_search = sql_search + Environment.NewLine + "join KLINIK.cs_patient b on (a.rm_no=b.rm_no) ";
            sql_search = sql_search + Environment.NewLine + "join KLINIK.cs_patient_info c on (b.patient_no=c.patient_no) ";
            sql_search = sql_search + Environment.NewLine + "join KLINIK.cs_guarantor d on (a.gr_no=d.gr_no) ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            sql_search = sql_search + Environment.NewLine + "and reg_date between to_date('" + dStartDt.Text + "','yyyy-mm-dd') and to_date('" + dEndDt.Text + "','yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "order by 1 ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                gridView1.OptionsBehavior.Editable = true;


                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "ID";
                gridView1.Columns[2].Caption = "No RM";
                gridView1.Columns[3].Caption = "Tanggal";
                gridView1.Columns[4].Caption = "Nama Pasien";
                gridView1.Columns[5].Caption = "Penjamin";
                gridView1.Columns[6].Caption = "Ruangan";
                gridView1.Columns[7].Caption = "Tgl Masuk";
                gridView1.Columns[8].Caption = "Tgl Keluar";
                gridView1.Columns[9].Caption = "Status";
                gridView1.Columns[10].Caption = "Cara Masuk";
                gridView1.Columns[11].Caption = "Dari";
                gridView1.Columns[12].Caption = "Remark";
                gridView1.Columns[13].Caption = "Cara Keluar";
                gridView1.Columns[14].Caption = "Status Pasien";

                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listStat;
                stLookup.ValueMember = "statusCode";
                stLookup.DisplayMember = "statusName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listStat.Count;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = stLookup;

                RepositoryItemLookUpEdit cmLookup = new RepositoryItemLookUpEdit();
                cmLookup.DataSource = statIn;
                cmLookup.ValueMember = "statCode";
                cmLookup.DisplayMember = "statName";

                cmLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                cmLookup.DropDownRows = statIn.Count;
                cmLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                cmLookup.AutoSearchColumnIndex = 1;
                cmLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = cmLookup;

                RepositoryItemLookUpEdit drLookup = new RepositoryItemLookUpEdit();
                drLookup.DataSource = statFrom;
                drLookup.ValueMember = "statCode";
                drLookup.DisplayMember = "statName";

                drLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                drLookup.DropDownRows = statFrom.Count;
                drLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                drLookup.AutoSearchColumnIndex = 1;
                drLookup.NullText = "";
                gridView1.Columns[11].ColumnEdit = drLookup;

                RepositoryItemLookUpEdit ckLookup = new RepositoryItemLookUpEdit();
                ckLookup.DataSource = statOut;
                ckLookup.ValueMember = "statCode";
                ckLookup.DisplayMember = "statName";

                ckLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                ckLookup.DropDownRows = statOut.Count;
                ckLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                ckLookup.AutoSearchColumnIndex = 1;
                ckLookup.NullText = "";
                gridView1.Columns[13].ColumnEdit = ckLookup;

                RepositoryItemLookUpEdit sPLookup = new RepositoryItemLookUpEdit();
                sPLookup.DataSource = statPasien;
                sPLookup.ValueMember = "statCode";
                sPLookup.DisplayMember = "statName";

                sPLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                sPLookup.DropDownRows = statPasien.Count;
                sPLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                sPLookup.AutoSearchColumnIndex = 1;
                sPLookup.NullText = "";
                gridView1.Columns[14].ColumnEdit = sPLookup;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[6].OptionsColumn.ReadOnly = true;
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                gridView1.Columns[8].OptionsColumn.ReadOnly = true;
                gridView1.Columns[9].OptionsColumn.ReadOnly = true;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnSaveRanap_Click(object sender, EventArgs e)
        {
            string action = "", id = "", way_in = "", dari = "", remark = "", way_out = "", stat = "";
            string sql_update2 = "", sql_cnt = "", sql_insert = "", sql_update = "";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                id = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                way_in = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                dari = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                remark = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                way_out = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                stat = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();

                if (id == "")
                {
                    MessageBox.Show("Data tidak ditemukan");
                }
                else
                {
                    if (action == "I")
                    {

                    }
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.cs_inpatient set rs_in = '" + way_in + "', came_from = '" + dari + "', came_remark = '" + remark + "', rs_out = '" + way_out + "', patient_stat = '" + stat + "', ";
                        sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + " where inpatient_id = '" + id + "'  ";

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
            loadData();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

            GridView View = sender as GridView;
            if (e.Column.Caption == "Cara Masuk" || e.Column.Caption == "Dari" || e.Column.Caption == "Remark" || e.Column.Caption == "Cara Keluar" || e.Column.Caption == "Status Pasien")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "ranap_mngt.xls",
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

