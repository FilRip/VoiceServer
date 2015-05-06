namespace VoiceServer
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblNbPlugins = new System.Windows.Forms.Label();
            this.txtNbPlugins = new System.Windows.Forms.TextBox();
            this.rapport = new System.Windows.Forms.RichTextBox();
            this.sysTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(12, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 41);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(12, 70);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "RELOAD";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblNbPlugins
            // 
            this.lblNbPlugins.AutoSize = true;
            this.lblNbPlugins.Location = new System.Drawing.Point(12, 109);
            this.lblNbPlugins.Name = "lblNbPlugins";
            this.lblNbPlugins.Size = new System.Drawing.Size(104, 13);
            this.lblNbPlugins.TabIndex = 3;
            this.lblNbPlugins.Text = "Nombre de plugins : ";
            // 
            // txtNbPlugins
            // 
            this.txtNbPlugins.Location = new System.Drawing.Point(122, 106);
            this.txtNbPlugins.Name = "txtNbPlugins";
            this.txtNbPlugins.ReadOnly = true;
            this.txtNbPlugins.Size = new System.Drawing.Size(50, 20);
            this.txtNbPlugins.TabIndex = 4;
            // 
            // rapport
            // 
            this.rapport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rapport.Location = new System.Drawing.Point(12, 141);
            this.rapport.Name = "rapport";
            this.rapport.Size = new System.Drawing.Size(461, 159);
            this.rapport.TabIndex = 5;
            this.rapport.Text = "";
            // 
            // sysTray
            // 
            this.sysTray.Icon = ((System.Drawing.Icon)(resources.GetObject("sysTray.Icon")));
            this.sysTray.Text = "notifyIcon1";
            this.sysTray.Visible = true;
            this.sysTray.DoubleClick += new System.EventHandler(this.sysTray_DoubleClick);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(97, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "QUITTER";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 312);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.rapport);
            this.Controls.Add(this.txtNbPlugins);
            this.Controls.Add(this.lblNbPlugins);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Name = "mainForm";
            this.Text = "IGOR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Shown += new System.EventHandler(this.mainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblNbPlugins;
        private System.Windows.Forms.TextBox txtNbPlugins;
        private System.Windows.Forms.RichTextBox rapport;
        private System.Windows.Forms.NotifyIcon sysTray;
        private System.Windows.Forms.Button btnQuit;
    }
}