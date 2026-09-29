using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Kingpool.Xgui;

using Kingpool.Core;
using Kingpool.Utility;
using Kingpool.Vision;

namespace Process
{
    public partial class FormMain : Form
    {
        IntPtr m_hContext = IntPtr.Zero;

        //the following strokes should never be destroyed manually
        //the strokes are used  for the Stroke page only to demonstrate
        //how to reuse or combine(uncombine) during painting on a window
        //for better performance
        //NOTE: if the Clear function in the other pages is revoked, 
        //      these stroke should never be used again.
        IntPtr m_hCurStroke = IntPtr.Zero;
        IntPtr m_hPictureStroke = IntPtr.Zero;
        IntPtr m_hRectangleStroke = IntPtr.Zero;
        IntPtr m_hMergeStroke = IntPtr.Zero;

#pragma warning disable CS0246 // 未能找到类型或命名空间名“KImage”(是否缺少 using 指令或程序集引用?)
        void ShowImagesInCanvas(KImage image)
#pragma warning restore CS0246 // 未能找到类型或命名空间名“KImage”(是否缺少 using 指令或程序集引用?)
        {
            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            Render.DrawImage(m_hContext, image.Handle, IntPtr.Zero);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }
        void ShowImagesInCanvas(KImage[] arr, int nOriginalIndex = 0)
        {
            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            int gap = 2;
            int x = 2, y = 0;
            int w = 100, h = 80;

            int canw = Render.GetCanvasWidth(m_hContext);
            int canh = Render.GetCanvasHeight(m_hContext);

            w = (canw - gap * 3) / 2;
            h = (canh - gap * 4) / 3;

            int c = 0;
            int row = 0, col = 0;
            foreach (KImage im in arr)
            {
                row = (c / 2);
                col = (c % 2);

                x = gap + col * (gap + w);
                y = gap + row * (gap + h);
                Render.DrawImageEx(m_hContext, im.Handle, x, y, w, h, IntPtr.Zero);

                if (c++ == 6) break;
            }

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        void ShowPyramidInCanvas(KImage[] arr)
        {
            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);


            foreach (KImage im in arr)
            {
                Render.DrawImageEx(m_hContext, im.Handle, 0, 0, -1, -1, IntPtr.Zero);

            }

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            m_hContext = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(155, 155, 155);
            Render.SetBackgroundColor(m_hContext, color);

            cmbOrient.SelectedIndex = 2;

            cmbChannel.SelectedIndex = 0;
            cmbMorphType.SelectedIndex = 0;

            cmbPixelWiseType.SelectedIndex = 0;

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hContext != IntPtr.Zero)
            {
                Render.DestroyContext(m_hContext);
                m_hContext = IntPtr.Zero;
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imRoot = img.Clone();

            img.Convert24To8(RgbToGray.Default);

            KImage imSim = img.Clone();
            Dip.Simple(imSim.Handle, 130, false);

            KImage imDark = img.Clone();
            Dip.Dark(imDark.Handle, 130, 255, true);

            KImage imLight = img.Clone();
            Dip.Light(imLight.Handle, 180, 255, true);

            KImage imInner = img.Clone();
            Dip.Inner(imInner.Handle, (byte)tkbLower.Value, (byte)tkbUpper.Value, 255, true);

            KImage imOuter = img.Clone();
            Dip.Outer(imOuter.Handle, (byte)tkbLower.Value, (byte)tkbUpper.Value, 255, true);

            KImage[] arr = new KImage[6] { imRoot, imSim, imDark, imLight, imInner, imOuter };
            ShowImagesInCanvas(arr, 0);


        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            //KImage imLocal = img.Clone();
            //Dip.LocalIntegral(imLocal.Handle, 10, 30, 0.42f);
            KImage imOtsu = img.Clone();
            Dip.Otsu(imOtsu.Handle, IntPtr.Zero);

            KImage imEntropy = img.Clone();
            Dip.MaxEntropy(imEntropy.Handle);


            KImage imError = img.Clone();
            Dip.MinError(imError.Handle, MinErrorMode.Gaussian);

            KImage[] arr = new KImage[4] { img, imOtsu, imEntropy, imError };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnBlur_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            int kernelSize = int.Parse(tbxKernelSize.Text);
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            KImage imBlur = img.Clone();

            Dip.Blur(imBlur.Handle, kernelSize, kernelSize);

            KImage imGauss = img.Clone();
            Dip.SmoothGaussian(imGauss.Handle, kernelSize, kernelSize);

            KImage imMedian = img.Clone();
            Dip.SmoothMedian(imMedian.Handle, kernelSize);

            KImage imAvg = img.Clone();
            Dip.SmoothAvg(imAvg.Handle, kernelSize);

            KImage[] arr = new KImage[4] { imBlur, imGauss, imMedian, imAvg };
            ShowImagesInCanvas(arr, 0);


        }


