#version 330 core
in vec2 texCoord;
out vec4 FragColor;

uniform vec4 uColor;
uniform vec4 uTexcoordAdjustment = vec4(0,0,1,1);

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, texCoord * uTexcoordAdjustment.zw + uTexcoordAdjustment.xy) * uColor;
}