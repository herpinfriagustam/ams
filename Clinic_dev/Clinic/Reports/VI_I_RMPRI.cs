using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_I_RMPRI : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_I_RMPRI(DataRow row = null)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
