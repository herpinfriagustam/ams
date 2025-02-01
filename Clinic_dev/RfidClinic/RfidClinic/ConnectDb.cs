using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RfidClinic
{
    class ConnectDb
    {
        private OleDbConnection _ConnectOra; 
         
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
    }
}
 