using DevExpress.Utils; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clinic
{
    public partial class FrmTindakan : DevExpress.XtraEditors.XtraForm
    {
        private KoneksiOra koneksi;
        ConnectDb ConnOra = new ConnectDb();
        DataTable dt_grdPersalinanLalu;
        DataTable dt_grdPemberianAnstesi;
        DataTable dt_grdSebelumBedah;
        DataTable dt_grdSetelahBedah;
        DataTable dt_grdSPemantauanAnastesih;
        DataTable dt_grdPemantauanIv;
        public string p_anamnesa_id = "", pnama = "", prekam_medis = "";

        private string _anastesiId = "";
        private string _RM_NO = "";
        private string _NAME = "";

        public FrmTindakan()
        {
            InitializeComponent();
            koneksi = new KoneksiOra();
            

            //_anastesiId = anamesa_id;
            //_RM_NO = RM_NO;
            //_NAME = NAME;
        }

        private void FrmTindakan_Load(object sender, EventArgs e)
        {
            txt_anastesi_id.Text =  p_anamnesa_id;
            txt_pasien.Text =  pnama;
            textEdit1.Text =  prekam_medis;
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "FrmTindakan");
            selectedIndexRb();
            //btnInputData_Click.PerformClick();
            btnInputData_Click(new object(), new EventArgs());
            cb_ada_tindakan.CheckState = CheckState.Checked;
            setDateGrid(rptDateSebelum);
            setDateGrid(rptDateSesudah);
            setDateGrid(rptAnastesiLokal);



        }
        private void setDateGrid(DevExpress.XtraEditors.Repository.RepositoryItemDateEdit rptDate)
        {

            rptDate.Mask.EditMask = "dd/MM/yyyy";
            rptDate.Mask.UseMaskAsDisplayFormat = true;
            rptDate.VistaDisplayMode = DefaultBoolean.False;
            rptDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            rptDate.DisplayFormat.FormatType = FormatType.DateTime;
            rptDate.EditFormat.FormatString = "dd/MM/yyyy";
            rptDate.EditFormat.FormatType = FormatType.DateTime;
        }

        private void btnInputData_Click(object sender, EventArgs e)
        {

            try
            {
                string query = @"select count(*) from T2_TINDAKAN_BIDAN tp where ANAMESA_ID = '" + txt_anastesi_id.Text + "'";
                object result = koneksi.GetScalar(query);

                if (Convert.ToInt32(result) >= 1)
                {
                    getData();
                }
                else
                {
                    string queryInsert = @"insert into T2_TINDAKAN_BIDAN (id, anamesa_id,ada_tindakan) values (tindakan_bidan_seq.nextval, '" + txt_anastesi_id.Text + "','" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + "')";

                    bool success = koneksi.ExecuteNonQuery(queryInsert);

                    if (success)
                    {
                        loadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Load Data Gagal !!!!");
                        return;
                    }
                }

                kondisiEnable(true);
                //btnInputData.Enabled = false;
                btnSave.Enabled = true;
                dateEdit1.Enabled = true;
                dateEdit2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load data gagal !!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txt_anastesi_id.Text == "")
            {
                MessageBox.Show("Anastesi ID Kosong !!");
                return;
            }

            updateTable();
        }

        private void cb_ada_tindakan_CheckedChanged(object sender, EventArgs e)
        {
            xtraTabPage2.PageVisible = cb_ada_tindakan.Checked;

            if (cb_ada_tindakan.Checked)
            {

                try
                {

                    string query = @"select count(*) from T2_TINDAKAN_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "' ";
                    object result = koneksi.GetScalar(query);

                    if (Convert.ToInt32(result) >= 1)
                    {
                        //get Data
                    }
                    else
                    {
                        string queryInsert = @"insert into T2_TINDAKAN_BIDAN (id, anamesa_id,ada_tindakan) values (tindakan_bidan_seq.nextval, '" + txt_anastesi_id.Text + "','" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + "')";

                        bool success = koneksi.ExecuteNonQuery(queryInsert);

                        if (!success)
                        {
                            MessageBox.Show("Simpan Data Gagal");
                            return;
                        }

                    }


                }
                catch (Exception ex)
                {

                }

            }
        }
        

        private void getData()
        {

            string querySelect3 = "SELECT * FROM T2_TINDAKAN_BIDAN where anamesa_id = '" + txt_anastesi_id.Text + "'";
            DataTable dataTable3 = koneksi.GetDataTable(querySelect3);

            if (dataTable3.Rows.Count > 0)
            {
                cb_ada_tindakan.Checked = functionChk(dataTable3.Rows[0]["ada_tindakan"].ToString(), "Ya");
            
                txt_nama_tindakan.EditValue = dataTable3.Rows[0]["nama_tindakan"].ToString();
                txt_dokter_pelaksana.EditValue = dataTable3.Rows[0]["dokter_pelaksana"].ToString();
                txt_pemberi_informasi.EditValue = dataTable3.Rows[0]["pemberi_informasi"].ToString();
                txt_penerima_informasi.EditValue = dataTable3.Rows[0]["penerima_informasi"].ToString();
                date_tgl_diberikan.EditValue = dataTable3.Rows[0]["tgl_diberikan"].ToString();
                txt_diagnosis.EditValue = dataTable3.Rows[0]["diagnosis"].ToString();
                txt_dasar_diagnosis.EditValue = dataTable3.Rows[0]["dasar_diagnosis"].ToString();
                txt_tindakan_medis.EditValue = dataTable3.Rows[0]["tindakan_medis"].ToString();
                txt_indikasi_tindakan.EditValue = dataTable3.Rows[0]["indikasi_tindakan"].ToString();
                txt_tata_cara.EditValue = dataTable3.Rows[0]["tata_cara"].ToString();
                txt_tujuan.EditValue = dataTable3.Rows[0]["tujuan"].ToString();
                txt_resiko.EditValue = dataTable3.Rows[0]["resiko"].ToString();
                txt_komplikasi.EditValue = dataTable3.Rows[0]["komplikasi"].ToString();
                txt_prognosis.EditValue = dataTable3.Rows[0]["prognosis"].ToString();
                txt_alternatif.EditValue = dataTable3.Rows[0]["alternatif"].ToString();
                txt_lain_lain.EditValue = dataTable3.Rows[0]["lain_lain"].ToString();
                txt_keluhan_utama.EditValue = dataTable3.Rows[0]["keluhan_utama"].ToString();
                txt_riwayat_penyakit_sekarang.EditValue = dataTable3.Rows[0]["riwayat_penyakit_sekarang"].ToString();
                txt_riwayat_penyakit_dahulu.EditValue = dataTable3.Rows[0]["riwayat_penyakit_dahulu"].ToString();
                txt_riwayat_penyakit_keluarga.EditValue = dataTable3.Rows[0]["riwayat_penyakit_keluarga"].ToString();
                txt_riwayat_alergi.EditValue = dataTable3.Rows[0]["riwayat_alergi"].ToString();
                txt_riwayat_pengobatan.EditValue = dataTable3.Rows[0]["riwayat_pengobatan"].ToString();
                txt_keadaan_umum.EditValue = dataTable3.Rows[0]["keadaan_umum"].ToString();
                txt_kesadaran.EditValue = dataTable3.Rows[0]["kesadaran"].ToString();
                txt_fisik_tanda_vital.EditValue = dataTable3.Rows[0]["fisik_tanda_vital"].ToString();
                txt_tekanan_darah.EditValue = dataTable3.Rows[0]["tekanan_darah"].ToString();
                txt_nadi.EditValue = dataTable3.Rows[0]["nadi"].ToString();
                txt_suhu.EditValue = dataTable3.Rows[0]["suhu"].ToString();
                txt_rr.EditValue = dataTable3.Rows[0]["rr"].ToString();
                txt_berat_badan.EditValue = dataTable3.Rows[0]["berat_badan"].ToString();
                txt_status_gizi.EditValue = dataTable3.Rows[0]["status_gizi"].ToString();
                txt_kepala_leher.EditValue = dataTable3.Rows[0]["kepala_leher"].ToString();
                txt_thoraks.EditValue = dataTable3.Rows[0]["thoraks"].ToString();
                txt_abdomen.EditValue = dataTable3.Rows[0]["abdomen"].ToString();
                txt_extremitas.EditValue = dataTable3.Rows[0]["extremitas"].ToString();
                txt_laboratorium.EditValue = dataTable3.Rows[0]["laboratorium"].ToString();
                txt_rontgen.EditValue = dataTable3.Rows[0]["rontgen"].ToString();
                txt_penunjang_lain_lain.EditValue = dataTable3.Rows[0]["penunjang_lain_lain"].ToString();
                txt_diagnosa_prabedah.EditValue = dataTable3.Rows[0]["diagnosa_prabedah"].ToString();
                txt_rencana_tindakan.EditValue = dataTable3.Rows[0]["rencana_tindakan"].ToString();
                txt_rencana_monitoring.EditValue = dataTable3.Rows[0]["rencana_monitoring"].ToString();
                txt_rencana_edukasi.EditValue = dataTable3.Rows[0]["rencana_edukasi"].ToString();

                functionSplitIndex_2(dataTable3.Rows[0]["pasien_alergi"].ToString(), rb_pasien_alergi);
                functionSplitIndex_2(dataTable3.Rows[0]["risiko_aspirasi"].ToString(), rb_risiko_aspirasi);
                functionSplitIndex_2(dataTable3.Rows[0]["pendarahan"].ToString(), rb_pendarahan);
                functionSplitIndex_2(dataTable3.Rows[0]["profilaksi"].ToString(), rb_profilaksi);
                functionSplitIndex_2(dataTable3.Rows[0]["hasil_imaging"].ToString(), rb_hasil_imaging);

            }

            loadDataGrid();


        }
        private void loadDataGrid()
        {
            


            string querySelect6 = "SELECT ID, ANAMESA_ID, URUTAN_KE, NAMA_OBAT, DOSIS, TEKNIK_ANASTESI, WAKTU, TENSI, NADI, RR, SUHU, DECODE (TANGGAL, '', '', TO_DATE (TANGGAL, 'DD/MM/YYYY')) TANGGAL FROM T2_MTR_PEMBERIAN_ANAMNESA  where anamesa_id = '" + txt_anastesi_id.Text + "'";
            dt_grdPemberianAnstesi = koneksi.GetDataTable(querySelect6);

            ConvertColumnNamesToUppercase(dt_grdPemberianAnstesi);
            grdPemberianAnstesi.DataSource = dt_grdPemberianAnstesi;


            string querySelect7 = "SELECT ID, ANAMESA_ID, URUTAN_KE, JENIS, DECODE (TANGGAL, '', '', TO_DATE (TANGGAL, 'DD/MM/YYYY')) TANGGAL, JAM, TINDAKAN, KESADARAN, TD, N, RR, S, KETERANGAN FROM T2_MTR_PEMBEDAHAN  where anamesa_id = '" + txt_anastesi_id.Text + "' and jenis = 'SEBELUM' ";
            dt_grdSebelumBedah = koneksi.GetDataTable(querySelect7);

            ConvertColumnNamesToUppercase(dt_grdSebelumBedah);
            grdSebelumBedah.DataSource = dt_grdSebelumBedah;

            string querySelect8 = "SELECT ID, ANAMESA_ID, URUTAN_KE, JENIS, DECODE (TANGGAL, '', '', TO_DATE (TANGGAL, 'DD/MM/YYYY')) TANGGAL, JAM, TINDAKAN, KESADARAN, TD, N, RR, S, KETERANGAN FROM T2_MTR_PEMBEDAHAN  where anamesa_id = '" + txt_anastesi_id.Text + "' and jenis = 'SETELAH' ";
            dt_grdSetelahBedah = koneksi.GetDataTable(querySelect8);

            ConvertColumnNamesToUppercase(dt_grdSetelahBedah);
            grdSetelahBedah.DataSource = dt_grdSetelahBedah;


            string querySelect9 = "SELECT * FROM T2_PMT_ANASTESI  where anamesa_id = '" + txt_anastesi_id.Text + "' ";
            dt_grdSPemantauanAnastesih = koneksi.GetDataTable(querySelect9);

            ConvertColumnNamesToUppercase(dt_grdSPemantauanAnastesih);
            grdPemantauanAnastesi.DataSource = dt_grdSPemantauanAnastesih;

            if (dt_grdSPemantauanAnastesih.Rows.Count > 0)
            {
                txtJam.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["JAM_MULAI"].ToString();
                date_pemantauan.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["TANGGAL"].ToString();
                txtKesadaran.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["KESADARAN"].ToString();
                txtJamSelesai.EditValue = dt_grdSPemantauanAnastesih.Rows[0]["JAM_SELESAI_TINDAKAN"].ToString();

            }
            
        }
        

        private void btnAddMonitoring3_Click(object sender, EventArgs e)
        {

            if (dt_grdSPemantauanAnastesih == null) return;

            DataRow newRow = dt_grdSPemantauanAnastesih.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemantauanAnastesi.RowCount) + 1).ToString();
            dt_grdSPemantauanAnastesih.Rows.Add(newRow);

            grdPemantauanAnastesi.DataSource = dt_grdSPemantauanAnastesih;
        }

        private void btnSaveMonitoring3_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSPemantauanAnastesih.Rows)
                {


                    string query = @"select count(*) from T2_PMT_ANASTESI where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_PMT_ANASTESI set
                                                            urutan_ke   = '" + row["URUTAN_KE"] + @"',
                                                            nama_obat   = '" + row["NAMA_OBAT"] + @"',
                                                            dosis       = '" + row["DOSIS"] + @"',
                                                            rr          = '" + row["RR"] + @"',
                                                            hr          = '" + row["HR"] + @"',
                                                            tds         = '" + row["TDS"] + @"',
                                                            tdd         = '" + row["TDD"] + @"',
                                                            waktu_15    = '" + row["WAKTU_15"] + @"',
                                                            waktu_30    = '" + row["WAKTU_30"] + @"',
                                                            waktu_45    = '" + row["WAKTU_45"] + @"',
                                                            waktu_60    = '" + row["WAKTU_60"] + @"',
                                                            waktu_90    = '" + row["WAKTU_90"] + @"',
                                                            keterangan  = '" + row["KETERANGAN"] + @"',
                                                            jam_mulai   = '" + txtJam.Text + @"',
                                                            tanggal     = '" + date_pemantauan.Text + @"',
                                                            kesadaran   = '" + txtKesadaran.Text + @"',
                                                            jam_selesai_tindakan = '" + txtJamSelesai.Text + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_PMT_ANASTESI(
                                                                    id,
                                                                    anamesa_id,
                                                                    urutan_ke,
                                                                    nama_obat,
                                                                    dosis,
                                                                    rr,
                                                                    hr,
                                                                    tds,
                                                                    tdd,
                                                                    waktu_15,
                                                                    waktu_30,
                                                                    waktu_45,
                                                                    waktu_60,
                                                                    waktu_90,
                                                                    keterangan,
                                                                    jam_mulai,
                                                                    tanggal,
                                                                    kesadaran,
                                                                    jam_selesai_tindakan) values (
                                                                    pemantauan_anastesi_seq.nextval,
                                                                    '" + txt_anastesi_id.Text + @"',
                                                                    '" + row["URUTAN_KE"] + @"',
                                                                    '" + row["NAMA_OBAT"] + @"',
                                                                    '" + row["DOSIS"] + @"',
                                                                    '" + row["RR"] + @"',
                                                                    '" + row["HR"] + @"',
                                                                    '" + row["TDS"] + @"',
                                                                    '" + row["TDD"] + @"',
                                                                    '" + row["WAKTU_15"] + @"',
                                                                    '" + row["WAKTU_30"] + @"',
                                                                    '" + row["WAKTU_45"] + @"',
                                                                    '" + row["WAKTU_60"] + @"',
                                                                    '" + row["WAKTU_90"] + @"',
                                                                    '" + row["KETERANGAN"] + @"',
                                                                    '" + txtJam.Text + @"',
                                                                    '" + date_pemantauan.Text + @"',
                                                                    '" + txtKesadaran.Text + @"',
                                                                    '" + txtJamSelesai.Text + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }

        }

        private void btnAddMonitoring4_Click(object sender, EventArgs e)
        {

            if (dt_grdSetelahBedah == null) return;

            DataRow newRow = dt_grdSetelahBedah.NewRow();

            newRow["URUTAN_KE"] = ((gvwSetelahBedah.RowCount) + 1).ToString();
            dt_grdSetelahBedah.Rows.Add(newRow);

            grdSetelahBedah.DataSource = dt_grdSetelahBedah;
        }

        private void btnSaveMonitoring4_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSetelahBedah.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBEDAHAN where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SETELAH'";
                    object result = koneksi.GetScalar(query);

                    string tanggal = row["TANGGAL"].ToString() != "" ? Convert.ToDateTime(row["TANGGAL"]).ToString("dd/MM/yyyy") : "";

                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_MTR_PEMBEDAHAN set
                                                                    urutan_ke       = '" + row["URUTAN_KE"] + @"',
                                                                    tanggal         = '" + tanggal + @"',
                                                                    jam             = '" + row["JAM"] + @"',
                                                                    tindakan        = '" + row["TINDAKAN"] + @"',
                                                                    kesadaran       = '" + row["KESADARAN"] + @"',
                                                                    td              = '" + row["TD"] + @"',
                                                                    n               = '" + row["N"] + @"',
                                                                    rr              = '" + row["RR"] + @"',
                                                                    s               = '" + row["S"] + @"',
                                                                    keterangan      = '" + row["KETERANGAN"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SETELAH' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBEDAHAN(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                jenis,
                                                                tanggal,
                                                                jam,
                                                                tindakan,
                                                                kesadaran,
                                                                td,
                                                                n,
                                                                rr,
                                                                s,
                                                                keterangan) values (
                                                                monitoring_pembedahan_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                'SETELAH',
                                                                '" + tanggal + @"',
                                                                '" + row["JAM"] + @"',
                                                                '" + row["TINDAKAN"] + @"',
                                                                '" + row["KESADARAN"] + @"',
                                                                '" + row["TD"] + @"',
                                                                '" + row["N"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + row["S"] + @"',
                                                                '" + row["KETERANGAN"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void btnAddMonitoring2_Click(object sender, EventArgs e)
        {

            if (dt_grdSebelumBedah == null) return;

            DataRow newRow = dt_grdSebelumBedah.NewRow();

            newRow["URUTAN_KE"] = ((gvwSebelumBedah.RowCount) + 1).ToString();
            dt_grdSebelumBedah.Rows.Add(newRow);

            grdSebelumBedah.DataSource = dt_grdSebelumBedah;
        }

        private void btnSaveMonitoring2_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdSebelumBedah.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBEDAHAN where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SEBELUM' ";
                    object result = koneksi.GetScalar(query);

                    string tanggal = row["TANGGAL"].ToString() != "" ? Convert.ToDateTime(row["TANGGAL"]).ToString("dd/MM/yyyy") : "";

                    if (Convert.ToInt32(result) >= 1)
                    {


                        string queryInsert = @"update  T2_MTR_PEMBEDAHAN set
                                                                    urutan_ke   = '" + row["URUTAN_KE"] + @"',
                                                                    tanggal     = '" + tanggal + @"',
                                                                    jam         = '" + row["JAM"] + @"',
                                                                    tindakan    = '" + row["TINDAKAN"] + @"',
                                                                    kesadaran   = '" + row["KESADARAN"] + @"',
                                                                    td          = '" + row["TD"] + @"',
                                                                    n           = '" + row["N"] + @"',
                                                                    rr          = '" + row["RR"] + @"',
                                                                    s           = '" + row["S"] + @"',
                                                                    keterangan  = '" + row["KETERANGAN"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' and jenis = 'SEBELUM' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBEDAHAN(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                jenis,
                                                                tanggal,
                                                                jam,
                                                                tindakan,
                                                                kesadaran,
                                                                td,
                                                                n,
                                                                rr,
                                                                s,
                                                                keterangan) values (
                                                                monitoring_pembedahan_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                'SEBELUM',
                                                                '" + tanggal + @"',
                                                                '" + row["JAM"] + @"',
                                                                '" + row["TINDAKAN"] + @"',
                                                                '" + row["KESADARAN"] + @"',
                                                                '" + row["TD"] + @"',
                                                                '" + row["N"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + row["S"] + @"',
                                                                '" + row["KETERANGAN"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }

        }

        private void btnAddMonitoring1_Click(object sender, EventArgs e)
        {

            if (dt_grdPemberianAnstesi == null) return;

            DataRow newRow = dt_grdPemberianAnstesi.NewRow();

            newRow["URUTAN_KE"] = ((gvwPemberianAnstesi.RowCount) + 1).ToString();
            dt_grdPemberianAnstesi.Rows.Add(newRow);

            grdPemberianAnstesi.DataSource = dt_grdPemberianAnstesi;
        }

        private void btnSaveMonitoring1_Click(object sender, EventArgs e)
        {
            try
            {

                bool success = false;
                foreach (DataRow row in dt_grdPemberianAnstesi.Rows)
                {


                    string query = @"select count(*) from T2_MTR_PEMBERIAN_ANAMNESA where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";
                    object result = koneksi.GetScalar(query);

                    string tanggal = row["TANGGAL"].ToString() != "" ? Convert.ToDateTime(row["TANGGAL"]).ToString("dd/MM/yyyy") : "";
                    if (Convert.ToInt32(result) >= 1)
                    {
                        string queryInsert = @"update  T2_MTR_PEMBERIAN_ANAMNESA set
                                                                urutan_ke       = '" + row["URUTAN_KE"] + @"',
                                                                nama_obat       = '" + row["NAMA_OBAT"] + @"',
                                                                dosis           = '" + row["DOSIS"] + @"',
                                                                teknik_anastesi = '" + row["TEKNIK_ANASTESI"] + @"',
                                                                waktu           = '" + row["WAKTU"] + @"',
                                                                tensi           = '" + row["TENSI"] + @"',
                                                                nadi            = '" + row["NADI"] + @"',
                                                                tanggal         = '" + tanggal + @"',
                                                                rr              = '" + row["RR"] + @"'
                                                      where anamesa_id = '" + txt_anastesi_id.Text + "' and urutan_ke = '" + row["URUTAN_KE"] + "' ";

                        success = koneksi.ExecuteNonQuery(queryInsert);

                    }
                    else
                    {
                        string queryInsert = @"insert into T2_MTR_PEMBERIAN_ANAMNESA(
                                                                id,
                                                                anamesa_id,
                                                                urutan_ke,
                                                                nama_obat,
                                                                dosis,
                                                                teknik_anastesi,
                                                                waktu,
                                                                tensi,
                                                                nadi,
                                                                rr,
                                                                tanggal,
                                                                suhu) values (
                                                                m_pemberian_anastesi_seq.nextval,
                                                                '" + txt_anastesi_id.Text + @"',
                                                                '" + row["URUTAN_KE"] + @"',
                                                                '" + row["NAMA_OBAT"] + @"',
                                                                '" + row["DOSIS"] + @"',
                                                                '" + row["TEKNIK_ANASTESI"] + @"',
                                                                '" + row["WAKTU"] + @"',
                                                                '" + row["TENSI"] + @"',
                                                                '" + row["NADI"] + @"',
                                                                '" + row["RR"] + @"',
                                                                '" + tanggal + @"',
                                                                '" + row["SUHU"] + @"'
                                                            ) ";

                        success = koneksi.ExecuteNonQuery(queryInsert);



                    }

                }

                if (success)
                {
                    MessageBox.Show("Data Berhasil Disimpan");
                }
                else
                {
                    MessageBox.Show("Data Gagal Disimpan !!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }

        }
        

        private void updateTable()
        {
            try
            {

                List<string> updateQueries = new List<string>
                {

                };
                
                
                updateQueries.Add(@"update T2_TINDAKAN_BIDAN set
                                        ada_tindakan        = '" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + @"',
                                        nama_tindakan       = '" + txt_nama_tindakan.Text + @"',
                                        dokter_pelaksana    = '" + txt_dokter_pelaksana.Text + @"',
                                        pemberi_informasi   = '" + txt_pemberi_informasi.Text + @"',
                                        penerima_informasi  = '" + txt_penerima_informasi.Text + @"',
                                        tgl_diberikan       = '" + date_tgl_diberikan.Text + @"',
                                        diagnosis           = '" + txt_diagnosis.Text + @"',
                                        dasar_diagnosis     = '" + txt_dasar_diagnosis.Text + @"',
                                        tindakan_medis      = '" + txt_tindakan_medis.Text + @"',
                                        indikasi_tindakan   = '" + txt_indikasi_tindakan.Text + @"',
                                        tata_cara           = '" + txt_tata_cara.Text + @"',
                                        tujuan              = '" + txt_tujuan.Text + @"',
                                        resiko              = '" + txt_resiko.Text + @"',
                                        komplikasi          = '" + txt_komplikasi.Text + @"',
                                        prognosis           = '" + txt_prognosis.Text + @"',
                                        alternatif          = '" + txt_alternatif.Text + @"',
                                        lain_lain           = '" + txt_lain_lain.Text + @"',
                                        keluhan_utama       = '" + txt_keluhan_utama.Text + @"',
                                        riwayat_penyakit_sekarang   = '" + txt_riwayat_penyakit_sekarang.Text + @"',
                                        riwayat_penyakit_dahulu     = '" + txt_riwayat_penyakit_dahulu.Text + @"',
                                        riwayat_penyakit_keluarga   = '" + txt_riwayat_penyakit_keluarga.Text + @"',
                                        riwayat_alergi      = '" + txt_riwayat_alergi.Text + @"',
                                        riwayat_pengobatan  = '" + txt_riwayat_pengobatan.Text + @"',
                                        keadaan_umum        = '" + txt_keadaan_umum.Text + @"',
                                        kesadaran           = '" + txt_kesadaran.Text + @"',
                                        fisik_tanda_vital   = '" + txt_fisik_tanda_vital.Text + @"',
                                        tekanan_darah       = '" + txt_tekanan_darah.Text + @"',
                                        nadi                = '" + txt_nadi.Text + @"',
                                        suhu                = '" + txt_suhu.Text + @"',
                                        rr                  = '" + txt_rr.Text + @"',
                                        berat_badan         = '" + txt_berat_badan.Text + @"',
                                        status_gizi         = '" + txt_status_gizi.Text + @"',
                                        kepala_leher        = '" + txt_kepala_leher.Text + @"',
                                        thoraks             = '" + txt_thoraks.Text + @"',
                                        abdomen             = '" + txt_abdomen.Text + @"',
                                        extremitas          = '" + txt_extremitas.Text + @"',
                                        laboratorium        = '" + txt_laboratorium.Text + @"',
                                        rontgen             = '" + txt_rontgen.Text + @"',
                                        penunjang_lain_lain = '" + txt_penunjang_lain_lain.Text + @"',
                                        diagnosa_prabedah   = '" + txt_diagnosa_prabedah.Text + @"',
                                        rencana_tindakan    = '" + txt_rencana_tindakan.Text + @"',
                                        rencana_monitoring  = '" + txt_rencana_monitoring.Text + @"',
                                        rencana_edukasi     = '" + txt_rencana_edukasi.Text + @"',
                                        pasien_alergi       = '" + rb_pasien_alergi.SelectedIndex.ToString() + "::" + rb_pasien_alergi.Text + @"',
                                        risiko_aspirasi     = '" + rb_risiko_aspirasi.SelectedIndex.ToString() + "::" + rb_risiko_aspirasi.Text + @"',
                                        pendarahan          = '" + rb_pendarahan.SelectedIndex.ToString() + "::" + rb_pendarahan.Text + @"',
                                        profilaksi          = '" + rb_profilaksi.SelectedIndex.ToString() + "::" + rb_profilaksi.Text + @"',
                                        hasil_imaging       = '" + rb_hasil_imaging.SelectedIndex.ToString() + "::" + rb_hasil_imaging.Text + @"'
                                 where anamesa_id = '" + txt_anastesi_id.Text + "' ");
                

                string query = @"select count(*) from T2_TINDAKAN_BIDAN tp where anamesa_id = '" + txt_anastesi_id.Text + "'";
                object result = koneksi.GetScalar(query);

                if (Convert.ToInt32(result) >= 1)
                {
                    updateQueries.Add(@"update T2_TINDAKAN_BIDAN set
                                        ada_tindakan        = '" + (cb_ada_tindakan.Checked ? cb_ada_tindakan.Text : "Tidak") + @"'
                                 where anamesa_id = '" + txt_anastesi_id.Text + "' ");

                }

                koneksi.OpenConnection();
                koneksi.BeginTransaction();

                foreach (string updateQuery in updateQueries)
                {
                    bool success = koneksi.ExecuteNonQueryCommitRollback(updateQuery);
                    if (!success)
                    {
                        if (!success)
                        {
                            koneksi.RollbackTransaction();
                            MessageBox.Show("Data Gagal Disimpan !!");
                            return;
                        }
                    }
                }

                koneksi.CommitTransaction();
                MessageBox.Show("Data Berhasil Disimpan");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Gagal Disimpan !!");

            }
            finally
            {
                koneksi.CloseConnection();
            }

        }

        private void selectedIndexRb()
        {
            rb_pasien_alergi.SelectedIndex = -1;
            rb_risiko_aspirasi.SelectedIndex = -1;
            rb_pendarahan.SelectedIndex = -1;
            rb_profilaksi.SelectedIndex = -1;
            rb_hasil_imaging.SelectedIndex = -1;
            

        }
        private void kondisiEnable(bool kondisi)
        {

            cb_ada_tindakan.Enabled = kondisi;
            
        }

        static void ConvertColumnNamesToUppercase(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                column.ColumnName = column.ColumnName.ToUpper();
            }
        }

        private bool functionChk(string data_asli, string pencarian)
        {
            string[] aa = data_asli.Split(new string[] { pencarian }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void functionSplitIndex_2(string data_asli, DevExpress.XtraEditors.RadioGroup rbt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 2)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
            }

        }
        private void functionSplitIndex_3(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 3)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
            }

        }
        private void functionSplitIndex_4_asi(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt, DevExpress.XtraEditors.TextEdit txt1)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 4)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
                txt1.EditValue = (aa[3] == null) ? "" : aa[3];
            }

        }
        private void functionSplitIndex_10(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 9)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[9] == null) ? "" : aa[9];
            }

        }
        private void functionSplitIndex_5(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.RadioGroup rbt1, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 5)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                rbt1.SelectedIndex = Convert.ToInt32(aa[2]);
                txt.EditValue = (aa[4] == null) ? "" : aa[4];
            }

        }
        
        
    }
}
