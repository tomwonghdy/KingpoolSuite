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

namespace ImageBasics
{
    public partial class FormMain : Form
    {
        KImage m_image;

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

        void FillRandom(KImage image)
        {
            UInt64 size = (UInt64)(image.GetHeight() * image.GetWidth() * image.GetBytesPerPixel());

            byte[] data = new byte[size];

            Random rnd = new Random();
            for (UInt64 i = 0; i < size; ++i)
            {
                data[i] = (byte)(rnd.Next() % 256);
            }
            image.FloodEx(data);
        }

        private void btnCreateFill_Click(object sender, EventArgs e)
        {
            m_image = new KImage(PixelFormat.BGR, 160, 120);

            //show default image
            ShowImageInPictureBox(m_image, picSrc);

            FillRandom(m_image);

            //show random image
            ShowImageInPictureBox(m_image, picDest);


        }

        string GetPixelFormatDesc(int pixfmt)
        {
            switch (pixfmt)
            {
                case (int)PixelFormat.Bin:
                    return ("BIN");
                case (int)PixelFormat.Gray:
                    return ("GRAY");
                case (int)PixelFormat.BGR:
                    return ("BGR");
                case (int)PixelFormat.BGRA:
                    return ("BGRA");
                default:
                    return ("UNKNOWN");
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\colorwave.jpg";

            m_image = new KImage();
            if (m_image.Load(strFile))
            {
                ShowImageInPictureBox(m_image, picSrc);

                string s;

                s = $"Width: {m_image.GetWidth()}\r\n";
                tbxOutput.Text += s;

                s = $"Height: {m_image.GetHeight()}\r\n";
                tbxOutput.Text += s;

                s = $"Pixel Format: {GetPixelFormatDesc((int)m_image.GetPixelFormat())}\r\n";
                tbxOutput.Text += s;

                s = $"Pixel Depth: { m_image.GetDepth()}\r\n";
                tbxOutput.Text += s;

                s = $"Channels: { m_image.GetChannels() }\r\n";
                tbxOutput.Text += s;
            }
            else
            {
                tbxOutput.Text += "image loading failed\r\n";
            }
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\lenna.png";

            m_image = new KImage();
            if (m_image.Load(strFile))
            {
                ShowImageInPictureBox(m_image, picSrc);
            }
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            m_image = new KImage(PixelFormat.BGR, 160, 120);

            m_image.Flood(255);

            KImage imNew = m_image.Clone();

            m_image.Flood(0);

            //show default image
            ShowImageInPictureBox(m_image, picSrc);

            ShowImageInPictureBox(imNew, picDest);
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            m_image = new KImage(PixelFormat.BGR, 160, 120);

            m_image.Flood(255);

            //show default image
            ShowImageInPictureBox(m_image, picSrc);

            m_image.SetSize(200, 200);
            FillRandom(m_image);

            ShowImageInPictureBox(m_image, picDest);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\waterdrop.png";

            m_image = new KImage();
            if (m_image.Load(strFile))
            {
                ShowImageInPictureBox(m_image, picSrc);

                KImage imgray = new KImage(PixelFormat.Gray, m_image.GetWidth(), m_image.GetHeight());

                m_image.Split(imgray, null, null);

                ShowImageInPictureBox(imgray, picDest);
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            KImage imRed = new KImage(PixelFormat.Gray, 120, 100);
            KImage imGreen = new KImage(PixelFormat.Gray, 120, 100);
            KImage imBlue = new KImage(PixelFormat.Gray, 120, 100);

            m_image = new KImage(PixelFormat.BGR, 120, 100);

            FillRandom(imRed);

            ShowImageInPictureBox(imRed, picSrc);

            m_image.Merge(imRed, imGreen, imBlue);

            ShowImageInPictureBox(m_image, picDest);

        }

        private void btnLoadFromMemory_Click(object sender, EventArgs e)
        {
            byte[] fileData = File.ReadAllBytes("..\\samples\\lenna.png");

            if (null != fileData)
            {
                m_image = new KImage();

                if (!m_image.LoadImageInMemory(fileData))
                {
                    tbxOutput.Text += "image loading failed\r\n";
                }
                else
                {
                    ShowImageInPictureBox(m_image, picDest);
                }
            }
        }

        private void btnExportToMemory_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\lenna.png";

            m_image = new KImage();

            bool ret = m_image.Load(strFile);
            Pool.Assert(ret);

            ShowImageInPictureBox(m_image, picSrc);

            m_image.Cast(PixelFormat.BGR);

            MemoryStream ms = new MemoryStream();

            UInt64 size = (UInt64)(m_image.GetHeight() * CalcBitmapPicth(m_image.GetWidth(), m_image.GetBytesPerPixel() * 8));

            byte[] data = new byte[size];
            uint sze = m_image.ExportImageToMemory(KImage.IMF_JPG, 0, ref data);

            Pool.Assert(size > 0);
            ms.Write(data, 0, (int)sze);

            Image im = Image.FromStream(ms);
            picDest.BackgroundImage = im;

        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            m_image = new KImage();
            FillRandom(m_image);

            ShowImageInPictureBox(m_image, picSrc);
            if (!m_image.Save(".\\tmp\\save_file_test.jpg"))
            {
                tbxOutput.Text += "Save image failure\r\n";
            }
        }

        private void btnPixelFormat_Click(object sender, EventArgs e)
        {
            m_image = new KImage(PixelFormat.BGR, 160, 120);

            FillRandom(m_image);

            ShowImageInPictureBox(m_image, picSrc);

            m_image.Cast(PixelFormat.Gray);

            ShowImageInPictureBox(m_image, picDest);
        }

        private void btnVirtualImage_Click(object sender, EventArgs e)
        {
            string strFile = "..\\samples\\lenna.png";

            m_image = new KImage();
            if (m_image.Load(strFile))
            {
                ShowImageInPictureBox(m_image, picSrc);
            }
        }

        private void btnCloneDummy_Click(object sender, EventArgs e)
        {
            m_image = new KImage(PixelFormat.BGR, 160, 120);

            m_image.Flood(255);

            //WARNING:
            //When clone in dummy, the lifetime of m_image must be longer
            //than that of imNew; otherwise, using imNew may cause a program crash.
            KImage imNew = m_image.Clone(true);

            FillRandom(m_image);

            //show default image
            ShowImageInPictureBox(m_image, picSrc);

            ShowImageInPictureBox(imNew, picDest);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
