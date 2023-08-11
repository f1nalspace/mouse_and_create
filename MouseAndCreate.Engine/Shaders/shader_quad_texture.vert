#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexcoord;

out vec2 texCoord;

uniform mat4 uViewProjectionMat;
uniform mat4 uModelMat;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * uModelMat * uViewProjectionMat;
    texCoord = aTexcoord;
}