using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace Clinic
{
    public partial class RawatInapBidan : DevExpress.XtraEditors.XtraForm
    {
        private KoneksiOra koneksi; ConnectDb ConnOra = new ConnectDb();
        DataTable dt_grdPersalinanLalu; DataTable dt_grdPemberianAnstesi; DataTable datstock = new DataTable();
        DataTable dt_grdSebelumBedah; DataTable dt_grdSetelahBedah;
        DataTable dt_grdSPemantauanAnastesih; DataTable dt_grdPemantauanIv;
        DataTable dtMedis; DataTable dtVisitDokter; DataTable dtObat;  DataTable dtGlMed = new DataTable();

        List<Layanan> listLaya2 = new List<Layanan>(); List<Layanan> listLayav = new List<Layanan>();
        List<Dokter> listDokter = new List<Dokter>(); List<Dosis> listDosis = new List<Dosis>();
        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Stat> listHours = new List<Stat>();
        List<Formula> listFormula = new List<Formula>();
        List<Formula2> listFormula2 = new List<Formula2>();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();

        string _AnamesaID = "" ;
        private string anamesaID = "", visitid = "", headid = "",   RMNO = "", pasienno = "", type_s = "";
        public string v_ptnumber = ""; 
        string _RM_NO = "";
        string _Name = "";
        public RawatInapBidan()
        {
            InitializeComponent();
            koneksi = new KoneksiOra();

            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            //System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
            //cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            foreach (GridColumn column in  gvPelayanBidan.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
            foreach (GridColumn column in gvMedisBidan.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
            foreach (GridColumn column in gvVisitBidan.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
            foreach (GridColumn column in gvObatUmumBidan.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
        }

        private void RawatInapBidan_Load(object sender, EventArgs e)
        {

            //txt_anastesi_id.Text = "11111111";
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "RawatInapBidan");
            selectedIndexRb();
            kondisiEnable(false);

            //string aaaa = "0::Asfiksia ringan::mengeringkan::menghangatkan::rangsangan taktil::::::::lain-lain::1";
            //string[] aa = aaaa.Split(new string[] { "::" }, StringSplitOptions.None);

            //MessageBox.Show(aa.Length.ToString());


            //Get Data Pasien
            splitContainerControlMain.SplitterPosition = 200;
            try
            {

                LoadRIBidan();
                //visitid = FN.strVal(gvwPasien, gvwPasien.FocusedRowHandle, "ID_VISIT");
                //headid = FN.strVal(gvwPasien, gvwPasien.FocusedRowHandle, "HEAD_ID");

                LoadItemLayanan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal get Data Pasien !");
            }
        }
        private void LoadRIBidan()
        {
            string dataPasen = @"SELECT  DISTINCT A.ANAMNESA_ID,
                                           A.RM_NO,
                                           B.PATIENT_NO,
                                           to_char(A.INSP_DATE,'yyyy-MM-dd') INSP_DATE,
                                           C.NAME,
                                           DECODE(D.TYPE_PATIENT, 'U', 'Umum','B','BPJS','Swasta') GROUP_PATIENT,
                                           DECODE(F.STATUS,'OPN', 'Proses', 'REG', 'Registrasi','CLS','Selesai', 'Batal') STATUS,
                                           C.FAMILY_HEAD, A.ID_VISIT, E.HEAD_ID, (select ROOM_NAME||' ['||substr(f.room_id,-2)||']' from CS_ROOM g, CS_BED h where g.room_id = h.room_id  and h.BED_ID = f.room_id ) room_id 
                                      FROM CS_ANAMNESA A, CS_PATIENT B, CS_PATIENT_INFO C, CS_VISIT D, CS_TREATMENT_HEAD E, KLINIK.cs_inpatient F
                                      WHERE A.ID_VISIT = D.ID_VISIT AND D.inpatient_id=f.inpatient_id
                                        and d.ID_VISIT = E.ID_VISIT
                                        AND D.STATUS not in ('CLS','CAN')  and F.status in ('OPN','PAY') 
                                        AND B.PATIENT_NO = D.PATIENT_NO
                                        AND B.PATIENT_NO = C.PATIENT_NO AND d.POLI_CD ='POL0004' and d.plan = 'TRT02' and d.purpose ='MID'";

            //grdPasien.DataSource = ConnOra.Data_Table_ora(dataPasen);
            //gvwPasien.BestFitColumns();

            DataTable dataPasien = koneksi.GetDataTable(dataPasen);
           
            grdPasien.DataSource = dataPasien;
            ConvertColumnNamesToUppercase(dataPasien);
            gvwPasien.OptionsView.ColumnAutoWidth = true;
            gvwPasien.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvwPasien.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvwPasien.IndicatorWidth = 30;
            gvwPasien.OptionsBehavior.Editable = false; 
            gvwPasien.BestFitColumns();
            //gvwPasien.Columns[2].OptionsColumn.ReadOnly = true;
            //gvwPasien.Columns[6].OptionsColumn.ReadOnly = true;
            //gvwPasien.Columns[7].OptionsColumn.ReadOnly = true;
            //gvwPasien.Columns[8].OptionsColumn.ReadOnly = true;


        }
        private void LoadItemLayanan()
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG01','TRG16') ";

            DataTable dtly = ConnOra.Data_Table_ora(SQL);

            //OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            //DataTable dtly = new DataTable();
            //adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            string SQL1 = "";
            SQL1 = SQL1 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL1 = SQL1 + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL1 = SQL1 + Environment.NewLine + "where 1=1 ";
            SQL1 = SQL1 + Environment.NewLine + "and treat_type_id = 'TRT02' and TREAT_GROUP_ID ='TRG16' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%DOKTER%'  ";

            DataTable dtlv = ConnOra.Data_Table_ora(SQL);
            //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOrav = new OleDbDataAdapter(SQL1, oraConnect);
            //DataTable dtlv = new DataTable();
            //adOrav.Fill(dtlv);
            listLayav.Clear();
            for (int i = 0; i < dtlv.Rows.Count; i++)
            {
                listLayav.Add(new Layanan() { layananCode = dtlv.Rows[i]["treat_item_id"].ToString(), layananName = dtlv.Rows[i]["treat_item_name"].ToString() });
            }

            dtGlMed.Clear();
            string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A'  and MED_GROUP ='OBAT' order by med_name ";
            DataTable dt3 = ConnOra.Data_Table_ora(sql_med);

            //OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            //DataTable dt3 = new DataTable();
            dtGlMed = dt3;
            //adSql3.Fill(dt3);
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }

            string SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select ID_DOKTER, initcap(NM_DOKTER) Nama_Dokter ";
            SQL2 = SQL2 + Environment.NewLine + "from KLINIK.CS_DOKTER ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1 AND F_AKTIF ='Y' and NM_DOKTER <> 'System' ";
            //SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT01'  ";

            DataTable dtdok = ConnOra.Data_Table_ora(SQL2);

            //OleDbConnection oraConny = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOra = new OleDbDataAdapter(SQL2, oraConny);
            //DataTable dtdok = new DataTable();
            //adOra.Fill(dtdok);
            listDokter.Clear();
            for (int i = 0; i < dtdok.Rows.Count; i++)
            {
                listDokter.Add(new Dokter() { ID_Dokter = dtdok.Rows[i]["ID_DOKTER"].ToString(), Nama_Dokter = dtdok.Rows[i]["Nama_Dokter"].ToString() });
            }

            string sql_dosis = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'DOSIS' order by SORT_ORDER ";
            DataTable dtgsis = ConnOra.Data_Table_ora(sql_dosis);
            //OleDbConnection oraCondsd = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOrados = new OleDbDataAdapter(sql_dosis, oraCondsd);
            //DataTable dtgsis = new DataTable();
            //adOrados.Fill(dtgsis);
            listDosis.Clear();
            for (int i = 0; i < dtgsis.Rows.Count; i++)
            {
                listDosis.Add(new Dosis() { DosisCode = dtgsis.Rows[i]["code_id"].ToString(), DosisName = dtgsis.Rows[i]["code_name"].ToString() });
            }

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            string sql_poli = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item  where status = 'A' and CAT_ID ='CAT030'  order by item_name ";
            DataTable dtd = ConnOra.Data_Table_ora(sql_poli);
             
            //listDiagnosa.Clear();
            for (int i = 0; i < dtd.Rows.Count; i++)
            {
                listDiagnosa.Add(new Diagnosa() { diagnosaCode = dtd.Rows[i]["item_cd"].ToString(), diagnosaName = dtd.Rows[i]["item_name"].ToString() });
            }

            txt_skl4_diagnosa.Properties.DataSource = listDiagnosa;
            txt_skl4_diagnosa.Properties.ValueMember = "diagnosaCode";
            txt_skl4_diagnosa.Properties.DisplayMember = "diagnosaName";

            txt_skl4_diagnosa.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            txt_skl4_diagnosa.Properties.DropDownRows = listDiagnosa.Count;
            txt_skl4_diagnosa.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            txt_skl4_diagnosa.Properties.AutoSearchColumnIndex = 1;
            txt_skl4_diagnosa.Properties.NullText = "";
            txt_skl4_diagnosa.ItemIndex = -1;

            txt_diagnosa_akhir.Properties.DataSource = listDiagnosa;
            txt_diagnosa_akhir.Properties.ValueMember = "diagnosaCode";
            txt_diagnosa_akhir.Properties.DisplayMember = "diagnosaName";

            txt_diagnosa_akhir.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            txt_diagnosa_akhir.Properties.DropDownRows = listDiagnosa.Count;
            txt_diagnosa_akhir.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            txt_diagnosa_akhir.Properties.AutoSearchColumnIndex = 1;
            txt_diagnosa_akhir.Properties.NullText = "";
            txt_diagnosa_akhir.ItemIndex = -1;
        }

        private void LoadItemLayananType(string t_status)
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG01','TRG16') ";
            SQL = SQL + Environment.NewLine + "and f_status  = '" + t_status + "' ";
            if( t_status.ToString().Equals("B"))
            { 
                SQL = SQL + Environment.NewLine + "union all ";
                SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) ||' [None BPJS]' treat_item_name  ";
                SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item  ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG01','TRG16')  ";
                SQL = SQL + Environment.NewLine + "and f_status  = 'U' ";
                SQL = SQL + Environment.NewLine + "and  initcap(treat_item_name) not in ( ";
                SQL = SQL + Environment.NewLine + "    select initcap(treat_item_name) treat_item_name  ";
                SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item  ";
                SQL = SQL + Environment.NewLine + "where 1=1  ";
                SQL = SQL + Environment.NewLine + "and treat_type_id = 'TRT02' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%VISIT DOKTER%'  and TREAT_GROUP_ID not in('TRG01','TRG16')  ";
                SQL = SQL + Environment.NewLine + "and f_status  = '" + t_status + "' )";

            }
            SQL = SQL + Environment.NewLine +  " Order by 2 ";

            DataTable dtly = ConnOra.Data_Table_ora(SQL); 
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
                listLayav.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            //string SQL1 = "";
            //SQL1 = SQL1 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            //SQL1 = SQL1 + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            //SQL1 = SQL1 + Environment.NewLine + "where 1=1 ";
            //SQL1 = SQL1 + Environment.NewLine + "and treat_type_id = 'TRT02' and TREAT_GROUP_ID ='TRG16' AND UPPER(TREAT_ITEM_NAME) NOT LIKE '%DOKTER%'  ";
            //SQL1 = SQL1 + Environment.NewLine + "   and f_status  = '" + t_status + "' ";

            //DataTable dtlv = ConnOra.Data_Table_ora(SQL1); 
            //listLayav.Clear();
            //for (int i = 0; i < dtlv.Rows.Count; i++)
            //{
            //    listLayav.Add(new Layanan() { layananCode = dtlv.Rows[i]["treat_item_id"].ToString(), layananName = dtlv.Rows[i]["treat_item_name"].ToString() });
            //}

            dtGlMed.Clear();
            string sql_med = "";
            if (t_status.ToString().Equals("B"))
            {
                sql_med = "";
                sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name)  med_name   ";
                sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS' ";
                sql_med = sql_med + Environment.NewLine + "    and POLI_CD ='POL0001'    ";
                sql_med = sql_med + Environment.NewLine + "  UNION ALL ";
                sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name) || ' [None BPJS)' med_name   ";
                sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 in('UMUM','ALL') ";
                sql_med = sql_med + Environment.NewLine + "    and POLI_CD = 'POL0001'    ";
                sql_med = sql_med + Environment.NewLine + "    and b.med_cd not in ( select b.med_cd  ";
                sql_med = sql_med + Environment.NewLine + "                           from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_med = sql_med + Environment.NewLine + "                            and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = 'BPJS' ";
                sql_med = sql_med + Environment.NewLine + "                            and POLI_CD ='POL0001'  ";
                sql_med = sql_med + Environment.NewLine + "                        ) ";
                sql_med = sql_med + Environment.NewLine + "  order by med_name ";
            }               
            else
            {
                sql_med = "";
                sql_med = sql_med + Environment.NewLine + " select b.med_cd, initcap(med_name)  med_name   ";
                sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1     ";
                sql_med = sql_med + Environment.NewLine + "    and a.status = 'A' and MED_GROUP ='OBAT'  and MINUS_STOK ='Y'  and att1 = decode('" + t_status.ToString() + "','U','UMUM','ASURANSI') ";
                sql_med = sql_med + Environment.NewLine + "    and POLI_CD ='POL0001'    "; 
                sql_med = sql_med + Environment.NewLine + "  order by med_name ";
            }

            DataTable dt3 = ConnOra.Data_Table_ora(sql_med);
             
            dtGlMed = dt3; 
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }

            string SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select ID_DOKTER, initcap(NM_DOKTER) Nama_Dokter ";
            SQL2 = SQL2 + Environment.NewLine + "from KLINIK.CS_DOKTER ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1 AND F_AKTIF ='Y' and NM_DOKTER <> 'System'  "; 

            DataTable dtdok = ConnOra.Data_Table_ora(SQL2); 
            listDokter.Clear();
            for (int i = 0; i < dtdok.Rows.Count; i++)
            {
                listDokter.Add(new Dokter() { ID_Dokter = dtdok.Rows[i]["ID_DOKTER"].ToString(), Nama_Dokter = dtdok.Rows[i]["Nama_Dokter"].ToString() });
            }

            string sql_dosis = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'DOSIS' order by SORT_ORDER ";
            DataTable dtgsis = ConnOra.Data_Table_ora(sql_dosis); 
            listDosis.Clear();
            for (int i = 0; i < dtgsis.Rows.Count; i++)
            {
                listDosis.Add(new Dosis() { DosisCode = dtgsis.Rows[i]["code_id"].ToString(), DosisName = dtgsis.Rows[i]["code_name"].ToString() });
            } 
        }

        private void grdPasien_DoubleClick(object sender, EventArgs e)
        {
            if (gvwPasien.RowCount <= 0) return;

            _AnamesaID = gvwPasien.GetRowCellValue(gvwPasien.FocusedRowHandle, "ANAMNESA_ID").ToString(); // _AnamesaID
            _RM_NO = gvwPasien.GetRowCellValue(gvwPasien.FocusedRowHandle, "RM_NO").ToString(); // _RM_NO
            _Name = gvwPasien.GetRowCellValue(gvwPasien.FocusedRowHandle, "NAME").ToString();
            visitid = gvwPasien.GetRowCellValue(gvwPasien.FocusedRowHandle, "ID_VISIT").ToString();
             
            headid = FN.strVal(gvwPasien, gvwPasien.FocusedRowHandle, "HEAD_ID"); 
            pasienno = FN.strVal(gvwPasien, gvwPasien.FocusedRowHandle, "PATIENT_NO");
            type_s = FN.strVal(gvwPasien, gvwPasien.FocusedRowHandle, "GROUP_PATIENT");

            if (type_s.ToString().Equals("Umum") || type_s.ToString().Equals("Swasta"))
            {
                type_s = "U";
                panelControl3.Visible = false;
            }
            else
            {
                panelControl3.Visible = true;
                type_s = "B";
            }

            t_anastesi_id.Text = _AnamesaID;
            t_nama.Text = _Name;
            t_rekam_medis.Text = _RM_NO;

            LoadItemLayananType(type_s);

            try
            { 
                string query = @"select count(*) from CS_ANAMNESA tp where ANAMNESA_ID = '" + _AnamesaID + "'";
                object result = koneksi.GetScalar(query);

                if (Convert.ToInt32(result) >= 1)
                {
                    getData(_AnamesaID);

                }
                else
                {
                    string queryInsert = "insert all \n";
                    queryInsert += @"into T2_R_INAP_BIDAN (id,anamesa_id) values (r_inap_bidan_seq.nextval, '" + _AnamesaID + "') \n";
                    queryInsert += @"into T2_R_INAP_BIDAN_1 (id,anamesa_id) values (r_inap_bidan_1_seq.nextval, '" + _AnamesaID + "') \n";
                    queryInsert += @"into T2_R_INAP_BIDAN_2 (id,anamesa_id) values (r_inap_bidan_2_seq.nextval,'" + _AnamesaID + "') \n";

                    queryInsert += @"into T2_DOKUMEN_SKALA_I (id,anamesa_id) values (dokumen_skala_i_seq.nextval,'" + _AnamesaID + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_II (id,anamesa_id) values (dokumen_skala_ii_seq.nextval,'" + _AnamesaID + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_III (id,anamesa_id) values (dokumen_skala_iii_seq.nextval,'" + _AnamesaID + "') \n";
                    queryInsert += @"into T2_DOKUMEN_SKALA_IV (id,anamesa_id) values (dokumen_skala_iv_seq.nextval,'" + _AnamesaID + "') \n";
                    queryInsert += "SELECT * FROM dual ";


                    bool success = koneksi.ExecuteNonQuery(queryInsert);
                    if (success)
                    {
                        loadDataGrid(_AnamesaID);
                    }
                    else
                    {
                        MessageBox.Show("Load Data Gagal !!");
                        return;
                    } 
                }

                kondisiEnable(true);
                //btnInputData.Enabled = false;
                btnSave.Enabled = true;
                btn_Tindakan.Enabled = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Load data gagal !!");

            }
        } 

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (_AnamesaID == "")
            {
                MessageBox.Show("Anastesi ID Kosong !!");
                return;
            }

            updateTable();
        }
        FrmTindakan FrmTindakan = null;
        private void btn_Tindakan_Click(object sender, EventArgs e)
        {
            FrmTindakan = new FrmTindakan();
            FrmTindakan.p_anamnesa_id = v_ptnumber;
            FrmTindakan.prekam_medis = t_rekam_medis.Text;
            FrmTindakan.pnama = txt_nama_istr.Text ;
            //FrmTindakan.MdiParent = this;
            //ReportForm.DB.vUserId = userEmpid;
            FrmTindakan.ShowDialog();
            FrmTindakan.Focus();
            //FrmTindakan Frm_Tindakan = new FrmTindakan(_AnamesaID, _RM_NO,_Name);

            //Frm_Tindakan.ShowDialog();
            //Frm_Tindakan.BringToFront();
        }

        private void getData( string id)
        {

            string querySelect = @"SELECT distinct a.*, 
                                           B.PATIENT_NO,
                                           to_char(A.INSP_DATE,'yyyy-MM-dd') INSP_DATE, 
                                           DECODE(B.GROUP_PATIENT, 'COMM', 'Umum','BPJS') GROUP_PATIENT,
                                           DECODE(B.STATUS,'A', 'Register', 'Progress') STATUS,
                                           C.FAMILY_HEAD,  E.HEAD_ID,
                                           C.NAME, round(((sysdate-c.birth_date)/30)/12) umur, c.AGAMA,
                                           c.PENDIDIKAN_TR, c.JOB,   C.ADDRESS,   
                                           G.*       
                                       FROM CS_ANAMNESA A, CS_PATIENT B, CS_PATIENT_INFO C, CS_VISIT D, CS_TREATMENT_HEAD E, KLINIK.cs_inpatient F, T2_R_INAP_BIDAN G
                                      WHERE A.ID_VISIT = D.ID_VISIT AND D.inpatient_id=f.inpatient_id
                                        AND d.ID_VISIT = E.ID_VISIT
                                        AND A.ANAMNESA_ID = G.anamesa_id
                                        AND D.STATUS not in ('CLS','CAN')  
                                        AND F.status in ('OPN','PAY') 
                                        AND B.PATIENT_NO = D.PATIENT_NO
                                        AND B.PATIENT_NO = C.PATIENT_NO
                                        AND d.POLI_CD ='POL0004' and d.plan = 'TRT02' and d.purpose ='MID'
                                        AND A.ANAMNESA_ID =  " + id + "";

            DataTable dataTable = koneksi.GetDataTable(querySelect);

            if (dataTable.Rows.Count > 0)
            {
                txt_nama_istr.EditValue = dataTable.Rows[0]["NAME"].ToString();
                txt_umur_istr.EditValue = dataTable.Rows[0]["umur"].ToString();
                //txt_agama_istr.EditValue = dataTable.Rows[0]["AGAMA"].ToString();

                //string txt_pendidikan_is = dataTable.Rows[0]["PENDIDIKAN_TR"].ToString();
                //string[] txt_pendidikan_ist = txt_pendidikan_is.ToString().Split(new string[] { "::" }, StringSplitOptions.None);

                //if (txt_pendidikan_ist.Length >= 2)
                //{
                //    //txt_umum_vita_td.EditValue = Split_stxt_umum_vita_td[0];
                //    txt_pendidikan_istr.EditValue = txt_pendidikan_ist[1];
                //}

                functionSplitIndex_1(dataTable.Rows[0]["AGAMA"].ToString(), txt_agama_istr);
                functionSplitIndex_1(dataTable.Rows[0]["PENDIDIKAN_TR"].ToString(), txt_pendidikan_istr);
                functionSplitIndex_1(dataTable.Rows[0]["JOB"].ToString(), txt_pekerjaan_istr);
                functionSplitIndex_1(dataTable.Rows[0]["ANAMNESA"].ToString(), txt_biodata_keluhan);
                functionSplitIndex_1(dataTable.Rows[0]["ADDRESS"].ToString(), txt_biodata_alamat);

                //functionSplitIndex_1(dataTable.Rows[0]["PENDIDIKAN_TR"].ToString(), txt_pendidikan_istr);
                //functionSplitIndex_1(dataTable.Rows[0]["PENDIDIKAN_TR"].ToString(), txt_pendidikan_istr);
                //functionSplitIndex_1(dataTable.Rows[0]["PENDIDIKAN_TR"].ToString(), txt_pendidikan_istr);

                //txt_pendidikan_istr.EditValue = dataTable.Rows[0]["PENDIDIKAN_TR"].ToString();
                //txt_pekerjaan_istr.EditValue = dataTable.Rows[0]["JOB"].ToString();
                txt_suku_istr.EditValue = dataTable.Rows[0]["suku_istr"].ToString();
                txt_kawin_lama_istr.EditValue = dataTable.Rows[0]["kawin_lama_istr"].ToString();
                txt_kawin_frek_istr.EditValue = dataTable.Rows[0]["kawin_frek_istr"].ToString();
                txt_nama_suami.EditValue = dataTable.Rows[0]["nama_suami"].ToString();
                txt_umur_suami.EditValue = dataTable.Rows[0]["umur_suami"].ToString();
                txt_agama_suami.EditValue = dataTable.Rows[0]["agama_suami"].ToString();
                txt_pendidikan_suami.EditValue = dataTable.Rows[0]["pendidikan_suami"].ToString();
                txt_pekerjaan_suami.EditValue = dataTable.Rows[0]["pekerjaan_suami"].ToString();
                txt_suku_suami.EditValue = dataTable.Rows[0]["suku_suami"].ToString();
                txt_kawin_lama_suami.EditValue = dataTable.Rows[0]["kawin_lama_suami"].ToString();
                txt_kawin_frek_suami.EditValue = dataTable.Rows[0]["kawin_frek_suami"].ToString();
                //txt_biodata_alamat.Text = dataTable.Rows[0]["biodata_alamat"].ToString();
                //txt_biodata_keluhan.Text = dataTable.Rows[0]["biodata_keluhan"].ToString();
                txt_rwyt_hamil_a.EditValue = dataTable.Rows[0]["rwyt_hamil_a"].ToString();
                date_rwyt_hamil_hpht_fr.EditValue = dataTable.Rows[0]["rwyt_hamil_hpht_fr"].ToString();
                date_rwyt_hamil_hpht_to.EditValue = dataTable.Rows[0]["rwyt_hamil_hpht_to"].ToString();
                date_rwyt_hamil_anc.EditValue = dataTable.Rows[0]["rwyt_hamil_anc"].ToString();
                txt_rwyt_hamil_komp.EditValue = dataTable.Rows[0]["rwyt_hamil_komp"].ToString();
                txt_rwyt_kb_kontra.EditValue = dataTable.Rows[0]["rwyt_kb_kontra"].ToString();
                txt_rwyt_kb_lama.EditValue = dataTable.Rows[0]["rwyt_kb_lama"].ToString();
                txt_rwyt_kb_alasan_henti.EditValue = dataTable.Rows[0]["rwyt_kb_alasan_henti"].ToString();
                txt_rwyt_penyakit_ibu.EditValue = dataTable.Rows[0]["rwyt_penyakit_ibu"].ToString();
                txt_aktiv_nutrisi.EditValue = dataTable.Rows[0]["aktiv_nutrisi"].ToString();
                txt_aktiv_eliminasi.EditValue = dataTable.Rows[0]["aktiv_eliminasi"].ToString();
                txt_aktiv_istirahat.EditValue = dataTable.Rows[0]["aktiv_istirahat"].ToString();
                txt_k_umum.EditValue = dataTable.Rows[0]["k_umum"].ToString();
                txt_k_sadar.EditValue = dataTable.Rows[0]["k_sadar"].ToString();

                //string stxt_umum_vita_td = dataTable.Rows[0]["umum_vita_td"].ToString();
                //string[] Split_stxt_umum_vita_td = stxt_umum_vita_td.ToString().Split(new string[] { "::" }, StringSplitOptions.None);

                //if (Split_stxt_umum_vita_td.Length >= 2)
                //{
                //    txt_umum_vita_td.EditValue = Split_stxt_umum_vita_td[0];
                //    txt_umum_vita_td_1.EditValue = Split_stxt_umum_vita_td[1];
                //}

                functionSplitIndex_1(dataTable.Rows[0]["BLOOD_PRESS"].ToString(), txt_umum_vita_td);
                functionSplitIndex_1(dataTable.Rows[0]["PULSE"].ToString(), txt_umum_vita_td_1);
                functionSplitIndex_1(dataTable.Rows[0]["TEMPERATURE"].ToString(), txt_umum_vital_s);

                txt_umum_vital_n.EditValue = dataTable.Rows[0]["umum_vital_n"].ToString();
                txt_umum_vital_r.EditValue = dataTable.Rows[0]["umum_vital_r"].ToString();
                //txt_umum_vital_s.EditValue = dataTable.Rows[0]["umum_vital_s"].ToString();
                txt_ekstermitas_atas.EditValue = dataTable.Rows[0]["ekstermitas_atas"].ToString();
                txt_ekstermitas_bawah.EditValue = dataTable.Rows[0]["ekstermitas_bawah"].ToString();
                txt_asesment_awal.EditValue = dataTable.Rows[0]["asesment_awal"].ToString();
                txt_planning_awal.EditValue = dataTable.Rows[0]["planning_awal"].ToString();
                txt_gen_inpeksi.EditValue = dataTable.Rows[0]["gen_inpeksi"].ToString();
                txt_gen_vulva.EditValue = dataTable.Rows[0]["gen_vulva"].ToString();
                txt_gen_portio.EditValue = dataTable.Rows[0]["gen_portio"].ToString();
                txt_gen_pembukaan.EditValue = dataTable.Rows[0]["gen_pembukaan"].ToString();
                txt_gen_penurunan.EditValue = dataTable.Rows[0]["gen_penurunan"].ToString();
                txt_gen_ketuban.EditValue = dataTable.Rows[0]["gen_ketuban"].ToString();
                txt_gen_presntasi.EditValue = dataTable.Rows[0]["gen_presntasi"].ToString();

            }


            string querySelect1 = "SELECT * FROM T2_R_INAP_BIDAN_1 where anamesa_id = " + id + "";
            DataTable dataTable1 = koneksi.GetDataTable(querySelect1);

            if (dataTable1.Rows.Count > 0)
            {
                txt_fisik_mata.EditValue = dataTable1.Rows[0]["fisik_mata"].ToString();
                txt_fisik_leher.EditValue = dataTable1.Rows[0]["fisik_leher"].ToString();
                txt_fisik_payudara.EditValue = dataTable1.Rows[0]["fisik_payudara"].ToString();
                txt_fisik_inpeksi.EditValue = dataTable1.Rows[0]["fisik_inpeksi"].ToString();
                txt_fisik_leopold_i.EditValue = dataTable1.Rows[0]["fisik_leopold_i"].ToString();
                txt_fisik_leopold_ii.EditValue = dataTable1.Rows[0]["fisik_leopold_ii"].ToString();
                txt_fisik_leopold_iii.EditValue = dataTable1.Rows[0]["fisik_leopold_iii"].ToString();
                txt_fisik_leopold_iv.EditValue = dataTable1.Rows[0]["fisik_leopold_iv"].ToString();
                txt_fisik_tfu.EditValue = dataTable1.Rows[0]["fisik_tfu"].ToString();
                txt_fisik_djj.EditValue = dataTable1.Rows[0]["fisik_djj"].ToString();
                txt_fisik_his.EditValue = dataTable1.Rows[0]["fisik_his"].ToString();
                txt_penunjang_i.EditValue = dataTable1.Rows[0]["penunjang_i"].ToString();
                txt_penunjang_iii.EditValue = dataTable1.Rows[0]["penunjang_iii"].ToString();
                txt_protein_urine.EditValue = dataTable1.Rows[0]["protein_urine"].ToString();
                txt_glukosa_urine.EditValue = dataTable1.Rows[0]["glukosa_urine"].ToString();
                date_tgl_persalinan.EditValue = dataTable1.Rows[0]["tgl_persalinan"].ToString();
                txt_nama_bidan.EditValue = dataTable1.Rows[0]["nama_bidan"].ToString();


                functionSplitIndex_3((dataTable1.Rows[0]["tempat_persalinan"].ToString()), rb_tempat_persalinan, txt_tempat_persalinan);

                txt_alamat_persalinan.EditValue = dataTable1.Rows[0]["alamat_persalinan"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["rujuk_kala"].ToString(), rb_rujuk_kala);

                txt_alasan_rujuk.EditValue = dataTable1.Rows[0]["alasan_rujuk"].ToString();
                txt_tempat_rujuk.EditValue = dataTable1.Rows[0]["tempat_rujuk"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["pendamping_rujuk"].ToString(), rb_pendamping_rujuk);

                functionSplitIndex_2(dataTable1.Rows[0]["kala_i_a"].ToString(), rb_kala_i_a);

                txt_kala_i_b.EditValue = dataTable1.Rows[0]["kala_i_b"].ToString();
                txt_kala_i_c.EditValue = dataTable1.Rows[0]["kala_i_c"].ToString();
                txt_kala_i_d.EditValue = dataTable1.Rows[0]["kala_i_d"].ToString();


                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_a"].ToString(), rb_kala_ii_a, txt_kala_ii_a);
                functionSplitIndex_2(dataTable1.Rows[0]["kala_ii_b"].ToString(), rb_kala_ii_b);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_c"].ToString(), rb_kala_ii_c, txt_kala_ii_c);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_ii_d"].ToString(), rb_kala_ii_d, txt_kala_ii_d);

                txt_kala_ii_e.EditValue = dataTable1.Rows[0]["kala_ii_e"].ToString();
                txt_kala_ii_f.EditValue = dataTable1.Rows[0]["kala_ii_f"].ToString();
                txt_kala_ii_g.EditValue = dataTable1.Rows[0]["kala_ii_g"].ToString();
                txt_kala_iii_a.EditValue = dataTable1.Rows[0]["kala_iii_a"].ToString();

                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_b"].ToString(), rb_kala_iii_b, txt_kala_iii_b);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_c"].ToString(), rb_kala_iii_c, txt_kala_iii_c);
                functionSplitIndex_3(dataTable1.Rows[0]["kala_iii_d"].ToString(), rb_kala_iii_d, txt_kala_iii_d);

                txt_baru_lahir_berat.EditValue = dataTable1.Rows[0]["baru_lahir_berat"].ToString();
                txt_baru_lahir_panjang.EditValue = dataTable1.Rows[0]["baru_lahir_panjang"].ToString();

                functionSplitIndex_2(dataTable1.Rows[0]["baru_lahir_jk"].ToString(), rb_baru_lahir_jk);
                functionSplitIndex_2(dataTable1.Rows[0]["baru_lahir_nilai"].ToString(), rb_baru_lahir_nilai);

                chk_baru_lahir_ket.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "Normal");
                chk_baru_lahir_ket_1.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "mengeringkan");
                chk_baru_lahir_ket_2.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "menghangatkan");
                chk_baru_lahir_ket_3.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "bungkus bayi dan tempatkan di sisi ibu");
                chk_baru_lahir_ket_4.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "rangsangan taktil");
                chk_baru_lahir_ket_5.Checked = functionChk(dataTable1.Rows[0]["baru_lahir_ket"].ToString(), "tindakan pencegahan infeksi mata");

            }

            string querySelect2 = "SELECT * FROM T2_R_INAP_BIDAN_2 where anamesa_id =  " + id + "";
            DataTable dataTable2 = koneksi.GetDataTable(querySelect2);

            if (dataTable2.Rows.Count > 0)
            {
                functionSplitIndex_3(dataTable2.Rows[0]["rangsang_laktil"].ToString(), rb_rangsang_laktil, txt_rangsang_laktil);
                functionSplitIndex_3(dataTable2.Rows[0]["plasenta_intack"].ToString(), rb_plasenta_intack, txt_plasenta_intack);
                functionSplitIndex_3(dataTable2.Rows[0]["plasenta_tidak_lahir"].ToString(), rb_plasenta_tidak_lahir, txt_plasenta_tidak_lahir);
                functionSplitIndex_3(dataTable2.Rows[0]["laserasi"].ToString(), rb_laserasi, txt_laserasi);

                functionSplitIndex_5(dataTable2.Rows[0]["laserasi_parinium"].ToString(), rb_laserasi_parinium, rb_laserasi_parinium_tindakan, txt_laserasi_parinium);

                functionSplitIndex_3(dataTable2.Rows[0]["atonia_uteri"].ToString(), rb_atonia_uteri, txt_atonia_uteri);

                txt_jumlah_pendarahan.EditValue = dataTable2.Rows[0]["jumlah_pendarahan"].ToString();
                txt_masalah_lain.EditValue = dataTable2.Rows[0]["masalah_lain"].ToString();
                txt_penata_masalah.EditValue = dataTable2.Rows[0]["penata_masalah"].ToString();
                txt_hasilnya.EditValue = dataTable2.Rows[0]["hasilnya"].ToString();



                functionSplitIndex_10(dataTable2.Rows[0]["baru_lahir"].ToString(), rb_baru_lahir, txt_cacat_ket_lain);

                chk_cacat_ket_1.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "mengeringkan");
                chk_cacat_ket_2.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "menghangatkan");
                chk_cacat_ket_3.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "rangsangan taktil");
                chk_cacat_ket_4.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "bungkus bayi dan tempatkan di sisi ibu");
                chk_cacat_ket_5.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "tindakan pencegahan infeksi mata");
                chk_cacat_ket_6.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "bebaskan jalan napas");
                chk_cacat_ket_lain.Checked = functionChk(dataTable2.Rows[0]["baru_lahir"].ToString(), "lain-lain");


                txt_bayi_cacat.EditValue = dataTable2.Rows[0]["bayi_cacat"].ToString();
                txt_hipotermia.EditValue = dataTable2.Rows[0]["hipotermia"].ToString();


                functionSplitIndex_4_asi(dataTable2.Rows[0]["pemberian_asi"].ToString(), rb_pemberian_asi, txt_pemberian_asi_ya, txt_pemberian_asi_tdk);

                txt_masalah_lahir.EditValue = dataTable2.Rows[0]["masalah_lahir"].ToString();
                txt_hasil_lahir.EditValue = dataTable2.Rows[0]["hasil_lahir"].ToString(); 
            }
            

            string querySkala1 = "SELECT * FROM T2_DOKUMEN_SKALA_I where anamesa_id =  " + id + "";
            DataTable dt_skala1 = koneksi.GetDataTable(querySkala1);

            if (dt_skala1.Rows.Count > 0)
            {
                txt_skl1_tanggal.EditValue = dt_skala1.Rows[0]["tanggal"].ToString();
                txt_skl1_jam.EditValue = dt_skala1.Rows[0]["jam"].ToString();
                txt_skl1_keluhan_utama.EditValue = dt_skala1.Rows[0]["keluhan_utama"].ToString();
                txt_skl1_kesadaran.EditValue = dt_skala1.Rows[0]["kesadaran"].ToString();
                txt_skl1_td_1.EditValue = dt_skala1.Rows[0]["td_1"].ToString();
                txt_skl1_td_2.EditValue = dt_skala1.Rows[0]["td_2"].ToString();
                txt_skl1_n.EditValue = dt_skala1.Rows[0]["n"].ToString();
                txt_skl1_r.EditValue = dt_skala1.Rows[0]["r"].ToString();
                txt_skl1_s.EditValue = dt_skala1.Rows[0]["s"].ToString();
                txt_skl1_frekuensi.EditValue = dt_skala1.Rows[0]["frekuensi"].ToString();
                txt_skl1_interfal.EditValue = dt_skala1.Rows[0]["interfal"].ToString();
                txt_skl1_durasi.EditValue = dt_skala1.Rows[0]["durasi"].ToString();

                //txt_skl1_djj.EditValue              = dt_skala1.Rows[0]["djj"].ToString();

                functionSplitIndex_3(dt_skala1.Rows[0]["djj"].ToString(), rb_skl1_djj, txt_skl1_djj);

                txt_skl1_diagnosa.EditValue = dt_skala1.Rows[0]["diagnosa"].ToString();
                txt_skl1_masalah_potensial.EditValue = dt_skala1.Rows[0]["masalah_potensial"].ToString();
                txt_skl1_antisipasi_masalah.EditValue = dt_skala1.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl1_vulva.EditValue = dt_skala1.Rows[0]["vulva"].ToString();
                txt_skl1_pembukaan.EditValue = dt_skala1.Rows[0]["pembukaan"].ToString();
                txt_skl1_keadaan_ketuban.EditValue = dt_skala1.Rows[0]["keadaan_ketuban"].ToString();
                txt_skl1_presentasi.EditValue = dt_skala1.Rows[0]["presentasi"].ToString();
                txt_skl1_bagian_teraba.EditValue = dt_skala1.Rows[0]["bagian_teraba"].ToString();
                txt_skl1_turunnya_bagian.EditValue = dt_skala1.Rows[0]["turunnya_bagian"].ToString();
                txt_skl1_molage.EditValue = dt_skala1.Rows[0]["molage"].ToString();

                functionSplitIndex_2(dt_skala1.Rows[0]["vesica_urineria"].ToString(), rb_skl1_vesica_urineria);

                //rb_skl1_vesica_urineria.EditValue       = dt_skala1.Rows[0]["vesica_urineria"].ToString();

                txt_skl1_planning.EditValue = dt_skala1.Rows[0]["planning"].ToString();
                txt_skl1_jam_plan.EditValue = dt_skala1.Rows[0]["jam_plan"].ToString();
                txt_skl1_tgl_plan.EditValue = dt_skala1.Rows[0]["tgl_plan"].ToString();
                txt_skl1_dipimpin.EditValue = dt_skala1.Rows[0]["dipimpin"].ToString();
                txt_skl1_jam_mulai.EditValue = dt_skala1.Rows[0]["jam_mulai"].ToString();


            }


            string querySkala2 = "SELECT * FROM T2_DOKUMEN_SKALA_II where anamesa_id =  " + id + "";
            DataTable dt_skala2 = koneksi.GetDataTable(querySkala2);

            if (dt_skala2.Rows.Count > 0)
            {
                txt_skl2_tanggal.EditValue = dt_skala2.Rows[0]["tanggal"].ToString();
                txt_skl2_jam.EditValue = dt_skala2.Rows[0]["jam"].ToString();
                txt_skl2_keluhan_utama.EditValue = dt_skala2.Rows[0]["keluhan_utama"].ToString();
                txt_skl2_kesadaran.EditValue = dt_skala2.Rows[0]["kesadaran"].ToString();
                txt_skl2_td_1.EditValue = dt_skala2.Rows[0]["td_1"].ToString();
                txt_skl2_td_2.EditValue = dt_skala2.Rows[0]["td_2"].ToString();
                txt_skl2_n.EditValue = dt_skala2.Rows[0]["n"].ToString();
                txt_skl2_r.EditValue = dt_skala2.Rows[0]["r"].ToString();
                txt_skl2_s.EditValue = dt_skala2.Rows[0]["s"].ToString();
                txt_skl2_frekuensi.EditValue = dt_skala2.Rows[0]["frekuensi"].ToString();
                txt_skl2_interfal.EditValue = dt_skala2.Rows[0]["interfal"].ToString();
                txt_skl2_durasi.EditValue = dt_skala2.Rows[0]["durasi"].ToString();

                functionSplitIndex_3(dt_skala2.Rows[0]["djj"].ToString(), rb_skl2_djj, txt_skl2_djj);
                //txt_skl2_djj.EditValue              = dt_skala2.Rows[0]["djj"].ToString();

                txt_skl2_diagnosa.EditValue = dt_skala2.Rows[0]["diagnosa"].ToString();
                txt_skl2_masalah_potensial.EditValue = dt_skala2.Rows[0]["masalah_potensial"].ToString();
                txt_skl2_antisipasi_masalah.EditValue = dt_skala2.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl2_vulva.EditValue = dt_skala2.Rows[0]["vulva"].ToString();
                txt_skl2_pembukaan.EditValue = dt_skala2.Rows[0]["pembukaan"].ToString();
                txt_skl2_keadaan_ketuban.EditValue = dt_skala2.Rows[0]["keadaan_ketuban"].ToString();
                txt_skl2_presentasi.EditValue = dt_skala2.Rows[0]["presentasi"].ToString();
                txt_skl2_bagian_teraba.EditValue = dt_skala2.Rows[0]["bagian_teraba"].ToString();
                txt_skl2_turunnya_bagian.EditValue = dt_skala2.Rows[0]["turunnya_bagian"].ToString();
                txt_skl2_molage.EditValue = dt_skala2.Rows[0]["molage"].ToString();

                functionSplitIndex_2(dt_skala2.Rows[0]["vesica_urineria"].ToString(), rb_skl2_vesica_urineria);

                //rb_skl2_vesica_urineria.EditValue   = dt_skala2.Rows[0]["vesica_urineria"].ToString();

                txt_skl2_planning.EditValue = dt_skala2.Rows[0]["planning"].ToString();
                txt_skl2_lahir_jam.EditValue = dt_skala2.Rows[0]["lahir_jam"].ToString();
                txt_skl2_jk.EditValue = dt_skala2.Rows[0]["jk"].ToString();
                txt_skl2_bb.EditValue = dt_skala2.Rows[0]["bb"].ToString();
                txt_skl2_lk.EditValue = dt_skala2.Rows[0]["lk"].ToString();
                txt_skl2_ld.EditValue = dt_skala2.Rows[0]["ld"].ToString();
                txt_skl2_kadaan_lahir.EditValue = dt_skala2.Rows[0]["kadaan_lahir"].ToString();
                txt_skl2_evaluasi.EditValue = dt_skala2.Rows[0]["evaluasi"].ToString();



            }

            string querySkala3 = "SELECT * FROM T2_DOKUMEN_SKALA_III where anamesa_id =  " + id + "";
            DataTable dt_skala3 = koneksi.GetDataTable(querySkala3);

            if (dt_skala3.Rows.Count > 0)
            {
                txt_skl3_tanggal.EditValue = dt_skala3.Rows[0]["tanggal"].ToString();
                txt_skl3_jam.EditValue = dt_skala3.Rows[0]["jam"].ToString();
                txt_skl3_keluhan_utama.EditValue = dt_skala3.Rows[0]["keluhan_utama"].ToString();
                txt_skl3_kesadaran.EditValue = dt_skala3.Rows[0]["kesadaran"].ToString();
                txt_skl3_td_1.EditValue = dt_skala3.Rows[0]["td_1"].ToString();
                txt_skl3_td_2.EditValue = dt_skala3.Rows[0]["td_2"].ToString();
                txt_skl3_n.EditValue = dt_skala3.Rows[0]["n"].ToString();
                txt_skl3_r.EditValue = dt_skala3.Rows[0]["r"].ToString();
                txt_skl3_s.EditValue = dt_skala3.Rows[0]["s"].ToString();
                txt_skl3_palpasi_abdomen.EditValue = dt_skala3.Rows[0]["palpasi_abdomen"].ToString();

                //cb_skl3_kontraksi_uterus_1.EditValue    = dt_skala3.Rows[0]["kontraksi_uterus"].ToString();

                cb_skl3_kontraksi_uterus_1.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "ada");
                cb_skl3_kontraksi_uterus_2.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "tidak dan lemah");
                cb_skl3_kontraksi_uterus_3.Checked = functionChk(dt_skala3.Rows[0]["kontraksi_uterus"].ToString(), "ade kuat");

                //rb_skl3_uterus_membulat.EditValue   = dt_skala3.Rows[0]["uterus_membulat"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["uterus_membulat"].ToString(), rb_skl3_uterus_membulat);

                txt_skl3_tinggi_fundus.EditValue = dt_skala3.Rows[0]["tinggi_fundus"].ToString();

                //rb_skl3_semburan_darah.EditValue    = dt_skala3.Rows[0]["semburan_darah"].ToString();
                //rb_skl3_vesica_urineria.EditValue   = dt_skala3.Rows[0]["vesica_urineria"].ToString();

                functionSplitIndex_2(dt_skala3.Rows[0]["semburan_darah"].ToString(), rb_skl3_semburan_darah);
                functionSplitIndex_2(dt_skala3.Rows[0]["vesica_urineria"].ToString(), rb_skl3_vesica_urineria);

                txt_skl3_diagnosa.EditValue = dt_skala3.Rows[0]["diagnosa"].ToString();
                txt_skl3_masalah_potensial.EditValue = dt_skala3.Rows[0]["masalah_potensial"].ToString();
                txt_skl3_antisipasi_masalah.EditValue = dt_skala3.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl3_planning.EditValue = dt_skala3.Rows[0]["planning"].ToString();
                txt_skl3_placenta_lahir.EditValue = dt_skala3.Rows[0]["placenta_lahir"].ToString();

                //rb_skl3_spontan.EditValue           = dt_skala3.Rows[0]["spontan"].ToString();
                //rb_skl3_lengkap.EditValue           = dt_skala3.Rows[0]["lengkap"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["spontan"].ToString(), rb_skl3_spontan);
                functionSplitIndex_2(dt_skala3.Rows[0]["lengkap"].ToString(), rb_skl3_lengkap);

                txt_skl3_kontraksi.EditValue = dt_skala3.Rows[0]["kontraksi"].ToString();
                txt_skl3_pendarahan.EditValue = dt_skala3.Rows[0]["pendarahan"].ToString();
                txt_skl3_keadaan_jalan.EditValue = dt_skala3.Rows[0]["keadaan_jalan"].ToString();
                //rb_skl3_bila_reptura.EditValue      = dt_skala3.Rows[0]["bila_reptura"].ToString();
                functionSplitIndex_2(dt_skala3.Rows[0]["bila_reptura"].ToString(), rb_skl3_bila_reptura); 
            }


            string querySkala4 = "SELECT * FROM T2_DOKUMEN_SKALA_IV where anamesa_id =  " + id + "";
            DataTable dt_skala4 = koneksi.GetDataTable(querySkala4);

            if (dt_skala4.Rows.Count > 0)
            {
                txt_skl4_tanggal.EditValue = dt_skala4.Rows[0]["tanggal"].ToString();
                txt_skl4_jam.EditValue = dt_skala4.Rows[0]["jam"].ToString();
                txt_skl4_keluhan_utama.EditValue = dt_skala4.Rows[0]["keluhan_utama"].ToString();
                txt_skl4_kesadaran.EditValue = dt_skala4.Rows[0]["kesadaran"].ToString();
                txt_skl4_td_1.EditValue = dt_skala4.Rows[0]["td_1"].ToString();
                txt_skl4_td_2.EditValue = dt_skala4.Rows[0]["td_2"].ToString();
                txt_skl4_n.EditValue = dt_skala4.Rows[0]["n"].ToString();
                txt_skl4_r.EditValue = dt_skala4.Rows[0]["r"].ToString();
                txt_skl4_s.EditValue = dt_skala4.Rows[0]["s"].ToString();

                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "Ada");
                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "tidak kuat");
                cb_skl4_kontraksi_uterus_1.Checked = functionChk(dt_skala4.Rows[0]["kontraksi_uterus"].ToString(), "kuat");

                txt_skl4_tinggi_fundus.EditValue = dt_skala4.Rows[0]["tinggi_fundus"].ToString();

                functionSplitIndex_2(dt_skala3.Rows[0]["vesica_urineria"].ToString(), rb_skl4_vesica_urineria);

                txt_skl4_jumlah_darah.EditValue = dt_skala4.Rows[0]["jumlah_darah"].ToString();
                txt_skl4_diagnosa.EditValue = dt_skala4.Rows[0]["diagnosa"].ToString();
                txt_skl4_masalah_potensial.EditValue = dt_skala4.Rows[0]["masalah_potensial"].ToString();
                txt_skl4_antisipasi_masalah.EditValue = dt_skala4.Rows[0]["antisipasi_masalah"].ToString();
                txt_skl4_planning.EditValue = dt_skala4.Rows[0]["planning"].ToString(); 
            }

            loadDataGrid(id);

            LoadDataResep();

            string SQL
                      = "select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                          "       b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status ,a.ID_VISIT " +
                          "  from KLINIK.cs_treatment_head a  " +
                          "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                          "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  " +
                          " where ID_VISIT = '" + visitid + "'   and b.ID_DOKTER is  null  AND B.F_ACTIVE ='Y' ";
            //"   and a.status = 'OPN'  ";

            dtMedis = ConnOra.Data_Table_ora(SQL);
            gcPelayanBidan.DataSource = dtMedis;

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya2;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gvPelayanBidan.Columns[3].ColumnEdit = glLaya;

        
            string SQL2 = " Select ROWNUM SEQ, b.detail_id, c.treat_group_id, b.treat_item_id, c.TREAT_ITEM_NAME, b.treat_qty, b.treat_item_price,  " +
                          "        b.remarks, 'S' action, a.head_id, b.treat_date  TANGGAL, TREAT_JAM JAM, a.pay_status, a.ID_VISIT, b.ID_DOKTER " +
                          "  from KLINIK.cs_treatment_head a  " +
                          "  join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id)  " +
                          "  join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id)  " +
                          "  join KLINIK.CS_DOKTER d on (b.ID_DOKTER = d.ID_DOKTER)  " +
                          " where ID_VISIT = '" + visitid + "'  and b.ID_DOKTER is not null AND B.F_ACTIVE ='Y' ";
            //"   and a.status = 'OPN'  ";

            dtVisitDokter = ConnOra.Data_Table_ora(SQL2); //ORADB.SetData(ORADB.XE, SQL2);
            gcVisitBidan.DataSource = dtVisitDokter;

            RepositoryItemGridLookUpEdit glvisit = new RepositoryItemGridLookUpEdit();
            glvisit.DataSource = listDokter;
            glvisit.ValueMember = "ID_Dokter";
            glvisit.DisplayMember = "Nama_Dokter";

            glvisit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glvisit.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glvisit.ImmediatePopup = true;
            glvisit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glvisit.NullText = "";
            gvVisitBidan.Columns[4].ColumnEdit = glvisit;

            RepositoryItemGridLookUpEdit glLayav = new RepositoryItemGridLookUpEdit();
            glLayav.DataSource = listLayav;
            glLayav.ValueMember = "layananCode";
            glLayav.DisplayMember = "layananName";

            glLayav.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLayav.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLayav.ImmediatePopup = true;
            glLayav.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLayav.NullText = "";
            gvVisitBidan.Columns[3].ColumnEdit = glLayav;

            //RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            //rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
            //rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //gvPelayanBidan.Columns["TANGGAL"].ColumnEdit = rptanggal;
            //gvVisitBidan.Columns["TANGGAL"].ColumnEdit = rptanggal; 
             
        }
        private void loadDataGrid(string idanamesa)
        {

            string querySelect5 = "SELECT * FROM T2_RIWAYAT_PERSALINAN_LALU  where anamesa_id = " + idanamesa + "";
            dt_grdPersalinanLalu = koneksi.GetDataTable(querySelect5);
            ConvertColumnNamesToUppercase(dt_grdPersalinanLalu);
            grdPersalinanLalu.DataSource = dt_grdPersalinanLalu;

            
            string querySelect10 = "SELECT * FROM T2_PMT_PERSALINAN_IV  where anamesa_id = " + idanamesa + "";
            dt_grdPemantauanIv = koneksi.GetDataTable(querySelect10);

            ConvertColumnNamesToUppercase(dt_grdPemantauanIv);
            grdPemantauanIv.DataSource = dt_grdPemantauanIv;

            if (dt_grdPemantauanIv.Rows.Count > 0)
            {
                txt_MasalahKala4.EditValue = dt_grdPemantauanIv.Rows[0]["MASALAH_KALA_IV"].ToString();
                txt_PenatalaksanaKala4.EditValue = dt_grdPemantauanIv.Rows[0]["PENATALAKSANAAN_KALA_IV"].ToString();
            }
        }

        private void btnAddPersalinanIV_Click(object sender, EventArgs e)
        {
            if (dt_grdPemantauanIv == null) return;

            DataRow newRow = dt_grdPemantauanIv.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemantauanIv.RowCount) + 1).ToString();
            dt_grdPemantauanIv.Rows.Add(newRow);

            grdPemantauanIv.DataSource = dt_grdPemantauanIv;
        }

        private void btnSavePersalinanIV_Click(object sender, EventArgs e)
        {

            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPemantauanIv.Rows)
                {


                    string query = @"select count(*) from T2_PMT_PERSALINAN_IV where anamesa_id = '" + _AnamesaID + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_PMT_PERSALINAN_IV set
                                                                urutan_ke           = '" + row["URUTAN_KE"] + @"',
                                                                jam_ke              = '" + row["JAM_KE"] + @"',
                                                                waktu               = '" + row["WAKTU"] + @"',
                                                                tekanan_darah       = '" + row["TEKANAN_DARAH"] + @"',
                                                                nadi                = '" + row["NADI"] + @"',
                                                                temperatur          = '" + row["TEMPERATUR"] + @"',
                                                                tinggi_fundus       = '" + row["TINGGI_FUNDUS"] + @"',
                                                                kontraksi_uterus    = '" + row["KONTRAKSI_UTERUS"] + @"',
                                                                kandung_kemih       = '" + row["KANDUNG_KEMIH"] + @"',
                                                                pendarahan          = '" + row["PENDARAHAN"] + @"',
                                                                masalah_kala_iv     = '" + txt_MasalahKala4.Text + @"',
                                                                penatalaksanaan_kala_iv = '" + txt_PenatalaksanaKala4.Text + @"'
                                                      where anamesa_id = '" + _AnamesaID + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_PMT_PERSALINAN_IV(
                                                                    id,
                                                                    anamesa_id,
                                                                    urutan_ke,
                                                                    jam_ke,
                                                                    waktu,
                                                                    tekanan_darah,
                                                                    nadi,
                                                                    temperatur,
                                                                    tinggi_fundus,
                                                                    kontraksi_uterus,
                                                                    kandung_kemih,
                                                                    pendarahan,
                                                                    masalah_kala_iv,
                                                                    penatalaksanaan_kala_iv) values ( 
                                                                    pemantauan_persalinan_iv_seq.nextval,
                                                                    '" + _AnamesaID + @"',
                                                                    '" + row["URUTAN_KE"] + @"',
                                                                    '" + row["JAM_KE"] + @"',
                                                                    '" + row["WAKTU"] + @"',
                                                                    '" + row["TEKANAN_DARAH"] + @"',
                                                                    '" + row["NADI"] + @"',
                                                                    '" + row["TEMPERATUR"] + @"',
                                                                    '" + row["TINGGI_FUNDUS"] + @"',
                                                                    '" + row["KONTRAKSI_UTERUS"] + @"',
                                                                    '" + row["KANDUNG_KEMIH"] + @"',
                                                                    '" + row["PENDARAHAN"] + @"',
                                                                    '" + txt_MasalahKala4.Text + @"',
                                                                    '" + txt_PenatalaksanaKala4.Text + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }
        }
        
        private void btnAddPersalinanLalu_Click(object sender, EventArgs e)
        {


            if (dt_grdPersalinanLalu == null) return;

            DataRow newRow = dt_grdPersalinanLalu.NewRow();
            newRow["HAMIL_KE"] = ((gvwPersalinanLalu.RowCount) + 1).ToString();

            dt_grdPersalinanLalu.Rows.Add(newRow);

            grdPersalinanLalu.DataSource = dt_grdPersalinanLalu;
        }
        private void btnSavePersalinanLalu_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPersalinanLalu.Rows)
                {


                    string query = @"select count(*) from T2_RIWAYAT_PERSALINAN_LALU where anamesa_id = '" + _AnamesaID + "' and hamil_ke = '" + row["HAMIL_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {


                        string queryInsert = @"update  T2_RIWAYAT_PERSALINAN_LALU set
                                                            hamil_ke            = '" + row["HAMIL_KE"] + @"',
                                                            umur_kehamilan      = '" + row["UMUR_KEHAMILAN"] + @"',
                                                            tahun_persalinan    = '" + row["TAHUN_PERSALINAN"] + @"',
                                                            penolong            = '" + row["PENOLONG"] + @"',
                                                            cara_persalinan     = '" + row["CARA_PERSALINAN"] + @"',
                                                            riwayat_komplikasi  = '" + row["RIWAYAT_KOMPLIKASI"] + @"',
                                                            tmpt_persalinan     = '" + row["TMPT_PERSALINAN"] + @"',
                                                            jk                  = '" + row["JK"] + @"',
                                                            bb                  = '" + row["BB"] + @"',
                                                            pb                  = '" + row["PB"] + @"',
                                                            bayi_h              = '" + row["BAYI_H"] + @"',
                                                            bayi_m              = '" + row["BAYI_M"] + @"'
                                                      where anamesa_id = '" + _AnamesaID + "' and hamil_ke = '" + row["HAMIL_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_RIWAYAT_PERSALINAN_LALU(
                                                            id,
                                                            anamesa_id,
                                                            hamil_ke,
                                                            umur_kehamilan,
                                                            tahun_persalinan,
                                                            penolong,
                                                            cara_persalinan,
                                                            riwayat_komplikasi,
                                                            tmpt_persalinan,
                                                            jk,
                                                            bb,
                                                            pb,
                                                            bayi_h,
                                                            bayi_m) 
                                                    values (
                                                            riwayat_persalinan_lalu_seq.nextval,
                                                            '" + _AnamesaID + @"',
                                                            '" + row["HAMIL_KE"] + @"',
                                                            '" + row["UMUR_KEHAMILAN"] + @"',
                                                            '" + row["TAHUN_PERSALINAN"] + @"',
                                                            '" + row["PENOLONG"] + @"',
                                                            '" + row["CARA_PERSALINAN"] + @"',
                                                            '" + row["RIWAYAT_KOMPLIKASI"] + @"',
                                                            '" + row["TMPT_PERSALINAN"] + @"',
                                                            '" + row["JK"] + @"',
                                                            '" + row["BB"] + @"',
                                                            '" + row["PB"] + @"',
                                                            '" + row["BAYI_H"] + @"',
                                                            '" + row["BAYI_M"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }
        }



        private void updateTable()
        {
            try
            {

                List<string> updateQueries = new List<string>
                {

                };

                updateQueries.Add(@" update T2_R_INAP_BIDAN set 
                                            nama_istr               = '" + txt_nama_istr.Text + @"',
                                            umur_istr               = '" + txt_umur_istr.Text + @"',
                                            agama_istr              = '" + txt_agama_istr.Text + @"',
                                            pendidikan_istr         = '" + txt_pendidikan_istr.Text + @"',
                                            pekerjaan_istr          = '" + txt_pekerjaan_istr.Text + @"',
                                            suku_istr               = '" + txt_suku_istr.Text + @"',
                                            kawin_lama_istr         = '" + txt_kawin_lama_istr.Text + @"',
                                            kawin_frek_istr         = '" + txt_kawin_frek_istr.Text + @"',
                                            nama_suami              = '" + txt_nama_suami.Text + @"',
                                            umur_suami              = '" + txt_umur_suami.Text + @"',
                                            agama_suami             = '" + txt_agama_suami.Text + @"',
                                            pendidikan_suami        = '" + txt_pendidikan_suami.Text + @"',
                                            pekerjaan_suami         = '" + txt_pekerjaan_suami.Text + @"',
                                            suku_suami              = '" + txt_suku_suami.Text + @"',
                                            kawin_lama_suami        = '" + txt_kawin_lama_suami.Text + @"',
                                            kawin_frek_suami        = '" + txt_kawin_frek_suami.Text + @"',
                                            biodata_alamat          = '" + txt_biodata_alamat.Text + @"',
                                            biodata_keluhan         = '" + txt_biodata_keluhan.Text + @"',
                                            rwyt_hamil_a            = '" + txt_rwyt_hamil_a.Text + @"',
                                            rwyt_hamil_hpht_fr      = '" + date_rwyt_hamil_hpht_fr.Text + @"',
                                            rwyt_hamil_hpht_to      = '" + date_rwyt_hamil_hpht_to.Text + @"',
                                            rwyt_hamil_anc          = '" + date_rwyt_hamil_anc.Text + @"',
                                            rwyt_hamil_komp         = '" + txt_rwyt_hamil_komp.Text + @"',
                                            rwyt_kb_kontra          = '" + txt_rwyt_kb_kontra.Text + @"',
                                            rwyt_kb_lama            = '" + txt_rwyt_kb_lama.Text + @"',
                                            rwyt_kb_alasan_henti    = '" + txt_rwyt_kb_alasan_henti.Text + @"',
                                            rwyt_penyakit_ibu       = '" + txt_rwyt_penyakit_ibu.Text + @"',
                                            aktiv_nutrisi           = '" + txt_aktiv_nutrisi.Text + @"',
                                            aktiv_eliminasi         = '" + txt_aktiv_eliminasi.Text + @"',
                                            aktiv_istirahat         = '" + txt_aktiv_istirahat.Text + @"',
                                            k_umum                  = '" + txt_k_umum.Text + @"',
                                            k_sadar                 = '" + txt_k_sadar.Text + @"',
                                            umum_vita_td            = '" + txt_umum_vita_td.Text + "::" + txt_umum_vita_td_1.Text + @"',    
                                            umum_vital_n            = '" + txt_umum_vital_n.Text + @"',
                                            umum_vital_r            = '" + txt_umum_vital_r.Text + @"',
                                            umum_vital_s            = '" + txt_umum_vital_s.Text + @"',
                                            ekstermitas_atas        = '" + txt_ekstermitas_atas.Text + @"',
                                            ekstermitas_bawah       = '" + txt_ekstermitas_bawah.Text + @"',
                                            asesment_awal           = '" + txt_asesment_awal.Text + @"',
                                            planning_awal           = '" + txt_planning_awal.Text + @"',
                                            gen_inpeksi             = '" + txt_gen_inpeksi.Text + @"',
                                            gen_vulva               = '" + txt_gen_vulva.Text + @"',
                                            gen_portio              = '" + txt_gen_portio.Text + @"',
                                            gen_pembukaan           = '" + txt_gen_pembukaan.Text + @"',
                                            gen_penurunan           = '" + txt_gen_penurunan.Text + @"',
                                            gen_ketuban             = '" + txt_gen_ketuban.Text + @"',
                                            gen_presntasi           = '" + txt_gen_presntasi.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            fisik_mata              = '" + txt_fisik_mata.Text + @"',
                                            fisik_leher             = '" + txt_fisik_leher.Text + @"',
                                            fisik_payudara          = '" + txt_fisik_payudara.Text + @"',
                                            fisik_inpeksi           = '" + txt_fisik_inpeksi.Text + @"',
                                            fisik_leopold_i         = '" + txt_fisik_leopold_i.Text + @"',
                                            fisik_leopold_ii        = '" + txt_fisik_leopold_ii.Text + @"',
                                            fisik_leopold_iii       = '" + txt_fisik_leopold_iii.Text + @"',
                                            fisik_leopold_iv        = '" + txt_fisik_leopold_iv.Text + @"',
                                            fisik_tfu               = '" + txt_fisik_tfu.Text + @"',
                                            fisik_djj               = '" + txt_fisik_djj.Text + @"',
                                            fisik_his               = '" + txt_fisik_his.Text + @"',
                                            penunjang_i             = '" + txt_penunjang_i.Text + @"',
                                            penunjang_iii           = '" + txt_penunjang_iii.Text + @"',
                                            protein_urine           = '" + txt_protein_urine.Text + @"',
                                            glukosa_urine           = '" + txt_glukosa_urine.Text + @"',
                                            tgl_persalinan          = '" + date_tgl_persalinan.Text + @"',
                                            nama_bidan              = '" + txt_nama_bidan.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            tempat_persalinan       = '" + rb_tempat_persalinan.SelectedIndex.ToString() + "::" + rb_tempat_persalinan.Text + "::" + txt_tempat_persalinan.Text + @"',
                                            alamat_persalinan       = '" + txt_alamat_persalinan.Text + @"',
                                            rujuk_kala              = '" + rb_rujuk_kala.SelectedIndex.ToString() + "::" + rb_rujuk_kala.Text + @"',
                                            alasan_rujuk            = '" + txt_alasan_rujuk.Text + @"',
                                            tempat_rujuk            = '" + txt_tempat_rujuk.Text + @"',
                                            pendamping_rujuk        = '" + rb_pendamping_rujuk.SelectedIndex.ToString() + "::" + rb_pendamping_rujuk.Text + @"',
                                            kala_i_a                = '" + rb_kala_i_a.SelectedIndex.ToString() + "::" + rb_kala_i_a.Text + @"',
                                            kala_i_b                = '" + txt_kala_i_b.Text + @"',
                                            kala_i_c                = '" + txt_kala_i_c.Text + @"',
                                            kala_i_d                = '" + txt_kala_i_d.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");


                updateQueries.Add(@" update T2_R_INAP_BIDAN_1 set 
                                            kala_ii_a               = '" + rb_kala_ii_a.SelectedIndex.ToString() + "::" + rb_kala_ii_a.Text + "::" + txt_kala_ii_a.Text + @"',
                                            kala_ii_b               = '" + rb_kala_ii_b.SelectedIndex.ToString() + "::" + rb_kala_ii_b.Text + @"',
                                            kala_ii_c               = '" + rb_kala_ii_c.SelectedIndex.ToString() + "::" + rb_kala_ii_c.Text + "::" + txt_kala_ii_c.Text + @"',
                                            kala_ii_d               = '" + rb_kala_ii_d.SelectedIndex.ToString() + "::" + rb_kala_ii_d.Text + "::" + txt_kala_ii_d.Text + @"',
                                            kala_ii_e               = '" + txt_kala_ii_e.Text + @"',
                                            kala_ii_f               = '" + txt_kala_ii_f.Text + @"',
                                            kala_ii_g               = '" + txt_kala_ii_g.Text + @"',
                                            kala_iii_a              = '" + txt_kala_iii_a.Text + @"',
                                            kala_iii_b              = '" + rb_kala_iii_b.SelectedIndex.ToString() + "::" + rb_kala_iii_b.Text + "::" + txt_kala_iii_b.Text + @"',
                                            kala_iii_c              = '" + rb_kala_iii_c.SelectedIndex.ToString() + "::" + rb_kala_iii_c.Text + "::" + txt_kala_iii_c.Text + @"',
                                            kala_iii_d              = '" + rb_kala_iii_d.SelectedIndex.ToString() + "::" + rb_kala_iii_d.Text + "::" + txt_kala_iii_d.Text + @"',
                                            baru_lahir_berat        = '" + txt_baru_lahir_berat.Text + @"',
                                            baru_lahir_panjang      = '" + txt_baru_lahir_panjang.Text + @"',
                                            baru_lahir_jk           = '" + rb_baru_lahir_jk.SelectedIndex.ToString() + "::" + rb_baru_lahir_jk.Text + @"',
                                            baru_lahir_nilai        = '" + rb_baru_lahir_nilai.SelectedIndex.ToString() + "::" + rb_baru_lahir_nilai.Text + @"',
                                            baru_lahir_ket          = '" + (chk_baru_lahir_ket.Checked ? chk_baru_lahir_ket.Text : "") + "::" + (chk_baru_lahir_ket_1.Checked ? chk_baru_lahir_ket_1.Text : "") + "::" + (chk_baru_lahir_ket_2.Checked ? chk_baru_lahir_ket_2.Text : "") + "::" + (chk_baru_lahir_ket_3.Checked ? chk_baru_lahir_ket_3.Text : "") + "::" + (chk_baru_lahir_ket_4.Checked ? chk_baru_lahir_ket_4.Text : "") + "::" + (chk_baru_lahir_ket_5.Checked ? chk_baru_lahir_ket_5.Text : "") + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");



                updateQueries.Add(@" update T2_R_INAP_BIDAN_2 set 
                                            rangsang_laktil         = '" + rb_rangsang_laktil.SelectedIndex.ToString() + "::" + rb_rangsang_laktil.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            plasenta_intack         = '" + rb_plasenta_intack.SelectedIndex.ToString() + "::" + rb_plasenta_intack.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            plasenta_tidak_lahir    = '" + rb_plasenta_tidak_lahir.SelectedIndex.ToString() + "::" + rb_plasenta_tidak_lahir.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            laserasi                = '" + rb_laserasi.SelectedIndex.ToString() + "::" + rb_laserasi.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            laserasi_parinium       = '" + rb_laserasi_parinium.SelectedIndex.ToString() + "::" + rb_laserasi_parinium.Text + "::" + rb_laserasi_parinium_tindakan.SelectedIndex.ToString() + "::" + rb_laserasi_parinium_tindakan.Text + "::" + txt_rangsang_laktil.Text + @"',
                                            atonia_uteri            = '" + rb_atonia_uteri.SelectedIndex.ToString() + "::" + rb_atonia_uteri.Text + "::" + txt_atonia_uteri.Text + @"',
                                            jumlah_pendarahan       = '" + txt_jumlah_pendarahan.Text + @"',
                                            masalah_lain            = '" + txt_masalah_lain.Text + @"',
                                            penata_masalah          = '" + txt_penata_masalah.Text + @"',
                                            hasilnya                = '" + txt_hasilnya.Text + @"',
                                            baru_lahir              = '" + rb_baru_lahir.SelectedIndex.ToString() + "::" + rb_baru_lahir.Text + "::" + (chk_cacat_ket_1.Checked ? chk_cacat_ket_1.Text : "") + "::" + (chk_cacat_ket_2.Checked ? chk_cacat_ket_2.Text : "") + "::" + (chk_cacat_ket_3.Checked ? chk_cacat_ket_3.Text : "") + "::" + (chk_cacat_ket_4.Checked ? chk_cacat_ket_4.Text : "") + "::" + (chk_cacat_ket_5.Checked ? chk_cacat_ket_5.Text : "") + "::" + (chk_cacat_ket_6.Checked ? chk_cacat_ket_6.Text : "") + "::" + (chk_cacat_ket_lain.Checked ? chk_cacat_ket_lain.Text : "") + "::" + txt_cacat_ket_lain.Text + @"',
                                            bayi_cacat              = '" + txt_bayi_cacat.Text + @"',
                                            hipotermia              = '" + txt_hipotermia.Text + @"',
                                            pemberian_asi           = '" + rb_pemberian_asi.SelectedIndex.ToString() + "::" + rb_pemberian_asi.Text + "::" + txt_pemberian_asi_ya.Text + "::" + txt_pemberian_asi_tdk.Text + @"',
                                            masalah_lahir           = '" + txt_masalah_lahir.Text + @"',
                                            hasil_lahir             = '" + txt_hasil_lahir.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");



                updateQueries.Add(@" update T2_DOKUMEN_SKALA_I set 
                                                tanggal             = '" + txt_skl1_tanggal.Text + @"',
                                                jam                 = '" + txt_skl1_jam.Text + @"',
                                                keluhan_utama       = '" + txt_skl1_keluhan_utama.Text + @"',
                                                kesadaran           = '" + txt_skl1_kesadaran.Text + @"',
                                                td_1                = '" + txt_skl1_td_1.Text + @"',
                                                td_2                = '" + txt_skl1_td_2.Text + @"',
                                                n                   = '" + txt_skl1_n.Text + @"',
                                                r                   = '" + txt_skl1_r.Text + @"',
                                                s                   = '" + txt_skl1_s.Text + @"',
                                                frekuensi           = '" + txt_skl1_frekuensi.Text + @"',
                                                interfal            = '" + txt_skl1_interfal.Text + @"',
                                                durasi              = '" + txt_skl1_durasi.Text + @"',
                                                djj                 = '" + rb_skl1_djj.SelectedIndex.ToString() + "::" + rb_skl1_djj.Text + "::" + txt_skl1_djj.Text + @"',
                                                diagnosa            = '" + txt_skl1_diagnosa.Text + @"',
                                                masalah_potensial   = '" + txt_skl1_masalah_potensial.Text + @"',
                                                antisipasi_masalah  = '" + txt_skl1_antisipasi_masalah.Text + @"',
                                                vulva               = '" + txt_skl1_vulva.Text + @"',
                                                pembukaan           = '" + txt_skl1_pembukaan.Text + @"',
                                                keadaan_ketuban     = '" + txt_skl1_keadaan_ketuban.Text + @"',
                                                presentasi          = '" + txt_skl1_presentasi.Text + @"',
                                                bagian_teraba       = '" + txt_skl1_bagian_teraba.Text + @"',
                                                turunnya_bagian     = '" + txt_skl1_turunnya_bagian.Text + @"',
                                                molage              = '" + txt_skl1_molage.Text + @"',
                                                vesica_urineria     = '" + rb_skl1_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl1_vesica_urineria.Text + @"',
                                                planning            = '" + txt_skl1_planning.Text + @"',
                                                jam_plan            = '" + txt_skl1_jam_plan.Text + @"',
                                                tgl_plan            = '" + txt_skl1_tgl_plan.Text + @"',
                                                dipimpin            = '" + txt_skl1_dipimpin.Text + @"',
                                                jam_mulai           = '" + txt_skl1_jam_mulai.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");


                updateQueries.Add(@"update T2_DOKUMEN_SKALA_II set
                                                tanggal         = '" + txt_skl2_tanggal.Text + @"',
                                                jam             = '" + txt_skl2_jam.Text + @"',
                                                keluhan_utama   = '" + txt_skl2_keluhan_utama.Text + @"',
                                                kesadaran       = '" + txt_skl2_kesadaran.Text + @"',
                                                td_1            = '" + txt_skl2_td_1.Text + @"',
                                                td_2            = '" + txt_skl2_td_2.Text + @"',
                                                n               = '" + txt_skl2_n.Text + @"',
                                                r               = '" + txt_skl2_r.Text + @"',
                                                s               = '" + txt_skl2_s.Text + @"',
                                                frekuensi       = '" + txt_skl2_frekuensi.Text + @"',
                                                interfal        = '" + txt_skl2_interfal.Text + @"',
                                                durasi          = '" + txt_skl2_durasi.Text + @"',
                                                djj             = '" + rb_skl2_djj.SelectedIndex.ToString() + "::" + rb_skl2_djj.Text + "::" + txt_skl2_djj.Text + @"',
                                                diagnosa        = '" + txt_skl2_diagnosa.Text + @"',
                                                masalah_potensial   = '" + txt_skl2_masalah_potensial.Text + @"',
                                                antisipasi_masalah  = '" + txt_skl2_antisipasi_masalah.Text + @"',
                                                vulva               = '" + txt_skl2_vulva.Text + @"',
                                                pembukaan           = '" + txt_skl2_pembukaan.Text + @"',
                                                keadaan_ketuban     = '" + txt_skl2_keadaan_ketuban.Text + @"',
                                                presentasi          = '" + txt_skl2_presentasi.Text + @"',
                                                bagian_teraba       = '" + txt_skl2_bagian_teraba.Text + @"',
                                                turunnya_bagian     = '" + txt_skl2_turunnya_bagian.Text + @"',
                                                molage              = '" + txt_skl2_molage.Text + @"',
                                                vesica_urineria     = '" + rb_skl2_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl2_vesica_urineria.Text + @"',
                                                planning            = '" + txt_skl2_planning.Text + @"',
                                                lahir_jam           = '" + txt_skl2_lahir_jam.Text + @"',
                                                jk                  = '" + txt_skl2_jk.Text + @"',
                                                bb                  = '" + txt_skl2_bb.Text + @"',
                                                lk                  = '" + txt_skl2_lk.Text + @"',
                                                ld                  = '" + txt_skl2_ld.Text + @"',
                                                kadaan_lahir        = '" + txt_skl2_kadaan_lahir.Text + @"',
                                                evaluasi            = '" + txt_skl2_evaluasi.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");



                updateQueries.Add(@"update T2_DOKUMEN_SKALA_III set
                                            tanggal         = '" + txt_skl3_tanggal.Text + @"',
                                            jam             = '" + txt_skl3_jam.Text + @"',
                                            keluhan_utama   = '" + txt_skl3_keluhan_utama.Text + @"',
                                            kesadaran       = '" + txt_skl3_kesadaran.Text + @"',
                                            td_1            = '" + txt_skl3_td_1.Text + @"',
                                            td_2            = '" + txt_skl3_td_2.Text + @"',
                                            n               = '" + txt_skl3_n.Text + @"',
                                            r               = '" + txt_skl3_r.Text + @"',
                                            s               = '" + txt_skl3_s.Text + @"',
                                            palpasi_abdomen = '" + txt_skl3_palpasi_abdomen.Text + @"',
                                            kontraksi_uterus    = '" + cb_skl3_kontraksi_uterus_1.Text + "::" + cb_skl3_kontraksi_uterus_2.Text + "::" + cb_skl3_kontraksi_uterus_3.Text + @"',
                                            uterus_membulat     = '" + rb_skl3_uterus_membulat.SelectedIndex.ToString() + "::" + rb_skl3_uterus_membulat.Text + @"',
                                            tinggi_fundus       = '" + txt_skl3_tinggi_fundus.Text + @"',
                                            semburan_darah      = '" + rb_skl3_semburan_darah.SelectedIndex.ToString() + "::" + rb_skl3_semburan_darah.Text + @"',
                                            vesica_urineria     = '" + rb_skl3_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl3_vesica_urineria.Text + @"',
                                            diagnosa            = '" + txt_skl3_diagnosa.Text + @"',
                                            masalah_potensial   = '" + txt_skl3_masalah_potensial.Text + @"',
                                            antisipasi_masalah  = '" + txt_skl3_antisipasi_masalah.Text + @"',
                                            planning            = '" + txt_skl3_planning.Text + @"',
                                            placenta_lahir      = '" + txt_skl3_placenta_lahir.Text + @"',
                                            spontan             = '" + rb_skl3_spontan.SelectedIndex.ToString() + "::" + rb_skl3_spontan.Text + @"',
                                            lengkap             = '" + rb_skl3_lengkap.SelectedIndex.ToString() + "::" + rb_skl3_lengkap.Text + @"',
                                            kontraksi           = '" + txt_skl3_kontraksi.Text + @"',
                                            pendarahan          = '" + txt_skl3_pendarahan.Text + @"',
                                            keadaan_jalan       = '" + txt_skl3_keadaan_jalan.Text + @"',
                                            bila_reptura        = '" + rb_skl3_bila_reptura.SelectedIndex.ToString() + "::" + rb_skl3_bila_reptura.Text + @"'
                                     where anamesa_id = '" + _AnamesaID + "' ");




                updateQueries.Add(@"update T2_DOKUMEN_SKALA_IV set
                                            tanggal         = '" + txt_skl4_tanggal.Text + @"',
                                            jam             = '" + txt_skl4_jam.Text + @"',
                                            keluhan_utama   = '" + txt_skl4_keluhan_utama.Text + @"',
                                            kesadaran       = '" + txt_skl4_kesadaran.Text + @"',
                                            td_1            = '" + txt_skl4_td_1.Text + @"',
                                            td_2            = '" + txt_skl4_td_2.Text + @"',
                                            n               = '" + txt_skl4_n.Text + @"',
                                            r               = '" + txt_skl4_r.Text + @"',
                                            s               = '" + txt_skl4_s.Text + @"',
                                            kontraksi_uterus = '" + cb_skl4_kontraksi_uterus_1.Text + "::" + cb_skl4_kontraksi_uterus_2.Text + "::" + cb_skl4_kontraksi_uterus_3.Text + @"',
                                            tinggi_fundus   = '" + txt_skl4_tinggi_fundus.Text + @"',
                                            vesica_urineria = '" + rb_skl4_vesica_urineria.SelectedIndex.ToString() + "::" + rb_skl4_vesica_urineria.Text + @"',
                                            jumlah_darah    = '" + txt_skl4_jumlah_darah.Text + @"',
                                            diagnosa        = '" + txt_skl4_diagnosa.EditValue.ToString() + @"',
                                            masalah_potensial   = '" + txt_skl4_masalah_potensial.Text + @"',
                                            antisipasi_masalah  = '" + txt_skl4_antisipasi_masalah.Text + @"',
                                            planning        = '" + txt_skl4_planning.Text + @"'
                                     where anamesa_id  = '" + _AnamesaID + "' ");

                


                koneksi.OpenConnection();
                koneksi.BeginTransaction();

                foreach (string updateQuery in updateQueries)
                {
                    bool success = koneksi.ExecuteNonQueryCommitRollback(updateQuery);
                    if (!success)
                    {
                        if (!success)
                        {
                            koneksi.RollbackTransaction();
                            MessageBox.Show("Data Gagal Disimpan !!");
                            return;
                        }
                    }
                }

                koneksi.CommitTransaction();
                MessageBox.Show("Data Berhasil Disimpan");

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }
            finally
            {
                koneksi.CloseConnection();
            }

        }

        private void selectedIndexRb()
        {
            rb_tempat_persalinan.SelectedIndex = -1;
            rb_rujuk_kala.SelectedIndex = -1;
            rb_pendamping_rujuk.SelectedIndex = -1;
            rb_kala_i_a.SelectedIndex = -1;
            rb_kala_ii_a.SelectedIndex = -1;
            rb_kala_ii_b.SelectedIndex = -1;
            rb_kala_ii_c.SelectedIndex = -1;
            rb_kala_ii_d.SelectedIndex = -1;
            rb_kala_iii_b.SelectedIndex = -1;
            rb_kala_iii_c.SelectedIndex = -1;
            rb_kala_iii_d.SelectedIndex = -1;

            rb_rangsang_laktil.SelectedIndex = -1;
            rb_plasenta_intack.SelectedIndex = -1;
            rb_plasenta_tidak_lahir.SelectedIndex = -1;
            rb_laserasi.SelectedIndex = -1;
            rb_laserasi_parinium.SelectedIndex = -1;
            rb_atonia_uteri.SelectedIndex = -1;
            rb_baru_lahir.SelectedIndex = -1;
            rb_pemberian_asi.SelectedIndex = -1;

            rb_skl2_djj.SelectedIndex = -1;
            rb_skl2_vesica_urineria.SelectedIndex = -1;
            rb_skl3_uterus_membulat.SelectedIndex = -1;
            rb_skl3_semburan_darah.SelectedIndex = -1;
            rb_skl3_vesica_urineria.SelectedIndex = -1;
            rb_skl3_spontan.SelectedIndex = -1;
            rb_skl3_lengkap.SelectedIndex = -1;
            rb_skl3_bila_reptura.SelectedIndex = -1;
            rb_skl4_vesica_urineria.SelectedIndex = -1;

            

        }
        private void kondisiEnable(bool kondisi)
        {
            txt_nama_istr.Enabled = kondisi;
            txt_umur_istr.Enabled = kondisi;
            txt_agama_istr.Enabled = kondisi;
            txt_pendidikan_istr.Enabled = kondisi;
            txt_pekerjaan_istr.Enabled = kondisi;
            txt_suku_istr.Enabled = kondisi;
            txt_kawin_lama_istr.Enabled = kondisi;
            txt_kawin_frek_istr.Enabled = kondisi;
            txt_nama_suami.Enabled = kondisi;
            txt_umur_suami.Enabled = kondisi;
            txt_agama_suami.Enabled = kondisi;
            txt_pendidikan_suami.Enabled = kondisi;
            txt_pekerjaan_suami.Enabled = kondisi;
            txt_suku_suami.Enabled = kondisi;
            txt_kawin_lama_suami.Enabled = kondisi;
            txt_kawin_frek_suami.Enabled = kondisi;
            txt_biodata_alamat.Enabled = kondisi;
            txt_biodata_keluhan.Enabled = kondisi;
            txt_rwyt_hamil_a.Enabled = kondisi;
            date_rwyt_hamil_hpht_fr.Enabled = kondisi;
            date_rwyt_hamil_hpht_to.Enabled = kondisi;
            date_rwyt_hamil_anc.Enabled = kondisi;
            txt_rwyt_hamil_komp.Enabled = kondisi;
            txt_rwyt_kb_kontra.Enabled = kondisi;
            txt_rwyt_kb_lama.Enabled = kondisi;
            txt_rwyt_kb_alasan_henti.Enabled = kondisi;
            txt_rwyt_penyakit_ibu.Enabled = kondisi;
            txt_aktiv_nutrisi.Enabled = kondisi;
            txt_aktiv_eliminasi.Enabled = kondisi;
            txt_aktiv_istirahat.Enabled = kondisi;
            txt_k_umum.Enabled = kondisi;
            txt_k_sadar.Enabled = kondisi;
            txt_umum_vita_td.Enabled = kondisi;
            txt_umum_vita_td_1.Enabled = kondisi;
            txt_umum_vital_n.Enabled = kondisi;
            txt_umum_vital_r.Enabled = kondisi;
            txt_umum_vital_s.Enabled = kondisi;
            txt_ekstermitas_atas.Enabled = kondisi;
            txt_ekstermitas_bawah.Enabled = kondisi;
            txt_asesment_awal.Enabled = kondisi;
            txt_planning_awal.Enabled = kondisi;
            txt_gen_inpeksi.Enabled = kondisi;
            txt_gen_vulva.Enabled = kondisi;
            txt_gen_portio.Enabled = kondisi;
            txt_gen_pembukaan.Enabled = kondisi;
            txt_gen_penurunan.Enabled = kondisi;
            txt_gen_ketuban.Enabled = kondisi;
            txt_gen_presntasi.Enabled = kondisi;

            txt_fisik_mata.Enabled = kondisi;
            txt_fisik_leher.Enabled = kondisi;
            txt_fisik_payudara.Enabled = kondisi;
            txt_fisik_inpeksi.Enabled = kondisi;
            txt_fisik_leopold_i.Enabled = kondisi;
            txt_fisik_leopold_ii.Enabled = kondisi;
            txt_fisik_leopold_iii.Enabled = kondisi;
            txt_fisik_leopold_iv.Enabled = kondisi;
            txt_fisik_tfu.Enabled = kondisi;
            txt_fisik_djj.Enabled = kondisi;
            txt_fisik_his.Enabled = kondisi;
            txt_penunjang_i.Enabled = kondisi;
            txt_penunjang_iii.Enabled = kondisi;
            txt_protein_urine.Enabled = kondisi;
            txt_glukosa_urine.Enabled = kondisi;
            date_tgl_persalinan.Enabled = kondisi;
            txt_nama_bidan.Enabled = kondisi;
            rb_tempat_persalinan.Enabled = kondisi;
            txt_alamat_persalinan.Enabled = kondisi;
            rb_rujuk_kala.Enabled = kondisi;
            txt_alasan_rujuk.Enabled = kondisi;
            txt_tempat_rujuk.Enabled = kondisi;
            rb_pendamping_rujuk.Enabled = kondisi;
            rb_kala_i_a.Enabled = kondisi;
            txt_kala_i_b.Enabled = kondisi;
            txt_kala_i_c.Enabled = kondisi;
            txt_kala_i_d.Enabled = kondisi;
            rb_kala_ii_a.Enabled = kondisi;
            rb_kala_ii_b.Enabled = kondisi;
            rb_kala_ii_c.Enabled = kondisi;
            rb_kala_ii_d.Enabled = kondisi;
            txt_kala_ii_e.Enabled = kondisi;
            txt_kala_ii_f.Enabled = kondisi;
            txt_kala_ii_g.Enabled = kondisi;
            txt_kala_iii_a.Enabled = kondisi;
            rb_kala_iii_b.Enabled = kondisi;
            rb_kala_iii_c.Enabled = kondisi;
            rb_kala_iii_d.Enabled = kondisi;
            txt_baru_lahir_berat.Enabled = kondisi;
            txt_baru_lahir_panjang.Enabled = kondisi;
            rb_baru_lahir_jk.Enabled = kondisi;
            rb_baru_lahir_nilai.Enabled = kondisi;
            chk_baru_lahir_ket.Enabled = kondisi;
            

            rb_rangsang_laktil.Enabled = kondisi;
            rb_plasenta_intack.Enabled = kondisi;
            rb_plasenta_tidak_lahir.Enabled = kondisi;
            rb_laserasi.Enabled = kondisi;
            rb_laserasi_parinium.Enabled = kondisi;
            rb_atonia_uteri.Enabled = kondisi;
            txt_jumlah_pendarahan.Enabled = kondisi;
            txt_masalah_lain.Enabled = kondisi;
            txt_penata_masalah.Enabled = kondisi;
            txt_hasilnya.Enabled = kondisi;
            rb_baru_lahir.Enabled = kondisi;
            txt_bayi_cacat.Enabled = kondisi;
            txt_hipotermia.Enabled = kondisi;
            rb_pemberian_asi.Enabled = kondisi;
            txt_masalah_lahir.Enabled = kondisi;
            txt_hasil_lahir.Enabled = kondisi;

            chk_baru_lahir_ket_1.Enabled = kondisi;
            chk_baru_lahir_ket_2.Enabled = kondisi;
            chk_baru_lahir_ket_3.Enabled = kondisi;
            chk_baru_lahir_ket_4.Enabled = kondisi;
            chk_baru_lahir_ket_5.Enabled = kondisi;

            chk_cacat_ket_1.Enabled = kondisi;
            chk_cacat_ket_2.Enabled = kondisi;
            chk_cacat_ket_3.Enabled = kondisi;
            chk_cacat_ket_4.Enabled = kondisi;
            chk_cacat_ket_5.Enabled = kondisi;
            chk_cacat_ket_6.Enabled = kondisi;
            chk_cacat_ket_lain.Enabled = kondisi;
            txt_cacat_ket_lain.Enabled = kondisi;
            

            txt_MasalahKala4.Enabled = kondisi;
            txt_PenatalaksanaKala4.Enabled = kondisi;


            txt_skl1_tanggal.Enabled = kondisi;
            txt_skl1_jam.Enabled = kondisi;
            txt_skl1_keluhan_utama.Enabled = kondisi;
            txt_skl1_kesadaran.Enabled = kondisi;
            txt_skl1_td_1.Enabled = kondisi;
            txt_skl1_td_2.Enabled = kondisi;
            txt_skl1_n.Enabled = kondisi;
            txt_skl1_r.Enabled = kondisi;
            txt_skl1_s.Enabled = kondisi;
            txt_skl1_frekuensi.Enabled = kondisi;
            txt_skl1_interfal.Enabled = kondisi;
            txt_skl1_durasi.Enabled = kondisi;
            txt_skl1_djj.Enabled = kondisi;
            txt_skl1_diagnosa.Enabled = kondisi;
            txt_skl1_masalah_potensial.Enabled = kondisi;
            txt_skl1_antisipasi_masalah.Enabled = kondisi;
            txt_skl1_vulva.Enabled = kondisi;
            txt_skl1_pembukaan.Enabled = kondisi;
            txt_skl1_keadaan_ketuban.Enabled = kondisi;
            txt_skl1_presentasi.Enabled = kondisi;
            txt_skl1_bagian_teraba.Enabled = kondisi;
            txt_skl1_turunnya_bagian.Enabled = kondisi;
            txt_skl1_molage.Enabled = kondisi;
            rb_skl1_vesica_urineria.Enabled = kondisi;
            txt_skl1_planning.Enabled = kondisi;
            txt_skl1_jam_plan.Enabled = kondisi;
            txt_skl1_tgl_plan.Enabled = kondisi;
            txt_skl1_dipimpin.Enabled = kondisi;
            txt_skl1_jam_mulai.Enabled = kondisi;

            txt_skl2_tanggal.Enabled = kondisi;
            txt_skl2_jam.Enabled = kondisi;
            txt_skl2_keluhan_utama.Enabled = kondisi;
            txt_skl2_kesadaran.Enabled = kondisi;
            txt_skl2_td_1.Enabled = kondisi;
            txt_skl2_td_2.Enabled = kondisi;
            txt_skl2_n.Enabled = kondisi;
            txt_skl2_r.Enabled = kondisi;
            txt_skl2_s.Enabled = kondisi;
            txt_skl2_frekuensi.Enabled = kondisi;
            txt_skl2_interfal.Enabled = kondisi;
            txt_skl2_durasi.Enabled = kondisi;
            txt_skl2_djj.Enabled = kondisi;
            txt_skl2_diagnosa.Enabled = kondisi;
            txt_skl2_masalah_potensial.Enabled = kondisi;
            txt_skl2_antisipasi_masalah.Enabled = kondisi;
            txt_skl2_vulva.Enabled = kondisi;
            txt_skl2_pembukaan.Enabled = kondisi;
            txt_skl2_keadaan_ketuban.Enabled = kondisi;
            txt_skl2_presentasi.Enabled = kondisi;
            txt_skl2_bagian_teraba.Enabled = kondisi;
            txt_skl2_turunnya_bagian.Enabled = kondisi;
            txt_skl2_molage.Enabled = kondisi;
            rb_skl2_vesica_urineria.Enabled = kondisi;
            txt_skl2_planning.Enabled = kondisi;
            txt_skl2_lahir_jam.Enabled = kondisi;
            txt_skl2_jk.Enabled = kondisi;
            txt_skl2_bb.Enabled = kondisi;
            txt_skl2_lk.Enabled = kondisi;
            txt_skl2_ld.Enabled = kondisi;
            txt_skl2_kadaan_lahir.Enabled = kondisi;
            txt_skl2_evaluasi.Enabled = kondisi;

            txt_skl3_tanggal.Enabled = kondisi;
            txt_skl3_jam.Enabled = kondisi;
            txt_skl3_keluhan_utama.Enabled = kondisi;
            txt_skl3_kesadaran.Enabled = kondisi;
            txt_skl3_td_1.Enabled = kondisi;
            txt_skl3_td_2.Enabled = kondisi;
            txt_skl3_n.Enabled = kondisi;
            txt_skl3_r.Enabled = kondisi;
            txt_skl3_s.Enabled = kondisi;
            txt_skl3_palpasi_abdomen.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_1.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_2.Enabled = kondisi;
            cb_skl3_kontraksi_uterus_3.Enabled = kondisi;
            rb_skl3_uterus_membulat.Enabled = kondisi;
            txt_skl3_tinggi_fundus.Enabled = kondisi;
            rb_skl3_semburan_darah.Enabled = kondisi;
            rb_skl3_vesica_urineria.Enabled = kondisi;
            txt_skl3_diagnosa.Enabled = kondisi;
            txt_skl3_masalah_potensial.Enabled = kondisi;
            txt_skl3_antisipasi_masalah.Enabled = kondisi;
            txt_skl3_planning.Enabled = kondisi;
            txt_skl3_placenta_lahir.Enabled = kondisi;
            rb_skl3_spontan.Enabled = kondisi;
            rb_skl3_lengkap.Enabled = kondisi;
            txt_skl3_kontraksi.Enabled = kondisi;
            txt_skl3_pendarahan.Enabled = kondisi;
            txt_skl3_keadaan_jalan.Enabled = kondisi;
            rb_skl3_bila_reptura.Enabled = kondisi;


            txt_skl4_tanggal.Enabled = kondisi;
            txt_skl4_jam.Enabled = kondisi;
            txt_skl4_keluhan_utama.Enabled = kondisi;
            txt_skl4_kesadaran.Enabled = kondisi;
            txt_skl4_td_1.Enabled = kondisi;
            txt_skl4_td_2.Enabled = kondisi;
            txt_skl4_n.Enabled = kondisi;
            txt_skl4_r.Enabled = kondisi;
            txt_skl4_s.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_1.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_2.Enabled = kondisi;
            cb_skl4_kontraksi_uterus_3.Enabled = kondisi;
            txt_skl4_tinggi_fundus.Enabled = kondisi;
            rb_skl4_vesica_urineria.Enabled = kondisi;
            txt_skl4_jumlah_darah.Enabled = kondisi;
            txt_skl4_diagnosa.Enabled = kondisi;
            txt_skl4_masalah_potensial.Enabled = kondisi;
            txt_skl4_antisipasi_masalah.Enabled = kondisi;
            txt_skl4_planning.Enabled = kondisi;

            rb_skl1_djj.Enabled = kondisi;
            rb_skl2_djj.Enabled = kondisi; 
        }

        static void ConvertColumnNamesToUppercase(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                column.ColumnName = column.ColumnName.ToUpper(); 
            }
        }

     
        private bool functionChk(string data_asli, string pencarian)
        {
            string[] aa = data_asli.Split(new string[] { pencarian }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void functionSplitIndex_1(string data_asli,  DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length == 1)
            {
                //rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[0] == null) ? "" : aa[0];
            }else  if (aa.Length == 2)
            {
                //rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[1] == null) ? "" : aa[1];
            }
            else if (aa.Length == 3)
            {
                //rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[1] == null) ? "" : aa[1];
            }

        }

        private void functionSplitIndex_2(string data_asli, DevExpress.XtraEditors.RadioGroup rbt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
            }

        }
        private void functionSplitIndex_3(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 3)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
            }

        }
        private void functionSplitIndex_4_asi(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt, DevExpress.XtraEditors.TextEdit txt1)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 4)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
                txt1.EditValue = (aa[3] == null) ? "" : aa[3];
            }

        }
        private void functionSplitIndex_10(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 9)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[9] == null) ? "" : aa[9];
            }

        }
        private void functionSplitIndex_5(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.RadioGroup rbt1, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 5)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                rbt1.SelectedIndex = Convert.ToInt32(aa[2]);
                txt.EditValue = (aa[4] == null) ? "" : aa[4];
            }

        }

        private void rb_tempat_persalinan_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_tempat_persalinan.Enabled = (rb_tempat_persalinan.SelectedIndex == 5);
        }

        private void rb_kala_ii_a_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_a.Enabled = (rb_kala_ii_a.SelectedIndex == 0);

        }

        private void rb_kala_ii_c_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_c.Enabled = (rb_kala_ii_c.SelectedIndex == 1);
        }

        private void rb_kala_ii_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_ii_d.Enabled = (rb_kala_ii_d.SelectedIndex == 1);
        }

        private void rb_kala_iii_b_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_b.Enabled = (rb_kala_iii_b.SelectedIndex == 0);
        }

        private void rb_kala_iii_c_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_c.Enabled = (rb_kala_iii_c.SelectedIndex == 0);
        }

        private void rb_kala_iii_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_kala_iii_d.Enabled = (rb_kala_iii_d.SelectedIndex == 1);
        }

        private void rb_rangsang_laktil_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_rangsang_laktil.Enabled = (rb_rangsang_laktil.SelectedIndex == 1);
        }

        private void rb_plasenta_intack_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_plasenta_intack.Enabled = (rb_plasenta_intack.SelectedIndex == 1);
        }

        private void rb_plasenta_tidak_lahir_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_plasenta_tidak_lahir.Enabled = (rb_plasenta_tidak_lahir.SelectedIndex == 0);
        }

        private void rb_laserasi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_laserasi.Enabled = (rb_laserasi.SelectedIndex == 0);
        }

        private void rb_laserasi_parinium_tindakan_SelectedIndexChanged(object sender, EventArgs e)
        {

            txt_laserasi_parinium.Enabled = (rb_laserasi_parinium_tindakan.SelectedIndex == 2);
        }

        private void rb_atonia_uteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_atonia_uteri.Enabled = (rb_atonia_uteri.SelectedIndex == 0);
        }

        private void gvwPasien_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvwPersalinanLalu_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvMedis_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";

            //date = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
            //que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            //rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString(); 

            if (e.Column.Caption == "Nama Pelayanan")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";
                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }

            if (e.Column.Caption == "Remark")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void gvJadwalObat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvJadwalObat_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                if (stok != "")
                {
                    if (Convert.ToInt32(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt32(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }

            if (e.Column.Caption == "Confirm")
            {
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }
 
        private void gvJadwalObat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                //dtStock = null;
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = koneksi.GetDataTable(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";
                //cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");


                //    OleDbConnection oraConn = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConn);
                ////dtStock.Clear();
                //adOra0.Fill(dtStock);
                //cek_stok = dtStock.Rows[0]["stock"].ToString();

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          //" stock - (select nvl(SUM(med_qty),0) from cs_receipt  " +
                          //"           where TO_CHAR(insp_date, 'yyyy-mm-dd') = '" + lMedDate.Text + "'  " +
                          //"             and confirm = 'N'  " +
                          //"             and med_cd = a.med_cd) stock, uom  " +
                          //" klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'"+ medcd + "') +  " +
                          //" klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') -  " +
                          //" klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') - " +
                          //" klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','yyyy-mm-dd'),'" + medcd + "') stock, uom " +
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = ConnOra.Data_Table_ora(sql_med);

                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                //DataTable dt = new DataTable();
                //adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                //DataTable dtf = new DataTable();
                //adOraf.Fill(dtf);
                listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                view.Columns[6].OptionsColumn.ReadOnly = true;
                view.Columns[10].OptionsColumn.ReadOnly = true;
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan);

                    //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    //OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    //DataTable dtf = new DataTable();
                    //adOraf.Fill(dtf);

                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataResep();
                    }
                }


            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock =0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for); 

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *
                
                if(!cstock.ToString().Equals(""))
                {
                    istock =  Convert.ToInt32(cstock);
                    if(istock - Convert.ToInt32(qty) < 0)
                    { 
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    } 
                } 
            } 
        }

        private void btnSimpanObat_Click(object sender, EventArgs e)
        {

            string r_id = "", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", RECEIPT_ID = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                if (gvMedisBidan.RowCount > 0)
                {
                    bool save = false; int ssave = 0;
                    for (int i = 0; i < gvMedisBidan.RowCount; i++)
                    {
                        r_id = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[0]).ToString();
                        kode = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[1]).ToString();
                        dosis = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[4]).ToString();
                        info = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[5]).ToString();
                        jumlah = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[7]).ToString();
                        stok = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[6]).ToString();
                        con = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[10]).ToString();
                        action = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[9]).ToString();
                        harga = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[12]).ToString();
                        hari = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[11]).ToString();
                        jph = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[13]).ToString();
                        info_dosis = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[14]).ToString();

                        string dte = "", sql = " ";
                        object tgl = gvMedisBidan.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                         
                        if (r_id.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " Insert into KLINIK.cs_receipt (rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME) ";
                            sql = sql + " values(  '" + _RM_NO + "', to_date('" + dte + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvMedisBidan, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvMedisBidan' ) ";
                            ssave = 2;
                            ORADB.Execute(ORADB.XE, sql);

                        }
                        else
                        {
                            ssave = 1;
                            if (con.ToString().Equals("N")) // receipt_id,  cs_receipt_seq.nextval, "+ RECEIPT_ID.ToString() +", 
                            {
                                sql = "";
                                sql = sql + " Update  KLINIK.cs_receipt ";
                                sql = sql + "    set  insp_date = to_date('" + today + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvMedisBidan, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                                sql = sql + "  where  RECEIPT_ID =  '" + r_id + "' and GRID_NAME =  'gvMedisBidan' ";

                                ORADB.Execute(ORADB.XE, sql);
                                ssave = 3;
                            }
                        } 
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    if (ssave == 1)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (ssave == 2)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataResep();
                    }
                    else if (ssave == 3)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataResep();
                    } 
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }

            //try
            //{
            //    if (gvMedisBidan.RowCount > 0)
            //    {

            //        DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
            //        //ORADB.SetData(ORADB.XE, "Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "'  ");
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            RECEIPT_ID = dt.Rows[0]["RECEIPT_ID"].ToString();
            //            ORADB.Execute(ORADB.XE, " insert into KLINIK.cs_receipt_del select a.*, sysdate, '" + DB.vUserId + "' as emp from KLINIK.cs_receipt a  where  ID_VISIT = '" + visitid + "'  and GRID_NAME = 'gvMedisBidan' ");
            //            ORADB.Execute(ORADB.XE, " Delete from KLINIK.cs_receipt  where ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvMedisBidan' ");
            //        }

            //        string sql = "insert all ";
            //        for (int i = 0; i < gvMedisBidan.RowCount; i++)
            //        {

            //            //id = gvJadwalObat.GetRowCellValue(i, gvJadwalObat.Columns[0]).ToString();
            //            kode = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[1]).ToString();
            //            dosis = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[4]).ToString();
            //            info = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[5]).ToString();
            //            jumlah = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[7]).ToString();
            //            stok = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[6]).ToString();
            //            con = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[10]).ToString();
            //            action = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[9]).ToString();
            //            harga = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[12]).ToString();
            //            hari = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[11]).ToString();
            //            jph = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[13]).ToString();
            //            info_dosis = gvMedisBidan.GetRowCellValue(i, gvMedisBidan.Columns[14]).ToString();

            //            string dte = "";
            //            object tgl = gvMedisBidan.GetRowCellValue(i, "TANGGAL");
            //            if (tgl != null && tgl is DateTime)
            //            {
            //                DateTime selectedDateTime = (DateTime)tgl;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }
            //            else
            //            {
            //                DateTime selectedDateTime = DateTime.Now;
            //                dte = selectedDateTime.ToString("yyyy-MM-dd");
            //            }
            //            if (RECEIPT_ID.ToString().Equals(""))
            //            {
            //                sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME) ";
            //                sql = sql + " values(cs_receipt_seq.nextval, '" + _RM_NO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
            //                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvMedisBidan, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvMedisBidan' ) ";
            //            }
            //            else
            //            {
            //                sql = sql + " into KLINIK.cs_receipt (receipt_id, rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME) ";
            //                sql = sql + " values(" + RECEIPT_ID.ToString() + ", '" + _RM_NO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
            //                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvMedisBidan, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvMedisBidan' ) ";
            //            }


            //            //sql = sql + " into KLINIK.cs_receipt (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
            //            //sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvMedisBidan, i, "HEAD_ID") + "','" + FN.strVal(gvMedisBidan, i, "TREAT_ITEM_ID") + "'  ,";
            //            //sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvMedisBidan, i, "TREAT_QTY") + "', '" + FN.strVal(gvMedisBidan, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvMedisBidan, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvMedisBidan, i, "TREAT_ITEM_PRICE")) + ", ";
            //            //sql = sql + " '" + FN.strVal(gvMedisBidan, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvMedisBidan, i, "JAM") + "' , 'gvMedis' )";
            //        }
            //        sql = sql + " select * from dual";
            //        bool save = ORADB.Execute(ORADB.XE, sql);
            //        if (save)
            //        {
            //            MessageBox.Show("Jadwal Pemberian Obat Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FN.errosMsg(ex.Message, "Error");
            //}
        }

        private void btnAddJadwalObat_Click(object sender, EventArgs e)
        {
            //gvMedisBidan.OptionsBehavior.EditingMode = GridEditingMode.Default;
            //gvMedisBidan.AddNewRow();

            if (dtMedis == null) return;

            DataRow newRow = dtMedis.NewRow();

            //newRow["SEQ"] = ((gvVisitBidan.RowCount) + 1).ToString();
            //newRow["HEAD_ID"] = headid;
            //newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            dtMedis.Rows.Add(newRow);

            gcVisitBidan.DataSource = dtMedis;
        }

        private void gvMedisBidan_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:MM")); 
        }

        private void bttambahobat_Click(object sender, EventArgs e)
        {
            gvMedisBidan.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvMedisBidan.AddNewRow();
        }

        private void rb_pemberian_asi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_pemberian_asi_ya.Enabled = (rb_pemberian_asi.SelectedIndex == 0);
            txt_pemberian_asi_tdk.Enabled = (rb_pemberian_asi.SelectedIndex == 1);
        }

        private void gvVisitBidan_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string a = "", tmp_stat = "", date = "", que = "", rm_no = "", no_visit = "";

            if (e.Column.Caption == "Nama Pelayanan")
            {
                a = view.GetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_ID"]).ToString();
                no_visit = view.GetRowCellValue(e.RowHandle, view.Columns["ID_VISIT"]).ToString();
                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();

                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";

                DataTable dt0 = ConnOra.Data_Table_ora(sql_);

                //OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                //DataTable dt0 = new DataTable();
                //adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["TREAT_GROUP_ID"].ToString();
                    price = dt0.Rows[0]["TREAT_ITEM_PRICE"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where ID_VISIT = '" + visitid + "'  ";
                DataTable dt1 = ConnOra.Data_Table_ora(sql_head);
                //OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                //DataTable dt1 = new DataTable();
                //adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["HEAD_ID"].ToString();
                    stbyr = dt1.Rows[0]["PAY_STATUS"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns["HEAD_ID"], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_GROUP_ID"], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_QTY"], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns["TREAT_ITEM_PRICE"], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns["PAY_STATUS"], stbyr);
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }

            }

            if (e.Column.Caption == "Note Visit")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "U");
                }
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                   "Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "' AND  f_active = 'Y' and receipt_id = '" + FN.strVal(gvMedisBidan, gvMedisBidan.FocusedRowHandle, "RECEIPT_ID") + "' and CONFIRM ='Y'");

                if (dt != null && dt.Rows.Count > 0)
                {
                    MessageBox.Show("Maaf Data Confirm Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update KLINIK.cs_receipt  set f_active = 'N', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where ID_VISIT = '" + visitid + "' and receipt_id = '" + FN.strVal(gvMedisBidan, gvMedisBidan.FocusedRowHandle, "RECEIPT_ID") + "' AND  f_active = 'Y' and CONFIRM ='N' and GRID_NAME='gvMedisBidan'   ";

                    try
                    {
                        ORADB.Execute(ORADB.XE, sql_delete); 
                         
                        gvMedisBidan.DeleteRow(gvMedisBidan.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil dihapus");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gvObatUmumBidan.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvObatUmumBidan.AddNewRow();
        }

        private void gvObatUmumBidan_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[15], DateTime.Now);
            view.SetRowCellValue(e.RowHandle, view.Columns[16], DateTime.Now.ToString("HH:MM"));
        }

        private void gvObatUmumBidan_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvObatUmumBidan_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                if (stok != "")
                {
                    if (Convert.ToInt32(stok) == 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt32(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                } 
            }

            if (e.Column.Caption == "Confirm")
            {
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                   "Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_receipt where ID_VISIT = '" + visitid + "' AND  f_active = 'Y' and receipt_id = '" + FN.strVal(gvObatUmumBidan, gvObatUmumBidan.FocusedRowHandle, "RECEIPT_ID") + "' and CONFIRM ='Y'");

                if (dt != null && dt.Rows.Count > 0)
                {
                    MessageBox.Show("Maaf Data Confirm Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update KLINIK.cs_receipt  set f_active = 'N', UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where ID_VISIT = '" + visitid + "' and receipt_id = '" + FN.strVal(gvObatUmumBidan, gvObatUmumBidan.FocusedRowHandle, "RECEIPT_ID") + "' AND  f_active = 'Y' and CONFIRM ='N' and GRID_NAME='gvObatUmumBidan'   ";

                    try
                    {
                        ORADB.Execute(ORADB.XE, sql_delete);

                        gvObatUmumBidan.DeleteRow(gvObatUmumBidan.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil dihapus");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }

            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string r_id = "", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", info_cara = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                if (gvObatUmumBidan.RowCount > 0)
                {
                    bool save = false; int ssave = 0;
                    for (int i = 0; i < gvObatUmumBidan.RowCount; i++)
                    {
                        r_id = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[0]).ToString();
                        kode = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[1]).ToString();
                        dosis = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[4]).ToString();
                        info = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[5]).ToString();
                        jumlah = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[7]).ToString();
                        stok = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[6]).ToString();
                        con = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[10]).ToString();
                        action = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[9]).ToString();
                        harga = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[12]).ToString();
                        hari = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[11]).ToString();
                        jph = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[13]).ToString();
                        info_dosis = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[14]).ToString();
                        info_cara = gvObatUmumBidan.GetRowCellValue(i, gvObatUmumBidan.Columns[21]).ToString();

                        string dte = "", sql = "";
                        object tgl = gvObatUmumBidan.GetRowCellValue(i, "TANGGAL");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }

                        if (r_id.ToString().Equals(""))
                        {
                            sql = " ";
                            sql = sql + " insert into KLINIK.cs_receipt ( rm_no, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ID_VISIT, ins_date, ins_emp, GRID_NAME, CARA, FLAG_PULANG) ";
                            sql = sql + " values( '" + _RM_NO + "', to_date('" + today + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                            sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObatUmumBidan, i, "INS_JAM") + "' , '" + visitid + "',sysdate, '" + DB.vUserId + "' , 'gvObatUmumBidan', '" + info_cara + "','Y' ) ";

                            ssave = 2;
                            ORADB.Execute(ORADB.XE, sql);

                        }
                        else
                        {
                            ssave = 1;
                            if (con.ToString().Equals("N"))
                            {
                                sql = " ";
                                sql = sql + " Update  KLINIK.cs_receipt ";
                                sql = sql + "    set  insp_date = to_date('" + today + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvObatUmumBidan, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "', CARA = '" + info_cara + "' , UPD_EMP = '" + DB.vUserId + "',UPD_DATE = SYSDATE  ";
                                sql = sql + "  where  RECEIPT_ID =  '" + r_id + "' and GRID_NAME =  'gvObatUmumBidan' ";

                                ORADB.Execute(ORADB.XE, sql);
                                ssave = 3;
                            }
                        }
                    }

                    if (ssave == 1)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Umum Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (ssave == 2)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Umum Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataResep();
                    }
                    else if (ssave == 3)
                    {
                        MessageBox.Show("Jadwal Pemberian Obat Umum Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataResep();
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void bdelnone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
               "Message",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_head where HEAD_ID = '" + FN.strVal(gvVisitBidan, gvVisitBidan.FocusedRowHandle, "HEAD_ID") + "' and STATUS ='OPN' and PAY_STATUS ='OPN' ");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update  KLINIK.cs_treatment_detail   set f_active = 'N', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where DETAIL_ID= '" + FN.strVal(gvVisitBidan, gvVisitBidan.FocusedRowHandle, "DETAIL_ID") + "' AND  f_active = 'Y'  ";

                    try
                    {
                        ORADB.Execute(ORADB.XE, sql_delete);

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvVisitBidan.DeleteRow(gvVisitBidan.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil dihapus");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Maaf Data Close Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void gvObatUmumBidan_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = koneksi.GetDataTable(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0";

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = koneksi.GetDataTable(sql_med);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);
                listFormula.Clear();
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[3], med_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    //view.SetRowCellValue(e.RowHandle, view.Columns[1], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan);
                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString();

                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                    }
                }
            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt32(tmp_hari); //Convert.ToInt32(tmp_hari) * Convert.ToInt32(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt32(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            }
        }

        private void bdelmedis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                  "Message",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_head where HEAD_ID = '" + FN.strVal(gvPelayanBidan, gvPelayanBidan.FocusedRowHandle, "HEAD_ID") + "' and STATUS ='OPN' and PAY_STATUS ='OPN' ");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql_delete = "";

                    sql_delete = "";
                    sql_delete = sql_delete + " update  KLINIK.cs_treatment_detail   set f_active = 'N', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = SYSDATE  ";
                    sql_delete = sql_delete + "  where DETAIL_ID= '" + FN.strVal(gvPelayanBidan, gvPelayanBidan.FocusedRowHandle, "DETAIL_ID") + "' AND  f_active = 'Y'  ";

                    try
                    {
                        ORADB.Execute(ORADB.XE, sql_delete);

                        //MessageBox.Show("Query Exec : " + sql_delete);
                        gvPelayanBidan.DeleteRow(gvPelayanBidan.FocusedRowHandle);
                        MessageBox.Show("Data Berhasil dihapus");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Maaf Data Close Tidak Dapat dihapus...! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadRIBidan(); 
            LoadItemLayanan();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            bool save;
            string tgl_out = dtKeluarx.DateTime.ToString("yyyy-MM-dd") + " " + txtjam.Text.ToString() + ":00";
            DataTable dtDiagnos = ConnOra.Data_Table_ora("select * from KLINIK.cs_diagnosa where ANAMNESA_ID = " + _AnamesaID + " and TYPE_DIAGNOSA = 'E' ");
            if (dtDiagnos.Rows.Count > 0)
            {
                Dictionary<string, string> DiagnosaPulang = new Dictionary<string, string>
                    {
                        { "rm_no", RMNO },
                        { "insp_date",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "item_cd", txt_diagnosa_akhir.EditValue.ToString() },
                        //{ "remark", txAnjuran.Text.ToString()  },
                        //{ "NOTED", txTerapiLanjtan.Text.ToString()  },
                        { "UPD_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "UPD_EMP", DB.vUserId }
                    };
                MD.UpdateData(ORADB.XE, "cs_diagnosa", "ANAMNESA_ID = " + _AnamesaID + " and type_diagnosa ='E' ", DiagnosaPulang);
            }
            else
            {
                //'" + txAnjuran.Text.ToString() + @"', ////'" + txTerapiLanjtan.Text.ToString() + @"',  NOTED, 
                string sql = @"INSERT INTO KLINIK.cs_diagnosa ( RM_NO, INSP_DATE, ITEM_CD, TYPE_DIAGNOSA,  
                                      INS_DATE, INS_EMP, VISIT_NO,   ANAMNESA_ID ) VALUES ( 
                                    '" + _RM_NO + @"',
                                    '" + tgl_out + @"',
                                    '" + txt_diagnosa_akhir.EditValue.ToString() + @"',
                                    'E', 
                                    '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"',
                                    '" + DB.vUserId + @"',
                                    '" + visitid + @"', 
                                    " + _AnamesaID + @") ";
                save = ORADB.Execute(ORADB.XE, sql);
            }
        }

        private void bsavemedis_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvPelayanBidan.RowCount > 0)
                { 
                    string sql = " "; bool save = false; //insert all 
                    for (int i = 0; i < gvPelayanBidan.RowCount; i++)
                    {
                        string dte = "", detailid = "", spay = "";
                        object tgl = gvPelayanBidan.GetRowCellValue(i, "TANGGAL");
                        detailid = FN.strVal(gvPelayanBidan, i, "DETAIL_ID");
                        spay = FN.strVal(gvPelayanBidan, i, "PAY_STATUS");
                        if (tgl != null && tgl is DateTime)
                        {
                            DateTime selectedDateTime = (DateTime)tgl;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            DateTime selectedDateTime = DateTime.Now;
                            dte = selectedDateTime.ToString("yyyy-MM-dd");
                        }
                         
                        if (detailid.ToString().Equals(""))
                        {
                            sql = "";
                            sql = sql + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME) values ( ";
                            sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvPelayanBidan, i, "HEAD_ID") + "','" + FN.strVal(gvPelayanBidan, i, "TREAT_ITEM_ID") + "'  ,";
                            sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvPelayanBidan, i, "TREAT_QTY") + "', '" + FN.strVal(gvPelayanBidan, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvPelayanBidan, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvPelayanBidan, i, "TREAT_ITEM_PRICE")) + ", ";
                            sql = sql + " '" + FN.strVal(gvPelayanBidan, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvPelayanBidan, i, "JAM") + "' , 'gvPelayanBidan' )";
                        }
                        else
                        {
                            sql = "";
                            sql = sql + " update KLINIK.cs_treatment_detail  set treat_date =  TO_DATE('" + dte + "', 'yyyy-MM-dd'), TREAT_JAM = '" + FN.strVal(gvPelayanBidan, i, "JAM") + "', ";
                            sql = sql + "        remarks   = '" + FN.strVal(gvPelayanBidan, i, "REMARKS") + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                            sql = sql + "  where detail_id   = " + detailid + " ";
                        }
                        save = ORADB.Execute(ORADB.XE, sql);
                    }
                    //sql = sql + " select * from dual";
                    //bool save = ORADB.Execute(ORADB.XE, sql);
                    if (save)
                    {
                        MessageBox.Show("Data Pelayanan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void xtraTabControl3_Click(object sender, EventArgs e)
        {

        }

        private void baddnone_Click(object sender, EventArgs e)
        {
            if (dtVisitDokter == null) return;

            DataRow newRow = dtVisitDokter.NewRow();

            newRow["SEQ"] = ((gvVisitBidan.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            newRow["TANGGAL"] = DateTime.Now;
            newRow["JAM"] = DateTime.Now.ToString("HH:MM");

            dtVisitDokter.Rows.Add(newRow);

            gcVisitBidan.DataSource = dtVisitDokter;
        }

        private void bsavenone_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvVisitBidan.RowCount > 0)
                {
                    DataTable dt = ConnOra.Data_Table_ora("Select * from KLINIK.cs_treatment_detail where HEAD_ID = '" + headid + "'  "); 
                    if (dt != null && dt.Rows.Count > 0)
                    {  
                        string sql = ""; bool save = false;
                        for (int i = 0; i < gvVisitBidan.RowCount; i++)
                        {
                            string dte = "", detailid = "", spay = "";
                            object tgl = gvVisitBidan.GetRowCellValue(i, "TANGGAL");
                            detailid = FN.strVal(gvVisitBidan, i, "DETAIL_ID");
                            spay = FN.strVal(gvVisitBidan, i, "PAY_STATUS");

                            if (tgl != null && tgl is DateTime)
                            {
                                DateTime selectedDateTime = (DateTime)tgl;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                DateTime selectedDateTime = DateTime.Now;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }

                            if (detailid.ToString().Equals(""))
                            {
                                sql = "";
                                sql = sql + " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp, TREAT_JAM, GRID_NAME, ID_DOKTER) values ( ";
                                sql = sql + " CS_TREATMENT_DETAIL_SEQ.nextval ,'" + FN.strVal(gvVisitBidan, i, "HEAD_ID") + "','" + FN.strVal(gvVisitBidan, i, "TREAT_ITEM_ID") + "'  ,";
                                sql = sql + " TO_DATE('" + dte + "', 'yyyy-MM-dd'), '" + FN.strVal(gvVisitBidan, i, "TREAT_QTY") + "', '" + FN.strVal(gvVisitBidan, i, "TREAT_ITEM_PRICE") + "', " + Convert.ToInt32(FN.strVal(gvVisitBidan, i, "TREAT_QTY")) * Convert.ToInt32(FN.strVal(gvVisitBidan, i, "TREAT_ITEM_PRICE")) + ", ";
                                sql = sql + " '" + FN.strVal(gvVisitBidan, i, "REMARKS") + "' ,  sysdate, '" + DB.vUserId + "', '" + FN.strVal(gvVisitBidan, i, "JAM") + "' , 'gvVisitBidan' , '" + FN.strVal(gvVisitBidan, i, "ID_DOKTER") + "' )";
                            }
                            else
                            {
                                sql = "";
                                sql = sql + " update KLINIK.cs_treatment_detail  set treat_date =  TO_DATE('" + dte + "', 'yyyy-MM-dd'), TREAT_JAM = '" + FN.strVal(gvVisitBidan, i, "JAM") + "', ";
                                sql = sql + "        remarks   = '" + FN.strVal(gvVisitBidan, i, "REMARKS") + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                                sql = sql + "  where detail_id   = " + detailid + " ";
                            }
                            save = ORADB.Execute(ORADB.XE, sql); 
                        } 
                        if (save)
                        {
                            MessageBox.Show("Data Kunjungan Bidan Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void cb_ada_tindakan_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void baddmedis_Click(object sender, EventArgs e)
        {
            if (dtMedis == null) return;

            DataRow newRow = dtMedis.NewRow();

            newRow["SEQ"] = ((gvPelayanBidan.RowCount) + 1).ToString();
            newRow["HEAD_ID"] = headid;
            newRow["ID_VISIT"] = visitid;
            newRow["ACTION"] = "I";
            newRow["TANGGAL"] = DateTime.Now;
            newRow["JAM"] = DateTime.Now.ToString("HH:MM");
            dtMedis.Rows.Add(newRow);

            gcPelayanBidan.DataSource = dtMedis;
        }

        private void LoadDataResep()
        {
            string sql_med_load = "", s_que = "";

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM " +
                           " from KLINIK.cs_receipt a  " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  " +
                           " where b.status = 'A'  " +
                           " and rm_no = '" + _RM_NO + "'  " +
                           " and ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvMedisBidan' and a.f_active ='Y'  ";

            dtObat = ConnOra.Data_Table_ora(sql_med_load);

            gcMedisBidan.DataSource = null;
            gcMedisBidan.DataSource = dtObat;

            gvMedisBidan.OptionsView.ColumnAutoWidth = true;
            gvMedisBidan.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvMedisBidan.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvMedisBidan.IndicatorWidth = 30; 
            gvMedisBidan.BestFitColumns();

            gvMedisBidan.Columns[6].OptionsColumn.ReadOnly = true;
            gvMedisBidan.Columns[10].OptionsColumn.ReadOnly = true;

            RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
            glmed.DataSource = listMedicine;
            glmed.ValueMember = "medicineCode";
            glmed.DisplayMember = "medicineName";

            glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glmed.ImmediatePopup = true;
            glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glmed.NullText = "";
            gvMedisBidan.Columns[1].ColumnEdit = glmed; 

            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 ";
            DataTable dtf = ConnOra.Data_Table_ora(sql_for);

            listFormula.Clear();
            listFormula2.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            } 

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gvMedisBidan.Columns["FORMULA"].ColumnEdit = glfor;

            RepositoryItemTextEdit rpjam = new RepositoryItemTextEdit();
            rpjam.Mask.EditMask = "90:00";
            rpjam.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            gvMedisBidan.Columns["INS_JAM"].ColumnEdit = rpjam;

            RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
            rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gvMedisBidan.Columns["INSP_DATE"].ColumnEdit = rptanggal;

            RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gvMedisBidan.Columns["DOSIS"].ColumnEdit = dosisLookup;

            if (type_s == "B")
            {

                s_que = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd med_cd1, formula, type_drink,  " +
                               " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                               " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                               " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                               " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                               " med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis, a.insp_date, a.INS_JAM, a.cara" +
                               " from KLINIK.cs_receipt a  " +
                               " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  " +
                               " where b.status = 'A'  " +
                               //" and rm_no = '" + RMNO + "'  " +
                               " and ID_VISIT = '" + visitid + "' and GRID_NAME = 'gvObatUmumBidan' and a.f_active ='Y' ";

                DataTable dtUmum = ConnOra.Data_Table_ora(s_que);

                gObatUmumBidan.DataSource = null;
                gObatUmumBidan.DataSource = dtUmum;

                gvObatUmumBidan.OptionsView.ColumnAutoWidth = true;
                gvObatUmumBidan.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gvObatUmumBidan.Appearance.HeaderPanel.FontSizeDelta = 0;
                gvObatUmumBidan.IndicatorWidth = 30;
                gvObatUmumBidan.BestFitColumns();

                gvObatUmumBidan.Columns[6].OptionsColumn.ReadOnly = true;
                gvObatUmumBidan.Columns[10].OptionsColumn.ReadOnly = true;
                gvObatUmumBidan.Columns[1].ColumnEdit = glmed;
                gvObatUmumBidan.Columns["FORMULA"].ColumnEdit = glfor;
                gvObatUmumBidan.Columns["INSP_DATE"].ColumnEdit = rptanggal;
                gvObatUmumBidan.Columns["INS_JAM"].ColumnEdit = rpjam;
                gvObatUmumBidan.Columns["DOSIS"].ColumnEdit = dosisLookup;

            }


        }
    }
}
