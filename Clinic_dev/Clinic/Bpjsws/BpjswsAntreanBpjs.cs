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
            txtAntreanCancelCheckDate.DateTime =  
            txtQueueCallCheckDate.DateTime = DateTime.Now;

            grdPoli.DataSource = new List<ModelPoli>();
            grdDokter.DataSource = new List<ModelDokter>();

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
                cboAntreanCancelPoli.Properties.DataSource = dt;
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
                string where = "";
                if (cboQueueAddPoli.EditValue != null)
                    where = $" AND A.POLI_CD = '{ cboQueueAddPoli.EditValue?.ToString() }'";

                string sql = $@"SELECT A.CALL_ID, 
	                                C.INSU_NO NO_KARTU_BPJS, 		C.NID NIK, 					C.PHONE NO_HP, 
	                                A.POLI_CD, 						d.POLI_NAME NAMA_POLI, 		E.RM_NO,
	                                TO_CHAR(A.INS_DATE, 'YYYY-MM-DD') TANGGAL_PERIKSA,
	                                '' KODE_DOKTER, 				'' NAMA_DOKTER, 			'' JAM_PRAKTEK,
	                                A.QUE NOMOR_ANTREAN, 			REGEXP_REPLACE(A.QUE, '[^0-9]', '')	ANGKA_ANTREAN,
	                                '' KETERANGAN, 					a.BPJSWS_STATUS
                                  FROM CS_CALL_LOG A 
	                                LEFT JOIN CS_VISIT B ON A.QUE = B.QUE01
	                                LEFT JOIN CS_PATIENT_INFO C ON B.PATIENT_NO = C.PATIENT_NO 
	                                LEFT JOIN CS_POLICLINIC D ON A.POLI_CD  = D.POLI_CD 
	                                LEFT JOIN CS_PATIENT E ON C.PATIENT_NO  = E.PATIENT_NO 
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
                    MessageBox.Show("LoadDataLookup Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (tab.SelectedTabPageIndex == 4) // Panggil antrian - load data antrean
            {
                string where = "";
                if (cboQueueAddPoli.EditValue != null)
                    where = $" AND A.POLI_CD = '{ cboQueueAddPoli.EditValue?.ToString() }'";

                string sql = $@"SELECT 
	                                C.INSU_NO NO_KARTU_BPJS, 		C.NID NIK, 					C.PHONE NO_HP, 
	                                A.POLI_CD, 						d.POLI_NAME NAMA_POLI, 		E.RM_NO,
	                                TO_CHAR(A.INS_DATE, 'YYYY-MM-DD') TANGGAL_PERIKSA,
	                                '' KODE_DOKTER, 				'' NAMA_DOKTER, 			'' JAM_PRAKTER,
	                                A.QUE NOMOR_ANTRIAN, 			REGEXP_REPLACE(A.QUE, '[^0-9]', '')	ANGKA_ANTREAN,
	                                '' KETERANGAN, 					a.BPJSWS_STATUS
                                  FROM CS_CALL_LOG A 
	                                LEFT JOIN CS_VISIT B ON A.QUE = B.QUE01
	                                LEFT JOIN CS_PATIENT_INFO C ON B.PATIENT_NO = C.PATIENT_NO 
	                                LEFT JOIN CS_POLICLINIC D ON A.POLI_CD  = D.POLI_CD 
	                                LEFT JOIN CS_PATIENT E ON C.PATIENT_NO  = E.PATIENT_NO 
                                WHERE TRUNC(A.INS_DATE) = TO_DATE('{ txtQueueAddCheckDate.DateTime.ToString("yyyy-MM-dd") }', 'YYYY-MM-DD')
                                    AND A.BPJSWS_STATUS = 1 { where }
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
                        LoadData();
                        MessageBox.Show("Pembatalan antrean telah dikirim ke BPJS WS!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                Cursor = Cursors.WaitCursor;

                JObject json = new JObject();
                json["tanggalperiksa"] = row["TANGGAL_PERIKSA"]?.ToString();
                json["kodepoli"] = row["POLI_CD"]?.ToString();
                json["nomorkartu"] = row["NO_KARTU_BPJS"]?.ToString();
                json["status"] = row["STATUS"]?.ToString();
                json["waktu"] = row["UNIX_TIME"]?.ToString();

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
                        LoadData();
                        MessageBox.Show("Panggil antrean telah dikirim ke BPJS WS!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (grdPoli.DataSource != null && gvwPoli.RowCount == 0)
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
            else if (grdPoli.DataSource == null)
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

        private void gvwDokter_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (grdDokter.DataSource != null && gvwDokter.RowCount == 0)
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
            else if (grdDokter.DataSource == null)
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

        private void gvwAntreanAdd_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (grdAntreanAdd.DataSource != null && gvwAntreanAdd.RowCount == 0)
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
            else if (grdAntreanAdd.DataSource == null)
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

        private void gvwAntreanCall_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (grdAntreanCall.DataSource != null && gvwAntreanCall.RowCount == 0)
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
            else if (grdAntreanCall.DataSource == null)
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

        private void btnAntreanAddReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtAntreanAddCheckDate_EditValueChanged(object sender, EventArgs e)
        {
            if (InitState == false)
                LoadData();
        }

        private void cboQueueCallPoli_EditValueChanged(object sender, EventArgs e)
        {
            if (InitState == false)
                LoadData();
        }

        private void txtQueueCallCheckDate_EditValueChanged(object sender, EventArgs e)
        {
            if (InitState == false)
                LoadData();
        }

        private void btnQueueCallReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        
    }
}