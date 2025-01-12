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
using Clinic.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors.Repository;

namespace Clinic
{
    public partial class Lap_KasHarian : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>(); List<Status> listStat2 = new List<Status>();
        List<PatientType> listPatientType = new List<PatientType>();
        List<WorkAccident> listWorkAccident = new List<WorkAccident>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "", sdate="", edate="";

        public Lap_KasHarian()
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

            listPatientType.Clear();
            listPatientType.Add(new PatientType() { patientTypeCode = "E", patientTypeName = "Emergency" });
            listPatientType.Add(new PatientType() { patientTypeCode = "U", patientTypeName = "Umum" });

            listWorkAccident.Clear();
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "Y", workAccidentName = "Yes" });
            listWorkAccident.Add(new WorkAccident() { workAccidentCode = "N", workAccidentName = "No" });

            listStat.Clear();
            listStat.Add(new Status() { statusCode = "", statusName = "All" });
            listStat.Add(new Status() { statusCode = "Tunai", statusName = "Tunai" });
            listStat.Add(new Status() { statusCode = "Transfer", statusName = "None Tunai" });
            listStat.Add(new Status() { statusCode = "B", statusName = "Piutang" });

            listStat2.Clear();
            listStat2.Add(new Status() { statusCode = "", statusName = "All" });
            listStat2.Add(new Status() { statusCode = "REG", statusName = "Pendaftaran" });
            listStat2.Add(new Status() { statusCode = "OPN", statusName = "Open" });
            listStat2.Add(new Status() { statusCode = "CLS", statusName = "Closed" });

            luType.Properties.DataSource = listStat;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusName";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listStat.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0; 

            string sql_date="";
            sql_date = " select to_char(sysdate,'yyyy-mm-dd') sdate, to_char(sysdate,'yyyy-mm-dd') edate from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            edate = dt.Rows[0]["edate"].ToString();
            dDateBgn.Text = sdate;
            dDateEnd.Text = edate;
        }

        private void LoadData()
        {
            string SQL, p_type = "";
 
            string Sql = "";
            Sql = Sql + Environment.NewLine + "  ";
            Sql = Sql + Environment.NewLine + "select distinct  d.VISIT_DATE, a.time_payment pay_date, ab.rm_no, b.name, ";
            Sql = Sql + Environment.NewLine + "       decode(type_patient,'B','BPJS','A','ASURANSI','UMUM') type_patient, ";
            Sql = Sql + Environment.NewLine + "       decode(a.plan,'TRT01','Rawat Jalan','TRT02','Rawat Inap','Lain-Lain') Pelayanan, ";
            Sql = Sql + Environment.NewLine + "       c.POLI_NAME, ";
            Sql = Sql + Environment.NewLine + "       (  ";
            Sql = Sql + Environment.NewLine + "        select max(NAME) ";
            Sql = Sql + Environment.NewLine + "          from KLINIK.cs_diagnosa ad,    ";
            Sql = Sql + Environment.NewLine + "               KLINIK.cs_user cd, ";
            Sql = Sql + Environment.NewLine + "               KLINIK.cs_anamnesa ca ";
            Sql = Sql + Environment.NewLine + "         where ad.ANAMNESA_ID = ca.ANAMNESA_ID   ";
            Sql = Sql + Environment.NewLine + "           and ad.INS_EMP = user_id ";
            Sql = Sql + Environment.NewLine + "           and ca.id_visit = a.id_visit ";
            Sql = Sql + Environment.NewLine + "       ) Dokter, ";
            Sql = Sql + Environment.NewLine + "       ( ";
            Sql = Sql + Environment.NewLine + "        select max(NAME) ";
            Sql = Sql + Environment.NewLine + "          from KLINIK.cs_receipt ad,    ";
            Sql = Sql + Environment.NewLine + "               KLINIK.cs_user cd  ";
            Sql = Sql + Environment.NewLine + "         where ad.ID_KASIR = user_id ";
            Sql = Sql + Environment.NewLine + "           and ad.id_visit = a.id_visit ";
            Sql = Sql + Environment.NewLine + "       ) Kasir, d.TOTAL_PAY, decode(type_patient,'B','Piutang',d.STS_PAY) STS_PAY,a.id_visit ";
            Sql = Sql + Environment.NewLine + " FROM cs_visit a   ";
            Sql = Sql + Environment.NewLine + " JOIN cs_patient_info b ON a.patient_no = b.patient_no   ";
            Sql = Sql + Environment.NewLine + " JOIN cs_patient ab     ON ab.patient_no = b.patient_no   ";
            Sql = Sql + Environment.NewLine + " JOIN cs_policlinic c ON a.poli_cd = c.poli_cd   ";
            Sql = Sql + Environment.NewLine + " join KLINIK.cs_treatment_head d on (d.id_visit = a.id_visit)  ";
            Sql = Sql + Environment.NewLine + " join KLINIK.cs_treatment_detail e on (d.head_id=e.head_id )  ";
            Sql = Sql + Environment.NewLine + " JOIN KLINIK.cs_treatment_item f  on (e.treat_item_id=f.treat_item_id)   ";
            Sql = Sql + Environment.NewLine + "where 1=1 ";
             
            if(radioButton1.Checked)
                Sql = Sql + Environment.NewLine + "   and trunc(d.VISIT_DATE) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";
            else if (radioButton2.Checked)
                Sql = Sql + Environment.NewLine + "   and trunc(a.time_payment) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd')  ";

            if(luType.EditValue.ToString().Equals("Tunai"))
                Sql = Sql + Environment.NewLine + "   and d.STS_PAY  ='" + luType.EditValue.ToString() + "' ";
            else if (luType.EditValue.ToString().Equals("Transfer"))
                Sql = Sql + Environment.NewLine + "   and  d.STS_PAY   ='" + luType.EditValue.ToString() + "' ";
            else if (luType.EditValue.ToString().Equals("BPJS"))
                Sql = Sql + Environment.NewLine + "   and type_patient  ='" + luType.EditValue.ToString() + "' ";
            Sql = Sql + Environment.NewLine + "order by d.VISIT_DATE, a.time_payment     ";

             
            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(Sql, sqlConnect);
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

                //gridView1.FixedLineWidth = 2;
                //gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "TGL KUNJUNGAN";  
                gridView1.Columns[1].Caption = "TGL BAYAR";
                gridView1.Columns[2].Caption = "RM NO";
                gridView1.Columns[3].Caption = "NAMA PASIEN";
                gridView1.Columns[4].Caption = "TYPE PASIEN";
                gridView1.Columns[5].Caption = "PELAYANAN";
                gridView1.Columns[6].Caption = "POLI";
                gridView1.Columns[7].Caption = "NAMA DOKTER/BIDAN";
                gridView1.Columns[8].Caption = "NAMA KASIR";
                gridView1.Columns[9].Caption = "TOTAL HARGA";
                gridView1.Columns[10].Caption = "STATUS";
                gridView1.Columns[11].Caption = "IDVISIT";
                gridView1.Columns[11].Visible = false; 

                gridView1.BestFitColumns();
                //gridView1.Columns[18].Width = 100;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                 
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

            //if (e.Column.Caption == "KK")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            //    if (kk == "Yes")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}

            if (e.Column.Caption == "Tipe Pasien")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
                if (kk == "BPJS")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Green);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Green);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            //if (e.Column.Caption == "Lama Reservasi")
            //{
            //    string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[15]);
            //    if (e.RowHandle > 0 && Convert.ToInt16(kk) > 60)
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}
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
          
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "laporan_kunjungan.xls",
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            labelControl3.Text = "Tanggal Kunjungan";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            labelControl3.Text = "Tanggal Bayar";
        }
         
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}