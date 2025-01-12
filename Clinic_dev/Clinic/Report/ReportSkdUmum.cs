using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportSkdUmum : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportSkdUmum(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string purpose = Convert.ToString(ds.Tables[0].Rows[0]["purpose"]);
            string nama = Convert.ToString(ds.Tables[0].Rows[0]["pic"]);

            xrSkdUName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrSkdUGender.DataBindings.Add("Text", ds.Tables[0], "gender");
            xrSkdUage.DataBindings.Add("Text", ds.Tables[0], "age");
            xrSkdUDept.DataBindings.Add("Text", ds.Tables[0], "job");
            xrSkdUDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");

            xrSkdUCnt.DataBindings.Add("Text", ds.Tables[0], "cnt_rest");
            xrSkdURestBgn.DataBindings.Add("Text", ds.Tables[0], "bgn_rest");
            xrSkdURestEnd.DataBindings.Add("Text", ds.Tables[0], "end_rest");
            xrSkdUDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt");
            xrSkdUPic.DataBindings.Add("Text", ds.Tables[0], "pic");
            xrSkdUPicInfo.DataBindings.Add("Text", ds.Tables[0], "pic_info");

        }

    }
}
