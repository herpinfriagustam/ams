using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportMRKb : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportMRKb(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrMRKNama.DataBindings.Add("Text", ds.Tables[0], "name");
            xrMRKNik.DataBindings.Add("Text", ds.Tables[0], "nik");
            xrMRKMr.DataBindings.Add("Text", ds.Tables[0], "rm");
            xrMRKDept.DataBindings.Add("Text", ds.Tables[0], "dept");
            xrMRKAge.DataBindings.Add("Text", ds.Tables[0], "age");

            xrMRKColQue.DataBindings.Add("Text", ds.Tables[0], "visit_no");
            xrMRKColTgl.DataBindings.Add("Text", ds.Tables[0], "ddate");
            xrMRKColAnam.DataBindings.Add("Text", ds.Tables[0], "anamnesa");
            xrMRKColDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrMRKColTerapi.DataBindings.Add("Text", ds.Tables[0], "terapi");
            xrMRKColPemeriksa.DataBindings.Add("Text", ds.Tables[0], "pic");
        }

    }
}
