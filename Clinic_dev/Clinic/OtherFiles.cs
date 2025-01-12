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
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using Clinic.Report;
using DevExpress.XtraReports.UI;
using System.IO;

namespace Clinic
{
    public partial class OtherFiles : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string currentDir = "";
        Image imgOriginal;

        public OtherFiles()
        {
            InitializeComponent();
        }

        private void ObservationList_Load(object sender, EventArgs e)
        {
            InitData();
            //LoadData();

        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitData()
        {

        }

        private void LoadData()
        {
            string SQL;

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.empid, name, dept ";
            SQL = SQL + Environment.NewLine + "from cs_patient a  ";
            SQL = SQL + Environment.NewLine + "join cs_employees b on (a.empid=b.empid)  ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and a.status = 'A' ";
            SQL = SQL + Environment.NewLine + "and b.retire_dt is null ";
            SQL = SQL + Environment.NewLine + "and a.group_patient = 'COMM' ";
            SQL = SQL + Environment.NewLine + "order by name asc ";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 60;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "NIK";
                gridView1.Columns[1].Caption = "Nama";
                gridView1.Columns[2].Caption = "Department";

                gridView1.Columns[0].Width = 80;
                gridView1.Columns[1].Width = 120;
                gridView1.Columns[2].Width = 180;

                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
            //loading.CloseWaitForm();
            
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

            //    if (stat == "Over")
            //    {
            //        e.Appearance.BackColor = Color.IndianRed;
            //        e.Appearance.BackColor2 = Color.Firebrick;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_nik = "", s_nama = "", s_dept = "";
            string tmp_empid = "", tmp_path = "", tmp_y = "", tmp_m = "";

            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_dept = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);

            tmp_empid = s_nik;
            tmp_y = "20" + tmp_empid.Substring(2, 2);
            tmp_m = tmp_empid.Substring(4, 2);
            tmp_path = @"\\172.70.10.40\clinic\RM\" + tmp_y + "\\" + tmp_m + "\\" + tmp_empid + "\\";

            try
            {
                if (!Directory.Exists(tmp_path))
                {
                    listBox1.Items.Clear();
                    pictureBox1.Image = null;
                    MessageBox.Show("Data Medical Record tidak ditemukan");
                }
                else
                {
                    //MessageBox.Show("Folder ada");

                    FolderBrowserDialog fb = new FolderBrowserDialog();
                    fb.SelectedPath = tmp_path;
                    currentDir = fb.SelectedPath;
                    if (fb.ShowDialog() == DialogResult.OK)
                    {
                        //currentDir = fb.SelectedPath;
                        DirectoryInfo dirInfo = new DirectoryInfo(currentDir);

                        //textBox1.Text = currentDir;

                        var files = dirInfo.GetFiles().Where(c => (c.Extension.Equals(".jpg") || c.Extension.Equals(".jpeg") || c.Extension.Equals(".bmp") || c.Extension.Equals(".png")));
                        pictureBox1.Image = null;
                        listBox1.Items.Clear();
                        foreach (var image in files)
                        {
                            listBox1.Items.Add(image.Name);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error:" + ex.Message + " " + ex.Source);
            }

        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = listBox1.SelectedItems[0].ToString();
                if (!string.IsNullOrEmpty(selectedImage) && !string.IsNullOrEmpty(currentDir))
                {
                    var fullPath = Path.Combine(currentDir, selectedImage);

                    pictureBox1.Image = Image.FromFile(fullPath);
                    imgOriginal = pictureBox1.Image;
                    trackBar1.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error:" + ex.Message + " " + ex.Source);
            }
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            Image img = pictureBox1.Image;
            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Image = img;
            imgOriginal = pictureBox1.Image;
            trackBar1.Value = 0;
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            Image img = pictureBox1.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = img;
            imgOriginal = pictureBox1.Image;
            trackBar1.Value = 0;
        }

        Image Zoom(Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value > 0)
            {
                pictureBox1.Image = Zoom(imgOriginal, new Size(trackBar1.Value, trackBar1.Value));
            }
        }

        private void OtherFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Dispose();
            }
        }
    }
}