#version 330 core
layout (location = 0) in vec3 aPosition;

flat out vec3 startPos;
out vec3 vertPos;

uniform mat4 uViewProjectionMat;

void main() {
	vec4 pos = vec4(aPosition, 1.0) * uViewProjectionMat;
	gl_Position = pos;
	vertPos = pos.xyz / pos.w;
	startPos = vertPos;
}