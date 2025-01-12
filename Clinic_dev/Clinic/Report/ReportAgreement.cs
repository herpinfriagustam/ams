using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportAgreement : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportAgreement(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string tmp_type = "", type = "";

            xrPasNm.DataBindings.Add("Text", ds.Tables[0], "name");
            xrPasAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrPasJob.DataBindings.Add("Text", ds.Tables[0], "job");
            xrPasAddr.DataBindings.Add("Text", ds.Tables[0], "address");

            xrPenNm.DataBindings.Add("Text", ds.Tables[0], "p_name");
            xrPenAge.DataBindings.Add("Text", ds.Tables[0], "p_age");
            xrPenJob.DataBindings.Add("Text", ds.Tables[0], "p_job");
            xrPenRel.DataBindings.Add("Text", ds.Tables[0], "relation");
            xrPasAddr.DataBindings.Add("Text", ds.Tables[0], "p_address");
            xrLDt.DataBindings.Add("Text", ds.Tables[0], "ddate");
        }

    }
}
