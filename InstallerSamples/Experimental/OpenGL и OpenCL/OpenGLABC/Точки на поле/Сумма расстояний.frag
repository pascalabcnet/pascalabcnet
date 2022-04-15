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
	dvec2 data[3];
} temp;
/**/

out vec3 color;

// Радиус красной точки
const float point_r = 5;
// Скорость затемнения при отдалении от точки
const float fall_speed = 0.6;

const bool fall_range_debug = false;

void main() {
	
	// В идеале это надо считать в геометрическом шейдере,
	// или даже на CPU... Но для этого надо менять код, общий с другими примерами
	float min_r_sum = 0;
	dvec2 d_center = {0,0};
	for (int i=0; i<point_count; ++i) d_center += points.data[i];
	d_center /= point_count;
	vec2 center = vec2( d_center );
	
	for (int i=0; i<point_count; ++i)
		min_r_sum += distance(center, vec2(points.data[i]));
	
	// Считаем в фрагментном шейдере, потому что интерполяция
	// 64-битного logic_pos (с типом dvec2) в OpenGL запрещена
	dvec2 pos = { logic_pos.x*camera_aspect_ratio, logic_pos.y };
	pos *= camera_scale;
	pos += camera_pos;
	
	if (fall_range_debug && distance(pos, d_center)<point_r) {
		color = vec3(0, 0, 0.7);
		return;
	}
	
	float r_sum = 0;
	for (int i=0; i<point_count; ++i) {
		dvec2 dp = pos - points.data[i];
		float r = sqrt(float(dot(dp, dp)));
		if (r<point_r) {
			color = vec3(0.7, 0, 0);
			return;
		}
		r_sum += r;
	}
	
	// Функция формы y=1/x, но маштабированная чтобы y(min_r_sum)=1
	double d_fall = min_r_sum/r_sum;
	float fall = pow(float(d_fall), fall_speed);
	
	// Ступеньки из оттенков серого
	if (true) {
		int color_count = 64;
		fall = round(fall*color_count) / color_count;
	}
	
	// center всё же не всегда имеет наименьшую сумму расстояний
	// Поэтому min_r_sum может считаться неправильно и fall оказываться >1
	// Наверное, нужен итерационный алгоритм на стороне CPU, чтобы адекватно это сделать
	// Напишите если есть идеи лучше
	if (fall_range_debug && fall>1) {
		color = vec3(0.8, 0, 0.8);
		return;
	}
	
	color = vec3( fall );
}


