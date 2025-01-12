using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_IX_RMPRI_PP : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_IX_RMPRI_PP(DataRow row, DataTable dtDetail)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
            if(dtDetail != null)
            {
                this.PROP1.DataBindings.Add("Text", dtDetail, "PROP1");
                this.PROP2.DataBindings.Add("Text", dtDetail, "PROP2");
                this.PROP3.DataBindings.Add("Text", dtDetail, "PROP3");
                this.PROP4.DataBindings.Add("Text", dtDetail, "PROP4");
            }
        }
    }
}
