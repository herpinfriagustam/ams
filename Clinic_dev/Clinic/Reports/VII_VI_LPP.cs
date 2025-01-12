using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_VI_LPP : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_VI_LPP()
        {
            InitializeComponent();
        }

        public VII_VI_LPP(DataRow row, DataTable dtDetail)
        {
            InitializeComponent();

            ReportHelper.FillReport(this, row);

            // binding
            if (dtDetail == null) return;
            DataRow[] rows = dtDetail.Select("DETAIL_TYPE = 'LPP'");
            if(rows.Length > 0)
            {
                DataTable dt = rows.CopyToDataTable();
                DataSource = dt;
                PROP1.DataBindings.Add("Text", dt, "PROP1");
                PROP2.DataBindings.Add("Text", dt, "PROP2");
                PROP3.DataBindings.Add("Text", dt, "PROP3");
                PROP4.DataBindings.Add("Text", dt, "PROP4");
                PROP5.DataBindings.Add("Text", dt, "PROP5");
                PROP6.DataBindings.Add("Text", dt, "PROP6");
                PROP7.DataBindings.Add("Text", dt, "PROP7");
            }
            
        }

    }
}
