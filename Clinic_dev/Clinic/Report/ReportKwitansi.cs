using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportKwitansi : DevExpress.XtraReports.UI.XtraReport
    {

        public ReportKwitansi(DataSet ds)
        {
            InitializeComponent(); 

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;
            
            string tmp_type = "", type = "";

            xrLabel14.DataBindings.Add("Text", ds.Tables[0], "id_visit");
            xrNm.DataBindings.Add("Text", ds.Tables[0], "name");
            xrAge.DataBindings.Add("Text", ds.Tables[0], "typests");
            xrHp.DataBindings.Add("Text", ds.Tables[0], "name");
            xrAlamat.DataBindings.Add("Text", ds.Tables[0], "ttlPay");

            xrRm.DataBindings.Add("Text", ds.Tables[0], "rm_no");
            //xrTipe.DataBindings.Add("Text", ds.Tables[0], "tipe");
            xrTerbilang.DataBindings.Add("Text", ds.Tables[0], "ttl");

            //xrBillCol01.DataBindings.Add("Text", ds.Tables[0], "nno");
            //xrBillCol02.DataBindings.Add("Text", ds.Tables[0], "treat_group_name");
            //xrBillCol03.DataBindings.Add("Text", ds.Tables[0], "a");
            //xrBillCol04.DataBindings.Add("Text", ds.Tables[0], "b");
            //xrBillCol05.DataBindings.Add("Text", ds.Tables[0], "c");

            xrBillDate.DataBindings.Add("Text", ds.Tables[0], "tgl");
        }

    }
}
