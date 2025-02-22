using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Clinic.Class;
using System.Threading;

namespace Clinic
{
    public partial class ClinicMngt : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        ConnectDb ConnOra = new ConnectDb();


        public string userEmpid = "", userName = "", userStatus = ""; //, my_IP=""
        OracleChangeNotification Notif = null  ;
        ReservationScan reservationScan = null;
        ReservationMngt reservationMngt = null;
        ReservationRegister ReservationRegister = null;
        ReservationRegisterRI ReservationRegisterRI = null;
        ReservationQueue reservationQueue = null;
        FirstInspection firstInspection = null;
        Inspection inspection = null;
        InspectionGigi inspectionGigi = null;
        InspectionPregnant inspectionPregnant = null;
        PrescriptionList prescriptionList = null;
        ObservationList observationList = null;
        MedicineUpload medicineUpload = null;
        MedicineStokUpload medicineStokUpload = null;
        DiagnosaGrpUpload diagnosaGrpUpload = null;
        DiagnosaItmUpload diagnosaItmUpload = null;
        McuUpload mcuUpload = null;
        McuList mcuList = null;
        ReservationMngt2 reservationMngt2 = null;
        AnamnesaMngt anamnesaMngt = null;
        InspectionUSG InspectionUSG = null;
        MedicalRecordMngt medicalRecordMngt = null;
        DiagnosaMngt diagnosaMngt = null;
        MedicineMngt medicineMngt = null;
        FirstInspectionPreg firstInspectionPreg = null;
        ReservationMngt3 reservationMngt3 = null;
        ReservationMngt5 reservationMngt5 = null;        
        MasterDiagnosa masterDiagnosa = null;
        MasterMedicine masterMedicine = null;
        MasterAsuransi MasterAsuransi = null;
        PatientReport patientReport = null;
        MedicalReport medicalReport = null;
        TransMedList transMedList = null;
        LetterReport letterReport = null;
        VisitReport visitReport = null;
        MedicineReport medicineReport = null;
        DashboardMonthly dashboardMonthly = null;
        PregnantReport pregnantReport = null;
        AudiometriUpload audiometriUpload = null;
        AudiometriList audiometriList = null;
        OtherFiles otherFiles = null;
        AudiometriReport audiometriReport = null;
        DiagnosaInactive diagnosaInactive = null;
        PatientInfoMngt patientInfoMngt = null;
        PatientInfo patientInfo = null;
        GuarantorMngt guarantorMngt = null;
        BillList billList = null;
        ReservationMngt4 reservationMngt4 = null;
        Inpatient inpatient = null;
        ActionResultMngt actionResultMngt = null;
        ReservationQueue2 reservationQueue2 = null;
        MasterPoli masterPoli = null;
        MasterFormula masterFormula = null;
        MasterRoom masterRoom = null;
        MasterSchedule masterSchedule = null;
        MasterTreatment masterTreatment = null;
        InpatientMngt inpatientMngt = null;
        PatientReport2 patientReport2 = null;
        TreatmentMngt treatmentMngt = null;
        OutPatientReport outPatientReport = null;
        InPatientReport inPatientReport = null;
        MasterUser masterUser = null;
        MasterCode masterCode = null;
        MasterDokter masterDokter = null;
        TreatNonMedis treatNonMedis = null;
        MedicineSeller MedicineSeller = null;
        ReportForm ReportForm = null;
        FrmRawatInap FrmRawatInap = null;
        RawatInapBidan RawatInapBidan = null;
        FrmTindakan FrmTindakan = null;
        DashboardAntrian DashboardAntrian = null;
        AntrianPoli AntrianPoli = null;
        MasterChgPass MasterChgPass = null;
        //Bpjs.BpjswsAntreanBpjs frmBpjswsAntreanBpjs = null;
        Bpjsws.BpjswsTool frmBpjswsTool = null;
        Lap_Kunjungan Lap_Kunjungan = null;
        Lap_KunjunganRI Lap_KunjunganRI = null;
        Lap_KasHarian Lap_KasHarian = null;
        Lap_PenggunaApp Lap_PenggunaApp = null;

        string version =  "Version " + Application.ProductVersion;

        private bool isLoggedOut = false;

        string mAppVersion = "";
        string mAppVersionServer = "";
        string sql_ = "";
        Thread mThread;

        DataRow mAppVersionInfoServer;

        const string APP_NAME = "Clinic.exe";
        const string APP_LAUNCHER = "Launcher.exe";

        //ConnectDb ConnOra = new ConnectDb();
        public ClinicMngt()
        {
            InitializeComponent();
        }

        private void ClinicMngt_Load(object sender, EventArgs e)
        {
            ConnOra.my_IP = GetLocalIPAddress();
            barStaticItem1.Caption = "My IP: " + ConnOra.my_IP;
            barStaticItem2.Caption = userEmpid;
            barStaticItem3.Caption = userName;
            barStaticItem4.Caption = userStatus;
            barStaticItem5.Caption = version;

            navBarItem1.Visible = false;
            navBarItem2.Visible = false;
            //navBarItem3.Visible = false;
            navBarItem10.Visible = false;

            MenuPrivilege();
            //initDashboard();

            //string connectionString = "Provider=MSDAORA.1;Password=KLINIK;Persist Security Info=True;User ID=KLINIK;Data Source = localhost:1521/XE";
            ////string _ConnectStringOra = "Provider=MSDAORA.1;Password=KLINIK;Persist Security Info=True;User ID=KLINIK;Data Source = 192.168.1.99:1521/XE";
             
            //OracleChangeNotification notificationListener = new OracleChangeNotification(connectionString);

            //notificationListener.StartListening();

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            //notificationListener.StopListening();

        }
        private void navBarItem72_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //Application.Restart();
                // Set status logout

                sql_ = "";
                sql_ = "UPDATE KLINIK.CS_HISTORY_LOGIN SET E_DATE = SYSDATE  WHERE USER_ID = '" + DB.vUserId + "' AND E_DATE IS NULL and trunc(S_DATE) = trunc(sysdate) ";

                ConnOra.ExeNonQuery(sql_); 

                isLoggedOut = true;

                // Clear session data (misalnya credentials atau settings)
                Properties.Settings.Default.UserLoggedIn = false;
                Properties.Settings.Default.Save();

                // Menutup semua child forms jika ada
                foreach (Form childForm in this.MdiChildren)
                {
                    childForm.Close();
                }

                mThread = new Thread(UpdateApp);
                mThread.Start();

