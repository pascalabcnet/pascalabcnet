


#define ValAt(B,X,Y) (B)[((X)+W)%W + ((Y)+W)%W * W]

__constant int min_max_neighbours[2][2] = {
	{ 3, 3 },
	{ 3, 4 },
};

__kernel void CalcStep(__global uchar* B, __global uchar* B_temp, int W)
{
	int X = get_global_id(0);
	int Y = get_global_id(1);
	
	int c = 0;
	for (int dx = -1; dx<2; ++dx)
		for (int dy = -1; dy<2; ++dy)
			c += ValAt(B, X+dx, Y+dy);
	uchar prev_alive = ValAt(B, X, Y);
	
	// Пока не смог заставить работать, поэтому нужен отдельный буфер B_temp...
	//work_group_barrier(CLK_LOCAL_MEM_FENCE);
	
	ValAt(B_temp, X, Y) = min_max_neighbours[prev_alive][0]<=c && c<=min_max_neighbours[prev_alive][1];
	
}


