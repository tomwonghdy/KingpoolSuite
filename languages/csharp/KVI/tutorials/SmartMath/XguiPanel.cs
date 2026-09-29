using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartMath
{
    public partial class XguiPanel : Control
    {
        public XguiPanel()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            //              ControlStyles.UserPaint |
            //              ControlStyles.DoubleBuffer |
            //              ControlStyles.ResizeRedraw, true);
            //this.UpdateStyles();

            SetStyle(ControlStyles.AllPaintingInWmPaint |   // 在 WM_PAINT 中绘制所有
                     ControlStyles.Opaque,                  // 控件不透明，跳过背景擦除
                     true);
            SetStyle(ControlStyles.UserPaint, true);        // 用户自定义绘制（但我们实际没画）
                                                            //  SetStyle(ControlStyles.DoubleBuffer, false);    // 禁用双缓冲（由用户自己决定）
            UpdateStyles();

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.DesignMode || this.Handle == IntPtr.Zero) return;

            // base.OnPaint(pe);
        }
    }
}
