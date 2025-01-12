using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IBase : DevExpress.XtraReports.UI.XtraReport
    {
        public IBase()
        {
            InitializeComponent();
        }

        public IBase(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }

    }
}
