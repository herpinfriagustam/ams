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
using System.Diagnostics;
using DevExpress.XtraReports.UI;
using Clinic.Reports;
using RestSharp;
using Clinic.Helpers;
using System.Net;
using Newtonsoft.Json.Linq;
using Clinic.Api.Entity;
using Clinic.Api.HL7;

namespace Clinic
{
    public partial class ReportForm : DevExpress.XtraEditors.XtraForm
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string sql = "";
            string where = "";

            
            if(txtSearchKeyword.Text.Trim() != "")
            {
                switch(cboSearchCategory.Text)
                {
                    case "All":
                        where += $" OR UPPER(NM_PASIEN) LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        where += $" OR UPPER(NOMOR_RM) LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        where += $" OR KTP_PASIEN LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        where += $" OR NIK_PASIEN LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        break;
                    case "Nama Pasien":
                        where += $" AND UPPER(NM_PASIEN) LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        break;
                    case "No. Rekam Medis":
                        where += $" AND UPPER(NOMOR_RM) LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        break;
                    case "Nik Pasien":
                        where += $" AND NIK_PASIEN LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        break;
                    case "Ktp Pasien":
                        where += $" AND KTP_PASIEN LIKE '%{ txtSearchKeyword.Text.ToUpper() }%'";
                        break;
                }
            }

            sql = "SELECT A.*, TO_CHAR(A.TGL_LAHIR, 'YYYY-MM-DD') TGL_LAHIR_F, SUBSTR(JK_PASIEN, 1,1 ) JK FROM REPORT_HEADER A WHERE 1=1 " + where + "";

