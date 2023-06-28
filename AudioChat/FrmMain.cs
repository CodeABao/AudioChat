using EdgeTTS;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using Microsoft.Scripting.Hosting;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using NAudio.Gui;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Whisper.net;
using static IronPython.Modules.PythonCsvModule;

namespace AudioChat
{
    public partial class FrmMain : Form
    {
        WhisperProcessor whisperProcessor;
        public FrmMain()
        {
            var factory = WhisperFactory.FromPath("D:\\AI\\Whisper\\whisper-bin-x64\\ggml-medium.bin");

            var builder = factory.CreateBuilder()
                .WithLanguage("auto");

            whisperProcessor = builder.Build();

            InitializeComponent();
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            webView21.Source = new Uri("https://chat.openai.com/c/ef83ba76-b6f2-4e7e-b840-cf132799fd6c");

            webView21.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;

            //helper = webView21.CoreWebView2.GetDevToolsProtocolHelper();
            //await helper.Network.EnableAsync();
        }

        private async void CoreWebView2_WebResourceResponseReceived(object sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
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
                        //document.getElementsByClassName("markdown")[58].innerText
                    }
                }
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await TextToVoice("测试一下播放");
        }

        WaveInEvent waveIn = null;
        WasapiLoopbackCapture capture = null;
        WaveFileWriter writer = null;
        Stream stream;
        public void StartRecordAudio()
        {
            try
            {
                waveIn = new WaveInEvent();
                writer = new WaveFileWriter(stream, waveIn.WaveFormat);
                //开始录音，写数据
                waveIn.DataAvailable += (s, a) =>
                {
                    writer.Write(a.Buffer, 0, a.BytesRecorded);
                };

                //结束录音
                waveIn.RecordingStopped += (s, a) =>
                {
                    writer.Dispose();
                    writer = null;
                    waveIn.Dispose();

                };
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private async void BtnSound_Click(object sender, EventArgs e)
        {
            await foreach (var result in whisperProcessor.ProcessAsync(stream))
            {
                System.Console.WriteLine($"{result.Start}->{result.End}: {result.Text}");
            }
        }

        private async Task TextToVoice(string text)
        {
            string voices = cbVoices.Text;
            var etts = new EdgeTTSClient();
            var result = await etts.SynthesisAsync(text, voices.Split('|').FirstOrDefault());
            if (result.Code != ResultCode.Success)
            {
                System.Console.WriteLine("生成失败");
                return;
            }

            var fileName = "\\data\\" + Guid.NewGuid() + ".mp3";

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
}
