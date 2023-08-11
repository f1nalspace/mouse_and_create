#version 330 core

flat in vec3 startPos;
in vec3 vertPos;

out vec4 fragColor;

uniform vec4 uColor;
uniform vec2 uViewport;
uniform uint uPattern;
uniform float uFactor;

void main() {
	vec2 dir = (vertPos.xy - startPos.xy) * uViewport / 2.0;
	float dist = length(dir);

	uint bit = uint(round(dist / uFactor)) & 15U;
	if ((uPattern & (1U << bit)) == 0U)
		discard;

	fragColor = uColor;
}