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
	cl_mem mem_obj = clCreateBuffer(context, flags, (size_t)bytes, ptr, &ret);

	printf("CB:%d : %d : %d", flags, bytes, (int)ptr);
	printf("--\n");

	printf("CreateBuf:");
	if (ret == CL_SUCCESS) {
		printf("Yes\n");
	}
	else {
		printf("No\n");
	}

	return mem_obj;

}

extern "C" FUSIONCL_API bool QueueWriteBuffer(cl_command_queue queue, cl_mem mem, bool blocking, int offset, int cb, const void *ptr)
{

	cl_int ret = clEnqueueWriteBuffer(queue, mem, blocking ? CL_TRUE : CL_FALSE, offset,
		cb, ptr, 0, NULL, NULL);

	printf("QueueWrite:");
	if (ret == CL_SUCCESS)
	{
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}
	
}

extern "C" FUSIONCL_API cl_program CreateProgram(const char * source,int size)
{

	printf("Prog:\n");
	printf(source);
	printf(":\n");
    const char ** src = (const char **)source;
	cl_int ret = 0;
	cl_program program = clCreateProgramWithSource(context, 1,
		src, (const size_t *)&size, &ret);

	printf("CreateProg:");
	if (ret == CL_SUCCESS)
	{
		printf("Yes\n");

	}
	else {
		printf("No\n");
		return NULL;
	}

	return program;

}

extern "C" FUSIONCL_API bool BuildProgram(cl_program prog)
{

	cl_int ret = 0;

	ret = clBuildProgram(prog, 1, &device_id, NULL, NULL, NULL);

	printf("BuildProg:");
	if (ret == CL_SUCCESS) {
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}

}

extern "C" FUSIONCL_API cl_kernel CreateKern(cl_program prog, const char *name)
{

	cl_int ret = 0;
	cl_kernel kern = clCreateKernel(prog, name, &ret);


	printf("CreateKern:");
	if (ret == CL_SUCCESS)
	{
		printf("Yes\n");
		return kern;
	}
	else {
		printf("No\n");
		return NULL;
	}

}

extern "C" FUSIONCL_API bool KernSetArgPtr(cl_kernel kern,int par,int size,const void *ptr) {

	cl_int ret = 0;





	clSetKernelArg(kern, (cl_uint)par, (size_t)size, ptr);

	printf("SetArgPtr:");
	if (ret == CL_SUCCESS)
	{
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}

}

extern "C" FUSIONCL_API bool KernSetArgMem(cl_kernel kern, int par, int size, cl_mem mem) {

	cl_int ret = 0;

	ret = clSetKernelArg(kern, (cl_uint)par, sizeof(mem) , (const void *)&mem);
	printf("Kern:%d :%d %d",kern, size, mem);
	printf("!!\n");

	printf("SetArgMem:");
	if (ret == CL_SUCCESS)
	{
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}

}

extern "C" FUSIONCL_API bool ExecRange(cl_command_queue queue, cl_kernel kernel, int global, int sub) {

	cl_int ret = 0;

	ret=clEnqueueNDRangeKernel(queue, kernel, 1, NULL,(const size_t *)&global, (const size_t *)&sub, 0, NULL, NULL);

	printf("ExecRange:");
	if (ret == CL_SUCCESS) {
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}
}

extern "C" FUSIONCL_API bool QueueReadBuffer(cl_command_queue queue, cl_mem mem, bool block,int size, void *ptr)
{

	cl_int ret = 0;

	ret = clEnqueueReadBuffer(queue, mem, block, 0, size, ptr, 0, NULL, NULL);

	printf("ReadBuf:");
	if(ret==CL_SUCCESS)
	{
		printf("Yes\n");
		return true;
	}
	else {
		printf("No\n");
		return false;
	}
}