
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Clinic.ControllerAntrian
{
    public partial class UcDokter : UserControl
    {
        public UcDokter()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            imgDokter.Paint += imgDokter_Paint;
        }

        public string DokterNama
        {
            get { return lblNamaDokter.Text; }
            set { lblNamaDokter.Text = value; }
        }

        public Image DokterImage
        {
            get { return imgDokter.Image; }
            set { imgDokter.Image = value; }
        }
        public int DokterImageWidth
        {
            get { return imgDokter.Width; }
            set { imgDokter.Width = value; }
        }
        public int DokterImageHeight
        {
            get { return imgDokter.Height; }
            set { imgDokter.Height = value; }
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
        private void imgDokter_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, imgDokter.Width - 0, imgDokter.Height - 0);
                imgDokter.Region = new Region(path);
            }
        }

    }
}
