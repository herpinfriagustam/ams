using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Reports
{
    public class ReportHelper
    {
        public static void FillReport(XtraReport rpt, DataRow row)
        {
            foreach (Band band in rpt.Bands)
            {
                foreach (XRControl control in band.Controls)
                {
                    FillControl(row, control);
                }
            }
        }

        private static void FillControl(DataRow row,XRControl control)
        {
            if (control.HasChildren)
            {
                foreach (XRControl ctrl in control.Controls)
                {
                    FillControl(row, ctrl);
                }
            }
            else
            {
                if (control is XRLabel && control.Tag != null)
                {
                    XRLabel lbl = control as XRLabel;
                    string val = GetRowValue(row, lbl.Tag?.ToString());
                    if (val != "")
                        lbl.Text = val;
                }
            }
        }

        private static string GetRowValue(DataRow row, string colname, string defaultReturn = "")
        {
            if (row == null) return defaultReturn;
            if (colname == "" || colname == null) return defaultReturn;

            if (row.Table.Columns.Contains(colname))
                return row[colname] != null ? row[colname].ToString() : defaultReturn;

            return defaultReturn;
        }
    }
}