            DB.DbResult result = DB.GetDataTable(sql);
            if(result.Success)
            {
                grd.DataSource = result.ToDataTable();
            }
            else
            {
                App.ShowErrorMessage(result.Message);
            }
        }

        private void txtSearchKeyword_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            DataRow row = gvw.GetFocusedDataRow();
            if(row == null)
            {
                App.ShowWarningMessage("Please select patient first!");
                return;
            }

            ReportViewForm fm = new ReportViewForm(row);
            fm.Show();
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            DataRow row = gvw.GetFocusedDataRow();
            if (row == null)
            {
                App.ShowWarningMessage("Please select patient first!");
                return;
            } 

            try
            { 
                DataTable dtDetail = GetDetail(row); 
                Reports.IBase baseRpt = new Reports.IBase(row);
                baseRpt.CreateDocument();

                Reports.IIGeneralConsent gsRpt = new Reports.IIGeneralConsent(row);
                gsRpt.CreateDocument();
                AddToBaseResport(baseRpt, gsRpt);

                Reports.IIIFirstAssessment faRpt = new Reports.IIIFirstAssessment(row);

                SetAndFillSubReport(faRpt, dtDetail, row, "RPO");
                SetAndFillSubReport(faRpt, dtDetail, row, "CPPT");
                SetAndFillSubReport(faRpt, dtDetail, row, "RGM");

                faRpt.CreateDocument();
                AddToBaseResport(baseRpt, faRpt);

                Reports.IV_I_PPTK pptkRpt = new IV_I_PPTK(row);

                DataTable dtPPTK = dtDetail.Select($"DETAIL_TYPE='PPTK'").CopyToDataTable();
                pptkRpt.DataSource = dtPPTK;
                pptkRpt.FindControl("xrcNo", true).DataBindings.Add("Text", dtPPTK, "PROP1");
                pptkRpt.FindControl("xrcJenisInformasi", true).DataBindings.Add("Text", dtPPTK, "PROP2");
                pptkRpt.FindControl("xrcIsiInformasi", true).DataBindings.Add("Text", dtPPTK, "PROP3");
                pptkRpt.FindControl("xrcTandaV", true).DataBindings.Add("Text", dtPPTK, "PROP4");

                pptkRpt.CreateDocument();
                AddToBaseResport(baseRpt, pptkRpt);

                Reports.IV_II_KPP kppRpt = new IV_II_KPP(row);
                kppRpt.CreateDocument();
                AddToBaseResport(baseRpt, kppRpt);

                Reports.IV_III_MIX iiiRpt = new IV_III_MIX(row);

                SetAndFillSubReport(iiiRpt, dtDetail, row, "FMSFPSPAL");
                SetAndFillSubReport(iiiRpt, dtDetail, row, "PEMANES");
                SetAndFillSubReport(iiiRpt, dtDetail, row, "MTBM_SBLP");
                SetAndFillSubReport(iiiRpt, dtDetail, row, "MTBM_STLP");

                iiiRpt.CreateDocument();
                AddToBaseResport(baseRpt, iiiRpt);

                // Rawat Inap
                Reports.VI_I_RMPRI viIRmpriRpt = new VI_I_RMPRI(row);
                viIRmpriRpt.CreateDocument();
                AddToBaseResport(baseRpt, viIRmpriRpt);

                Reports.VI_II_RMPRI_PPIA viIIRmpriPPIA = new VI_II_RMPRI_PPIA(row);
                viIIRmpriPPIA.CreateDocument();
                AddToBaseResport(baseRpt, viIIRmpriPPIA);

                Reports.VI_III_RMPRI_FPRJPD viIIIRmppriFPRJPD = new VI_III_RMPRI_FPRJPD(row);
                viIIIRmppriFPRJPD.CreateDocument();
                AddToBaseResport(baseRpt, viIIIRmppriFPRJPD);

                Reports.VI_III_RMPRI_FPRJPA viIIIRmppriFPRJPA = new VI_III_RMPRI_FPRJPA(row);
                viIIIRmppriFPRJPA.CreateDocument();
                AddToBaseResport(baseRpt, viIIIRmppriFPRJPA);

                Reports.VI_IV_RMPRI_PAMKRI viIVRmppriPAMKRI = new VI_IV_RMPRI_PAMKRI(row);

                viIVRmppriPAMKRI.CreateDocument();
                AddToBaseResport(baseRpt, viIVRmppriPAMKRI);

                DataTable cpptDt = dtDetail.Select($"DETAIL_TYPE = 'CPPT'").CopyToDataTable();
                Reports.IIIFirstAssessmentCPPT cpptRpt = new IIIFirstAssessmentCPPT(cpptDt, row);
                cpptRpt.CreateDocument();
                AddToBaseResport(baseRpt, cpptRpt);

                Reports.VI_VII_RMPRI_JPO viViiRmppriJPO = new VI_VII_RMPRI_JPO(row); 

                SetAndFillSubReport(viViiRmppriJPO, dtDetail, row, "JPO_LL");
                SetAndFillSubReport(viViiRmppriJPO, dtDetail, row, "JPO_ORAL");
                SetAndFillSubReport(viViiRmppriJPO, dtDetail, row, "JPO_UNJEKSI");
                SetAndFillSubReport(viViiRmppriJPO, dtDetail, row, "JPO_INFUS");
                SetAndFillSubReport(viViiRmppriJPO, dtDetail, row, "JPO_SIT");

                viViiRmppriJPO.CreateDocument();
                AddToBaseResport(baseRpt, viViiRmppriJPO);

                Reports.VI_VIII_RMPRI_AAGPRI viViiiRmppriAAGPRI = new VI_VIII_RMPRI_AAGPRI(row);
                SetAndFillSubReport(viViiiRmppriAAGPRI, dtDetail, row, "AAGPRI_AM");
                viViiiRmppriAAGPRI.CreateDocument();
                AddToBaseResport(baseRpt, viViiiRmppriAAGPRI);

                DataTable dtViIVRmppriAAGPRI = dtDetail.Select($"DETAIL_TYPE='PPLG_OBAT'").CopyToDataTable();
                Reports.VI_IX_RMPRI_PP viIVRmppriAAGPRI = new VI_IX_RMPRI_PP(row, dtViIVRmppriAAGPRI);
                viIVRmppriAAGPRI.CreateDocument();
                AddToBaseResport(baseRpt, viIVRmppriAAGPRI);

                Reports.VI_X_RMPRI_RP viXRmppriRP = new VI_X_RMPRI_RP(row);
                viXRmppriRP.CreateDocument();
                AddToBaseResport(baseRpt, viXRmppriRP);

                // Reset all page numbers in the resulting document.
                baseRpt.PrintingSystem.ContinuousPageNumbering = true;

                Cursor = Cursors.Default;


                // Show the Print Preview form.
                baseRpt.ShowPreviewDialog();

                //var printTool = new ReportPrintTool(new Reports.GeneralConsent());
                //printTool.ShowRibbonPreviewDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private DataTable GetDetail(DataRow row)
        {
            if(row != null)
            {
                string noRm = row["NOMOR_RM"]?.ToString();
                string sql = "SELECT * FROM REPORT_DETAIL WHERE NOMOR_RM = '" + noRm + "' ORDER BY DETAIL_TYPE, ROW_ORDER";
                DB.DbResult result = DB.GetDataTable(sql);
                if (result.Success)
                {
                    return result.ToDataTable();
                }
                else
                {
                    App.ShowErrorMessage(result.Message);
                }
            }

            return null;
        }

        private void AddToBaseResport(XtraReport baseReport, XtraReport report)
        {
            for (int i = 0; i < report.Pages.Count; i++)
            {
                baseReport.Pages.Add(report.Pages[i]);
            }
        }

        private void SetAndFillSubReport(XtraReport parentRpt, DataTable dtDetail, DataRow rowHeader, string detailType)
        {
            if(dtDetail != null) {
                switch(detailType)
                {
                    case "RPO":
                        DataTable dt = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport subRpt = parentRpt.FindControl("subMedicine", true) as XRSubreport;
                        subRpt.ReportSource.DataSource = dt;

                        subRpt.ReportSource.FindControl("xrcRowNum", true).DataBindings.Add("Text", dt, "PROP1");
                        subRpt.ReportSource.FindControl("xrcMedicineName", true).DataBindings.Add("Text", dt, "PROP2");
                        subRpt.ReportSource.FindControl("xrcMedicineDate", true).DataBindings.Add("Text", dt, "PROP3");
                        subRpt.ReportSource.FindControl("xrcNote", true).DataBindings.Add("Text", dt, "PROP4");
                        break;

                    case "CPPT":
                        DataTable cpptDt = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport cpptSubRpt = parentRpt.FindControl("subCPPT", true) as XRSubreport;
                        cpptSubRpt.ReportSource.DataSource = cpptDt;

                        ReportHelper.FillReport(cpptSubRpt.ReportSource, rowHeader);

                        cpptSubRpt.ReportSource.FindControl("xrcTgl", true).DataBindings.Add("Text", cpptDt, "PROP1");
                        cpptSubRpt.ReportSource.FindControl("xrcKodePPA", true).DataBindings.Add("Text", cpptDt, "PROP2");
                        cpptSubRpt.ReportSource.FindControl("xrcSOAP", true).DataBindings.Add("Text", cpptDt, "PROP3");
                        cpptSubRpt.ReportSource.FindControl("xrcHasil", true).DataBindings.Add("Text", cpptDt, "PROP4");
                        cpptSubRpt.ReportSource.FindControl("xrcInstruksi", true).DataBindings.Add("Text", cpptDt, "PROP5");
                        cpptSubRpt.ReportSource.FindControl("xrcTTD", true).DataBindings.Add("Text", cpptDt, "PROP6");
                        break;

                    case "RGM":
                        DataTable toothDt = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport toothSubRpt = parentRpt.FindControl("subTooths", true) as XRSubreport;
                        toothSubRpt.ReportSource.DataSource = toothDt;

                        toothSubRpt.ReportSource.FindControl("xrcRowNum", true).DataBindings.Add("Text", toothDt, "PROP1");
                        toothSubRpt.ReportSource.FindControl("xrcGigi", true).DataBindings.Add("Text", toothDt, "PROP2");
                        toothSubRpt.ReportSource.FindControl("xrcAnamnesa", true).DataBindings.Add("Text", toothDt, "PROP3");
                        toothSubRpt.ReportSource.FindControl("xrcPemeriksaanFisik", true).DataBindings.Add("Text", toothDt, "PROP4");
                        toothSubRpt.ReportSource.FindControl("xrcDiagnosa", true).DataBindings.Add("Text", toothDt, "PROP5");
                        toothSubRpt.ReportSource.FindControl("xrcTerapiTindakan", true).DataBindings.Add("Text", toothDt, "PROP6");
                        toothSubRpt.ReportSource.FindControl("xrcAsuhanKeperawatan", true).DataBindings.Add("Text", toothDt, "PROP7");
                        toothSubRpt.ReportSource.FindControl("xrcTTDPetugas", true).DataBindings.Add("Text", toothDt, "PROP8");
                        toothSubRpt.ReportSource.FindControl("xrcTTDPasien", true).DataBindings.Add("Text", toothDt, "PROP9");
                        break;
                    case "FMSFPSPAL":
                        DataTable dtFisiologi = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport fisiologiSubRpt = parentRpt.FindControl("subFisiologi", true) as XRSubreport;
                        fisiologiSubRpt.ReportSource.DataSource = dtFisiologi;

                        fisiologiSubRpt.ReportSource.FindControl("xrcNo", true).DataBindings.Add("Text", dtFisiologi, "PROP1");
                        fisiologiSubRpt.ReportSource.FindControl("xrcObat", true).DataBindings.Add("Text", dtFisiologi, "PROP2");
                        fisiologiSubRpt.ReportSource.FindControl("xrcDosis", true).DataBindings.Add("Text", dtFisiologi, "PROP3");
                        fisiologiSubRpt.ReportSource.FindControl("xrcAnestesi", true).DataBindings.Add("Text", dtFisiologi, "PROP4");
                        fisiologiSubRpt.ReportSource.FindControl("xrcWaktu", true).DataBindings.Add("Text", dtFisiologi, "PROP5");
                        fisiologiSubRpt.ReportSource.FindControl("xrcTensi", true).DataBindings.Add("Text", dtFisiologi, "PROP6");
                        fisiologiSubRpt.ReportSource.FindControl("xrcNadi", true).DataBindings.Add("Text", dtFisiologi, "PROP7");
                        fisiologiSubRpt.ReportSource.FindControl("xrcRR", true).DataBindings.Add("Text", dtFisiologi, "PROP8");
                        fisiologiSubRpt.ReportSource.FindControl("xrcSuhu", true).DataBindings.Add("Text", dtFisiologi, "PROP9");
                        break;
                    case "PEMANES":
                        DataTable dtAnestesi = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport anestesiSubRpt = parentRpt.FindControl("subAnestesi", true) as XRSubreport;
                        anestesiSubRpt.ReportSource.DataSource = dtAnestesi;

                        anestesiSubRpt.ReportSource.FindControl("PROP1", true).DataBindings.Add("Text", dtAnestesi, "PROP1");
                        anestesiSubRpt.ReportSource.FindControl("PROP2", true).DataBindings.Add("Text", dtAnestesi, "PROP2");
                        anestesiSubRpt.ReportSource.FindControl("PROP3", true).DataBindings.Add("Text", dtAnestesi, "PROP3");
                        anestesiSubRpt.ReportSource.FindControl("PROP4", true).DataBindings.Add("Text", dtAnestesi, "PROP4");
                        anestesiSubRpt.ReportSource.FindControl("PROP5", true).DataBindings.Add("Text", dtAnestesi, "PROP5");
                        anestesiSubRpt.ReportSource.FindControl("PROP6", true).DataBindings.Add("Text", dtAnestesi, "PROP6");
                        anestesiSubRpt.ReportSource.FindControl("PROP7", true).DataBindings.Add("Text", dtAnestesi, "PROP7");
                        anestesiSubRpt.ReportSource.FindControl("PROP8", true).DataBindings.Add("Text", dtAnestesi, "PROP8");
                        anestesiSubRpt.ReportSource.FindControl("PROP9", true).DataBindings.Add("Text", dtAnestesi, "PROP9");
                        anestesiSubRpt.ReportSource.FindControl("PROP10", true).DataBindings.Add("Text", dtAnestesi, "PROP10");
                        anestesiSubRpt.ReportSource.FindControl("PROP11", true).DataBindings.Add("Text", dtAnestesi, "PROP11");
                        anestesiSubRpt.ReportSource.FindControl("PROP12", true).DataBindings.Add("Text", dtAnestesi, "PROP12");
                        anestesiSubRpt.ReportSource.FindControl("PROP13", true).DataBindings.Add("Text", dtAnestesi, "PROP13");
                        break;
                    case "MTBM_SBLP":
                        DataTable dtSblBdh = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport sblBdhSubRpt = parentRpt.FindControl("subOperasiBefore", true) as XRSubreport;
                        sblBdhSubRpt.ReportSource.DataSource = dtSblBdh;

                        sblBdhSubRpt.ReportSource.FindControl("PROP1", true).DataBindings.Add("Text", dtSblBdh, "PROP1");
                        sblBdhSubRpt.ReportSource.FindControl("PROP2", true).DataBindings.Add("Text", dtSblBdh, "PROP2");
                        sblBdhSubRpt.ReportSource.FindControl("PROP3", true).DataBindings.Add("Text", dtSblBdh, "PROP3");
                        sblBdhSubRpt.ReportSource.FindControl("PROP4", true).DataBindings.Add("Text", dtSblBdh, "PROP4");
                        sblBdhSubRpt.ReportSource.FindControl("PROP5", true).DataBindings.Add("Text", dtSblBdh, "PROP5");
                        sblBdhSubRpt.ReportSource.FindControl("PROP6", true).DataBindings.Add("Text", dtSblBdh, "PROP6");
                        sblBdhSubRpt.ReportSource.FindControl("PROP7", true).DataBindings.Add("Text", dtSblBdh, "PROP7");
                        sblBdhSubRpt.ReportSource.FindControl("PROP8", true).DataBindings.Add("Text", dtSblBdh, "PROP8");
                        sblBdhSubRpt.ReportSource.FindControl("PROP9", true).DataBindings.Add("Text", dtSblBdh, "PROP9");
                        break;
                    case "MTBM_STLP":
                        DataTable dtStlBdh = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport stlBdhSubRpt = parentRpt.FindControl("subOperasiSetelah", true) as XRSubreport;
                        stlBdhSubRpt.ReportSource.DataSource = dtStlBdh;

                        stlBdhSubRpt.ReportSource.FindControl("PROP1", true).DataBindings.Add("Text", dtStlBdh, "PROP1");
                        stlBdhSubRpt.ReportSource.FindControl("PROP2", true).DataBindings.Add("Text", dtStlBdh, "PROP2");
                        stlBdhSubRpt.ReportSource.FindControl("PROP3", true).DataBindings.Add("Text", dtStlBdh, "PROP3");
                        stlBdhSubRpt.ReportSource.FindControl("PROP4", true).DataBindings.Add("Text", dtStlBdh, "PROP4");
                        stlBdhSubRpt.ReportSource.FindControl("PROP5", true).DataBindings.Add("Text", dtStlBdh, "PROP5");
                        stlBdhSubRpt.ReportSource.FindControl("PROP6", true).DataBindings.Add("Text", dtStlBdh, "PROP6");
                        stlBdhSubRpt.ReportSource.FindControl("PROP7", true).DataBindings.Add("Text", dtStlBdh, "PROP7");
                        stlBdhSubRpt.ReportSource.FindControl("PROP8", true).DataBindings.Add("Text", dtStlBdh, "PROP8");
                        stlBdhSubRpt.ReportSource.FindControl("PROP9", true).DataBindings.Add("Text", dtStlBdh, "PROP9");
                        break;

                    case "AAGPRI_AM":
                        DataTable dtAAGPRIAM = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport AAGPRIAMSubRpt = parentRpt.FindControl("subAAGPRIAM", true) as XRSubreport;
                        AAGPRIAMSubRpt.ReportSource.DataSource = dtAAGPRIAM;

                        AAGPRIAMSubRpt.ReportSource.FindControl("PROP1", true).DataBindings.Add("Text", dtAAGPRIAM, "PROP1");
                        AAGPRIAMSubRpt.ReportSource.FindControl("PROP2", true).DataBindings.Add("Text", dtAAGPRIAM, "PROP2");
                        AAGPRIAMSubRpt.ReportSource.FindControl("PROP3", true).DataBindings.Add("Text", dtAAGPRIAM, "PROP3");
                        AAGPRIAMSubRpt.ReportSource.FindControl("PROP4", true).DataBindings.Add("Text", dtAAGPRIAM, "PROP4");
                        break;

                    case "JPO_LL":
                        DataTable dtJPOLL = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();
                        XRSubreport JPOLLSubRpt = parentRpt.FindControl("subLainLain", true) as XRSubreport;
                        JPOLLSubRpt.ReportSource.DataSource = dtJPOLL;

                        JPOLLSubRpt.ReportSource.FindControl("PROP1", true).DataBindings.Add("Text", dtJPOLL, "PROP1");
                        JPOLLSubRpt.ReportSource.FindControl("PROP2", true).DataBindings.Add("Text", dtJPOLL, "PROP2");
                        JPOLLSubRpt.ReportSource.FindControl("PROP3", true).DataBindings.Add("Text", dtJPOLL, "PROP3");
                        JPOLLSubRpt.ReportSource.FindControl("PROP4", true).DataBindings.Add("Text", dtJPOLL, "PROP4");
                        JPOLLSubRpt.ReportSource.FindControl("PROP5", true).DataBindings.Add("Text", dtJPOLL, "PROP5");
                        JPOLLSubRpt.ReportSource.FindControl("PROP6", true).DataBindings.Add("Text", dtJPOLL, "PROP6");
                        break;

                    case "JPO_ORAL":
                        DataRow[] rowTgl = dtDetail.Select("DETAIL_TYPE = 'JPO_ORAL_TGL'");
                        DataRow[] rowJam = dtDetail.Select("DETAIL_TYPE = 'JPO_ORAL_JAM'");
                        DataTable dtOral = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();

                        Reports.VI_VII_RMPRI_JPO_JDWL jpoOralRpt = new VI_VII_RMPRI_JPO_JDWL("NAMA OBAT ORAL", rowTgl?[0], rowJam?[0], dtOral);
                        XRSubreport oralSubRpt = parentRpt.FindControl("subObatOral", true) as XRSubreport;
                        oralSubRpt.ReportSource = jpoOralRpt;
                        break;
                    case "JPO_UNJEKSI":
                        DataRow[] rowTglUnjeksi = dtDetail.Select("DETAIL_TYPE = 'JPO_UNJEKSI_TGL'");
                        DataRow[] rowJamUnjeksi = dtDetail.Select("DETAIL_TYPE = 'JPO_UNJEKSI_JAM'");
                        DataTable dtUnjeksi = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();

                        Reports.VI_VII_RMPRI_JPO_JDWL jpoUnjeksiRpt = new VI_VII_RMPRI_JPO_JDWL("NAMA OBAT UNJEKSI", rowTglUnjeksi?[0], rowJamUnjeksi?[0], dtUnjeksi);
                        XRSubreport unjeksiSubRpt = parentRpt.FindControl("subObatUnjeksi", true) as XRSubreport;
                        unjeksiSubRpt.ReportSource = jpoUnjeksiRpt;
                        break;
                    case "JPO_INFUS":
                        DataRow[] rowTglInfus = dtDetail.Select("DETAIL_TYPE = 'JPO_INFUS_TGL'");
                        DataRow[] rowJamInfus = dtDetail.Select("DETAIL_TYPE = 'JPO_INFUS_JAM'");
                        DataTable dtInfus = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();

                        Reports.VI_VII_RMPRI_JPO_JDWL jpoInfusRpt = new VI_VII_RMPRI_JPO_JDWL("CAIRAN INFUS", rowTglInfus?[0], rowJamInfus?[0], dtInfus);
                        XRSubreport infusSubRpt = parentRpt.FindControl("subCairanInfus", true) as XRSubreport;
                        infusSubRpt.ReportSource = jpoInfusRpt;
                        break;
                    case "JPO_SIT":
                        DataRow[] rowTglSIT = dtDetail.Select("DETAIL_TYPE = 'JPO_SIT_TGL'");
                        DataRow[] rowJamSIT = dtDetail.Select("DETAIL_TYPE = 'JPO_SIT_JAM'");
                        DataTable dtSIT = dtDetail.Select($"DETAIL_TYPE = '{ detailType }'").CopyToDataTable();

                        Reports.VI_VII_RMPRI_JPO_JDWL jpoSITRpt = new VI_VII_RMPRI_JPO_JDWL("SUPP, INHALASI, TOPIKAL", rowTglSIT?[0], rowJamSIT?[0], dtSIT);
                        XRSubreport SITSubRpt = parentRpt.FindControl("subSIT", true) as XRSubreport;
                        SITSubRpt.ReportSource = jpoSITRpt;
                        break;
                }
                

            }
        }
    }
}