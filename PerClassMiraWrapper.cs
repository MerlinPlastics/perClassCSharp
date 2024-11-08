using System;
using System.Runtime.InteropServices;

public class PerClassMiraWrapper : IDisposable
{
    private const string DllName = "perclass_mira_gpu.dll";
    private IntPtr _pmr;

    // Private constructor to prevent direct instantiation
    private PerClassMiraWrapper(IntPtr pmr)
    {
        _pmr = pmr;
    }

    // Static method to create an instance and initialize it
    public static PerClassMiraWrapper Create(string path)
    {
        IntPtr pmr = mira_Init(path);
        if (pmr == IntPtr.Zero)
        {
            throw new Exception("Failed to initialize PerClassMira.");
        }
        return new PerClassMiraWrapper(pmr);
    }

    // DLL Imports and Wrapper Methods
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_Init(string path);

    //public static IntPtr Init(string path) => mira_Init(path);
    public bool IsInitialized()
    {
        return _pmr != IntPtr.Zero;
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void mira_Release(IntPtr pmr);

    public void Release()
    {
        if (_pmr != IntPtr.Zero)
        {
            mira_Release(_pmr);
            _pmr = IntPtr.Zero;
        }
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetVersion();

    public string GetVersion()
    {
        IntPtr ptr = mira_GetVersion();
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetErrorCode(IntPtr pmr);

    public int GetErrorCode()
    {
        return mira_GetErrorCode(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetErrorMsg(IntPtr pmr);

    public string GetErrorMsg()
    {
        IntPtr ptr = mira_GetErrorMsg(_pmr);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_RefreshDeviceList(IntPtr pmr, int listNVIDIA, int listOpenCL);

    public int RefreshDeviceList(bool listNVIDIA, bool listOpenCL)
    {
        return mira_RefreshDeviceList(_pmr, listNVIDIA ? 1 : 0, listOpenCL ? 1 : 0);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetDeviceCount(IntPtr pmr);

    public int GetDeviceCount()
    {
        return mira_GetDeviceCount(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetDeviceName(IntPtr pmr, int deviceInd);

    public string GetDeviceName(int deviceInd)
    {
        IntPtr ptr = mira_GetDeviceName(_pmr, deviceInd);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SetDevice(IntPtr pmr, int deviceInd);

    public int SetDevice(int deviceInd)
    {
        return mira_SetDevice(_pmr, deviceInd);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_LoadModel(IntPtr pmr, string filename);

    public int LoadModel(string filename)
    {
        return mira_LoadModel(_pmr, filename);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_LoadCorrection(IntPtr pmr, string dirname, string scanname);

    public int LoadCorrection(string dirname, string scanname)
    {
        return mira_LoadCorrection(_pmr, dirname, scanname);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SetMinObjSize(IntPtr pmr, int minSize);

    public int SetMinObjSize(int minSize)
    {
        return mira_SetMinObjSize(_pmr, minSize);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SetSegmentation(IntPtr pmr, int enable);

    public int SetSegmentation(bool enable)
    {
        return mira_SetSegmentation(_pmr, enable ? 1 : 0);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetInputWidth(IntPtr pmr);

    public int GetInputWidth()
    {
        return mira_GetInputWidth(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetInputHeight(IntPtr pmr);

    public int GetInputHeight()
    {
        return mira_GetInputHeight(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetInputBands(IntPtr pmr);

    public int GetInputBands()
    {
        return mira_GetInputBands(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SetInputWidth(IntPtr pmr, int width);

    public int SetInputWidth(int width)
    {
        return mira_SetInputWidth(_pmr, width);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetInputDataType(IntPtr pmr);

    public int GetInputDataType()
    {
        return mira_GetInputDataType(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetInputDataLayout(IntPtr pmr);

    public int GetInputDataLayout()
    {
        return mira_GetInputDataLayout(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SetInputDataLayout(IntPtr pmr, int layout);

    public int SetInputDataLayout(int layout)
    {
        return mira_SetInputDataLayout(_pmr, layout);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_StartAcquisition(IntPtr pmr);

    public int StartAcquisition()
    {
        return mira_StartAcquisition(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_StopAcquisition(IntPtr pmr);

    public int StopAcquisition()
    {
        return mira_StopAcquisition(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_ProcessFrame(IntPtr pmr, IntPtr pData);

    public int ProcessFrame(IntPtr pData)
    {
        return mira_ProcessFrame(_pmr, pData);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetFrameDecisions(IntPtr pmr);

    public IntPtr GetFrameDecisions()
    {
        return mira_GetFrameDecisions(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetFrameRegOutputVar(IntPtr pmr, int varInd, int maskBackground, float maskVal);

    public IntPtr GetFrameRegOutputVar(int varInd, bool maskBackground, float maskVal)
    {
        return mira_GetFrameRegOutputVar(_pmr, varInd, maskBackground ? 1 : 0, maskVal);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_GetRegVarCount(IntPtr pmr);

    public int GetRegVarCount()
    {
        return mira_GetRegVarCount(_pmr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mira_GetRegVarName(IntPtr pmr, int varInd);

    public string GetRegVarName(int varInd)
    {
        IntPtr ptr = mira_GetRegVarName(_pmr, varInd);
        return Marshal.PtrToStringAnsi(ptr);
    }

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int mira_SaveImage(IntPtr pmr, string filename);

    public int SaveImage(string filename)
    {
        return mira_SaveImage(_pmr, filename);
    }

    // Implementing IDisposable to clean up unmanaged resources
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_pmr != IntPtr.Zero)
        {
            mira_Release(_pmr);
            _pmr = IntPtr.Zero;
        }
    }

    ~PerClassMiraWrapper()
    {
        Dispose(false);
    }
}
