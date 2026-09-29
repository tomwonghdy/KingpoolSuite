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
using Kingpool.Xgui;


namespace Kingpool.Vision
{
    public struct MatchResult
    {
        public float score;          //相似度分数
        public float cx, cy;         //中心点坐标
        public float angle;          //旋转的角度
    }

    public class KCaliber : KUcobj
    {
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberCreate(string strClassName);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberDestroy(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberShot(IntPtr hCaliber, /*int mode,*/ IntPtr image);
        [DllImport("UniVision.dll")]
        private static extern bool uniCaliberInspect(IntPtr hCaliber);
        [DllImport("UniVision.dll")]

        private static extern bool uniCaliberSerialize(IntPtr hCaliber, IntPtr hDisk, bool bIn);
        [DllImport("UniVision.dll")]
        private static extern int uniCaliberShowParamWin(IntPtr hCaliber, IntPtr image);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetReading(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetSearchRegion(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetPatternRegion(IntPtr hCaliber);

        [DllImport("UniVision.dll")]
        private static extern void uniCaliberReset(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetImage(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetMask(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberGetBinary(IntPtr hCaliber);

        [DllImport("UniVision.dll")]
        private static extern bool uniCaliberGetResult(IntPtr hCaliber, ref bool pResult);
        [DllImport("UniVision.dll")]
        private static extern bool uniCaliberGetPos(IntPtr hCaliber, ref RvPoint pResult);
        [DllImport("UniVision.dll")]
        private static extern bool uniCaliberGetPosArray(IntPtr hCaliber, IntPtr pArrOut, int arrSize);
        [DllImport("UniVision.dll")]
        private static extern bool uniCaliberGetArraySize(IntPtr hCaliber, ref uint pArrSize);

        ~KCaliber()
        {
            Destroy();
        }

        //public IntPtr GetHandle() { return m_hWidget; }

        public override bool Create(string strClassName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
                // throw new Exception("Object exist already!");
            }

            m_hWidget = uniCaliberCreate(strClassName);
            return (m_hWidget != IntPtr.Zero);
        }

        public override void Destroy()
        {
            if (m_bAttached) return;

            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberDestroy(m_hWidget);
                m_hWidget = IntPtr.Zero;
            }
        }



        public void Shot(/*int mode,*/ IntPtr image)
        {
            if (image == IntPtr.Zero) return;

            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberShot(m_hWidget, /*mode,*/ image);
            }
        }

        public static void Shot(IntPtr hCaliber, /*int mode,*/ IntPtr image)
        {
            if (image == IntPtr.Zero) return;

            if (hCaliber != IntPtr.Zero)
            {
                uniCaliberShot(hCaliber, /*mode,*/ image);
            }
        }

        public bool Inspect()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberInspect(m_hWidget);
            }
            return false;
        }

        public static bool Inspect(IntPtr hCaliber)
        {
            if (hCaliber != IntPtr.Zero)
            {
                return uniCaliberInspect(hCaliber);
            }
            return false;
        }

        //public bool Serialize(IntPtr hDisk, bool bIn)
        //{
        //    if (m_hWidget != IntPtr.Zero)
        //    {
        //        return uniCaliberSerialize(m_hWidget, hDisk, bIn);
        //    }
        //    return false;
        //}

        public int ShowParamWin(IntPtr image)
        {
            if (image == IntPtr.Zero) return 0;

            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberShowParamWin(m_hWidget, image);
            }
            return 0;
        }

        public IntPtr GetReading()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetReading(m_hWidget);
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetReading(IntPtr hCaliber)
        {
            if (hCaliber != IntPtr.Zero)
            {
                return uniCaliberGetReading(hCaliber);
            }
            return IntPtr.Zero;
        }


