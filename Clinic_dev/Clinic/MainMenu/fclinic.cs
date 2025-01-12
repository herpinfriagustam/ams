using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clinic
{
    public partial class fclinic : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();

        public fclinic()
        {
            InitializeComponent();
        }

        private void fclinic_Load(object sender, EventArgs e)
        {
            //txtUser.Text = "TT17100003";
            //txtPass.Text = "za0120";
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            
        }

        private void LoadData()
        {
            string empid = "", pass = "", sql_cek1 = "", sql_cek2 = "", name = "", role = "", sqlins ="";

            if (txtUser.Text == "" || txtPass.Text == "")
            {
                MessageBox.Show("Silahkan isi User ID dan Password");
            }
            else
            {
                sql_cek1 = " select a.user_id, name, user_role, pass, nik from KLINIK.cs_user a where status = 'A' and UPPER(a.user_id) = UPPER('" + txtUser.Text + "') ";

                //loading.ShowWaitForm();
                try
                {
                    OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adSql = new OleDbDataAdapter(sql_cek1, sqlConnect);
                    DataTable dt = new DataTable();
                    adSql.Fill(dt);
                    load.ShowWaitForm();
                    if (dt.Rows.Count > 0) 
                    {
                        DB.vUserId = dt.Rows[0]["user_id"].ToString();
                        DB.vUserName = dt.Rows[0]["name"].ToString();
                        DB.vUserRole = dt.Rows[0]["user_role"].ToString();
                        pass = dt.Rows[0]["pass"].ToString();
                        ConnOra.v_nik  = dt.Rows[0]["nik"].ToString();
                        if (txtUser.Text.ToString().ToUpper() == DB.vUserId.ToString().ToUpper() && txtPass.Text.ToString().ToUpper() == pass.ToString().ToUpper())
                        {
                            sqlins = ""; 
                            sqlins = sqlins + " insert into KLINIK.CS_HISTORY_LOGIN (SEQ_ID,USER_ID, IP_KOMPUTER,S_DATE,E_DATE) values (CS_HISTORY_LOGIN_SEQ.nextval,'" + DB.vUserId + "',  '" + ConnOra.my_IP + "',sysdate, null) "; 

                            try
                            {
                                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                                OleDbCommand cm = new OleDbCommand(sqlins, oraConnect);
                                oraConnect.Open();
                                cm.ExecuteNonQuery();
                                oraConnect.Close();
                                cm.Dispose();
                                 
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }

                            load.CloseWaitForm();
                            this.Hide();
                            ClinicMngt frm = new ClinicMngt();
                            frm.userEmpid = DB.vUserId;
                            frm.userName = DB.vUserName;
                            frm.userStatus = DB.vUserRole;
                            frm.ShowDialog();
                            frm.userStatus = "";
                            //this.Show();
                            this.Close();
                        }
                        else
                        {
                            load.CloseWaitForm();
                            MessageBox.Show("Login Gagal. Kombinasi User ID dan Password salah. ");
                            //txtUser.Text = "";
                            txtPass.Text = "";
                        }
                    }
                    else
                    {
                        load.CloseWaitForm();
                        MessageBox.Show("Login Gagal. Anda Belum terdaftar. ");
                        //txtUser.Text = "";
                        txtPass.Text = "";
                    }

                    //loading.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    //load.CloseWaitForm();
                    MessageBox.Show("ERROR: " + ex.Message);
                    //txtUser.Text = "";
                    txtPass.Text = "";
                    //loading.CloseWaitForm();
                }
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }
    }
}
