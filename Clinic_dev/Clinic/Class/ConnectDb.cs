using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors.Repository;
using System.Drawing;

namespace Clinic
{
    class ConnectDb
    {
        private OleDbConnection _ConnectOra;
        private OleDbConnection _ConnectSql;

        string _ConnectStringOra = "";
        string _ConnectStringSql = "";
        public string my_IP = "", v_iddokter = "", v_nik = "";
        public OleDbConnection Create_Connect_Ora()
        {
            //string _ConnectStringOra = "Provider=MSDAORA.1;Password=KLINIK;Persist Security Info=True;User ID=KLINIK;Data Source = localhost:1521/XE";
            string _ConnectStringOra = "Provider=MSDAORA.1;Password=KLINIK;Persist Security Info=True;User ID=KLINIK;Data Source = 192.168.1.99:1521/XE";

            try
            {
                _ConnectOra = new OleDbConnection(_ConnectStringOra);
                return _ConnectOra;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
            finally
            {
                if (_ConnectOra != null) _ConnectOra.Close();
            }
        }
        public DataTable Data_Table_ora(string sql)
        {
            OleDbConnection con = Create_Connect_Ora();
            OleDbDataAdapter ad = new OleDbDataAdapter(sql, con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        public bool ExeNonQuery(string query)
        {
            OleDbConnection con = Create_Connect_Ora();
            OleDbCommand cm = new OleDbCommand(query, con);
            try
            {
                con.Open();
                cm.ExecuteNonQuery();
                con.Close();
                cm.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LookUpGridFilter<T>(
                List<T> listsql,
                GridView gridviw,
                string scode,
                string sname,
                RepositoryItemGridLookUpEdit lokup,
                int col
            ) where T : class
        {
            // Set DataSource untuk lookup editor
            lokup.DataSource = listsql;
            lokup.ValueMember = scode;
            lokup.DisplayMember = sname;
            var gridView = lokup.View;
            gridView.OptionsView.ShowAutoFilterRow = true; // Tampilkan AutoFilterRow
            gridView.OptionsCustomization.AllowSort = true;

            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView.Columns)
            {
                column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            }
            if (gridView.Columns[scode] == null)
            {
                gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
                {
                    FieldName = scode,
                    Caption = scode,
                    Visible = true
                });
            }
            if (gridView.Columns[sname] == null)
            {
                gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
                {
                    FieldName = sname,
                    Caption = sname,
                    Visible = true
                });
            }
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.Columns[scode].Width = 110; // Kolom pertama
            gridView.Columns[sname].Width = 530;
            gridView.RowHeight = 27;
            gridView.Appearance.Row.Font = new Font("Arial", 11, FontStyle.Regular);        // Baris data
            gridView.Appearance.HeaderPanel.Font = new Font("Arial", 11, FontStyle.Bold);  // Header kolom
            gridView.Appearance.FocusedRow.Font = new Font("Arial", 11, FontStyle.Regular);

            lokup.PopupFormWidth = 700;
            lokup.ImmediatePopup = true;
            lokup.Appearance.Font = new Font("Arial", 11, FontStyle.Regular);
            lokup.Appearance.Options.UseFont = true;
            lokup.AppearanceDropDown.Font = new Font("Arial", 11, FontStyle.Regular);
            lokup.AppearanceDropDown.Options.UseFont = true;

            lokup.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            lokup.NullText = "";
            gridviw.Columns[col].ColumnEdit = lokup;
        }

        public void LookUpEditFilter<T>(
               List<T> listsql,
               GridLookUpEdit LokUpEdit,
               string scode,
               string sname,
               RepositoryItemGridLookUpEdit lokup,
               int col
           ) where T : class
        {
            // Set DataSource untuk lookup editor
            LokUpEdit.Properties.DataSource = listsql;
            //lokup.DataSource = listsql;
            LokUpEdit.Properties.ValueMember = scode;
            LokUpEdit.Properties.DisplayMember = sname;
            var gridView = LokUpEdit.Properties.View;
            gridView.OptionsView.ShowAutoFilterRow = true; // Tampilkan AutoFilterRow
            gridView.OptionsCustomization.AllowSort = true;

            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView.Columns)
            {
                column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            }
            if (gridView.Columns[scode] == null)
            {
                gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
                {
                    FieldName = scode,
                    Caption = scode,
                    Visible = true
                });
            }
            if (gridView.Columns[sname] == null)
            {
                gridView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
                {
                    FieldName = sname,
                    Caption = sname,
                    Visible = true
                });
            }
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.Columns[scode].Width = 110; // Kolom pertama
            gridView.Columns[sname].Width = 530;
            gridView.RowHeight = 27;
            gridView.Appearance.Row.Font = new Font("Arial", 11, FontStyle.Regular);        // Baris data
            gridView.Appearance.HeaderPanel.Font = new Font("Arial", 11, FontStyle.Bold);  // Header kolom
            gridView.Appearance.FocusedRow.Font = new Font("Arial", 11, FontStyle.Regular);

