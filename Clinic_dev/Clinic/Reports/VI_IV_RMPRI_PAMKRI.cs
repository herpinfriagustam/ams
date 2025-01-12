using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_IV_RMPRI_PAMKRI : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_IV_RMPRI_PAMKRI(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
