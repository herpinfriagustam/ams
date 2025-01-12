using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Clinic
{
    public partial class ReportViewForm : DevExpress.XtraEditors.XtraForm
    {
        ReportEntity ReportObject = new ReportEntity();
        DataRow ReportDataRow;

        public ReportViewForm(DataRow row = null)
        {
            InitializeComponent();

            ReportDataRow = row;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            recMain.LoadDocumentTemplate("Templates\\Rawat_Jalan.rtf");
            recMain.Options.DocumentCapabilities.Tables = DocumentCapability.Disabled;
        }

        private void recMain_CalculateDocumentVariable(object sender, DevExpress.XtraRichEdit.CalculateDocumentVariableEventArgs e)
        {
            //if (ReportDataRow != null && ReportDataRow.Table.Columns.Contains(e.VariableName))
            //{
            //    e.Value = GetRowValue(e.VariableName);
            //    e.Handled = true;
            //}
        }

        private ParagraphProperties CreateParagraphProperties(Document doc, DocumentRange range, ParagraphAlignment align)
        {
            ParagraphProperties alignment = doc.BeginUpdateParagraphs(range);
            alignment.Alignment = align;
            doc.EndUpdateParagraphs(alignment);

            return alignment;
        }

        public string GetRowValue(string colname, string defaultReturn = "")
        {
            if (ReportDataRow == null) return defaultReturn; 
            if (ReportDataRow.Table.Columns.Contains(colname))
                return ReportDataRow[colname] != null ? ReportDataRow[colname].ToString() : defaultReturn;

            return defaultReturn;
        }
    }
}
