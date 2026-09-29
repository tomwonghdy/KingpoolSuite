
namespace BlobBasics
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
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnDensity = new System.Windows.Forms.Button();
            this.btnBlobDistance = new System.Windows.Forms.Button();
            this.btnToMask = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbEncoder = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbMinimumSize = new System.Windows.Forms.TextBox();
            this.xguiPanel1 = new BlobBasics.XguiPanel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxOutput
            // 
            this.tbxOutput.BackColor = System.Drawing.Color.Black;
            this.tbxOutput.ForeColor = System.Drawing.Color.White;
            this.tbxOutput.Location = new System.Drawing.Point(248, 417);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.ReadOnly = true;
            this.tbxOutput.Size = new System.Drawing.Size(503, 129);
            this.tbxOutput.TabIndex = 6;
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(32, 28);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(172, 48);
            this.btnExtract.TabIndex = 16;
            this.btnExtract.Text = "Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(32, 87);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(172, 48);
            this.btnProperties.TabIndex = 17;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click_1);
            // 
            // btnDensity
            // 
            this.btnDensity.Location = new System.Drawing.Point(32, 264);
            this.btnDensity.Name = "btnDensity";
            this.btnDensity.Size = new System.Drawing.Size(172, 48);
            this.btnDensity.TabIndex = 19;
            this.btnDensity.Text = "Blob Density";
            this.btnDensity.UseVisualStyleBackColor = true;
            this.btnDensity.Click += new System.EventHandler(this.btnDensity_Click);
            // 
            // btnBlobDistance
            // 
            this.btnBlobDistance.Location = new System.Drawing.Point(32, 205);
            this.btnBlobDistance.Name = "btnBlobDistance";
            this.btnBlobDistance.Size = new System.Drawing.Size(172, 48);
            this.btnBlobDistance.TabIndex = 18;
            this.btnBlobDistance.Text = "Blob Distance";
            this.btnBlobDistance.UseVisualStyleBackColor = true;
            this.btnBlobDistance.Click += new System.EventHandler(this.btnBlobDistance_Click);
            // 
            // btnToMask
            // 
            this.btnToMask.Location = new System.Drawing.Point(32, 323);
            this.btnToMask.Name = "btnToMask";
            this.btnToMask.Size = new System.Drawing.Size(172, 48);
            this.btnToMask.TabIndex = 20;
            this.btnToMask.Text = "To Mask";
            this.btnToMask.UseVisualStyleBackColor = true;
            this.btnToMask.Click += new System.EventHandler(this.btnToMask_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(32, 146);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(172, 48);
            this.btnStatistics.TabIndex = 21;
            this.btnStatistics.Text = "Pixel Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbMinimumSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbEncoder);
            this.groupBox1.Location = new System.Drawing.Point(39, 406);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 139);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // cmbEncoder
            // 
            this.cmbEncoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoder.FormattingEnabled = true;
            this.cmbEncoder.Items.AddRange(new object[] {
            "Default",
            "Custer",
            "Contour"});
            this.cmbEncoder.Location = new System.Drawing.Point(24, 50);
            this.cmbEncoder.Name = "cmbEncoder";
            this.cmbEncoder.Size = new System.Drawing.Size(129, 20);
            this.cmbEncoder.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Encoder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Minimum Size";
            // 
            // txbMinimumSize
            // 
            this.txbMinimumSize.Location = new System.Drawing.Point(24, 97);
            this.txbMinimumSize.Name = "txbMinimumSize";
            this.txbMinimumSize.Size = new System.Drawing.Size(74, 21);
            this.txbMinimumSize.TabIndex = 3;
            this.txbMinimumSize.Text = "7";
            // 
            // xguiPanel1
            // 
            this.xguiPanel1.BackColor = System.Drawing.Color.Silver;
            this.xguiPanel1.Location = new System.Drawing.Point(248, 15);
            this.xguiPanel1.Name = "xguiPanel1";
            this.xguiPanel1.Size = new System.Drawing.Size(500, 396);
            this.xguiPanel1.TabIndex = 22;
            this.xguiPanel1.Text = "xguiPanel1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 554);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.xguiPanel1);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.btnToMask);
            this.Controls.Add(this.btnDensity);
            this.Controls.Add(this.btnBlobDistance);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.tbxOutput);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blob Basics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnDensity;
        private System.Windows.Forms.Button btnBlobDistance;
        private System.Windows.Forms.Button btnToMask;
        private System.Windows.Forms.Button btnStatistics;
        private XguiPanel xguiPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbMinimumSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEncoder;
    }
}

