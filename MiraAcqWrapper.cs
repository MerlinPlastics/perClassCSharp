using System;
using System.Runtime.InteropServices;

public class MiraAcqWrapper : IDisposable
{
    private const string DllName = "miraacq_ximea_1.13.1.dll";

    // Public constants from miraacq.h
    public const int MIRA_OK = 0;
    public const int MIRA_ERROR = -1;

    // Frame data types
    public const int ACQ_DATATYPE_UNKNOWN = 0;
    public const int ACQ_DATATYPE_UINT16 = 1;
    public const int ACQ_DATATYPE_FLOAT = 2;
    public const int ACQ_DATATYPE_UINT8 = 3;

    // Data layout types
    public const int ACQ_DATALAYOUT_BIP = 1; // spectrum-by-spectrum (bands-width-height)
    public const int ACQ_DATALAYOUT_BIL = 2; // frame-by-frame (width-bands-height)
    public const int ACQ_DATALAYOUT_BSQ = 3; // spatial frame by frame (width-height-bands)

    public enum AcqDataType
    {
        Unknown = 0,
        UInt16 = 1,
        Float = 2,
        UInt8 = 3
    }

    public enum AcqDataLayout
    {
        BIP = 1, // spectrum-by-spectrum (bands-width-height)
        BIL = 2, // frame-by-frame (width-bands-height)
        BSQ = 3  // spatial frame by frame (width-height-bands)
    }

    private IntPtr _pma;

    public IntPtr KernelPointer { get { return _pma; } }

    // Private constructor to prevent direct instantiation
    private MiraAcqWrapper(IntPtr pma)
    {
        _pma = pma;
    }

    // Static method to create an instance and initialize it
    public static MiraAcqWrapper Create(string path)
    {
        IntPtr pma = miraacq_Init(path);
        if (pma == IntPtr.Zero)
        {
            throw new Exception("Failed to initialize MiraAcq.");
        }
        return new MiraAcqWrapper(pma);
    }

    // DLL Imports and Wrapper Methods
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_Init(string path);

    //public static IntPtr Init(string path) => miraacq_Init(path);
    public bool IsInitialized()
    {
        return _pma != IntPtr.Zero;
    }


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void miraacq_Release(IntPtr pma);

