using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_I_DIC : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_I_DIC(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
