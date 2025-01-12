using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VI_VIII_RMPRI_AAGPRI : DevExpress.XtraReports.UI.XtraReport
    {
        public VI_VIII_RMPRI_AAGPRI(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
