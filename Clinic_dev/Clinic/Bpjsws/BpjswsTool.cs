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

        private void InitializeApiCatalog()
        {
            DtApiCatalog = new DataTable();

            DtApiCatalog.Columns.Add("CODE");
            DtApiCatalog.Columns.Add("DESCRIPTION");
            DtApiCatalog.Columns.Add("TYPE");
            DtApiCatalog.Columns.Add("URL");
            DtApiCatalog.Columns.Add("CONS_NAME");
            DtApiCatalog.Columns.Add("FUNC_NAME");

            DtApiCatalog.Rows.Add(111, "Get Referensi Poli", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL,
                "WS_ANTREAN_FKTP_BPJS_REF_POLI_URL",
                "");

            DtApiCatalog.Rows.Add(121, "Get Referensi Dokter", "ANTROL",
                Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL,
                "WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL",
                "");

            DtApiCatalog.Rows.Add(211, "Get Diagnosa", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL,
                "WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetDiagnosa(string codeOrName, int offset = 1, int limit = 10)");

            DtApiCatalog.Rows.Add(221, "Get Dokter", "PCARE",
                Class.Bpjsws.Bpjsws.WS_PCARE_DOKTER_GET_URL,
                "WS_PCARE_DOKTER_GET_URL",
                "Clinic.Class.Bpjsws.BpjswsPacre.GetDokter(int offset = 1, int limit = 10)");
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
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string url = cboApiCatalog.GetColumnValue("URL")?.ToString();
            
            if(radApiType.EditValue?.ToString() == "ANTROL")
            {
                Class.Bpjsws.BpjswsResponse resp;
                switch (url)
                {
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_POLI_URL:
                        resp = Class.Bpjsws.BpjswsAntrol.GetReferensiPoli(txtParam1.Text);
                        txtResponse.Text = resp.GetResponseString();
                        break;
                    case Class.Bpjsws.Bpjsws.WS_ANTREAN_FKTP_BPJS_REF_DOKTER_URL:
                        resp = Class.Bpjsws.BpjswsAntrol.GetReferensiDokter(txtParam1.Text, txtParam2.Text);
                        txtResponse.Text = resp.GetResponseString();
                        break;
                }
            }
            else if (radApiType.EditValue?.ToString() == "PCARE")
            {
                Class.Bpjsws.BpjswsResponseList resp;
                switch (url)
                {
                    case Class.Bpjsws.Bpjsws.WS_PCARE_DIAGNOSA_GET_DIAGNOSA_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetDiagnosa(txtParam1.Text, int.Parse(txtParam2.Text), int.Parse(txtParam3.Text));
                        txtResponse.Text = resp.GetResponseString();
                        break;
                    case Class.Bpjsws.Bpjsws.WS_PCARE_DOKTER_GET_URL:
                        resp = Class.Bpjsws.BpjswsPcare.GetDokter(int.Parse(txtParam1.Text), int.Parse(txtParam2.Text));
                        txtResponse.Text = resp?.GetResponseString();
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
    }
}