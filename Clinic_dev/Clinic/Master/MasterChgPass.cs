using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Clinic
{
    public partial class MasterChgPass : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> userStatus = new List<FlagYn>();
        List<Medicine> listRole = new List<Medicine>();
        DataTable dtGlRole = new DataTable();

        public string  v_name = "";
        string ssql = "", pass ="", sqlupd = ""; 

        public MasterChgPass()
        {
            InitializeComponent();
        }
         
        private void initData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
         
        private void MasterChgPass_Load(object sender, EventArgs e)
        {
            initData();
            textBox1.Text = DB.vUserId;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SavePasswor();
            }
        }

        private void btnAddDosis_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SavePasswor();
            }
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            SavePasswor();
        }
        private void SavePasswor()
        {
            ssql = " ";
            ssql = " select a.user_id, name, user_role, pass from KLINIK.cs_user a where status = 'A' and UPPER(a.user_id) = UPPER('" + textBox1.Text + "') ";

            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(ssql, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DB.vUserId = dt.Rows[0]["user_id"].ToString();
                    DB.vUserName = dt.Rows[0]["name"].ToString();
                    DB.vUserRole = dt.Rows[0]["user_role"].ToString();
                    pass = dt.Rows[0]["pass"].ToString();

                    if (textBox1.Text.ToString().ToUpper() == DB.vUserId.ToString().ToUpper() && textBox2.Text.ToString().ToUpper() == pass.ToString().ToUpper())
                    {
                        if (textBox2.Text.ToString().ToUpper() == textBox3.Text.ToString().ToUpper())
                        {
                            MessageBox.Show("Password Baru tidak boleh sama dengan Password Lama...!!", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error); textBox3.Text = ""; textBox3.Focus(); return; 
                        }
                        else if (textBox3.Text.ToString().ToUpper() != textBox4.Text.ToString().ToUpper())
                        { 
                            MessageBox.Show("Password Baru dan Confirm Password tidak sama...!!", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error); textBox4.Text = ""; textBox4.Focus(); return;
                        }
                        else if (textBox3.Text.ToString().ToUpper() == textBox4.Text.ToString().ToUpper() && textBox2.Text.ToString().ToUpper() != textBox4.Text.ToString().ToUpper())
                        {
                            sqlupd = " ";
                            sqlupd = " update  KLINIK.cs_user  set pass = '" + textBox3.Text.ToString() + "' where   status = 'A' and UPPER(user_id) = UPPER('" + textBox1.Text + "') ";

                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sqlupd, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();
                             
                            MessageBox.Show("Password Berhasil di Ganti.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
                        }
                    }
                    else
                    { 
                        MessageBox.Show("Pergantian Password Gagal. Password salah...!!!", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = ""; textBox2.Focus();
                    }
                }
                else
                { 
                    MessageBox.Show("Pergantian Password Gagal. Koneksi tidak stabil...!!", "Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = ""; textBox2.Focus();
                }

            }
            catch (Exception ex)
            { 
                MessageBox.Show("ERROR: " + ex.Message); 
                textBox2.Text = ""; 
            }
        }
    
    } 
}