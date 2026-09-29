
namespace MaskBasics
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.picPreview1 = new System.Windows.Forms.PictureBox();
            this.btnDeriveMask = new System.Windows.Forms.Button();
            this.picPreview2 = new System.Windows.Forms.PictureBox();
            this.btnDeriveImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(33, 22);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(169, 34);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreateFill_Click);
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(33, 67);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(169, 34);
            this.btnClone.TabIndex = 4;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(33, 157);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(169, 34);
            this.btnProperties.TabIndex = 5;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnResize
            // 
            this.btnResize.Location = new System.Drawing.Point(33, 112);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(169, 34);
            this.btnResize.TabIndex = 9;
            this.btnResize.Text = "Resize";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(33, 202);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(169, 34);
            this.btnMerge.TabIndex = 12;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // picPreview1
            // 
            this.picPreview1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picPreview1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picPreview1.Location = new System.Drawing.Point(248, 12);
            this.picPreview1.Name = "picPreview1";
            this.picPreview1.Size = new System.Drawing.Size(331, 342);
            this.picPreview1.TabIndex = 15;
            this.picPreview1.TabStop = false;
            // 
            // btnDeriveMask
            // 
            this.btnDeriveMask.Location = new System.Drawing.Point(33, 247);
            this.btnDeriveMask.Name = "btnDeriveMask";
            this.btnDeriveMask.Size = new System.Drawing.Size(169, 34);
            this.btnDeriveMask.TabIndex = 16;
            this.btnDeriveMask.Text = "Image To Mask";
            this.btnDeriveMask.UseVisualStyleBackColor = true;
            this.btnDeriveMask.Click += new System.EventHandler(this.btnDeriveMask_Click);
            // 
            // picPreview2
            // 
            this.picPreview2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picPreview2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picPreview2.Location = new System.Drawing.Point(600, 12);
            this.picPreview2.Name = "picPreview2";
            this.picPreview2.Size = new System.Drawing.Size(331, 342);
            this.picPreview2.TabIndex = 17;
            this.picPreview2.TabStop = false;
            // 
            // btnDeriveImage
            // 
            this.btnDeriveImage.Location = new System.Drawing.Point(33, 292);
            this.btnDeriveImage.Name = "btnDeriveImage";
            this.btnDeriveImage.Size = new System.Drawing.Size(169, 34);
            this.btnDeriveImage.TabIndex = 18;
            this.btnDeriveImage.Text = "Mask To Image";
            this.btnDeriveImage.UseVisualStyleBackColor = true;
            this.btnDeriveImage.Click += new System.EventHandler(this.btnDeriveImage_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 435);
            this.Controls.Add(this.btnDeriveImage);
            this.Controls.Add(this.picPreview2);
            this.Controls.Add(this.btnDeriveMask);
            this.Controls.Add(this.picPreview1);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnResize);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.btnCreate);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mask Basics";
            ((System.ComponentModel.ISupportInitialize)(this.picPreview1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.PictureBox picPreview1;
        private System.Windows.Forms.Button btnDeriveMask;
        private System.Windows.Forms.PictureBox picPreview2;
        private System.Windows.Forms.Button btnDeriveImage;
    }
}

