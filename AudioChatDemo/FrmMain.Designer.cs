﻿namespace AudioChatDemo
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            cbVoices = new ComboBox();
            btnStart = new Button();
            btnSound = new Button();
            cnDevices = new ComboBox();
            设备 = new Label();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(3, 3);
            webView21.Name = "webView21";
            webView21.Size = new Size(868, 677);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(877, 5);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 1;
            label1.Text = "扮演";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(879, 25);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(332, 191);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(879, 231);
            label2.Name = "label2";
            label2.Size = new Size(32, 17);
            label2.TabIndex = 3;
            label2.Text = "发声";
            // 
            // cbVoices
            // 
            cbVoices.FormattingEnabled = true;
            cbVoices.Items.AddRange(new object[] { "zh-CN-XiaoxiaoNeural|Female", "zh-CN-XiaoyiNeural|Female", "zh-CN-YunjianNeuralMale", "zh-CN-YunxiNeural|Male", "zh-CN-YunyangNeural|Male", "zh-CN-liaoning-XiaobeiNeural|Female", "zh-CN-shaanxi-XiaoniNeural|Female", "zh-TW-HsiaoChenNeural|Female", "zh-TW-HsiaoYuNeural|Female", "zh-TW-YunJheNeural|Male", "en-US-AnaNeural|Female", "en-US-AriaNeural|Female", "en-US-ChristopherNeural|Male", "en-US-EricNeural|Male" });
            cbVoices.Location = new Point(879, 251);
            cbVoices.Name = "cbVoices";
            cbVoices.Size = new Size(332, 25);
            cbVoices.TabIndex = 4;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(1117, 292);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 35);
            btnStart.TabIndex = 5;
            btnStart.Text = "开始";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnSound
            // 
            btnSound.Location = new Point(879, 600);
            btnSound.Name = "btnSound";
            btnSound.Size = new Size(334, 35);
            btnSound.TabIndex = 6;
            btnSound.Text = "说话";
            btnSound.UseVisualStyleBackColor = true;
            btnSound.Click += BtnSound_Click;
            // 
            // cnDevices
            // 
            cnDevices.FormattingEnabled = true;
            cnDevices.Items.AddRange(new object[] { "zh-CN-XiaoxiaoNeural|Female", "zh-CN-XiaoyiNeural|Female", "zh-CN-YunjianNeuralMale", "zh-CN-YunxiNeural|Male", "zh-CN-YunyangNeural|Male", "zh-CN-liaoning-XiaobeiNeural|Female", "zh-CN-shaanxi-XiaoniNeural|Female", "zh-TW-HsiaoChenNeural|Female", "zh-TW-HsiaoYuNeural|Female", "zh-TW-YunJheNeural|Male", "en-US-AnaNeural|Female", "en-US-AriaNeural|Female", "en-US-ChristopherNeural|Male", "en-US-EricNeural|Male" });
            cnDevices.Location = new Point(881, 553);
            cnDevices.Name = "cnDevices";
            cnDevices.Size = new Size(332, 25);
            cnDevices.TabIndex = 7;
            // 
            // 设备
            // 
            设备.AutoSize = true;
            设备.Location = new Point(881, 533);
            设备.Name = "设备";
            设备.Size = new Size(32, 17);
            设备.TabIndex = 8;
            设备.Text = "发声";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1221, 681);
            Controls.Add(设备);
            Controls.Add(cnDevices);
            Controls.Add(btnSound);
            Controls.Add(btnStart);
            Controls.Add(cbVoices);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(webView21);
            Name = "FrmMain";
            Text = "ChatGPT";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private ComboBox cbVoices;
        private Button btnStart;
        private Button btnSound;
        private ComboBox cnDevices;
        private Label 设备;
    }
}