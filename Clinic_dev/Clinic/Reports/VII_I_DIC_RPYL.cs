using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class VII_I_DIC_RPYL : DevExpress.XtraReports.UI.XtraReport
    {
        public VII_I_DIC_RPYL()
        {
        }

        public VII_I_DIC_RPYL(DataTable dtDetail = null)
        {
            InitializeComponent();

            // binding
            DataSource = dtDetail;
            PROP1.DataBindings.Add("Text", dtDetail, "PROP1");
            PROP2.DataBindings.Add("Text", dtDetail, "PROP2");
            PROP3.DataBindings.Add("Text", dtDetail, "PROP3");
            PROP4.DataBindings.Add("Text", dtDetail, "PROP4");
            PROP5.DataBindings.Add("Text", dtDetail, "PROP5");
            PROP6.DataBindings.Add("Text", dtDetail, "PROP6");
            PROP7.DataBindings.Add("Text", dtDetail, "PROP7");
            PROP8.DataBindings.Add("Text", dtDetail, "PROP8");
            PROP9.DataBindings.Add("Text", dtDetail, "PROP9");
            PROP9.DataBindings.Add("Text", dtDetail, "PROP10");
            PROP9.DataBindings.Add("Text", dtDetail, "PROP11");
            PROP9.DataBindings.Add("Text", dtDetail, "PROP12");
        }

    }
}
