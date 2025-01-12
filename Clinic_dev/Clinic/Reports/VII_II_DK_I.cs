using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_II_DK_I : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_II_DK_I()
        {
            InitializeComponent();
        }

        public VII_II_DK_I(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
