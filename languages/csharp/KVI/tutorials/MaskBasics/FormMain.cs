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

namespace MaskBasics
{
    public partial class FormMain : Form
    {

        int CalcBitmapPicth(int width, int pixbits)
        {
            return (((width) * (pixbits) + 31) / 32 * 4);
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void ShowMaskInPictureBox(KMask mask, PictureBox picBox)
        {
            KImage im = mask.Derive();

            MemoryStream ms = new MemoryStream();

            UInt64 size = (UInt64)(mask.GetHeight() * CalcBitmapPicth(mask.GetWidth(), 8));

            byte[] data = new byte[size];
            uint sze = im.ExportImageToMemory(KImage.IMF_JPG, 0, ref data);

            if (sze > 0)
            {
                ms.Write(data, 0, (int)sze);
                picBox.BackgroundImage = Image.FromStream(ms);
            }
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

        private void btnCreateFill_Click(object sender, EventArgs e)
        {

            KMask mask = new KMask();

            mask.Create(MaskShape.FilledEllipse, 160, 160);

            ShowMaskInPictureBox(mask, picPreview1);

            //角度 单位： 为1/100度
            mask.CreateBanana(MaskShape.BananaQ1, 160, 4500, 60);

            ShowMaskInPictureBox(mask, picPreview2);

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
            KMask mask = new KMask();
            mask.Create(MaskShape.FullStrap, 120, 100);

            ShowMaskInPictureBox(mask, picPreview1);

            string str = "Properies used frequently \r\n";

            str += $"Width: {mask.GetWidth()} \r\n";
            str += $"Height: {mask.GetWidth()} \r\n";
            str += $"Pitch: {mask.GetPitch()} \r\n";
            str += $"Size: {mask.GetSize()} \r\n";
            str += $"Ancor Pos: {mask.GetAnchor().ToString()} \r\n";
            str += $"Origin: {mask.GetOrigin().ToString()} \r\n";
            str += $"Area: {mask.GetArea()} \r\n";

            MessageBox.Show(str, "Properties");
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            //string strFile = "..\\samples\\lenna.png";

            //m_image = new KImage();
            //if (m_image.Load(strFile))
            //{
            //    ShowImageInPictureBox(m_image, picSrc);
            //}
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            KMask mask = new KMask(60, 60);
            mask.Create(MaskShape.Ring, 100, 80, 20);
            ShowMaskInPictureBox(mask, picPreview1);

            KMask newMask = mask.Clone();
            newMask.Toggle();

            ShowMaskInPictureBox(newMask, picPreview2);
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            KMask mask = new KMask();
            mask.Create(MaskShape.Ring, 100, 80);
            mask.Resize(100, 100, true);
            ShowMaskInPictureBox(mask, picPreview1);

            mask.Scale(1.2f);

            ShowMaskInPictureBox(mask, picPreview2);
        }



        private void btnDeriveMask_Click(object sender, EventArgs e)
        {
            KImage im = new KImage();
            bool ret = im.Load("..\\samples\\triangle.png");
            Pool.Assert(ret);

            im.Cast(PixelFormat.Bin);
            ShowImageInPictureBox(im, picPreview1);

            KMask m = new KMask();
            m.Reshape(im);

            ShowMaskInPictureBox(m, picPreview2);
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            KMask m1 = new KMask();
            m1.Create(MaskShape.HorizontalStrap, 100, 80);

            KMask m2 = new KMask();
            m2.Create(MaskShape.VerticalStrap, 100, 80);

            KMask m = KMask.Merge(m1, m2, (int)KMask.MergeType.AnyOf);

            ShowMaskInPictureBox(m, picPreview1);

            m = KMask.Merge(m1, m2, (int)KMask.MergeType.Both);

            ShowMaskInPictureBox(m, picPreview2);


        }

        private void btnDeriveImage_Click(object sender, EventArgs e)
        {
            KMask m = new KMask();
            m.Create(MaskShape.HorizontalStrap, 100, 80);

            ShowMaskInPictureBox(m, picPreview1);

            KImage im = m.Derive();
            ShowImageInPictureBox(im, picPreview2);

        }
    }


}
