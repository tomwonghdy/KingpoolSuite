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

namespace BlobBasics
{
    public partial class FormMain : Form
    {
        IntPtr m_hContext = IntPtr.Zero;

        int CalcBitmapPicth(int width, int pixbits)
        {
            return (((width) * (pixbits) + 31) / 32 * 4);
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void ShowImageInPictureBox(KImage image, PictureBox picBox)
        {
            MemoryStream ms = new MemoryStream();

            UInt64 size = (UInt64)(image.GetHeight() * CalcBitmapPicth(image.GetWidth(), image.GetBytesPerPixel() * 8));

            byte[] data = new byte[size];
            uint sze = image.ExportImageToMemory(KImage.IMF_JPG, 0, ref data);

            if (sze > 0)
            {
                ms.Write(data, 0, (int)sze);
                picBox.BackgroundImage = Image.FromStream(ms);
            }
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            m_hContext = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            cmbEncoder.SelectedIndex = 0;
        }

        void ShowBlobInCanvas(KImage image, KSequence seq)
        {
            Pool.Assert(m_hContext != IntPtr.Zero);

            int dep = 24;
            if (image.GetPixelFormat() == PixelFormat.BGRA) dep = 32;
            else if (image.GetPixelFormat() == PixelFormat.Bin) dep = 2;
            else if (image.GetPixelFormat() == PixelFormat.Gray) dep = 8;
            else if (image.GetPixelFormat() == PixelFormat.BGR) dep = 24;
            else return;

            Render.Clear(m_hContext, CanvasLayer.All);


            Render.SetCanvasSize(m_hContext, image.GetWidth(), image.GetHeight());
            Render.SetCanvasDepth(m_hContext, dep);

            Render.FeedFrame(m_hContext, image.Handle);


            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            if (seq != null)
            {
                //Geometric Properties
                for (int i = 0; i < seq.Count; i++)
                {
                    KBlob blob = new KBlob(seq.GetAt(i), true);
                    RvRect re = blob.GetRect();

                    Render.DrawRect(m_hContext, re.left, re.top, re.right, re.bottom, false, IntPtr.Zero);
                }
            }

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);



            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\cell.jpg");

            KImage imOri = im.Clone();

            im.Cast(PixelFormat.Gray);
            Dip.Invert(im.Handle);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);
            Dip.RemoveBorder(im.Handle);

            int msize = 4;
            msize = int.Parse(txbMinimumSize.Text);

            KSequence seq = KBlob.ExtractRawList(im, false, msize);

            if (seq != null)
            {
                string str = $"{seq.Count} blobs extracted";
                tbxOutput.Text = str;

                ShowBlobInCanvas(imOri, seq);

                KBlob.ReleaseBlobList(seq);
            }


        }

        private void btnProperties_Click_1(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\cell.jpg");

            KImage imOri = im.Clone();

            im.Cast(PixelFormat.Gray);
            Dip.Invert(im.Handle);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);
            Dip.RemoveBorder(im.Handle);

            int msize = 4;
            msize = int.Parse(txbMinimumSize.Text);

            KSequence seq = null;

            if (cmbEncoder.SelectedIndex == 0)
            {
                seq = KBlob.ExtractRawList(im, false, msize);
            }
            else if (cmbEncoder.SelectedIndex == 1)
            {
                seq = KBlob.ExtractClusterList(im, false, msize);
            }
            else
            {
                seq = KBlob.ExtractContourList(im, 0, msize);
            }

            if (seq != null)
            {
                string str = $"{seq.Count} blobs extracted\r\n";


                //Geometric Properties
                for (int i = 0; i < seq.Count; i++)
                {
                    KBlob blob = new KBlob(seq.GetAt(i), true);
                    str += $"The {i + 1}th blob: \r\n";
                    str += $"Centroid : {blob.GetCentroid().ToString()}\r\n";
                    str += $"Circular : {blob.GetCircular()}\r\n";
                    str += $"Offset : {blob.GetOffset().ToString()}\r\n";
                    str += $"Perimeter : {blob.GetPerimeter()}\r\n";
                    str += $"Slope : {blob.GetSlope()}\r\n";
                    str += $"Rect : {blob.GetRect().ToString()}\r\n";
                    str += $"Area : {blob.GetArea()}\r\n";

                    str += "\r\n";
                }

                tbxOutput.Text = str;

                ShowBlobInCanvas(imOri, seq);

                KBlob.ReleaseBlobList(seq);
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\cell.jpg");



            Dip.Invert(im.Handle);
            im.Cast(PixelFormat.Gray);

            KImage imGray = im.Clone();

            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KSequence seq = KBlob.ExtractClusterList(im, false, 14);

            if (seq != null)
            {
                string str = $"{seq.Count} blobs extracted\r\n";


                //Pixel Statistics 
                for (int i = 0; i < seq.Count; i++)
                {
                    KBlob blob = new KBlob(seq.GetAt(i), true);
                    str += $"The {i + 1}th blob: \r\n";

                    double n = blob.GetStrength(imGray);
                    str += $"Strength : {n}\r\n";

                    n = blob.GetAverage(imGray);
                    str += $"Average : {n}\r\n";

                    n = blob.GetVariance(imGray);
                    str += $"Variance : {n}\r\n";

                    str += "\r\n";
                }

                tbxOutput.Text = str;

                ShowBlobInCanvas(imGray, seq);

                KBlob.ReleaseBlobList(seq);
            }
        }

