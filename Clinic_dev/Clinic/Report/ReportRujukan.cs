using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Report
{
    public partial class ReportRujukan : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportRujukan(DataSet ds)
        {
            InitializeComponent();

            DataSource = ds;
            DataMember = ds.Tables[0].TableName;

            xrRefDate.DataBindings.Add("Text", ds.Tables[0], "letter_dt2");
            xrRefDoc.DataBindings.Add("Text", ds.Tables[0], "hos_doc");
            xrRefHos.DataBindings.Add("Text", ds.Tables[0], "hos_name");
            xrLNo.DataBindings.Add("Text", ds.Tables[0], "letter_no");
            xrLNo2.DataBindings.Add("Text", ds.Tables[0], "mm");

            xrRefName.DataBindings.Add("Text", ds.Tables[0], "name");
            xrRefAddr.DataBindings.Add("Text", ds.Tables[0], "address");
            xrRefAge.DataBindings.Add("Text", ds.Tables[0], "age");
            xrRefGender.DataBindings.Add("Text", ds.Tables[0], "gender");

            xrRefAnam.DataBindings.Add("Text", ds.Tables[0], "anamnesa");
            xrRefHis.DataBindings.Add("Text", ds.Tables[0], "riwayat");
            xrRefDiag.DataBindings.Add("Text", ds.Tables[0], "diagnosa");
            xrRefRec.DataBindings.Add("Text", ds.Tables[0], "resep");
        }

    }
}
