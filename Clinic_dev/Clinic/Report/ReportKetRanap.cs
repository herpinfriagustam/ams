using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportKetRanap : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportKetRanap(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string tmp_nama = "", tmp_umur = "";
            tmp_nama = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
            tmp_umur = Convert.ToString(ds.Tables[0].Rows[0]["age"]);

            xrKetInapNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrKetInapNm.Text = tmp_nama;
            xrKetInapAge.Text = tmp_umur;
            //xrKetInapNm.DataBindings.Add("Text", ds.Tables[0], "name");
            //xrKetInapAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrKetInapAddr.DataBindings.Add("Text", ds.Tables[0], "address");
            xrKetInapDiag.DataBindings.Add("Text", ds.Tables[0], "d_name");

            xrKetInapBgn.DataBindings.Add("Text", ds.Tables[0], "bgndt");
            xrKetInapEnd.DataBindings.Add("Text", ds.Tables[0], "enddt");

            xrKetInapNm2.Text = tmp_nama;
            xrKetInapAge2.Text = tmp_umur;
            //xrKetInapNm2.DataBindings.Add("Text", ds.Tables[0], "p_name");
            //xrKetInapAge2.DataBindings.Add("Text", ds.Tables[0], "p_age");
            xrKetInapCom.DataBindings.Add("Text", ds.Tables[0], "company");
            xrKetInapComAddr.DataBindings.Add("Text", ds.Tables[0], "company_addr");
            xrLDt.DataBindings.Add("Text", ds.Tables[0], "ddate");
        }

    }
}
