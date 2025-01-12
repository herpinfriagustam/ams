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

namespace Clinic
{
    public partial class PatientReport : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();
        DataSet dsMRUmum = new DataSet();
        DataSet dsMRHamil = new DataSet();
        DataSet dsMRKb = new DataSet();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string type = "";
        ReportMRUmum reportUmum = null;
        ReportMRHamil reportHamil = null;
        ReportMRKb reportKb = null;

        public PatientReport()
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
            SQL = SQL + Environment.NewLine + "select a.patient_no, name, null dept, a.rm_no, ";
            SQL = SQL + Environment.NewLine + "address, birth_place || ' , ' || birth_date || ' ('  || round((sysdate-birth_date)/30/12) || ' Tahun)' as ttl, null age, gender, ";
            SQL = SQL + Environment.NewLine + "null blood_type, null as gpa, null hpht, ";
            SQL = SQL + Environment.NewLine + "null as tp ";
            SQL = SQL + Environment.NewLine + "from cs_patient a  ";
            SQL = SQL + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no)  ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and a.status = 'A' ";
            SQL = SQL + Environment.NewLine + "and group_patient = '" + p_type + "' order by name asc ";


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

                gridView1.Columns[0].Caption = "NIK";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Department";
                gridView1.Columns[3].Caption = "Medical Record";
                gridView1.Columns[4].Caption = "Alamat";
                gridView1.Columns[5].Caption = "TTL";
                gridView1.Columns[6].Caption = "Umur";
                gridView1.Columns[7].Caption = "Jenis Kelamin";
                gridView1.Columns[8].Caption = "Darah";
                gridView1.Columns[9].Caption = "GPA";
                gridView1.Columns[10].Caption = "HPHT";
                gridView1.Columns[11].Caption = "TP";

                gridView1.Columns[0].Width = 80;
                gridView1.Columns[1].Width = 150;
                gridView1.Columns[2].Width = 150;
                gridView1.Columns[3].Width = 120;

                gridView1.Columns[2].Visible = false;
                gridView1.Columns[4].Visible = false;
                gridView1.Columns[5].Visible = false;
                gridView1.Columns[6].Visible = false;
                gridView1.Columns[7].Visible = false;
                gridView1.Columns[8].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[11].Visible = false;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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
            GridView View = sender as GridView;
            string sql_mr_print="", s_nik = "", s_nama = "", s_rm = "", s_alamat = "", s_umur = "", s_jk = "", p_type = "";
            string s_dept = "", s_gpa = "", s_hpht = "", s_tp = "", s_darah = "";

            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_dept = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
            s_alamat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            s_jk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
            s_darah = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
            s_gpa = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            s_hpht = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_tp = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            if (luType.Text == "Umum")
            {
                p_type = "COMM";
                s_umur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
            }
            else if (luType.Text == "Ibu Hamil")
            {
                p_type = "PREG";
                s_umur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            }
            else if (luType.Text == "KB")
            {
                p_type = "FAMP";
                s_umur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            }

            //sql_mr_print = " select '" + s_nama + "' name, '" + s_nik + "' nik, '" + s_rm + "' rm, '" + s_alamat + "' addr, '" + s_umur + "' age, '" + s_jk + "' gender, " +
            //              " '" + s_dept + "' dept, '" + s_gpa + "' gpa, '" + s_hpht + "' hpht, '" + s_tp + "' tp, '" + s_darah + "' darah, " +
            //              " visit_no, to_char(b.insp_date,'yyyy-mm-dd') ddate,  " +
            //              " 'Tensi : ' || blood_press || ', Nadi : ' || pulse ||  " +
            //              " ', Suhu : ' || temperature || ', Alergi : ' || allergy ||  " +
            //              " ', Keluhan : ' || anamnesa as anamnesa,  " +
            //              " (select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  " +
            //              " from cs_diagnosa a  " +
            //              " join cs_diagnosa_item b on (a.item_cd = b.item_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) diagnosa,  " +
            //              " (select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep  " +
            //              " from cs_receipt a  " +
            //              " join cs_medicine b on (a.med_cd = b.med_cd)  " +
            //              " where b.status = 'A'  " +
            //              " and rm_no = b.rm_no  " +
            //              " and insp_date = b.insp_date  " +
            //              " and visit_no = b.visit_no) terapi  " +
            //              " from cs_patient a  " +
            //              " join cs_anamnesa b on (a.rm_no = b.rm_no)  " +
            //              " where a.status = 'A'  " +
            //              " and group_patient = '" + p_type + "'  " +
            //              " and b.rm_no = '" + s_rm + "' order by b.insp_date, visit_no desc ";

            sql_mr_print = "";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select '" + s_nama + "' name, '" + s_nik + "' nik, '" + s_rm + "' rm,";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'" + s_alamat + "' addr, '" + s_umur + "' age, '" + s_jk + "' gender,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'" + s_dept + "' dept, '" + s_gpa + "' gpa, '" + s_hpht + "' hpht, '" + s_tp + "' tp, '" + s_darah + "' darah,  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "visit_no, to_char(b.insp_date,'yyyy-mm-dd') ddate, "; 
            sql_mr_print = sql_mr_print + Environment.NewLine + "b.visit_no,   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select distinct poli_name from cs_visit aa   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_policlinic bb on (aa.poli_cd=bb.poli_cd)   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where trunc(visit_date)=b.insp_date   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and b.visit_no=que01   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select to_char(visit_date,'yyyy-mm-dd hh24:mi:ss') ddate   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_visit aa  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_patient bb  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "on aa.patient_no=bb.patient_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where bb.status='A'  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and trunc(visit_date)=b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and que01=b.visit_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and aa.ID_VISIT = c.ID_VISIT ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no=a.rm_no) ddate,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tensi : ' || blood_press || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Nadi : ' || pulse || ','    ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Suhu : ' || temperature || ','  ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'BB : ' || bb || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'TB : ' || tb || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Alergi : ' || allergy || ','      ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Sekarang : ' || disease_now || ','||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Kel : ' || disease_family || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa ,   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_diagnosa a   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_diagnosa_item b on (a.item_cd = b.item_cd)   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where b.status = 'A'   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no = b.rm_no  and   a.ANAMNESA_ID = b.ANAMNESA_ID  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and insp_date = b.insp_date    ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no = b.visit_no) diagnosa,    ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ') WITHIN GROUP (ORDER BY med_name asc) resep  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_receipt a   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd)  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A'  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_sick_leter a  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_observation a  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tindakan : ' || (select distinct act_name  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_action a , cs_treatment_detail x ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where a.detail_id = x.detail_id ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "   and x.HEAD_ID = d.HEAD_ID ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "   and rm_no = b.rm_no   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "   and insp_date = b.insp_date   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "   and visit_no = b.visit_no )    terapi,   ";

            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select distinct poli_name from cs_visit aa  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_policlinic bb on (aa.poli_cd=bb.poli_cd)  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "where trunc(visit_date)=b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no=que01  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd, ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select to_char(visit_date,'yyyy-mm-dd hh24:mi:ss') ddate  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_visit aa ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_patient bb ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "on aa.patient_no=bb.patient_no ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "where bb.status='A' ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and trunc(visit_date)=b.insp_date ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and que01=b.visit_no ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no=a.rm_no) ddate, ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Tensi : ' || blood_press || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Nadi : ' || pulse || ','    || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Suhu : ' || temperature || ','  || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'BB : ' || bb || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'TB : ' || tb || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Alergi : ' || allergy || ','      || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Sekarang : ' || disease_now || ','|| ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Kel : ' || disease_family || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa ,  ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa   ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_diagnosa a  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_diagnosa_item b on (a.item_cd = b.item_cd)  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "where b.status = 'A'  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no = b.rm_no   ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and insp_date = b.insp_date   ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no = b.visit_no) diagnosa,   ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep  ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_receipt a  ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_medicine b on (a.med_cd = b.med_cd)   ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "where b.status = 'A' ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no = b.rm_no   ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "and insp_date = b.insp_date   ";
            ////sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no = b.visit_no) terapi , ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_receipt a  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd) ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A' ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) || ', ' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_sick_leter a ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_observation a ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Tindakan : ' || (select act_name ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_action a ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_refer a ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_recommendation a ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no )  terapi,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "     klinik.FN_GET_PIC(b.rm_no, c.ID_VISIT) pic  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from cs_patient a   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_anamnesa b on (a.rm_no = b.rm_no)   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join cs_treatment_head d ON (b.ID_VISIT = d.ID_VISIT )  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join KLINIK.cs_visit c on (b.ID_VISIT = c.ID_VISIT)   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where a.status = 'A'   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and group_patient = '" + p_type + "'   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and b.rm_no = '" + s_rm + "'  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "order by b.insp_date ";


            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            if (luType.Text == "Umum")
            {
                dsMRUmum.Tables.Clear();
                dsMRUmum.Tables.Add(dt3);

                reportUmum = new ReportMRUmum(dsMRUmum);
                reportUmum.CreateDocument();
                //reportUmum.ShowPreviewDialog();
                documentViewer1.DocumentSource = reportUmum;
            }
            else if (luType.Text == "Ibu Hamil")
            {
                dsMRHamil.Tables.Clear();
                dsMRHamil.Tables.Add(dt3);

                reportHamil = new ReportMRHamil(dsMRHamil);
                reportHamil.CreateDocument();
                //reportHamil.ShowPreviewDialog();
                documentViewer1.DocumentSource = reportHamil;
            }
            else if (luType.Text == "KB")
            {
                dsMRKb.Tables.Clear();
                dsMRKb.Tables.Add(dt3);

                reportKb = new ReportMRKb(dsMRKb);
                reportKb.CreateDocument();
                //reportKb.ShowPreviewDialog();
                documentViewer1.DocumentSource = reportKb;
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (luType.Text == "Umum")
            {
                reportUmum.ShowPreviewDialog();
            }
            else if (luType.Text == "Ibu Hamil")
            {
                reportHamil.ShowPreviewDialog();
            }
            else if (luType.Text == "KB")
            {
                reportKb.ShowPreviewDialog();
            }
            

            
        }
    }
}