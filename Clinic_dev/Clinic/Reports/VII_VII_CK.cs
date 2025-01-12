using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_VII_CK : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_VII_CK()
        {
            InitializeComponent();
        }

        public VII_VII_CK(DataRow row, DataTable dtDetail)
        {
            InitializeComponent();

            ReportHelper.FillReport(this, row);

            // binding
            if (dtDetail == null) return;
            DataRow[] rows = dtDetail.Select("DETAIL_TYPE = 'CK'");
            if (rows.Length > 0)
            {
                DataTable dt = rows.CopyToDataTable();
                DataSource = dt;
                PROP1.DataBindings.Add("Text", dt, "PROP1");
                PROP2.DataBindings.Add("Text", dt, "PROP2");
                PROP3.DataBindings.Add("Text", dt, "PROP3");
            }
        }

    }
}