        private void btnTransform_Click(object sender, EventArgs e)
        {



        }

        private void btnDifference_Click(object sender, EventArgs e)
        {
            int idx = cmbOrient.SelectedIndex;
            RvDirection direct = RvDirection.Horizontal;
            if (idx == 1) direct = RvDirection.Vertical;
            else if (idx == 2) direct = RvDirection.Both;

            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            int gap = int.Parse(tbxGap.Text);
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            KImage imGrad = img.Clone();

            Dip.Gradient(imGrad.Handle, direct, gap);

            KImage imDiff1 = img.Clone();
            Dip.DiffFirst(imDiff1.Handle, direct);

            KImage imDiff2 = img.Clone();
            Dip.DiffSecond(imDiff2.Handle, direct);



            KImage[] arr = new KImage[3] { imGrad, imDiff1, imDiff2 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnGenericConvolution_Click(object sender, EventArgs e)
        {
            int idx = cmbOrient.SelectedIndex;
            RvDirection direct = RvDirection.Horizontal;
            if (idx == 1) direct = RvDirection.Vertical;
            else if (idx == 2) direct = RvDirection.Both;

            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            KImage imScharr = img.Clone();
            //使用Scharr滤波核对图像边缘进行增强;
            Dip.Scharr(imScharr.Handle, direct);

            KImage imPrewit = img.Clone();
            Dip.Prewitt(imPrewit.Handle, direct);

            KImage imSobel = img.Clone();
            Dip.Sobel(imSobel.Handle, direct);

            KImage imKirsch = img.Clone();
            Dip.Kirsch(imKirsch.Handle, direct);   // 修正：改为 imKirsch.Handle

            KImage imLap = img.Clone();
            Dip.Laplacian(imLap.Handle);

            KImage imRoberts = img.Clone();
            Dip.Roberts(imRoberts.Handle);

            KImage imS1541 = img.Clone();
            Dip.Sharp1541(imS1541.Handle);

            // 修正：数组包含全部 7 幅图像
            KImage[] arr = new KImage[7] { imScharr, imPrewit, imSobel, imKirsch, imLap, imRoberts, imS1541 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnDistanceTransform_Click(object sender, EventArgs e)
        {


        }

        private void btnFillHoles_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Invert(img.Handle);

            KImage imOtsu = img.Clone();
            Dip.Otsu(imOtsu.Handle, IntPtr.Zero);

            KImage imHoles = imOtsu.Clone();
            Dip.FillHole(imHoles.Handle);

            KImage imHoles2 = imOtsu.Clone();
            KMask msk = new KMask(MaskShape.FilledEllipse, imHoles2.GetWidth(), imHoles2.GetHeight());
            Dip.FillHoleEx(imHoles2.Handle, msk.Handle);

            KImage[] arr = new KImage[3] { imOtsu, imHoles, imHoles2 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnFillRectangle_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            //填充矩形;
            RvRect rect;

            rect.top = 30;
            rect.left = 30;
            rect.bottom = rect.top + 200;
            rect.right = rect.left + 200;

            uint color = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            Dip.FillRect(img.Handle, rect, color, !ckbFillOut.Checked);



            ShowImagesInCanvas(img);

        }

        private void btnFillPixel2_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            Byte gray = lblFillColor.BackColor.R;
            int chn = Dip.COLOR_CH1;
            if (cmbChannel.SelectedIndex == 1)
            {
                gray = lblFillColor.BackColor.G;
                chn = Dip.COLOR_CH2;
            }
            else if (cmbChannel.SelectedIndex == 2)
            {
                gray = lblFillColor.BackColor.B;
                chn = Dip.COLOR_CH3;
            }

            Dip.FillEx(img.Handle, chn, gray);
            ShowImagesInCanvas(img);


        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();


            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imScale1 = img.Clone();
            Dip.Scale(imScale1.Handle, (float)tkbScale.Value / 100.0f);

            KImage imScale2 = new KImage(img.GetPixelFormat(), 100, 200);
            Dip.ScaleEx(img.Handle, imScale2.Handle);

            KImage imScale3 = img.Clone();
            Dip.ScaleE1(imScale3.Handle, (float)0.4f, Pool.RGB(253, 0, 34), PixelFilterType.Bilinear, true);


            KImage[] arr = new KImage[4] { img, imScale1, imScale2, imScale3 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnRotate_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imRot = img.Clone();
            //rotate with n times of 90 degrees angle 
            Dip.Rotate(imRot.Handle, 1);

            KImage imRot1 = img.Clone();
            Dip.RotateEx(imRot1.Handle, (double)tkbAngle.Value, true, false);

            KImage imRot2 = img.Clone();
            Dip.RotateE2(imRot2.Handle, (double)tkbAngle.Value, Pool.RGB(255, 0, 0), true, false);

            KImage[] arr = new KImage[4] { img, imRot, imRot1, imRot2 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {


            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imTrans = img.Clone();
            Dip.Translate(imTrans.Handle, float.Parse(txbOffsetX.Text), float.Parse(txbOffsetY.Text), false);

            KImage imTrans1 = img.Clone();
            Dip.TranslateEx(imTrans1.Handle, float.Parse(txbOffsetX.Text), float.Parse(txbOffsetY.Text), false, Pool.RGB(255, 0, 0));

            KImage[] arr = new KImage[3] { img, imTrans, imTrans1 };
            ShowImagesInCanvas(arr, 0);


        }


        private void btnLinearTrans_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            int gap = int.Parse(tbxGap.Text);
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            KImage imLinear = img.Clone();
            Dip.Linear(imLinear.Handle, float.Parse(tbxGain.Text), (float.Parse(tbxOffset.Text)));

            KImage imCont = img.Clone();
            Dip.Contrast(imCont.Handle, (float)tkbLowPercent.Value / 100.0f, (float)tkbUpperPercent.Value / 100.0f);

            KImage[] arr = new KImage[2] { imLinear, imCont };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnMedian_Click(object sender, EventArgs e)
        {
            //imSrc2 = rviDuplicate(imSrc1);
            //if (imSrc2 != NULL)
            //{
            //    //对图像进行中值滤波; kernelsize 为3,5,7,9; 图象格式为灰度，或RGB图象;
            //    imSrc2 = rvMedian(imSrc2, 5);
            //    m_cw2.Show(imSrc2);
            //}
        }

        private void btnMorphology_Click(object sender, EventArgs e)
        {
            //rviCast(imSrc1, RIT_GRAY);
            //rvbMinError(imSrc1, RV_ME_GAUSSIAN);


            //imSrc2 = rviDuplicate(imSrc1);
            //if (imSrc2 != NULL)
            //{
            //    //二值图形态操作;
            //    imSrc2 = rvMorphology(imSrc2, RV_MT_ERODE);
            //    m_cw.Show(imSrc2);
            //}

            //imSrc3 = rviDuplicate(imSrc1);
            //if (imSrc3 != NULL)
            //{
            //    //二值图形态操作;
            //    imSrc3 = rvMorphology(imSrc3, RV_MT_CLOSE);
            //    m_cw1.Show(imSrc3);
            //}

            //imSrc4 = rviDuplicate(imSrc1);
            //if (imSrc4 != NULL)
            //{
            //    //二值图形态操作;
            //    imSrc4 = rvMorphology(imSrc4, RV_MT_GRADIENT);
            //    m_cw2.Show(imSrc4);
            //}

            //imSrc5 = rviDuplicate(imSrc1);
            //if (imSrc5 != NULL)
            //{
            //    //二值图形态操作;
            //    imSrc5 = rvMorphology(imSrc5, RV_MT_TOPHAT);
            //    m_cw3.Show(imSrc5);
            //}

        }


        private void btnCopyPixels_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            int gap = int.Parse(tbxGap.Text);
            bool b = img.Load(strFile);
            Pool.Assert(b);



            KImage imSub1 = new KImage(img.GetPixelFormat(), 100, 100);

            Dip.CopyPixels(img.Handle, 80, 20, -1, -1, imSub1.Handle);

            KImage imSub2 = new KImage(img.GetPixelFormat(), 120, 120);
            KMask msk = new KMask(MaskShape.FilledEllipse, 120, 120);
            Dip.CopyPixelsE2(img.Handle, imSub2.Handle, msk.Handle);

            KImage imSub3 = new KImage(img.GetPixelFormat(), img.GetWidth(), img.GetHeight());

            RvPointF32[] pntVertex = new RvPointF32[4]  {
               new RvPointF32(   0, 0),
                new RvPointF32(  200, 0),
                new RvPointF32(   200, 250),
               new RvPointF32(    0, 250),
            };
            Dip.CopyPixelsE3(img.Handle, imSub3.Handle, pntVertex);

            KImage[] arr = new KImage[4] { img, imSub1, imSub2, imSub3 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnPixelOperate_Click(object sender, EventArgs e)
        {


            KImage img1 = new KImage("..\\samples\\colorwave.jpg");
            KImage img2 = new KImage("..\\samples\\waterdrop.png");

            img1.Cast(PixelFormat.BGR);
            img2.Cast(PixelFormat.BGR);

            KImage imgResult = img1.Clone();

            Dip.PixelMerge(img2.Handle, imgResult.Handle, (PixelOperator)cmbPixelWiseType.SelectedIndex);

            KImage[] arr = new KImage[3] { img1, img2, imgResult };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnPyramid_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.BGR);

            KImage im1 = new KImage(Dip.Pyramid(img.Handle, false, IntPtr.Zero), false);

            KImage im2 = new KImage(Dip.Pyramid(im1.Handle, false, IntPtr.Zero), false);

            KImage im3 = new KImage(Dip.Pyramid(im2.Handle, false, IntPtr.Zero), false);

            KImage im4 = new KImage(Dip.Pyramid(img.Handle, true, IntPtr.Zero), false);

            KImage[] arr = new KImage[5] { im4, img, im1, im2, im3 };
            ShowPyramidInCanvas(arr);

        }

        private void btnRemoveBorder_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Invert(img.Handle);

            KImage imOtsu = img.Clone();
            Dip.Otsu(imOtsu.Handle, IntPtr.Zero);

            KImage imBorder = imOtsu.Clone();
            Dip.RemoveBorder(imBorder.Handle);

            KImage imBorder2 = imOtsu.Clone();
            KMask msk = new KMask(MaskShape.FilledEllipse, imBorder2.GetWidth(), imBorder2.GetHeight());
            Dip.RemoveBorderEx(imBorder2.Handle, msk.Handle);

            KImage[] arr = new KImage[3] { imOtsu, imBorder, imBorder2 };
            ShowImagesInCanvas(arr, 0);


        }

        private void btnThinning_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Invert(img.Handle);

            KImage imOtsu = img.Clone();
            Dip.Otsu(imOtsu.Handle, IntPtr.Zero);

            KImage imSkeleton = imOtsu.Clone();
            Dip.Skeleton(imSkeleton.Handle);

            KImage imThin = imOtsu.Clone();
            Dip.Thinning(imThin.Handle);

            KImage[] arr = new KImage[3] { imOtsu, imSkeleton, imThin };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnWarp_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.BGR);

            RvPointF32[] pnSrc = new RvPointF32[4]  {
                 new RvPointF32(40, 35),
                 new RvPointF32( 289, 112),
                 new RvPointF32(292, 273),
                 new RvPointF32 (56, 263)
            };


            int w = 120, h = 200;
            KImage imWarp = new KImage(img.GetPixelFormat(), 120, 200);

            RvPointF32[] pnDest = new RvPointF32[4]{
                new RvPointF32( 0,0),
                new RvPointF32( w -1, 0),
                new RvPointF32( w -1, h-1),
                new RvPointF32( 0, h-1)
            };

            Dip.Warp(img, pnSrc, imWarp);



            KImage[] arr = new KImage[2] { img, imWarp };
            ShowImagesInCanvas(arr, 0);


        }

        private void btnAdaptive_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Invert(img.Handle);

            int ksz = int.Parse(txbKernelSizeBin.Text);

            KImage imAda1 = img.Clone();
            Dip.Adaptive(imAda1.Handle, AdaptiveBinarize.Mean, ksz, 0, 0);

            KImage imAda2 = img.Clone();
            Dip.Adaptive(imAda2.Handle, AdaptiveBinarize.Gaussian, ksz, 5, 0);

            KImage imAda3 = img.Clone();
            Dip.Adaptive(imAda3.Handle, AdaptiveBinarize.LocalInteger, ksz, 30, 0.65);

            KImage imAda4 = img.Clone();
            Dip.Adaptive(imAda4.Handle, AdaptiveBinarize.IsoData, ksz, 5, 0);

            KImage[] arr = new KImage[5] { img, imAda1, imAda2, imAda3, imAda4 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnFillEllipse_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            uint color = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            Dip.SetEllipse(img.Handle, 220, 220, 160, 60, 45.0f, color);


            ShowImagesInCanvas(img);


        }

        private void btnFillPolygon_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            uint color = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            RvPoint[] pnVertex3 = new RvPoint[5] {
                    new RvPoint(0+77, 0+77),
                    new RvPoint(200+77, 0+77),
                    new RvPoint(250+77, 200+77),
                    new RvPoint(250+77, 300+77),
                    new RvPoint(0+77, 300+77)
                };

            Dip.FillPolygonE2(img.Handle, pnVertex3, color, ckbFillOut.Checked);

            ShowImagesInCanvas(img);

        }

        private void btnFillLine_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            uint color = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            RvPoint[] vteArr = new RvPoint[5]{
                new RvPoint(10, 10),
                new RvPoint(200, 10),
                new RvPoint(250, 200),
                new RvPoint(250, 300),
                new RvPoint(10, 300)
            };

            //Smath.MovePolyline(vteArr, 66, 66);

            Dip.SetPolyline(img.Handle, vteArr, color);

            Dip.SetLine(img.Handle, new RvPoint(150, 360), new RvPoint(280, 350), color);


            ShowImagesInCanvas(img);
        }

        private void lblFillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblFillColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnFloodFill_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);


