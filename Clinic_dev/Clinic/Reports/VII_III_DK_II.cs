using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_III_DK_II : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_III_DK_II()
        {
            InitializeComponent();
        }

        public VII_III_DK_II(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }
    }
}
