using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IV_I_PPTK : DevExpress.XtraReports.UI.XtraReport
    {
        public IV_I_PPTK()
        {
            InitializeComponent();
        }

        public IV_I_PPTK(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);

            
        }

    }
}