            RvPoint pnPos1 = new RvPoint(100, 100);

            RvScalarF64 lower = new RvScalarF64() { val = new double[4] };
            lower.val[0] = 20.0;
            lower.val[1] = 20.0;
            lower.val[2] = 10;
            lower.val[3] = 0.0;

            RvScalarF64 upper = new RvScalarF64() { val = new double[4] };
            upper.val[0] = 22.0;
            upper.val[1] = 22.0;
            upper.val[2] = 10;
            upper.val[3] = 0.0;

            RvRgb colorFill1;
            colorFill1.red = lblFillColor.BackColor.R;
            colorFill1.green = lblFillColor.BackColor.G;
            colorFill1.blue = lblFillColor.BackColor.B;

            Dip.FloodFill(img.Handle, pnPos1, colorFill1, lower, upper, 8);

            ShowImagesInCanvas(img);



            //imSrc6 = rviDuplicate(imSrc1);
            //if (imSrc6 != NULL)
            //{
            //    //二值图像四周的若干像素设置为RV_BIN_ZERO;
            //    rvCutMargin(imSrc6, 30);
            //    m_cw4.Show(imSrc6);
            //}
        }

        private void btnFloodFill2_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\Earth.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            RvRgb colorFill1;
            colorFill1.red = lblFillColor.BackColor.R;
            colorFill1.green = lblFillColor.BackColor.G;
            colorFill1.blue = lblFillColor.BackColor.B;

