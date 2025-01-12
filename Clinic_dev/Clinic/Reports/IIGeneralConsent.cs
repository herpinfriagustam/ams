using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IIGeneralConsent : DevExpress.XtraReports.UI.XtraReport
    {
        DataRow RowInfo;
        public IIGeneralConsent()
        {
            InitializeComponent();
        }

        public IIGeneralConsent(DataRow row)
        {
            InitializeComponent();
            ReportHelper.FillReport(this, row);
        }
        
    }
}
