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
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Media;
using DevExpress.XtraEditors.Repository;
using System.Diagnostics;
using DevExpress.XtraLayout;
using System.Reflection;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace RfidClinic
{
    public partial class Reservation : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reservation));

        string InputData_scanner = String.Empty;
        delegate void SetTextCallback(string text);

        string lsMSG = "";
        int lsOK = 0;
        bool bl_klap = true;
        string visit_cnt = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        bool p_enable;
        string id = "", poli = "", attr01 = "", attr02 = "", attr03 = "", attr04 = "", attr05 = "", gender = "";
        string que = "", c_que = "", poliname="";
        private PrintDocument printDocument;

        // Import untuk mengakses API printer
        [DllImport("winspool.Drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool OpenPrinter(string src, out IntPtr hPrinter, IntPtr pd);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, ref DOCINFOA pDocInfo);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        public Reservation()
        {
            InitializeComponent();
            //printDocument = new PrintDocument();
            //printDocument.PrintPage += new PrintPageEventHandler(PrintPage);
            //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_scanner);
        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            load_List("1", true );
            //img_rfid_tap();
            //check_rfid();
            lInfo.Text = "Selamat Datang " + "\r\n" + "Silahkan Tentukan Poli yang Anda Tuju";
        }

        public void img_rfid_tap()
        {
            for (int i = 0; i < 1; i++)
            {
                //Panel p_left = new Panel();
                //p_left.Tag = i;
                //flowLayoutPanel1.Controls.Add(p_left);
                

                //PictureBox pictureBox1 = new PictureBox();
                //pictureBox1.Image = global::RfidClinic.Properties.Resources.rfid_tap;
                //pictureBox1.Name = "pictureBox1";
                //pictureBox1.Size = new System.Drawing.Size(461, 325);
                //pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

                //pictureBox1.TabIndex = 0;
                //pictureBox1.TabStop = false;
                //flowLayoutPanel1.Controls.Add(pictureBox1);
                ////pictureBox1.Left = (this.ClientSize.Width - pictureBox1.Width) / 2;
                ////pictureBox1.Top = (this.ClientSize.Height - pictureBox1.Height) / 2;
                ////pictureBox1.Anchor = AnchorStyles.None;

                //p_left.Size = new System.Drawing.Size(((this.ClientSize.Width - pictureBox1.Width) / 2)-200, 325);
                //flowLayoutPanel1.Margin = new Padding(0, 10, 0, 10);


            }
        }

        public void load_List(string p_attr, bool p_bol)
        {
            int tot = 0;
            //string id = "", poli = "", attr01 = "", attr02 = "", attr03 = "", attr04 = "", attr05 = "";

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select code_id, code_name,  ";
            SQL = SQL + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05 ";
            SQL = SQL + Environment.NewLine + "from CS_CODE_DATA ";
            SQL = SQL + Environment.NewLine + "where code_class_id='RESV_ITEM' ";
            SQL = SQL + Environment.NewLine + "and status='A' ";
            SQL = SQL + Environment.NewLine + "and attr_01='" + p_attr + "' ";
            SQL = SQL + Environment.NewLine + "order by sort_order asc ";


            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);

                tot = dt2.Rows.Count;

                LayoutControl layoutControl1 = new LayoutControl();
                layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
                panel3.Controls.Clear();
                panel3.Controls.Add(layoutControl1);

                layoutControl1.BeginUpdate();
                

                LayoutControlGroup group1 = new LayoutControlGroup();
                group1.Name = "GroupDetails";
                group1.Text = "Details";
                group1.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Flow;
                group1.GroupBordersVisible = false;

                for (int i = 0; i < tot; i++)
                {
                    id = dt2.Rows[i]["code_id"].ToString();
                    poli = dt2.Rows[i]["code_name"].ToString();
                    attr01 = dt2.Rows[i]["attr_01"].ToString();
                    attr02 = dt2.Rows[i]["attr_02"].ToString();
                    attr03 = dt2.Rows[i]["attr_03"].ToString();
                    attr04 = dt2.Rows[i]["attr_04"].ToString();
                    attr05 = dt2.Rows[i]["attr_05"].ToString();
                    

                    SimpleButton button = new SimpleButton();
                    button.Appearance.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    button.Appearance.ForeColor = System.Drawing.Color.Black;
                    button.Appearance.Options.UseFont = true;
                    button.Appearance.Options.UseForeColor = true;
                    //button.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
                    //button.Image = imageCollection1.Images[0];
                    if (tot <= 2)
                    {
                        if (attr05 == "DOC")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.doctor_m256;
                        }
                        else if (attr05 == "MID")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.doctor_f256;
                        }
                        else if (attr05 == "SWA")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.swab1_256;
                        }
                        else if (attr05 == "MCU")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.checkup1_256;
                        }
                        else
                        {
                            button.Image = global::RfidClinic.Properties.Resources.checkup1_256;
                        }

                        button.ImageLocation = ImageLocation.TopCenter;
                        button.Size = new System.Drawing.Size(500, 300);
                    }
                    else
                    {
                        if (attr05 == "DOC")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.doctor_m64;
                        }
                        else if (attr05 == "MID")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.doctor_f64;
                        }
                        else if (attr05 == "SWA")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.swab1_64;
                        }
                        else if (attr05 == "MCU")
                        {
                            button.Image = global::RfidClinic.Properties.Resources.checkup1_64;
                        }
                        else
                        {
                            button.Image = global::RfidClinic.Properties.Resources.checkup1_64;
                        }

                        button.ImageLocation = ImageLocation.Default;
                        button.Size = new System.Drawing.Size(500, 100);
                    }
                    button.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                    button.LookAndFeel.SkinMaskColor = System.Drawing.Color.GhostWhite;
                    //button.LookAndFeel.SkinName = "DevExpress Dark Style";
                    button.LookAndFeel.UseDefaultLookAndFeel = false;
                    button.Name = id;
                    button.Text = poli;
                    button.Tag = attr02;
                    //button.Enabled = p_bol;
                    
                    LayoutControlItem itemBtn = group1.AddItem();
                    itemBtn.Name = id;
                    itemBtn.Control = button;
                    itemBtn.Text = poli;
                    itemBtn.TextVisible = false;
                    itemBtn.SizeConstraintsType = SizeConstraintsType.Custom;
                    //itemBtn.Enabled = true;
                    //itemOKButton.Width = 440;
                    if (tot <= 2)
                    {
                        itemBtn.MaxSize = new Size(500, 300);
                        itemBtn.MinSize = new Size(500, 300);
                    }
                    else
                    {
                        itemBtn.MaxSize = new Size(500, 100);
                        itemBtn.MinSize = new Size(500, 100);
                    }
                    p_enable = p_bol;
                    itemBtn.StartNewLine = false;

                    button.Click += layoutControlItem1_Click;
                }
                
                layoutControl1.Root.Add(group1);
                //layoutControl1.AddGroup(group1);
                int aa = group1.Items.Count;
                layoutControl1.EndUpdate();

                if (Convert.ToInt16(attr01) > 1)
                {
                    pictureEdit2.Visible = true;
                }
                else
                {
                    pictureEdit2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }

            
        }

        private void layoutControlItem1_Click(object sender, EventArgs e)
        {
            //LayoutControlItem item = (LayoutControlItem)sender;
            if (p_enable == true)
            {
                SimpleButton clickedButton = (SimpleButton)sender;
                if (gender == "L" && (clickedButton.Text == "Bidan" || clickedButton.Text == "Poli Obgyn"))
                {
                    MessageBox.Show("Anda tidak bisa memilih poli tersebut.");
                    return;
                }
                else
                {
                    //MessageBox.Show(attr03);
                    string SQL = "", vcode_id="", vcode_name="", vattr_01="", vattr_02 = "", vattr_03 = "";
                    string vattr_04 = "", vattr_05 = "", vattr_06 = "";

                    SQL = SQL + Environment.NewLine + "select code_id, code_name,  ";
                    SQL = SQL + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05, attr_06 ";
                    SQL = SQL + Environment.NewLine + "from CS_CODE_DATA ";
                    SQL = SQL + Environment.NewLine + "where code_class_id='RESV_ITEM' ";
                    SQL = SQL + Environment.NewLine + "and status='A' ";
                    SQL = SQL + Environment.NewLine + "and code_id='" + clickedButton.Name + "' ";
                    SQL = SQL + Environment.NewLine + "order by sort_order asc ";

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        vcode_id = dt2.Rows[0]["code_id"].ToString();
                        vcode_name = dt2.Rows[0]["code_name"].ToString();
                        vattr_01 = dt2.Rows[0]["attr_01"].ToString();
                        vattr_02 = dt2.Rows[0]["attr_02"].ToString();
                        vattr_03 = dt2.Rows[0]["attr_03"].ToString();
                        vattr_04 = dt2.Rows[0]["attr_04"].ToString();
                        vattr_05 = dt2.Rows[0]["attr_05"].ToString();
                        vattr_06 = dt2.Rows[0]["attr_06"].ToString();
                    }
                    else
                    {
                        vcode_id = "";
                        vcode_name = "";
                        vattr_01 = "";
                        vattr_02 = "";
                        vattr_03 = "";
                        vattr_04 = "";
                        vattr_05 = "";
                        vattr_06 = "";
                    }

                    if (vattr_03 != "")
                    {
                        //MessageBox.Show("Input data");
                        if (vattr_04 == "R")
                        {
                            typeRsv(vattr_03);
                        }
                        else if (vattr_04 == "A")
                        {
                            typeAct(vattr_03);
                        }
                        load_List("1", true);
                    }
                    else
                    {
                        load_List(clickedButton.Tag.ToString(), true);
                    } 
                } 
            } 
        }

        public void load_List2()
        {
            int tot = 14;
            for (int i = 0; i < tot; i++)
            {
                SimpleButton button = new SimpleButton();
                button.Appearance.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.Appearance.ForeColor = System.Drawing.Color.Black;
                button.Appearance.Options.UseFont = true;
                button.Appearance.Options.UseForeColor = true;
                //button.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
                //button.Image = imageCollection1.Images[0];
                if (tot <= 2)
                {
                    button.Image = global::RfidClinic.Properties.Resources.doctor_f256;
                    button.ImageLocation = ImageLocation.TopCenter;
                    button.Size = new System.Drawing.Size(500, 300);
                }
                else
                {
                    button.Image = global::RfidClinic.Properties.Resources.swab_64;
                    button.ImageLocation = ImageLocation.Default;
                    button.Size = new System.Drawing.Size(500, 100);
                }
                button.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                button.LookAndFeel.SkinMaskColor = System.Drawing.Color.GhostWhite;
                //button.LookAndFeel.SkinName = "DevExpress Dark Style";
                button.LookAndFeel.UseDefaultLookAndFeel = false;
                //button.Name = "btnOk";
                button.Text = "Poli Umum";
                button.Tag = "test";// <--Store it in Tag
                //button.Enabled = false;
                //flowLayoutPanel1.Controls.Add(button);
                button.Click += btnNew_Click;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SimpleButton clickedButton = (SimpleButton)sender;
            Process.Start((string)clickedButton.Tag);
        }

        //public void check_rfid()
        //{
        //    string temp = "";
        //    try
        //    {
        //        if (serialPort1.IsOpen)

        //        serialPort1.Close();
        //        temp = temp + " " + Convert.ToString(serialPort1.PortName);
        //        temp = temp + " " + Convert.ToString(serialPort1.BaudRate);
        //        temp = temp + " " + Convert.ToString(serialPort1.DataBits);
        //        temp = temp + " " + Convert.ToString(serialPort1.StopBits);
        //        temp = temp + " " + Convert.ToString(serialPort1.Parity);
        //        temp = temp + " " + Convert.ToString(serialPort1.Handshake);
        //        richTextBox1.Text = temp;
        //        serialPort1.Open();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR: " + ex.Message);
        //        Blinking("Check COM Port RFID!", 0);
        //    }
        //}

        //private void port_DataReceived_scanner(object sender, SerialDataReceivedEventArgs e)
        //{
        //    Thread.Sleep(500);
        //    InputData_scanner = serialPort1.ReadExisting();
        //    if (textScanOut.Enabled == true)
        //    {
        //        if (InputData_scanner != String.Empty)
        //        {
        //            this.BeginInvoke(new SetTextCallback(SetText_scanner), new object[] { InputData_scanner });
        //        }
        //    }

        //}

        //private void SetText_scanner(string text)
        //{
        //    this.textScanOut.Text = "";
        //    this.textScanOut.Text = text;
        //    text = text.Replace("\u0002", "");
        //    text = text.Replace("\r\n", "");
        //    //text = text.Replace("\n", "");
        //    text = text.Replace("\u0003", "");
        //    this.textRecvTemp.Text = text;
        //    this.lRfid.Text = text;
        //}

        //private void textRecvTemp_TextChanged_1(object sender, EventArgs e)
        //{
        //    if (textRecvTemp.Text != "")
        //    {
        //        check_rfid(textRecvTemp.Text);
        //    }
        //}

        //private void check_rfid(string rfid)
        //{
        //    string sql_check, sql_cnt;

        //    sql_check = " ";
        //    //sql_check = sql_check + " select empid, name, lpad(rfid,10,'0') as rfid, gender from tthcm.view_cl_emp@DL_TTERGTOTTHCMIF a where lpad(rfid,10, '0') = '" + rfid + "' ";
        //    sql_check = sql_check + " select patient_no, name, lpad(rfid_no,10,'0') as rfid, gender from cs_patient_info a where lpad(rfid_no,10, '0') = '" + rfid + "' ";

        //    loading.ShowWaitForm();
        //    try
        //    {
        //        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
        //        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
        //        DataTable dt = new DataTable();
        //        adOra.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {

        //            lEmpid.Text = dt.Rows[0]["patient_no"].ToString();
        //            lName.Text = dt.Rows[0]["name"].ToString();
        //            lRfid.Text = dt.Rows[0]["rfid"].ToString();
        //            gender = dt.Rows[0]["gender"].ToString();

        //            //sql_cnt = " select count(empid) cnt from cs_visit where empid = '" + lEmpid.Text + "' and to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and status not in ('CLS','CAN') ";
        //            sql_cnt = " select count(a.patient_no) cnt, max(a.que01) que01, max(b.poli_name) poli_name from cs_visit a, cs_policlinic b  where a.poli_cd = b.poli_cd AND b.status = 'A' and a.patient_no = '" + lEmpid.Text + "' and trunc(a.visit_date)= trunc(sysdate) and a.status not in ('CLS','CAN')  ";
        //            // PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

        //            try
        //            {
        //                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
        //                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
        //                DataTable dt2 = new DataTable();
        //                adOra2.Fill(dt2);
        //                visit_cnt = dt2.Rows[0]["cnt"].ToString();
        //                if (Convert.ToInt32(visit_cnt) > 0)
        //                {
        //                    lPurpose.Text = dt2.Rows[0]["poli_name"].ToString();
        //                    Blinking("DALAM PEMERIKSAAN", 0);
        //                    lInfo.Text = "Scan gagal. Anda dalam proses pemeriksaan. No. Antrian : " + dt2.Rows[0]["que01"].ToString() + "";
                            
        //                }
        //                else
        //                {
        //                    Blinking("SCAN ID OK", 1);
        //                    lInfo.Text = "Silahkan pilih tujuan Anda";
        //                    load_List("1", true);
                            
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //loading.CloseWaitForm();
        //                MessageBox.Show("ERROR: " + ex.Message);
        //                loading.CloseWaitForm();
        //            }
        //        }
        //        else
        //        {
        //            Blinking("DATA TIDAK DITEMUKAN", 0);
        //            lEmpid.Text = "-";
        //            lName.Text = "-";
        //            lRfid.Text = "-";
        //            lPurpose.Text = "-";
        //            lInfo.Text = "Silahkan menghubungi petugas reservasi";
        //            load_List("1", false);
        //        }
        //        loading.CloseWaitForm();
        //    }
        //    catch (Exception ex)
        //    {
        //        //loading.CloseWaitForm();
        //        MessageBox.Show("ERROR: " + ex.Message);
        //        loading.CloseWaitForm();
        //    }
        //}

        private void Blinking(String Message, int mbOk)
        {
            lsMSG = Message;
            lsOK = mbOk;
            timerStart.Interval = 150;
            timerStart.Enabled = true;
            timer1.Interval = 2000;
            timer1.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 2000;
            timer2.Interval = 4000;
            timer2.Enabled = true;
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            //if (lsOK == 0)
            //{
            //    if (bl_klap == true)
            //    {
            //        lStatus.Appearance.ForeColor = Color.Red;
            //        lStatus.Text = lsMSG;
            //        lStatus.Visible = true;
            //        bl_klap = false;
            //    }
            //    else
            //    {
            //        bl_klap = true;
            //        lStatus.Visible = false;
            //    }
            //}
            //else
            //{
            //    if (bl_klap == true)
            //    {
            //        lStatus.Appearance.ForeColor = Color.ForestGreen;
            //        lStatus.Text = lsMSG;
            //        lStatus.Visible = true;
            //        bl_klap = false;
            //    }
            //    else
            //    {
            //        lStatus.Visible = false;
            //        bl_klap = true;
            //    }

            //}
        }

        private void timerEnd_Tick(object sender, EventArgs e)
        {
            //timerStart.Enabled = false;
            //timerEnd.Enabled = false;
            //lStatus.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            lInfo.Text = "Selamat Datang " + "\r\n" + "Silahkan Tentukan Poli yang Anda Tuju";
            lbl_noantrian.Text = "No Antrian";
        }

        private void typeRsv(string policd)
        {
            string purpose = "", sql_cnt = "", v_cnt = "", v_est = "", pic="";

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select poli_pic, poli_name  ";
            SQL = SQL + Environment.NewLine + "  from CS_POLICLINIC ";
            SQL = SQL + Environment.NewLine + " where poli_cd = '" + policd + "' ";
            SQL = SQL + Environment.NewLine + "   and status='A' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            purpose = dt.Rows[0]["poli_name"].ToString();
            pic = dt.Rows[0]["poli_pic"].ToString();

            lPurpose.Text = purpose;

            sql_cnt = "";
            // PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

            sql_cnt = sql_cnt + Environment.NewLine + "select count(0) cnt, count(0) * 4 as est  ";
            sql_cnt = sql_cnt + Environment.NewLine + "from cs_visit ";
            sql_cnt = sql_cnt + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd') ";
            sql_cnt = sql_cnt + Environment.NewLine + "and purpose = '" + pic + "' ";
            sql_cnt = sql_cnt + Environment.NewLine + "and poli_cd = '" + policd + "' ";
            sql_cnt = sql_cnt + Environment.NewLine + "and status in ('PRE','RSV','NUR') ";


            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);
                v_cnt = dt2.Rows[0]["cnt"].ToString();
                v_est = dt2.Rows[0]["est"].ToString();
                if (Convert.ToInt32(v_est) > 60)
                {
                    //Warning warning = new Warning();
                    //warning.p_cnt = v_cnt;
                    //warning.p_est = v_est;
                    //warning.ShowDialog();
                    //warning.Focus();

                    //if (warning.p_select == "")
                    //{
                    //    //MessageBox.Show("Cancel");
                    //}
                    //else
                    //{
                        //MessageBox.Show("OK");
                        reservation(pic, policd);
                    //}
                }
                else
                {
                    reservation(pic, policd);
                }
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
            }


            //for (int i = 0; i <= 1; i++)
            //{
            //    SoundPlayer player = new SoundPlayer("D:\\TT17100003\\Project\\Clinic\\Program\\Clinic\\Clinic\\Resources\\SCAN_ID_FIRST.wav");
            //    SoundPlayer player2 = new SoundPlayer(Properties.Resources.OK);
            //    player.Play();
            //    Thread.Sleep(2000);
            //    player2.Play();
            //    Thread.Sleep(2000);
            //}
        }

        private void typeAct(string policd)
        {
            string purpose = "", sql_cnt = "", v_cnt = "", v_est = "", pic="";

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select poli_pic, poli_name  ";
            SQL = SQL + Environment.NewLine + "from CS_POLICLINIC ";
            SQL = SQL + Environment.NewLine + "where poli_cd = '" + policd + "' ";
            SQL = SQL + Environment.NewLine + "and status='A' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            purpose = dt.Rows[0]["poli_name"].ToString();
            pic = dt.Rows[0]["poli_pic"].ToString();
            poliname = dt.Rows[0]["poli_name"].ToString();
            lPurpose.Text = purpose;


            sql_cnt = "";
            // PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

            sql_cnt = sql_cnt + Environment.NewLine + "select count(0) cnt, count(0) * 4 as est  ";
            sql_cnt = sql_cnt + Environment.NewLine + "  from cs_visit ";
            sql_cnt = sql_cnt + Environment.NewLine + " where to_char(visit_date,'yyyy-mm-dd') = to_char(sysdate,'yyyy-mm-dd') ";
            sql_cnt = sql_cnt + Environment.NewLine + "   and purpose = '" + pic + "' ";
            sql_cnt = sql_cnt + Environment.NewLine + "   and poli_cd = '" + policd + "' ";
            sql_cnt = sql_cnt + Environment.NewLine + "   and status in ('PRE','RSV','NUR') ";


            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);
                v_cnt = dt2.Rows[0]["cnt"].ToString();
                v_est = dt2.Rows[0]["est"].ToString();
                if (Convert.ToInt32(v_est) > 60)
                {
                    Warning warning = new Warning();
                    warning.p_cnt = v_cnt;
                    warning.p_est = v_est;
                    warning.ShowDialog();
                    warning.Focus();

                    if (warning.p_select == "")
                    {
                        //MessageBox.Show("Cancel");
                    }
                    else
                    {
                        //MessageBox.Show("OK");
                        reservation(pic, policd);
                    }
                }
                else
                {
                    reservation(pic, policd);
                }
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
            }
            //reservation(purpose);
        }

        private void reservation(string purpose, string policd)
        {
            string sql_check = "", tmp_purpose = "", tmp_queue = "",   sql_check5 = "";
            string sql_insert= "", sql_cnt = "", rm_number ="", teks="";
            int visit, queue, tmp_visit_no = 0;

            string SQL = " ";
            SQL = SQL + Environment.NewLine + "select poli_pic, poli_name  ";
            SQL = SQL + Environment.NewLine + "from CS_POLICLINIC ";
            SQL = SQL + Environment.NewLine + "where poli_cd = '" + policd + "' ";
            SQL = SQL + Environment.NewLine + "and status='A' ";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(SQL, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5); 

            poliname = dt5.Rows[0]["poli_name"].ToString();

            // PRE RSV NUR INS MED OBS CLS CAN  
            //poliname = "";
            //if (purpose == "DOC")
            //{
            //    tmp_purpose = "DOC";
            //    c_que = "D";
            //}
            //else if (purpose == "MID")
            //{
            //    tmp_purpose = "MID";
            //    c_que = "M";
            //}
            //else if (purpose == "ETC")
            //{
            //    tmp_purpose = "ETC";
            //    c_que = "E";
            //}

            //sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and purpose = '" + tmp_purpose + "' ";

            //sql_check = " select  nvl(max(to_number(substr(que,2,3))),0) que from cs_call_log where trunc(ins_date)= trunc(sysdate) and POLI_CD = '" + policd + "' ";

            //if (policd == "POL0001") 
            //    c_que = "D"; 
            //else if (policd == "POL0002") 
            //    c_que = "M"; 
            //else if (policd == "POL0003") 
            //    c_que = "K";
            //else if (policd == "POL0006")
            //    c_que = "G";
            //else if (purpose == "ETC") 
            //    c_que = "E"; 

            //sql_check = " ";
            //sql_check = sql_check + "  select  nvl(max(to_number(substr(que,2,3))),0) que from cs_call_log a, CS_POLICLINIC b ";
            //sql_check = sql_check + "   where a.POLI_CD = b.POLI_CD ";
            //sql_check = sql_check + "     and to_char(a.ins_date, 'yyyy-mm-dd')= to_char(sysdate, 'yyyy-mm-dd') ";
            //sql_check = sql_check + "     and  a.POLI_CD = '" + policd + "'   ";

            //try
            //{
            //    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //    OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
            //    DataTable dt = new DataTable();
            //    adOra.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //        tmp_queue = dt.Rows[0]["que"].ToString();
            //    else
            //        tmp_queue = "0";

            //    queue = Convert.ToInt32(tmp_queue) + 1;
            //    que = queue.ToString();
            //    if (queue < 10)
            //    {
            //        que = que.PadLeft(que.Length + 2, '0');
            //    }
            //    else if (queue < 100)
            //    {
            //        que = que.PadLeft(que.Length + 1, '0');
            //    }
            //    //poliname = dt.Rows[0]["POLI_NAME"].ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ERROR: " + ex.Message);
            //} 


            sql_check = " ";
            sql_check = sql_check + "  select  KLINIK.CS_GET_ANTRIAN('" + policd + "') as que from dual ";
            //sql_check = sql_check + "   where a.POLI_CD = b.POLI_CD ";
            //sql_check = sql_check + "     and to_char(a.ins_date, 'yyyy-mm-dd')= to_char(sysdate, 'yyyy-mm-dd') ";
            //sql_check = sql_check + "     and  a.POLI_CD = '" + policd + "'   ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            if (dt.Rows.Count > 0)
                tmp_queue = dt.Rows[0]["que"].ToString();
            else
                return;


            teks = "Nomor Antrian " + tmp_queue + " silahkan menuju Pendaftaran"; 

            sql_insert = "";
            sql_insert = sql_insert + " insert into cs_call_log (call_id, que, type_ins, stat, param, flag, ins_emp, ins_date, POLI_CD) ";
            sql_insert = sql_insert + " values (cs_call_log_seq.nextval, '" + tmp_queue + "','REG','Pendaftaran','" + teks + "','W','Antrian',sysdate, '" + policd +"')";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect2);
                oraConnect2.Open();
                cm.ExecuteNonQuery();
                oraConnect2.Close();
                cm.Dispose();

                //string sql_anamnesa_id = " select cs_anamnesa_seq.nextval cnt from dual";
                //OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_anamnesa_id, oraConnect2);
                //DataTable dt3 = new DataTable();
                //adOra3.Fill(dt3);
                //int anamnesa_id = Convert.ToInt32(dt3.Rows[0]["cnt"].ToString());

                //string sql_ins  = " insert into KLINIK.cs_anamnesa (anamnesa_id, rm_no, insp_date, visit_no, ins_date, ins_emp, ID_VISIT) values(" + anamnesa_id + ", '" + rm_number + "', trunc(sysdate), '" + c_que + que + "', sysdate, '" + lEmpid.Text + "', " + tmp_visit_no + ") ";

                //OleDbConnection oraConnec = ConnOra.Create_Connect_Ora();
                //OleDbCommand cm3 = new OleDbCommand(sql_ins, oraConnec);
                //oraConnec.Open();
                //cm3.ExecuteNonQuery();
                //oraConnec.Close();
                //cm3.Dispose();

                //MessageBox.Show(sql_insert);
                //MessageBox.Show("Query Exec : " + sql);
                Blinking("RESERVASI BERHASIL", 1);
                lInfo.Text = "Silahkan Menunggu " + "\r\n" + "Ditempat yang sudah disediakan. ";
                lbl_noantrian.Text = tmp_queue ;
                //loading.CloseWaitForm();


                PrintDocument printDocument = new PrintDocument();

                printDocument.PrinterSettings.PrinterName = "XP-80";
                printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, 315);
                printDocument.DefaultPageSettings.Margins = new Margins(5, 5, 0, 5);

                printDocument.PrintPage += new PrintPageEventHandler(PrintPage);
                printDocument.EndPrint += new PrintEventHandler(PrintDocument_EndPrint);

                try
                {
                    printDocument.Print();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                //Thread.Sleep(1200);
                //lInfo.Text = "Selamat Datang, Silahkan Tentukan Poli yang Anda Tuju";
                //lbl_noantrian.Text = "No Antrian";
                //lInfo.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Tentukan font dan warna
            Font font = new Font("Arial", 14, FontStyle.Bold);
            Font fontLarge = new Font("Arial", 28, FontStyle.Bold);
            Brush brush = Brushes.Black; 

            Image logo = Properties.Resources.Logo_Santosa;
            string queueNumber = lbl_noantrian.Text;
            string line2 = poliname.ToString();

            // Tentukan teks yang ingin dicetak
            string[] lines = {
                                "Klinik Santosa",
                                "NO ANTRIAN",
                                queueNumber,
                                line2
                            };

            // Mendapatkan lebar dan tinggi area cetak
            float pageWidth = e.PageBounds.Width;
            float pageHeight = e.PageBounds.Height;

            // Menghitung posisi X untuk memusatkan gambar logo
            float logoWidth = 80; // atur lebar logo
            float logoHeight = 40; // atur tinggi logo
            float logoX = ((pageWidth - logoWidth) / 2)-15;
            float logoY = 1; // jarak dari atas ke logo

            // Cetak logo
            e.Graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);

            // Mengatur posisi awal untuk teks setelah logo
            float startY = logoY + logoHeight ;

            // Cetak setiap baris teks
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                Font currentFont = (i == 2) ? fontLarge : font; // Ukuran lebih besar untuk nomor antrian

                // Hitung lebar teks untuk memusatkan secara horizontal
                float textWidth = e.Graphics.MeasureString(line, currentFont).Width;
                float startX = ((pageWidth - textWidth) / 2) -15;

                // Cetak teks di posisi (startX, startY)
                e.Graphics.DrawString(line, currentFont, brush, startX, startY);

                // Update posisi Y untuk baris berikutnya
                startY += currentFont.GetHeight(e.Graphics) + 3;
            }

            // Set e.HasMorePages ke false untuk menghentikan pencetakan setelah satu halaman
            e.HasMorePages = false;


            ////string logoPath = "Logo-Clicic.ico"; // Ganti dengan path file logo Anda
            ////Image logo = Image.FromFile(logoPath);
            //// Posisi dan ukuran logo
            //int logoWidth = 30; // Sesuaikan lebar logo
            //int logoHeight = 30; // Sesuaikan tinggi logo
            //int logoX = e.MarginBounds.Left; // Posisi X logo
            //int logoY = e.MarginBounds.Top-20; // Posisi Y logo

            //// Cetak logo
            //e.Graphics.DrawImage(logo, new Rectangle(logoX, logoY, logoWidth, logoHeight));

            //// Posisi awal teks setelah logo
            //float textX = logoX + logoWidth + 10; // Teks di samping logo
            //float y = logoY;

            //// Cetak nama klinik di sebelah logo
            //string clinicName = "Klinik Santosa";
            //Font clinicFont = new Font("Arial", 10, FontStyle.Bold);
            //e.Graphics.DrawString(clinicName, clinicFont, Brushes.Black, textX, y);

            //// Pindahkan posisi y ke bawah setelah logo dan nama klinik
            //y += 40;

            //// Pengaturan teks berikutnya
            //string line1 = "NO ANTRIAN";
           


            //// Font untuk teks
            //Font lineFont = new Font("Arial", 10, FontStyle.Regular);
            //Font queueFont = new Font("Arial", 24, FontStyle.Bold);

            //// Cetak "NO ANTRIAN" di bawah nama klinik
            //e.Graphics.DrawString(line1, lineFont, Brushes.Black, e.MarginBounds.Left + 50, y);
            //y += 30;

            //// Cetak nomor antrian
            //e.Graphics.DrawString(queueNumber, queueFont, Brushes.Black, e.MarginBounds.Left + 50, y);
            //y += 50;

            //// Cetak "POLI UMUM"
            //e.Graphics.DrawString(line2, lineFont, Brushes.Black, e.MarginBounds.Left + 50, y);
            //e.HasMorePages = false;
            // Buang objek gambar setelah selesai
            logo.Dispose(); 
        }
        private static void PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            // Kirim perintah pemotong otomatis setelah pencetakan selesai
            SendCutCommand("XP-80"); // Ganti dengan nama printer Anda
        }
        private static void SendCutCommand(string printerName)
        {
            IntPtr hPrinter;
            DOCINFOA di = new DOCINFOA();
            di.pDocName = "Auto-Cut";
            di.pDataType = "RAW";

            if (OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, ref di))
                {
                    StartPagePrinter(hPrinter);

                    // Kode ESC/POS untuk auto-cutter (untuk banyak printer thermal: ESC i atau ESC m)
                    byte[] cutCommand = { 0x1B, 0x69 }; // ESC i (kode ini bisa bervariasi, tergantung printer Anda)

                    IntPtr pUnmanagedBytes = Marshal.AllocCoTaskMem(cutCommand.Length);
                    Marshal.Copy(cutCommand, 0, pUnmanagedBytes, cutCommand.Length);

                    // Deklarasikan dwWritten sebelum digunakan
                    int dwWritten;
                    WritePrinter(hPrinter, pUnmanagedBytes, cutCommand.Length, out dwWritten);

                    Marshal.FreeCoTaskMem(pUnmanagedBytes);

                    EndPagePrinter(hPrinter);
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
        }
        private void lRfid_Click(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureEdit2_Click(object sender, EventArgs e)
        {

            string SQL = "", att="";
            SQL = SQL + Environment.NewLine + "select code_id, code_name,  ";
            SQL = SQL + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05 ";
            SQL = SQL + Environment.NewLine + "from CS_CODE_DATA ";
            SQL = SQL + Environment.NewLine + "where code_class_id='RESV_ITEM' ";
            SQL = SQL + Environment.NewLine + "and status='A' ";
            SQL = SQL + Environment.NewLine + "and attr_02='" + attr01 + "' ";
            SQL = SQL + Environment.NewLine + "order by sort_order asc ";


            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);

                if (dt2.Rows.Count > 0)
                {
                    att = dt2.Rows[0]["attr_01"].ToString();
                    if (att == "")
                    {
                        load_List("1", false);
                    }
                    else
                    {
                        load_List(att, true);
                    }
                }
                
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }

            
        }

        private void pictureEdit2_EditValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
