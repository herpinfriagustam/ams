using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Clinic.ControllerAntrian
{
    public partial class UcAntrianNoNm : UserControl
    {
        public UcAntrianNoNm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }
        private Color _gradientStartColor = Color.White;
        private Color _gradientEndColor = Color.Gray;
        public Color GradientStartColor
        {
            get { return _gradientStartColor; }
            set { _gradientStartColor = value; this.Invalidate(); }
        }
        public Color GradientEndColor
        {
            get { return _gradientEndColor; }
            set { _gradientEndColor = value; this.Invalidate(); }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, _gradientStartColor, _gradientEndColor, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }

        public Size PoliSize
        {
            get { return lblPoli.Size; }
            set { lblPoli.Size = value; }
        }
        public Size Antrian
        {
            get { return lblAntrian.Size; }
            set { lblAntrian.Size = value; }
        }
        public Size Pasien
        {
            get { return lblPasien.Size; }
            set { lblPasien.Size = value; }
        }
        public string PoliText
        {
            get { return lblPoli.Text; }
            set { lblPoli.Text = value; }
        }
        public string AntrianText
        {
            get { return lblAntrian.Text; }
            set { lblAntrian.Text = value; }
        }
        public string PasienText
        {
            get { return lblPasien.Text; }
            set { lblPasien.Text = value; }
        }
        public Font PoliFont
        {
            get { return lblPoli.Font; }
            set { lblPoli.Font = value; }
        }
        public Font AntrianFont
        {
            get { return lblAntrian.Font; }
            set { lblAntrian.Font = value; }
        }
        public Font PasienFont
        {
            get { return lblPasien.Font; }
            set { lblPasien.Font = value; }
        }

        public Color PoliTextColor
        {
            get { return lblPoli.ForeColor; }
            set { lblPoli.ForeColor = value; }
        }
        public Color AntrianTextColor
        {
            get { return lblAntrian.ForeColor; }
            set { lblAntrian.ForeColor = value; }
        }
        public Color PasienTextColor
        {
            get { return lblPasien.ForeColor; }
            set { lblPasien.ForeColor = value; }
        }
        public Color PoliBgColor
        {
            get { return lblPoli.BackColor; }
            set { lblPoli.BackColor = value; }
        }
        public Color AntrianBgColor
        {
            get { return lblAntrian.BackColor; }
            set { lblAntrian.BackColor = value; }
        }
        public Color PasienBgColor
        {
            get { return lblPasien.BackColor; }
            set { lblPasien.BackColor = value; }
        }
        public Color LineBgColor
        {
            get { return lblLine.BackColor; }
            set { lblLine.BackColor = value; }
        }
    }
}
