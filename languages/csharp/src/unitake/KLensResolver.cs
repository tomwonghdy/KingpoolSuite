//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


using Kingpool.Utility;
using Kingpool.Core;

namespace Kingpool.Vision
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UniLrExtrinsic
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = CAP.LR_RVEC_SIZE)]
        public double[] rvec/*[UNI_RVEC_SIZE]*/; //旋转系数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = CAP.LR_TVEC_SIZE)]
        public double[] tvec/*[UNI_TVEC_SIZE]*/; //平移系数
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UniBoardDesc
    {
        public uint id;
        public int cols;
        public int rows;
        public int oriX, oriY; //原点位置(图像)
        public int imW, imH;   //图像尺寸
        public float spacing;    //输入
        public float scale;      //输出
    };

    public class KLensResolver
    {
        //there must be more than 25 dots on the calibration board
        public const int MIN_DOT_COUNT = 25;

        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr uniCreateLensResolver();
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern void uniDestroyLensResolver(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern bool uniLensResolverLearn(IntPtr hLensResolver, double thres, ref double pRms);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        //  const KImage image,  UNI_BOARD_DESC * boardDesc, RvPoint_f64* pImagePosArray, int maxCount
        private static extern int uniLensResolverExtract(IntPtr hLensResolver, IntPtr image, bool bDarkDot, bool bAppend, ref UniBoardDesc boardDesc, IntPtr/* RvPoint_f64**/ hImagePosArray, int maxCount);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]

        private static extern bool uniLensResolverSerialize(IntPtr hLensResolver, IntPtr hFile, bool bReadOnly);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]

        private static extern bool uniLensResolverIsLearned(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern bool uniLensResolverCorrect(IntPtr hLensResolver, IntPtr image);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern double uniLensResolverGetScale(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr /*const double* */  uniLensResolverGetIntrinsicMatrix(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr/*const double**/   uniLensResolverGetDistortMatrix(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern int uniLensResolverGetExtrinsicCount(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr/*const UNI_LR_EXTRINSIC**/   uniLensResolverGetExtrinsAt(IntPtr hLensResolver, int index);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern void uniLensResolverReset(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern bool uniLensGenUndistorter(IntPtr hLensResolver, double alpha, IntPtr/*const KImage*/ imDistort, IntPtr/*const KImage*/ imUndist);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern int uniLensGetSampleCount(IntPtr hLensResolver);
        [DllImport("UniVision.dll", CharSet = CharSet.Ansi)]
        private static extern bool uniLensGenUndistorterE1(IntPtr hLensResolver, double alpha, int type, bool bDeleteExist);

        private IntPtr m_hResolver = IntPtr.Zero;

        public KLensResolver()
        {
            m_hResolver = uniCreateLensResolver();
            Pool.Assert(m_hResolver != IntPtr.Zero);
        }

        ~KLensResolver()
        {
            if (m_hResolver != IntPtr.Zero)
            {
                uniDestroyLensResolver(m_hResolver);
                m_hResolver = IntPtr.Zero;
            }
        }

        public bool Learn(double thres, ref double pRms)
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverLearn(m_hResolver, thres, ref pRms);
            }

            return false;
        }

        public int Extract(KImage image, bool bDarkDot, bool bAppend, int maxDotCount, ref UniBoardDesc boardDesc, out RvPointF64[] pDotArray)
        {

            pDotArray = null;

            if (image == null) return 0;
            if (maxDotCount < MIN_DOT_COUNT) return 0;

            if (m_hResolver != IntPtr.Zero)
            {
                int cnt = maxDotCount;
                IntPtr h = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF64)) * cnt);

                int n = uniLensResolverExtract(m_hResolver, image.GetHandle(), bDarkDot, bAppend, ref boardDesc, h, cnt);

                if (n > 0)
                {
                    pDotArray = new RvPointF64[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        IntPtr ptrtmp = (IntPtr)(h + i * Marshal.SizeOf(typeof(RvPointF64)));
                        pDotArray[i] = (RvPointF64)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF64));
                    }
                }

                Marshal.FreeHGlobal(h);
                return n;
            }

            return 0;
        }


        public bool Serialize(DiskHelper diskHelper, bool bReadOnly)
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverSerialize(m_hResolver, diskHelper.GetHandle(), bReadOnly);
            }

            return false;
        }
        public bool IsLearned()
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverIsLearned(m_hResolver);
            }

            return false;
        }


        public bool Correct(KImage image)
        {
            if (null == image) return false;

            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverCorrect(m_hResolver, image.GetHandle());
            }

            return false;
        }

        public double GetScale(KImage image)
        {
            if (null == image) return 0;

            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverGetScale(m_hResolver);
            }

            return 0;
        }

        public double[] GetIntrinsicMatrix()
        {

            if (m_hResolver != IntPtr.Zero)
            {
                IntPtr h = uniLensResolverGetIntrinsicMatrix(m_hResolver);
                if (h != IntPtr.Zero)
                {
                    int sze = 3 * 3;
                    double[] arr = new double[sze];
                    Marshal.Copy(h, arr, 0, sze);
                    return arr;
                }
            }

            return null;
        }

        public double[] GetDistortMatrix()
        {

            if (m_hResolver != IntPtr.Zero)
            {
                IntPtr h = uniLensResolverGetDistortMatrix(m_hResolver);
                if (h != IntPtr.Zero)
                {
                    int sze = 5 * 1;
                    double[] arr = new double[sze];
                    Marshal.Copy(h, arr, 0, sze);
                    return arr;
                }
            }

            return null;
        }

        public int GetExtrinsicCount()
        {

            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensResolverGetExtrinsicCount(m_hResolver);
            }

            return 0;
        }

        public UniLrExtrinsic? GetExtrinsAt(int index)
        {

            if (m_hResolver != IntPtr.Zero)
            {
                IntPtr h = uniLensResolverGetExtrinsAt(m_hResolver, index);
                if (h != IntPtr.Zero)
                {
                    return (UniLrExtrinsic)Marshal.PtrToStructure(h, typeof(UniLrExtrinsic));
                }
            }

            return null;
        }

        public void Reset()
        {
            if (m_hResolver != IntPtr.Zero)
            {
                uniLensResolverReset(m_hResolver);
            }

        }


        public bool GenUndistorter(double alpha, int type, bool bDeleteExist)
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensGenUndistorterE1(m_hResolver, alpha, type, bDeleteExist);
            }

            return false;
        }
        public bool GenUndistorter(double alpha, KImage imDistort, KImage imUndist)
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensGenUndistorter(m_hResolver, alpha,
                    (null == imDistort ? IntPtr.Zero : imDistort.GetHandle()),
                    (null == imUndist ? IntPtr.Zero : imUndist.GetHandle()));
            }

            return false;
        }

        public bool GenUndistorter()
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensGenUndistorter(m_hResolver, 0, IntPtr.Zero, IntPtr.Zero);
            }

            return false;
        }

        public bool GenUndistorter(double alpha)
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensGenUndistorter(m_hResolver, alpha, IntPtr.Zero, IntPtr.Zero);
            }

            return false;
        }

        public int GetSampleCount()
        {
            if (m_hResolver != IntPtr.Zero)
            {
                return uniLensGetSampleCount(m_hResolver);
            }

            return 0;
        }

    }
}
