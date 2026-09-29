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
    public class KSpatialCorrMapper
    {
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr uniCreateRobotSpaceMap();
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniDestroyRobotSpaceMap(IntPtr hRsm);

        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]

        public static extern double uniRsmGetScale(IntPtr hRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmIsLearned(IntPtr hRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniRsmReset(IntPtr hRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern RvPointF64 uniRsmGetCoord(IntPtr hRsm, double x, double y, bool bAbsolute, bool bReverse);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmLearn(IntPtr hRsm, IntPtr pCtcpImage, IntPtr pCtcpMach, int count, bool bReverseLearn);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmCopyFrom(IntPtr hRsm, IntPtr hSrcRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmCopyFromArray(IntPtr hRsm, IntPtr pParaArr, int paraCount, bool bReverse);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmIsReverseLearned(IntPtr hRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniRsmSerialze(IntPtr hRsm, IntPtr hFile, bool bIn);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern RvPointF64 uniRsmGetRefPos(IntPtr hRsm, bool bImagePos);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniRsmSetRefPos(IntPtr hRsm, RvPointF64 refPos, bool bImagePos);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern int uniRsmGetMatrixElemCount(IntPtr hRsm);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern double uniRsmGetMatrixElem(IntPtr hRsm, int index, bool bReverse);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr uniRsmGetMatrix(IntPtr hRsm, bool bReverse);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniReadMobileW2cParams(string strParamFile, IntPtr hRsm, ref uint pId, ref RvPointF64 pCaliPos);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniWriteMobileW2cParams(string strParamFile, IntPtr hRsm, uint id, RvPointF64 caliPos);

        private IntPtr m_hRsm = IntPtr.Zero;
        private uint m_id = 0;
        private RvPointF64 m_caliPos = new RvPointF64();

        public KSpatialCorrMapper()
        {
            bool ret = Create();
            Pool.Assert(ret);
        }

        ~KSpatialCorrMapper()
        {
            Destroy();
        }

        public bool Create()
        {
            if (m_hRsm != IntPtr.Zero) return false;

            m_hRsm = uniCreateRobotSpaceMap();

            return m_hRsm != IntPtr.Zero;
        }

        public void Destroy()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                uniDestroyRobotSpaceMap(m_hRsm);
                m_hRsm = IntPtr.Zero;
            }
        }

        public IntPtr GetHandle()
        {
            return m_hRsm;
        }

        public bool ReadFromFile(string strFilePath)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniReadMobileW2cParams(strFilePath, m_hRsm, ref m_id, ref m_caliPos);
            }

            return false;
        }

        public bool WriteToFile(string strFilePath, uint id, double caliX, double caliY)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                RvPointF64 pt = new RvPointF64(caliX, caliY);

                if (!uniWriteMobileW2cParams(strFilePath, m_hRsm, id, pt))
                {
                    return false;
                }

                m_id = id;
                m_caliPos = pt;
                return true;
            }

            return false;
        }


        public double GetScale()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmGetScale(m_hRsm);
            }
            return 0;
        }

        public bool IsLearned()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmIsLearned(m_hRsm);
            }
            return false;
        }

        public void Reset()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                uniRsmReset(m_hRsm);
            }
        }

        public RvPointF64 GetCaliPos()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return GetRefPos(false);
            }
            return new RvPointF64();
        }

        public RvPointF64 GetCoord(double x, double y, bool bAbsolute = true, bool bReverse = false)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmGetCoord(m_hRsm, x, y, bAbsolute, bReverse);
            }
            return new RvPointF64();
        }

        public bool Learn(RvPointF32[] imagePointArray, RvPointF64[] machPointArray, int maxCount, bool bReverseLearn)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                IntPtr pCtcpImage = IntPtr.Zero;
                IntPtr pCtcpMach = IntPtr.Zero;

                int count = imagePointArray.Length;
                if (imagePointArray.Length != machPointArray.Length) return false;

                RvPointF64[] imgarr64 = new RvPointF64[count];
                for (int i = 0; i < count; i++)
                {
                    imgarr64[i].x = imagePointArray[i].x;
                    imgarr64[i].y = imagePointArray[i].y;
                }

                pCtcpImage = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF64)) * count);
                for (int i = 0; i < count; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(pCtcpImage + i * Marshal.SizeOf(typeof(RvPointF64)));
                    Marshal.StructureToPtr(imgarr64[i], ptrtmp, false);
                }


                pCtcpMach = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF64)) * count);
                for (int i = 0; i < count; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(pCtcpMach + i * Marshal.SizeOf(typeof(RvPointF64)));
                    Marshal.StructureToPtr(machPointArray[i], ptrtmp, false);
                }

                if (maxCount <= 0) maxCount = count;

                bool ret = uniRsmLearn(m_hRsm, pCtcpImage, pCtcpMach, maxCount, bReverseLearn);

                Marshal.FreeHGlobal(pCtcpImage);
                Marshal.FreeHGlobal(pCtcpMach);


                return ret;

            }
            return false;
        }

        public bool CopyFrom(IntPtr hSrcRsm)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmCopyFrom(m_hRsm, hSrcRsm);
            }
            return false;
        }

        public bool CopyFromArray(double[] paraArr, bool bReverse)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                IntPtr hParaArr = IntPtr.Zero;
                int cnt = paraArr.Length;
                Pool.Assert(false);

                return uniRsmCopyFromArray(m_hRsm, hParaArr, cnt, bReverse);
            }
            return false;
        }

        public bool IsReverseLearned()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmIsReverseLearned(m_hRsm);
            }
            return false;
        }

        public bool Serialze(IntPtr hFile, bool bIn)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmSerialze(m_hRsm, hFile, bIn);
            }
            return false;
        }

        public bool Serialze(DiskHelper diskHelper, bool bIn)
        {
            return Serialze(diskHelper.GetHandle(), bIn);
        }

        public RvPointF64 GetRefPos(bool bImagePos)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmGetRefPos(m_hRsm, bImagePos);
            }
            return new RvPointF64();
        }

        public void SetRefPos(RvPointF64 refPos, bool bImagePos)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                uniRsmSetRefPos(m_hRsm, refPos, bImagePos);
            }
        }

        public int GetMatrixElemCount()
        {
            if (m_hRsm != IntPtr.Zero)
            {
                uniRsmGetMatrixElemCount(m_hRsm);
            }
            return 0;
        }

        public double GetMatrixElemAt(int index, bool bReverse)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                uniRsmGetMatrixElem(m_hRsm, index, bReverse);
            }
            return 0;
        }

        public IntPtr GetMatrixPtr(bool bReverse)
        {
            if (m_hRsm != IntPtr.Zero)
            {
                return uniRsmGetMatrix(m_hRsm, bReverse);
            }
            return IntPtr.Zero;
        }

    }
}
