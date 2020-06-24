namespace MADS
{
    partial class MemoryViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoryViewer));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TextSeg = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StaticSeg = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DynSeg = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.LazyViewer = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.interpretDataDomain = new System.Windows.Forms.DomainUpDown();
            this.LoadDataBtn = new System.Windows.Forms.Button();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TextSeg);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 501);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text Segment";
            // 
            // TextSeg
            // 
            this.TextSeg.Location = new System.Drawing.Point(6, 19);
            this.TextSeg.Name = "TextSeg";
            this.TextSeg.Size = new System.Drawing.Size(192, 476);
            this.TextSeg.TabIndex = 0;
            this.TextSeg.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StaticSeg);
            this.groupBox2.Location = new System.Drawing.Point(224, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 497);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Static Segment";
            // 
            // StaticSeg
            // 
            this.StaticSeg.Location = new System.Drawing.Point(8, 14);
            this.StaticSeg.Name = "StaticSeg";
            this.StaticSeg.Size = new System.Drawing.Size(192, 477);
            this.StaticSeg.TabIndex = 0;
            this.StaticSeg.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DynSeg);
            this.groupBox3.Location = new System.Drawing.Point(578, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(538, 251);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dynamic Segment";
            // 
            // DynSeg
            // 
            this.DynSeg.Location = new System.Drawing.Point(6, 19);
            this.DynSeg.Name = "DynSeg";
            this.DynSeg.Size = new System.Drawing.Size(524, 226);
            this.DynSeg.TabIndex = 0;
            this.DynSeg.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.LazyViewer);
            this.groupBox4.Location = new System.Drawing.Point(580, 269);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(536, 251);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "The Lazy Viewer";
            // 
            // LazyViewer
            // 
            this.LazyViewer.Location = new System.Drawing.Point(6, 32);
            this.LazyViewer.Name = "LazyViewer";
            this.LazyViewer.Size = new System.Drawing.Size(524, 213);
            this.LazyViewer.TabIndex = 0;
            this.LazyViewer.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.LoadDataBtn);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(12, 519);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1124, 149);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Options";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioButton4);
            this.groupBox6.Controls.Add(this.radioButton3);
            this.groupBox6.Controls.Add(this.radioButton2);
            this.groupBox6.Controls.Add(this.radioButton1);
            this.groupBox6.Location = new System.Drawing.Point(6, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(217, 124);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ranges";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(563, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(433, 104);
            this.label1.TabIndex = 6;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // interpretDataDomain
            // 
            this.interpretDataDomain.Items.Add("Hex (4 bit)");
            this.interpretDataDomain.Items.Add("Hex (8 bit)");
            this.interpretDataDomain.Items.Add("Hex (32 bit)");
            this.interpretDataDomain.Items.Add("Int (32 bit)");
            this.interpretDataDomain.Location = new System.Drawing.Point(6, 19);
            this.interpretDataDomain.Name = "interpretDataDomain";
            this.interpretDataDomain.Size = new System.Drawing.Size(120, 20);
            this.interpretDataDomain.TabIndex = 5;
            this.interpretDataDomain.Text = "Interpret Data As:";
            // 
            // LoadDataBtn
            // 
            this.LoadDataBtn.Location = new System.Drawing.Point(458, 120);
            this.LoadDataBtn.Name = "LoadDataBtn";
            this.LoadDataBtn.Size = new System.Drawing.Size(75, 23);
            this.LoadDataBtn.TabIndex = 4;
            this.LoadDataBtn.Text = "Load Data";
            this.LoadDataBtn.UseVisualStyleBackColor = true;
            this.LoadDataBtn.Click += new System.EventHandler(this.LoadDataBtn_Click);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 81);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(160, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "View Single Segment Range";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 39);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(114, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "View From Ranges";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 62);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(144, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "View Single Segment Full";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(102, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "View All Memory";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.domainUpDown1);
            this.groupBox7.Controls.Add(this.interpretDataDomain);
            this.groupBox7.Location = new System.Drawing.Point(229, 19);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(223, 124);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Viewing Options";
            // 
            // domainUpDown1
            // 
            this.domainUpDown1.Items.Add("Integer");
            this.domainUpDown1.Items.Add("Hexadecimal");
            this.domainUpDown1.Location = new System.Drawing.Point(6, 45);
            this.domainUpDown1.Name = "domainUpDown1";
            this.domainUpDown1.Size = new System.Drawing.Size(120, 20);
            this.domainUpDown1.TabIndex = 6;
            this.domainUpDown1.Text = "View Addresses In:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(417, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "This merely the list the changes in the memory that happened in the order of occu" +
                "rance";
            // 
            // MemoryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 680);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MemoryViewer";
            this.Text = "MemoryViewer";
            this.Load += new System.EventHandler(this.MemoryViewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox TextSeg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox StaticSeg;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox DynSeg;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox LazyViewer;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button LoadDataBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DomainUpDown interpretDataDomain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
    }
}