#version 460 core

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;
out vec2 logic_pos;

void SendVertex(float coord1, float coord2) {
	logic_pos = vec2(coord1, coord2);
	gl_Position = vec4(coord1, coord2, 0, 1);
	EmitVertex();
}

// На вход приходит одна точка без данных
// На выход посылаем 2 треугольника, покрывающих весь экран
// При чём передаём логическую позицию на экране в виде logic_pos
// В фрагментном шейдере значения logic_pos интерполируются с noperspective
void main() {
	SendVertex(-1,-1);
	SendVertex(-1,+1);
	SendVertex(+1,-1);
	SendVertex(+1,+1);
	EndPrimitive();
}
