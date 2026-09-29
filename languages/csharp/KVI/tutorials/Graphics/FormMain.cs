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

namespace Graphics
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



        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            m_hContext = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

            //if (m_hContext != IntPtr.Zero)
            //{
            //    Render.SetRvbLogoVisible(m_hContext, true);
            //    Render.Realize(m_hContext, CanvasLayer.All);
            //    Render.Flush(m_hContext);
            //}
            cmbMoldType.SelectedIndex = 0;
            cmbBaskStyle.SelectedIndex = 1;

            cmbAlignVert.SelectedIndex = 0;
            cmbAlignHori.SelectedIndex = 0;
            cmbViewMode.SelectedIndex = 0;

            cmbFamilyName.SelectedIndex = 0;
            cmbFontHeight.SelectedIndex = 0;

            cmbTextHorizonAlign.SelectedIndex = 0;
            cmbTextVerticalAlign.SelectedIndex = 0;

            cmbFontHeight.SelectedIndex = 2;

            cmbLineType.SelectedIndex = 0;
            cmbHatchType.SelectedIndex = 0;

            tbxStrokePageOut.Text = "current stroke is null\r\n";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hContext != IntPtr.Zero)
            {
                Render.DestroyContext(m_hContext);
                m_hContext = IntPtr.Zero;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void cmbLogoPlacement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            int idx = cmbLogoPlacement.SelectedIndex;

            if (idx == 0)
            {
                Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.LeftTop);
                Render.SetRvbLogoVisible(m_hContext, true);

            }
            else if (idx == 1)
            {
                Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.RightTop);
                Render.SetRvbLogoVisible(m_hContext, true);

            }
            else if (idx == 2)
            {
                Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.LeftBottom);
                Render.SetRvbLogoVisible(m_hContext, true);

            }
            else if (idx == 3)
            {
                Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.RightBottom);
                Render.SetRvbLogoVisible(m_hContext, true);

            }
            else if (idx == 4)
            {
                Render.SetRvbLogoVisible(m_hContext, false);

            }

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnCanvasCoord_Click(object sender, EventArgs e)
        {
            //KImage imSrc = new KImage("..\\samples\\lenna.png");

            //Render.Clear(m_hContext, (int)CanvasLayer.All);

            //RvPoint pnWin = new RvPoint(80, 20);
            //Render.DrawImage(m_hContext, imSrc.GetHandle(), pnWin.x, pnWin.y, 0, 0, IntPtr.Zero);


            //imSrc = new KImage("..\\samples\\waterdrop.png");
            //GRect pnWinRect = new GRect(10, 10, 120, 120);

            //Render.DrawImageE2(m_hContext, imSrc.GetHandle(), 0, 0, pnWinRect, IntPtr.Zero);

            //btnRefresh.PerformClick();
        }

        private void xguiPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            if (ckbCanvaCoordSystem.Checked)
            {
                Point pt = e.Location;
                GPoint pnWin = Render.ConvertPointToCanvas(m_hContext, pt.X, pt.Y);

                string str = $"Mouse clicked at ({pt.X },{pt.Y}) in the panel window, and ({pnWin.x},{pnWin.y}) in the canvas correspondingly";
                MessageBox.Show(str);

                ckbCanvaCoordSystem.Checked = false;
            }

        }

        private void ckbCanvaCoordSystem_CheckedChanged(object sender, EventArgs e)
        {

            if (m_hContext == IntPtr.Zero) return;

            if (ckbCanvaCoordSystem.Checked)
            {
                Render.SetCanvasSize(m_hContext, 800, 600);
                Render.SetViewMode(m_hContext, ViewMode.Zoom);

                Render.Realize(m_hContext, CanvasLayer.All);
                Render.Flush(m_hContext);

                MessageBox.Show("Click at any place of the xguiPanel1, \r\nyou will get the mouse position in both canvas and display coordination systems");
            }
        }

        private void btnCanvasFormat_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage imSrc = new KImage("..\\samples\\Earth.png");

            Render.SetCanvasSize(m_hContext, imSrc.GetWidth(), imSrc.GetHeight());
            Render.FeedFrame(m_hContext, imSrc.GetHandle());
            Render.SetViewMode(m_hContext, ViewMode.Zoom);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }



        private void lblBackColor_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void lblBlank_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblBlankColor.BackColor = colorDialog1.Color;
            }
        }

        private void lblBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblBackColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnSetBackStyle_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb color = new GRgb(255);


            //clear all for a tidy canvas(optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //resize the canvas and set the view mode to zoom 
            //so that we can see the blank color and the background color 
            //at the same time(optional)
            Render.SetCanvasSize(m_hContext, 800, 600);
            Render.SetViewMode(m_hContext, ViewMode.Zoom);

            Render.SetCanvasBackStyle(m_hContext, cmbBaskStyle.SelectedIndex);

            color.red = lblBlankColor.BackColor.R;
            color.green = lblBlankColor.BackColor.G;
            color.blue = lblBlankColor.BackColor.B;
            Render.SetBlankColor(m_hContext, color);

            color.red = lblBackColor.BackColor.R;
            color.green = lblBackColor.BackColor.G;
            color.blue = lblBackColor.BackColor.B;
            Render.SetBackgroundColor(m_hContext, color);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnOpenGlInfo_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            int version = Render.GetDriverVersion(m_hContext);
            int sz = Render.GetSurfaceSize(m_hContext);
            int msz = Render.GetMaxSurfaceSize(m_hContext);

            string s = $"OpenGL version: {version >> 16}.{version & 0xFFFF}, Surface size: {sz}, Max surface size: {msz}";

            MessageBox.Show(s);

        }

        private void btnDrawImage_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\waterdrop.png");

            IntPtr hMold = IntPtr.Zero;
            IntPtr hOld = IntPtr.Zero;

            int x = int.Parse(tbxRegionX.Text);
            int y = int.Parse(tbxRegionY.Text);
            int w = int.Parse(tbxRegionW.Text);
            int h = int.Parse(tbxRegionH.Text);

            Render.SetTrasparence(m_hContext, ((int)(nudTransRatio.Value)) / 100.0f);

            //Remove alpha channel(if it exists) so the transparent ratio should be applied properly
            //otherwise the original alpha channel will be used (Optional)
            im.Cast(PixelFormat.BGR);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //draw rectangle(optional)
            Render.DrawRect(m_hContext, x, y, x + w, y + h, false, IntPtr.Zero);

            switch (cmbMoldType.SelectedIndex)
            {
                case 1://align mold
                    {
                        hMold = Render.CreateAlignMold(cmbAlignHori.SelectedIndex + 1, cmbAlignVert.SelectedIndex + 1);
                    }
                    break;
                case 2:
                    {
                        hMold = Render.CreateRotateMold(float.Parse(tbxRotateAngle.Text), ckbRotateKeepSize.Checked);
                    }
                    break;
                case 3:
                    {
                        hMold = Render.CreateStretchMold(ckbStretchKeepRatio.Checked, ckbStretchCentered.Checked);
                    }
                    break;
                case 4:
                    {
                        hMold = Render.CreateScaleMold(float.Parse(tbxScaleValue.Text), float.Parse(tbxScaleValue.Text), cbxScaleCentered.Checked);
                    }
                    break;
                case 5:
                    {
                        RvPoint[] vtx = new RvPoint[4];
                        vtx[0].x = 15; vtx[0].y = 30;
                        vtx[1].x = 15 + 300; vtx[1].y = 30 + 50;
                        vtx[2].x = 15 + 300; vtx[2].y = 30 + 350;
                        vtx[3].x = 15 + 10; vtx[3].y = 30 + 320;

                        hMold = Render.CreateSkewMold(vtx, cbxKeepSize.Checked);
                    }
                    break;
                case 6:
                    {
                        TileType type = TileType.None;
                        if (cbxHori.Checked && cbxVert.Checked)
                        {
                            type = TileType.Both;
                        }
                        else if (cbxHori.Checked)
                        {
                            type = TileType.Horizontal;
                        }
                        else if (cbxVert.Checked)
                        {
                            type = TileType.Vertical;
                        }

                        hMold = Render.CreateTileMold(int.Parse(tbxTileRows.Text), int.Parse(tbxTileCols.Text), (int)type);
                    }
                    break;
                case 7:
                    {
                        FlipType type = FlipType.Never;
                        if (cbxHori.Checked && cbxVert.Checked)
                        {
                            type = FlipType.Both;
                        }
                        else if (cbxHori.Checked)
                        {
                            type = FlipType.Horizontal;
                        }
                        else if (cbxVert.Checked)
                        {
                            type = FlipType.Vertical;
                        }

                        hMold = Render.CreateFlipMold((int)type);
                    }
                    break;
                default:
                    {
                        hMold = Render.CreateMold(MoldType.Default, 0, 0);
                    }
                    break;
            }

            hOld = Render.SelectMold(m_hContext, hMold);

            Render.DrawImageEx(m_hContext, im.GetHandle(),
                                          x, y, w, h,
                                          IntPtr.Zero);

            Render.SelectMold(m_hContext, hOld);

            Render.DestroyMold(hMold);

            //flush
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }



        private void btnFeedFrame_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\Earth.png");

            //before feeding image , the format of canvas should be 
            //adjusted as the image (optional )
            Render.SetCanvasSize(m_hContext, im.GetWidth(), im.GetHeight());
            Render.SetCanvasDepth(m_hContext, im.GetDepth());

            Render.FeedFrame(m_hContext, im.GetHandle());

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

            //update scrollbars
            UpdateScrollBars();

        }

        private void btnPaintImage_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\lenna.png");

            im.Cast(PixelFormat.BGR);

            Render.PaintImage(m_hContext, im.GetHandle(), 10, 10);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnTextOut_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            Render.TextOut(m_hContext, "This is Kingpool World");

            //当前位置移动到新坐标
            Render.MoveTo(m_hContext, 12, 36);

            IntPtr hFont = Render.CreateFont("Arial", 12, 0);
            IntPtr hOldFont = Render.SelectFont(m_hContext, hFont);

            IntPtr hPen = Render.CreatePen(new GRgb(255, 0, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.TextOut(m_hContext, "This is Kingpool World");

            Render.SelectFont(m_hContext, hOldFont);
            Render.SelectPen(m_hContext, hOldPen);

            Render.DestroyFont(hFont);
            Render.DestroyPen(hPen);


            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }


        private void btnDrawDummy_Click_1(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            m_hCurStroke = Render.DrawDummy(m_hContext, IntPtr.Zero);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

            tbxStrokePageOut.Text = $"current stroks is a dummy drawing, \r\nits handle is 0x{m_hCurStroke.ToInt64():X}";

        }

        private void btnReuseStroke_Click_1(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;
            if (m_hCurStroke == IntPtr.Zero) return;


            if (IntPtr.Zero != m_hCurStroke)
            {
                Render.DrawText(m_hContext, "This is Kingpool world", 13, 13, m_hCurStroke);

                tbxStrokePageOut.Text = $"The Stroke was a picture drawing, and its content is a text drawing now";
            }

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jpg file|*.jpg";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (m_hContext == IntPtr.Zero) return;

                KImage im = new KImage();

                Render.ExportCanvas(m_hContext, im.GetHandle());

                im.Save(saveFileDialog1.FileName);
            }

        }

        void UpdateScrollBars()
        {
            GSize sze = Render.GetViewDeltaEx(m_hContext);
            sbarHorizon.Value = 0;
            sbarVertical.Value = 0;
            sbarHorizon.Maximum = sze.sx;
            sbarVertical.Maximum = sze.sy;

        }

        private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            if (cmbViewMode.SelectedIndex == 1)
            {
                Render.SetViewMode(m_hContext, ViewMode.Cender);
            }
            else if (cmbViewMode.SelectedIndex == 2)
            {
                Render.SetViewMode(m_hContext, ViewMode.Stretch);
            }
            else if (cmbViewMode.SelectedIndex == 3)
            {
                Render.SetViewMode(m_hContext, ViewMode.Zoom);
            }
            else
            {
                Render.SetViewMode(m_hContext, ViewMode.Default);

            }


            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


            //update scroll bars
            UpdateScrollBars();
        }

        private void tkbScale_ValueChanged(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            float scale = tkbScale.Value / 10.0f;

            Render.SetViewScaleEx(m_hContext, scale, scale);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

            //update scroll bars
            UpdateScrollBars();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            //clear current frame
            Render.Clear(m_hContext, CanvasLayer.Background);

            //remove all strokes
            Render.Clear(m_hContext, CanvasLayer.Foreground);


            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void sbarHorizon_ValueChanged(object sender, EventArgs e)
        {
            int n = sbarHorizon.Value;

            Render.SetViewPos(m_hContext, n, false);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void sbarVertical_ValueChanged(object sender, EventArgs e)
        {
            int n = sbarVertical.Value;

            Render.SetViewPos(m_hContext, n, true);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);
            GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            int lineWidth = 1;
            int n = 1;
            if (int.TryParse(tbxLineWidth.Text, out n))
            {
                lineWidth = Math.Max(1, Math.Min(7, n));
            }

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //draw a single line
            Render.DrawLine(m_hContext, 30, 120, 100, 120, IntPtr.Zero);

            //  draw dots;
            RvPoint[] pnDots = new RvPoint[12]
            {
                new RvPoint( 120, 220 ), new RvPoint( 440, 220 ),
                new RvPoint(  260, 220 ), new RvPoint( 480, 220 ),
                new RvPoint(120, 230), new RvPoint(440, 230),
                new RvPoint(460, 230),new RvPoint( 480, 230),
                new RvPoint(120, 240), new RvPoint(440, 240),
                new RvPoint(460, 240), new RvPoint(480, 240),
            };
            Render.DrawDots(m_hContext, pnDots, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 0, 0));
            Array.Resize(ref pnDots, 4);
            pnDots[0].x += 30; pnDots[0].y += 40;
            pnDots[3].x += 50; pnDots[3].y += 60;

            Render.DrawPolyline(m_hContext, pnDots, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void lblTextColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblTextColor.BackColor = colorDialog1.Color;
            }
        }

        private void lblTextBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblTextBackColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnDrawText_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            string strText = "This is Kingpool world";

            string strFamilyName = cmbFamilyName.Text;

            GRgb textColor = new GRgb(lblTextColor.BackColor.R, lblTextColor.BackColor.G, lblTextColor.BackColor.B);
            GRgb backColor = new GRgb(lblTextBackColor.BackColor.R, lblTextBackColor.BackColor.G, lblTextBackColor.BackColor.B);

            int fontHeight = 11;
            int n = 11;
            if (int.TryParse(cmbFontHeight.Text, out n))
            {
                fontHeight = n;
            }

            int x = int.Parse(txbTextAlignX.Text);
            int y = int.Parse(txbTextAlignY.Text);
            int w = int.Parse(txbTextAlignW.Text);
            int h = int.Parse(txbTextAlignH.Text);

            FontFlags fontflags = FontFlags.Default;

            int format = 0;
            int linespace = 0;
            int.TryParse(tbxLineSpace.Text, out linespace);

            GRect rect = new GRect(x, y, x + w, y + h);


            if (ckbBold.Checked) { fontflags |= FontFlags.Bold; }
            if (ckbItalic.Checked) { fontflags |= FontFlags.Italic; }
            if (ckbUnderline.Checked) { fontflags |= FontFlags.Underline; }
            if (ckbStrikeOut.Checked) { fontflags |= FontFlags.Strikeout; }

            //create font
            IntPtr hFont = Render.CreateFontEx(strFamilyName, fontHeight, textColor, backColor, fontflags);
            IntPtr hOld = Render.SelectFont(m_hContext, hFont);

            if (cmbTextHorizonAlign.SelectedIndex == 0) { format |= (int)TextAlign.Left; }
            else if (cmbTextHorizonAlign.SelectedIndex == 1) { format |= (int)TextAlign.Hcenter; }
            else if (cmbTextHorizonAlign.SelectedIndex == 2) { format |= (int)TextAlign.Right; }

            if (cmbTextVerticalAlign.SelectedIndex == 0) { format |= (int)TextAlign.Top; }
            else if (cmbTextVerticalAlign.SelectedIndex == 1) { format |= (int)TextAlign.Vcenter; }
            else if (cmbTextVerticalAlign.SelectedIndex == 2) { format |= (int)TextAlign.Bottom; }

            float prevTransRatio = Render.GetTrasparence(m_hContext);
            if (ckbTextTranparent.Checked)
            {
                format |= Render.DT_TRANSPARENT;

                Render.SetTrasparence(m_hContext, ((int)(nudTransRatio.Value)) / 100.0f);
            }

            if (ckbMultiLines.Checked)
            {
                format |= Render.DT_MULTILINE;
                string s = strText;
                //make the length of text large enough to break into multiple lines
                for (int i = 0; i < 4; i++)
                {
                    s += "\n";
                    s += strText;
                }
                s += "\t";
                for (int i = 0; i < 4; i++)
                {
                    s += strText;
                    s += " ";
                }
                strText = s;
            }
            if (ckbWordBreak.Checked) { format |= Render.DT_WORDBREAK; }

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //draw rectangle(optional)
            Render.DrawRect(m_hContext, x, y, x + w, y + h, false, IntPtr.Zero);



            Render.DrawTextEx(m_hContext, strText, rect, format, linespace, IntPtr.Zero);

            Render.SelectFont(m_hContext, hOld);

            Render.DestroyFont(hFont);

            //restore current tranparence ratio(optional)
            if (ckbTextTranparent.Checked)
            {
                Render.SetTrasparence(m_hContext, prevTransRatio);
            }

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);




        }

        private void btnDrawRectangle_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);
            GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            int lineWidth = 1;
            int n = 1;
            if (int.TryParse(tbxLineWidth.Text, out n))
            {
                lineWidth = Math.Max(1, Math.Min(7, n));
            }
            int brushSize = 6;
            if (int.TryParse(tbxBrushSize.Text, out n))
            {
                brushSize = Math.Max(1, Math.Min(15, n));
            }


            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);





            //draw rect without filling anything 
            Render.DrawRect(m_hContext, 30, 50, 100, 100, false, IntPtr.Zero);

            IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);
            IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

            //filling a rectangle
            Render.DrawRect(m_hContext, 30 + 100, 50, 100 + 100, 100, true, IntPtr.Zero);

            Render.SelectBrush(m_hContext, hOldBrush);
            Render.DestroyBrush(hBrush);

            Render.SelectPen(m_hContext, hOldPen);


            Render.DestroyPen(hPen);



            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);
            GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            int lineWidth = 1;
            int n = 1;
            if (int.TryParse(tbxLineWidth.Text, out n))
            {
                lineWidth = Math.Max(1, Math.Min(7, n));
            }
            int brushSize = 6;
            if (int.TryParse(tbxBrushSize.Text, out n))
            {
                brushSize = Math.Max(1, Math.Min(15, n));
            }


            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //     draw arc             
            Render.DrawArc(m_hContext, 320, 350, 50, 0, 135, IntPtr.Zero);
            //     draw circle
            Render.DrawCircle(m_hContext, 120, 200, 35, false, IntPtr.Zero);
            //     draw ellipse
            Render.DrawEllipse(m_hContext, 160, 380, 50, 80, 40, false, IntPtr.Zero);

            IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);
            IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

            Render.DrawCircle(m_hContext, 360, 200, 35, true, IntPtr.Zero);

            //     draw ellipse
            Render.DrawEllipse(m_hContext, 260, 80, 50, 80, 72, true, IntPtr.Zero);

            Render.SelectBrush(m_hContext, hOldBrush);
            Render.DestroyBrush(hBrush);

            Render.SelectPen(m_hContext, hOldPen);


            Render.DestroyPen(hPen);



            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnDrawComposite_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);
            GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            int lineWidth = 1;
            int n = 1;
            if (int.TryParse(tbxLineWidth.Text, out n))
            {
                lineWidth = Math.Max(1, Math.Min(7, n));
            }
            int brushSize = 6;
            if (int.TryParse(tbxBrushSize.Text, out n))
            {
                brushSize = Math.Max(1, Math.Min(15, n));
            }

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //    draw cross
            Render.DrawCross(m_hContext, 220, 80, 70, 60, 45.0f, IntPtr.Zero);

            //     draw segments 
            //    : each segment has two points
            RvPoint[] pnSegments = new RvPoint[4] { new RvPoint(170, 370), new RvPoint(220, 400), new RvPoint(320, 370), new RvPoint(370, 400) };
            Render.DrawSegments(m_hContext, pnSegments, IntPtr.Zero);

            RvBox2D box = new RvBox2D(230, 230, 150, 80, -50);
            Render.DrawBox2D(m_hContext, box, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);


            Render.DestroyPen(hPen);



            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);





        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);
            GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

            int lineWidth = 1;
            int n = 1;
            if (int.TryParse(tbxLineWidth.Text, out n))
            {
                lineWidth = Math.Max(1, Math.Min(7, n));
            }
            int brushSize = 6;
            if (int.TryParse(tbxBrushSize.Text, out n))
            {
                brushSize = Math.Max(1, Math.Min(15, n));
            }


            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            //set canvas back color to white for better viewing(optional )
            Render.SetCanvasBackStyle(m_hContext, 0);
            GRgb color = new GRgb(255, 255, 255);
            Render.SetBackgroundColor(m_hContext, color);

            IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //     draw equilateral triangle
            Render.DrawTriangle(m_hContext, 220, 350, 90, 30, false, IntPtr.Zero);

            //   draw polygon;
            RvPoint[] pnPolyon = new RvPoint[6] {new RvPoint( 120, 50), new RvPoint( 270, 50), new RvPoint( 320, 100),
                new RvPoint( 270, 150),new RvPoint( 120, 150),new RvPoint( 170, 100) };

            Render.DrawPolygon(m_hContext, pnPolyon, false, IntPtr.Zero);


            IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);
            IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

            Render.DrawTriangle(m_hContext, 100, 350, 90, 30, true, IntPtr.Zero);

            //   draw polygon;
            pnPolyon = new RvPoint[6] {new RvPoint( 120, 160), new RvPoint( 270, 160), new RvPoint( 320, 210),
                new RvPoint( 270, 260),new RvPoint( 120, 260),new RvPoint( 170,210) };

            Render.DrawPolygon(m_hContext, pnPolyon, true, IntPtr.Zero);


            Render.SelectBrush(m_hContext, hOldBrush);
            Render.DestroyBrush(hBrush);

            Render.SelectPen(m_hContext, hOldPen);


            Render.DestroyPen(hPen);



            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);



        }

        private void lblLineColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblLineColor.BackColor = colorDialog1.Color;
            }
        }

        private void lblFillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblFillColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;
            if (m_hCurStroke == IntPtr.Zero) return;


            if (m_hCurStroke == m_hPictureStroke)
            {
                KImage im = new KImage("..\\samples\\waterdrop.png");

                NativeArg arg = new NativeArg(im.Handle);
                Render.Modify(m_hContext, m_hCurStroke, ModifyType.Image, arg.Ptr);
            }

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (m_hContext == IntPtr.Zero) return;
            if (m_hCurStroke == IntPtr.Zero) return;

            Render.Erase(m_hContext, m_hCurStroke, EraseType.Delete);

            if (m_hCurStroke == m_hPictureStroke)
            {
                m_hPictureStroke = IntPtr.Zero;
            }
            if (m_hCurStroke == m_hRectangleStroke)
            {
                m_hRectangleStroke = IntPtr.Zero;
            }
            m_hCurStroke = IntPtr.Zero;

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnCombinie_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;
            if (m_hPictureStroke == IntPtr.Zero || m_hRectangleStroke == IntPtr.Zero) return;

            IntPtr[] arr = new IntPtr[2] { m_hPictureStroke, m_hRectangleStroke };

            m_hMergeStroke = Render.Combine(m_hContext, arr);
            Pool.Assert(m_hMergeStroke != IntPtr.Zero);

            m_hPictureStroke = IntPtr.Zero;
            m_hRectangleStroke = IntPtr.Zero;

            m_hCurStroke = m_hMergeStroke;

            tbxStrokePageOut.Text = $"Two stroke were combined into one, \r\nits handle is 0x{m_hCurStroke.ToInt64():X}";

        }

        private void btnUncombine_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;
            if (m_hMergeStroke == IntPtr.Zero) return;

            IntPtr[] subarr = Render.Uncombine(m_hContext, m_hMergeStroke, 2);
            m_hMergeStroke = IntPtr.Zero;

            if (subarr != null)
            {
                tbxStrokePageOut.Text = $"The combined stroke were split to two strokes, \r\n";

                int c = 1;
                string tmp = "";
                foreach (IntPtr p in subarr)
                {
                    Pool.Assert(p != IntPtr.Zero);

                    string s = $"Handle of Stroke {c} is 0x{p.ToInt64():X} \r\n";
                    tmp += s;
                    c++;
                }

                tbxStrokePageOut.Text += tmp;
            }

        }



        private void btnHide_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;
            if (m_hCurStroke == IntPtr.Zero) return;

            if (btnHideStroke.Text == "Hide")
            {
                btnHideStroke.Text = "Show";
                Render.Hide(m_hContext, m_hCurStroke, true);
            }
            else
            {
                btnHideStroke.Text = "Hide";
                Render.Hide(m_hContext, m_hCurStroke, false);
            }


            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnDrawPitcture_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\lenna.png");

            GRect rect = new GRect(30, 30, 150, 120);
            m_hPictureStroke = Render.DrawImageE2(m_hContext, im.GetHandle(), rect, IntPtr.Zero);

            m_hCurStroke = m_hPictureStroke;

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

            btnHideStroke.Text = "Hide";
            tbxStrokePageOut.Text = $"Current stroks is a picture drawing, \r\nits handle is 0x{m_hCurStroke.ToInt64():X}";


        }

        private void btnDrawRectInStroke_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            int left = 160;
            int top = 140;
            int width = 100;
            int height = 140;
            m_hRectangleStroke = Render.DrawRect(m_hContext, left, top, left + width, top + height, false, IntPtr.Zero);

            m_hCurStroke = m_hRectangleStroke;

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

            btnHideStroke.Text = "Hide";
            tbxStrokePageOut.Text = $"Current stroks is a rectangle drawing, \r\nits handle is 0x{m_hCurStroke.ToInt64():X}";

        }

        private void btnDrawMask_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\shapes.png");

            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.GetHandle(), MinErrorMode.Poisson);

            KMask msk = new KMask(im);

            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hBrush = Render.GetCurBrush(m_hContext);
            Render.SetBrushColor(hBrush, new GRgb(255, 0, 0));

            Render.DrawMask(m_hContext, msk.GetHandle(), 0, 0, IntPtr.Zero);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnDrawBlob_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\triangle.png");

            im.Cast(PixelFormat.Gray);
            Dip.Simple(im.GetHandle(), 128, false);

            KBlob blob = KBlob.FromBinaryImage(im);

            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hBrush = Render.GetCurBrush(m_hContext);
            Render.SetBrushColor(hBrush, new GRgb(0, 255, 0));

            Render.DrawBlob(m_hContext, blob.GetHandle(), 0, 0, IntPtr.Zero);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }


        private void btnDrawContour_Click(object sender, EventArgs e)
        {
            if (m_hContext == IntPtr.Zero) return;

            KImage im = new KImage("..\\samples\\shapes.png");

            im.Cast(PixelFormat.Gray);
            Dip.Simple(im.GetHandle(), 128, false);

            KSequence seq = new KSequence();

            KContour.FindContours(im, ContourCoder.Default, ContourFilter.Any, 12, seq);

            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.GetCurPen(m_hContext);
            Render.SetPenColor(hPen, new GRgb(0, 0, 255));

            for (int i = 0; i < seq.Count; i++)
            {
                Render.DrawContour(m_hContext, seq.GetAt(i), 0, 0, IntPtr.Zero);
            }

            //release all contours
            for (int i = 0; i < seq.Count; i++)
            {
                KContour.ReleaseContour(seq.GetAt(i));
            }

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void tkbScale_Scroll(object sender, EventArgs e)
        {

        }
    }


}
