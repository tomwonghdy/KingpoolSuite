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

namespace SmartMath
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


        void ShowImagesInCanvas(KImage image)
        {
            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            Render.DrawImage(m_hContext, image.Handle, IntPtr.Zero);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }
#pragma warning disable CS0246 // 未能找到类型或命名空间名“KImage”(是否缺少 using 指令或程序集引用?)
        void ShowImagesInCanvas(KImage[] arr, int nOriginalIndex = 0)
#pragma warning restore CS0246 // 未能找到类型或命名空间名“KImage”(是否缺少 using 指令或程序集引用?)
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

        public FormMain()
        {
            InitializeComponent();
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

        private void btnArc3p_Click(object sender, EventArgs e)
        {
            RvPoint p1 = new RvPoint(130, 150);
            RvPoint p2 = new RvPoint(180, 210);
            RvPoint p3 = new RvPoint(225, 155);



            RvPoint center;
            int radius;

            bool b = Smath.GetArcCenter(p1, p2, p3, out center, out radius);

            Pool.Assert(b);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawCross(m_hContext, p1.x, p1.y, 12, 12, 0, IntPtr.Zero);
            Render.DrawCross(m_hContext, p2.x, p2.y, 12, 12, 0, IntPtr.Zero);
            Render.DrawCross(m_hContext, p3.x, p3.y, 12, 12, 0, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //     draw circle
            Render.DrawCircle(m_hContext, center.x, center.y, radius, false, IntPtr.Zero);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnRotateLine_Click(object sender, EventArgs e)
        {
            RvPointF32 p0 = new RvPointF32(130, 50);
            RvPointF32 p1 = new RvPointF32(180, 110);

            RvPoint[] ptarr = new RvPoint[4] { new RvPoint(216,102), new RvPoint(372, 299),
                                               new RvPoint(216, 299) ,new RvPoint(216,102) };



            RvPointF32 newpt = Smath.RotatePoint(p0, p1.x, p1.y, -45);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawLineE1(m_hContext, p0.x, p0.y, p1.x, p1.y, IntPtr.Zero);
            Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //     draw circle
            Render.DrawLineE1(m_hContext, p1.x, p1.y, newpt.x, newpt.y, IntPtr.Zero);

            Smath.RotateVertex(ptarr, 60, 70, 25);

            Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);
            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnScaleLine_Click(object sender, EventArgs e)
        {

            RvPoint[] ptarr = new RvPoint[3] { new RvPoint(216,102), new RvPoint(372, 299),
                                               new RvPoint(216, 299)  };


            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);


            Smath.ScaleVertex(ptarr, 1.2f, 1.2f);

            Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);
            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnExtendTrim_Click(object sender, EventArgs e)
        {
            RvLineF64 line = new RvLineF64(new RvPointF64(30, 80), new RvPointF64(150, 50));
            RvLineF64 baseln = new RvLineF64(new RvPointF64(130, 30), new RvPointF64(130, 150));
            RvPointF64 outer = new RvPointF64(120, 136);

            RvLineF64 newline1 = Smath.ExtendLine(line, 60);
            RvLineF64 newline2 = Smath.TrimLine(line, baseln, 1);



            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            //move for better view
            newline1.Move(300, 130);
            newline2.Move(0, 130);

            Render.DrawLineE1(m_hContext, (float)newline1.p0.x, (float)newline1.p0.y, (float)newline1.p1.x, (float)newline1.p1.y, IntPtr.Zero);
            Render.DrawLineE1(m_hContext, (float)newline2.p0.x, (float)newline2.p0.y, (float)newline2.p1.x, (float)newline2.p1.y, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            Render.DrawLineE1(m_hContext, (float)line.p0.x, (float)line.p0.y, (float)line.p1.x, (float)line.p1.y, IntPtr.Zero);
            Render.DrawLineE1(m_hContext, (float)baseln.p0.x, (float)baseln.p0.y, (float)baseln.p1.x, (float)baseln.p1.y, IntPtr.Zero);
            Render.DrawCross(m_hContext, (int)outer.x, (int)outer.y, 12, 12, 0, IntPtr.Zero);


            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }


        private void btnRectangle_Click(object sender, EventArgs e)
        {
            RvRect re1 = new RvRect(89, 45, 253, 181);
            RvRect re2 = new RvRect(170, 118, 343, 215);

            RvRect rect1 = Smath.RotateRect(re1, 90);
            RvRect rect2 = Smath.MoveRect(re2, 0, 200);
            RvRect rect3 = Smath.ScaleRect(rect2, 1.2);
            RvRect rect4 = Smath.RectUnion(re1, re2);
            RvRect rect5 = Smath.RectIntersect(re1, re2);


            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            Render.DrawRect(m_hContext, re1.left, re1.top, re1.right, re1.bottom, false, IntPtr.Zero);
            Render.DrawRect(m_hContext, re2.left, re2.top, re2.right, re2.bottom, false, IntPtr.Zero);


            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawRect(m_hContext, rect1.left, rect1.top, rect1.right, rect1.bottom, false, IntPtr.Zero);
            Render.DrawRect(m_hContext, rect2.left, rect2.top, rect2.right, rect2.bottom, false, IntPtr.Zero);
            Render.DrawRect(m_hContext, rect3.left, rect3.top, rect3.right, rect3.bottom, false, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 0, 0));
            Render.DrawRect(m_hContext, rect4.left, rect4.top, rect4.right, rect4.bottom, false, IntPtr.Zero);
            Render.DrawRect(m_hContext, rect5.left, rect5.top, rect5.right, rect5.bottom, false, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);


            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnRectAndVertex_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\heart.jpg");

            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KBlob blob = KBlob.FromBinaryImageCluster(im);

            RvPoint[] arr = blob.GetEdges();
            Pool.Assert(arr != null);

            RvRect rect = Smath.CalcBoundRect(arr);
            RvBox2D box = new RvBox2D();
            //Smath.CalcBoundRect();
            double e1, e2, e3;
            bool b = Smath.ApproxEllipse(arr, out box, out e1, out e2, out e3);
            Pool.Assert(b);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawRect(m_hContext, rect.left, rect.top, rect.right, rect.bottom, false, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(0, 123, 255));

            Render.DrawBox2D(m_hContext, box, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 123, 0));
            Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);


            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnPointInPolygon_Click(object sender, EventArgs e)
        {
            KImage im = new KImage("..\\samples\\triangle.png");

            im.Cast(PixelFormat.Gray);
            Dip.MinError(im.Handle, MinErrorMode.Poisson);

            KBlob blob = KBlob.FromBinaryImageCluster(im);

            RvPoint[] arr = blob.GetEdges();
            Pool.Assert(arr != null);

            Smath.PolylineMove(arr, 100, 100);

            RvPoint pt1 = new RvPoint(178, 173);
            RvPoint pt2 = new RvPoint(129, 140);

            bool b1 = Smath.IsPointInPolygon(pt1, arr);
            bool b2 = Smath.IsPointInPolygon(pt2, arr);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawCross(m_hContext, pt1.x, pt1.y, 12, 12, 0, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 0, 0));
            Render.DrawCross(m_hContext, pt2.x, pt2.y, 12, 12, 0, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 123, 0));
            Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);


        }

        private void btnIntersectAndParallel_Click(object sender, EventArgs e)
        {
            RvLineF64 ln1 = new RvLineF64(new RvPointF64(85, 40), new RvPointF64(247, 166));
            RvLineF64 ln2 = new RvLineF64(new RvPointF64(40, 135), new RvPointF64(260, 48));

            RvLineF64 newline1 = Smath.DeriveParallel(ln1, 35);
            RvLineF64 newline2 = Smath.DerivePerpend(ln2, 120, false, false);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawLineE1(m_hContext, (float)newline1.p0.x, (float)newline1.p0.y, (float)newline1.p1.x, (float)newline1.p1.y, IntPtr.Zero);
            Render.DrawLineE1(m_hContext, (float)newline2.p0.x, (float)newline2.p0.y, (float)newline2.p1.x, (float)newline2.p1.y, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 123, 0));
            Render.DrawLineE1(m_hContext, (float)ln1.p0.x, (float)ln1.p0.y, (float)ln1.p1.x, (float)ln1.p1.y, IntPtr.Zero);
            Render.DrawLineE1(m_hContext, (float)ln2.p0.x, (float)ln2.p0.y, (float)ln2.p1.x, (float)ln2.p1.y, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnOffsetPolygon_Click(object sender, EventArgs e)
        {
            RvPoint[] arr = new RvPoint[5] {
                new RvPoint(67, 94), new RvPoint(179, 39),
                new RvPoint(271, 130), new RvPoint(208, 240),
                new RvPoint(91, 229) };


            RvPoint[] newarr1 = Smath.PolygonOffset(arr, 22);
            RvPoint[] newarr2 = Smath.PolygonOffset(arr, -22);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(255, 0, 0));
            Render.DrawPolygon(m_hContext, newarr1, false, IntPtr.Zero);
            Render.DrawPolygon(m_hContext, newarr2, false, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }

        private void btnOrientaion_Click(object sender, EventArgs e)
        {

        }

        private void btnCaluationOrientation_Click(object sender, EventArgs e)
        {

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            string s;
            //
            RvBool b = Smath.IsClockWise(new RvPoint(561, 223), new RvPoint(681, 333), new RvPoint(547, 388));
            s = "The three points on the same arc are in clock-wise order";
            if (b == RvBool.False)
            {
                s = "The three points on the same arc are in counter clock-wise order";
            }
            Render.MoveTo(m_hContext, 30, 40);
            Render.TextOut(m_hContext, s, 16);

            RvRect re1 = new RvRect(89, 45, 253, 181);
            RvRect re2 = new RvRect(170, 118, 343, 215);

            b = Smath.IsRectSurround(re1, re2);
            s = "The first rect is surrounded by the second rectangle";
            if (b == RvBool.False)
            {
                s = "The first rect is not surrounded by the second rectangle";
            }

            Render.MoveTo(m_hContext, 30, 40 * 2);
            Render.TextOut(m_hContext, s, 16);

            b = Smath.IsPointInsideRect(re1, new RvPoint(145, 105));
            s = "The point is inside the first rectangle ";
            if (b == RvBool.False)
            {
                s = "The point is not inside the first rectangle ";
            }

            Render.MoveTo(m_hContext, 30, 40 * 3);
            Render.TextOut(m_hContext, s, 16);

            b = Smath.IsRightOfLine(new RvLine(new RvPoint(250, 307), new RvPoint(430, 167)), new RvPoint(314, 219));
            s = "The point is on the right of the line";
            if (b == RvBool.False)
            {
                s = "The point is on the left of the line";
            }
            Render.MoveTo(m_hContext, 30, 40 * 4);
            Render.TextOut(m_hContext, s, 16);

            b = Smath.IsPointInsideCircle(new RvPointF64(514, 321), 90, new RvPointF64(506, 292));
            s = "The point is inside  the circle";
            if (b == RvBool.False)
            {
                s = "The point is outside the circle";
            }
            Render.MoveTo(m_hContext, 30, 40 * 5);
            Render.TextOut(m_hContext, s, 16);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnCalcAreaLengthAngle_Click(object sender, EventArgs e)
        {
            RvPointF64[] arr = new RvPointF64[5]
            {
                new RvPointF64(248,142), new RvPointF64(338,67), new RvPointF64(428,144),
                new RvPointF64(393,264), new RvPointF64(282,264)
            };

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            string s;
            double n = Smath.CalcPolygonArea(arr);
            s = $"The area of the polygon is {n}";
            Render.MoveTo(m_hContext, 30, 40);
            Render.TextOut(m_hContext, s, 16);

            n = Smath.CalcPolylineLength(arr, false);
            s = $"The length of the polyline is {n}";
            Render.MoveTo(m_hContext, 30, 40 * 2);
            Render.TextOut(m_hContext, s, 16);

            n = Smath.Calc3PAngle(new RvPointF64(192, 205), new RvPointF64(399, 208), new RvPointF64(389, 76), true);
            s = $"The angle formed by the three points is {n} degrees.";
            Render.MoveTo(m_hContext, 30, 40 * 3);
            Render.TextOut(m_hContext, s, 16);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);



        }

        private void btnCalcDistance_Click(object sender, EventArgs e)
        {
            RvLineF64 line1 = new RvLineF64(new RvPointF64(224, 221), new RvPointF64(666, 128));
            RvLineF64 line2 = new RvLineF64(new RvPointF64(254, 345), new RvPointF64(298, 247));
            RvPointF64 pt1 = new RvPointF64(239, 117);
            RvPointF64 pt2 = new RvPointF64(422, 78);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            string s;
            double n = Smath.DistPointToPoint(pt1, pt2);
            s = $"The distance between two points is {n}";
            Render.MoveTo(m_hContext, 30, 40);
            Render.TextOut(m_hContext, s, 16);

            n = Smath.DistPointToLine(pt1, line1);
            s = $"The distance between point 1 and line 1 is {n}";
            Render.MoveTo(m_hContext, 30, 40 * 2);
            Render.TextOut(m_hContext, s, 16);

            n = Smath.DistLineToLine(line1, line2);
            s = $"The distance between two lines is {n}.";
            Render.MoveTo(m_hContext, 30, 40 * 3);
            Render.TextOut(m_hContext, s, 16);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnGeomExtract_Click(object sender, EventArgs e)
        {
            RvPointF64[] polygon = new RvPointF64[5]
            {
                new RvPointF64(248,142), new RvPointF64(338,67), new RvPointF64(428,144),
                new RvPointF64(393,264), new RvPointF64(282,264)
            };

            RvLineF64 line = new RvLineF64(new RvPointF64(81, 377), new RvPointF64(378, 484));
            RvPointF64 outer = new RvPointF64(170, 313);
            RvPointF64 perd;
            bool b = Smath.CalcPerpendicularPoint(line, outer, out perd);
            Pool.Assert(b);

            RvPointF64 center = Smath.CalcPolygonCenter(polygon);
            RvPointF64 mid = Smath.DeriveMidPoint(line.p0, line.p1);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);



            Render.DrawLineE1(m_hContext, (float)line.p0.x, (float)line.p0.y, (float)line.p1.x, (float)line.p1.y, IntPtr.Zero);
            Render.DrawPolygon(m_hContext, polygon, false, IntPtr.Zero);
            Render.DrawCross(m_hContext, (int)outer.x, (int)outer.y, 12, 12, 0, IntPtr.Zero);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawCross(m_hContext, (int)perd.x, (int)perd.y, 12, 12, 0, IntPtr.Zero);
            Render.DrawCross(m_hContext, (int)center.x, (int)center.y, 12, 12, 0, IntPtr.Zero);
            Render.DrawCross(m_hContext, (int)mid.x, (int)mid.y, 12, 12, 0, IntPtr.Zero);

            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);

            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);



        }

        private void btnPixelStrength_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            RvRgb rgb = new RvRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            int width = rand.Next(4094);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            string s;
            int n = Smath.CalcPitch(Smath.CalcDepth(PixelFormat.BGR), width);
            s = $"The number of bytes in one row of a BGR image with width {width} is {n}.";
            Render.MoveTo(m_hContext, 30, 40);
            Render.TextOut(m_hContext, s, 16);

            RvRgb brigher = Smath.GetBrighterPixel(rgb, 0.3f);

            s = $"A pixel with red = {rgb.red },  green = {rgb.green}, blue = {rgb.blue} becomes ({brigher.red}, {brigher.green}, {brigher.blue }) after brightening";
            Render.MoveTo(m_hContext, 30, 40 * 2);
            Render.TextOut(m_hContext, s, 16);

            n = Smath.CalcPixelStrength(rgb, ColorSpace.HSV);
            s = $"The strength of a pixel with red {rgb.red }, green {rgb.green}, blue {rgb.blue}";
            Render.MoveTo(m_hContext, 30, 40 * 3);
            Render.TextOut(m_hContext, s, 16);

            s = $"is {n} in the color space HSV.";
            Render.MoveTo(m_hContext, 30, 40 * 3 + 17);
            Render.TextOut(m_hContext, s, 16);

            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);

        }

        private void btnAdaptRect_Click(object sender, EventArgs e)
        {


            double ratio = 0;
            RvRect container = new RvRect(new RvPoint(117, 187), new RvPoint(476, 386));
            RvRect insideRect = Smath.AdaptRect(container, 380, 340, ref ratio);

            //Clear canvas (optional)
            Render.Clear(m_hContext, CanvasLayer.All);

            IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);
            IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

            Render.DrawRect(m_hContext, container.left, container.top, container.right, container.bottom, false, IntPtr.Zero);

            Render.SetPenColor(hPen, new GRgb(0, 123, 255));

            Render.DrawRect(m_hContext, insideRect.left, insideRect.top, insideRect.right, insideRect.bottom, false, IntPtr.Zero);


            Render.SelectPen(m_hContext, hOldPen);
            Render.DestroyPen(hPen);


            //refresh
            Render.Realize(m_hContext, CanvasLayer.All);
            Render.Flush(m_hContext);
        }
    }
}


