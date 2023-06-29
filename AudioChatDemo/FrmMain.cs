using EdgeTTS;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Microsoft.Web.WebView2.WinForms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Whisper.net;

namespace AudioChatDemo
{
    public partial class FrmMain : Form
    {
        static string voice = "zh-CN-XiaoxiaoNeural";
        DevToolsProtocolHelper helper = null;
        WhisperProcessor whisperProcessor;
        public FrmMain()
        {
            var factory = WhisperFactory.FromPath("D:\\AI\\Whisper\\whisper-bin-x64\\ggml-base.bin");

            var builder = factory.CreateBuilder()
                .WithLanguage("auto");

            whisperProcessor = builder.Build();

            InitializeComponent();
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {

            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--allow-no-sandbox-job");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);

            await webView21.EnsureCoreWebView2Async(environment);

            webView21.Source = new Uri("https://chat.openai.com/c/ef83ba76-b6f2-4e7e-b840-cf132799fd6c");

            helper = webView21.CoreWebView2.GetDevToolsProtocolHelper();
            Bridge bridge = new Bridge(helper, webView21);

            webView21.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
            webView21.CoreWebView2.AddHostObjectToScript("bridge", bridge);



            //await helper.Network.EnableAsync();
            string commentJs = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\scripts\\comment.js");
            await webView21.CoreWebView2.ExecuteScriptAsync(commentJs);

            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();

            cnDevices.DataSource = devices;

        }

        private async void CoreWebView2_WebResourceResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (isStart)
            {
                if (e.Request.Uri.StartsWith("https://chat.openai.com/backend-api/conversation"))
                {
                    using (var data = await e.Response.GetContentAsync())
                    {
                        if (data != null)
                        {
                            StreamReader streamReader = new StreamReader(data);
                            string context = streamReader.ReadToEnd();

                            System.Console.WriteLine("<<" + context + ">>");
                        }
                        else
                        {
                            //TODO:获取内容
                            await webView21.CoreWebView2.ExecuteScriptAsync("DOUYINDATA.GetResponse()");
                        }
                    }
                }
            }
        }

        bool isStart = false;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isStart)
            {
                isStart = false;
                btnSound.Text = "开始";
            }
            else
            {
                if (string.IsNullOrEmpty(cbVoices.Text))
                {
                    MessageBox.Show("请先选择话说角色");
                    return;
                }
                isStart = true;
                btnSound.Text = "结束";
                voice = cbVoices.Text.Split('|').FirstOrDefault();
            }
        }

        WaveInEvent waveIn = null;
        WaveFileWriter writer = null;
        string inputVoice = string.Empty;
        public void StartRecordAudio()
        {
            try
            {
                inputVoice = "voices\\" + DateTime.Now.Ticks + ".wav";
                var deviceNumber = cnDevices.SelectedIndex - 1;
                waveIn = new WaveInEvent() { DeviceNumber = deviceNumber };
                waveIn.WaveFormat = new WaveFormat(16000, 1);

                writer = new WaveFileWriter(inputVoice, waveIn.WaveFormat);
                //开始录音，写数据
                waveIn.DataAvailable += WaveIn_DataAvailable;

                //结束录音
                waveIn.RecordingStopped += WaveIn_RecordingStopped;
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void Cleanup()
        {
            if (waveIn != null)
            {
                waveIn.DataAvailable -= WaveIn_DataAvailable;
                waveIn.RecordingStopped -= WaveIn_RecordingStopped;
                waveIn.Dispose();
                waveIn = null;
            }
            FinalizeWaveFile();
        }

        private void FinalizeWaveFile()
        {
            writer?.Dispose();
            writer = null;
        }


        private async void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();
            writer = null;
            waveIn.Dispose();

            using var fileStream = File.OpenRead(inputVoice);
            using var wavStream = new MemoryStream();

            using var reader = new WaveFileReader(fileStream);
            var resampler = new WdlResamplingSampleProvider(reader.ToSampleProvider(), 16000);
            WaveFileWriter.WriteWavFileToStream(wavStream, resampler.ToWaveProvider16());
            wavStream.Seek(0, SeekOrigin.Begin);


            await foreach (var result in whisperProcessor.ProcessAsync(wavStream))
            {
                System.Console.WriteLine($"{result.Start}->{result.End}: {result.Text}");
                await webView21.CoreWebView2.ExecuteScriptAsync("DOUYINDATA.InputComment('" + result.Text + "')");
            }
        }

        bool listening = false;
        private void BtnSound_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                listening = false;
                btnSound.Text = "说话";

                waveIn.StopRecording();
            }
            else
            {
                Cleanup();
                StartRecordAudio();
                listening = true;
                btnSound.Text = "说话中...";
            }
        }

        public static async Task TextToVoice(string text)
        {
            text = text.Replace("\n\n", ".");
            var etts = new EdgeTTSClient();
            var result = await etts.SynthesisAsync(text, voice);
            if (result.Code != ResultCode.Success)
            {
                System.Console.WriteLine("生成失败");
                return;
            }

            var fileName = "data\\" + Guid.NewGuid() + ".mp3";

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(result.Data.ToArray()); ;
            fs.Close();

            Mp3FileReader reader = new Mp3FileReader(fileName);
            WaveOut wout = new WaveOut();
            wout.Init(reader);
            wout.Play();
        }
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Bridge
    {
        DevToolsProtocolHelper _helper = null;
        WebView2 _webView2 = null;
        public Bridge(DevToolsProtocolHelper helper, WebView2 webView2)
        {
            _helper = helper;
            _webView2 = webView2;
        }
        public async void InsertTextAsync(string msg)
        {
            await _helper.Input.InsertTextAsync(msg);
            System.Threading.Thread.Sleep(1000);
            await _webView2.CoreWebView2.ExecuteScriptAsync("DOUYINDATA.CommitComment()");
        }

        public async void BackResponse(string msg)
        {
            await FrmMain.TextToVoice(msg);
        }

    }
}