        public IntPtr GetSearchRegion()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetSearchRegion(m_hWidget);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetPatternRegion()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetPatternRegion(m_hWidget);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetImage()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetImage(m_hWidget);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetMask()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetMask(m_hWidget);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetBinary()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetBinary(m_hWidget);
            }
            return IntPtr.Zero;

        }

        public void Reset()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberReset(m_hWidget);
            }
        }

        public bool GetResult(ref bool result)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetResult(m_hWidget, ref result);
            }
            return false;
        }

        public bool GetResult(ref RvPoint pResult)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetPos(m_hWidget, ref pResult);
            }
            return false;

        }
        public bool GetResult(ref RvPoint[] arr)
        {
            if (arr == null)
            {
                return false;
            }

            if (m_hWidget != IntPtr.Zero)
            {
                int cnt = arr.Length;
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPoint)) * cnt);

                bool ret = uniCaliberGetPosArray(m_hWidget, ptr, cnt);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPoint)));
                    arr[i] = (RvPoint)Marshal.PtrToStructure(ptrtmp, typeof(RvPoint));
                }

                Marshal.FreeHGlobal(ptr);

                return ret;
            }
            return false;
        }

        public bool GetResult(ref uint nArrSize)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetArraySize(m_hWidget, ref nArrSize);
            }
            return false;
        }

    }

    public class TemplateMatch : KCaliber
    {
        public const string TEMPLATE_MATCH = "TemplateMatch";

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCaliberCreate(string strClassName);

        ~TemplateMatch()
        {
            Destroy();
        }

        public override bool Create(string strClassName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
                // throw new Exception("Object exist already!");
            }

            //   m_hWidget = uniCaliberCreate(strClassName);
            return (m_hWidget != IntPtr.Zero);
        }

        public override void Destroy()
        {
            if (m_bAttached) return;

            if (m_hWidget != IntPtr.Zero)
            {
                //   uniCaliberDestroy(m_hWidget);
                m_hWidget = IntPtr.Zero;
            }
        }
    }

    public class MatchCaliber : KCaliber
    {
        public const string MATCH_CALIBER_NAME = "MatchCaliber";

        public const int DT_SEARCH_ROI = (1 << 14);
        public const int DT_PATTERN_ROI = (1 << 15);
        public const int DT_POSTIONS = (1 << 16);


        [DllImport("UniVision.dll")]
        private static extern int uniCaliberGetMatchResult(IntPtr hCaliber, IntPtr pMatchArray, int arraySize);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberSetMinScore(IntPtr hCaliber, float score);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberSetMaxInstanceCount(IntPtr hCaliber, int count);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberSetCoarseLevel(IntPtr hCaliber, int level);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberSetFineLevel(IntPtr hCaliber, int level);
        [DllImport("UniVision.dll")]
        private static extern void uniCaliberEnableRotation(IntPtr hCaliber, bool flag);
        [DllImport("UniVision.dll")]
        private static extern bool uniMatchCalSetCustomMatcher(IntPtr hCaliber, string strLibName);
        [DllImport("UniVision.dll")]
        private static extern bool uniMatchCalLearn(IntPtr hCaliber, IntPtr image, IntPtr mask/*, int left, int top*/);
        [DllImport("UniVision.dll")]
        private static extern int uniMatchCalRecognize(IntPtr hCaliber, IntPtr image, IntPtr mask, bool bSubpixel, double overlap, bool bCoarseMatchOnly, int maxCount, double minScore/*, IntPtr reading*/);
        [DllImport("UniVision.dll")]
        private static extern bool uniMatchCalIsPatternLearned(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern int uniMatchCalLocate(IntPtr hCaliber, IntPtr image, IntPtr mask, ref float minScore, ref RvPointF32 result);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalShot(IntPtr hCaliber, IntPtr image, int roi);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalSetPatternRect(IntPtr hCaliber, RvRect rect);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalSetSearchRect(IntPtr hCaliber, RvRect rect);
        [DllImport("UniVision.dll")]
        private static extern bool uniMatchCalCopyImage(IntPtr hCaliber, IntPtr image, int roi);
        [DllImport("UniVision.dll")]
        private static extern float uniCaliberGetMinScore(IntPtr hCaliber);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalSetMinusAngleToler(IntPtr hCaliber, double toler);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalSetPlusAngleToler(IntPtr hCaliber, double toler);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalDumpPattern(IntPtr hCaliber, string strFolder);
        [DllImport("UniVision.dll")]
        private static extern void uniMatchCalSetOptionFile(IntPtr hCaliber, string strFilePath);


        public int GetMatchResult(MatchResult[] matchArray)
        {
            if (matchArray == null) return 0;

            if (m_hWidget != IntPtr.Zero)
            {
                int cnt = matchArray.Length;
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MatchResult)) * cnt);

                cnt = uniCaliberGetMatchResult(m_hWidget, ptr, cnt);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(MatchResult)));
                    matchArray[i] = (MatchResult)Marshal.PtrToStructure(ptrtmp, typeof(MatchResult));
                }

                Marshal.FreeHGlobal(ptr);

                return cnt;
            }

            return 0;
        }
        public void SetMinScore(float score)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberSetMinScore(m_hWidget, score);
            }
        }

        public float GetMinScore()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCaliberGetMinScore(m_hWidget);
            }

            return 0;
        }

        public void SetMinusAngleToler(double toler)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniMatchCalSetMinusAngleToler(m_hWidget, toler);
            }
        }
        public void SetPlusAngleToler(double toler)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniMatchCalSetPlusAngleToler(m_hWidget, toler);
            }
        }

        public void DumpPattern(string strFolder)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniMatchCalDumpPattern(m_hWidget, strFolder);
            }
        }

        public void SetOptionFile(string strFilePath)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniMatchCalSetOptionFile(m_hWidget, strFilePath);
            }
        }


        public void SetFineLevel(int level)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberSetFineLevel(m_hWidget, level);
            }
        }
        public void SetCoarseLevel(int level)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberSetCoarseLevel(m_hWidget, level);
            }
        }
        public void EnableRotation(bool flag)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberEnableRotation(m_hWidget, flag);
            }
        }
        public void SetMaxInstanceCount(int count)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCaliberSetMaxInstanceCount(m_hWidget, count);
            }
        }

        public bool SetCustomMatcher(string strLibName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniMatchCalSetCustomMatcher(m_hWidget, strLibName);
            }
            return false;
        }

        public bool IsPatternLearned()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniMatchCalIsPatternLearned(m_hWidget);
            }

            return false;
        }

        public bool Learn(IntPtr hImage, IntPtr hMask /*, int left, int top*/)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniMatchCalLearn(m_hWidget, hImage, hMask/*,   left,   top*/);
            }
            return false;
        }
        public bool Learn(KImage image, KMask mask /*, int left, int top*/)
        {
            return Learn(image.Handle, mask.Handle/*, left, top*/);
        }

        public int Recognize(IntPtr image, IntPtr mask, bool bSubpixel, double overlap, bool bCoarseMatchOnly, int maxCount, double minScore/*, IntPtr reading*/)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniMatchCalRecognize(m_hWidget, image, mask, bSubpixel, overlap, bCoarseMatchOnly, maxCount, minScore/*,  reading*/);
            }
            return 0;
        }

        public int Recognize(KImage image, KMask mask, bool bSubpixel, double overlap, bool bCoarseMatchOnly, int maxCount, double minScore/*, IntPtr reading*/)
        {
            if (null == image) return 0;

            if (mask != null)
            {
                return Recognize(image.Handle, mask.Handle, bSubpixel, overlap, false, maxCount, minScore/*, IntPtr.Zero*/);
            }
            else
            {
                return Recognize(image.Handle, IntPtr.Zero, bSubpixel, overlap, false, maxCount, minScore/*, IntPtr.Zero*/);
            }
        }

        //获取当个实例
        public bool Locate(KImage image, KMask mask, ref float minScore, out RvPointF32 result)
        {
            result = new RvPointF32();
            if (m_hWidget == IntPtr.Zero) return false;


            IntPtr h = image == null ? IntPtr.Zero : image.Handle;
            IntPtr hMsk = mask == null ? IntPtr.Zero : mask.Handle;

            int n = uniMatchCalLocate(m_hWidget, h, hMsk, ref minScore, ref result);

            return n > 0;
        }

        public void Shot(KImage image, int roi)
        {
            if (m_hWidget == IntPtr.Zero) return;

            uniMatchCalShot(m_hWidget, image.Handle, roi);
        }

        public void SetPatternRect(RvRect rect)
        {
            if (m_hWidget == IntPtr.Zero) return;
            uniMatchCalSetPatternRect(m_hWidget, rect);
        }
        public void SetSearchRect(RvRect rect)
        {
            if (m_hWidget == IntPtr.Zero) return;
            uniMatchCalSetSearchRect(m_hWidget, rect);
        }

        public bool CopyImage(KImage image, int roi)
        {
            if (image == null || image.Handle == IntPtr.Zero) return false;

            if (m_hWidget == IntPtr.Zero) return false;

            return uniMatchCalCopyImage(m_hWidget, image.Handle, roi);

        }


    }


    public class PixelMatch : MatchCaliber
    {
        public const string PIXEL_MATCH_NAME = "PixelMatch";



        ~PixelMatch()
        {
            Destroy();
        }

        //public IntPtr GetHandle() { return m_hWidget; }

        public override bool Create(string strClassName = PIXEL_MATCH_NAME)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
                // throw new Exception("Object exist already!");
            }

            if (PIXEL_MATCH_NAME != strClassName)
            {
                return false;
            }

            //   m_hWidget = uniCaliberCreate(strClassName);
            return (m_hWidget != IntPtr.Zero);
        }

        public override void Destroy()
        {
            if (m_bAttached) return;

            if (m_hWidget != IntPtr.Zero)
            {
                // uniCaliberDestroy(m_hWidget);
                m_hWidget = IntPtr.Zero;
            }
        }

    }

    public class EdegeMatch : MatchCaliber
    {

    }

    public class ShapeMatch : MatchCaliber
    {

    }
}
