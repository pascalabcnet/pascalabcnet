#version 460 core

noperspective in vec2 logic_pos;

layout(location = 0) uniform int point_count;
layout(binding = 0) buffer points_in {
	dvec2 data[];
} points;

layout(location = 1) uniform double camera_aspect_ratio;
layout(location = 2) uniform double camera_scale;
layout(location = 3) uniform dvec2 camera_pos;

layout(binding = 1) buffer temp_otp {
	dvec2 data[3];
} temp;

out vec3 color;

// Радиус красной точки
const float point_r = 5;
const float point_r_sqr = point_r*point_r;
// Скорость затемнения при отдалении от точки
const float fall_speed = 0.3;

void main() {
	
	// Считаем в фрагментном шейдере, потому что интерполяция
	// 64-битного logic_pos (с типом dvec2) в OpenGL запрещена
	dvec2 pos = { logic_pos.x*camera_aspect_ratio, logic_pos.y };
	pos *= camera_scale;
	pos += camera_pos;
	
	// Храним и сравниваем квадраты расстояний,
	// чтобы не считать корень лишний раз - он дорогой
	double min_r_sqr = double(1)/0;
	
	for (int i=0; i<point_count; ++i) {
		dvec2 dp = pos - points.data[i];
		double r_sqr = dot(dp, dp);
		if (r_sqr<point_r_sqr) {
			color = vec3(0.7, 0, 0);
			return;
		}
		if (r_sqr<min_r_sqr) min_r_sqr = r_sqr;
	}
	
	// Функция формы y=1/x, но маштабированная чтобы y(point_r)=1
	double fall_sqr = point_r_sqr/min_r_sqr;
	// И в данном случае результат в квадрате, поэтому дальше степень делим на 2
	float fall = pow(float(fall_sqr), fall_speed/2);
	
	// Ступеньки из оттенков серого
	if (false) {
		int color_count = 64;
		fall = round(fall*color_count) / color_count;
	}
	
	color = vec3( fall );
}


