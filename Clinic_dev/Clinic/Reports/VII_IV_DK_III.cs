using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_IV_DK_III : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_IV_DK_III()
        {
            InitializeComponent();
        }

        public VII_IV_DK_III(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
