#pragma OPENCL EXTENSION cl_khr_fp64: enable



__kernel void MatrMltMatr(__global double* A, __global double* B, __global double* C, int W)
{
	int cX = get_global_id(0);
	int cY = get_global_id(1);
	
	double sum = 0.0;
	for (int i=0; i<W; i++)
		sum += A[i + cX*W] * B[cY + i*W];
	
	C[cX + cY*W] = sum;
}

__kernel void MatrMltVec(__global double* C, __global double* V1, __global double* V2, int W)
{
	int i = get_global_id(0);
	
	double sum = 0.0;
	for (int j=0; j<W; j++)
		sum += C[j + i*W] * V1[j];
	
	V2[i] = sum;
}


