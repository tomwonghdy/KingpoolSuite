
namespace SmartMath
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnArc3p = new System.Windows.Forms.Button();
            this.btnRotateLine = new System.Windows.Forms.Button();
            this.btnScaleLine = new System.Windows.Forms.Button();
            this.btnExtendTrim = new System.Windows.Forms.Button();
            this.btnRectangle = new System.Windows.Forms.Button();
            this.btnPointInPolygon = new System.Windows.Forms.Button();
            this.btnOffsetPolygon = new System.Windows.Forms.Button();
            this.btnIntersectAndParallel = new System.Windows.Forms.Button();
            this.btnRectAndVertex = new System.Windows.Forms.Button();
            this.btnCaluation = new System.Windows.Forms.Button();
            this.btnCalcAreaLengthAngle = new System.Windows.Forms.Button();
            this.btnCalcDistance = new System.Windows.Forms.Button();
            this.btnGeomExtract = new System.Windows.Forms.Button();
            this.btnPixelStrength = new System.Windows.Forms.Button();
            this.xguiPanel1 = new SmartMath.XguiPanel();
            this.btnAdaptRect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnArc3p
            // 
            this.btnArc3p.Location = new System.Drawing.Point(28, 32);
            this.btnArc3p.Name = "btnArc3p";
            this.btnArc3p.Size = new System.Drawing.Size(119, 43);
            this.btnArc3p.TabIndex = 20;
            this.btnArc3p.Text = "3P Arc";
            this.btnArc3p.UseVisualStyleBackColor = true;
            this.btnArc3p.Click += new System.EventHandler(this.btnArc3p_Click);
            // 
            // btnRotateLine
            // 
            this.btnRotateLine.Location = new System.Drawing.Point(196, 32);
            this.btnRotateLine.Name = "btnRotateLine";
            this.btnRotateLine.Size = new System.Drawing.Size(119, 43);
            this.btnRotateLine.TabIndex = 21;
            this.btnRotateLine.Text = "Rotate Line";
            this.btnRotateLine.UseVisualStyleBackColor = true;
            this.btnRotateLine.Click += new System.EventHandler(this.btnRotateLine_Click);
            // 
            // btnScaleLine
            // 
            this.btnScaleLine.Location = new System.Drawing.Point(28, 84);
            this.btnScaleLine.Name = "btnScaleLine";
            this.btnScaleLine.Size = new System.Drawing.Size(119, 43);
            this.btnScaleLine.TabIndex = 22;
            this.btnScaleLine.Text = "Scale Line";
            this.btnScaleLine.UseVisualStyleBackColor = true;
            this.btnScaleLine.Click += new System.EventHandler(this.btnScaleLine_Click);
            // 
            // btnExtendTrim
            // 
            this.btnExtendTrim.Location = new System.Drawing.Point(196, 84);
            this.btnExtendTrim.Name = "btnExtendTrim";
            this.btnExtendTrim.Size = new System.Drawing.Size(119, 43);
            this.btnExtendTrim.TabIndex = 23;
            this.btnExtendTrim.Text = "Extend / Trim";
            this.btnExtendTrim.UseVisualStyleBackColor = true;
            this.btnExtendTrim.Click += new System.EventHandler(this.btnExtendTrim_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Location = new System.Drawing.Point(196, 136);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(119, 43);
            this.btnRectangle.TabIndex = 25;
            this.btnRectangle.Text = "Rectangle";
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // btnPointInPolygon
            // 
            this.btnPointInPolygon.Location = new System.Drawing.Point(28, 188);
            this.btnPointInPolygon.Name = "btnPointInPolygon";
            this.btnPointInPolygon.Size = new System.Drawing.Size(119, 43);
            this.btnPointInPolygon.TabIndex = 26;
            this.btnPointInPolygon.Text = "Point In Polygon";
            this.btnPointInPolygon.UseVisualStyleBackColor = true;
            this.btnPointInPolygon.Click += new System.EventHandler(this.btnPointInPolygon_Click);
            // 
            // btnOffsetPolygon
            // 
            this.btnOffsetPolygon.Location = new System.Drawing.Point(28, 240);
            this.btnOffsetPolygon.Name = "btnOffsetPolygon";
            this.btnOffsetPolygon.Size = new System.Drawing.Size(119, 43);
            this.btnOffsetPolygon.TabIndex = 28;
            this.btnOffsetPolygon.Text = "Offset Polygon";
            this.btnOffsetPolygon.UseVisualStyleBackColor = true;
            this.btnOffsetPolygon.Click += new System.EventHandler(this.btnOffsetPolygon_Click);
            // 
            // btnIntersectAndParallel
            // 
            this.btnIntersectAndParallel.Location = new System.Drawing.Point(196, 188);
            this.btnIntersectAndParallel.Name = "btnIntersectAndParallel";
            this.btnIntersectAndParallel.Size = new System.Drawing.Size(119, 43);
            this.btnIntersectAndParallel.TabIndex = 31;
            this.btnIntersectAndParallel.Text = "Perpendicular / Parallel";
            this.btnIntersectAndParallel.UseVisualStyleBackColor = true;
            this.btnIntersectAndParallel.Click += new System.EventHandler(this.btnIntersectAndParallel_Click);
            // 
            // btnRectAndVertex
            // 
            this.btnRectAndVertex.Location = new System.Drawing.Point(28, 136);
            this.btnRectAndVertex.Name = "btnRectAndVertex";
            this.btnRectAndVertex.Size = new System.Drawing.Size(119, 43);
            this.btnRectAndVertex.TabIndex = 32;
            this.btnRectAndVertex.Text = "Bound Rectangle Box2D Vertex";
            this.btnRectAndVertex.UseVisualStyleBackColor = true;
            this.btnRectAndVertex.Click += new System.EventHandler(this.btnRectAndVertex_Click);
            // 
            // btnCaluation
            // 
            this.btnCaluation.Location = new System.Drawing.Point(196, 240);
            this.btnCaluation.Name = "btnCaluation";
            this.btnCaluation.Size = new System.Drawing.Size(119, 43);
            this.btnCaluation.TabIndex = 33;
            this.btnCaluation.Text = "Calculation Orientation";
            this.btnCaluation.UseVisualStyleBackColor = true;
            this.btnCaluation.Click += new System.EventHandler(this.btnCaluationOrientation_Click);
            // 
            // btnCalcAreaLengthAngle
            // 
            this.btnCalcAreaLengthAngle.Location = new System.Drawing.Point(28, 292);
            this.btnCalcAreaLengthAngle.Name = "btnCalcAreaLengthAngle";
            this.btnCalcAreaLengthAngle.Size = new System.Drawing.Size(119, 43);
            this.btnCalcAreaLengthAngle.TabIndex = 34;
            this.btnCalcAreaLengthAngle.Text = "Calulation Area/Length/Angle";
            this.btnCalcAreaLengthAngle.UseVisualStyleBackColor = true;
            this.btnCalcAreaLengthAngle.Click += new System.EventHandler(this.btnCalcAreaLengthAngle_Click);
            // 
            // btnCalcDistance
            // 
            this.btnCalcDistance.Location = new System.Drawing.Point(196, 292);
            this.btnCalcDistance.Name = "btnCalcDistance";
            this.btnCalcDistance.Size = new System.Drawing.Size(119, 43);
            this.btnCalcDistance.TabIndex = 35;
            this.btnCalcDistance.Text = "Calculation Distance";
            this.btnCalcDistance.UseVisualStyleBackColor = true;
            this.btnCalcDistance.Click += new System.EventHandler(this.btnCalcDistance_Click);
            // 
            // btnGeomExtract
            // 
            this.btnGeomExtract.Location = new System.Drawing.Point(28, 344);
            this.btnGeomExtract.Name = "btnGeomExtract";
            this.btnGeomExtract.Size = new System.Drawing.Size(119, 43);
            this.btnGeomExtract.TabIndex = 36;
            this.btnGeomExtract.Text = "Geometry Extraction";
            this.btnGeomExtract.UseVisualStyleBackColor = true;
            this.btnGeomExtract.Click += new System.EventHandler(this.btnGeomExtract_Click);
            // 
            // btnPixelStrength
            // 
            this.btnPixelStrength.Location = new System.Drawing.Point(196, 344);
            this.btnPixelStrength.Name = "btnPixelStrength";
            this.btnPixelStrength.Size = new System.Drawing.Size(119, 43);
            this.btnPixelStrength.TabIndex = 37;
            this.btnPixelStrength.Text = "Pixel Strength";
            this.btnPixelStrength.UseVisualStyleBackColor = true;
            this.btnPixelStrength.Click += new System.EventHandler(this.btnPixelStrength_Click);
            // 
            // xguiPanel1
            // 
            this.xguiPanel1.BackColor = System.Drawing.Color.White;
            this.xguiPanel1.Location = new System.Drawing.Point(401, 12);
            this.xguiPanel1.Name = "xguiPanel1";
            this.xguiPanel1.Size = new System.Drawing.Size(596, 522);
            this.xguiPanel1.TabIndex = 19;
            this.xguiPanel1.Text = "xguiPanel1";
            // 
            // btnAdaptRect
            // 
            this.btnAdaptRect.Location = new System.Drawing.Point(28, 396);
            this.btnAdaptRect.Name = "btnAdaptRect";
            this.btnAdaptRect.Size = new System.Drawing.Size(119, 43);
            this.btnAdaptRect.TabIndex = 38;
            this.btnAdaptRect.Text = "Adapt Rect";
            this.btnAdaptRect.UseVisualStyleBackColor = true;
            this.btnAdaptRect.Click += new System.EventHandler(this.btnAdaptRect_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 565);
            this.Controls.Add(this.btnAdaptRect);
            this.Controls.Add(this.btnPixelStrength);
            this.Controls.Add(this.btnGeomExtract);
            this.Controls.Add(this.btnCalcDistance);
            this.Controls.Add(this.btnCalcAreaLengthAngle);
            this.Controls.Add(this.btnCaluation);
            this.Controls.Add(this.btnRectAndVertex);
            this.Controls.Add(this.btnIntersectAndParallel);
            this.Controls.Add(this.btnOffsetPolygon);
            this.Controls.Add(this.btnPointInPolygon);
            this.Controls.Add(this.btnRectangle);
            this.Controls.Add(this.btnExtendTrim);
            this.Controls.Add(this.btnScaleLine);
            this.Controls.Add(this.btnRotateLine);
            this.Controls.Add(this.btnArc3p);
            this.Controls.Add(this.xguiPanel1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Mathematics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private XguiPanel xguiPanel1;
        private System.Windows.Forms.Button btnArc3p;
        private System.Windows.Forms.Button btnRotateLine;
        private System.Windows.Forms.Button btnScaleLine;
        private System.Windows.Forms.Button btnExtendTrim;
        private System.Windows.Forms.Button btnRectangle;
        private System.Windows.Forms.Button btnPointInPolygon;
        private System.Windows.Forms.Button btnOffsetPolygon;
        private System.Windows.Forms.Button btnIntersectAndParallel;
        private System.Windows.Forms.Button btnRectAndVertex;
        private System.Windows.Forms.Button btnCaluation;
        private System.Windows.Forms.Button btnCalcAreaLengthAngle;
        private System.Windows.Forms.Button btnCalcDistance;
        private System.Windows.Forms.Button btnGeomExtract;
        private System.Windows.Forms.Button btnPixelStrength;
        private System.Windows.Forms.Button btnAdaptRect;
    }
}

