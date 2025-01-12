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
using System.Speech.Synthesis;
using System.Media;

namespace Clinic
{
    public partial class ReservationScan : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        string InputData_scanner = String.Empty;
        delegate void SetTextCallback(string text);

        string lsMSG = "";
        int lsOK = 0;
        bool bl_klap = true;
        string visit_cnt = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");


        public ReservationScan()
        {
            InitializeComponent();
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_scanner);
        }

        private void ReservationScan_Load(object sender, EventArgs e)
        {
            check_rfid();
            btnDoc.Enabled = false;
            btnMid.Enabled = false;
            lInfo.Text = "Selamat Datang, silahkan scan ID Anda untuk mendaftar";
        }

        public void check_rfid() {
            string temp="";
            try
            {
                if (serialPort1.IsOpen)

                serialPort1.Close();
                temp = temp +" " + Convert.ToString(serialPort1.PortName);
                temp = temp + " " + Convert.ToString(serialPort1.BaudRate);
                temp = temp + " " + Convert.ToString(serialPort1.DataBits);
                temp = temp + " " + Convert.ToString(serialPort1.StopBits);
                temp = temp + " " + Convert.ToString(serialPort1.Parity);
                temp = temp + " " + Convert.ToString(serialPort1.Handshake);
                richTextBox1.Text = temp;
                serialPort1.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                Blinking("Check COM Port RFID!", 0);
            }
        }

        private void port_DataReceived_scanner(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            InputData_scanner = serialPort1.ReadExisting();
            if (textScanOut.Enabled == true)
            {
                if (InputData_scanner != String.Empty)
                {
                    this.BeginInvoke(new SetTextCallback(SetText_scanner), new object[] { InputData_scanner });
                }
            }

        }

        private void SetText_scanner(string text)
        {
            this.textScanOut.Text = "";
            this.textScanOut.Text = text;
            text = text.Replace("\u0002", "");
            text = text.Replace("\r\n", "");
            text = text.Replace("\u0003", "");
            this.textRecvTemp.Text = text;
            this.lRfid.Text = text;
        }

        private void textRecvTemp_TextChanged(object sender, EventArgs e)
        {
            if (textRecvTemp.Text != "")
            {
                check_rfid(textRecvTemp.Text);
            }
        }

        private void check_rfid(string rfid)
        {
            string sql_check, sql_cnt, gender ="";

            sql_check = " ";
            //sql_check = sql_check + " select empid, name, lpad(rfid,10,'0') as rfid, gender from tthcm.view_cl_emp@DL_TTERGTOTTHCMIF a where lpad(rfid,10, '0') = '" + rfid + "' ";
            sql_check = sql_check + " select empid, name, lpad(rfid,10,'0') as rfid, gender from cs_employees a where lpad(rfid,10, '0') = '" + rfid + "' ";

            loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    
                    lEmpid.Text = dt.Rows[0]["empid"].ToString();
                    lName.Text = dt.Rows[0]["name"].ToString();
                    lRfid.Text = dt.Rows[0]["rfid"].ToString();
                    gender = dt.Rows[0]["gender"].ToString();
                    

                    if (gender == "Laki-laki")
                    {
                        btnDoc.Enabled = true;
                        btnMid.Enabled = false;
                    }
                    else
                    {
                        btnDoc.Enabled = true;
                        btnMid.Enabled = true;
                    }

                    sql_cnt = " select count(empid) cnt from cs_visit where empid = '" + lEmpid.Text + "' and to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and status not in ('CLS','CAN') ";
                    // PRE, RSV, NUR, INS, OBS, MED, CLS, CAN

                    try
                    {
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        visit_cnt = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(visit_cnt) > 0)
                        {
                            Blinking("DALAM PEMERIKSAAN", 0);
                            lInfo.Text = "Scan gagal. Anda dalam proses pemeriksaan";
                            btnDoc.Enabled = false;
                            btnMid.Enabled = false;

                        }
                        else
                        {
                            Blinking("SCAN ID OK", 1);
                            lInfo.Text = "Silahkan pilih tujuan Anda";
                        }
                    }
                    catch (Exception ex)
                    {
                        //loading.CloseWaitForm();
                        MessageBox.Show("ERROR: " + ex.Message);
                        loading.CloseWaitForm();
                    }
                }
                else
                {
                    Blinking("DATA TIDAK DITEMUKAN", 0);
                    lEmpid.Text = "-";
                    lName.Text = "-";
                    lRfid.Text = "-";
                    lPurpose.Text = "-";
                    btnDoc.Enabled = false;
                    btnMid.Enabled = false;
                    lInfo.Text = "Silahkan menghubungi petugas reservasi";
                }
                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
            }
        }

        private void Blinking(String Message, int mbOk)
        {
            lsMSG = Message;
            lsOK = mbOk;
            timerStart.Interval = 150;
            timerStart.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 2000;

        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            if (lsOK == 0)
            {
                if (bl_klap == true)
                {
                    lStatus.Appearance.ForeColor = Color.Red;
                    lStatus.Text = lsMSG;
                    lStatus.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    bl_klap = true;
                    lStatus.Visible = false;
                }
            }
            else
            {
                if (bl_klap == true)
                {
                    lStatus.Appearance.ForeColor = Color.ForestGreen;
                    lStatus.Text = lsMSG;
                    lStatus.Visible = true;
                    bl_klap = false;
                }
                else
                {
                    lStatus.Visible = false;
                    bl_klap = true;
                }

            }
        }

        private void timerEnd_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            timerEnd.Enabled = false;
            lStatus.Visible = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string purpose = "";
            purpose = "Dokter";
            lPurpose.Text = purpose;
            reservation(purpose);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string purpose = "";
            purpose = "Bidan";
            lPurpose.Text = purpose;
            reservation(purpose);
        }


        private void reservation(string purpose)
        {
            string sql_check="", tmp_purpose="",tmp_queue="", que="", c_que="";
            string sql_insert;
            int visit, queue;

            // PRE RSV NUR INS MED OBS CLS CAN  

            if (purpose == "Dokter")
            {
                tmp_purpose = "DOC";
                c_que = "D";
            }
            else
            {
                tmp_purpose = "MID";
                c_que = "M";
            }

            sql_check = " select  nvl(max(to_number(substr(que01,2,3))),0) que from cs_visit where to_char(visit_date,'yyyy-mm-dd')= '" + today + "' and purpose = '" + tmp_purpose + "' ";

            try
            {
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_check, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                tmp_queue = dt.Rows[0]["que"].ToString();
                queue = Convert.ToInt32(tmp_queue) + 1;
                que = queue.ToString();
                if (queue < 10)
                {
                    que = que.PadLeft(que.Length + 2, '0');
                }
                else if (queue < 100)
                {
                    que = que.PadLeft(que.Length + 1, '0');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }


            visit = Convert.ToInt32(visit_cnt) + 1;

            sql_insert = " insert into cs_visit (empid, visit_date, status, type_patient, work_accident, purpose, visit_cnt, que01, ins_date, ins_emp) values ('" + lEmpid.Text + "',sysdate, 'PRE', 'U', 'N', '" + tmp_purpose + "', '" + Convert.ToString(visit) + "', '" + c_que + que + "' , sysdate, '" + lEmpid.Text + "') ";

            loading.ShowWaitForm();
            try
            {
                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect2);
                oraConnect2.Open();
                cm.ExecuteNonQuery();
                oraConnect2.Close();
                cm.Dispose();

                //MessageBox.Show(sql_insert);
                //MessageBox.Show("Query Exec : " + sql);
                Blinking("RESERVASI BERHASIL", 1);
                lInfo.Text = "Silahkan menunggu ditempat yang sudah disediakan";
                btnDoc.Enabled = false;
                btnMid.Enabled = false;
                loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                loading.CloseWaitForm();
            }
        }
    }
}