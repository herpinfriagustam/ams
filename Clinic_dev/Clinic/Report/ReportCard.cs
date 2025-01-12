using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportCard : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportCard(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrNama.DataBindings.Add("Text", ds.Tables[0], "name");
            xrKk.DataBindings.Add("Text", ds.Tables[0], "kk");
            xrRm.DataBindings.Add("Text", ds.Tables[0], "rm_no");
            xrAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrJk.DataBindings.Add("Text", ds.Tables[0], "jk");
            xrAlamat.DataBindings.Add("Text", ds.Tables[0], "alamat");
            
        }

    }
}
