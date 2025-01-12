using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IIIFirstAssessment : DevExpress.XtraReports.UI.XtraReport
    {
        public IIIFirstAssessment()
        {
            InitializeComponent();
        }

        public IIIFirstAssessment(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
