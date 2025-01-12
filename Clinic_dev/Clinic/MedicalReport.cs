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
    public partial class MedicalReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "", sdate="", edate="";

        public MedicalReport()
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
            listStat.Clear();
            listStat.Add(new Status() { statusCode = "COMM", statusName = "Umum" });
            listStat.Add(new Status() { statusCode = "PREG", statusName = "Ibu Hamil" });
            listStat.Add(new Status() { statusCode = "FAMP", statusName = "KB" });
            
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
            sql_date = " select to_char(trunc(sysdate,'MM'),'yyyy-mm-dd') sdate, to_char(last_day(sysdate),'yyyy-mm-dd') edate from dual ";

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

            if (luType.Text == "Umum")
            {
                p_type = "COMM";
            }
            else if (luType.Text == "Ibu Hamil")
            {
                p_type = "PREG";
            }
            else if (luType.Text == "KB")
            {
                p_type = "FAMP";
            }
            

            SQL = "";
            SQL = SQL + Environment.NewLine + "select patient_no, name, dept, group_patient, work_accident, que01, vdate, ";
            SQL = SQL + Environment.NewLine + "anamnesa, diagnosa, terapi || ' ' ||  ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_RESEP_OUT(rm_no,que01,to_date(visit_date,'yyyy-mm-dd')) as terapi, skd,  ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_PIC(rm_no, to_date(visit_date, 'yyyy-mm-dd'), que01) pic ";
            SQL = SQL + Environment.NewLine + "from ( ";
            SQL = SQL + Environment.NewLine + "select c.rm_no,a.patient_no, name, null dept,  ";
            SQL = SQL + Environment.NewLine + "decode(group_patient,'COMM','Umum','PREG','Ibu Hamil','FAMP','KB') group_patient,  ";
            SQL = SQL + Environment.NewLine + "work_accident, que01, to_char(visit_date,'yyyy-mm-dd') visit_date, to_char(visit_date,'yyyy-mm-dd hh24:mi:ss') vdate,  ";
            //SQL = SQL + Environment.NewLine + "(select 'Tensi : ' || blood_press || ', Nadi : ' || pulse ||     ";
            //SQL = SQL + Environment.NewLine + " ', Suhu : ' || temperature || ', Alergi : ' || allergy ||     ";
            //SQL = SQL + Environment.NewLine + " ', Keluhan : ' || anamnesa as anamnesa from cs_anamnesa  ";
            SQL = SQL + Environment.NewLine + "(select  ";
            SQL = SQL + Environment.NewLine + "'Tensi : ' || blood_press || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'Nadi : ' || pulse || ',' ||        ";
            SQL = SQL + Environment.NewLine + "'Suhu : ' || temperature || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'BB : ' || bb || ',' || ";
            SQL = SQL + Environment.NewLine + "'TB : ' || tb || ',' || ";
            SQL = SQL + Environment.NewLine + "'Alergi : ' || allergy || ',' ||      ";
            SQL = SQL + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'R.Sekarang : ' || disease_now || ',' || ";
            SQL = SQL + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'R.Kel : ' || disease_family || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa ";
            SQL = SQL + Environment.NewLine + " from cs_anamnesa where insp_date=trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no=que01  ";
            SQL = SQL + Environment.NewLine + " and rm_no=c.rm_no) anamnesa,  ";
            //SQL = SQL + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  ";
            SQL = SQL + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            SQL = SQL + Environment.NewLine + " from cs_diagnosa a     ";
            SQL = SQL + Environment.NewLine + " join cs_diagnosa_item b on (a.item_cd = b.item_cd)  ";
            SQL = SQL + Environment.NewLine + " where b.status = 'A'     ";
            SQL = SQL + Environment.NewLine + " and rm_no = c.rm_no     ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01) diagnosa,  ";
            //SQL = SQL + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep  ";
            //SQL = SQL + Environment.NewLine + " from cs_receipt a   ";
            //SQL = SQL + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd)   ";
            //SQL = SQL + Environment.NewLine + " where b.status = 'A'   ";
            //SQL = SQL + Environment.NewLine + " and rm_no = c.rm_no     ";
            //SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            //SQL = SQL + Environment.NewLine + " and visit_no = que01) terapi,  ";
            SQL = SQL + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ') WITHIN GROUP (ORDER BY med_name asc) resep  ";
            SQL = SQL + Environment.NewLine + " from cs_receipt a   ";
            SQL = SQL + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd)  ";
            SQL = SQL + Environment.NewLine + " where b.status = 'A'  ";
            SQL = SQL + Environment.NewLine + " and rm_no = c.rm_no   ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01) || ', ' || ";
            SQL = SQL + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
            SQL = SQL + Environment.NewLine + " from cs_sick_leter a ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date) ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01) || ', ' || ";
            SQL = SQL + Environment.NewLine + "'OBS : ' || (select sum(hrs_cnt) hrs_cnt ";
            SQL = SQL + Environment.NewLine + " from cs_observation a ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date) ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01 ) || ', ' ||  ";
            SQL = SQL + Environment.NewLine + "'Tindakan : ' || (select act_name ";
            SQL = SQL + Environment.NewLine + " from cs_action a ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01 ) || ', ' ||  ";
            SQL = SQL + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
            SQL = SQL + Environment.NewLine + " from cs_refer a ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01 ) || ', ' ||  ";
            SQL = SQL + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
            SQL = SQL + Environment.NewLine + " from cs_recommendation a ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date) ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01)  terapi,  ";
            SQL = SQL + Environment.NewLine + "nvl((select 'Y' skd   ";
            SQL = SQL + Environment.NewLine + " from cs_sick_leter  ";
            SQL = SQL + Environment.NewLine + " where rm_no = c.rm_no  ";
            SQL = SQL + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            SQL = SQL + Environment.NewLine + " and visit_no = que01),'N') skd,  ";
            SQL = SQL + Environment.NewLine + " a.ins_date  ";
            SQL = SQL + Environment.NewLine + "from cs_visit a    ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no)    ";
            SQL = SQL + Environment.NewLine + "join cs_patient c on (b.patient_no=c.patient_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and a.status <> 'CAN'";
            SQL = SQL + Environment.NewLine + "and group_patient = '" + p_type + "' ";
            if (p_type == "COMM")
            {
                SQL = SQL + Environment.NewLine + "and a.poli_cd in ('POL0001','POL0000') ";
            }
            else if (p_type == "PREG")
            {
                SQL = SQL + Environment.NewLine + "and a.poli_cd = 'POL0002' ";
            }
            else if (p_type == "FAMP")
            {
                SQL = SQL + Environment.NewLine + "and a.poli_cd = 'POL0003' ";
            }
            SQL = SQL + Environment.NewLine + "and trunc(visit_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd') ) ";
            SQL = SQL + Environment.NewLine + "order by ins_date  ";



            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
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

                gridView1.Columns[0].Caption = "Pasien No";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Department";
                gridView1.Columns[3].Caption = "Type";
                gridView1.Columns[4].Caption = "KK";
                gridView1.Columns[5].Caption = "Antrian";
                gridView1.Columns[6].Caption = "Tanggal";
                gridView1.Columns[7].Caption = "Anamnesa";
                gridView1.Columns[8].Caption = "Diagnosa";
                gridView1.Columns[9].Caption = "Terapi";
                gridView1.Columns[10].Caption = "SKD";
                gridView1.Columns[11].Caption = "Pemeriksa";

                gridView1.Columns[0].Width = 80;
                gridView1.Columns[1].Width = 150;
                gridView1.Columns[2].Width = 150;
                gridView1.Columns[3].Width = 50;
                gridView1.Columns[4].Width = 40;
                gridView1.Columns[5].Width = 50;
                gridView1.Columns[6].Width = 80;
                gridView1.Columns[7].Width = 250;
                gridView1.Columns[8].Width = 250;
                gridView1.Columns[9].Width = 250;
                gridView1.Columns[10].Width = 60;
                //gridView1.Columns[11].Width = 150;

                RepositoryItemMemoEdit tgl = new RepositoryItemMemoEdit();
                tgl.WordWrap = true;
                gridView1.Columns[6].ColumnEdit = tgl;

                RepositoryItemMemoEdit anamnesa = new RepositoryItemMemoEdit();
                anamnesa.WordWrap = true;
                gridView1.Columns[7].ColumnEdit = anamnesa;

                RepositoryItemMemoEdit diagnosa = new RepositoryItemMemoEdit();
                diagnosa.WordWrap = true;
                gridView1.Columns[8].ColumnEdit = diagnosa;

                RepositoryItemMemoEdit terapi = new RepositoryItemMemoEdit();
                terapi.WordWrap = true;
                gridView1.Columns[9].ColumnEdit = terapi;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[2].Visible = false;
                gridView1.Columns[4].Visible = false;

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

            if (e.Column.Caption == "KK")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
                if (kk == "Y")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.OrangeRed);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.Caption == "SKD")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
                if (kk == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
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
          
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "medical_report.xls",
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