        private void btnBlobDistance_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\shapes.png");
            KImage imOri = im.Clone();

            int msize = 4;
            msize = int.Parse(txbMinimumSize.Text);

            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KSequence seq = KBlob.ExtractClusterList(im, false, msize);

            if (seq != null)
            {
                string str = $"{seq.Count} blobs extracted\r\n";

                for (int i = 0; i < seq.Count - 1; i++)
                {
                    using (KBlob blob1 = new KBlob(seq.GetAt(i), true))
                    {
                        for (int j = i + 1; j < seq.Count; j++)
                        {
                            using (KBlob blob2 = new KBlob(seq.GetAt(j), true))
                            {
                                double n = KBlob.Distance(blob1, blob2);
                                str += $"The distance between the {i + 1}th blob and the {j + 1}th blob is \r\n{n} \r\n";

                                n = KBlob.Distance(blob1, blob2, BlobDistance.Rect);
                                str += $"The bound rectangle center distance\r\nbetween the {i + 1}th blob and the {j + 1}th blob is \r\n{n} \r\n";

                                n = KBlob.Distance(blob1, blob2, BlobDistance.MinBox);
                                str += $"The minimum rectangle center distance\r\nbetween the {i + 1}th blob and the {j + 1}th blob is \r\n{n} \r\n";

                                n = KBlob.Distance(blob1, blob2, BlobDistance.Contour);
                                str += $"The contour distance \r\nbetween the {i + 1}th blob and the {j + 1}th blob is \r\n{n} \r\n";


                                str += "\r\n";
                            }
                        }

                    }

                }

                tbxOutput.Text = str;

                ShowBlobInCanvas(imOri, seq);

                KBlob.ReleaseBlobList(seq);
            }

        }



        private void btnDensity_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\shapes.png");

            KImage imOri = im.Clone();
            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KSequence seq = KBlob.ExtractClusterList(im, false, 4);

            if (seq != null)
            {
                string str = $"{seq.Count} blobs extracted\r\n";

                for (int i = 0; i < seq.Count - 1; i++)
                {
                    using (KBlob blob = new KBlob(seq.GetAt(i), true))
                    {
                        double n = blob.GetDensity(BlobPart.Whole);

                        str += $"The while density of the {i + 1}th blob is {n} \r\n";

                        n = blob.GetDensity(BlobPart.East);
                        str += $"The east density of the {i + 1}th blob is {n} \r\n";

                        n = blob.GetDensity(BlobPart.South);
                        str += $"The south density of the {i + 1}th blob is {n} \r\n";

                        n = blob.GetDensity(BlobPart.West);
                        str += $"The west density of the {i + 1}th blob is {n} \r\n";

                        n = blob.GetDensity(BlobPart.North);
                        str += $"The north density of the {i + 1}th blob is {n} \r\n";

                        str += "\r\n";
                    }

                }

                tbxOutput.Text = str;

                ShowBlobInCanvas(imOri, seq);

                KBlob.ReleaseBlobList(seq);
            }
        }

        private void btnToMask_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\triangle.png");

            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KBlob blob = KBlob.FromBinaryImageRaw(im);

            KMask mask = blob.ToMask();


            FormMaskViewer fbv = new FormMaskViewer();
            fbv.m_mask = mask;

            fbv.ShowDialog();

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hContext != IntPtr.Zero)
            {
                Render.DestroyContext(m_hContext);
                m_hContext = IntPtr.Zero;
            }
        }
    }
}
