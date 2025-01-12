using System;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace Clinic.Class
{
    public class OracleChangeNotification
    {
        private OracleConnection _connection;
        ConnectDb ConnOra = new ConnectDb();
        public OracleChangeNotification(string connectionString)
        {
            // Inisialisasi koneksi
            _connection = new OracleConnection(@"Data Source=(DESCRIPTION = 
                        (ADDRESS = 
                            (PROTOCOL = TCP)
                            (HOST = localhost )(PORT = 1521))
                        (CONNECT_DATA = (SERVER = DEDICATED)
                            (SERVICE_NAME = XE))
                        );User Id=KLINIK;Password=KLINIK;");
        }

        public void StartListening()
        {
            // Membuka koneksi
            _connection.Open();

            // Inisialisasi OracleDependency
            OracleDependency dependency = new OracleDependency();
            dependency.OnChange += OnDataChanged;

            // Buat OracleCommand yang memantau tabel
            OracleCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT CALL_ID, QUE, TYPE_INS FROM CS_CALL_LOG";
            command.AddRowid = true;

            // Hubungkan OracleDependency dengan command
            dependency.AddCommandDependency(command);

            // Jalankan command (ini diperlukan untuk memulai notifikasi)
            OracleDataReader reader = command.ExecuteReader();
            reader.Close(); // Kita hanya perlu menjalankan query tanpa mengambil data

            Console.WriteLine("Listening for data changes...");
        }

        // Method yang dipanggil ketika ada perubahan pada tabel
        private void OnDataChanged(object sender, OracleNotificationEventArgs args)
        {
            //Console.WriteLine("Notification received: Data has been changed!");
            MessageBox.Show("Pasien Baru sudah mendaftar.","Info",MessageBoxButtons.OK);
            //foreach (OracleNotificationRowChange rowChange in args.Details.Rows)
            //{
            //    Console.WriteLine($"Row with RowId {rowChange.Rowid} has been {rowChange.Operation}");
            //}
        }

        public void StopListening()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}
