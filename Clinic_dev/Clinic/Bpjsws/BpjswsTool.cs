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
using Newtonsoft.Json.Linq;
using Clinic.Class.Bpjsws;

namespace Clinic.Bpjsws
{
    public partial class BpjswsTool : DevExpress.XtraEditors.XtraForm
    {
        DataTable DtApiCatalog;
        bool InitState;

        public BpjswsTool()
        {
            InitializeComponent();
            InitializeApiCatalog();
        }

        private void BpjswsTool_Load(object sender, EventArgs e)
        {
            txtConsId.Text = Clinic.Class.Bpjsws.Bpjsws.CONS_ID;
            txtConsSecret.Text = Clinic.Class.Bpjsws.Bpjsws.CONS_SECRET;
            txtUserKey.Text = Clinic.Class.Bpjsws.Bpjsws.USER_KEY;
            txtAuthorization.Text = Clinic.Class.Bpjsws.Bpjsws.AUTHORIZATION;
            txtUnixTime.Text = Clinic.Class.Bpjsws.Bpjsws.CurrentUnixTime.ToString();

            btnSignatureRefresh.PerformClick();

            cboApiCatalog.Properties.DataSource = DtApiCatalog;
        }

        private void btnSignatureRefresh_Click(object sender, EventArgs e)
        {
            if (txtConsId.Text == "" || txtConsSecret.Text == "" || txtUnixTime.Text == "")
            {
                MessageBox.Show("Consumer ID, Consumer Secreat and Time (Unix) is required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cekAutoUnixTime.Checked) txtUnixTime.PerformClick(txtUnixTime.Properties.Buttons[0]);

            string signature = Clinic.Class.Bpjsws.Bpjsws.CreateSignature(txtConsId.Text, txtConsSecret.Text, txtUnixTime.Text);
            txtSignature.Text = signature;
        }

        private void txtUnixTime_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            txtUnixTime.Text = Clinic.Class.Bpjsws.Bpjsws.CurrentUnixTime.ToString();
        }

        private void radApiType_EditValueChanged(object sender, EventArgs e)
        {
            ClearInput();
            string type = radApiType.EditValue?.ToString();
            if(type == "ANTROL")
            {
                DataRow[] rows = DtApiCatalog.Select("TYPE = 'ANTROL'");
                if (rows.Length > 0) cboApiCatalog.Properties.DataSource = rows.CopyToDataTable();
                else cboApiCatalog.Properties.DataSource = null;
            }
            else if(type == "PCARE")
            {
                DataRow[] rows = DtApiCatalog.Select("TYPE = 'PCARE'");
                if (rows.Length > 0) cboApiCatalog.Properties.DataSource = rows.CopyToDataTable();
                else cboApiCatalog.Properties.DataSource = null;
            }

            cboApiCatalog.ShowPopup();
        }

        private void cboApiCatalog_EditValueChanged(object sender, EventArgs e)
        {
            string url = cboApiCatalog.GetColumnValue("URL")?.ToString();
            string constName = cboApiCatalog.GetColumnValue("CONS_NAME")?.ToString();
            string funcName = cboApiCatalog.GetColumnValue("FUNC_NAME")?.ToString();

            txtUrl.Text = url;
            txtConst.Text = constName;
            txtFunc.Text = funcName;

            txtParam1.ResetText();
            txtParam2.ResetText();
            txtParam3.ResetText();
            txtParam4.ResetText();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string url = cboApiCatalog.GetColumnValue("URL")?.ToString();
            JObject json = null;
            
            if(radApiType.EditValue?.ToString() == "ANTROL")
            {
                Class.Bpjsws.BpjswsResponse resp;
                switch (url)
                {
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL:
                        resp = Class.Bpjsws.BpjswsAntrol.GetReferensiPoli(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL:
                        resp = Class.Bpjsws.BpjswsAntrol.GetReferensiDokter(txtParam1.Text, txtParam2.Text);
                        if (resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex) {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsAntrol.TambahAntrean(json);
                        if (resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsAntrol.PanggilAntrean(json);
                        if (resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsAntrol.BatalAntrian(json);
                        if (resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;
                }
            }
            else if (radApiType.EditValue?.ToString() == "PCARE")
            {
                BpjswsResponse resp;
                switch (url)
                {
                    case Class.Bpjsws.Bpjsws.WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetDiagnosa(txtParam1.Text, int.Parse(txtParam2.Text), int.Parse(txtParam3.Text));
                        if(resp != null) txtResponse.Text = resp.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_DOKTER_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetDokter(int.Parse(txtParam1.Text), int.Parse(txtParam2.Text));
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // Kelompok
                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_GET_CLUB_PROTANIS_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetClubProtanis(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_GET_ACTIVITY_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetKegiatanKelompok(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_GET_PATIENT_ACTIVITY_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetPesertaKegiatanKelompok(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_POST_ACTIVITY_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.AddKegiatanKelompok(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_POST_PATIENT_ACTIVITY_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.AddPesertaKegiatanKelompok(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_DELETE_ACTIVITY_URL:
                        resp = Class.Bpjsws.BpjswsPcare.DeleteKegiatanKelompok(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_GROUP_DELETE_PATIENT_ACTIVITY_URL:
                        resp = Class.Bpjsws.BpjswsPcare.DeletePesertaKegiatanKelompok(txtParam1.Text, txtParam2.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // Kesadaran
                    case Class.Bpjsws.Bpjsws.WS_PCARE_KESADARAN_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetKesadaran();
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // Kunjungan

                    case Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_RUJUKAN_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetRujukan(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_RIWAYAT_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetRiwayatKunjungan(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_ADD_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.AddKunjungan(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_EDIT_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.EditKunjungan(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_DELETE_URL:
                        resp = Class.Bpjsws.BpjswsPcare.DeleteKunjungan(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // MCU

                    case Class.Bpjsws.Bpjsws.WS_PCARE_MCU_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetMCU(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_MCU_ADD_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.AddMCU(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_MCU_EDIT_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.EditMCU(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_MCU_DELETE_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.EditKunjungan(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // Obat

                    case Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_DPHO_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetDPHO(txtParam1.Text, int.Parse(txtParam2.Text), int.Parse(txtParam3.Text));
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_KUNJUNGAN_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetObatByKunjungan(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_ADD_URL:
                        try { json = JObject.Parse(txtBody.Text); }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                            return;
                        }

                        resp = Class.Bpjsws.BpjswsPcare.AddObat(json);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_DELETE_URL:
                        resp = Class.Bpjsws.BpjswsPcare.DeleteObat(txtParam1.Text, txtParam2.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    // Peserta
                    case Class.Bpjsws.Bpjsws.WS_PCARE_PESERTA_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetPeserta(txtParam1.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;

                    case Class.Bpjsws.Bpjsws.WS_PCARE_PESERTA_BY_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetPesertaByJenisKartu(txtParam1.Text, txtParam2.Text);
                        if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        else txtResponse.Text = "Unknown error! please call the administrator";
                        break;
                }
            }


            Cursor = Cursors.Default;
        }

        private void ClearInput()
        {
            txtUrl.ResetText();
            txtConst.ResetText();
            txtFunc.ResetText();

            txtParam1.ResetText();
            txtParam2.ResetText();
            txtParam3.ResetText();
        }

        private void InitializeApiCatalog()
        {
            DtApiCatalog = new DataTable();

            DtApiCatalog.Columns.Add("CODE");
            DtApiCatalog.Columns.Add("DESCRIPTION");
            DtApiCatalog.Columns.Add("TYPE");
            DtApiCatalog.Columns.Add("URL");
            DtApiCatalog.Columns.Add("CONS_NAME");
            DtApiCatalog.Columns.Add("FUNC_NAME");

            // Antrol
            // Antrol - Poli
            DtApiCatalog.Rows.Add(111, "Get Referensi Poli", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL,
                "WS_ANTREAN_FKTP_BPJS_REF_POLI_URL",
                "Clinic.Class.Bpjsws.BpjswsAntrol.GetReferensiPoli(string tgl)");

            // Antrol - Dokter
            DtApiCatalog.Rows.Add(121, "Get Referensi Dokter", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL,
                "WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL",
                "Clinic.Class.Bpjsws.BpjswsAntrol.GetReferensiDokter(string poli, string checkDate)");

            // Add Antrian
            DtApiCatalog.Rows.Add(122, "Tambah Antrean", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL,
                "WS_ANTREAN_FKTP_BPJS_ADD_QUEUE_URL",
                "Clinic.Class.Bpjsws.BpjswsAntrol.TambahAntrean(JObject json)");

            // Panggil Antrian
            DtApiCatalog.Rows.Add(123, "Update Status/Panggil Antrean", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL,
                "WS_ANTREAN_FKTP_BPJS_CALL_QUEUE_URL",
                "Clinic.Class.Bpjsws.BpjswsAntrol.PanggilAntrean(JObject json)");

            // Batal Antrian
            DtApiCatalog.Rows.Add(124, "Batal Antrean", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL,
                "WS_ANTREAN_FKTP_BPJS_CANCEL_QUEUE_URL",
                "Clinic.Class.Bpjsws.BpjswsAntrol.BatalAntrian(JObject json)");

            // Pcare
            // Diagnosa
            DtApiCatalog.Rows.Add(211, "Diagnosa - Get Diagnosa", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL,
                "WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetDiagnosa(string codeOrName, int offset = 1, int limit = 10)");

            // Dokter
            DtApiCatalog.Rows.Add(221, "Dokter - Get Dokter", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DOKTER_GET_URL,
                "WS_PCARE_DOKTER_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetDokter(int offset = 1, int limit = 10)");

            // Kesadaran
            DtApiCatalog.Rows.Add(231, "Kesadaran - Get Kesadaran", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KESADARAN_GET_URL,
                "WS_PCARE_KESADARAN_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetKesadaran()");

            // Kunjungan
            DtApiCatalog.Rows.Add(241, "Kunjungan - Get Rujukan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_RUJUKAN_GET_URL,
                "WS_PCARE_KUNJUNGAN_RUJUKAN_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetRujukan(string noKunjungan)");

            DtApiCatalog.Rows.Add(242, "Kunjungan - Get Riwayat Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_RIWAYAT_GET_URL,
                "WS_PCARE_KUNJUNGAN_RIWAYAT_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetRiwayatKunjungan(string noBpjs)");

            DtApiCatalog.Rows.Add(243, "Kunjungan - Add Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_ADD_URL,
                "WS_PCARE_KUNJUNGAN_ADD_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.AddKunjungan(JObject json)");

            DtApiCatalog.Rows.Add(244, "Kunjungan - Edit Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_EDIT_URL,
                "WS_PCARE_KUNJUNGAN_EDIT_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.EditKunjungan(JObject json)");

            DtApiCatalog.Rows.Add(245, "Kunjungan - Delete Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_KUNJUNGAN_DELETE_URL,
                "WS_PCARE_KUNJUNGAN_DELETE_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.DeleteKunjungan(string noKunjungan)");

            // MCU

            DtApiCatalog.Rows.Add(251, "MCU - Get MCU", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_MCU_GET_URL,
                "WS_PCARE_MCU_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetMCU(string noKunjungan)");

            DtApiCatalog.Rows.Add(252, "MCU - Add MCU", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_MCU_ADD_URL,
                "WS_PCARE_MCU_ADD_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.AddMCU(JObject json))");

            DtApiCatalog.Rows.Add(253, "MCU - Edit MCU", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_MCU_EDIT_URL,
                "WS_PCARE_MCU_EDIT_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.EditMCU(JObject json)");

            DtApiCatalog.Rows.Add(254, "MCU - Delete MCU", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_MCU_DELETE_URL,
                "WS_PCARE_MCU_DELETE_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.DeleteMCU(string kodeMCU, string noKunjungan)");

            // Obat

            DtApiCatalog.Rows.Add(261, "Obat - Get DPHO", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_DPHO_GET_URL,
                "WS_PCARE_OBAT_DPHO_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetDPHO(string kodeOrNameDPHO, int offset, int limit)");

            DtApiCatalog.Rows.Add(262, "Obat - Get Obat By Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_KUNJUNGAN_GET_URL,
                "WS_PCARE_OBAT_KUNJUNGAN_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetObatByKunjungan(string noKunjungan)");

            DtApiCatalog.Rows.Add(263, "Obat - Add Obat", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_ADD_URL,
                "WS_PCARE_OBAT_ADD_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.AddObat(JObject json)");

            DtApiCatalog.Rows.Add(264, "Obat - Delete Obat", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_OBAT_DELETE_URL,
                "WS_PCARE_OBAT_DELETE_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.DeleteObat(string kodeObatSK, string noKunjungan)");

            // Pendaftaran

            DtApiCatalog.Rows.Add(271, "Pendaftaran - Get Pendaftaran by Nomor Urut", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DAFT_BY_NO_GET_URL,
                "WS_PCARE_DAFT_BY_NO_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPendaftaranByNoUrut(string noUrutDaftar, string tglDaftar)");

            DtApiCatalog.Rows.Add(272, "Pendaftaran - Get Pendaftaran Provider", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DAFT_PROVIDER_GET_URL,
                "WS_PCARE_DAFT_PROVIDER_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPendaftaranProvider(string tglDaftar, int offset, int limit)");

            DtApiCatalog.Rows.Add(273, "Pendaftaran - Add Pendaftaran", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DAFT_ADD_URL,
                "WS_PCARE_DAFT_ADD_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.AddPendaftaran(JObject json)");

            DtApiCatalog.Rows.Add(274, "Pendaftaran - Delete Pendaftaran", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DAFT_DELETE_URL,
                "WS_PCARE_DAFT_DELETE_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.DeletePendaftaran(string noBpjs, string tglDaftar, string noDaftar, string kodePoli)");

            // Peserta
            DtApiCatalog.Rows.Add(291, "Peserta - Get Peserta", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_PESERTA_GET_URL,
                "WS_PCARE_PESERTA_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPeserta(string noBpjs)");

            DtApiCatalog.Rows.Add(292, "Peserta - Get Peserta by Jenis Kartu", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_PESERTA_BY_GET_URL,
                "WS_PCARE_PESERTA_BY_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPesertaByJenisKartu(string jenisKartu, string nomorKartu)");

            // Poli

            DtApiCatalog.Rows.Add(2101, "Poli - Get Poli FKTP", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_POLI_GET_URL,
                "WS_PCARE_POLI_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPoliFKTP(int offset, int limit)");

            // Provider

            DtApiCatalog.Rows.Add(2111, "Provider - Get Provider Rayonisasi", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_PROVIDER_RAYONISASI_GET_URL,
                "WS_PCARE_PROVIDER_RAYONISASI_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetProviderRayonisasi(int offset, int limit)");

            // Spesialis

            DtApiCatalog.Rows.Add(2121, "Spesialis - Get Referensi Spesialis", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_REF_GET_URL,
                "WS_PCARE_SPESIALIS_REF_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetReferensiSpesialis()");

            DtApiCatalog.Rows.Add(2122, "Spesialis - Get Referensi Sub Spesialis", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_SUB_REF_GET_URL,
                "WS_PCARE_SPESIALIS_SUB_REF_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetReferensiSubSpesialis(string kdSpesialis)");

            DtApiCatalog.Rows.Add(2123, "Spesialis - Get Referensi Sarana", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_SARANA_REF_GET_URL,
                "WS_PCARE_SPESIALIS_SARANA_REF_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetReferensiSaran()");

            DtApiCatalog.Rows.Add(2124, "Spesialis - Get Referensi Khusus", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_KHUSUS_REF_GET_URL,
                "WS_PCARE_SPESIALIS_KHUSUS_REF_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetReferensiKhusus()");

            DtApiCatalog.Rows.Add(2125, "Spesialis - Get Faskes Rujukan Sub Spesialis", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_FRSS_GET_URL,
                "WS_PCARE_SPESIALIS_FRSS_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetFRSS(string kdSubSpesialis, string kdSarana, string tglEstRujuk)");

            DtApiCatalog.Rows.Add(2126, "Spesialis - Get Faskes Rujukan Khusus ALIH RAWAT, HEMODIALISA, JIWA, KUSTA, TB-MDR, SARANA KEMOTERAPI, SARANA RADIOTERAPI, HIV-ODHA", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_FRK1_GET_URL,
                "WS_PCARE_SPESIALIS_FRK1_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetFRK1(string kdKhusus, string noBpjs, string tglEstRujuk)");

            DtApiCatalog.Rows.Add(2127, "Spesialis - Get Faskes Rujukan Khusus THALASEMIA dan HEMOFILI", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_SPESIALIS_FRK2_GET_URL,
                "WS_PCARE_SPESIALIS_FRK2_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetFRK2(string kdKhusus, string kdSubSpesialis, string noBpjs, string tglEstRujuk)");

            // Status Pulang

            DtApiCatalog.Rows.Add(2131, "Status Pulang - Get Status Pulang", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_STATUS_PULANG_GET_URL,
                "WS_PCARE_STATUS_PULANG_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetStatusPulang(bool isRawatInap)");

            // Tindakan

            DtApiCatalog.Rows.Add(2141, "Tindakan - Get Tindakan by Kunjungan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_TINDAKAN_BY_KUNJUNGAN_GET_URL,
                "WS_PCARE_TINDAKAN_BY_KUNJUNGAN_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetTindakanByKunjungan(string noKunjungan)");

            DtApiCatalog.Rows.Add(2142, "Tindakan - Get Referensi Tindakan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_TINDAKAN_REF_GET_URL,
                "WS_PCARE_TINDAKAN_REF_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetReferensiTindakan(string kdTkp, int offset, int limit)");

            DtApiCatalog.Rows.Add(2143, "Tindakan - Add Tindakan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_TINDAKAN_ADD_URL,
                "WS_PCARE_TINDAKAN_ADD_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.AddTindakan(JObject json)");

            DtApiCatalog.Rows.Add(2144, "Tindakan - Edit Tindakan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_TINDAKAN_EDIT_URL,
                "WS_PCARE_TINDAKAN_EDIT_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.EditTindakan(JObject json)");

            DtApiCatalog.Rows.Add(2145, "Tindakan - Delete Tindakan", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_TINDAKAN_DELETE_URL,
                "WS_PCARE_TINDAKAN_DELETE_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.DeleteTindakan(string kdTindakanSK, string noKunjungan)");

            // Alergi

            DtApiCatalog.Rows.Add(2151, "Alergi - Get Alergi", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_ALERGI_GET_URL,
                "WS_PCARE_ALERGI_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetAlergi(string jenisAlergi)");

            // Prognosa

            DtApiCatalog.Rows.Add(2161, "Prognosa - Get Prognosa", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_PROGNOSA_GET_URL,
                "WS_PCARE_PROGNOSA_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetPrognosa()");

        }
    }
}