using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportSkdKK : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportSkdKK(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string limitStart = Convert.ToString(ds.Tables[0].Rows[0]["bgn_limit"]);
            string limitEnd = Convert.ToString(ds.Tables[0].Rows[0]["end_limit"]);
            string restStart = Convert.ToString(ds.Tables[0].Rows[0]["bgn_rest"]);
            string endStart = Convert.ToString(ds.Tables[0].Rows[0]["end_rest"]);
            string ctrl = Convert.ToString(ds.Tables[0].Rows[0]["control"]);

            xrSkdKNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrSkdKVisit.DataBindings.Add("Text", ds.Tables[0], "visit_date");
            xrSkdKName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrSkdKNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrSkdKPos.DataBindings.Add("Text", ds.Tables[0], "position");
            xrSkdKDept.DataBindings.Add("Text", ds.Tables[0], "dept");
            //xrSkdKDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrSkdKDiag.Text = "Kecelakaan Kerja";
            xrSkdKDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt");

            xrSkdKReturn.DataBindings.Add("Text", ds.Tables[0], "return_work");

            if (limitStart != "" && limitEnd != "") { xrSkdKLimit.Text = "V"; }
            xrSkdKLimitStart.DataBindings.Add("Text", ds.Tables[0], "bgn_limit");
            xrSkdKLimitStart2.DataBindings.Add("Text", ds.Tables[0], "bgn_limit");
            xrSkdKLimitEnd.DataBindings.Add("Text", ds.Tables[0], "end_limit");
            xrSkdKLimitEnd2.DataBindings.Add("Text", ds.Tables[0], "end_limit");

            xrSkdKLimit01.DataBindings.Add("Text", ds.Tables[0], "limit01");
            xrSkdKLimit02.DataBindings.Add("Text", ds.Tables[0], "limit02");
            xrSkdKLimit03.DataBindings.Add("Text", ds.Tables[0], "limit03");
            xrSkdKMachine.DataBindings.Add("Text", ds.Tables[0], "remark_machine");
            xrSkdKMachine2.DataBindings.Add("Text", ds.Tables[0], "remark_machine");
            xrSkdKLimit04.DataBindings.Add("Text", ds.Tables[0], "limit04");
            xrSkdKLimit05.DataBindings.Add("Text", ds.Tables[0], "limit05");
            xrSkdKLimit06.DataBindings.Add("Text", ds.Tables[0], "limit06");
            xrSkdKLimit07.DataBindings.Add("Text", ds.Tables[0], "limit07");
            xrSkdKLimit08.DataBindings.Add("Text", ds.Tables[0], "limit08");
            xrSkdKLimit09.DataBindings.Add("Text", ds.Tables[0], "limit09");
            xrSkdKLimit10.DataBindings.Add("Text", ds.Tables[0], "limit10");
            xrSkdKRemark.DataBindings.Add("Text", ds.Tables[0], "remark");
            xrSkdKRemark2.DataBindings.Add("Text", ds.Tables[0], "remark");

            if (restStart != "" && endStart != "") { xrSkdKRest.Text = "V"; }
            xrSkdKRestStart.DataBindings.Add("Text", ds.Tables[0], "bgn_rest");
            xrSkdKRestStart2.DataBindings.Add("Text", ds.Tables[0], "bgn_rest");
            xrSkdKRestEnd.DataBindings.Add("Text", ds.Tables[0], "end_rest");
            xrSkdKRestEnd2.DataBindings.Add("Text", ds.Tables[0], "end_rest");

            if (ctrl != "" ) { xrSkdKCtrl.Text = "V"; }
            xrSkdKControl.DataBindings.Add("Text", ds.Tables[0], "control");
            xrSkdKControl2.DataBindings.Add("Text", ds.Tables[0], "control");
        }

    }
}
