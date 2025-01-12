using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_II_RMPRI_PPIA : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_II_RMPRI_PPIA(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }
    }
}
