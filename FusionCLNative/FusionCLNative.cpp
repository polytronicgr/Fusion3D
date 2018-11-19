// FusionCLNative.cpp : Defines the exported functions for the DLL application.
//

#pragma once

#include "stdafx.h"
#include <stdio.h>
#include <CL/cl.h>
#include <memory.h>
#include <malloc.h>

#ifdef FUSIONCLNATIVE_EXPORTS
#define FUSIONCL_API __declspec(dllexport)
#else
#define FUSIONCL_API __declspec(dllimport)
#endif

cl_platform_id platform_id = NULL;
cl_device_id device_id = NULL;
cl_uint ret_num_devices;
cl_uint ret_num_platforms;

cl_context context;

extern "C" FUSIONCL_API void InitFusionCL()
{
	printf("Init FusionCLNative begun.\n");
	
	cl_int ret = clGetPlatformIDs(1, &platform_id, &ret_num_platforms);
	ret = clGetDeviceIDs(platform_id, CL_DEVICE_TYPE_GPU, 1,
		&device_id, &ret_num_devices);
	//printf("Device:");

	
	const char * dev_name = (const char *)malloc(255);

	size_t asize = 0;

	clGetDeviceInfo(device_id, CL_DEVICE_NAME, 255, (void *)dev_name, &asize);

	printf("Dev:");
	printf(dev_name);
	printf("!\n");

	// Create an OpenCL context
	context = clCreateContext(NULL, 1, &device_id, NULL, NULL, &ret);



}

extern "C" FUSIONCL_API cl_command_queue CreateComQueue()
{
	cl_int ret;

	cl_command_queue command_queue = clCreateCommandQueue(context, device_id, 0, &ret);

	printf("CreateQueue:");
	if (ret == CL_SUCCESS) {
		printf("Yes\n");
	}
	else {
		printf("No\n");
	}
	printf("\n");
	return command_queue;

}

extern "C" FUSIONCL_API cl_mem CreateBuf(int bytes,cl_mem_flags flags,void * ptr) {

	cl_int ret = 0;
	cl_mem mem_obj = clCreateBuffer(context, CL_MEM_READ_ONLY, bytes, ptr, &ret);

	printf("CreateBuf:");
	if (ret == CL_SUCCESS) {
		printf("Yes\n");
	}
	else {
		printf("No\n");
	}

	return mem_obj;

}
