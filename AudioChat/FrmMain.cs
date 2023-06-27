using EdgeTTS;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using Microsoft.Scripting.Hosting;
using Microsoft.Web.WebView2.Core;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioChat
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://chat.openai.com/c/ef83ba76-b6f2-4e7e-b840-cf132799fd6c");
            await webView21.EnsureCoreWebView2Async();
            webView21.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            
        }

        List<string> responseMessage = new List<string>();
        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();

            Console.WriteLine(message);

            if (message.Contains("[DONE]"))
            {
                string allContext = responseMessage[responseMessage.Count - 1];
                allContext = allContext.Remove(0, 6);

                responseMessage.Clear();
                var gptMessage = JsonConvert.DeserializeObject<ChatGPTMessage>(allContext);

                foreach (var item in gptMessage.content.parts)
                {
                    //await TextToVoice(item);
                }
            }
            else
            {
                responseMessage.Add(message);
            }
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

                        Console.WriteLine("<<" + context + ">>");
                    }
                    else
                    {
                        Console.WriteLine("data is null");
                    }
                }
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void BtnSound_Click(object sender, EventArgs e)
        {

        }

        private async Task TextToVoice(string text)
        {
            string voices = cbVoices.Text;
            var etts = new EdgeTTSClient();
            var result = await etts.SynthesisAsync(text, voices.Split('|').FirstOrDefault());
            if (result.Code != ResultCode.Success)
            {
                Console.WriteLine("生成失败");
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
