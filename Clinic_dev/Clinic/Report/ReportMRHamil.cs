using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportMRHamil : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportMRHamil(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrMRHNama.DataBindings.Add("Text", ds.Tables[0], "name");
            xrMRHNik.DataBindings.Add("Text", ds.Tables[0], "nik");
            xrMRHAddr.DataBindings.Add("Text", ds.Tables[0], "addr");
            xrMRHDept.DataBindings.Add("Text", ds.Tables[0], "dept");
            xrMRHAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrMRHGpa.DataBindings.Add("Text", ds.Tables[0], "gpa");
            xrMRHHpht.DataBindings.Add("Text", ds.Tables[0], "hpht");
            xrMRHTp.DataBindings.Add("Text", ds.Tables[0], "tp");
            xrMRHDarah.DataBindings.Add("Text", ds.Tables[0], "darah");

            xrMRHColQue.DataBindings.Add("Text", ds.Tables[0], "poli_cd");
            xrMRHColTgl.DataBindings.Add("Text", ds.Tables[0], "ddate");
            xrMRHColAnam.DataBindings.Add("Text", ds.Tables[0], "anamnesa");
            xrMRHColDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrMRHColTerapi.DataBindings.Add("Text", ds.Tables[0], "terapi");
            xrMRHColPemeriksa.DataBindings.Add("Text", ds.Tables[0], "pic");
        }

    }
}
