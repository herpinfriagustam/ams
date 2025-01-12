using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_VIII_CBBL : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_VIII_CBBL()
        {
            InitializeComponent();
        }

        public VII_VIII_CBBL(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }
    }
}
