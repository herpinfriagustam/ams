using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace Clinic.Report
{
    public partial class ReportAudiometri : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportAudiometri(DataSet ds)
        {
            InitializeComponent();

            //DataSource = ds;
            //DataMember = ds.Tables[0].TableName;

            DataTable dt = ds.Tables[0];
            int no = 0;

            xrNik.DataBindings.Add("Text", ds.Tables[0], "empid");
            xrNama.DataBindings.Add("Text", ds.Tables[0], "name");
            xrUmur.DataBindings.Add("Text", ds.Tables[0], "age");
            xrDept.DataBindings.Add("Text", ds.Tables[0], "dept");
            xrLine.DataBindings.Add("Text", ds.Tables[0], "line");

            xrChart1.Series.Clear();
            xrChart2.Series.Clear();
            foreach (DataRow row in dt.Rows)
            {
                xrChart1.Series.Add(new Series(dt.Rows[no]["periode"].ToString(), ViewType.Line));
                xrChart1.Series[no].Points.Add(new SeriesPoint("250", Convert.ToDouble(dt.Rows[no][2].ToString())));
                xrChart1.Series[no].Points.Add(new SeriesPoint("500", new double[] { Convert.ToDouble(row[3]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("1000", new double[] { Convert.ToDouble(row[4]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("2000", new double[] { Convert.ToDouble(row[5]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("3000", new double[] { Convert.ToDouble(row[6]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("4000", new double[] { Convert.ToDouble(row[7]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("6000", new double[] { Convert.ToDouble(row[8]) }));
                xrChart1.Series[no].Points.Add(new SeriesPoint("8000", new double[] { Convert.ToDouble(row[9]) }));

                // Add the series to the chart. 
                //xrChart1.Series.Add(series1);
                xrChart1.Series[no].ArgumentScaleType = ScaleType.Qualitative;
                xrChart1.Series[no].ValueScaleType = ScaleType.Numerical;
                xrChart1.Series[no].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;

                //((LineSeriesView)series1.View).ColorEach = true;
                ((LineSeriesView)xrChart1.Series[no].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)xrChart1.Series[no].View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)xrChart1.Diagram).EnableAxisXZooming = true;
                ((XYDiagram)xrChart1.Diagram).AxisX.Alignment = AxisAlignment.Far;
                ((XYDiagram)xrChart1.Diagram).AxisY.Reverse = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                xrChart1.Series[no].Label.Font = new Font(xrChart1.Series[no].Label.Font.FontFamily, 9, FontStyle.Bold);

                xrChart2.Series.Add(new Series(dt.Rows[no]["periode"].ToString(), ViewType.Line));
                xrChart2.Series[no].Points.Add(new SeriesPoint("250", Convert.ToDouble(dt.Rows[no][10].ToString())));
                xrChart2.Series[no].Points.Add(new SeriesPoint("500", new double[] { Convert.ToDouble(row[11]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("1000", new double[] { Convert.ToDouble(row[12]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("2000", new double[] { Convert.ToDouble(row[13]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("3000", new double[] { Convert.ToDouble(row[14]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("4000", new double[] { Convert.ToDouble(row[15]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("6000", new double[] { Convert.ToDouble(row[16]) }));
                xrChart2.Series[no].Points.Add(new SeriesPoint("8000", new double[] { Convert.ToDouble(row[17]) }));

                // Add the series to the chart. 
                //xrChart2.Series.Add(series1);
                xrChart2.Series[no].ArgumentScaleType = ScaleType.Qualitative;
                xrChart2.Series[no].ValueScaleType = ScaleType.Numerical;
                xrChart2.Series[no].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;

                //((LineSeriesView)series1.View).ColorEach = true;
                ((LineSeriesView)xrChart2.Series[no].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)xrChart2.Series[no].View).LineMarkerOptions.Kind = MarkerKind.Circle;
                //((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Dash;

                // Access the type-specific options of the diagram. 
                ((XYDiagram)xrChart2.Diagram).EnableAxisXZooming = true;
                ((XYDiagram)xrChart2.Diagram).AxisX.Alignment = AxisAlignment.Far;
                ((XYDiagram)xrChart2.Diagram).AxisY.Reverse = true;

                // Specify the text pattern of series labels. 
                //series1.Label.TextPattern = "{A}: {V} , {VP:P}";
                xrChart2.Series[no].Label.Font = new Font(xrChart2.Series[no].Label.Font.FontFamily, 9, FontStyle.Bold);

                no++;
            }


            // Specify how series points are sorted. 
            //series1.SeriesPointsSorting = SortingMode.Ascending;
            //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

            // Add a title to the chart and hide the legend. 
            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "Telinga Kanan";

            xrChart1.Titles.Clear();
            xrChart1.Titles.Add(chartTitle1);
            xrChart1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

            // Add the chart to the form. 
            //xrChart1.DataBindings.Add("Text", dt, "percent");
            //tableLayoutPanel3.Controls.Add(xrChart1);

            // Add a title to the chart and hide the legend. 
            ChartTitle chartTitle2 = new ChartTitle();
            chartTitle2.Text = "Telinga Kiri";

            xrChart2.Titles.Clear();
            xrChart2.Titles.Add(chartTitle2);
            xrChart2.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

            // Add the chart to the form. 
            //chartControl2.Dock = DockStyle.Fill;
            //tableLayoutPanel3.Controls.Add(chartControl2);
        }

    }
}
