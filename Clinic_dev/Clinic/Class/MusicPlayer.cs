
using NAudio.Wave;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media; 

namespace Clinic
{

    public class MusicPlayer
    {
        private MediaPlayer mediaPlayer;
        //private bool isPlaying = false;
        //private bool waitingForPlaybackCompletion = false;

        public event EventHandler PlaybackCompleted;

        public MusicPlayer()
        {
            mediaPlayer = new MediaPlayer();
        }
        
        public void CallPasien(string Poli)
        {
            string teks = Poli;


            //PlaySoundUrl(teks);
            PlaySoundSequence(teks);
        }
        private void PlaySoundSequence(string text)
        {
            Task.Run(async () =>
            {
                try
                {
                    //ganti sesuai path lokasi
                    await PlayLocalSound(@"C:\Clinic\suara_antrian1.wav");

                    string url = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl=id&client=gtx", Uri.EscapeDataString(text));
                    await PlaySoundFromUrl(url);

                    //ganti sesuai path lokasi
                    await PlayLocalSound(@"C:\Clinic\suara_antrian2.wav");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            });
        }
        private async Task PlayLocalSound(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var audioFile = new AudioFileReader(filePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    var tcs = new TaskCompletionSource<bool>();
                    outputDevice.PlaybackStopped += (sender, e) => tcs.SetResult(true);

                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    await tcs.Task;
                }
            }

        }
        private async Task PlaySoundFromUrl(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                using (Stream stream = client.OpenRead(url))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    using (Mp3FileReader mp3Reader = new Mp3FileReader(memoryStream))
                    using (WaveOutEvent waveOut = new WaveOutEvent())
                    {
                        var tcs = new TaskCompletionSource<bool>();
                        waveOut.PlaybackStopped += (sender, e) => tcs.SetResult(true);

                        waveOut.Init(mp3Reader);
                        waveOut.Play();

                        await tcs.Task;
                    }
                }
            }
            catch
            {

            }
        }
        private void PlaySoundUrl(string text)
        {
            Task.Run(async () =>
            {
                try
                {
                    //SoundPlayer player = new SoundPlayer(p_dir + "suara_antrian1" + fname);
                    //player.PlaySync();

                    string url = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl=id&client=gtx", Uri.EscapeDataString(text));

                    using (WebClient client = new WebClient())
                    {
                        using (Stream stream = client.OpenRead(url))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memoryStream);
                                memoryStream.Position = 0; 
                                
                                using (Mp3FileReader mp3Reader = new Mp3FileReader(memoryStream))
                                {
                                    using (WaveOutEvent waveOut = new WaveOutEvent())
                                    {
                                        var tcs = new TaskCompletionSource<bool>();
                                        waveOut.PlaybackStopped += (sender, e) => tcs.SetResult(true);

                                        waveOut.Init(mp3Reader);
                                        waveOut.Play();
                                        
                                        await tcs.Task;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            });
        }

        public void WaitForPlaybackCompletion()
        {
            //if (isPlaying)
            //{
            //    waitingForPlaybackCompletion = true;
            //}
            //else
            //{
            //    PlaybackCompleted?.Invoke(this, EventArgs.Empty);
            //}
        }
    }


}
