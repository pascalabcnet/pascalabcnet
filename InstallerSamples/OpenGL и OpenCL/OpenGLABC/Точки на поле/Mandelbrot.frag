#version 460 core

// Подсматривал сюда:
// https://stackoverflow.com/questions/44354589/optimizing-mandelbrot-fractal

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
	dvec2 data[3];
} temp;
/**/

out vec3 color;

// Радиус белой точки
// (хотел сделать взаимодействие с основным рисунком, но и так лагает)
const float point_r = 5;
// Скорость затемнения при отдалении от точки
const float fall_speed = 0.6;

const float extra_scale = 0.01;

const int MAX_DEPTH = 128;
int CalculateMandelbrotDepth(dvec2 c, dvec2 start) {
	c *= extra_scale;
	//start *= extra_scale;
	if (dot(start,start)>2) return 0;
	
	dvec2 prev[MAX_DEPTH];
	dvec2 curr = start;
	
	for (int depth = 1; depth < MAX_DEPTH; ++depth) {
		prev[depth-1] = curr;
		
		curr = dvec2( curr.x*curr.x - curr.y*curr.y, 2*curr.x*curr.y )+c;
		
		if (dot(curr,curr)>2) return depth;
		for (int i = depth-1; i>=0; --i)
			if (curr == prev[i]) return depth;
	}
	
	return MAX_DEPTH;
}

// All components are in the range [0…1], including hue.
vec3 hsv2rgb(float hue)
{
	vec3 c = {hue, 0.75, 0.5};
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
	
	// Считаем в фрагментном шейдере, потому что интерполяция
	// 64-битного logic_pos (с типом dvec2) в OpenGL запрещена
	dvec2 pos = { logic_pos.x*camera_aspect_ratio, logic_pos.y };
	pos *= camera_scale;
	pos += camera_pos;
	
	for (int i=0; i<point_count; ++i) {
		dvec2 dp = pos-points.data[i];
		if (dot(dp,dp)<point_r*point_r) {
			color = vec3(1);
			return;
		}
	}
	
	int depth = CalculateMandelbrotDepth(pos, dvec2(0));
	/**
	for (int i=0; i<point_count; ++i) {
		//TODO Какое-то взаимодействие
	}
	/**/
	
	if (depth == 0)
		color = vec3(0); else
	{
		//color = vec3( float(depth) / MAX_DEPTH ); // Чёрно-белый рисунок
		//color = hsv2rgb( float(depth) / MAX_DEPTH * 0.8 + 0.1 ); // Все цвета радуги
		color = hsv2rgb( fract(float(depth) / MAX_DEPTH*radians(180)) ); // Псевдо-случайные цвета
	}
	
}


