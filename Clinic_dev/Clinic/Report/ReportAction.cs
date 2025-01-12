using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportAction : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportAction(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string tmp_type = "", type = "";
            tmp_type = Convert.ToString(ds.Tables[0].Rows[0]["act_type"]);
            if (tmp_type == "A")
            {
                type = "PERSETUJUAN";
            }
            else if (tmp_type == "D")
            {
                type = "PENOLAKAN";
            }
            else
            {
                type = "";
            }

            xrActNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrActNama.DataBindings.Add("Text", ds.Tables[0], "name");
            xrActJK.DataBindings.Add("Text", ds.Tables[0], "gender");
            xrActUmur.DataBindings.Add("Text", ds.Tables[0], "age");
            xrActDept.DataBindings.Add("Text", ds.Tables[0], "line");

            xrActType.Text = type;
            xrActTindakan.DataBindings.Add("Text", ds.Tables[0], "act_name");
            xrActDt.DataBindings.Add("Text", ds.Tables[0], "tgl");
            xrActPasien.DataBindings.Add("Text", ds.Tables[0], "name");
            xrActDokter.DataBindings.Add("Text", ds.Tables[0], "pic");
        }

    }
}
