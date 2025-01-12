using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_III_RMPRI_FPRJPA : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_III_RMPRI_FPRJPA(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
