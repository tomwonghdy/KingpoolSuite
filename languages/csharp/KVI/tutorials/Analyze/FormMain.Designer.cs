
namespace Analize
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.btnBinaryFeature = new System.Windows.Forms.Button();
            this.btnCarity = new System.Windows.Forms.Button();
            this.btnHistogram = new System.Windows.Forms.Button();
            this.btnNonzeroPixels = new System.Windows.Forms.Button();
            this.btnProject = new System.Windows.Forms.Button();
            this.btnStrengthStatistics = new System.Windows.Forms.Button();
            this.xguiPanel1 = new Analize.XguiPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbTop = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbLeft = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbHeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCannyContrast = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxOutput
            // 
            this.tbxOutput.BackColor = System.Drawing.Color.Black;
            this.tbxOutput.ForeColor = System.Drawing.Color.White;
            this.tbxOutput.Location = new System.Drawing.Point(276, 23);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.ReadOnly = true;
            this.tbxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxOutput.Size = new System.Drawing.Size(473, 355);
            this.tbxOutput.TabIndex = 6;
            // 
            // btnBinaryFeature
            // 
            this.btnBinaryFeature.Location = new System.Drawing.Point(47, 27);
            this.btnBinaryFeature.Name = "btnBinaryFeature";
            this.btnBinaryFeature.Size = new System.Drawing.Size(158, 41);
            this.btnBinaryFeature.TabIndex = 7;
            this.btnBinaryFeature.Text = "Binary Features";
            this.btnBinaryFeature.UseVisualStyleBackColor = true;
            this.btnBinaryFeature.Click += new System.EventHandler(this.btnBinaryFeature_Click);
            // 
            // btnCarity
            // 
            this.btnCarity.Location = new System.Drawing.Point(47, 74);
            this.btnCarity.Name = "btnCarity";
            this.btnCarity.Size = new System.Drawing.Size(158, 41);
            this.btnCarity.TabIndex = 8;
            this.btnCarity.Text = "Clearness";
            this.btnCarity.UseVisualStyleBackColor = true;
            this.btnCarity.Click += new System.EventHandler(this.btnCarity_Click);
            // 
            // btnHistogram
            // 
            this.btnHistogram.Location = new System.Drawing.Point(47, 121);
            this.btnHistogram.Name = "btnHistogram";
            this.btnHistogram.Size = new System.Drawing.Size(158, 41);
            this.btnHistogram.TabIndex = 9;
            this.btnHistogram.Text = "Histogram";
            this.btnHistogram.UseVisualStyleBackColor = true;
            this.btnHistogram.Click += new System.EventHandler(this.btnHistogram_Click);
            // 
            // btnNonzeroPixels
            // 
            this.btnNonzeroPixels.Location = new System.Drawing.Point(47, 168);
            this.btnNonzeroPixels.Name = "btnNonzeroPixels";
            this.btnNonzeroPixels.Size = new System.Drawing.Size(158, 41);
            this.btnNonzeroPixels.TabIndex = 11;
            this.btnNonzeroPixels.Text = "Non-zero Pixels";
            this.btnNonzeroPixels.UseVisualStyleBackColor = true;
            this.btnNonzeroPixels.Click += new System.EventHandler(this.btnNonzeroPixels_Click);
            // 
            // btnProject
            // 
            this.btnProject.Location = new System.Drawing.Point(47, 215);
            this.btnProject.Name = "btnProject";
            this.btnProject.Size = new System.Drawing.Size(158, 41);
            this.btnProject.TabIndex = 12;
            this.btnProject.Text = "Project";
            this.btnProject.UseVisualStyleBackColor = true;
            this.btnProject.Click += new System.EventHandler(this.btnProject_Click);
            // 
            // btnStrengthStatistics
            // 
            this.btnStrengthStatistics.Location = new System.Drawing.Point(47, 262);
            this.btnStrengthStatistics.Name = "btnStrengthStatistics";
            this.btnStrengthStatistics.Size = new System.Drawing.Size(158, 41);
            this.btnStrengthStatistics.TabIndex = 13;
            this.btnStrengthStatistics.Text = "Strength Statistics";
            this.btnStrengthStatistics.UseVisualStyleBackColor = true;
            this.btnStrengthStatistics.Click += new System.EventHandler(this.btnStrengthStatistics_Click);
            // 
            // xguiPanel1
            // 
            this.xguiPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.xguiPanel1.Location = new System.Drawing.Point(276, 385);
            this.xguiPanel1.Name = "xguiPanel1";
            this.xguiPanel1.Size = new System.Drawing.Size(473, 180);
            this.xguiPanel1.TabIndex = 15;
            this.xguiPanel1.Text = "xguiPanel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbTop);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txbLeft);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txbHeight);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txbWidth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(47, 423);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 142);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mask";
            // 
            // txbTop
            // 
            this.txbTop.Location = new System.Drawing.Point(112, 51);
            this.txbTop.Name = "txbTop";
            this.txbTop.Size = new System.Drawing.Size(55, 21);
            this.txbTop.TabIndex = 7;
            this.txbTop.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Top";
            // 
            // txbLeft
            // 
            this.txbLeft.Location = new System.Drawing.Point(29, 51);
            this.txbLeft.Name = "txbLeft";
            this.txbLeft.Size = new System.Drawing.Size(55, 21);
            this.txbLeft.TabIndex = 5;
            this.txbLeft.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Left";
            // 
            // txbHeight
            // 
            this.txbHeight.Location = new System.Drawing.Point(112, 95);
            this.txbHeight.Name = "txbHeight";
            this.txbHeight.Size = new System.Drawing.Size(55, 21);
            this.txbHeight.TabIndex = 3;
            this.txbHeight.Text = "80";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Height";
            // 
            // txbWidth
            // 
            this.txbWidth.Location = new System.Drawing.Point(29, 95);
            this.txbWidth.Name = "txbWidth";
            this.txbWidth.Size = new System.Drawing.Size(55, 21);
            this.txbWidth.TabIndex = 1;
            this.txbWidth.Text = "100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Width";
            // 
            // btnCannyContrast
            // 
            this.btnCannyContrast.Location = new System.Drawing.Point(47, 309);
            this.btnCannyContrast.Name = "btnCannyContrast";
            this.btnCannyContrast.Size = new System.Drawing.Size(158, 41);
            this.btnCannyContrast.TabIndex = 17;
            this.btnCannyContrast.Text = "Canny Contrast";
            this.btnCannyContrast.UseVisualStyleBackColor = true;
            this.btnCannyContrast.Click += new System.EventHandler(this.btnCannyContrast_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 577);
            this.Controls.Add(this.btnCannyContrast);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.xguiPanel1);
            this.Controls.Add(this.btnStrengthStatistics);
            this.Controls.Add(this.btnProject);
            this.Controls.Add(this.btnNonzeroPixels);
            this.Controls.Add(this.btnHistogram);
            this.Controls.Add(this.btnCarity);
            this.Controls.Add(this.btnBinaryFeature);
            this.Controls.Add(this.tbxOutput);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Analyse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.Button btnBinaryFeature;
        private System.Windows.Forms.Button btnCarity;
        private System.Windows.Forms.Button btnHistogram;
        private System.Windows.Forms.Button btnNonzeroPixels;
        private System.Windows.Forms.Button btnProject;
        private System.Windows.Forms.Button btnStrengthStatistics;
        private XguiPanel xguiPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbTop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbLeft;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCannyContrast;
    }
}