    public void Release()
    {
        if (_pma != IntPtr.Zero)
        {
            miraacq_Release(_pma);
            _pma = IntPtr.Zero;
        }
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_GetAPIVersion(out int api, out int step, out int rev);

    public static string GetAPIVersion(out int api, out int step, out int rev)
    {
        IntPtr ptr = miraacq_GetAPIVersion(out api, out step, out rev);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_GetVersion();

    public static string GetVersion()
    {
        IntPtr ptr = miraacq_GetVersion();
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_GetRecorderType();

    public static string GetRecorderType()
    {
        IntPtr ptr = miraacq_GetRecorderType();
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetErrorCode(IntPtr pma);

    public int GetLastErrorCode()
    {
        return miraacq_GetErrorCode(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_GetErrorMsg(IntPtr pma);

    public string GetLastErrorMsg()
    {
        IntPtr ptr = miraacq_GetErrorMsg(_pma);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_ScanDevices(IntPtr pma);

    public int ScanDevices()
    {
        return miraacq_ScanDevices(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetDeviceCount(IntPtr pma);

    public int GetDeviceCount()
    {
        return miraacq_GetDeviceCount(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr miraacq_GetDeviceName(IntPtr pma, int devInd);

    public string GetDeviceName(int devInd)
    {
        IntPtr ptr = miraacq_GetDeviceName(_pma, devInd);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_OpenDevice(IntPtr pma, int devInd);

    public int OpenDevice(int devInd)
    {
        return miraacq_OpenDevice(_pma, devInd);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_CloseDevice(IntPtr pma, int devInd);

    public int CloseDevice(int devInd)
    {
        return miraacq_CloseDevice(_pma, devInd);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_DeviceIsSnapshot(IntPtr pma);

    public bool DeviceIsSnapshot()
    {
        return miraacq_DeviceIsSnapshot(_pma) != 0;
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_InitializeAcquisition(IntPtr pma);

    public int InitializeAcquisition()
    {
        return miraacq_InitializeAcquisition(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameSize(IntPtr pma);

    public int GetFrameSize()
    {
        return miraacq_GetFrameSize(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameWidth(IntPtr pma);

    public int GetFrameWidth()
    {
        return miraacq_GetFrameWidth(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameHeight(IntPtr pma);

    public int GetFrameHeight()
    {
        return miraacq_GetFrameHeight(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameBands(IntPtr pma);

    public int GetFrameBands()
    {
        return miraacq_GetFrameBands(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameDataType(IntPtr pma);

    public int GetFrameDataType()
    {
        return miraacq_GetFrameDataType(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameDataLayout(IntPtr pma);

    public int GetFrameDataLayout()
    {
        return miraacq_GetFrameDataLayout(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_CanReturnWavelengths(IntPtr pma);

    public bool CanReturnWavelengths()
    {
        return miraacq_CanReturnWavelengths(_pma) != 0;
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameWavelength(IntPtr pma, int bandInd, out double pWavelength);

    public int GetFrameWavelength(int bandInd, out double wavelength)
    {
        return miraacq_GetFrameWavelength(_pma, bandInd, out wavelength);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_StartAcquisition(IntPtr pma);

    public int StartAcquisition()
    {
        return miraacq_StartAcquisition(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_StopAcquisition(IntPtr pma);

    public int StopAcquisition()
    {
        return miraacq_StopAcquisition(_pma);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrame(IntPtr pma, IntPtr pBuf, ref ulong pFrameInd, int timeout);

    public int GetFrame(IntPtr buffer, ref ulong frameIndex, int timeout)
    {
        return miraacq_GetFrame(_pma, buffer, ref frameIndex, timeout);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetExposure(IntPtr pma, double val);

    public int SetExposure(double value)
    {
        return miraacq_SetExposure(_pma, value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetExposure(IntPtr pma, out double pVal);

    public int GetExposure(out double value)
    {
        return miraacq_GetExposure(_pma, out value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetFrameRate(IntPtr pma, double val);

    public int SetFrameRate(double value)
    {
        return miraacq_SetFrameRate(_pma, value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFrameRate(IntPtr pma, out double pVal);

    public int GetFrameRate(out double value)
    {
        return miraacq_GetFrameRate(_pma, out value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFeatureInt(IntPtr pma, string pName, out int pVal);

    public int GetFeatureInt(string name, out int value)
    {
        return miraacq_GetFeatureInt(_pma, name, out value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetFeatureInt(IntPtr pma, string pName, int val);

    public int SetFeatureInt(string name, int value)
    {
        return miraacq_SetFeatureInt(_pma, name, value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFeatureBool(IntPtr pma, string pName, out int pVal);

    public int GetFeatureBool(string name, out bool value)
    {
        int result = miraacq_GetFeatureBool(_pma, name, out int intValue);
        value = intValue != 0;
        return result;
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetFeatureBool(IntPtr pma, string pName, int val);

    public int SetFeatureBool(string name, bool value)
    {
        return miraacq_SetFeatureBool(_pma, name, value ? 1 : 0);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFeatureDouble(IntPtr pma, string pName, out double pVal);

    public int GetFeatureDouble(string name, out double value)
    {
        return miraacq_GetFeatureDouble(_pma, name, out value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetFeatureDouble(IntPtr pma, string pName, double val);

    public int SetFeatureDouble(string name, double value)
    {
        return miraacq_SetFeatureDouble(_pma, name, value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_GetFeatureString(IntPtr pma, string pName, IntPtr pBuf, int bufLen);

    public string GetFeatureString(string name, int bufferLength)
    {
        IntPtr buffer = Marshal.AllocHGlobal(bufferLength);
        try
        {
            int result = miraacq_GetFeatureString(_pma, name, buffer, bufferLength);
            return result == 0 ? Marshal.PtrToStringAnsi(buffer) : null;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_SetFeatureString(IntPtr pma, string pName, string value);

    public int SetFeatureString(string name, string value)
    {
        return miraacq_SetFeatureString(_pma, name, value);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int miraacq_ExecuteCommand(IntPtr pma, string pName, IntPtr ptr1, IntPtr ptr2, IntPtr ptr3);

    public int ExecuteCommand(string commandName, IntPtr param1, IntPtr param2, IntPtr param3)
    {
        return miraacq_ExecuteCommand(_pma, commandName, param1, param2, param3);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "miraacq_SetResampling")]
    public static extern int miraacq_SetResampling(IntPtr pma, int enable);

    public int SetResampling(bool enable)
    {
        if (enable)
            return miraacq_SetResampling(_pma, 1);
        else
            return miraacq_SetResampling(_pma, 0);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int miraacq_SetResamplingWavelengthCount(IntPtr pma, int count);

    public int SetResamplingWavelengthCount(int count)
    {
        return miraacq_SetResamplingWavelengthCount(_pma, count);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int miraacq_SetResamplingWavelength(IntPtr pma, int bandInd, double wavelength);

    public int SetResamplingWavelength(int bandInd, double wavelength)
    {
        return miraacq_SetResamplingWavelength(_pma, bandInd, wavelength);
    }


    // Implementing IDisposable to clean up unmanaged resources
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_pma != IntPtr.Zero)
        {
            miraacq_Release(_pma);
            _pma = IntPtr.Zero;
        }
    }

    ~MiraAcqWrapper()
    {
        Dispose(false);
    }
}