            LokUpEdit.Properties.PopupFormWidth = 700;
            LokUpEdit.Properties.ImmediatePopup = true;
            LokUpEdit.Properties.Appearance.Font = new Font("Arial", 11, FontStyle.Regular);
            LokUpEdit.Properties.Appearance.Options.UseFont = true;
            LokUpEdit.Properties.AppearanceDropDown.Font = new Font("Arial", 11, FontStyle.Regular);
            LokUpEdit.Properties.AppearanceDropDown.Options.UseFont = true;

            LokUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            LokUpEdit.Properties.NullText = "";
            //LokUpEdit.Properties.DataSource = listsql; 
        }
        public void LongTanggal(GridView gridviw, int col)
        {
            RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            rptanggal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            rptanggal.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            rptanggal.Mask.UseMaskAsDisplayFormat = true;
            gridviw.Columns[col].ColumnEdit = rptanggal;
        }
        public void ShortTanggal(GridView gridviw, int col)
        {
            RepositoryItemDateEdit rptanggal = new RepositoryItemDateEdit();
            rptanggal.DisplayFormat.FormatString = "yyyy-MM-dd";
            rptanggal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            rptanggal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            rptanggal.Mask.EditMask = "yyyy-MM-dd";
            rptanggal.Mask.UseMaskAsDisplayFormat = true;
            gridviw.Columns[col].ColumnEdit = rptanggal;
        }
    }
    //192.168.100.95 
    public class DB
    {
        public static string vUserId = "", vUserName = "", vUserRole = "";
        public static string vUserIP = "";

        static Oracle.ManagedDataAccess.Client.OracleConnection connection;

        static Oracle.ManagedDataAccess.Client.OracleConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    //connection = new Oracle.ManagedDataAccess.Client.OracleConnection(@"Data Source=(DESCRIPTION = 
                    //    (ADDRESS = 
                    //        (PROTOCOL = TCP)
                    //        (HOST = localhost )(PORT = 1521))
                    //    (CONNECT_DATA = (SERVER = DEDICATED)
                    //        (SERVICE_NAME = XE))
                    //    );User Id=KLINIK;Password=KLINIK;");

                    connection = new Oracle.ManagedDataAccess.Client.OracleConnection(@"Data Source=(DESCRIPTION = 
                        (ADDRESS = 
                            (PROTOCOL = TCP)
                            (HOST =  192.168.1.99 )(PORT = 1521))
                        (CONNECT_DATA = (SERVER = DEDICATED)
                            (SERVICE_NAME = XE))
                        );User Id=KLINIK;Password=KLINIK;");
                }
                return connection;
            }
        }

        public static DbResult GetDataTable(string sql)
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    return new DbResult(false, "Connection already in use, pleas try after several time!");

                Oracle.ManagedDataAccess.Client.OracleDataAdapter adapter = new Oracle.ManagedDataAccess.Client.OracleDataAdapter(sql, Connection);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);

                return new DbResult(true, dt);
            }
            catch (System.Exception ex)
            {
                return new DbResult(false, "Exception : " + ex.Message);
            }
        }

        public class DbResult
        {
            public bool Success { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }

            public DbResult() { }
            public DbResult(bool success, object data)
            {
                Success = success;
                Data = data;
                Message = "";
            }

            public DbResult(bool success, string message)
            {
                Success = success;
                Data = null;
                Message = message;
            }

            public System.Data.DataTable ToDataTable()
            {
                if (Data != null)
                    return Data as System.Data.DataTable;

                return null;
            }
        }
    }

    class KoneksiOra
    {
        private OleDbConnection connection;
        private string connectionString;
        private OleDbTransaction transaction;


        //private string server = "192.168.1.99"; //  "localhost/XE";// 
        //private string database = "localhost/XE";// 
        private string database = "192.168.1.99:1521/XE";
        private string uid = "KLINIK";
        private string password = "KLINIK";

        public OleDbConnection Connection

        {
            get { return connection; }
        }

        public KoneksiOra()
        {
            InitializeConnection();
        }
        //string _ConnectStringOra = "Provider=MSDAORA.1;Password=KLINIK;Persist Security Info=True;User ID=KLINIK;Data Source = localhost:1521/XE"; OraOLEDB.Oracle
        private void InitializeConnection()
        {
            connectionString = $"Provider = OraOLEDB.Oracle; Password = {password}; Persist Security Info = True; Connection Lifetime = 0; PLSQLRSet = True; User ID = {uid}; Data Source = {database}";
            connection = new OleDbConnection(connectionString);
        }

        public bool ExecuteNonQuery(string query)
        {
            try
            {
                OpenConnection();

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }



        public DataTable GetDataTable(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                OpenConnection();

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }



        public object GetScalar(string query)
        {
            try
            {
                OpenConnection();

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ExecuteNonQueryCommitRollback(string query)
        {
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(query, connection, transaction))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing non-query: " + ex.Message);
                return false;
            }
        }

        public void BeginTransaction()
        {
            OpenConnection();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Commit();
                    Console.WriteLine("Transaksi berhasil di-commit.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error committing transaction: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaksi di-rollback.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error rolling back transaction: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening connection: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening connection: " + ex.Message);
            }
        }
    }

    class ORADB
    {

        #region DBConncetion
        //public static OleDbConnection XE = NewConnection("localhost:1521/XE", "KLINIK", "KLINIK");
        public static OleDbConnection XE = NewConnection("192.168.1.99:1521/XE", "KLINIK", "KLINIK");

        #endregion

        static OleDbConnection _ConnectOra;
        public static OleDbConnection NewConnection(string dbAlias, string User, string Pass)
        {
            try
            {
                //_ConnectOra = new OleDbConnection("Provider=MSDAORA.1;Password=" + Pass + ";Persist Security Info=True;  Connection Lifetime = 0; PLSQLRSet = True;User ID=" + User + ";Data Source = " + dbAlias + "");
                _ConnectOra = new OleDbConnection("Provider=OraOLEDB.Oracle;Password=" + Pass + ";Persist Security Info=True; Connection Lifetime = 0; PLSQLRSet = True; User ID=" + User + ";Data Source = " + dbAlias + "");
                return _ConnectOra;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
            finally
            {
                if (_ConnectOra != null) _ConnectOra.Close();
            }
        }

        public static bool Execute(OleDbConnection db, String sql)
        {
            OleDbConnection con = db;
            OleDbCommand cm = new OleDbCommand(sql, con);
            try
            {
                con.Open();
                cm.ExecuteNonQuery();
                con.Close();
                cm.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void DbTrans(OleDbConnection con, List<String> sql)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Connection = con;
                cmd.Transaction = transaction;
                for (int i = 0; i < sql.Count; i++)
                {
                    cmd.CommandText = sql[i];
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                transaction.Rollback();
            }

            con.Close();
        }
        public static DataTable SetData(OleDbConnection Con, String sql)
        {
            OleDbConnection db = Con;
            OleDbDataAdapter ad = new OleDbDataAdapter(sql, db);
            DataTable dt = new DataTable();
            try
            {
                ad.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        public static string getData(OleDbConnection Con, string sql, string col)
        {
            string value = "";
            OleDbConnection db = Con;
            OleDbDataAdapter ad = new OleDbDataAdapter(sql, db);
            DataTable dt = new DataTable();
            try
            {
                ad.Fill(dt);
                value = dt.Rows[0][col].ToString();
                return value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return value;
            }
        }

    }

    class MD //Model Data
    {
        public static bool UpdateData(OleDbConnection db, string tableName, string condition, Dictionary<string, string> columnValues)
        {
            OleDbConnection con = db;
            try
            {
                con.Open();
                string setClause = "";
                foreach (var kvp in columnValues)
                {
                    setClause += $"{kvp.Key} = '{kvp.Value}', ";
                }
                setClause = setClause.TrimEnd(',', ' ');

                string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {condition}";

                OleDbCommand command = new OleDbCommand(updateQuery, con);
                command.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (OleDbException ex)
            {
                FN.errosMsg(ex.Message, "Update Failed");
                con.Close();
                return false;
            }
        }

    }

    class FN //Quick Functions
    {

        public static string strVal(GridView gv, int row, int col)
        {
            return gv.GetRowCellDisplayText(row, gv.Columns[col]);
        }

        public static string strVal(GridView gv, int row, string col)
        {
            return gv.GetRowCellDisplayText(row, col);
        }

        public static string rowVal(DataTable dt, string col)
        {
            return dt.Rows[0][col]?.ToString();
        }
        public static void splitVal(string data, RadioGroup rg)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 2)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
            }
        }
        public static object GetDataLook(DevExpress.XtraEditors.LookUpEdit Look, string sname, string value)
        {
            return Look.Properties.GetDataSourceValue(Look.Properties.ValueMember, Look.Properties.GetDataSourceRowIndex(sname, value));
        }

        public static void splitVal1(string data, RadioGroup rg, TextBox tx)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
                tx.Text = dt[2]?.ToString();
            }
        }
        public static void splitValJam(string data, RadioGroup rg, TextEdit tx)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
                tx.Text = dt[2]?.ToString();
            }
        }
        public static void splitText(string data, TextBox tx)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 3)
            {
                tx.Text = dt[1]?.ToString();
            }
        }
        public static void splitVal2(string data, Control containerControl, TextBox tx)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 2)
            {
                List<CheckBox> checkBoxes = containerControl.Controls.OfType<CheckBox>().ToList();
                SetCheckBoxValues(checkBoxes, dt[0].ToString());
                tx.Text = dt[1]?.ToString();
            }
        }

        public static void splitVal3(string data, RadioGroup rg, TextBox tx, TextBox tx2, TextBox tx3)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
                if (dt[2].ToString() == "")
                {
                    tx.Text = tx2.Text = tx3.Text = "";
                }
                else
                {
                    string[] dt1 = dt[2].Split(new string[] { "=>" }, StringSplitOptions.None);
                    tx.Text = dt1[0]?.ToString();
                    tx2.Text = dt1[1]?.ToString();
                    tx3.Text = dt1[2]?.ToString();
                }
            }
        }

        public static void splitVal4(string data, Control containerControl, RadioGroup rg, TextBox tx)
        {
            string[] dt = data.Split(new string[] { "::" }, StringSplitOptions.None);
            if (dt.Length == 4)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
                List<CheckBox> checkBoxes = containerControl.Controls.OfType<CheckBox>().ToList();
                SetCheckBoxValues(checkBoxes, dt[2].ToString());
                tx.Text = dt[3]?.ToString();
            }
            else if (dt.Length == 3)
            {
                rg.SelectedIndex = Convert.ToInt32(dt[0]);
                List<CheckBox> checkBoxes = containerControl.Controls.OfType<CheckBox>().ToList();
                SetCheckBoxValues(checkBoxes, dt[2].ToString());
            }
        }

        public static void setCheckList(string data, CheckedListBox checkedListBox)
        {
            string[] values = data.Split(',');
            foreach (string value in values)
            {
                int index = checkedListBox.Items.IndexOf(value);
                if (index != -1)
                {
                    checkedListBox.SetItemChecked(index, true);
                }
            }
        }

        static void SetCheckBoxValues(List<CheckBox> checkBoxes, string checkBoxValues)
        {
            string[] values = checkBoxValues.Split(',');

            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Checked = values.Contains(checkBox.Text);
            }
        }

        public static string getVal(Control containerControl, int type = 1)
        {
            string result = "";
            var sortedControls = containerControl.Controls.Cast<Control>().OrderBy(c => c.TabIndex);

            string radioVal = "";
            string checkBoxVal = "";
            string textBoxVal = "";
            string lookUpVal = "";

            bool isCheckboxExist;

            foreach (Control innerControl in sortedControls)
            {
                if (innerControl is RadioGroup)
                {
                    RadioGroup radioGroup = (RadioGroup)innerControl;
                    string selectedValue = radioGroup.EditValue?.ToString();
                    string labelText = radioGroup.Properties.Items.FirstOrDefault(item => item.Value.ToString() == selectedValue)?.Description ?? "";
                    radioVal = $"{radioGroup.SelectedIndex.ToString()}::{labelText}::";
                }
                else if (innerControl is CheckBox)
                {
                    CheckBox cb = (CheckBox)innerControl;
                    if (cb.Checked)
                        checkBoxVal += (checkBoxVal == "" ? "" : ",") + cb.Text;
                }
                else if (innerControl is TextBox)
                {
                    TextBox textBox = (TextBox)innerControl;
                    if (textBox.Tag?.ToString() != "exc")
                    {
                        if (textBox.Text != "")
                            textBoxVal += (textBoxVal == "" ? "" : "=>") + textBox.Text;
                    }
                }
                else if (innerControl is LookUpEdit)
                {
                    LookUpEdit look = (LookUpEdit)innerControl;
                    lookUpVal += look.EditValue?.ToString();
                }
            }

            //result = $"{(radioVal != "" ? radioVal : "")}{(checkBoxVal != "" ? checkBoxVal + "::" : "")}{(textBoxVal != "" ? textBoxVal : "")}{(lookUpVal != "" ? lookUpVal : "")}";

            if (type == 1)
            {
                result = $"{(radioVal != "" ? radioVal : "")}{(textBoxVal != "" ? textBoxVal : "")}";
            }
            else if (type == 2)
            {
                result = $"{(radioVal != "" ? radioVal : "")}{(checkBoxVal != "" ? checkBoxVal : "")}";
            }
            else if (type == 3)
            {
                result = $"{(radioVal != "" ? radioVal : "")}{(checkBoxVal != "" ? checkBoxVal : "")}::{(textBoxVal != "" ? textBoxVal : "")}";
            }
            else if (type == 4)
            {
                result = $"{(radioVal != "" ? radioVal : "")}{(lookUpVal != "" ? lookUpVal : "")}";
            }
            //else if (type == 5)
            //{
            //    result = $"{(checkBoxVal != "" ? checkBoxVal : "")}{(textBoxVal != "" ? textBoxVal : "")}";
            //}
            else
            {
                result = $"{(checkBoxVal != "" ? checkBoxVal : "")}::{(textBoxVal != "" ? textBoxVal : "")}";
            }

            return result;
        }

        public static string chkListOf(CheckedListBox checkedListBox)
        {
            string result = "";

            foreach (object item in checkedListBox.CheckedItems)
            {
                result += (result == "" ? "" : ",") + item.ToString();
            }

            return result;
        }

        public static string radioVal(RadioGroup rg)
        {
            string selectedValue = rg.EditValue?.ToString();
            string labelText = rg.Properties.Items.FirstOrDefault(item => item.Value?.ToString() == selectedValue)?.Description ?? "";
            return rg.SelectedIndex.ToString() + "::" + labelText;
        }

        public static string joinVal(RadioGroup rg, TextBox tb)
        {
            string selectedValue = rg.EditValue?.ToString();
            string labelText = rg.Properties.Items.FirstOrDefault(item => item.Value?.ToString() == selectedValue)?.Description ?? "";

            return $"{rg.SelectedIndex.ToString()}::{labelText}::{tb.Text?.ToString()}";
        }
        public static string joinVal2(RadioGroup rg, TextEdit tb)
        {
            string selectedValue = rg.EditValue?.ToString();
            string labelText = rg.Properties.Items.FirstOrDefault(item => item.Value?.ToString() == selectedValue)?.Description ?? "";

            return $"{rg.SelectedIndex.ToString()}::{labelText}::{tb.Text?.ToString()}";
        }

        public static void errosMsg(string msg, string tittle)
        {
            MessageBox.Show(msg, tittle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void EnableControls(Control parentControl, bool enabled, Control lastSender)
        {
            foreach (Control innerControl in parentControl.Controls)
            {
                if (innerControl != lastSender)
                {
                    if (!(innerControl is Label) && !(innerControl is LabelControl))
                    {
                        innerControl.Enabled = enabled;
                    }

                    if (innerControl is TextEdit)
                    {
                        TextEdit textEdit = (TextEdit)innerControl;
                        textEdit.Text = "";
                    }
                    if (innerControl is TextBox)
                    {
                        TextBox textBox = (TextBox)innerControl;
                        if (textBox.Tag?.ToString() != "exc")
                            textBox.Text = "";
                        else textBox.Enabled = true;
                    }
                    if (innerControl is CheckEdit)
                    {
                        CheckEdit checkEdit = (CheckEdit)innerControl;
                        checkEdit.Checked = false;
                    }
                    if (innerControl is CheckBox)
                    {
                        CheckBox checkEdit = (CheckBox)innerControl;
                        checkEdit.Checked = false;
                    }
                    if (innerControl is LookUpEdit)
                    {
                        LookUpEdit L = (LookUpEdit)innerControl;
                        L.EditValue = "";
                    }
                }
            }
        }

        public class LookupData
        {
            public string Value { get; set; }
            public string Display { get; set; }

            public LookupData(string value, string display)
            {
                Value = value;
                Display = display;
            }
        }


        public static void ResetInput(Control parentControl)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                if (childControl is TextBox)
                {
                    ((TextBox)childControl).Text = string.Empty;
                }
                else if (childControl is CheckBox)
                {
                    ((CheckBox)childControl).Checked = false;
                }
                else if (childControl is RadioButton)
                {
                    ((RadioButton)childControl).Checked = false;
                }
                else if (childControl is RadioGroup)
                {
                    RadioGroup radioGroup = (RadioGroup)childControl;
                    radioGroup.EditValue = null;
                }
                else if (childControl is MemoEdit)
                {
                    ((MemoEdit)childControl).Text = string.Empty;
                }
                else if (childControl is CheckedListBox)
                {
                    CheckedListBox checkedListBox = (CheckedListBox)childControl;
                    for (int i = 0; i < checkedListBox.Items.Count; i++)
                    {
                        checkedListBox.SetItemChecked(i, false);
                    }
                }
                else if (childControl is LookUpEdit)
                {
                    ((LookUpEdit)childControl).EditValue = "";
                }
                else if (childControl is TextEdit)
                {
                    ((TextEdit)childControl).Text = string.Empty;
                }
                else if (childControl is DateEdit)
                {
                    ((DateEdit)childControl).DateTime = DateTime.Now;
                }
                else if (childControl is GridControl)
                {
                    GridControl gridControl = (GridControl)childControl;
                    gridControl.DataSource = null;
                }
                else if (childControl is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)childControl;
                    comboBox.SelectedIndex = -1;
                }
                else if (childControl is ChartControl)
                {
                    ChartControl chartControl = (ChartControl)childControl;
                    foreach (Series series in chartControl.Series)
                    {
                        series.Points.Clear();
                    }
                }

                ResetInput(childControl);
            }
        }

    }

}
