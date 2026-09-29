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


using Kingpool.Vision;
using Kingpool.Core;

namespace BlobBasics
{

    public partial class FormMaskViewer : Form
    {
        public KMask m_mask = null;

        int CalcBitmapPicth(int width, int pixbits)
        {
            return (((width) * (pixbits) + 31) / 32 * 4);
        }

        private void ShowBlobInPictureBox(KMask mask, PictureBox picBox)
        {
            if (null == mask) return;

            KImage image = mask.Derive();

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
        public FormMaskViewer()
        {
            InitializeComponent();
        }

        private void FormMaskViewer_Load(object sender, EventArgs e)
        {
            if (m_mask != null)
            {
                ShowBlobInPictureBox(m_mask, picPreview);
            }

        }
    }
}
