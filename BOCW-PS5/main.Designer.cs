namespace BOCW_PS5
{
    partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            label_ps5_ip = new Label();
            box_ps5_ip = new TextBox();
            btn_connect = new Button();
            label_log = new Label();
            groupBox1 = new GroupBox();
            btn_cbuf = new Button();
            box_cbuf = new TextBox();
            btn_loadElf = new Button();
            groupBox2 = new GroupBox();
            label1 = new Label();
            btn_reloadElf = new Button();
            btn_unloadElf = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label_ps5_ip
            // 
            label_ps5_ip.AutoSize = true;
            label_ps5_ip.Location = new Point(12, 15);
            label_ps5_ip.Name = "label_ps5_ip";
            label_ps5_ip.Size = new Size(42, 15);
            label_ps5_ip.TabIndex = 0;
            label_ps5_ip.Text = "PS5 IP:";
            // 
            // box_ps5_ip
            // 
            box_ps5_ip.Location = new Point(60, 12);
            box_ps5_ip.Name = "box_ps5_ip";
            box_ps5_ip.Size = new Size(100, 23);
            box_ps5_ip.TabIndex = 1;
            box_ps5_ip.TextChanged += box_ps5_ip_TextChanged;
            // 
            // btn_connect
            // 
            btn_connect.Location = new Point(166, 12);
            btn_connect.Name = "btn_connect";
            btn_connect.Size = new Size(75, 23);
            btn_connect.TabIndex = 0;
            btn_connect.Text = "Connect";
            btn_connect.UseVisualStyleBackColor = true;
            btn_connect.Click += btn_connect_Click;
            // 
            // label_log
            // 
            label_log.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label_log.BackColor = SystemColors.ControlLight;
            label_log.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label_log.Location = new Point(12, 45);
            label_log.Name = "label_log";
            label_log.Size = new Size(229, 380);
            label_log.TabIndex = 2;
            label_log.Text = "Idle";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(btn_cbuf);
            groupBox1.Controls.Add(box_cbuf);
            groupBox1.Location = new Point(247, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(541, 51);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Console Command";
            // 
            // btn_cbuf
            // 
            btn_cbuf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_cbuf.Location = new Point(485, 22);
            btn_cbuf.Name = "btn_cbuf";
            btn_cbuf.Size = new Size(50, 23);
            btn_cbuf.TabIndex = 2;
            btn_cbuf.Text = "Send";
            btn_cbuf.UseVisualStyleBackColor = true;
            btn_cbuf.Click += btn_cbuf_Click;
            // 
            // box_cbuf
            // 
            box_cbuf.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            box_cbuf.Location = new Point(6, 22);
            box_cbuf.Name = "box_cbuf";
            box_cbuf.Size = new Size(473, 23);
            box_cbuf.TabIndex = 3;
            box_cbuf.Text = "g_speed 500";
            // 
            // btn_loadElf
            // 
            btn_loadElf.Enabled = false;
            btn_loadElf.Location = new Point(6, 22);
            btn_loadElf.Name = "btn_loadElf";
            btn_loadElf.Size = new Size(75, 23);
            btn_loadElf.TabIndex = 4;
            btn_loadElf.Text = "Load ELF...";
            btn_loadElf.UseVisualStyleBackColor = true;
            btn_loadElf.Click += btn_loadElf_Click;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(btn_reloadElf);
            groupBox2.Controls.Add(btn_unloadElf);
            groupBox2.Controls.Add(btn_loadElf);
            groupBox2.Location = new Point(247, 69);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(541, 80);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "ELF Loader";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(249, 26);
            label1.Name = "label1";
            label1.Size = new Size(195, 15);
            label1.TabIndex = 7;
            label1.Text = "(Unload and load elf from last path)";
            // 
            // btn_reloadElf
            // 
            btn_reloadElf.Enabled = false;
            btn_reloadElf.Location = new Point(87, 22);
            btn_reloadElf.Name = "btn_reloadElf";
            btn_reloadElf.Size = new Size(156, 23);
            btn_reloadElf.TabIndex = 6;
            btn_reloadElf.Text = "Reload Last ELF";
            btn_reloadElf.UseVisualStyleBackColor = true;
            btn_reloadElf.Click += btn_reloadElf_Click;
            // 
            // btn_unloadElf
            // 
            btn_unloadElf.Enabled = false;
            btn_unloadElf.Location = new Point(6, 51);
            btn_unloadElf.Name = "btn_unloadElf";
            btn_unloadElf.Size = new Size(75, 23);
            btn_unloadElf.TabIndex = 5;
            btn_unloadElf.Text = "Unload ELF";
            btn_unloadElf.UseVisualStyleBackColor = true;
            btn_unloadElf.Click += btn_unloadElf_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(122, 17);
            toolStripStatusLabel1.Text = "Created by DeathRGH";
            // 
            // main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label_log);
            Controls.Add(btn_connect);
            Controls.Add(box_ps5_ip);
            Controls.Add(label_ps5_ip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "main";
            Text = "BOCW 1.26 PS5";
            Load += main_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_ps5_ip;
        private TextBox box_ps5_ip;
        private Button btn_connect;
        private Label label_log;
        private GroupBox groupBox1;
        private Button btn_cbuf;
        private TextBox box_cbuf;
        private Button btn_loadElf;
        private GroupBox groupBox2;
        private Button btn_unloadElf;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btn_reloadElf;
        private Label label1;
    }
}