                //// Tampilkan form login dan sembunyikan form utama (MDI parent form)
                //fclinic loginForm = new fclinic();
                //loginForm.MdiParent = this;  // Set parent form ke MDI
                //loginForm.Show();
                ////Sembunyikan MDI form utama
                //this.Hide();
                ////Application.Restart();
                Application.Exit();
            } 
        }
        private void UpdateApp()
        {
            // 1. Checking for update
            //bool updateAvailable = isUpdateAvailable();
            //if (updateAvailable == false)
            //{
            Process.Start(Application.StartupPath + "//" + APP_LAUNCHER);
            //}

            //// 2. If Update available Download it
            //bool downloaded = DownloadUpdate();

            //// 3. If download success close and launch the app
            //if (downloaded)
            //{
            //    Process.Start(Application.StartupPath + "//" + APP_LAUNCHER);
            //}
            //else
            //{
            //    //Process.Start(Application.StartupPath + "//klinik//" + APP_NAME);
            //}

            Process.GetCurrentProcess().Kill();
        }
        private void MenuPrivilege()
        {
            if (userStatus == "DOH")
            {
                navBarGroup9.Visible = true; // Pendaftaran 
                navBarItem62.Visible = true; // Pendaftaran Pasien Baru
                navBarItem41.Visible = true; // Daftar Penjamin Pasien
                navBarItem46.Visible = true; // Dashboard Antrian

                navBarGroup1.Visible = true; // Group Reservation
                navBarItem58.Visible = true; // Pelayanan Non Medis
                navBarItem65.Visible = true; // Register Rawat Jalan
                navBarItem66.Visible = true; // Register Rawat Inap
                navBarItem59.Visible = true; // Reservasi Rawat Jalan
                navBarItem43.Visible = true; // Reservasi Rawat Inap 

                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem4.Visible = true;  // Pemeriksaan Dokter  
                navBarItem73.Visible = true; // Pemeriksaan Dokter Gigi
                navBarItem68.Visible = true; // Pemeriksaan USG 
                navBarItem15.Visible = true; // Pemeriksaan Bidan
                navBarItem64.Visible = true; // Rawat Inap Bidan  
                navBarItem61.Visible = true; // Rawat Inap 
                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem45.Visible = true; // Pemeriksaan Lab

                GFarmasi.Visible = true;     // Group Farmasi 
                navBarItem5.Visible = true;  // Daftar Confirm Obat
                navBarItem70.Visible = true; // Pengeluaran Obat
                navBarItem23.Visible = true; // List Transaksi Obat  

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem7.Visible = true;  // Laporan Pasien
                navBarItem8.Visible = true;  // Laporan Medical
                navBarItem24.Visible = true; // Laporan Obat
                navBarItem9.Visible = true;  // Laporan Surat
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
                navBarItem76.Visible = true; // Laporan Kas Harian
                navBarItem77.Visible = true; // Laporan Pengguna System

                navBarGroup5.Visible = true; // Group Master Data
                navBarItem12.Visible = true; // Master Data Diagnosa
                navBarItem13.Visible = true; // Master Data Obat
                navBarItem39.Visible = true; // Daftar Data Diagnosa Tidak Aktif
                navBarItem47.Visible = true; // Master Data Poli
                navBarItem48.Visible = true; // Master Data Formula
                navBarItem49.Visible = true; // Master Data Ruangan
                navBarItem50.Visible = true; // Master Data Layanan
                navBarItem56.Visible = true; // Master Data User
                navBarItem57.Visible = true; // Master Data Code 
                navBarItem69.Visible = true; // Master Data Asuransi
                navBarItem74.Visible = true; // Master Data Dokter
                navBarItem80.Visible = true; // Master Data Schedule

                navBarGroup6.Visible = true; // Group Upload Data
                navBarItem29.Visible = true; // Upload Diagnosa Group 
                navBarItem32.Visible = true; // Upload Diagnosa Detail Item
                navBarItem34.Visible = true; // Upload Data Obat
                navBarItem35.Visible = true; // Upload Data Stock Obat 

                navBarGroup7.Visible = true; // Group Management Data
                navBarItem6.Visible = true; //  Reservasi Manajemen
                navBarItem18.Visible = true; // Pasien Manajemen
                navBarItem19.Visible = true; // Anamnesa Manajemen
                navBarItem20.Visible = true; // Diagnosa Management
                navBarItem21.Visible = true; // Resep Manajemen
                navBarItem51.Visible = true; // Rawat Inap Manajemen
                navBarItem52.Visible = true; // Layanan Manajemen  

                navBarGroup8.Visible = true; // Group Payment
                navBarItem42.Visible = true; // Daftar Tagihan 
            }
            else if (userStatus == "PIC")
            {
                navBarGroup9.Visible = true; // Pendaftaran 
                navBarItem62.Visible = true; // Pendaftaran Pasien Baru
                navBarItem41.Visible = true; // Daftar Penjamin Pasien
                navBarItem46.Visible = false; // Dashboard Antrian

                navBarGroup1.Visible = true; // Group Reservation
                navBarItem65.Visible = true; // Pendaftaran Rawat Jalan
                navBarItem66.Visible = true; // Pendaftaran Rawat Inap
                navBarItem58.Visible = true; // Pelayanan None Medis

                navBarGroup2.Visible = false; // Group Pemeriksaan
                GFarmasi.Visible = true;      // Group Pharmacy
                navBarItem23.Visible = true; // Daftar Transaksi Obat

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem7.Visible = true;  // Laporan Pasien
                navBarItem8.Visible = true;  // Laporan Medical
                navBarItem24.Visible = true; // Laporan Obat
                navBarItem9.Visible = true;  // Laporan Surat
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
                navBarItem76.Visible = true; // Laporan Kas Harian
                navBarItem77.Visible = true; // Laporan Pengguna System
                navBarGroup8.Visible = false; // Group Payment
                 
                navBarGroup5.Visible = true; // Group Master Data
                navBarItem12.Visible = true; // Master Data Diagnosa
                navBarItem13.Visible = true; // Master Data Obat
                navBarItem39.Visible = true; // Daftar Data Diagnosa Tidak Aktif
                navBarItem47.Visible = true; // Master Data Poli
                navBarItem48.Visible = true; // Master Data Formula
                navBarItem49.Visible = true; // Master Data Ruangan
                navBarItem50.Visible = true; // Master Data Layanan
                navBarItem56.Visible = true; // Master Data User
                navBarItem57.Visible = true; // Master Data Code 
                navBarItem69.Visible = true; // Master Data Asuransi
                navBarItem74.Visible = true; // Master Data Dokter
                navBarItem80.Visible = true; // Master Data Schedule

                navBarGroup6.Visible = true; // Group Upload Data
                navBarItem29.Visible = true; // Upload Diagnosa Group 
                navBarItem32.Visible = true; // Upload Diagnosa Detail Item
                navBarItem34.Visible = true; // Upload Data Obat
                navBarItem35.Visible = true; // Upload Data Stock Obat 

                navBarGroup7.Visible = true; // Group Management Data
                navBarItem6.Visible = true; //  Reservasi Manajemen
                navBarItem18.Visible = true; // Pasien Manajemen
                navBarItem19.Visible = true; // Anamnesa Manajemen
                navBarItem20.Visible = true; // Diagnosa Management
                navBarItem21.Visible = true; // Resep Manajemen
                navBarItem51.Visible = true; // Rawat Inap Manajemen
                navBarItem52.Visible = true; // Layanan Manajemen   

                navBarGroup8.Visible = true; // Group Payment
                navBarItem42.Visible = true; // Daftar Tagihan 
            }
            else if (userStatus == "DOC")
            {
                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem4.Visible = true;  // Pemeriksaan Dokter 

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
            }
            else if (userStatus == "DOU")
            {
                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem4.Visible = true;  // Pemeriksaan Dokter 
                navBarItem68.Visible = true; // Pemeriksaan USG 

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis 
            }
            else if (userStatus == "DGI")
            {
                navBarGroup2.Visible = true; // Group Pemeriksaan 
                navBarItem73.Visible = true;  // Pemeriksaan Dokter Gigi
                navBarGroup4.Visible = true; // Group Laporan
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
            }
            else if (userStatus == "LAB")
            {
                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem45.Visible = true; // Pemeriksaan Lab
            }
            else if (userStatus == "MID")
            {
                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem15.Visible = true; // Pemeriksaan Bidan
                navBarItem64.Visible = true; // Rawat Inap Bidan
                navBarItem68.Visible = true; // Pemeriksaan USG 

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
            } 
            else if (userStatus == "NUR")
            {
                navBarGroup9.Visible = true; // Pendaftaran 
                navBarItem46.Visible = true; // Dashboard Antrian

                navBarGroup1.Visible = true; // Group Reservation
                navBarItem59.Visible = true; // Reservasi Rawat Jalan
                navBarItem43.Visible = true; // Reservasi Rawat Inap
                navBarItem58.Visible = true; // Pelayanan Non Medis

                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem4.Visible  = true; // Pemeriksaan Dokter
                navBarItem61.Visible = true; // Rawat Inap 

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem7.Visible = true;  // Laporan Pasien
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
            }
            else if (userStatus == "MED")
            {
                navBarGroup1.Visible = true; // Group Reservation
                navBarItem58.Visible = true; // Pelayanan Non Medis

                GFarmasi.Visible = true;     // Group Farmasi 
                navBarItem5.Visible = true;  // Daftar Confirm Obat
                navBarItem70.Visible = true; // Pengeluaran Obat
                navBarItem23.Visible = true; // List Transaksi Obat  

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem24.Visible = true; // Laporan Obat

                navBarGroup5.Visible = true; // Group Master Data
                navBarItem13.Visible = true; // Master Obat 
            }
            else if (userStatus == "KSR")
            {
                navBarGroup9.Visible = true; // Pendaftaran 
                navBarItem62.Visible = true; // Pendaftaran Pasien Baru
                navBarItem41.Visible = true; // Daftar Penjamin Pasien
                navBarItem46.Visible = true; // Dashboard Antrian

                navBarGroup1.Visible = true; // Group Reservation
                navBarItem58.Visible = true; // Pelayanan Non Medis
                navBarItem65.Visible = true; // Register Rawat Jalan
                navBarItem66.Visible = true; // Register Rawat Inap  

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem7.Visible = true;  // Laporan Pasien
                navBarItem8.Visible = true;  // Laporan Medical 
                navBarItem9.Visible = true;  // Laporan Surat
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
                navBarItem76.Visible = true;// Laporan Kas Harian

                navBarGroup8.Visible = true; // Group Payment
                navBarItem42.Visible = true; // Daftar Tagihan 
            }
            else if(userStatus == "SYS")
            {
                navBarGroup9.Visible = true; // Pendaftaran 
                navBarItem62.Visible = true; // Pendaftaran Pasien Baru
                navBarItem41.Visible = true; // Daftar Penjamin Pasien
                navBarItem46.Visible = true; // Dashboard Antrian

                navBarGroup1.Visible = true; // Group Reservation
                navBarItem58.Visible = true; // Pelayanan Non Medis
                navBarItem65.Visible = true; // Register Rawat Jalan
                navBarItem66.Visible = true; // Register Rawat Inap
                navBarItem59.Visible = true; // Reservasi Rawat Jalan
                navBarItem43.Visible = true; // Reservasi Rawat Inap 

                navBarGroup2.Visible = true; // Group Pemeriksaan
                navBarItem4.Visible = true;  // Pemeriksaan Dokter 
                navBarItem73.Visible = true;  // Pemeriksaan Dokter Gigi
                navBarItem68.Visible = true; // Pemeriksaan USG 
                navBarItem15.Visible = true; // Pemeriksaan Bidan
                navBarItem64.Visible = true; // Rawat Inap Bidan  
                navBarItem61.Visible = true; // Rawat Inap 
                navBarItem45.Visible = true; // Pemeriksaan Lab

                GFarmasi.Visible = true;     // Group Farmasi 
                navBarItem5.Visible = true; // Daftar Confirm Obat
                navBarItem70.Visible = true; // Pengeluaran Obat
                navBarItem23.Visible = true; // List Transaksi Obat  

                navBarGroup4.Visible = true; // Group Laporan
                navBarItem7.Visible = true;  // Laporan Pasien
                navBarItem8.Visible = true;  // Laporan Medical
                navBarItem24.Visible = true; // Laporan Obat
                navBarItem9.Visible = true;  // Laporan Surat
                navBarItem14.Visible = true; // Laporan Kunjungan
                navBarItem53.Visible = true; // Laporan Pasien Ranap
                navBarItem54.Visible = true; // Laporan Rawat Jalan
                navBarItem55.Visible = true; // Laporan Rawat Inap
                navBarItem60.Visible = true; // Laporan Rekam Medis
                navBarItem76.Visible = true; // Laporan Kas Harian
                navBarItem77.Visible = true; // Laporan Pengguna System

                navBarGroup5.Visible = true; // Group Master Data
                navBarItem12.Visible = true; // Master Data Diagnosa
                navBarItem13.Visible = true; // Master Data Obat
                navBarItem39.Visible = true; // Daftar Data Diagnosa Tidak Aktif
                navBarItem47.Visible = true; // Master Data Poli
                navBarItem48.Visible = true; // Master Data Formula
                navBarItem49.Visible = true; // Master Data Ruangan
                navBarItem50.Visible = true; // Master Data Layanan
                navBarItem56.Visible = true; // Master Data User
                navBarItem57.Visible = true; // Master Data Code 
                navBarItem69.Visible = true; // Master Data Asuransi
                navBarItem74.Visible = true; // Master Data Dokter
                navBarItem80.Visible = true; // Master Data Schedule

                navBarGroup6.Visible = true; // Group Upload Data
                navBarItem29.Visible = true; // Upload Diagnosa Group 
                navBarItem32.Visible = true; // Upload Diagnosa Detail Item
                navBarItem34.Visible = true; // Upload Data Obat
                navBarItem35.Visible = true; // Upload Data Stock Obat 

                navBarGroup7.Visible = true; // Group Management Data
                navBarItem6.Visible = true; //  Reservasi Manajemen
                navBarItem18.Visible = true; // Pasien Manajemen
                navBarItem19.Visible = true; // Anamnesa Manajemen
                navBarItem20.Visible = true; // Diagnosa Management
                navBarItem21.Visible = true; // Resep Manajemen
                navBarItem51.Visible = true; // Rawat Inap Manajemen
                navBarItem52.Visible = true; // Layanan Manajemen  

                navBarGroup8.Visible = true; // Group Payment
                navBarItem42.Visible = true; // Daftar Tagihan 
                navBarGroup10.Visible = true; // Tool BPJS
            }
            navBarItem71.Visible = true; // Pergantian Password
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

       

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            if (reservationScan == null || reservationScan.Text == "")
            {
                reservationScan = new ReservationScan();
                reservationScan.MdiParent = this;
                reservationScan.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(reservationScan.Text))
            {
                reservationScan.WindowState = FormWindowState.Maximized;
                reservationScan.Show();
                reservationScan.Focus();
            }
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            if (reservationMngt == null || reservationMngt.Text == "")
            {
                reservationMngt = new ReservationMngt();
                reservationMngt.MdiParent = this;
                reservationMngt.v_empid = userEmpid;
                reservationMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(reservationMngt.Text))
            {
                reservationMngt.WindowState = FormWindowState.Maximized;
                reservationMngt.Show();
                reservationMngt.Focus();
            }
        }

        private void navBarItem16_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationQueue == null || reservationQueue.Text == "")
            {
                reservationQueue = new ReservationQueue();                
                //reservationQueue.MdiParent = this;
                reservationQueue.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(reservationMngt.Text))
            {
                reservationMngt.WindowState = FormWindowState.Maximized;
                reservationMngt.Show();
                reservationMngt.Focus();
            }
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (prescriptionList == null || prescriptionList.Text == "")
            {
                prescriptionList = new PrescriptionList();
                prescriptionList.MdiParent = this;
                //prescriptionList.v_empid = userEmpid;
                prescriptionList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(prescriptionList.Text))
            {
                prescriptionList.WindowState = FormWindowState.Maximized;
                prescriptionList.Show();
                prescriptionList.Focus();
            }
        }

        private void navBarItem17_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (observationList == null || observationList.Text == "")
            {
                observationList = new ObservationList();
                observationList.MdiParent = this;
                observationList.v_empid = userEmpid;
                observationList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(observationList.Text))
            {
                observationList.WindowState = FormWindowState.Maximized;
                observationList.Show();
                observationList.Focus();
            }
        }

        private void navBarItem22_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (mcuList == null || mcuList.Text == "")
            {
                mcuList = new McuList();
                mcuList.MdiParent = this;
                mcuList.v_empid = userEmpid;
                mcuList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(mcuList.Text))
            {
                mcuList.WindowState = FormWindowState.Maximized;
                mcuList.Show();
                mcuList.Focus();
            }
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationMngt3 == null || reservationMngt3.Text == "")
            {
                reservationMngt3 = new ReservationMngt3();
                reservationMngt3.MdiParent = this;
                reservationMngt3.v_empid = userEmpid;
                reservationMngt3.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(reservationMngt3.Text))
            {
                reservationMngt3.WindowState = FormWindowState.Maximized;
                reservationMngt3.Show();
                reservationMngt3.Focus();
            }
        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterDiagnosa == null || masterDiagnosa.Text == "")
            {
                masterDiagnosa = new MasterDiagnosa();
                masterDiagnosa.MdiParent = this;
                //masterDiagnosa.v_empid = userEmpid;
                masterDiagnosa.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(masterDiagnosa.Text))
            {
                masterDiagnosa.WindowState = FormWindowState.Maximized;
                masterDiagnosa.Show();
                masterDiagnosa.Focus();
            }
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterMedicine == null || masterMedicine.Text == "")
            {
                masterMedicine = new MasterMedicine();
                masterMedicine.MdiParent = this;
                //masterMedicine.v_empid = userEmpid;
                masterMedicine.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(masterMedicine.Text))
            {
                masterMedicine.WindowState = FormWindowState.Maximized;
                masterMedicine.Show();
                masterMedicine.Focus();
            }
        }

        private void navBarItem28_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (mcuUpload == null || mcuUpload.Text == "")
            {
                mcuUpload = new McuUpload();
                mcuUpload.MdiParent = this;
                mcuUpload.v_empid = userEmpid;
                mcuUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(mcuUpload.Text))
            {
                mcuUpload.WindowState = FormWindowState.Maximized;
                mcuUpload.Show();
                mcuUpload.Focus();
            }
        }

        private void navBarItem29_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (diagnosaGrpUpload == null || diagnosaGrpUpload.Text == "")
            {
                diagnosaGrpUpload = new DiagnosaGrpUpload();
                diagnosaGrpUpload.MdiParent = this;
                diagnosaGrpUpload.v_empid = userEmpid;
                diagnosaGrpUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(diagnosaGrpUpload.Text))
            {
                diagnosaGrpUpload.WindowState = FormWindowState.Maximized;
                diagnosaGrpUpload.Show();
                diagnosaGrpUpload.Focus();
            }
        }

        private void navBarItem32_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (diagnosaItmUpload == null || diagnosaItmUpload.Text == "")
            {
                diagnosaItmUpload = new DiagnosaItmUpload();
                diagnosaItmUpload.MdiParent = this;
                diagnosaItmUpload.v_empid = userEmpid;
                diagnosaItmUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(diagnosaItmUpload.Text))
            {
                diagnosaItmUpload.WindowState = FormWindowState.Maximized;
                diagnosaItmUpload.Show();
                diagnosaItmUpload.Focus();
            }
        }

        private void navBarItem34_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicineUpload == null || medicineUpload.Text == "")
            {
                medicineUpload = new MedicineUpload();
                medicineUpload.MdiParent = this;
                medicineUpload.v_empid = userEmpid;
                medicineUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicineUpload.Text))
            {
                medicineUpload.WindowState = FormWindowState.Maximized;
                medicineUpload.Show();
                medicineUpload.Focus();
            }
        }

        private void navBarItem35_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicineStokUpload == null || medicineStokUpload.Text == "")
            {
                medicineStokUpload = new MedicineStokUpload();
                medicineStokUpload.MdiParent = this;
                medicineStokUpload.v_empid = userEmpid;
                medicineStokUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicineStokUpload.Text))
            {
                medicineStokUpload.WindowState = FormWindowState.Maximized;
                medicineStokUpload.Show();
                medicineStokUpload.Focus();
            }
        }

        private void navBarItem6_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationMngt2 == null || reservationMngt2.Text == "")
            {
                reservationMngt2 = new ReservationMngt2();
                reservationMngt2.MdiParent = this;
                reservationMngt2.v_empid = userEmpid;
                reservationMngt2.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(reservationMngt2.Text))
            {
                reservationMngt2.WindowState = FormWindowState.Maximized;
                reservationMngt2.Show();
                reservationMngt2.Focus();
            }
        }

        private void navBarItem18_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicalRecordMngt == null || medicalRecordMngt.Text == "")
            {
                medicalRecordMngt = new MedicalRecordMngt();
                medicalRecordMngt.MdiParent = this;
                medicalRecordMngt.v_empid = userEmpid;
                medicalRecordMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicalRecordMngt.Text))
            {
                medicalRecordMngt.WindowState = FormWindowState.Maximized;
                medicalRecordMngt.Show();
                medicalRecordMngt.Focus();
            }
        }

        private void navBarItem19_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (anamnesaMngt == null || anamnesaMngt.Text == "")
            {
                anamnesaMngt = new AnamnesaMngt();
                anamnesaMngt.MdiParent = this;
                anamnesaMngt.v_empid = userEmpid;
                anamnesaMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(anamnesaMngt.Text))
            {
                anamnesaMngt.WindowState = FormWindowState.Maximized;
                anamnesaMngt.Show();
                anamnesaMngt.Focus();
            }
        }

        private void navBarItem20_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (diagnosaMngt == null || diagnosaMngt.Text == "")
            {
                diagnosaMngt = new DiagnosaMngt();
                diagnosaMngt.MdiParent = this;
                //diagnosaMngt.v_empid = userEmpid;
                diagnosaMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(diagnosaMngt.Text))
            {
                diagnosaMngt.WindowState = FormWindowState.Maximized;
                diagnosaMngt.Show();
                diagnosaMngt.Focus();
            }
        }

        private void navBarItem21_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicineMngt == null || medicineMngt.Text == "")
            {
                medicineMngt = new MedicineMngt();
                medicineMngt.MdiParent = this;
                medicineMngt.v_empid = userEmpid;
                medicineMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicineMngt.Text))
            {
                medicineMngt.WindowState = FormWindowState.Maximized;
                medicineMngt.Show();
                medicineMngt.Focus();
            }
        }

        private void navBarItem3_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationMngt3 == null || reservationMngt3.Text == "")
            {
                reservationMngt3 = new ReservationMngt3();
                reservationMngt3.MdiParent = this;
                reservationMngt3.v_empid = userEmpid;
                reservationMngt3.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(reservationMngt3.Text))
            {
                reservationMngt3.WindowState = FormWindowState.Maximized;
                reservationMngt3.Show();
                reservationMngt3.Focus();
            }
            //if (firstInspection == null || firstInspection.Text == "")
            //{
            //    firstInspection = new FirstInspection();
            //    firstInspection.MdiParent = this;
            //    firstInspection.v_empid = userEmpid;
            //    firstInspection.Show();
            //    this.panel1.Hide();
            //    this.pictureBox1.Hide();


            //}
            //else if (CheckOpened(firstInspection.Text))
            //{
            //    firstInspection.WindowState = FormWindowState.Maximized;
            //    firstInspection.Show();
            //    firstInspection.Focus();
            //}
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (patientReport == null || patientReport.Text == "")
            {
                patientReport = new PatientReport();
                patientReport.MdiParent = this;
                patientReport.v_empid = userEmpid;
                patientReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(patientReport.Text))
            {
                patientReport.WindowState = FormWindowState.Maximized;
                patientReport.Show();
                patientReport.Focus();
            }
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicalReport == null || medicalReport.Text == "")
            {
                medicalReport = new MedicalReport();
                medicalReport.MdiParent = this;
                medicalReport.v_empid = userEmpid;
                medicalReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicalReport.Text))
            {
                medicalReport.WindowState = FormWindowState.Maximized;
                medicalReport.Show();
                medicalReport.Focus();
            }
        }

        private void navBarItem23_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (transMedList == null || transMedList.Text == "")
            {
                transMedList = new TransMedList();
                transMedList.MdiParent = this;
                //transMedList.v_empid = userEmpid;
                transMedList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(transMedList.Text))
            {
                transMedList.WindowState = FormWindowState.Maximized;
                transMedList.Show();
                transMedList.Focus();
            }
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (letterReport == null || letterReport.Text == "")
            {
                letterReport = new LetterReport();
                letterReport.MdiParent = this;
                letterReport.v_empid = userEmpid;
                letterReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(letterReport.Text))
            {
                letterReport.WindowState = FormWindowState.Maximized;
                letterReport.Show();
                letterReport.Focus();
            }
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (Lap_Kunjungan == null || Lap_Kunjungan.Text == "")
            {
                Lap_Kunjungan = new Lap_Kunjungan();
                Lap_Kunjungan.MdiParent = this;
                Lap_Kunjungan.v_empid = userEmpid;
                Lap_Kunjungan.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(Lap_Kunjungan.Text))
            {
                Lap_Kunjungan.WindowState = FormWindowState.Maximized;
                Lap_Kunjungan.Show();
                Lap_Kunjungan.Focus();
            }

            //if (visitReport == null || visitReport.Text == "")
            //{
            //    visitReport = new VisitReport();
            //    visitReport.MdiParent = this;
            //    visitReport.v_empid = userEmpid;
            //    visitReport.Show();
            //    this.panel1.Hide();
            //    this.pictureBox1.Hide();
                
                
            //}
            //else if (CheckOpened(visitReport.Text))
            //{
            //    visitReport.WindowState = FormWindowState.Maximized;
            //    visitReport.Show();
            //    visitReport.Focus();
            //}
        }

        private void navBarItem24_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (medicineReport == null || medicineReport.Text == "")
            {
                medicineReport = new MedicineReport();
                medicineReport.MdiParent = this;
                medicineReport.v_empid = userEmpid;
                medicineReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(medicineReport.Text))
            {
                medicineReport.WindowState = FormWindowState.Maximized;
                medicineReport.Show();
                medicineReport.Focus();
            }
        }

        private void navBarItem25_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (dashboardMonthly == null || dashboardMonthly.Text == "")
            {
                dashboardMonthly = new DashboardMonthly();
                dashboardMonthly.MdiParent = this;
                dashboardMonthly.v_empid = userEmpid;
                dashboardMonthly.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(dashboardMonthly.Text))
            {
                dashboardMonthly.WindowState = FormWindowState.Maximized;
                dashboardMonthly.Show();
                dashboardMonthly.Focus();
            }
        }

        private void navBarItem26_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (pregnantReport == null || pregnantReport.Text == "")
            {
                pregnantReport = new PregnantReport();
                pregnantReport.MdiParent = this;
                pregnantReport.v_empid = userEmpid;
                pregnantReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(pregnantReport.Text))
            {
                pregnantReport.WindowState = FormWindowState.Maximized;
                pregnantReport.Show();
                pregnantReport.Focus();
            }
        }

        private void navBarItem27_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (audiometriUpload == null || audiometriUpload.Text == "")
            {
                audiometriUpload = new AudiometriUpload();
                audiometriUpload.MdiParent = this;
                audiometriUpload.v_empid = userEmpid;
                audiometriUpload.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(audiometriUpload.Text))
            {
                audiometriUpload.WindowState = FormWindowState.Maximized;
                audiometriUpload.Show();
                audiometriUpload.Focus();
            }
        }

        private void navBarItem36_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (audiometriList == null || audiometriList.Text == "")
            {
                audiometriList = new AudiometriList();
                audiometriList.MdiParent = this;
                audiometriList.v_empid = userEmpid;
                audiometriList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(audiometriList.Text))
            {
                audiometriList.WindowState = FormWindowState.Maximized;
                audiometriList.Show();
                audiometriList.Focus();
            }
        }

        private void navBarItem37_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (otherFiles == null || otherFiles.Text == "")
            {
                otherFiles = new OtherFiles();
                otherFiles.MdiParent = this;
                otherFiles.v_empid = userEmpid;
                otherFiles.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(otherFiles.Text))
            {
                otherFiles.WindowState = FormWindowState.Maximized;
                otherFiles.Show();
                otherFiles.Focus();
            }
        }

        private void navBarItem38_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (audiometriReport == null || audiometriReport.Text == "")
            {
                audiometriReport = new AudiometriReport();
                audiometriReport.MdiParent = this;
                audiometriReport.v_empid = userEmpid;
                audiometriReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(audiometriReport.Text))
            {
                audiometriReport.WindowState = FormWindowState.Maximized;
                audiometriReport.Show();
                audiometriReport.Focus();
            }
        }

        private void navBarItem39_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (diagnosaInactive == null || diagnosaInactive.Text == "")
            {
                diagnosaInactive = new DiagnosaInactive();
                diagnosaInactive.MdiParent = this;
                diagnosaInactive.v_empid = userEmpid;
                diagnosaInactive.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(diagnosaInactive.Text))
            {
                diagnosaInactive.WindowState = FormWindowState.Maximized;
                diagnosaInactive.Show();
                diagnosaInactive.Focus();
            }
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (firstInspectionPreg == null || firstInspectionPreg.Text == "")
            {
                firstInspectionPreg = new FirstInspectionPreg();
                firstInspectionPreg.MdiParent = this;
                firstInspectionPreg.v_empid = userEmpid;
                firstInspectionPreg.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(firstInspectionPreg.Text))
            {
                firstInspectionPreg.WindowState = FormWindowState.Maximized;
                firstInspectionPreg.Show();
                firstInspectionPreg.Focus();
            }
        } 

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (inspection == null || inspection.Text == "")
            {
                inspection = new Inspection();
                inspection.MdiParent = this;
                //inspection.v_empid = userEmpid;
                inspection.v_name = ConnOra.my_IP;
                inspection.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(inspection.Text))
            {
                inspection.WindowState = FormWindowState.Maximized;
                inspection.Show();
                inspection.Focus();
            }
        }

        private void navBarItem41_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (guarantorMngt == null || guarantorMngt.Text == "")
            {
                guarantorMngt = new GuarantorMngt();
                guarantorMngt.MdiParent = this;
                //guarantorMngt.v_empid = userEmpid;
                guarantorMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(guarantorMngt.Text))
            {
                guarantorMngt.WindowState = FormWindowState.Maximized;
                guarantorMngt.Show();
                guarantorMngt.Focus();
            }
        }


        private void navBarItem15_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (inspectionPregnant == null || inspectionPregnant.Text == "")
            {
                inspectionPregnant = new InspectionPregnant();
                inspectionPregnant.MdiParent = this;
                //inspectionPregnant.v_empid = userEmpid;
                inspectionPregnant.v_name = ConnOra.my_IP;
                inspectionPregnant.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(inspectionPregnant.Text))
            {
                inspectionPregnant.WindowState = FormWindowState.Maximized;
                inspectionPregnant.Show();
                inspectionPregnant.Focus();
            }
        }


        private void navBarItem40_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (patientInfoMngt == null || patientInfoMngt.Text == "")
            {
                patientInfoMngt = new PatientInfoMngt();
                patientInfoMngt.MdiParent = this;
                //patientInfoMngt.v_empid = userEmpid;
                patientInfoMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(patientInfoMngt.Text))
            {
                patientInfoMngt.WindowState = FormWindowState.Maximized;
                patientInfoMngt.Show();
                patientInfoMngt.Focus();
            }
        }


        private void navBarItem42_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (billList == null || billList.Text == "")
            {
                billList = new BillList();
                billList.MdiParent = this;
                //billList.v_empid = userEmpid;
                billList.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(billList.Text))
            {
                billList.WindowState = FormWindowState.Maximized;
                billList.Show();
                billList.Focus();
            }
        }
        private void navBarItem43_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationMngt4 == null || reservationMngt4.Text == "")
            {
                reservationMngt4 = new ReservationMngt4();
                reservationMngt4.MdiParent = this;
                //reservationMngt4.v_empid = userEmpid;
                reservationMngt4.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(reservationMngt4.Text))
            {
                reservationMngt4.WindowState = FormWindowState.Maximized;
                reservationMngt4.Show();
                reservationMngt4.Focus();
            }
        }

        private void navBarItem44_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (inpatient == null || inpatient.Text == "")
            {
                inpatient = new Inpatient();
                inpatient.MdiParent = this;
                //inpatient.v_empid = userEmpid;
                inpatient.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(inpatient.Text))
            {
                inpatient.WindowState = FormWindowState.Maximized;
                inpatient.Show();
                inpatient.Focus();
            }
        }
        
        private void navBarItem45_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (actionResultMngt == null || actionResultMngt.Text == "")
            {
                actionResultMngt = new ActionResultMngt();
                actionResultMngt.MdiParent = this;
                //actionResultMngt.v_empid = userEmpid;
                actionResultMngt.v_status = userStatus;
                actionResultMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(actionResultMngt.Text))
            {
                actionResultMngt.WindowState = FormWindowState.Maximized;
                actionResultMngt.Show();
                actionResultMngt.Focus();
            }
        }
        

        private void navBarItem46_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (DashboardAntrian == null || DashboardAntrian.Text == "")
            {
                 
                DashboardAntrian = new DashboardAntrian(); 
                SetFormToSecondaryScreenPosition(DashboardAntrian, 1);
                DashboardAntrian.Show();

                this.panel1.Hide();
                this.pictureBox1.Hide();

            }
            else if (CheckOpened(DashboardAntrian.Text))
            {
                DashboardAntrian.WindowState = FormWindowState.Maximized;
                DashboardAntrian.Show();
                DashboardAntrian.Focus();
            }
        }
        private bool SetFormToSecondaryScreenPosition(Form formToPosition, int whichMonitor)
        {
            // get all the screens ...
            Screen[] theScreens = Screen.AllScreens;

            // reality check
            // only one monitor, or no monitor matching the index value in 'whichMonitor ?
            if (theScreens.Length == 1 || (whichMonitor > theScreens.Length)) return false;

            // precaution to avoid possible strange side-effects
            formToPosition.StartPosition = FormStartPosition.Manual;
            formToPosition.WindowState = FormWindowState.Normal;

            formToPosition.Location = theScreens[whichMonitor].WorkingArea.Location;

            return true;
        }
        private void navBarItem47_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterPoli == null || masterPoli.Text == "")
            {
                masterPoli = new MasterPoli();
                masterPoli.MdiParent = this;
                //masterPoli.v_empid = userEmpid;
                masterPoli.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(masterPoli.Text))
            {
                masterPoli.WindowState = FormWindowState.Maximized;
                masterPoli.Show();
                masterPoli.Focus();
            }
        }
        

        private void navBarItem48_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterFormula == null || masterFormula.Text == "")
            {
                masterFormula = new MasterFormula();
                masterFormula.MdiParent = this;
                //masterFormula.v_empid = userEmpid;
                masterFormula.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(masterFormula.Text))
            {
                masterFormula.WindowState = FormWindowState.Maximized;
                masterFormula.Show();
                masterFormula.Focus();
            }
        }

        private void navBarItem49_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterRoom == null || masterRoom.Text == "")
            {
                masterRoom = new MasterRoom();
                masterRoom.MdiParent = this;
                //masterRoom.v_empid = userEmpid;
                masterRoom.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(masterRoom.Text))
            {
                masterRoom.WindowState = FormWindowState.Maximized;
                masterRoom.Show();
                masterRoom.Focus();
            }
        }

        private void navBarItem50_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterTreatment == null || masterTreatment.Text == "")
            {
                masterTreatment = new MasterTreatment();
                masterTreatment.MdiParent = this;
                //masterTreatment.v_empid = userEmpid;
                masterTreatment.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(masterTreatment.Text))
            {
                masterTreatment.WindowState = FormWindowState.Maximized;
                masterTreatment.Show();
                masterTreatment.Focus();
            }
        }

        private void navBarItem51_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (inpatientMngt == null || inpatientMngt.Text == "")
            {
                inpatientMngt = new InpatientMngt();
                inpatientMngt.MdiParent = this;
                inpatientMngt.v_empid = userEmpid;
                inpatientMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(inpatientMngt.Text))
            {
                inpatientMngt.WindowState = FormWindowState.Maximized;
                inpatientMngt.Show();
                inpatientMngt.Focus();
            }
        }

        private void navBarItem53_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (patientReport2 == null || patientReport2.Text == "")
            {
                patientReport2 = new PatientReport2();
                patientReport2.MdiParent = this;
                patientReport2.v_empid = userEmpid;
                patientReport2.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(patientReport2.Text))
            {
                patientReport2.WindowState = FormWindowState.Maximized;
                patientReport2.Show();
                patientReport2.Focus();
            }
        }

        private void navBarItem52_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (treatmentMngt == null || treatmentMngt.Text == "")
            {
                treatmentMngt = new TreatmentMngt();
                treatmentMngt.MdiParent = this;
                //treatmentMngt.v_empid = userEmpid;
                treatmentMngt.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(treatmentMngt.Text))
            {
                treatmentMngt.WindowState = FormWindowState.Maximized;
                treatmentMngt.Show();
                treatmentMngt.Focus();
            }
        }
        

        private void navBarItem54_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (outPatientReport == null || outPatientReport.Text == "")
            {
                outPatientReport = new OutPatientReport();
                outPatientReport.MdiParent = this;
                outPatientReport.v_empid = userEmpid;
                outPatientReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(outPatientReport.Text))
            {
                outPatientReport.WindowState = FormWindowState.Maximized;
                outPatientReport.Show();
                outPatientReport.Focus();
            }
        }
        

        private void navBarItem55_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //if (inPatientReport == null || inPatientReport.Text == "")
            //{
            //    inPatientReport = new InPatientReport();
            //    inPatientReport.MdiParent = this;
            //    inPatientReport.v_empid = userEmpid;
            //    inPatientReport.Show();
            //    this.panel1.Hide();
            //    this.pictureBox1.Hide();


            //}
            //else if (CheckOpened(inPatientReport.Text))
            //{
            //    inPatientReport.WindowState = FormWindowState.Maximized;
            //    inPatientReport.Show();
            //    inPatientReport.Focus();
            //}
            if (inPatientReport == null || inPatientReport.Text == "")
            {
                inPatientReport = new InPatientReport();
                inPatientReport.MdiParent = this;
                inPatientReport.v_empid = userEmpid;
                inPatientReport.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(inPatientReport.Text))
            {
                inPatientReport.WindowState = FormWindowState.Maximized;
                inPatientReport.Show();
                inPatientReport.Focus();
            }
        }

        
        private void navBarItem56_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterUser == null || masterUser.Text == "")
            {
                masterUser = new MasterUser();
                masterUser.MdiParent = this;
                //masterUser.v_empid = userEmpid;
                masterUser.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
                
                
            }
            else if (CheckOpened(masterUser.Text))
            {
                masterUser.WindowState = FormWindowState.Maximized;
                masterUser.Show();
                masterUser.Focus();
            }
        }

        private void navBarItem59_ItemChanged(object sender, EventArgs e)
        {
            
        }

        private void navBarItem59_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (reservationMngt5 == null || reservationMngt5.Text == "")
            {
                reservationMngt5 = new ReservationMngt5();
                reservationMngt5.MdiParent = this;
                //reservationMngt5.v_empid = userEmpid;
                reservationMngt5.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(reservationMngt5.Text))
            {
                reservationMngt5.WindowState = FormWindowState.Maximized;
                reservationMngt5.Show();
                reservationMngt5.Focus();
            }
          
        }

        private void navBarItem60_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (ReportForm == null || ReportForm.Text == "")
            {
                ReportForm = new ReportForm();
                ReportForm.MdiParent = this;
                //ReportForm.v_empid = userEmpid;
                ReportForm.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(ReportForm.Text))
            {
                ReportForm.WindowState = FormWindowState.Maximized;
                ReportForm.Show();
                ReportForm.Focus();
            }
        }

        private void navBarItem61_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (FrmRawatInap == null || FrmRawatInap.Text == "")
            {
                FrmRawatInap = new FrmRawatInap();
                FrmRawatInap.MdiParent = this;
                //ReportForm.v_empid = userEmpid;
                FrmRawatInap.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(FrmRawatInap.Text))
            {
                FrmRawatInap.WindowState = FormWindowState.Maximized;
                FrmRawatInap.Show();
                FrmRawatInap.Focus();
            }
        }

        private void navBarItem62_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (patientInfo == null || patientInfo.Text == "")
            {
                patientInfo = new PatientInfo();
                patientInfo.MdiParent = this;
                //patientInfo.v_empid = userEmpid;
                patientInfo.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(patientInfo.Text))
            {
                patientInfo.WindowState = FormWindowState.Maximized;
                patientInfo.Show();
                patientInfo.Focus();
            }
        }

        private void navBarItem63_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (FrmTindakan == null || FrmTindakan.Text == "")
            {
                FrmTindakan = new FrmTindakan();
                FrmTindakan.MdiParent = this;
                //ReportForm.v_empid = userEmpid;
                FrmTindakan.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(FrmTindakan.Text))
            {
                FrmTindakan.WindowState = FormWindowState.Maximized;
                FrmTindakan.Show();
                FrmTindakan.Focus();
            }
        }

        private void navBarItem64_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (RawatInapBidan == null || RawatInapBidan.Text == "")
            {
                RawatInapBidan = new RawatInapBidan();
                RawatInapBidan.MdiParent = this;
                //ReportForm.v_empid = userEmpid;
                RawatInapBidan.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(RawatInapBidan.Text))
            {
                RawatInapBidan.WindowState = FormWindowState.Maximized;
                RawatInapBidan.Show();
                RawatInapBidan.Focus();
            }
        }

        private void navBarItem65_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (ReservationRegister == null || ReservationRegister.Text == "")
            {
                ReservationRegister = new ReservationRegister();
                ReservationRegister.MdiParent = this;
                //ReservationRegister.v_empid = userEmpid;
                ReservationRegister.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(ReservationRegister.Text))
            {
                ReservationRegister.WindowState = FormWindowState.Maximized;
                ReservationRegister.Show();
                ReservationRegister.Focus();
            }
        }

        private void navBarItem66_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (ReservationRegisterRI == null || ReservationRegisterRI.Text == "")
            {
                ReservationRegisterRI = new ReservationRegisterRI();
                ReservationRegisterRI.MdiParent = this;
                //ReservationRegisterRI.v_empid = userEmpid;
                ReservationRegisterRI.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(ReservationRegisterRI.Text))
            {
                ReservationRegisterRI.WindowState = FormWindowState.Maximized;
                ReservationRegisterRI.Show();
                ReservationRegisterRI.Focus();
            }
        }

        private void navBarItem67_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (AntrianPoli == null || AntrianPoli.Text == "")
            {
                //reservationQueue2 = new ReservationQueue2();
                AntrianPoli = new AntrianPoli();
                //reservationQueue2.MdiParent = this;
                AntrianPoli.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(AntrianPoli.Text))
            {
                AntrianPoli.WindowState = FormWindowState.Maximized;
                AntrianPoli.Show();
                AntrianPoli.Focus();
            }
        }

        private void navBarItem68_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (InspectionUSG == null || InspectionUSG.Text == "")
            {
                InspectionUSG = new InspectionUSG();
                InspectionUSG.MdiParent = this;
                //InspectionUSG.v_empid = userEmpid;
                InspectionUSG.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide(); 
            }
            else if (CheckOpened(InspectionUSG.Text))
            {
                InspectionUSG.WindowState = FormWindowState.Maximized;
                InspectionUSG.Show();
                InspectionUSG.Focus();
            } 
        }

        private void navBarItem69_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (MasterAsuransi == null || MasterAsuransi.Text == "")
            {
                MasterAsuransi = new MasterAsuransi();
                MasterAsuransi.MdiParent = this;
                //MasterAsuransi.v_empid = userEmpid;
                MasterAsuransi.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(MasterAsuransi.Text))
            {
                MasterAsuransi.WindowState = FormWindowState.Maximized;
                MasterAsuransi.Show();
                MasterAsuransi.Focus();
            }
        }

        private void navBarItem70_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (MedicineSeller == null || MedicineSeller.Text == "")
            {
                MedicineSeller = new MedicineSeller();
                MedicineSeller.MdiParent = this;
                //MedicineSeller.v_empid = userEmpid;
                MedicineSeller.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide(); 
            }
            else if (CheckOpened(MedicineSeller.Text))
            {
                MedicineSeller.WindowState = FormWindowState.Maximized;
                MedicineSeller.Show();
                MedicineSeller.Focus();
            }
        }

        private void navBarItem71_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (MasterChgPass == null || MasterChgPass.Text == "")
            {
                MasterChgPass = new MasterChgPass();
                MasterChgPass.MdiParent = this;
                //MedicineSeller.v_empid = userEmpid;
                MasterChgPass.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(MasterChgPass.Text))
            {
                MasterChgPass.WindowState = FormWindowState.Maximized;
                MasterChgPass.Show();
                MasterChgPass.Focus();
            }
        }

       
        private void ClinicMngt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoggedOut)
            {
              
                Application.Exit();
                // Jika pengguna sudah logout, kita tidak perlu konfirmasi keluar, cukup close aplikasi
                //Application.Exit(); // Aplikasi akan ditutup
            }
            else
            {
                if (Application.OpenForms.Count > 1)
                {
                    // Jika belum logout, tampilkan pesan konfirmasi keluar aplikasi
                    var result = MessageBox.Show("Do you want to Exit Application?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true; // Membatalkan penutupan aplikasi jika memilih "No"
                    }
                    //else
                    //{
                    //    Application.Exit();
                    //}
                }  
            }

            //if (Application.OpenForms.Count == 1)
            //{
            //    if (MessageBox.Show("Do you want to Exit Application?",
            //          "Message",
            //           MessageBoxButtons.YesNo,
            //           MessageBoxIcon.Information) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        Application.Exit();  // Menutup aplikasi sepenuhnya
            //    }

            //}
            //else
            //{
            //    e.Cancel = true;  // Mencegah form ditutup
            //    this.Hide();      // Sembunyikan form dan tetap jalankan aplikasi
            //} 
            //reservationScan = new ReservationScan();
            //reservationScan.Close();
        }
        private void ClinicMngt_FormClosed(object sender, FormClosedEventArgs e)
        {
            // if (MessageBox.Show("Do you want to Exit Application?",
            //           "Message",
            //            MessageBoxButtons.YesNo,
            //            MessageBoxIcon.Information) == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}

            //reservationScan = new ReservationScan();
            //reservationScan.Close();
        }

        private void navBarItem73_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (inspectionGigi == null || inspectionGigi.Text == "")
            {
                inspectionGigi = new InspectionGigi();
                inspectionGigi.MdiParent = this;
                //inspection.v_empid = userEmpid;
                inspectionGigi.v_name = ConnOra.my_IP;
                inspectionGigi.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide(); 
            }
            else if (CheckOpened(inspectionGigi.Text))
            {
                inspectionGigi.WindowState = FormWindowState.Maximized;
                inspectionGigi.Show();
                inspectionGigi.Focus();
            }
        }

        private void navBarItem74_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterDokter == null || masterDokter.Text == "")
            {
                masterDokter = new MasterDokter();
                masterDokter.MdiParent = this;
                masterDokter.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(masterDokter.Text))
            {
                masterDokter.WindowState = FormWindowState.Maximized;
                masterDokter.Show();
                masterDokter.Focus();
            }
        }

        private void navBarItem57_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterCode == null || masterCode.Text == "")
            {
                masterCode = new MasterCode();
                masterCode.MdiParent = this;
                //masterCode.v_empid = userEmpid;
                masterCode.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(masterCode.Text))
            {
                masterCode.WindowState = FormWindowState.Maximized;
                masterCode.Show();
                masterCode.Focus();
            }
        }

        private void navBarItem75_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (Lap_KunjunganRI == null || Lap_KunjunganRI.Text == "")
            {
                Lap_KunjunganRI = new Lap_KunjunganRI();
                Lap_KunjunganRI.MdiParent = this;
                Lap_KunjunganRI.v_empid = userEmpid;
                Lap_KunjunganRI.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide(); 
            }
            else if (CheckOpened(Lap_KunjunganRI.Text))
            {
                Lap_KunjunganRI.WindowState = FormWindowState.Maximized;
                Lap_KunjunganRI.Show();
                Lap_KunjunganRI.Focus();
            }
        }

        private void navBarItem76_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (Lap_KasHarian == null || Lap_KasHarian.Text == "")
            {
                Lap_KasHarian = new Lap_KasHarian();
                Lap_KasHarian.MdiParent = this;
                Lap_KasHarian.v_empid = userEmpid;
                Lap_KasHarian.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(Lap_KasHarian.Text))
            {
                Lap_KasHarian.WindowState = FormWindowState.Maximized;
                Lap_KasHarian.Show();
                Lap_KasHarian.Focus();
            }
        }

        private void navBarItem77_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (Lap_PenggunaApp == null || Lap_PenggunaApp.Text == "")
            {
                Lap_PenggunaApp = new Lap_PenggunaApp();
                Lap_PenggunaApp.MdiParent = this;
                Lap_PenggunaApp.v_empid = userEmpid;
                Lap_PenggunaApp.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();
            }
            else if (CheckOpened(Lap_PenggunaApp.Text))
            {
                Lap_PenggunaApp.WindowState = FormWindowState.Maximized;
                Lap_PenggunaApp.Show();
                Lap_PenggunaApp.Focus();
            }
        }

        private void navBarItem78_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //if (frmBpjswsAntreanBpjs == null || frmBpjswsAntreanBpjs.Text == "")
            //{
            //    frmBpjswsAntreanBpjs = new Bpjs.BpjswsAntreanBpjs();
            //    frmBpjswsAntreanBpjs.MdiParent = this;
            //    //treatNonMedis.v_empid = userEmpid;
            //    frmBpjswsAntreanBpjs.Show();
            //    this.panel1.Hide();
            //    this.pictureBox1.Hide();


            //}
            //else if (CheckOpened(frmBpjswsAntreanBpjs.Text))
            //{
            //    frmBpjswsAntreanBpjs.WindowState = FormWindowState.Maximized;
            //    frmBpjswsAntreanBpjs.Show();
            //    frmBpjswsAntreanBpjs.Focus();
            //}
        }

        private void navBarItem79_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (frmBpjswsTool == null || frmBpjswsTool.Text == "")
            {
                frmBpjswsTool = new Bpjsws.BpjswsTool();
                frmBpjswsTool.MdiParent = this;
                //treatNonMedis.v_empid = userEmpid;
                frmBpjswsTool.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(frmBpjswsTool.Text))
            {
                frmBpjswsTool.WindowState = FormWindowState.Maximized;
                frmBpjswsTool.Show();
                frmBpjswsTool.Focus();
            }
        }

        private void navBarItem80_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (masterSchedule == null || masterSchedule.Text == "")
            {
                masterSchedule = new MasterSchedule();
                masterSchedule.MdiParent = this;
                masterSchedule.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide(); 
            }
            else if (CheckOpened(masterSchedule.Text))
            {
                masterSchedule.WindowState = FormWindowState.Maximized;
                masterSchedule.Show();
                masterSchedule.Focus();
            }
        }

        private void navBarItem58_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //treatNonMedis
            if (treatNonMedis == null || treatNonMedis.Text == "")
            {
                treatNonMedis = new TreatNonMedis();
                treatNonMedis.MdiParent = this;
                //treatNonMedis.v_empid = userEmpid;
                treatNonMedis.Show();
                this.panel1.Hide();
                this.pictureBox1.Hide();


            }
            else if (CheckOpened(treatNonMedis.Text))
            {
                treatNonMedis.WindowState = FormWindowState.Maximized;
                treatNonMedis.Show();
                treatNonMedis.Focus();
            }
        }


        private void initDashboard()
        {
            if (userStatus == "SYS" || userStatus == "OFF" || userStatus == "DOH")
            {
                if (dashboardMonthly == null || dashboardMonthly.Text == "")
                {
                    dashboardMonthly = new DashboardMonthly();
                    dashboardMonthly.MdiParent = this;
                    dashboardMonthly.v_empid = userEmpid;
                    dashboardMonthly.Show();
                    this.panel1.Hide();
                    this.pictureBox1.Hide();
                    
                    
                }
                else if (CheckOpened(dashboardMonthly.Text))
                {
                    dashboardMonthly.WindowState = FormWindowState.Maximized;
                    dashboardMonthly.Show();
                    dashboardMonthly.Focus();
                }
            }
        }

    }
}