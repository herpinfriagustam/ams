using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportBill : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportBill(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            string tmp_type = "", type = "";

            xrNm.DataBindings.Add("Text", ds.Tables[0], "name");
            xrAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrHp.DataBindings.Add("Text", ds.Tables[0], "phone");
            xrAlamat.DataBindings.Add("Text", ds.Tables[0], "address");

            xrRm.DataBindings.Add("Text", ds.Tables[0], "rm");
            xrTipe.DataBindings.Add("Text", ds.Tables[0], "tipe");
            xrTgl.DataBindings.Add("Text", ds.Tables[0], "tgl");

            xrBillCol01.DataBindings.Add("Text", ds.Tables[0], "nno");
            xrBillCol02.DataBindings.Add("Text", ds.Tables[0], "treat_group_name");
            xrBillCol03.DataBindings.Add("Text", ds.Tables[0], "a");
            xrBillCol04.DataBindings.Add("Text", ds.Tables[0], "b");
            xrBillCol05.DataBindings.Add("Text", ds.Tables[0], "c");

            xrBillDate.DataBindings.Add("Text", ds.Tables[0], "tgl");
        }

    }
}
