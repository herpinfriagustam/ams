using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportMRUmum : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportMRUmum(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrMrName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrMrNik.DataBindings.Add("Text", ds.Tables[0], "nik");
            xrMrRM.DataBindings.Add("Text", ds.Tables[0], "rm");
            xrMrAddr.DataBindings.Add("Text", ds.Tables[0], "addr");
            xrMrTtl.DataBindings.Add("Text", ds.Tables[0], "age");
            xrMrJk.DataBindings.Add("Text", ds.Tables[0], "gender");

            xrMrCol01.DataBindings.Add("Text", ds.Tables[0], "poli_cd");
            xrMrCol02.DataBindings.Add("Text", ds.Tables[0], "ddate");
            xrMrCol03.DataBindings.Add("Text", ds.Tables[0], "anamnesa");
            xrMrCol04.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrMrCol05.DataBindings.Add("Text", ds.Tables[0], "terapi");
            xrMrCol06.DataBindings.Add("Text", ds.Tables[0], "pic");
        }

    }
}
