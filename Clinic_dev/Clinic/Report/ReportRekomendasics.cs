using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportRekomendasics : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportRekomendasics(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrRekDept.DataBindings.Add("Text", ds.Tables[0], "line");
            xrRekDept2.DataBindings.Add("Text", ds.Tables[0], "line");
            xrRekName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrRekNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrRekAge.DataBindings.Add("Text", ds.Tables[0], "age");

            xrRek.DataBindings.Add("Text", ds.Tables[0], "recom_remark");
            xrRekDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt2");
        }

    }
}
