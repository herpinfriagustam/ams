using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IV_III_MIX : DevExpress.XtraReports.UI.XtraReport
    {
        public IV_III_MIX(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
