//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Kingpool.Core;

namespace Kingpool.Vision
{
    public delegate void FrameArrivalDelegate(IntPtr hCamera, IntPtr pData, UInt64 size, IntPtr pUserData); //声明委托

    public class KCamera
    {
        public const int TRIG_NONE = -1;
        public const int TRIG_SOFT = 0;
        public const int TRIG_WIRE = 1;

        //翻转
        public const int FLIP_UNK = 0;
        public const int FLIP_HORI = 1;       //水平
        public const int FLIP_VERT = (1 << 1);  //垂直
        public const int FLIP_BOTH = (FLIP_HORI | FLIP_VERT);  //双向

        //参数名称
        public const string PNAME_EXPOSURE = "exposure";
        public const string PNAME_GAIN = "gain";
        public const string PNAME_BRIGHT = "bright";
        public const string PNAME_FLIP = "flip";
        public const string PNAME_WIDTH = "width";
        public const string PNAME_HEIGHT = "height";









        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraIsValid(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetCurIndex(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetDeviceCount(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetDeviceName(IntPtr hCamera, int index, StringBuilder val, int size);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraSetDeviceName(IntPtr hCamera, int index, string strName);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCameraGetHandle(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateCamera(string strModel);

        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyCamera(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraStart(IntPtr hCamera, int nTriggerType);

        [DllImport("UniDevice.dll")]
        private static extern void uniCameraStop(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraIsStarted(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraOpenE1(IntPtr hCamera, string strName);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraOpen(IntPtr hCamera, int index);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraReopen(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern void uniCameraClose(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern void uniCameraCloseE1(IntPtr hCamera, bool bThrough);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraIsOpened(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraTrigger(IntPtr hCamera, int timeout);

        //[DllImport("UniDevice.dll")]
        //private static extern bool uniCameraWaitCam(IntPtr hCamera, IntPtr image, int timeout);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraFetchImage(IntPtr hCamera, IntPtr hImage, int timeout);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetDepth(IntPtr hCamera, out int pValue);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetWidth(IntPtr hCamera, out int pValue);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetHeight(IntPtr hCamera, out int pValue);

        //private delegate void UNI_FRAME_ARRIVAL_FUNC(IntPtr pUserData);

        [DllImport("UniDevice.dll")]
        private static extern void uniCameraSetFrameArrivalCallback(IntPtr hCamera, FrameArrivalDelegate pfnFrameArrival, IntPtr pUserData);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCameraGetFrameArrivalUserData(IntPtr hCamera);
        [DllImport("UniDevice.dll")]
        private static extern FrameArrivalDelegate uniCameraGetFrameArrivalCallback(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetTriggerType(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetRotateType(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern void uniCameraSetRotateType(IntPtr hCamera, int type);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraSetParam(IntPtr hCamera, string strName, int value);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParam(IntPtr hCamera, string strName, out int pValue);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamMax(IntPtr hCamera, string strName, out int pValue);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamMin(IntPtr hCamera, string strName, out int pValue);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraSetRoi(IntPtr hCamera, int offsetx, int offsety, int width, int height);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraSetResolution(IntPtr hCamera, int width, int height);

        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraAutoBalanceWhite(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetSerialNum(IntPtr hCamera, StringBuilder strOut, int size);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetSerialNumAtIndex(IntPtr hCamera, int index, StringBuilder strOut, int size);

        [DllImport("UniDevice.dll")]
        private static extern int uniCameraGetLastError(IntPtr hCamera);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCameraGetErrorDesc(IntPtr hCamera, int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamDelta(IntPtr hCamera, string strName, ref int pValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamF(IntPtr hCamera, string strName, ref double pValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamDeltaF(IntPtr hCamera, string strName, ref double pValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamMaxF(IntPtr hCamera, string strName, ref double pValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraGetParamMinF(IntPtr hCamera, string strName, ref double pValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniCameraSetParamF(IntPtr hCamera, string strName, double value);


        private IntPtr m_hCamera = IntPtr.Zero;



        ~KCamera()
        {
            Destroy();
        }

        public bool IsCamValid()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraIsValid(m_hCamera);
            }
            return false;
        }

        public int GetCurIndex()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetCurIndex(m_hCamera);
            }
            return -1;
        }

        public int GetDeviceCount()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetDeviceCount(m_hCamera);
            }
            return -1;
        }

        public static int GetDeviceCount(string strModule, ref string[] names)
        {
            IntPtr h = uniCreateCamera(strModule);
            if (h == IntPtr.Zero)
            {
                return 0;
            }

            int cnt = uniCameraGetDeviceCount(h);

            int manlen = 127;
            StringBuilder sb = new StringBuilder(manlen);


            for (int i = 0; i < cnt; i++)
            {
                int n = uniCameraGetDeviceName(h, i, sb, manlen);

                if (i < names.Length)
                {
                    if (n > 0)
                    {
                        names[i] = sb.ToString();
                    }
                    else
                    {
                        names[i] = "noname";
                    }

                }
            }

            uniDestroyCamera(h);

            return cnt;
        }

        public string GetDeviceName(int index)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                int manlen = 127;
                StringBuilder sb = new StringBuilder(manlen);

                int n = uniCameraGetDeviceName(m_hCamera, index, sb, manlen);
                return sb.ToString();
            }
            return string.Empty;
        }

        public bool SetDeviceName(int index, string strName)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraSetDeviceName(m_hCamera, index, strName);
            }
            return false;
        }

        public IntPtr GetHandle()
        {
            return m_hCamera;
        }

        public bool Create(string strModel)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hCamera = uniCreateCamera(strModel);

            return (IntPtr.Zero != m_hCamera);
        }

        public void Destroy()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                if (IsStarted())
                {
                    Stop();
                }

                if (IsOpened())
                {
                    Close();
                }
                uniDestroyCamera(m_hCamera);
                m_hCamera = IntPtr.Zero;
            }
        }

        public bool Start(int nTriggerType)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraStart(m_hCamera, nTriggerType);
            }
            return false;
        }

        public void Stop()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                uniCameraStop(m_hCamera);
            }
        }

        public bool IsStarted()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraIsStarted(m_hCamera);
            }
            return false;
        }

        public bool OpenE1(string strName)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraOpenE1(m_hCamera, strName);
            }
            return false;
        }

