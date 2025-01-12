using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Clinic.Reports
{
    public partial class IIIFirstAssessmentTooths : DevExpress.XtraReports.UI.XtraReport
    {
        public IIIFirstAssessmentTooths()
        {
            InitializeComponent();
        }
        public IIIFirstAssessmentTooths(DataTable dtDetail)
        {
            InitializeComponent();
            SetDataSource(dtDetail);
        }

        public void SetDataSource(DataTable dtDetail)
        {
            DataSource = dtDetail;

            xrcRowNum.DataBindings.Add("Text", dtDetail, "PROP1");
            xrcGigi.DataBindings.Add("Text", dtDetail, "PROP2");
            xrcAnamnesa.DataBindings.Add("Text", dtDetail, "PROP3");
            xrcPemeriksaanFisik.DataBindings.Add("Text", dtDetail, "PROP4");
            xrcDiagnosa.DataBindings.Add("Text", dtDetail, "PROP5");
            xrcTerapiTindakan.DataBindings.Add("Text", dtDetail, "PROP6");
            xrcAsuhanKeperawatan.DataBindings.Add("Text", dtDetail, "PROP7");
            xrcTTDPetugas.DataBindings.Add("Text", dtDetail, "PROP8");
            xrcTTDPasien.DataBindings.Add("Text", dtDetail, "PROP9");
        }
    }
}
