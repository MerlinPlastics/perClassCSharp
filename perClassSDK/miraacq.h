#ifndef MIRAACQ_H
#define MIRAACQ_H

#define SD_VERSION  "perClass Camera API 4.2.4 (20-apr-2023)"
#define SD_COPYRIGHT  "Copyright (C) 2007-2023, perClass BV, All rights reserved"

#define SD_BUILD_DATE __DATE__
#define SD_BUILD_TIME __TIME__

#include <stddef.h>

typedef struct _makernel makernel;

#if ( SD_PLATFORM_WIN32==1 || SD_PLATFORM_WIN64==1 )
  #define DLL_EXPORT __declspec( dllexport )
  #if SD_CDECL
     #define API __cdecl
  #else
     #define API __stdcall
  #endif
#else
  #define DLL_EXPORT extern
  #define API
#endif

#ifdef __cplusplus
extern "C" {
#endif

#define MIRA_OK     0
#define MIRA_ERROR -1

// macro checking the result code (int res needs to be declared). If error is encountered,
// jumping to Error label
#define MIRAACQ_CHECK( ARG ) {res=ARG; if(res!=MIRA_OK) goto Error;}

// acq plugin API version
// returns string with full info example: "acquisition plugin 1.0.0 (cubert), perClass Mira 4.0.1 23-mar-2022"
// numerical plugin version
//  api = major outer API of all supported plugins
// step = functionality version of this specific plugin (perClass Mira functionality changes)
//  rev = revision of the functionality (only the plugin changes)
DLL_EXPORT const char* API miraacq_GetAPIVersion(int* pApi,int* pStep,int* pRev);
DLL_EXPORT const char * API miraacq_GetVersion();

DLL_EXPORT const char* API miraacq_GetRecorderType();

DLL_EXPORT int API miraacq_GetErrorCode(makernel* pma);
DLL_EXPORT const char* API miraacq_GetErrorMsg(makernel* pma);

DLL_EXPORT makernel* miraacq_Init(const char* path);
DLL_EXPORT void miraacq_Release(makernel* pma);

// device handling
DLL_EXPORT int API miraacq_ScanDevices(makernel* pma);
DLL_EXPORT int API miraacq_GetDeviceCount(makernel* pma);
DLL_EXPORT const char* API miraacq_GetDeviceName(makernel* pma,int devInd);

DLL_EXPORT int API miraacq_OpenDevice(makernel* pma,int devInd);
DLL_EXPORT int API miraacq_CloseDevice(makernel* pma,int devInd);

DLL_EXPORT int API miraacq_DeviceIsSnapshot(makernel* pma);

// acquisition
DLL_EXPORT int API miraacq_InitializeAcquisition(makernel* pma);

// get frame details
DLL_EXPORT int API miraacq_GetFrameSize(makernel* pma);
DLL_EXPORT int API miraacq_GetFrameWidth(makernel* pma);
DLL_EXPORT int API miraacq_GetFrameHeight(makernel* pma);
DLL_EXPORT int API miraacq_GetFrameBands(makernel* pma);

#define ACQ_DATATYPE_UNKNOWN  0
#define ACQ_DATATYPE_UINT16   1
#define ACQ_DATATYPE_FLOAT    2
#define ACQ_DATATYPE_UINT8    3
DLL_EXPORT int API miraacq_GetFrameDataType(makernel* pma);

#define ACQ_DATALAYOUT_BIP      1 /* spectrum-by-spectrum (dimensions: bands-width-height) */
#define ACQ_DATALAYOUT_BIL      2 /* frame-by-frame (dimensions: width-bands-height) */
#define ACQ_DATALAYOUT_BSQ      3 /* spatial frame by frame (dimensions: width-height-bands) */
DLL_EXPORT int API miraacq_GetFrameDataLayout(makernel* pma);

DLL_EXPORT int miraacq_CanReturnWavelengths(makernel* pma);
DLL_EXPORT int miraacq_GetFrameWavelength(makernel* pma,int bandInd,double* pWavelength);

DLL_EXPORT int API miraacq_StartAcquisition(makernel* pma);
DLL_EXPORT int API miraacq_StopAcquisition(makernel* pma);

DLL_EXPORT int API miraacq_GetFrame(makernel* pma,void* pBuf,size_t* pFrameInd,int timeOut);

// parameters
DLL_EXPORT int API miraacq_SetExposure(makernel* pma,double val);
DLL_EXPORT int API miraacq_GetExposure(makernel* pma,double* pVal);

DLL_EXPORT int API miraacq_SetFrameRate(makernel* pma,double val);
DLL_EXPORT int API miraacq_GetFrameRate(makernel* pma,double* pVal);

// feature access
DLL_EXPORT int API miraacq_GetFeatureInt(makernel* pma,const char* pName,int* pVal);
DLL_EXPORT int API miraacq_SetFeatureInt(makernel* pma, const char* pName, int val);

DLL_EXPORT int API miraacq_GetFeatureBool(makernel* pma,const char* pName,int* pVal);
DLL_EXPORT int API miraacq_SetFeatureBool(makernel* pma, const char* pName, int val);

DLL_EXPORT int API miraacq_GetFeatureDouble(makernel* pma,const char* pName,double* pVal);
DLL_EXPORT int API miraacq_SetFeatureDouble(makernel* pma, const char* pName, double val);

DLL_EXPORT int API miraacq_GetFeatureString(makernel* pma,const char* pName,char* pBuf,int bufLen);
DLL_EXPORT int API miraacq_SetFeatureString(makernel* pma, const char* pName,char* pBuf);

DLL_EXPORT int API miraacq_GetFeatureEnum(makernel* pma,const char* pName,char* pBuf,int bufLen);
DLL_EXPORT int API miraacq_SetFeatureEnum(makernel* pma, const char* pName,char* pBuf);

// custom commands (specific for each plugin or camera type)
DLL_EXPORT int API miraacq_ExecuteCommand(makernel* pma,const char* pName,void* ptr1,void* ptr2,void* ptr3);

/* wavelength resampling */

DLL_EXPORT int miraacq_SetResampling(makernel* pma,int enable);
DLL_EXPORT int miraacq_SetResamplingWavelengthCount(makernel* pma,int count);
DLL_EXPORT int miraacq_SetResamplingWavelength(makernel* pma,int bandInd,double wavelenegth);

#ifdef __cplusplus
}
#endif

#endif // MIRAACQ_FILEREADER_H
