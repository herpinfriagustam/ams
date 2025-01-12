using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportCutiHamil : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportCutiHamil(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrHamilNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrHamilName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrHamilUmur.DataBindings.Add("Text", ds.Tables[0], "age");
            xrHamilDept.DataBindings.Add("Text", ds.Tables[0], "line");
            xrHamilNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrHamilMinggu.DataBindings.Add("Text", ds.Tables[0], "info02");
            xrHamilTaf.DataBindings.Add("Text", ds.Tables[0], "birth_date2");
            xrHamilTipe.DataBindings.Add("Text", ds.Tables[0], "cer_type");
            xrHamilCuti.DataBindings.Add("Text", ds.Tables[0], "cnt_leave");
            xrHamilMulai.DataBindings.Add("Text", ds.Tables[0], "bgn_date2");
            xrHamilSelesai.DataBindings.Add("Text", ds.Tables[0], "end_date2");
            xrHamilMasuk.DataBindings.Add("Text", ds.Tables[0], "bgn_work2");
            xrHamilTanggal.DataBindings.Add("Text", ds.Tables[0], "letter_dt2");
        }

    }
}
