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


using Kingpool.Core;
using Kingpool.Utility;
using Kingpool.Vision;
using Kingpool.Xgui;

namespace Analize
{
    public partial class FormMain : Form
    {

        IntPtr m_hContext = IntPtr.Zero;
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnBinaryFeature_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\triangle.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);
            Dip.MinError(img.Handle, MinErrorMode.Poisson);


            string s = "";
            s += $"The Density is {Dia.Density(img.Handle, (int)BlobPart.Whole)} \r\n";
            s += $"The Circularity is {Dia.Circular(img.Handle, IntPtr.Zero)} \r\n";
            s += $"The Slope is {Dia.Slope(img.Handle, IntPtr.Zero)} \r\n";
            s += $"The Centoid is {Dia.Centroid(img.Handle, IntPtr.Zero).ToString()} \r\n";
            RvBox2D box = Dia.GetBoundBox(img.Handle, IntPtr.Zero);
            s += $"The Box2d: cx={box.cx}, cy={box.cy}, angle={box.angle}, width = {box.width}, height={box.height} \r\n";
            s += $"The Bound Rect is {Dia.GetBoundRect(img.Handle, IntPtr.Zero).ToString()} \r\n";

            tbxOutput.Text = s;
        }

        private void btnCarity_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\colorwave.jpg";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);

            KMask mask = CreateMask();

            double n = Dia.GetClearness(img.Handle, mask.Handle);
            string s = "";
            s += $"Original sharpness is  {n} \r\n";

            Dip.Blur(img.Handle, 5, 5);
            n = Dia.GetClearness(img.Handle, mask.Handle);

            s += $"The sharpness after blurring is  {n} \r\n";


            Dip.Blur(img.Handle, 3, 3);
            n = Dia.GetClearness(img.Handle, mask.Handle);

            s += $"The sharpness after 2 times of blurring is  {n} \r\n";

            tbxOutput.Text = s;

        }

        void GetMaxValue(Int64[] redArr, Int64[] greenArr, Int64[] blueArr, out Int64 maxValue, out Int64 minValue)
        {
            maxValue = 0;
            minValue = 0x7FFFFFFF;

            if (redArr != null)
            {
                foreach (Int64 n in redArr)
                {
                    if (n > maxValue) maxValue = n;
                    if (n < minValue) minValue = n;
                }
            }

            if (greenArr != null)
            {
                foreach (Int64 n in greenArr)
                {
                    if (n > maxValue) maxValue = n;
                    if (n < minValue) minValue = n;
                }
            }

            if (blueArr != null)
            {
                foreach (Int64 n in blueArr)
                {
                    if (n > maxValue) maxValue = n;
                    if (n < minValue) minValue = n;
                }
            }
        }

        RvPoint[] GetPointInCanvas(int canw, int canh, Int64 maxv, Int64 minv, Int64[] arrIn)
        {
            if (arrIn == null) return null;


            RvPoint[] arr = new RvPoint[arrIn.Length];
            for (int i = 0; i < arrIn.Length; i++)
            {
                arr[i].x = (int)(i * canw * 1.0 / arrIn.Length);
                arr[i].y = (int)((canh - 1) - (arrIn[i] - minv) * 1.0 / (maxv - minv) * canh);
            }

            return arr;
        }

        void DrawCurve(Int64[] redArr, Int64[] greenArr, Int64[] blueArr)
        {
            if (m_hContext == IntPtr.Zero) return;

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            int canw = Render.GetCanvasWidth(m_hContext);
            int canh = Render.GetCanvasHeight(m_hContext);
            Int64 minv, maxv;
            GetMaxValue(redArr, greenArr, blueArr, out maxv, out minv);

            if (minv == maxv) return;

            GRgb rgb = new GRgb(255, 0, 0);

            IntPtr hPen = Render.CreatePen(rgb, 2, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //;;;
            if (redArr != null)
            {
                RvPoint[] arr = GetPointInCanvas(canw, canh, maxv, minv, redArr);

                Render.DrawPolyline(m_hContext, arr, IntPtr.Zero);
            }

            if (greenArr != null)
            {
                RvPoint[] arr = GetPointInCanvas(canw, canh, maxv, minv, greenArr);

                rgb = new GRgb(0, 255, 0);
                Render.SetPenColor(hPen, rgb);
                Render.DrawPolyline(m_hContext, arr, IntPtr.Zero);
            }

            if (blueArr != null)
            {
                RvPoint[] arr = GetPointInCanvas(canw, canh, maxv, minv, blueArr);

                rgb = new GRgb(0, 0, 255);
                Render.SetPenColor(hPen, rgb);

                Render.DrawPolyline(m_hContext, arr, IntPtr.Zero);
            }

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }
        private void btnHistogram_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\lenna.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.BGR);


            KMask mask = CreateMask();
            string s = "";

            KFdox dox = Dia.HistogramEx(img);

            if (dox != null)
            {
                int chns = img.GetChannels();
                Int64[] rarr = new Int64[256];
                Int64[] garr = new Int64[256];
                Int64[] barr = new Int64[256];

                for (int c = 0; c < chns; c++)
                {
                    KFdox sub = dox.GetChild(c);

                    s += $"Channel {c + 1}:\r\n";
                    for (int i = 0; i < sub.GetCount(); i++)
                    {
                        Pool.Assert(i < 256);

                        KFdox child = sub.GetChild(i);

                        Int64 n;
                        if (child.GetInt64(out n))
                        {
                            s += $"{n} ";
                            if (c == 0) barr[i] = n;
                            if (c == 1) garr[i] = n;
                            if (c == 2) rarr[i] = n;
                        }
                        else
                        {
                            s += "-1 ";
                        }
                    }
                    s += "\r\n";
                }

                DrawCurve(rarr, garr, barr);
            }

            tbxOutput.Text = s;


        }

        private void btnNonzeroPixels_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\triangle.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);
            Dip.MaxEntropy(img.Handle);

            long n = Dia.CountPixels(img.Handle, IntPtr.Zero);
            string s = "";
            s += $"The count of non-zero pixels is {n} \r\n";

            tbxOutput.Text = s;
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\lenna.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.BGR);

            string s = "";

            KFdox dox = Dia.ProjectEx(img, (int)RvDirection.Both, null);

            if (dox != null)
            {
                int chns = img.GetChannels();
                Int64[] rarr = null;
                Int64[] garr = null;
                Int64[] barr = null;

                for (int c = 0; c < chns; c++)
                {
                    KFdox sub = dox.GetChild(c);

                    int subcnt = sub.GetCount();
                    if (c == 0)
                        barr = new Int64[subcnt];
                    else if (c == 1)
                        garr = new Int64[subcnt];
                    else if (c == 2)
                        rarr = new Int64[subcnt];

                    s += $"Channel {c + 1}:\r\n";
                    for (int i = 0; i < sub.GetCount(); i++)
                    {

                        KFdox child = sub.GetChild(i);

                        Int64 n;
                        if (child.GetInt64(out n))
                        {
                            s += $"{n} ";
                            if (c == 0) barr[i] = n;
                            if (c == 1) garr[i] = n;
                            if (c == 2) rarr[i] = n;
                        }
                        else
                        {
                            s += "-1 ";
                        }
                    }
                    s += "\r\n";
                }

                DrawCurve(rarr, garr, barr);
            }

            tbxOutput.Text = s;
        }

        private void btnStrengthStatistics_Click(object sender, EventArgs e)
        {

            string strFile = "..\\samples\\lenna.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);

            KFdox dox = Dia.CalcGrayStatsEx(img);

            string s = "";
            double n = 0;

            dox.GetDoubleAt(0, out n);
            s += $"Summary of gray: {n}\r\n";

            dox.GetDoubleAt(1, out n);
            s += $"Average of gray: {n}\r\n";

            dox.GetDoubleAt(2, out n);
            s += $"Variance of gray: {n}\r\n";

            tbxOutput.Text = s;
        }

        private void btnContrastRange_Click(object sender, EventArgs e)
        {
            //Warning: The following functions may take a very long time depending
            //on the computer hardware, and may cause the program to appear frozen(hang).

            string strFile = "..\\samples\\lenna.png";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);

            KMask mask = CreateMask();



            string s = "";
            // double n = 0;

            float minContrast = 0;
            float maxContrast = 0;

            Dia.EstimateCannyContrast(img, mask, Dia.ECC_MEDINA, 0, ref minContrast, ref maxContrast);
            s += $"Canny constrast estimation by Medina: {minContrast}, {maxContrast}\r\n";

            Dia.EstimateCannyContrast(img, mask, Dia.ECC_ADAPTIVE, 0, ref minContrast, ref maxContrast);
            s += $"Canny constrast estimation by Adaptive: {minContrast}, {maxContrast}\r\n";

            Dia.EstimateCannyContrast(img, mask, Dia.ECC_HALIKE, 0, ref minContrast, ref maxContrast);
            s += $"Canny constrast estimation by Halike: {minContrast}, {maxContrast}\r\n";

            tbxOutput.Text = s;
        }

        private void btnOptimum_Click(object sender, EventArgs e)
        {

        }

        KMask CreateMask()
        {
            int x = int.Parse(txbLeft.Text);
            int y = int.Parse(txbTop.Text);
            int w = int.Parse(txbWidth.Text);
            int h = int.Parse(txbHeight.Text);

            KMask m = new KMask(MaskShape.Rect, w, h);

            m.SetAncor(x, y);

            return m;
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            m_hContext = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hContext != IntPtr.Zero)
            {
                Render.DestroyContext(m_hContext);
                m_hContext = IntPtr.Zero;
            }
        }

        private void btnCannyContrast_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\cell.jpg";

            KImage img = new KImage();
            bool b = img.Load(strFile);
            Pool.Assert(b);

            img.Cast(PixelFormat.Gray);




            string s = "";
            //double n = 0;
            float minContrast = 0;
            float maxContrast = 0;

            if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_MEDINA, 0, ref minContrast, ref maxContrast))
            {
                s += $"Median Contrast: MinContrast ={minContrast}, MaxContrast ={maxContrast}\r\n";
            }
            else
            {
                s += "Median Contrast failed\r\n";
            }

            if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_ADAPTIVE, 0.15f, ref minContrast, ref maxContrast))
            {
                s += $"Adaptive Contrast: MinContrast ={minContrast}, MaxContrast ={maxContrast}\r\n";
            }
            else
            {
                s += "Adaptive Contrast failed\r\n";
            }

            if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_HALIKE, 3, ref minContrast, ref maxContrast))
            {
                s += $"Halike Contrast: MinContrast ={minContrast}, MaxContrast ={maxContrast}\r\n";
            }
            else
            {
                s += "Halike Contrast failed\r\n";
            }

            tbxOutput.Text = s;


        }
    }
}
