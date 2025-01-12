using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportRujukanOri : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportRujukanOri(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrRefDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt2");
            xrRefDoc.DataBindings.Add("Text", ds.Tables[0], "hos_doc");
            xrRefHos.DataBindings.Add("Text", ds.Tables[0], "hos_name");
            xrRefNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrRefNo2.DataBindings.Add("Text", ds.Tables[0], "letter_no");

            xrRefName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrRefName2.DataBindings.Add("Text", ds.Tables[0], "name");
            xrRefNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrRefAddr.DataBindings.Add("Text", ds.Tables[0], "address");
            xrRefAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRefAge2.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRefGender.DataBindings.Add("Text", ds.Tables[0], "gender");
            xrRefGender2.DataBindings.Add("Text", ds.Tables[0], "gender");

            xrRefAnam.DataBindings.Add("Text", ds.Tables[0], "work_accident");
            xrRefHis.DataBindings.Add("Text", ds.Tables[0], "riwayat");
            xrRefDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrRefRec.DataBindings.Add("Text", ds.Tables[0], "resep");
        }

    }
}
