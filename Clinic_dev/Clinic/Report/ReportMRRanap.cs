using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportMRRanap : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportMRRanap(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrNm.DataBindings.Add("Text", ds.Tables[0], "nama");
            xrAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRoom.DataBindings.Add("Text", ds.Tables[0], "room");
            xrRm.DataBindings.Add("Text", ds.Tables[0], "rm");

            xrCol01.DataBindings.Add("Text", ds.Tables[0], "ord");
            xrCol02.DataBindings.Add("Text", ds.Tables[0], "info");
            xrCol03.DataBindings.Add("Text", ds.Tables[0], "val");
        }

    }
}
