using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RfidClinic
{
    public partial class Warning : DevExpress.XtraEditors.XtraForm
    {
        public string p_cnt = "", p_est = "", p_select ="";

        private void Warning_Load(object sender, EventArgs e)
        {
            lAntrian.Text = "Anda akan menunggu " + p_cnt + " antrian, \n dengan estimasi waktu tunggu - +";
            lEstimasi.Text = p_est + " Menit";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            p_select = "OK";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            p_select = "";
            this.Close();
        }

        public Warning()
        {
            InitializeComponent();
        }
    }
}