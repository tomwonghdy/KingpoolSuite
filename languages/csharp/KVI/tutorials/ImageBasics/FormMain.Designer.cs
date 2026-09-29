
namespace ImageBasics
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
            this.btnCreateFill = new System.Windows.Forms.Button();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.btnPixelFormat = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnLoadFromMemory = new System.Windows.Forms.Button();
            this.btnExportToMemory = new System.Windows.Forms.Button();
            this.picSrc = new System.Windows.Forms.PictureBox();
            this.picDest = new System.Windows.Forms.PictureBox();
            this.btnCloneDummy = new System.Windows.Forms.Button();
            this.lblHome = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDest)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateFill
            // 
            this.btnCreateFill.Location = new System.Drawing.Point(15, 23);
            this.btnCreateFill.Name = "btnCreateFill";
            this.btnCreateFill.Size = new System.Drawing.Size(106, 31);
            this.btnCreateFill.TabIndex = 0;
            this.btnCreateFill.Text = "Create And Fill";
            this.btnCreateFill.UseVisualStyleBackColor = true;
            this.btnCreateFill.Click += new System.EventHandler(this.btnCreateFill_Click);
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(137, 23);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(106, 31);
            this.btnReadFile.TabIndex = 2;
            this.btnReadFile.Text = "Read From File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(137, 66);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(106, 31);
            this.btnSaveFile.TabIndex = 3;
            this.btnSaveFile.Text = "Save To File";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(15, 66);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(106, 31);
            this.btnClone.TabIndex = 4;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(15, 152);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(106, 31);
            this.btnProperties.TabIndex = 5;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // tbxOutput
            // 
            this.tbxOutput.BackColor = System.Drawing.Color.Black;
            this.tbxOutput.ForeColor = System.Drawing.Color.White;
            this.tbxOutput.Location = new System.Drawing.Point(276, 401);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.ReadOnly = true;
            this.tbxOutput.Size = new System.Drawing.Size(473, 154);
            this.tbxOutput.TabIndex = 6;
            // 
            // btnPixelFormat
            // 
            this.btnPixelFormat.Location = new System.Drawing.Point(137, 109);
            this.btnPixelFormat.Name = "btnPixelFormat";
            this.btnPixelFormat.Size = new System.Drawing.Size(106, 31);
            this.btnPixelFormat.TabIndex = 7;
            this.btnPixelFormat.Text = "Pixel Format";
            this.btnPixelFormat.UseVisualStyleBackColor = true;
            this.btnPixelFormat.Click += new System.EventHandler(this.btnPixelFormat_Click);
            // 
            // btnResize
            // 
            this.btnResize.Location = new System.Drawing.Point(15, 109);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(106, 31);
            this.btnResize.TabIndex = 9;
            this.btnResize.Text = "Resize";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(137, 152);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(106, 31);
            this.btnSplit.TabIndex = 10;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(137, 195);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(106, 31);
            this.btnMerge.TabIndex = 11;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnLoadFromMemory
            // 
            this.btnLoadFromMemory.Location = new System.Drawing.Point(15, 238);
            this.btnLoadFromMemory.Name = "btnLoadFromMemory";
            this.btnLoadFromMemory.Size = new System.Drawing.Size(106, 39);
            this.btnLoadFromMemory.TabIndex = 12;
            this.btnLoadFromMemory.Text = "Load From Memory";
            this.btnLoadFromMemory.UseVisualStyleBackColor = true;
            this.btnLoadFromMemory.Click += new System.EventHandler(this.btnLoadFromMemory_Click);
            // 
            // btnExportToMemory
            // 
            this.btnExportToMemory.Location = new System.Drawing.Point(137, 238);
            this.btnExportToMemory.Name = "btnExportToMemory";
            this.btnExportToMemory.Size = new System.Drawing.Size(106, 39);
            this.btnExportToMemory.TabIndex = 13;
            this.btnExportToMemory.Text = "Export To Memory";
            this.btnExportToMemory.UseVisualStyleBackColor = true;
            this.btnExportToMemory.Click += new System.EventHandler(this.btnExportToMemory_Click);
            // 
            // picSrc
            // 
            this.picSrc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picSrc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picSrc.Location = new System.Drawing.Point(15, 353);
            this.picSrc.Name = "picSrc";
            this.picSrc.Size = new System.Drawing.Size(233, 202);
            this.picSrc.TabIndex = 14;
            this.picSrc.TabStop = false;
            // 
            // picDest
            // 
            this.picDest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picDest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picDest.Location = new System.Drawing.Point(276, 12);
            this.picDest.Name = "picDest";
            this.picDest.Size = new System.Drawing.Size(473, 383);
            this.picDest.TabIndex = 15;
            this.picDest.TabStop = false;
            // 
            // btnCloneDummy
            // 
            this.btnCloneDummy.Location = new System.Drawing.Point(15, 195);
            this.btnCloneDummy.Name = "btnCloneDummy";
            this.btnCloneDummy.Size = new System.Drawing.Size(106, 31);
            this.btnCloneDummy.TabIndex = 16;
            this.btnCloneDummy.Text = "Dummy Image";
            this.btnCloneDummy.UseVisualStyleBackColor = true;
            this.btnCloneDummy.Click += new System.EventHandler(this.btnCloneDummy_Click);
            // 
            // lblHome
            // 
            this.lblHome.Location = new System.Drawing.Point(17, 328);
            this.lblHome.Name = "lblHome";
            this.lblHome.Size = new System.Drawing.Size(231, 22);
            this.lblHome.TabIndex = 17;
            this.lblHome.TabStop = true;
            this.lblHome.Text = "Home page at Github ";
            this.lblHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 577);
            this.Controls.Add(this.lblHome);
            this.Controls.Add(this.btnCloneDummy);
            this.Controls.Add(this.picDest);
            this.Controls.Add(this.picSrc);
            this.Controls.Add(this.btnExportToMemory);
            this.Controls.Add(this.btnLoadFromMemory);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.btnResize);
            this.Controls.Add(this.btnPixelFormat);
            this.Controls.Add(this.tbxOutput);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.btnSaveFile);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.btnCreateFill);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Basics";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateFill;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.Button btnPixelFormat;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnLoadFromMemory;
        private System.Windows.Forms.Button btnExportToMemory;
        private System.Windows.Forms.PictureBox picSrc;
        private System.Windows.Forms.PictureBox picDest;
        private System.Windows.Forms.Button btnCloneDummy;
        private System.Windows.Forms.LinkLabel lblHome;
    }
}

