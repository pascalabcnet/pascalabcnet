#version 460 core

noperspective in vec2 logic_pos;

layout(location = 0) uniform int point_count;
layout(binding = 0) buffer points_in {
	dvec2 data[];
} points;

layout(location = 1) uniform double camera_aspect_ratio;
layout(location = 2) uniform double camera_scale;
layout(location = 3) uniform dvec2 camera_pos;

/**
layout(binding = 1) buffer temp_otp {
	double data[3];
} temp;
/**/

out vec3 color;

const int arm_count = 5;

void main() {
	
	// Считаем в фрагментном шейдере, потому что интерполяция
	// 64-битного logic_pos (с типом dvec2) в OpenGL запрещена
	dvec2 pos = { logic_pos.x*camera_aspect_ratio, logic_pos.y };
	pos *= camera_scale;
	pos += camera_pos;
	
	double total_ang = 0;
	double total_r = 0;
	
	for (int i=0; i<point_count; ++i) {
		dvec2 dp = pos - points.data[i];
		dp /= point_count;
		
		// Угол от 0 до 1
		float angle = atan(
			float(dp.y/abs(dp.x)), float(sign(dp.x))
		)/radians(360)+0.5;
		total_ang += double(angle)*arm_count;
		
		double radius = sqrt(dot(dp,dp));
		radius = log2(float(radius));
		total_r += radius;
		
	}
	
	//total_r = log2(float(total_r));
	double res = total_ang + total_r;
	
	/**
	if (distance(pos, dvec2(temp.data[0],temp.data[1])) < camera_scale/500)
		temp.data[2] = res;
	/**/
	
	//res /= point_count;
	res = fract(res);
	
	color = vec3( res );
}


