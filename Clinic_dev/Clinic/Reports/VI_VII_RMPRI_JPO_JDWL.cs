using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_VII_RMPRI_JPO_JDWL : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_VII_RMPRI_JPO_JDWL(string nmObat, DataRow rowTgl, DataRow rowJam, DataTable dtDetail)
        {
            InitializeComponent();

            lblNamaObat.Text = nmObat;
            lblTgl1.Text = rowTgl["PROP1"]?.ToString();
            lblTgl2.Text = rowTgl["PROP2"]?.ToString();
            lblTgl3.Text = rowTgl["PROP3"]?.ToString();

            lblTgl1Jam1.Text = rowJam["PROP1"]?.ToString();
            lblTgl1Jam2.Text = rowJam["PROP2"]?.ToString();
            lblTgl1Jam3.Text = rowJam["PROP3"]?.ToString();
            lblTgl1Jam4.Text = rowJam["PROP4"]?.ToString();

            lblTgl2Jam1.Text = rowJam["PROP6"]?.ToString();
            lblTgl2Jam2.Text = rowJam["PROP7"]?.ToString();
            lblTgl2Jam3.Text = rowJam["PROP8"]?.ToString();
            lblTgl2Jam4.Text = rowJam["PROP9"]?.ToString();

            lblTgl3Jam1.Text = rowJam["PROP11"]?.ToString();
            lblTgl3Jam2.Text = rowJam["PROP12"]?.ToString();
            lblTgl3Jam3.Text = rowJam["PROP13"]?.ToString();
            lblTgl3Jam4.Text = rowJam["PROP14"]?.ToString();

            // binding
            DataSource = dtDetail;
            PROP1.DataBindings.Add("Text", dtDetail, "PROP1");
            PROP2.DataBindings.Add("Text", dtDetail, "PROP2");
            PROP3.DataBindings.Add("Text", dtDetail, "PROP3");
            PROP4.DataBindings.Add("Text", dtDetail, "PROP4");
            PROP5.DataBindings.Add("Text", dtDetail, "PROP5");
            PROP6.DataBindings.Add("Text", dtDetail, "PROP6");
            PROP7.DataBindings.Add("Text", dtDetail, "PROP7");
            PROP8.DataBindings.Add("Text", dtDetail, "PROP8");
            PROP9.DataBindings.Add("Text", dtDetail, "PROP9");
            PROP10.DataBindings.Add("Text", dtDetail, "PROP10");
            PROP11.DataBindings.Add("Text", dtDetail, "PROP11");
            PROP12.DataBindings.Add("Text", dtDetail, "PROP12");
            PROP13.DataBindings.Add("Text", dtDetail, "PROP13");
            PROP14.DataBindings.Add("Text", dtDetail, "PROP14");
            PROP15.DataBindings.Add("Text", dtDetail, "PROP15");
            PROP16.DataBindings.Add("Text", dtDetail, "PROP16");
            PROP17.DataBindings.Add("Text", dtDetail, "PROP17");
            PROP18.DataBindings.Add("Text", dtDetail, "PROP18");
        }

    }
}