            Dip.Fill(img.Handle, 45, 45, Pool.RGB_TO_COLOR(colorFill1));

            ShowImagesInCanvas(img);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            uint color = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            Dip.CutMargin(img.Handle, 4, color);

            ShowImagesInCanvas(img);
        }

        private void btnFillGradient_Click(object sender, EventArgs e)
        {
            KImage img = new KImage(PixelFormat.BGR, 120, 80);

            RvRgb start = new RvRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);
            RvRgb end = new RvRgb(lblFillColor.BackColor.B, lblFillColor.BackColor.G, lblFillColor.BackColor.R);


            Dip.FillGradient(img.Handle, start, end, GradientDirection.Horizontal);
            ShowImagesInCanvas(img);
        }

        private void btnMorphology_Click_1(object sender, EventArgs e)
        {


            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();
            int kernelSize = int.Parse(tbxKernelSize.Text);

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Morphology(img.Handle, (MorphologyType)cmbMorphType.SelectedIndex, kernelSize);

            ShowImagesInCanvas(img);
        }

        private void btnCanny_Click_1(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();
            int kernelSize = int.Parse(tbxKernelSize.Text);

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            Dip.Canny(img.Handle, 50, 150, true, kernelSize);

            ShowImagesInCanvas(img);


        }

        private void btnFillText_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            string str = "This is Kingpool world";

            uint foreColor = Pool.RGB(lblTextColor.BackColor.R, lblTextColor.BackColor.G, lblTextColor.BackColor.B);
            uint backColor = Pool.RGB(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);
            int style = 0;

            if (ckbBold.Checked) style |= (int)FillTextStyle.Bold;
            if (ckbItalic.Checked) style |= (int)FillTextStyle.Italic;
            if (ckbUnderline.Checked) style |= (int)FillTextStyle.Underline;
            if (ckbTransparent.Checked) style |= (int)FillTextStyle.Transparent;

            Dip.FillTextEx(img.Handle, str, 10, 10, "Arial", 16, foreColor, backColor, style);

            ShowImagesInCanvas(img);
        }

        private void lblTextColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblTextColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnNormalize_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            int gap = int.Parse(tbxGap.Text);
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);

            KImage imNorm = img.Clone();
            Dip.Normalize(imNorm.Handle, (float)tkbAverage.Value, (float)tkbVariance.Value);


            KImage imEqual = img.Clone();
            Dip.Equalize(imEqual.Handle);

            KImage imExp = img.Clone();
            Dip.Expand(imExp.Handle);



            KImage[] arr = new KImage[3] { imNorm, imEqual, imExp };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnHysteresis_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Convert24To8(RgbToGray.Default);
            Dip.Invert(img.Handle);

            Dip.Hysteresis(img.Handle, (int)tkbLower.Value, (int)tkbUpper.Value, 3);

            ShowImagesInCanvas(img);
        }


        private void btnFlip_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\waterdrop.png";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.BGR);

            KImage im1 = img.Clone();
            Dip.Flip(im1.Handle, RvDirection.Horizontal);

            KImage im2 = img.Clone();
            Dip.Flip(im2.Handle, RvDirection.Vertical);

            KImage im3 = img.Clone();
            Dip.Flip(im3.Handle, RvDirection.Both);


            KImage[] arr = new KImage[4] { img, im1, im2, im3 };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnColorFusion_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imFuse1 = img.Clone();
            Dip.FuseGradients(imFuse1.Handle, ColorFusionType.Salience, 0);

            KImage imFuse2 = img.Clone();
            Dip.FuseGradients(imFuse2.Handle, ColorFusionType.Voting, (float)tkbEdgeThres.Value);

            KImage imFuse3 = img.Clone();
            Dip.FuseGradients(imFuse3.Handle, ColorFusionType.Direction, (float)tkbAngleThres.Value);

            KImage[] arr = new KImage[4] { img, imFuse1, imFuse2, imFuse3 };
            ShowImagesInCanvas(arr, 0);

        }

        private void btnSetInMask_Click(object sender, EventArgs e)
        {
            KImage img = new KImage(PixelFormat.BGR, 240, 160);

            Dip.Fill(img.Handle, 10, 10, Pool.RGB((byte)0, (byte)155, (byte)0));


            KImage imMask = img.Clone();
            KMask msk = new KMask(MaskShape.HorizontalStrap, imMask.GetWidth(), imMask.GetHeight(), 22);

            Dip.SetPixelEx(imMask.Handle, Pool.RGB((byte)255, (byte)155, (byte)123), msk.Handle, false);

            KImage[] arr = new KImage[2] { img, imMask };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnSetPixel_Click(object sender, EventArgs e)
        {
            KImage img = new KImage(PixelFormat.BGR, 240, 160);

            KImage imRand = img.Clone();

            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    //在指定位置更改像素颜色;
                    Dip.SetPixel(imRand.Handle, 20 + j, 20 + i, Pool.RGB((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255)));

                }
            }

            KImage imPosArr = img.Clone();
            RvPoint[] pnArr = new RvPoint[10000];

            int c = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    pnArr[c].x = i + 50;
                    pnArr[c].y = j + 50;
                    c++;
                }
            }
            //在指定位置更改像素颜色;
            Dip.SetPixelE1(imPosArr.Handle, pnArr, Pool.RGB((byte)255, (byte)55, (byte)123));

            KImage[] arr = new KImage[3] { img, imRand, imPosArr };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnInvert_Click_1(object sender, EventArgs e)
        {
            KImage img = new KImage(PixelFormat.BGR, 240, 160);

            img.Flood(Pool.RGB(124, 0, 235));
            KImage imInvert = img.Clone();


            Dip.Invert(imInvert.Handle);

            KImage[] arr = new KImage[2] { img, imInvert };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnClip_Click_1(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();


            bool b = img.Load(strFile);
            Pool.Assert(b);

            KImage imClip = Dip.Clip(img, 50, 50, 100, 100);

            KImage[] arr = new KImage[2] { img, imClip };
            ShowImagesInCanvas(arr, 0);
        }

        private void btnDistanceTransform_Click_1(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();

            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);
            Dip.Invert(img.Handle);
            Dip.MaxEntropy(img.Handle);

            IntPtr h = Dip.DistTransE1(img.Handle, (int)tkbDistance.Value);
            KImage imDt = new KImage(h, false);

            KImage[] arr = new KImage[2] { img, imDt };
            ShowImagesInCanvas(arr, 0);
        }
    }
}


