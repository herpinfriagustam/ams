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
using Clinic.Class.Bpjsws;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic.Bpjs
{
    public partial class BpjswsAntreanBpjs : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb OracleConnection = new ConnectDb();
        bool InitState = true;

        public BpjswsAntreanBpjs()
        {
            InitializeComponent();
        }

        private void BpjswsAntreanBpjs_Load(object sender, EventArgs e)
        {
            txtConsId.Text = Bpjsws.CONS_ID;
            txtConsSecret.Text = Bpjsws.CONS_SECRET;
            txtUserKey.Text = Bpjsws.USER_KEY;
            txtAuthorization.Text = Bpjsws.AUTHORIZATION;
            txtUnixTime.Text = Bpjsws.CurrentUnixTime.ToString();

            txtPoliCheckDate.DateTime = 
            txtDokterCheckDate.DateTime =
            txtQueueAddCheckDate.DateTime = 
            txtQueueCancelCheckDate.DateTime =  
            txtQueueCallCheckDate.DateTime = DateTime.Now;

            grdPoli.DataSource = new List<ModelPoli>();
            grdDokter.DataSource = new List<ModelDokter>();
            grdAntreanAdd.DataSource = new DataTable();
            grdAntreanCancel.DataSource = new DataTable();
            grdAntreanCall.DataSource = new DataTable();

            LoadDataLookup();

            InitState = false;
        }

        private void LoadDataLookup()
        {
            string sql = $@"SELECT
	                            POLI_CD ,
	                            POLI_NAME,
	                            '[' || POLI_CD || '] ' || POLI_NAME POLI_DESCRIPTION,
	                            POLI_GROUP,
	                            POLI_PIC ,
	                            STATUS
                            FROM cs_policlinic
                            WHERE VISIBLE = 'Y'
                            ORDER BY POLI_CD";

            try
            {
                OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboPoli.Properties.DataSource = dt;
                cboQueueAddPoli.Properties.DataSource = dt;
                cboQueueCancelPoli.Properties.DataSource = dt;
                cboQueueCallPoli.Properties.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show("LoadDataLookup Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadData()
        {
            if(tab.SelectedTabPageIndex == 2) // add antrian - load data antrean
            {
                grdAntreanAdd.DataSource = null;

                string where = "";
                if (cboQueueAddPoli.EditValue != null)
                    where += $" AND A.POLI_CD = '{ cboQueueAddPoli.EditValue?.ToString() }'";

                if (cekQueueAddNotYetOnly.Checked)
                    where += " AND A.BPJSWS_STATUS = 0";

                string sql = $@"SELECT A.CALL_ID, 
	                                C.INSU_NO NO_KARTU_BPJS, 		C.NID NIK, 					C.PHONE NO_HP, 
	                                A.POLI_CD, 						d.POLI_NAME NAMA_POLI, 		E.RM_NO,
	                                TO_CHAR(A.INS_DATE, 'YYYY-MM-DD') TANGGAL_PERIKSA,
	                                G.ID_DOKTER KODE_DOKTER, 		G.NM_DOKTER  NAMA_DOKTER,
                                    CASE WHEN F.ID_JADWAL IS NULL THEN '' ELSE F.JAM_AWAL || ' ~ ' || F.JAM_AKHIR END JAM_PRAKTEK,
	                                A.QUE NOMOR_ANTREAN, 			TO_NUMBER(REGEXP_REPLACE(A.QUE, '[^0-9]', ''))	ANGKA_ANTREAN,
	                                '' KETERANGAN, 					a.BPJSWS_STATUS,            A.TYPE_INS,
	                                A.FLAG
                                  FROM CS_CALL_LOG A 
	                                LEFT JOIN CS_VISIT B ON A.QUE = B.QUE01
	                                LEFT JOIN CS_PATIENT_INFO C ON B.PATIENT_NO = C.PATIENT_NO 
	                                LEFT JOIN CS_POLICLINIC D ON A.POLI_CD  = D.POLI_CD 
	                                LEFT JOIN CS_PATIENT E ON C.PATIENT_NO  = E.PATIENT_NO
                                    LEFT JOIN CS_DOKTER_SCH F ON A.POLI_CD = F.POLI_CD
                                        AND TRUNC(F.TGL_JADWAL) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD')
                                        AND TO_CHAR(A.INS_DATE, 'HH24:MI') BETWEEN F.JAM_AWAL AND F.JAM_AKHIR
                                    LEFT JOIN CS_DOKTER G ON F.ID_DOKTER  = G.ID_DOKTER 
                                WHERE TRUNC(A.INS_DATE) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD') { where }
                               ORDER BY A.BPJSWS_STATUS DESC, A.INS_DATE";

                try
                {
                    OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    grdAntreanAdd.DataSource = dt;
                }
                catch (Exception ex)
                {
                    grdAntreanAdd.DataSource = new DataTable();
                    MessageBox.Show("LoadDataLookup Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tab.SelectedTabPageIndex == 3) // batal antrian - load data antrean
            {
                grdAntreanCancel.DataSource = null;

                string where = "";
                if (cboQueueAddPoli.EditValue != null)
                    where += $" AND A.POLI_CD = '{ cboQueueAddPoli.EditValue?.ToString() }'";

                if (cekQueueCancelNotYetOnly.Checked) where += " AND A.BPJSWS_STATUS = 1";
                else where += " AND (A.BPJSWS_STATUS = 1 OR A.BPJSWS_STATUS >= 1)";

                string sql = $@"SELECT A.CALL_ID, 
	                                C.INSU_NO NO_KARTU_BPJS, 		C.NID NIK, 					C.PHONE NO_HP, 
	                                A.POLI_CD, 						d.POLI_NAME NAMA_POLI, 		E.RM_NO,
	                                TO_CHAR(A.INS_DATE, 'YYYY-MM-DD') TANGGAL_PERIKSA,
	                                G.ID_DOKTER KODE_DOKTER, 		G.NM_DOKTER  NAMA_DOKTER,
                                    CASE WHEN F.ID_JADWAL IS NULL THEN '' ELSE F.JAM_AWAL || ' ~ ' || F.JAM_AKHIR END JAM_PRAKTEK,
	                                A.QUE NOMOR_ANTREAN, 			TO_NUMBER(REGEXP_REPLACE(A.QUE, '[^0-9]', ''))	ANGKA_ANTREAN,
	                                '' ALASAN, 					a.BPJSWS_STATUS,            A.TYPE_INS,
	                                A.FLAG
                                  FROM CS_CALL_LOG A 
	                                LEFT JOIN CS_VISIT B ON A.QUE = B.QUE01
	                                LEFT JOIN CS_PATIENT_INFO C ON B.PATIENT_NO = C.PATIENT_NO 
	                                LEFT JOIN CS_POLICLINIC D ON A.POLI_CD  = D.POLI_CD 
	                                LEFT JOIN CS_PATIENT E ON C.PATIENT_NO  = E.PATIENT_NO
                                    LEFT JOIN CS_DOKTER_SCH F ON A.POLI_CD = F.POLI_CD 
                                        AND TRUNC(F.TGL_JADWAL) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD')
                                        AND TO_CHAR(A.INS_DATE, 'HH24:MI') BETWEEN F.JAM_AWAL AND F.JAM_AKHIR
                                    LEFT JOIN CS_DOKTER G ON F.ID_DOKTER  = G.ID_DOKTER 
                                WHERE TRUNC(A.INS_DATE) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD') { where }
                               ORDER BY A.BPJSWS_STATUS DESC, A.INS_DATE";

                try
                {
                    OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    grdAntreanCancel.DataSource = dt;
                }
                catch (Exception ex)
                {
                    grdAntreanCancel.DataSource = new DataTable();
                    MessageBox.Show("LoadDataLookup Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tab.SelectedTabPageIndex == 4) // Panggil antrian - load data antrean
            {
                grdAntreanCall.DataSource = null;

                string where = "";
                if (cboQueueAddPoli.EditValue != null)
                    where += $" AND A.POLI_CD = '{ cboQueueAddPoli.EditValue?.ToString() }'";

                if (cekQueueCallNotYetOnly.Checked) where += " AND A.BPJSWS_STATUS = 1";
                else where += " AND (A.BPJSWS_STATUS = 1 OR A.BPJSWS_STATUS >= 1)";

                string sql = $@"SELECT A.CALL_ID, 
	                                C.INSU_NO NO_KARTU_BPJS, 		C.NID NIK, 					C.PHONE NO_HP, 
	                                A.POLI_CD, 						d.POLI_NAME NAMA_POLI, 		E.RM_NO,
	                                TO_CHAR(A.INS_DATE, 'YYYY-MM-DD') TANGGAL_PERIKSA,
	                                G.ID_DOKTER KODE_DOKTER, 		G.NM_DOKTER  NAMA_DOKTER,
                                    CASE WHEN F.ID_JADWAL IS NULL THEN '' ELSE F.JAM_AWAL || ' ~ ' || F.JAM_AKHIR END JAM_PRAKTEK,
	                                A.QUE NOMOR_ANTREAN, 			TO_NUMBER(REGEXP_REPLACE(A.QUE, '[^0-9]', ''))	ANGKA_ANTREAN,
	                                '' KETERANGAN, 					a.BPJSWS_STATUS,            A.TYPE_INS,
	                                A.FLAG
                                  FROM CS_CALL_LOG A 
	                                LEFT JOIN CS_VISIT B ON A.QUE = B.QUE01
	                                LEFT JOIN CS_PATIENT_INFO C ON B.PATIENT_NO = C.PATIENT_NO 
	                                LEFT JOIN CS_POLICLINIC D ON A.POLI_CD  = D.POLI_CD 
	                                LEFT JOIN CS_PATIENT E ON C.PATIENT_NO  = E.PATIENT_NO
                                    LEFT JOIN CS_DOKTER_SCH F ON A.POLI_CD = F.POLI_CD 
                                        AND TRUNC(F.TGL_JADWAL) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD')
                                        AND TO_CHAR(A.INS_DATE, 'HH24:MI') BETWEEN F.JAM_AWAL AND F.JAM_AKHIR
                                    LEFT JOIN CS_DOKTER G ON F.ID_DOKTER  = G.ID_DOKTER 
                                WHERE TRUNC(A.INS_DATE) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD') 
                                    AND A.BPJSWS_STATUS >= 1 { where }
                               ORDER BY A.BPJSWS_STATUS DESC, A.INS_DATE";

                try
                {
                    OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    grdAntreanCall.DataSource = dt;
                }
                catch (Exception ex)
                {
                    grdAntreanCall.DataSource = new DataTable();
                    MessageBox.Show("LoadDataLookup Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtUnixTime_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            txtUnixTime.Text = Bpjsws.CurrentUnixTime.ToString();
        }

        private void txtSignature_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if(txtConsId.Text == "" || txtConsSecret.Text == "" || txtUnixTime.Text == "")
            {
                MessageBox.Show("Consumer ID, Consumer Secreat and Time (Unix) is required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cekAutoUnixTime.Checked) txtUnixTime.PerformClick(txtUnixTime.Properties.Buttons[0]);

            string signature = Bpjsws.CreateSignature(txtConsId.Text, txtConsSecret.Text, txtUnixTime.Text);
            txtSignature.Text = signature;

            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtConsId.Text == "" || txtSignature.Text == "" || txtUnixTime.Text == "" || txtUserKey.Text == "")
            {
                MessageBox.Show("Consumer ID, Signature, User Key and Time (Unix) is required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-cons-id",  txtConsId.Text },
                { "x-timestamp",  txtUnixTime.Text },
                { "x-signature",  txtSignature.Text },
                { "user_key",  txtUserKey.Text },
            };


            if (tab.SelectedTabPageIndex == 0) // referensi poli
            {
                
                Cursor = Cursors.WaitCursor;

                grdPoli.DataSource = null;

                string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL.Replace(@"{tanggal}", txtPoliCheckDate.Text);
                BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, Bpjsws.HttpMethodMode.Get, Bpjsws.PostDataType.Json, headers);
                txtPoliResponseString.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                Cursor = Cursors.Default;
                if(response != null)
                {
                    if(response.Metadata.Code == 200)
                    {
                        string key = txtConsId.Text + txtConsSecret.Text + txtUnixTime.Text;
                        string decData = Bpjsws.Decrypt(key, response.Response);
                        List<ModelPoli> listPoli = BpjswsResponseConvert.Convert<List<ModelPoli>>(decData);

                        grdPoli.DataSource = listPoli;
                    }
                    else
                    {
                        grdPoli.DataSource = new List<ModelPoli>();
                        MessageBox.Show($"code: { response.Metadata.Code + "" } Message: { response.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show($"Tidak ada response, silahkan hubungi administrator!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tab.SelectedTabPageIndex == 1) // referensi dokter
            {
                
                if(cboPoli.EditValue == null)
                {
                    MessageBox.Show("Kode Poli is required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                Cursor = Cursors.WaitCursor;

                grdDokter.DataSource = null;

                string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL
                    .Replace(@"{kodepoli}",cboPoli.EditValue?.ToString())
                    .Replace(@"{tanggal}", txtDokterCheckDate.Text);
                BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, Bpjsws.HttpMethodMode.Get, Bpjsws.PostDataType.Json, headers);
                txtDokterResponseString.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                Cursor = Cursors.Default;
                if (response != null)
                {
                    if (response.Metadata.Code == 200)
                    {
                        string key = txtConsId.Text + txtConsSecret.Text + txtUnixTime.Text;
                        string decData = Bpjsws.Decrypt(key, response.Response);
                        List<ModelDokter> listDokter = BpjswsResponseConvert.Convert<List<ModelDokter>>(decData);

                        grdDokter.DataSource = listDokter;
                    }
                    else
                    {
                        grdDokter.DataSource = new List<ModelDokter>();
                        MessageBox.Show($"code: { response.Metadata.Code + "" } Message: { response.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show($"Tidak ada response, silahkan hubungi administrator!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tab.SelectedTabPageIndex == 2) // tambah antrian
            {
                DataRow row = gvwAntreanAdd.GetFocusedDataRow();
                if(row == null)
                {
                    MessageBox.Show("Please select a record!");
                    return;
                }

                string bpjswsStatus = row["BPJSWS_STATUS"]?.ToString();
                if(bpjswsStatus != "0")
                {
                    MessageBox.Show("Status WS Harus 0 (Antrean belum dikirim ke BPJS WS)", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (bpjswsStatus == "1")
                {
                    MessageBox.Show("Antrean sudah terkirim", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                JObject json = new JObject();
                json["nomorkartu"] = row["NO_KARTU_BPJS"]?.ToString();
                json["nik"] = row["NIK"]?.ToString();
                json["nohp"] = row["NO_HP"]?.ToString();
                json["kodepoli"] = row["POLI_CD"]?.ToString();
                json["namapoli"] = row["NAMA_POLI"]?.ToString();
                json["norm"] = row["RM_NO"]?.ToString();
                json["tanggalperiksa"] = row["TANGGAL_PERIKSA"]?.ToString();
                json["kodedokter"] = row["KODE_DOKTER"]?.ToString();
                json["namadokter"] = row["NAMA_DOKTER"]?.ToString();
                json["jampraktek"] = row["JAM_PRAKTEK"]?.ToString();
                json["nomorantrean"] = row["NOMOR_ANTREAN"]?.ToString();
                json["angkaantrean"] = row["ANGKA_ANTREAN"]?.ToString();
                json["keterangan"] = row["KETERANGAN"]?.ToString();

                string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL;
                BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, Bpjsws.HttpMethodMode.Post, Bpjsws.PostDataType.Json, headers, new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });
                txtDokterResponseString.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                Cursor = Cursors.Default;
                if (response != null)
                {
                    if (response.Metadata.Code == 200)
                    {
                        string sql = $"UPDATE CS_CALL_LOG SET BPJSWS_STATUS = 1 WHERE CALL_ID = { row["CALL_ID"]?.ToString() }";

                        OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                        try
                        {
                            connection.Open();

                            OleDbCommand cmd = new OleDbCommand(sql, connection);
                            cmd.ExecuteNonQuery();

                            connection.Close();

                            LoadData();
                            MessageBox.Show("Penambahan antrean telah dikirim ke BPJS WS!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            if (connection.State == ConnectionState.Open) connection.Close();
                            LoadData();
                            MessageBox.Show("Update Call log Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"code: { response.Metadata.Code + "" } Message: { response.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show($"Tidak ada response, silahkan hubungi administrator!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tab.SelectedTabPageIndex == 3) // batal antrian
            {
                DataRow row = gvwAntreanCancel.GetFocusedDataRow();
                if (row == null)
                {
                    MessageBox.Show("Please select a record!");
                    return;
                }

                string bpjswsStatus = row["BPJSWS_STATUS"]?.ToString();
                if (bpjswsStatus != "1")
                {
                    MessageBox.Show("Status WS Harus 1 (Antrian sudah dikirim ke BPJS WS)", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (bpjswsStatus == "-1")
                {
                    MessageBox.Show("Antrean sudah dikirim untuk dibatalkan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                JObject json = new JObject();
                json["tanggalperiksa"] = row["TANGGAL_PERIKSA"]?.ToString();
                json["kodepoli"] = row["POLI_CD"]?.ToString();
                json["nomorkartu"] = row["NO_KARTU_BPJS"]?.ToString();
                json["alasan"] = row["ALASAN"]?.ToString();

                string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL;
                BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, Bpjsws.HttpMethodMode.Post, Bpjsws.PostDataType.Json, headers, new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });
                txtDokterResponseString.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                Cursor = Cursors.Default;
                if (response != null)
                {
                    if (response.Metadata.Code == 200)
                    {
                        string sql = $"UPDATE CS_CALL_LOG SET BPJSWS_STATUS = -1 WHERE CALL_ID = { row["CALL_ID"]?.ToString() }";

                        OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                        try
                        {
                            connection.Open();

                            OleDbCommand cmd = new OleDbCommand(sql, connection);
                            cmd.ExecuteNonQuery();

                            connection.Close();

                            LoadData();
                            MessageBox.Show("Pembatalan antrean telah dikirim ke BPJS WS!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            if (connection.State == ConnectionState.Open) connection.Close();
                            LoadData();
                            MessageBox.Show("Cancel Call log Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"code: { response.Metadata.Code + "" } Message: { response.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show($"Tidak ada response, silahkan hubungi administrator!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tab.SelectedTabPageIndex == 4) // panggil antrian
            {
                DataRow row = gvwAntreanCall.GetFocusedDataRow();
                if (row == null)
                {
                    MessageBox.Show("Please select a record!");
                    return;
                }

                string bpjswsStatus = row["BPJSWS_STATUS"]?.ToString();
                if (bpjswsStatus != "1")
                {
                    MessageBox.Show("Status WS Harus 1 (Antrian sudah dikirim ke BPJS WS)", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (bpjswsStatus == "2")
                {
                    MessageBox.Show("Antrean sudah terkirim", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                string typeIns = row["TYPE_INS"]?.ToString();

                JObject json = new JObject();
                json["tanggalperiksa"] = row["TANGGAL_PERIKSA"]?.ToString();
                json["kodepoli"] = row["POLI_CD"]?.ToString();
                json["nomorkartu"] = row["NO_KARTU_BPJS"]?.ToString();
                json["status"] = typeIns == "CAN" ? 0 : 1;
                json["waktu"] = Bpjsws.CurrentUnixTime;

                string url = Bpjsws.WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL;
                BpjswsResponse response = Bpjsws.Request<BpjswsResponse>(url, Bpjsws.HttpMethodMode.Post, Bpjsws.PostDataType.Json, headers, new Dictionary<string, string>
                {
                    { "RAW", json.ToString() }
                });
                txtDokterResponseString.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                Cursor = Cursors.Default;
                if (response != null)
                {
                    if (response.Metadata.Code == 200)
                    {
                        string sql = $"UPDATE CS_CALL_LOG SET BPJSWS_STATUS = 2 WHERE CALL_ID = { row["CALL_ID"]?.ToString() }";

                        OleDbConnection connection = OracleConnection.Create_Connect_Ora();
                        try
                        {
                            connection.Open();

                            OleDbCommand cmd = new OleDbCommand(sql, connection);
                            cmd.ExecuteNonQuery();

                            connection.Close();

                            LoadData();
                            MessageBox.Show("Update antrean telah dikirim ke BPJS WS!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            if (connection.State == ConnectionState.Open) connection.Close();
                            LoadData();
                            MessageBox.Show("Update Call log Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"code: { response.Metadata.Code + "" } Message: { response.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show($"Tidak ada response, silahkan hubungi administrator!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gvwPoli_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            EmptyGrid(grdPoli, e);
        }

        private void gvwDokter_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            EmptyGrid(grdDokter, e);
        }

        private void gvwAntreanAdd_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            EmptyGrid(grdAntreanAdd, e);
        }

        private void gvwAntreanCancel_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            EmptyGrid(grdAntreanCancel, e);
        }

        private void gvwAntreanCall_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            EmptyGrid(grdAntreanCall, e);
        }

        private void EmptyGrid(GridControl grid, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            GridView gvw = grid.MainView as GridView;
            Console.WriteLine(grid.Name + " DataRowCount " + gvw.DataRowCount);
            if (grid.DataSource != null && gvw.DataRowCount == 0)
            {
                // Menggambar teks atau gambar untuk menunjukkan loading
                string loadingText = "Data kosong";
                Font font = new Font("Tahoma", 8.25f, FontStyle.Bold);
                Color textColor = Color.Gray;

                // Menggambar teks
                e.Graphics.DrawString(loadingText, font, new SolidBrush(textColor),
                    (e.Bounds.Width - e.Graphics.MeasureString(loadingText, font).Width) / 2,
                    (e.Bounds.Height - font.Height) / 2);
            }
            else if (grid.DataSource == null)
            {
                // Menggambar teks atau gambar untuk menunjukkan loading
                string loadingText = "Data sedang dimuat...";
                Font font = new Font("Tahoma", 8.25f, FontStyle.Bold);
                Color textColor = Color.Gray;

                // Menggambar teks
                e.Graphics.DrawString(loadingText, font, new SolidBrush(textColor),
                    (e.Bounds.Width - e.Graphics.MeasureString(loadingText, font).Width) / 2,
                    (e.Bounds.Height - font.Height) / 2);
            }
        }

        private void cboAntreanAddPoli_EditValueChanged(object sender, EventArgs e)
        {
            if(InitState == false)
                LoadData();
        }

        private void gvwAntreanAdd_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if(e.RowHandle >= 0)
            {
                DataRow row = gvwAntreanAdd.GetDataRow(e.RowHandle);
                if(row != null)
                {
                    string bpjswsStatus = row["BPJSWS_STATUS"]?.ToString();
                    if(bpjswsStatus == "0")
                        e.Appearance.BackColor = Color.LightPink;
                }
            } 
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Control_EditValueChanged(object sender, EventArgs e)
        {
            if (InitState == false)
                LoadData();
        }

        private void Cek_CheckedChanged(object sender, EventArgs e)
        {
            if(InitState == false)
                LoadData();
        }
    }
}