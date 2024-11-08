#ifndef MIRARUNTIME_H
#define MIRARUNTIME_H

/* perclass_mira.h: Interface of perClass Mira runtime
 *
 * Copyright (C) 2023, perClass BV, All rights reserved
 */

#ifdef __cplusplus
extern "C" {
#endif

#define SD_VERSION  "perClass Mira Runtime 4.2 (13-feb-2023)"
#define SD_COPYRIGHT  "Copyright (C) 2007-2023, perClass BV, All rights reserved"

typedef struct _mrkernel mrkernel;

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

// macro checking the result code (int res needs to be declared). If error is encountered,
// jumping to Error label
#define MIRA_CHECK( ARG ) {res=ARG; if(res!=MIRA_OK) goto Error;}

/* Return runtime version string */
DLL_EXPORT const char* API mira_GetVersion();

DLL_EXPORT int API mira_GetErrorCode(mrkernel* pmr);
DLL_EXPORT const char* API mira_GetErrorMsg(mrkernel* pmr);

DLL_EXPORT mrkernel* mira_Init(const char* path);

DLL_EXPORT int mira_RefreshDeviceList(mrkernel *pmr,int listNVIDIA,int listOpenCL);
DLL_EXPORT int mira_GetDeviceCount(mrkernel *pmr);
DLL_EXPORT const char* mira_GetDeviceName(mrkernel* pmr,int deviceInd);

/* note that if a device is changed, model and correction needs to be loaded again */
DLL_EXPORT int mira_SetDevice(mrkernel* pmr,int deviceInd);

DLL_EXPORT int mira_LoadModel(mrkernel *pmr, const char* filename);

DLL_EXPORT int mira_LoadCorrection(mrkernel *pmr, const char* dirname, const char *scanname);

DLL_EXPORT int mira_SetMinObjSize(mrkernel *pmr,int minSize);
DLL_EXPORT int mira_SetSegmentation(mrkernel *pmr,int enable);

/* information on expected input image size and data type of the user-provided cube buffer can be inspected
 * via mira_GetCube* API */
DLL_EXPORT int mira_GetInputWidth(mrkernel* pmr);
DLL_EXPORT int mira_GetInputHeight(mrkernel* pmr);
DLL_EXPORT int mira_GetInputBands(mrkernel* pmr);

/* input data width may be set manually after loading model */
DLL_EXPORT int mira_SetInputWidth(mrkernel* pmr,int width);

#define MIRA_DATATYPE_UNKNOWN  0
#define MIRA_DATATYPE_UINT16   1
#define MIRA_DATATYPE_FLOAT    2
#define MIRA_DATATYPE_UINT8    3
DLL_EXPORT int mira_GetInputDataType(mrkernel* pmr);

#define MIRA_DATALAYOUT_UNKNOWN  0
#define MIRA_DATALAYOUT_BIP      1 /* spectrum-by-spectrum (dimensions: bands-width-height) */
#define MIRA_DATALAYOUT_BIL      2 /* frame-by-frame (dimensions: width-bands-height) */
#define MIRA_DATALAYOUT_BSQ      3 /* spatial frame by frame (dimensions: width-height-bands) */
DLL_EXPORT int mira_GetInputDataLayout(mrkernel* pmr);
DLL_EXPORT int mira_SetInputDataLayout(mrkernel* pmr,int layout);

#define MIRA_MASK_EACH_FOREGROUND  1  /* single material objects: each foreground class segmented separately */
#define MIRA_MASK_ALL_FOREGROUND   2  /* complex objects: all foreground classes joined to one mask */
DLL_EXPORT int mira_GetMaskType(mrkernel* pmr);

DLL_EXPORT int mira_GetRegVarCount(mrkernel *pmr);
DLL_EXPORT const char* mira_GetRegVarName(mrkernel* pmr,int varInd);

/* acquisition */
DLL_EXPORT int mira_StartAcquisition(mrkernel* pmr);
DLL_EXPORT int mira_StopAcquisition(mrkernel* pmr);

/* line-scan API: Process individual frames (pixels x bands) */
/*  - each uncorrected frame has pixels x bands uint16_t values in BIL format) */
DLL_EXPORT int mira_ProcessFrame(mrkernel* pmr, void* pData);

/* outputs: per-pixel decisions */
DLL_EXPORT const unsigned char *mira_GetFrameDecisions(mrkernel* pmr);

DLL_EXPORT const float *mira_GetFrameRegOutputVar(mrkernel* pmr, int varInd, int maskBackground, float maskVal);

/* snapshot API: Process complete cube */
DLL_EXPORT int mira_ProcessCube(mrkernel* pmr, void* pData);

/* outputs: cube per-pixel decisions */
DLL_EXPORT const unsigned char *mira_GetCubeDecisions(mrkernel* pmr);

/* region of interest ROI */
DLL_EXPORT int mira_SetCubeROI(mrkernel* pmr, int col, int row, int width, int height, unsigned char valueOutsideROI);

/* Get information on decisions */
DLL_EXPORT int mira_GetDecCount(mrkernel* pmr);
DLL_EXPORT const char* mira_GetDecName(mrkernel* pmr,int decInd);
DLL_EXPORT int mira_GetDecColor(mrkernel* pmr,int decInd,unsigned char* R,unsigned char* G,unsigned char* B);

/* outputs: objects */
DLL_EXPORT int mira_GetObjCount(mrkernel* pmr);
/* Object details entriID options */
#define MIRA_OBJECT_ID        0
#define MIRA_OBJECT_FRAME     1
#define MIRA_OBJECT_POS       2
#define MIRA_OBJECT_MINFRAME  3
#define MIRA_OBJECT_MAXFRAME  4
#define MIRA_OBJECT_MINCOL    5
#define MIRA_OBJECT_MAXCOL    6
#define MIRA_OBJECT_SIZE      7
#define MIRA_OBJECT_CLASS     8
DLL_EXPORT int mira_GetObjDataInt(mrkernel* pmr, int entryInd, int** ppObjData);
DLL_EXPORT int mira_GetObjDataClassSize(mrkernel* pmr, int entryInd, int classInd);
DLL_EXPORT float mira_GetObjDataClassFrac(mrkernel* pmr, int entryInd, int classInd);

DLL_EXPORT int mira_GetObjDataRegOutput(mrkernel* pmr, int entryInd, const float** ppObjData);

/* outputs: visualization */
DLL_EXPORT int mira_SaveImage(mrkernel* pmr,const char* filename);

DLL_EXPORT void mira_Release(mrkernel* pmr);

/* status and error codes ================================= */

#define MIRA_OK     0
#define MIRA_ERROR -1

/* specific error codes

-101   Passing NULL pointer
-102   mira_GetDeviceName: Device index out of bounds
-103   mira_StartAcquisition: Project not loaded
-104   mira_StartAcquisition: Classifier model not loaded

-110   mira_LoadModel: Error loading model from file
-111   mira_LoadModel: Wrong file format
-112   mira_LoadModel: Internal error when loading
-113   mira_LoadModel: File cannot be opened
-114   mira_LoadModel: Project type not supported by this runtime build

-120   mira_saveImage: Label image does not exist

-130   mira_LoadCorrection: Loading meta-data from the correction scan failed.
-131   mira_LoadCorrection: Error loading dark reference data
-132   mira_LoadCorrection: Dark and White reference images have different width or band count.
-134   mira_LoadCorrection: Both dark and white reference scans need to be loaded.
-135   mira_LoadCorrection: Reference file not present
-136   mira_LoadCorrection: Unsupported data layout or data type
-137   mira_LoadCorrection: Cannot load correction scan

-140   Error switching to the computation device
-141   mira_RefreshDeviceList: Error setting CUDA backend
-142   mira_RefreshDeviceList: Error setting OpenCL backend
-143   mira_RefreshDeviceList: listNVIDIA and listOpenCL must be specified as 0 or 1 values

-150   Feature does not exist
-151   Wrong feature type requested

-160   Max number of objects per frame reached
-161   mira_GetObjData*: Object index out of bounds
-162   mira_GetObjData*: Class index out of bounds (0..9)
-163   mira_GetObjDataClassSize: Segmentation not set to required 'All foreground' mode.
-164   mira_GetMaskType: Object segmetation not defined
-165   mira_GetInputDataType: Project and acquisition type unsupported

-170   mira_StartAcquisition: Acquisition already running
-171   mira_StopAcquisition: Acquisition not running
-172   mira_StartAcquisition: Object segmentation cannot proceed

-173   mira_StartAcquisition: Input data layout not set
-174   mira_StartAcquisition: Input data type not set

-180   mira_GetDecName: Decision index out of bounds

-190   mira_SetForegroundClass: Foreground class index out of bounds

-201   mira_ProcessCube: Line-scan project type cannot process cubes
-202   mira_StartAcquisition: Label image dimension mismatch

-203   mira_ProcessCube: Missing image geometry description

-204   mira_ProcessFrame: Line-scan processing requires BIL layout

-205   mira_GetInputDataLayout: Undefined data layout
-206   mira_GetInputDataType: Undefined data type

-207   mira_ProcessCube: Unsupported project type

-208   mira_ProcessFrame: Unsupported data type
-208   mira_ProcessCube: Unsupported data type or data layout

-209   mira_ProcessFrame: Correction not supported

-210   mira_SetCubeROI: Invalid ROI specification

-211   mira_GetFrameDecisions: Line-scan project type expected, got snapshot.
-212   mira_GetCubeDecisions: Snapshot project type expected, got linescan

-213   mira_ProcessCube: Object segmentation error

-221   mira_ProcessCube: ROI processing unsupported for this project type

-230   mira_SetInputWidth: Width value must be positive

-301   mira_GetFrameRegOutputVar: Variable index out of bounds
-302   mira_GetRegVarName: No regression model available
-303   mira_GetFrameRegOutputVar: Masking cannot be used as segmentation is not enabled

-401   mira_SetResampling: Resampling start/step/stop not set
*/


#ifdef __cplusplus
}
#endif

#endif // MIRARUNTIME_H
