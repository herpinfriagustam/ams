using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IIIFirstAssessmentCPPT : DevExpress.XtraReports.UI.XtraReport
    {
        public IIIFirstAssessmentCPPT() {
            InitializeComponent();
        }
        public IIIFirstAssessmentCPPT(DataTable dtDetail, DataRow rowHead)
        {
            InitializeComponent();
            SetDataSource(dtDetail, rowHead);
        }

        public void SetDataSource(DataTable dtDetail, DataRow rowHead)
        {
            ReportHelper.FillReport(this, rowHead);

            DataSource = dtDetail;

            xrcTgl.DataBindings.Add("Text", dtDetail, "PROP1");
            xrcKodePPA.DataBindings.Add("Text", dtDetail, "PROP2");
            xrcSOAP.DataBindings.Add("Text", dtDetail, "PROP3");
            xrcHasil.DataBindings.Add("Text", dtDetail, "PROP4");
            xrcInstruksi.DataBindings.Add("Text", dtDetail, "PROP5");
            xrcTTD.DataBindings.Add("Text", dtDetail, "PROP6");
            
        }

    }
}
