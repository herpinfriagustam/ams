namespace Clinic
{
    partial class ReportViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.recMain = new DevExpress.XtraRichEdit.RichEditControl();
            this.SuspendLayout();
            // 
            // recMain
            // 
            this.recMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recMain.Location = new System.Drawing.Point(0, 0);
            this.recMain.Name = "recMain";
            this.recMain.Size = new System.Drawing.Size(900, 538);
            this.recMain.TabIndex = 0;
            this.recMain.CalculateDocumentVariable += new DevExpress.XtraRichEdit.CalculateDocumentVariableEventHandler(this.recMain_CalculateDocumentVariable);
            // 
            // ReportViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 538);
            this.Controls.Add(this.recMain);
            this.Name = "ReportViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraRichEdit.RichEditControl recMain;
    }
}

