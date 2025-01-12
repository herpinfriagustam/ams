using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportVisitNote : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportVisitNote(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrNm.DataBindings.Add("Text", ds.Tables[0], "name");
            xrAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRoom.DataBindings.Add("Text", ds.Tables[0], "ruangan");
            xrRm.DataBindings.Add("Text", ds.Tables[0], "rm_no");

            xrColl01.DataBindings.Add("Text", ds.Tables[0], "tgl"); 
            xrColl02.DataBindings.Add("Text", ds.Tables[0], "visit_note");
            xrColl03.DataBindings.Add("Text", ds.Tables[0], "pic");
        }

    }
}
