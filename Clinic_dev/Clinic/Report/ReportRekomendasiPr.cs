using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportRekomendasiPr : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportRekomendasiPr(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrRekomNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrRekomDept.DataBindings.Add("Text", ds.Tables[0], "line");
            xrRekomName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrRekomNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrRekomDept2.DataBindings.Add("Text", ds.Tables[0], "line");
            xrRekomAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRekomAddr.DataBindings.Add("Text", ds.Tables[0], "address");
            xrRekomHamil.DataBindings.Add("Text", ds.Tables[0], "info01");
            xrRekomMinggu.DataBindings.Add("Text", ds.Tables[0], "info02");
            xrRekomAnak.DataBindings.Add("Text", ds.Tables[0], "info03");
            xrRekomHPHT.DataBindings.Add("Text", ds.Tables[0], "info05");
            xrRekomDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt2");

            string rekom01 = Convert.ToString(ds.Tables[0].Rows[0]["recom_01"]);
            string rekom02 = Convert.ToString(ds.Tables[0].Rows[0]["recom_02"]);
            string rekom03 = Convert.ToString(ds.Tables[0].Rows[0]["recom_03"]);
            string rekom04 = Convert.ToString(ds.Tables[0].Rows[0]["recom_04"]);

            if (rekom01 != "N") { xrRekom01.Text = "V"; } else { xrRekom01.Text = ""; } 
            if (rekom02 != "N") { xrRekom02.Text = "V"; } else { xrRekom02.Text = ""; }
            if (rekom03 != "N") { xrRekom03.Text = "V"; } else { xrRekom03.Text = ""; }
            if (rekom04 != "N") { xrRekom04.Text = "V"; } else { xrRekom04.Text = ""; }
        }

    }
}
