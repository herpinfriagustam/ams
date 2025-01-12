using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_VII_RMPRI_JPO : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_VII_RMPRI_JPO(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
