using System;
using System.Data;
using System.Windows.Forms;

namespace Clinic.ControllerAntrian
{
    public partial class UcMarquee : UserControl
    {
        private KoneksiOra koneksi;
        ConnectDb ConnOra = new ConnectDb();
        DataTable dtMarqueeText = null;
        private int currentPosition;
        private int currentTextIndex = 0;

        private Timer timerMarquee;
        private Label labelMarquee;

        public UcMarquee()
        {
            InitializeComponent();
            InitializeMarqueeLabel();

            koneksi = new KoneksiOra();
            loadMarqueeText();


        }

        private void InitializeMarqueeLabel()
        {
            labelMarquee = new Label();
            labelMarquee.AutoSize = true;
            labelMarquee.Text = "SELAMAT DATANG DI KLINIK PRATAMA SANTOSA";
            labelMarquee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelMarquee.ForeColor = System.Drawing.Color.White;
            labelMarquee.Font = new System.Drawing.Font("Tahoma", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.Controls.Add(labelMarquee);

            timerMarquee = new Timer();
            timerMarquee.Interval = 50;
            timerMarquee.Tick += TimerMarquee_Tick;
        }
        public string Z_MarqueeText
        {
            get { return labelMarquee.Text; }
            set { labelMarquee.Text = value; }
        }

        public int Z_Interval
        {
            get { return timerMarquee.Interval; }
            set { timerMarquee.Interval = value; }
        }

        public void StartMarquee()
        {
            currentPosition = this.Width;
            timerMarquee.Start();
        }

        public void StopMarquee()
        {
            timerMarquee.Stop();
        }
        private void TimerMarquee_Tick(object sender, EventArgs e)
        {
            currentPosition -= 2;
            labelMarquee.Location = new System.Drawing.Point(currentPosition, labelMarquee.Location.Y);

            if (labelMarquee.Right < 0)
            {
                currentPosition = this.Width;
                if (dtMarqueeText != null)
                {
                    if (dtMarqueeText.Rows.Count > 0)
                    {
                        currentTextIndex = (currentTextIndex + 1) % dtMarqueeText.Rows.Count;
                        labelMarquee.Text = dtMarqueeText.Rows[currentTextIndex]["NREMARK"]?.ToString();
                    }

                }
                loadMarqueeText();

                labelMarquee.Location = new System.Drawing.Point(currentPosition, labelMarquee.Location.Y);
            }
        }

        private void loadMarqueeText()
        {
            try
            {
                dtMarqueeText = null;

                string sql = @" 
                                  SELECT NREMARK
                                    FROM KLINIK.TABLE_ANTRIAN_FILE
                                   WHERE FFLAG = 'Y' AND NFILE = 'TULISAN'
                                ORDER BY SEQ
                              ";

                dtMarqueeText = ConnOra.Data_Table_ora(sql);
            }
            catch { }

        }


    }
}