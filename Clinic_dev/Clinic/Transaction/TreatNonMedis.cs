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
using DevExpress.XtraGrid.Columns;

namespace Clinic
{
    public partial class TreatNonMedis : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<FlagYn> gender = new List<FlagYn>();
        List<Medicine> listMedicine = new List<Medicine>();
        DataTable dtGlMed = new DataTable();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Stat> listType = new List<Stat>();
        List<Stat> listKir = new List<Stat>();
        public string  v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string type = "", sdate = "", edate = "";
        RepositoryItemLookUpEdit LookType = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit LookLynan = new RepositoryItemLookUpEdit();
        //string today = "2019-11-27";

        public TreatNonMedis()
        {
            InitializeComponent();
            foreach (GridColumn column in gridView1.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void MasterFormula_Load(object sender, EventArgs e)
        {
            //string sql_date = "";
            //sql_date = " select to_char(sysdate,'yyyy-mm-dd') sdate, to_char(sysdate,'yyyy-mm-dd') edate from dual ";

            //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            //DataTable dt = new DataTable();
            //adOra.Fill(dt);

            //sdate = dt.Rows[0]["sdate"].ToString();
            //edate = dt.Rows[0]["edate"].ToString();
            dDateBgn.Text = today;
            dDateEnd.Text = today;
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "TreatNonMedis");
            initData();
            loadData();
        }

        private void initData()
        {
            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) || ' : ' || to_char(treat_item_price) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and treat_group_id in ('TRG08','TRG03')  and TREAT_TYPE_ID = 'TRT01' ";

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            gender.Clear();
            gender.Add(new FlagYn() { flagCode = "L", flagName = "Laki-Laki" });
            gender.Add(new FlagYn() { flagCode = "P", flagName = "Perempuan" });

            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "N", flagName = "Normal" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "P", flagName = "Parsial" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "T", flagName = "Total" }); 

            listType.Clear();
            listType.Add(new Stat() { statCode = "KIR", statName = "KIR" });
            listType.Add(new Stat() { statCode = "MCU", statName = "MCU" });
        }

        private void btnLoadDosis_Click(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, kir_id, regis_date regis_date, ";
            sql_search = sql_search + Environment.NewLine + "nid, name, gender, birth_place, birth_date  birth_date, ";
            sql_search = sql_search + Environment.NewLine + "addrs, jobs, purpose, height, weight, blood_press, d_now, d_his, eye_status, ";
            sql_search = sql_search + Environment.NewLine + "ID_ITEM_LAYANAN Layanan, f_type, decode(STAT_PAY,'Y','Closed','Belum Bayar') STAT_PAY ";
            sql_search = sql_search + Environment.NewLine + "from cs_kir a, cs_treatment_item b ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 and STAT_PAY <> 'X' and a.ID_ITEM_LAYANAN = b.treat_item_id  ";
            if(radKIR.Checked)
                sql_search = sql_search + Environment.NewLine + "  AND f_type ='KIR'  ";
            else
                sql_search = sql_search + Environment.NewLine + "  AND f_type ='MCU'  ";
            sql_search = sql_search + Environment.NewLine + "and trunc(regis_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "order by regis_date, name ";

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

                gridView1.FixedLineWidth = 6;
                gridView1.Columns[18].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "KIR ID";
                gridView1.Columns[2].Caption = "Tgl Regis";
                gridView1.Columns[3].Caption = "No KTP";
                gridView1.Columns[4].Caption = "Nama";
                gridView1.Columns[5].Caption = "JK";
                gridView1.Columns[6].Caption = "TempatLahir";
                gridView1.Columns[7].Caption = "Tgl Lahir";
                gridView1.Columns[8].Caption = "Alamat";
                gridView1.Columns[9].Caption = "Pekerjaan";
                gridView1.Columns[10].Caption = "Keperluan";
                gridView1.Columns[11].Caption = "TB";
                gridView1.Columns[12].Caption = "BB";
                gridView1.Columns[13].Caption = "Tek.Darah";
                gridView1.Columns[14].Caption = "P.Sekarang";
                gridView1.Columns[15].Caption = "P.Dahulu";
                gridView1.Columns[16].Caption = "Status Mata";
                gridView1.Columns[17].Caption = "Layanan";
                gridView1.Columns[18].Caption = "Type";
                gridView1.Columns[19].Caption = "Status Pay";

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[19].Visible = true ;
                //gridView1.Columns[1].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[16].OptionsColumn.ReadOnly = true;

                gridView1.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns[2].DisplayFormat.FormatString = "yyyy-MM-dd";

                gridView1.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns[7].DisplayFormat.FormatString = "yyyy-MM-dd";

                RepositoryItemLookUpEdit genderLookup = new RepositoryItemLookUpEdit();
                genderLookup.DataSource = gender;
                genderLookup.ValueMember = "flagCode";
                genderLookup.DisplayMember = "flagName";

                genderLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                genderLookup.DropDownRows = gender.Count;
                genderLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                genderLookup.AutoSearchColumnIndex = 1;
                genderLookup.NullText = "";
                gridView1.Columns[5].ColumnEdit = genderLookup;

                RepositoryItemLookUpEdit mataLookup = new RepositoryItemLookUpEdit();
                mataLookup.DataSource = diagnosaStatus;
                mataLookup.ValueMember = "flagCode";
                mataLookup.DisplayMember = "flagName";

                mataLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                mataLookup.DropDownRows = diagnosaStatus.Count;
                mataLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                mataLookup.AutoSearchColumnIndex = 1;
                mataLookup.NullText = "";
                gridView1.Columns[16].ColumnEdit = mataLookup;

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) || ' : ' || to_char(treat_item_price) treat_item_name ";
                SQL = SQL + Environment.NewLine + "from cs_treatment_item ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                if (radKIR.Checked)
                    SQL = SQL + Environment.NewLine + "and treat_group_id ='TRG13'  ";
                else
                    SQL = SQL + Environment.NewLine + "and treat_group_id = 'TRG03' ";
                SQL = SQL + Environment.NewLine + "  and TREAT_TYPE_ID = 'TRT01' order by 2";

                OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
                DataTable dtly = new DataTable();
                adOraly.Fill(dtly);
                listLaya2.Clear();
                for (int i = 0; i < dtly.Rows.Count; i++)
                {
                    listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
                }

                RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                glLaya.DataSource = listLaya2;
                glLaya.ValueMember = "layananCode";
                glLaya.DisplayMember = "layananName";

                glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glLaya.ImmediatePopup = true;
                glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glLaya.NullText = "";
                gridView1.Columns[17].ColumnEdit = glLaya; 

                LookType.DataSource = listType;
                LookType.ValueMember = "statCode";
                LookType.DisplayMember = "statName";

                LookType.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                LookType.DropDownRows = listType.Count + 1;
                LookType.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                LookType.AutoSearchColumnIndex = 1;
                LookType.NullText = "";
                gridView1.Columns[18].ColumnEdit = LookType;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnAddDosis_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            gridView1.Columns[2].OptionsColumn.ReadOnly = false;
            gridView1.AddNewRow();
        }
        
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            //gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            if (radKIR.Checked)
                view.SetRowCellValue(e.RowHandle, view.Columns[18], "KIR");
            else
                view.SetRowCellValue(e.RowHandle, view.Columns[18], "MCU");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "",  pay_sts ="", p_kirid = "", tglregis = "";
            string p_class = "", p_tgl = "", p_ktp="", p_nama = "", pjk = "", p_status = "", p_attr="", p_action = "", p_layanan="", tglahir = "";
            DateTime parsedDate;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                 
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_kirid = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_layanan = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                p_class = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                p_tgl = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_ktp = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                pjk = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                pay_sts = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();
                tglahir = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();

                if (p_layanan == "")
                {
                    MessageBox.Show("Item Layanan harus diisi");
                }
                else if (p_class == "")
                {
                    MessageBox.Show("Type harus di Tentukan");
                }
                else if (p_nama == "")
                {
                    MessageBox.Show("Nama harus diisi");
                } 
                else
                {
                    parsedDate = DateTime.Parse(p_tgl);
                    tglregis = parsedDate.ToString("yyyy-MM-dd");

                    parsedDate = DateTime.Parse(tglahir);
                    tglahir = parsedDate.ToString("yyyy-MM-dd");

                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_kir (KIR_ID, REGIS_DATE, NID, NAME, GENDER, BIRTH_PLACE, BIRTH_DATE, ADDRS, JOBS, PURPOSE, HEIGHT, WEIGHT, BLOOD_PRESS, D_NOW, D_HIS, EYE_STATUS,ID_ITEM_LAYANAN,F_TYPE,INS_DATE,INS_EMP ) values ";
                        sql_insert = sql_insert + " (KLINIK.CS_KIR_SEQ.nextval, to_date('" + tglregis.ToString()  + "','yyyy-MM-dd'), '" + p_ktp + "', '" + p_nama + "', '" + pjk + "', ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString() + "', to_date('" + tglahir.ToString() + "','yyyy-MM-dd'), ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "', ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString() + "', '" + gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString() + "', ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString() + "', '" + gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString() + "', ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString() + "', '" + gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString() + "', ";
                        sql_insert = sql_insert + " '" + gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString() + "', '" + p_layanan + "', ";
                        sql_insert = sql_insert + " '" + p_class + "', sysdate, '" + DB.vUserId + "')"; 

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose(); 

                            MessageBox.Show("Data Berhasil ditambah");
                            loadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U" && pay_sts == "N")
                    {
                        sql_update = " ";

                        sql_update = sql_update + " update cs_kir set REGIS_DATE =  to_date('" + tglregis.ToString() + "','yyyy-MM-dd'), NID = '" + p_ktp + "', NAME = '" + p_nama + "', GENDER = '" + pjk + "', ";
                        sql_update = sql_update + "        BIRTH_PLACE  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString() + "', BIRTH_DATE =  to_date('" + tglahir.ToString() + "','yyyy-MM-dd'), ";
                        sql_update = sql_update + "        ADDRS  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', JOBS = '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "', ";
                        sql_update = sql_update + "        PURPOSE  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', HEIGHT = '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "', ";
                        sql_update = sql_update + "        WEIGHT  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', BLOOD_PRESS = '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "', ";
                        sql_update = sql_update + "        D_NOW  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', D_HIS = '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "', ";
                        sql_update = sql_update + "        EYE_STATUS  = '" + gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString() + "', ID_ITEM_LAYANAN = '" + gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString() + "' "; 
                        sql_update = sql_update + " where  KIR_ID = '" + p_kirid + "' and F_TYPE = '" + p_class + "' ";

                        try
                        {
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                            oraConnect2.Open();
                            cm2.ExecuteNonQuery();
                            oraConnect2.Close();
                            cm2.Dispose();
                             
                            MessageBox.Show("Data Berhasil dirubah");
                            loadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
           
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveDosis.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[18]).ToString();
            string sqlkir = "", sttype ="";

            if (e.Column.Caption == "Type" && ((a.ToString() == "KIR") || (a.ToString() == "MCU")))
            {
                if (a.ToString() == "KIR")
                    sttype = "TRG13";
                else
                    sttype = "TRG03";

                sqlkir = " ";
                sqlkir = sqlkir + " select treat_item_id, initcap(treat_item_name) || ' : ' || to_char(treat_item_price) treat_item_name  from KLINIK.cs_treatment_item a ";
                sqlkir = sqlkir + "  where status = 'A'  and TREAT_TYPE_ID = 'TRT01' and TREAT_GROUP_ID = '" + sttype + "' ORDER BY 2";

                OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraly = new OleDbDataAdapter(sqlkir, oraConnectly);
                DataTable dtly = new DataTable();
                adOraly.Fill(dtly);
                listLaya2.Clear();
                for (int i = 0; i < dtly.Rows.Count; i++)
                {
                    listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
                }

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sqlkir, oraConnectf);
                //DataTable dt3 = new DataTable();
                //adOraf.Fill(dt3);
                //listLaya2.Clear();
                //for (int i = 0; i < dt3.Rows.Count; i++)
                //{
                //    listLaya2.Add(new Layanan() { layananCode = dt3.Rows[i]["treat_item_id"].ToString(), layananName = dt3.Rows[i]["treat_item_name"].ToString() }); 
                //}

                //LookLynan.DataSource = listLaya2;
                //LookLynan.ValueMember = "statCode";
                //LookLynan.DisplayMember = "statName";

                //LookLynan.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //LookLynan.DropDownRows = listLaya2.Count + 1;
                //LookLynan.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //LookLynan.AutoSearchColumnIndex = 1;
                //LookLynan.NullText = "";
                //gridView1.Columns[16].ColumnEdit = LookLynan; 
            }

            if (e.Column.Caption == "Code Class ID" || e.Column.Caption == "Code ID" || e.Column.Caption == "Code Nm" || e.Column.Caption == "Order")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                }
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Code Class ID" || e.Column.Caption == "Code ID" || e.Column.Caption == "Code Nm" || e.Column.Caption == "Order")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void radMCU_CheckedChanged(object sender, EventArgs e)
        {
            //loadData();
        }

        private void radKIR_CheckedChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnDelDosis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", p_class = "", p_kode = "";

                p_class = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                p_kode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update cs_kir set STAT_PAY = 'X', UPD_EMP = '" + DB.vUserId + "', UPD_DATE = sysdate ";
                sql_delete = sql_delete + " where KIR_ID = '" + p_class + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "dosis_obat.xls",
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