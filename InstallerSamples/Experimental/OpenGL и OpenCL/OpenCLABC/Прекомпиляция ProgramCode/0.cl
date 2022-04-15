


__kernel void TEST(__global int* message)
{
	int gid = get_global_id(0); // номер текущего вызова TEST
	
	message[gid] += gid;
}