        public bool Open(int index)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraOpen(m_hCamera, index);
            }
            return false;
        }



        public bool Reopen()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraReopen(m_hCamera);
            }
            return false;
        }

        public void Close()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                uniCameraClose(m_hCamera);
            }
        }

        public void CloseE1(bool bThrough)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                uniCameraCloseE1(m_hCamera, bThrough);
            }
        }

        public bool IsOpened()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraIsOpened(m_hCamera);
            }
            return false;
        }

        public bool Trigger(int timeout)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraTrigger(m_hCamera, timeout);
            }
            return false;
        }

        public bool FetchImage(IntPtr hImage, int timeout)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraFetchImage(m_hCamera, hImage, timeout);
            }
            return false;
        }
        public bool FetchImage(KImage image, int timeout)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraFetchImage(m_hCamera, image.Handle, timeout);
            }
            return false;
        }

        public bool GetDepth(out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetDepth(m_hCamera, out pValue);
            }
            pValue = -1;
            return false;
        }

        public bool GetWidth(out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetWidth(m_hCamera, out pValue);
            }
            pValue = -1;
            return false;
        }

        public bool GetHeight(out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetHeight(m_hCamera, out pValue);
            }
            pValue = -1;
            return false;
        }

        public void SetFrameArrivalCallback(FrameArrivalDelegate pfnFrameArrival, IntPtr pUserData)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                uniCameraSetFrameArrivalCallback(m_hCamera, pfnFrameArrival, pUserData);
            }
        }

        public FrameArrivalDelegate GetFrameArrivalCallback()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetFrameArrivalCallback(m_hCamera);
            }

            return null;
        }


        public IntPtr GetFrameArrivalUserData()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetFrameArrivalUserData(m_hCamera);
            }
            return IntPtr.Zero;
        }

        public int GetTriggerType()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetTriggerType(m_hCamera);
            }
            return -1;
        }

        public int GetRotateType()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetRotateType(m_hCamera);
            }
            return -1;
        }

        public void SetRotateType(int type)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                uniCameraSetRotateType(m_hCamera, type);
            }
        }

        public bool SetParam(string strName, int value)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraSetParam(m_hCamera, strName, value);
            }
            return false;
        }

        public bool GetParam(string strName, out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParam(m_hCamera, strName, out pValue);
            }
            pValue = -1;
            return false;
        }

        public bool GetParamMax(string strName, out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamMax(m_hCamera, strName, out pValue);
            }
            pValue = -1;
            return false;
        }

        public bool GetParamMin(string strName, out int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamMin(m_hCamera, strName, out pValue);
            }
            pValue = -1;
            return false;
        }

        public bool SetRoi(int offsetx, int offsety, int width, int height)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraSetRoi(m_hCamera, offsetx, offsety, width, height);
            }
            return false;
        }

        public bool SetResolution(int width, int height)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraSetResolution(m_hCamera, width, height);
            }
            return false;
        }

        public bool AutoBalanceWhite()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraAutoBalanceWhite(m_hCamera);
            }
            return false;
        }

        public string GetSerialNum()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1204);

                uniCameraGetSerialNum(m_hCamera, sb, 1204);

                return sb.ToString();
            }
            return null;
        }

        public string GetSerialNumAtIndex(int index)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1204);
                uniCameraGetSerialNumAtIndex(m_hCamera, index, sb, 1204);

                return sb.ToString();
            }

            return null;
        }

        public IntPtr GetInnerHandle()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetHandle(m_hCamera);
            }
            return IntPtr.Zero;
        }

        public int GetLastError()
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetLastError(m_hCamera);
            }
            return -1;
        }

        public string GetErrorDescription(int errorCode)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                IntPtr ptr = uniCameraGetErrorDesc(m_hCamera, errorCode);

                string errorDesc = Marshal.PtrToStringAnsi(ptr);

                return errorDesc;
            }

            return string.Empty; // null string
        }

        public bool GetParamDelta(string strName, ref int pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamDelta(m_hCamera, strName, ref pValue);
            }
            return false;
        }

        public bool GetParam(string strName, ref double pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamF(m_hCamera, strName, ref pValue);
            }
            return false;
        }

        public bool bGetParamDelta(string strName, ref double pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamDeltaF(m_hCamera, strName, ref pValue);
            }
            return false;
        }

        public bool GetParamMax(string strName, ref double pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamMaxF(m_hCamera, strName, ref pValue);
            }
            return false;
        }

        public bool GetParamMin(string strName, ref double pValue)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraGetParamMinF(m_hCamera, strName, ref pValue);
            }
            return false;
        }

        public bool SetParam(string strName, double value)
        {
            if (m_hCamera != IntPtr.Zero)
            {
                return uniCameraSetParamF(m_hCamera, strName, value);
            }
            return false;
        }


    }
}
