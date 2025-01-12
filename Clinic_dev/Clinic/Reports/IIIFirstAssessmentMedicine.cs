using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IIIFirstAssessmentMedicine : DevExpress.XtraReports.UI.XtraReport
    {
        public IIIFirstAssessmentMedicine()
        {
            InitializeComponent();
        }

        public IIIFirstAssessmentMedicine(DataTable dtDetail)
        {
            InitializeComponent();
            SetDataSource(dtDetail);
        }

        public void SetDataSource(DataTable dtDetail)
        {
            DataSource = dtDetail;

            xrcRowNum.DataBindings.Add("Text", dtDetail, "PROP1");
            xrcMedicineName.DataBindings.Add("Text", dtDetail, "PROP2");
            xrcMedicineDate.DataBindings.Add("Text", dtDetail, "PROP3");
            xrcNote.DataBindings.Add("Text", dtDetail, "PROP4");
        }
    }
}
