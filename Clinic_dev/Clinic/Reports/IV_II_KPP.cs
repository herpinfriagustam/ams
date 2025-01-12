using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IV_II_KPP : DevExpress.XtraReports.UI.XtraReport
    {
        public IV_II_KPP(DataRow row = null)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
