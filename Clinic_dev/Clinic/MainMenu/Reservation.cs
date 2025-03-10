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
using Newtonsoft.Json.Linq;
using Clinic.Class.Bpjsws;

namespace Clinic
{
    public partial class Reservation : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        string nobpjs = ""; private LabelControl _currentLabel;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reservation));

        string InputData_scanner = String.Empty;
        delegate void SetTextCallback(string text);
        DataTable DataDokter = null; DataTable DataPasien = null;
        string lsMSG = ""; 
        int lsOK = 0;
        bool bl_klap = true;
        string visit_cnt = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        bool p_enable;
        string id = "", poli = "", attr01 = "", attr02 = "", attr03 = "", attr04 = "", attr05 = "", gender = "";
        string que = "", c_que = "", poliname="", TPoli ="";
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
            lInfo.Text = "Selamat Datang " + "\r\n" + ""; //Silahkan Tentukan Poli yang Anda Tuju
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

            if(p_attr.ToString().Equals("2") && TPoli.ToString().Equals("BPJS"))
            {
                nobpjs = "";
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

                //LayoutControl layoutControl = new LayoutControl
                //{
                //    Dock = DockStyle.Fill
                //};
                //this.Controls.Add(layoutControl);

                // Membuat TextBox untuk input angka
                //TextBox inputBox = new TextBox
                //{
                //    Name = "inputBox",
                //    Multiline = true,
                //    Height = 120,  // Menetapkan tinggi secara langsung
                //    Width = 450,   // Menetapkan lebar secara langsung
                //    Font = new System.Drawing.Font("Arial", 28, FontStyle.Bold),
                //    TextAlign = HorizontalAlignment.Center 
                //};

                TextEdit inputBox = new TextEdit
                {
                    Name = "inputBox", 
                    Height = 120,  // Menetapkan tinggi secara langsung
                    Width = 450,   // Menetapkan lebar secara langsung
                    Font = new System.Drawing.Font("Arial", 28, FontStyle.Bold) 
                };

                //inputBox.Properties.Appearance.Font = new System.Drawing.Font("Arial", 24);
                inputBox.Properties.AutoHeight = false;
                inputBox.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                // Menambahkan TextBox ke LayoutControl
                LayoutControlItem inputBoxItem = group1.AddItem("INPUT NIK KTP/NO. BPJS", inputBox);
                inputBoxItem.TextLocation = DevExpress.Utils.Locations.Top;
                inputBoxItem.AppearanceItemCaption.Font = new System.Drawing.Font("Arial", 26, FontStyle.Bold);
                inputBoxItem.AppearanceItemCaption.ForeColor = Color.YellowGreen;

                // Menetapkan ukuran kustom untuk LayoutControlItem
                inputBoxItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
                inputBoxItem.MinSize = new System.Drawing.Size(450, 120); // Ukuran minimal
                inputBoxItem.MaxSize = new System.Drawing.Size(450, 120); // Ukuran maksimal 

                // Membuat Panel untuk menampung tombol Numpad
                PanelControl numpadPanel = new PanelControl();
                //group1.AddItem("", numpadPanel);
                //numpadPanel.Text.vis = false;

                LayoutControlItem numpadItem = group1.AddItem(string.Empty, numpadPanel);
                numpadItem.TextLocation = DevExpress.Utils.Locations.Top;
                numpadItem.AppearanceItemCaption.Font = new System.Drawing.Font("Arial", 48, FontStyle.Bold);
                numpadItem.AppearanceItemCaption.ForeColor  =  Color.MediumBlue;
                numpadItem.Text = " ";
                //numpadItem.TextVisible = false;

                // Posisi tombol numpad
                int x = 10, y = 10;
                for (int i = 0; i < 10; i++)
                {
                    SimpleButton numButton = new SimpleButton
                    {
                        Text = i.ToString(),
                        Width = 80,
                        Height = 80,
                        Font = new System.Drawing.Font("Arial", 14,FontStyle.Bold),
                        Location = new System.Drawing.Point(x, y)
                    };

                    // Ketika tombol numpad ditekan
                    numButton.Click += (btnSender, btnE) =>
                    {
                        inputBox.Text += numButton.Text;
                    };

                    numpadPanel.Controls.Add(numButton);

                    // Penataan posisi tombol (grid layout)
                    x += 80;
                    if (x > 320)
                    {
                        x = 10;
                        y += 80;
                    }
                }

                SimpleButton backspaceButton = new SimpleButton
                {
                    Text = "←",  // Backspace symbol
                    Width = 80,
                    Height = 80,
                    Font = new System.Drawing.Font("Arial", 14, FontStyle.Bold),
                    Location = new System.Drawing.Point(x, y)
                };

                backspaceButton.Click += (backspaceSender, backspaceE) =>
                {
                    if (inputBox.Text.Length > 0)
                    {
                        inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1);
                    }
                };

                //// Tombol Clear untuk menghapus input
                //SimpleButton clearButton = new SimpleButton
                //{
                //    Text = "Clear",
                //    Width = 50,
                //    Height = 50,
                //    Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold),
                //    Location = new System.Drawing.Point(x, y)
                //};
                //clearButton.Click += (clearSender, clearE) =>
                //{
                //    inputBox.Text = string.Empty;
                //};
                numpadPanel.Controls.Add(backspaceButton);

                // Tombol OK
                SimpleButton okButton = new SimpleButton
                {
                    Text = "OK",
                    Width = 80,
                    Height = 80,
                    Font = new System.Drawing.Font("Arial", 14, FontStyle.Bold),
                    Location = new System.Drawing.Point(x + 80, y) // Menambahkan tombol OK di samping tombol Clear
                };

                okButton.Click += (clearSender, clearE) =>
                {
                    // Menangani aksi setelah tombol OK diklik
                    //MessageBox.Show($"Input Anda: {inputBox.Text}", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nobpjs = inputBox.Text;
                    CekPeserta(nobpjs);
                };

                numpadPanel.Controls.Add(okButton);

                labelControl3.Text = "SILAHKAN INPUT NO BPJS/KTP ANDA";

                numpadPanel.MaximumSize = new Size(340, 270);
                numpadPanel.MinimumSize = new Size(340, 270);

                layoutControl1.Root.Add(group1);
                //layoutControl1.AddGroup(group1);
                int aa = group1.Items.Count;
                layoutControl1.EndUpdate();

                pictureEdit2.Visible = true; 
            } 
            else
            {
                if (p_attr.ToString().Equals("3") && TPoli.ToString().Equals("BPJS"))
                {
                    p_attr = "2";
                }

                string SQL = "";
                SQL = "";
                SQL = SQL + Environment.NewLine + "select code_id, code_name,  ";
                SQL = SQL + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05 ";
                SQL = SQL + Environment.NewLine + "from CS_CODE_DATA ";
                SQL = SQL + Environment.NewLine + "where code_class_id='ANTRIAN'  ";
                SQL = SQL + Environment.NewLine + "and status='A' ";
                SQL = SQL + Environment.NewLine + "and attr_01='" + p_attr + "' ";
                if (p_attr.ToString().Equals("2"))
                    SQL = SQL + Environment.NewLine + "and attr_05 ='" + TPoli + "' AND ATTR_04 ='R'";
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
                        if (tot <= 3)
                        {
                            if (attr05 == "BPJS")
                            {
                                button.Image = global::Clinic.Properties.Resources.BPJS1;
                            }
                            else if (attr05 == "UMUM")
                            {
                                button.Image = global::Clinic.Properties.Resources.UMUM1;
                            }
                            else if (attr05 == "ASURANSI")
                            {
                                button.Image = global::Clinic.Properties.Resources.ASURANSI1;
                            }
                            //else if (attr05 == "MCU")
                            //{
                            //    button.Image = global::RfidClinic.Properties.Resources.checkup1_256;
                            //}
                            //else
                            //{
                            //    button.Image = global::RfidClinic.Properties.Resources.checkup1_256;
                            //}
                            //labelControl3.Text = "NOMOR ANTRIAN " + TPoli + " ANDA";
                            button.ImageLocation = ImageLocation.TopCenter;
                            button.Size = new System.Drawing.Size(500, 300);
                        }
                        else
                        {
                            if (attr05 == "BPJS")
                            {
                                button.Image = global::Clinic.Properties.Resources.doctor_m64;
                            }
                            else if (attr05 == "UMUM")
                            {
                                button.Image = global::Clinic.Properties.Resources.doctor_f64;
                            }
                            else if (attr05 == "ASURANSI")
                            {
                                button.Image = global::Clinic.Properties.Resources.checkup1_64;
                            }
                            else if (attr05 == "MCU")
                            {
                                button.Image = global::Clinic.Properties.Resources.checkup1_64;
                            }
                            else
                            {
                                button.Image = global::Clinic.Properties.Resources.checkup1_64;
                            }

                            button.ImageLocation = ImageLocation.Default;
                            button.Size = new System.Drawing.Size(350, 100);
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
                        if (tot <= 3)
                        {
                            itemBtn.MaxSize = new Size(450, 170);
                            itemBtn.MinSize = new Size(450, 170);
                            TPoli = ""; button.Text = "";
                        }
                        else
                        {
                            itemBtn.MaxSize = new Size(300, 180);
                            itemBtn.MinSize = new Size(300, 180);
                        }
                        p_enable = p_bol;
                        itemBtn.StartNewLine = false;

                        button.Click += layoutControlItem1_Click;
                    }

                    labelControl3.Text = "NOMOR ANTRIAN " + TPoli + " ANDA";

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
        }
        public void CekPeserta(string nomor)
        {
            if (nomor.ToString().Trim().Length < 13)
            {
                MessageBox.Show("Nomor BPJS Anda Tidak Benar.");
                return;
            }

            if (nomor.ToString().Trim().Length >= 14 && nomor.ToString().Trim().Length < 16)
            {
                MessageBox.Show("Nomor KTP Anda Tidak Benar.");
                return;
            }
            else if (nomor.ToString().Trim().Length > 16)
            {
                MessageBox.Show("Nomor KTP Anda Tidak Benar.");
                return;
            }

            string SQL = " ";
            SQL = SQL + Environment.NewLine + "select NID, INSU_NO from CS_PATIENT_INFO ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            if (nomor.ToString().Trim().Length == 13)
                SQL = SQL + Environment.NewLine + "  and INSU_NO ='" + nomor + "' ";
            else if (nomor.ToString().Trim().Length == 16)
                SQL = SQL + Environment.NewLine + "  and NID  ='" + nomor + "' ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(SQL, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            if (dt2.Rows.Count > 0)
            {
                load_List("3", true);
            }
            else
            {
                //if(RunAsyncBPJS())
                //{
                    RunAsyncBPJS();
                    load_List("3", true);
                //}
                
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

                    if (clickedButton.Name.ToString().Equals("001"))
                    {

                    }
                    string SQL = "", vcode_id="", vcode_name="", vattr_01="", vattr_02 = "", vattr_03 = "";
                    string vattr_04 = "", vattr_05 = "", vattr_06 = "";
                     
                    SQL = SQL + Environment.NewLine + "select code_id, code_name,  ";
                    SQL = SQL + Environment.NewLine + "attr_01, attr_02, attr_03, attr_04, attr_05, attr_06 ";
                    SQL = SQL + Environment.NewLine + "from CS_CODE_DATA ";
                    SQL = SQL + Environment.NewLine + "where code_class_id='ANTRIAN' ";
                    SQL = SQL + Environment.NewLine + "and status='A' AND code_id ='" + clickedButton.Name + "' ";
                    //SQL = SQL + Environment.NewLine + "and attr_02 ='" + 1 + "' ";
                    SQL = SQL + Environment.NewLine + "order by sort_order asc ";

                    if (clickedButton.Name.ToString().Equals("001"))
                        TPoli = "BPJS";
                    else if (clickedButton.Name.ToString().Equals("002"))
                        TPoli = "UMUM";
                    else if (clickedButton.Name.ToString().Equals("003"))
                        TPoli = "ASURANSI";
                    //else
                    //    TPoli = "";

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
                            //typeRsv(vattr_03,TPoli);
                            InsertAntrian(vattr_03, TPoli, clickedButton.Name);
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

        private void InsertAntrian(string policd, string SPoli, string SCode)
        {
            string sql_check = "", tmp_queue ="" , NIK = "", nohp = "", kodepoli = "", namapoli = "", norm = "", teks ="";
            string sql_insert = "", sql_cnt = "", tanggalperiksa = "",  namadokter ="", jampraktek="", nomorantrean="",   keterangan= "";
            int visit, queue, tmp_visit_no, angkaantrean = 0, kodedokter =0;
            if (SPoli.ToString().Equals("BPJS"))
            {
                sql_check = " ";
                sql_check = sql_check + "  select QUE from CS_CALL_LOG where trunc(ins_date) = trunc(sysdate) and no_bpjs =  '" + nobpjs + "'  and POLI_CD = '" + policd + "' ";  //select  KLINIK.CS_GET_ANTRIAN_POLI('" + policd + "', '" + SPoli + "', '" + SCode + "') as que from dual ";

                OleDbConnection oraCon1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_check, oraCon1);
                DataTable dt1 = new DataTable();
                adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                { 
                    lInfo.Text = "GAGAL. ID SUDAH TERDAFTAR";
                    Blinking(lInfo, 0);
                    lbl_noantrian.Text = dt1.Rows[0]["que"].ToString();
                    return;
                }
            } 


            sql_check = " ";
            sql_check = sql_check + "  select  KLINIK.CS_GET_ANTRIAN_POLI('" + policd + "', '" + SPoli + "', '" + SCode + "') as que from dual ";
            
            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);
            if (dt.Rows.Count > 0)
                tmp_queue = dt.Rows[0]["que"].ToString();
            else
                return;

            if (SPoli.ToString().Equals("BPJS"))
            {
                
                angkaantrean = Convert.ToInt32(tmp_queue.Substring(1, 3));
                nomorantrean = tmp_queue.Substring(0, 1) + "-" + angkaantrean;
                string SQL = "";
                SQL = SQL + Environment.NewLine + "select distinct to_char(tgl_jadwal,'YYYY-MM-DD') tanggalperiksa, d.BPJS_KODE_POLI BPJS_KODE_POLI, d.BPJS_NAMA_POLI, nvl(b.BPJS_ID_DOKTER,0) BPJS_ID_DOKTER , b.BPJS_NAMA_DOKTER, a.JAM_AWAL||'-'||a.JAM_AKHIR jampraktek     ";
                SQL = SQL + Environment.NewLine + "  from CS_DOKTER_SCH a ";
                SQL = SQL + Environment.NewLine + "  join CS_DOKTER b on (a.ID_DOKTER = b.BPJS_ID_DOKTER) ";
                SQL = SQL + Environment.NewLine + "  left join CS_DOKTER c on (a.ID_PENGGANTI = c.ID_DOKTER) ";
                SQL = SQL + Environment.NewLine + "  join CS_POLICLINIC d on (a.poli_cd = d.BPJS_KODE_POLI) ";
                SQL = SQL + Environment.NewLine + " where trunc(tgl_jadwal) = trunc(sysdate) ";
                SQL = SQL + Environment.NewLine + "   and d.poli_cd = '" + policd + "'  ";

                DataDokter = ConnOra.Data_Table_ora(SQL);

                if(DataDokter.Rows.Count > 0)
                {
                    tanggalperiksa = DataDokter.Rows[0]["tanggalperiksa"].ToString();
                    kodepoli = DataDokter.Rows[0]["BPJS_KODE_POLI"].ToString();
                    namapoli = DataDokter.Rows[0]["BPJS_NAMA_POLI"].ToString();
                    kodedokter = Convert.ToInt32(DataDokter.Rows[0]["BPJS_ID_DOKTER"].ToString()); 
                    namadokter = DataDokter.Rows[0]["BPJS_NAMA_DOKTER"].ToString();
                    jampraktek = DataDokter.Rows[0]["jampraktek"].ToString();
                }

                string SQL2 = "";
                SQL2 = SQL2 + Environment.NewLine + "  select NID, PHONE, REPLACE(PATIENT_NO,'P','') NORM from cs_patient_info ";
                SQL2 = SQL2 + Environment.NewLine + "   where 1=1  ";
                if (nobpjs.ToString().Trim().Length == 13)
                    SQL2 = SQL2 + Environment.NewLine + "  and INSU_NO ='" + nobpjs + "' ";
                else if (nobpjs.ToString().Trim().Length == 16)
                    SQL2 = SQL2 + Environment.NewLine + "  and NID  ='" + nobpjs + "' ";

                DataPasien = ConnOra.Data_Table_ora(SQL2);
                if (DataPasien.Rows.Count > 0)
                {
                    NIK = DataPasien.Rows[0]["NID"].ToString();
                    nohp = DataPasien.Rows[0]["PHONE"].ToString();
                    norm = DataPasien.Rows[0]["NORM"].ToString(); 
                }

                // struktur json
                JObject json = new JObject();
                json.Add("nomorkartu", nobpjs);                 //nobpjs
                json.Add("nik", NIK);                           //NIK
                json.Add("nohp", nohp);                         //nohp
                json.Add("kodepoli", kodepoli);                 //kodepoli
                json.Add("namapoli", namapoli);                 //namapoli
                json.Add("norm", norm);                         //norm
                json.Add("tanggalperiksa", tanggalperiksa);     //tanggalperiksa
                json.Add("kodedokter", kodedokter);            //kodedokter
                json.Add("namadokter",  namadokter);            //namadokter
                json.Add("jampraktek", jampraktek);             //jampraktek
                json.Add("nomorantrean", nomorantrean);         //nomorantrean
                json.Add("angkaantrean", Convert.ToInt32(angkaantrean));             //angkaantrean
                json.Add("keterangan", keterangan);             //keterangan
                //json.Add("waktu", Clinic.Class.Bpjsws.Bpjsws.CurrentUnixTime);

                // kirim ke bpjs
                // jika gagal langsung munculkan error dan aplikasi terhenti
                // jika berhasil system meneruskan penyimpanan seperti biasanya
                BpjswsResponse resp = BpjswsAntrol.TambahAntrean(json);
                if (resp.Metadata.Code != 200)
                {
                    MessageBox.Show($"Code: { resp.Metadata.Code }, Message: { resp.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                } 
            } 


            teks = "Nomor Antrian " + tmp_queue + " silahkan menuju Pendaftaran";

            sql_insert = "";
            sql_insert = sql_insert + " insert into cs_call_log (call_id, que, type_ins, stat, param, flag, ins_emp, ins_date, POLI_CD, STYPE,NO_BPJS,KD_DOKTER) ";
            sql_insert = sql_insert + " values (cs_call_log_seq.nextval, '" + tmp_queue + "','REG','Pendaftaran','" + teks + "','W','Antrian',sysdate, '" + policd + "', decode('" + SPoli + "','BPJS','B','UMUM','U','ASURANSI','A') ";
            if (SPoli.ToString().Equals("BPJS"))
            {
                sql_insert = sql_insert + "   , '" + nobpjs + "' ";
            }
            else
            {
                sql_insert = sql_insert + " ,'' ";
            }
            sql_insert = sql_insert + "  ," + kodedokter + " )";
            //loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect2);
                oraConnect2.Open();
                cm.ExecuteNonQuery();
                oraConnect2.Close();
                cm.Dispose();
                lInfo.Text = "RESERVASI BERHASIL";
                Blinking(lInfo, 1);
                //lInfo.Text = "Silahkan Menunggu " + "\r\n" + "Ditempat yang sudah disediakan. ";
                lbl_noantrian.Text = tmp_queue;
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

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
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
                    button.Image = global::Clinic.Properties.Resources.doctor_f256;
                    button.ImageLocation = ImageLocation.TopCenter;
                    button.Size = new System.Drawing.Size(500, 300);
                }
                else
                {
                    button.Image = global::Clinic.Properties.Resources.swab_64;
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


        //private void Blinking(String Message,    int mbOk)
        //{

        //    lsMSG = Message;
        //    lsOK = mbOk;
        //    timerStart.Interval = 150;
        //    timerStart.Enabled = true;
        //    timer1.Interval = 2000;
        //    timer1.Enabled = true;

        //    timerEnd.Enabled = true;
        //    timerEnd.Interval = 2000;
        //    timer2.Interval = 4000;
        //    timer2.Enabled = true;
        //}

        private void Blinking(LabelControl ctrl, int mbOk)
        {
            //lsMSG = Message;
            lsOK = mbOk;
            _currentLabel = ctrl;
            timerStart.Interval = 150;
            timerStart.Enabled = true;
            //timer1.Interval = 2000;
            //timer1.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 3000;
            //timer3.Interval = 4000;
            //timer3.Enabled = true;
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {

            if (lsOK == 0)
            {
                if (bl_klap == true)
                {
                    _currentLabel.Appearance.ForeColor = Color.Red;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    bl_klap = true;
                    _currentLabel.Visible = false;
                }
            }
            else
            {
                if (bl_klap == true)
                {
                    _currentLabel.Appearance.ForeColor = Color.ForestGreen;
                    _currentLabel.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    _currentLabel.Visible = false;
                    bl_klap = true;
                }
            }
        }

        private void timerEnd_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            timerEnd.Enabled = false;
            _currentLabel.Visible = false ;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            lInfo.Text = "Selamat Datang " + "\r\n" + "Silahkan Tentukan Poli yang Anda Tuju";
            lbl_noantrian.Text = "No Antrian";
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void typeRsv(string policd, string SPoli)
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
  
            sql_check = " ";
            sql_check = sql_check + "  select  KLINIK.CS_GET_ANTRIAN_POLI('" + policd + "') as que from dual ";
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

                lInfo.Text = "RESERVASI BERHASIL";
                Blinking(lInfo, 1);

                //Blinking("RESERVASI BERHASIL", 1);
                //lInfo.Text = "Silahkan Menunggu " + "\r\n" + "Ditempat yang sudah disediakan. ";
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
            Reservation_Load(sender, e);
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
                if (attr01.ToString().Equals("1") && TPoli.ToString().Equals("BPJS"))
                {
                    load_List("1", true);
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
        //public void RunAsyncBPJS()
        //{
        //    InsertThreadExample dbHelper = new InsertThreadExample();
        //    await dbHelper.InsertDataAsync("John Doe", 30);
        //}

         
        //public void RunAsyncBPJS()
        //{
        //    BpjswsResponse resp;
        //    string tmp_pas_no = "";

        //    Task.Run(() =>
        //    {
        //    using (OleDbConnection conn = ConnOra.Create_Connect_Ora())
        //        {
        //            OleDbTransaction trans = null;
        //            try
        //            {
        //                OleDbCommand command = new OleDbCommand();
        //                command.Connection = conn;
        //                conn.Open();
        //                Console.WriteLine("Koneksi berhasil dibuka.");

        //                resp = BpjswsPcare.GetPeserta(nobpjs);
        //                if (resp.Metadata.Code != 200)
        //                {
        //                    Console.WriteLine($"Code: {resp.Metadata.Code}, Message: {resp.Metadata.Message}");
        //                    return false;
        //                }

        //                JObject jsonObj = JObject.Parse(resp?.GetResponseString());
        //                JObject response = (JObject)jsonObj["Response"];

        //                // Ambil nomor pasien
        //                string sql_cnt = "SELECT 'P' || TO_CHAR(SYSDATE, 'yymm') || LPAD(COUNT(0)+1, 3, '0') AS pno FROM cs_patient_info WHERE TO_CHAR(ins_date, 'yyyymm') = TO_CHAR(SYSDATE, 'yyyymm')";
        //                OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, conn);
        //                DataTable dt4 = new DataTable();
        //                adOra4.Fill(dt4);
        //                tmp_pas_no = dt4.Rows[0]["pno"].ToString();

        //                if (dt4.Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        //                        Console.WriteLine("Transaksi dimulai...");

        //                        string query = @"INSERT INTO cs_patient_info (patient_no, nid, NAME, gender, birth_date, insu_no, insu_class,
        //                                                    TGL_MULAI_AKTIF_BPJS, TGL_AKHIR_BERLAKU_BPJS, JENIS_PESERTA_BPJS, JENIS_PESERTA_KODE_BPJS, STATUS_BPJS, KET_STATUS_BPJS, TUNGGAKAN_BPJS,PHONE,GOL_DARAH,HUB_KELUARGA, INS_DATE, INS_EMP)
        //                                VALUES (?, ?, ?, ?, TO_DATE(?, 'DD-MM-YYYY'), ?, ?,
        //                                        TO_DATE(?, 'DD-MM-YYYY'), TO_DATE(?, 'DD-MM-YYYY'),
        //                                        ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE, 'BPJS PCARE')";

        //                        using (OleDbCommand cmd = new OleDbCommand(query, conn, trans))
        //                        {
        //                            cmd.Parameters.AddWithValue("?", tmp_pas_no);
        //                            cmd.Parameters.AddWithValue("?", response["noKTP"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["nama"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["sex"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", DateTime.ParseExact(response["tglLahir"].ToString(), "dd-MM-yyyy", null).ToString("dd-MM-yyyy"));
        //                            cmd.Parameters.AddWithValue("?", response["noKartu"]?.ToString() ?? "0");
        //                            cmd.Parameters.AddWithValue("?", response["jnsKelas"]["kode"]?.ToString() ?? "0");
        //                            cmd.Parameters.AddWithValue("?", DateTime.ParseExact(response["tglMulaiAktif"].ToString(), "dd-MM-yyyy", null).ToString("dd-MM-yyyy"));
        //                            cmd.Parameters.AddWithValue("?", DateTime.ParseExact(response["tglAkhirBerlaku"].ToString(), "dd-MM-yyyy", null).ToString("dd-MM-yyyy"));
        //                            cmd.Parameters.AddWithValue("?", response["jnsPeserta"]["nama"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["jnsPeserta"]["kode"]?.ToString() ?? "0");
        //                            cmd.Parameters.AddWithValue("?", response["aktif"] != null && (bool)response["aktif"] ? 1 : 0);
        //                            cmd.Parameters.AddWithValue("?", response["ketAktif"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["tunggakan"] != null ? Convert.ToInt32(response["tunggakan"]) : 0);
        //                            cmd.Parameters.AddWithValue("?", response["noHP"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["golDarah"]?.ToString() ?? "");
        //                            cmd.Parameters.AddWithValue("?", response["hubunganKeluarga"]?.ToString() ?? "");

        //                            int rowsAffected = cmd.ExecuteNonQuery();
        //                            Console.WriteLine($"Insert sukses! {rowsAffected} baris ditambahkan.");
        //                        }

        //                        trans.Commit();
        //                        Console.WriteLine("Transaksi berhasil di-commit."); 
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        trans.Rollback();
        //                        Console.WriteLine("Error saat insert: " + ex.Message);
        //                        return false;
        //                    }
        //                }
        //                conn.Close();
        //                Console.WriteLine("Koneksi ditutup."); 
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //                return false ;
        //            }
        //        }
        //    });
        //    return true;
        //}

        public void RunAsyncBPJS()
        {
            BpjswsResponse resp;
            string tmp_pas_no = "";

            Task.Run(() =>
            {
                using (OleDbConnection conn = ConnOra.Create_Connect_Ora())
                {
                    try
                    {

                        OleDbCommand command = new OleDbCommand();
                        OleDbTransaction trans = null;

                        command.Connection = conn;
                        conn.Open();

                        // struktur json
                        //JObject json = new JObject();
                        //json.Add("param", nobpjs);

                        // kirim ke bpjs
                        // jika gagal langsung dilewati
                        // jika berhasil system akan menyimpan data ke table CS_PATIENT_INFO
                        resp = BpjswsPcare.GetPeserta(nobpjs);
                        if (resp.Metadata.Code != 200)
                        {
                            MessageBox.Show($"Code: { resp.Metadata.Code }, Message: { resp.Metadata.Message }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conn.Close();
                            return;
                        }

                        JObject jsonObj = JObject.Parse(resp?.GetResponseString());
                        JObject response = (JObject)jsonObj["Response"];

                        //switch (url)
                        //{
                        //case Class.Bpjsws.Bpjsws.WS_PCARE_PESERTA_GET_URL:
                        //    resp = Class.Bpjsws.BpjswsPcare.GetPeserta(txtParam1.Text);
                        //    if (resp != null) txtResponse.Text = resp?.GetResponseString();
                        //    else txtResponse.Text = "Unknown error! please call the administrator";
                        //    break;

                        //string query = "INSERT INTO BPJS (Nama, NoBPJS) VALUES (@nama, @noBPJS)";

                        //using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        //{
                        //    cmd.Parameters.AddWithValue("@nama", "John Doe");
                        //    cmd.Parameters.AddWithValue("@noBPJS", "123456789");

                        //    cmd.ExecuteNonQuery();
                        //} 

                        string sql_cnt = " select 'P' || to_char(sysdate,'yymm') || lpad(count(0)+1,3,'0') pno from cs_patient_info where to_char(ins_date, 'yyyymm') = to_char(sysdate, 'yyyymm')  ";
                        OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, conn);
                        DataTable dt4 = new DataTable();
                        adOra4.Fill(dt4);
                        tmp_pas_no = dt4.Rows[0]["pno"].ToString();
                        if (Convert.ToInt32(dt4.Rows.Count) > 0)
                        {
                            //using (OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora())
                            //{
                            //conn.Open();
                            //OleDbTransaction trans = conn.BeginTransaction(); // Mulai transaksi
                            try
                            {
                                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                                //string query = @" INSERT INTO BPJS_TABLE (NO_KARTU, NAMA, HUBUNGAN_KELUARGA, SEX, TGL_LAHIR, 
                                //                                        TGL_MULAI_AKTIF, TGL_AKHIR_BERLAKU, KELAS_NAMA, KELAS_KODE, 
                                //                                        JENIS_PESERTA, JENIS_PESERTA_KODE, NO_KTP, AKTIF, KET_AKTIF, TUNGGAKAN)
                                //                VALUES (?, ?, ?, ?, TO_DATE(?, 'DD-MM-YYYY'),
                                //                        TO_DATE(?, 'DD-MM-YYYY'), TO_DATE(?, 'DD-MM-YYYY'),
                                //                        ?, ?, ?, ?, ?, ?, ?, ?)";
                                string query = @" INSERT INTO cs_patient_info (patient_no, nid,NAME, gender, birth_date, insu_no,insu_class,
                                                                            TGL_MULAI_AKTIF_BPJS, TGL_AKHIR_BERLAKU_BPJS, JENIS_PESERTA_BPJS, JENIS_PESERTA_KODE_BPJS, STATUS_BPJS,KET_STATUS_BPJS,TUNGGAKAN_BPJS,HUB_KELUARGA,KD_PROVIDER,NM_PROVIDER, INS_DATE,INS_EMP,STATUS )
                                                    VALUES (?, ?, ?, ?, TO_DATE(?, 'DD-MM-YYYY'),?,?,
                                                            TO_DATE(?, 'DD-MM-YYYY'), TO_DATE(?, 'DD-MM-YYYY'),
                                                            ?, ?, ?, ?, ? ,?, ?, ? ,SYSDATE,'BPJS PCARE','A')";

                                using (OleDbCommand cmd = new OleDbCommand(query, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("?", (string)tmp_pas_no);
                                    cmd.Parameters.AddWithValue("?", (string)response["noKTP"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["nama"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["sex"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["tglLahir"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["noKartu"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["jnsKelas"]["kode"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["tglMulaiAktif"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["tglAkhirBerlaku"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["jnsPeserta"]["nama"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["jnsPeserta"]["kode"]);
                                    cmd.Parameters.AddWithValue("?", (bool)response["aktif"] ? 1 : 0);
                                    cmd.Parameters.AddWithValue("?", (string)response["ketAktif"]);
                                    cmd.Parameters.AddWithValue("?", (int)response["tunggakan"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["hubunganKeluarga"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["kdProviderPst"]["kdProvider"]);
                                    cmd.Parameters.AddWithValue("?", (string)response["kdProviderPst"]["nmProvider"]);

                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    Console.WriteLine($"Insert sukses! {rowsAffected} baris ditambahkan."); 
                                }

                                string query2 = @"  insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) 
                                                    VALUES (?, ?, ?, ?, SYSDATE,'BPJS PCARE')";

                                using (OleDbCommand cmd2 = new OleDbCommand(query2, conn, trans))
                                {
                                    cmd2.Parameters.AddWithValue("?", (string)tmp_pas_no.Replace("P",""));
                                    cmd2.Parameters.AddWithValue("?", (string)tmp_pas_no);
                                    cmd2.Parameters.AddWithValue("?", (string)"COMM");
                                    cmd2.Parameters.AddWithValue("?", (string)"A"); 

                                    int rowsAffected2 = cmd2.ExecuteNonQuery();
                                    Console.WriteLine($"Insert sukses! {rowsAffected2} baris ditambahkan.");
                                }

                                trans.Commit(); // Commit transaksi jika berhasil
                                                //Console.WriteLine("Transaksi berhasil!");
                                                //command.CommandText = " insert into cs_patient_info (patient_no, nid, name, birth_place, birth_date, gender, address, " +
                                                //                            " city, insu_no, status, job, family_head, " +
                                                //                            " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp) values " +
                                                //                            " ( '" + tmp_pas_no + "', '" + ktp + "','" + nama + "',  '" + tmt_lahir + "',to_date('" + tgl_lahir.Substring(0, 10).ToString() + "','dd/MM/yyyy'),'" + jk + "','" + alamat + "', " +
                                                //                            " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "', '" + kk + "', " +
                                                //                            " '" + nohp + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "') ";

                                //    command.ExecuteNonQuery();

                                //    command.CommandText = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) " +
                                //                            " values ('C' || to_char(sysdate,'yymmdd') || replace('" + tmp_pas_no + "','P'), '" + tmp_pas_no + "', 'COMM', 'A', sysdate, '" + DB.vUserId + "') ";

                                //    command.ExecuteNonQuery();

                                //    trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                //MessageBox.Show("ERROR: " + ex.Message);
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            //oraConnectTrans.Close();
                            //}
                            //catch (Exception ex)
                            //{
                            //    transaction.Rollback(); // Rollback jika ada error
                            //    Console.WriteLine("Transaksi dibatalkan! Error: " + ex.Message);
                            //}

                        }
                        conn.Close();
                        Console.WriteLine("Insert berhasil!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            });
        }
    }
}
