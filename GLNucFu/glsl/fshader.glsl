#version 460 core

in vec2 TexCoords;

out vec4 fragColor;

uniform float gridWidth;
uniform float gridHeight;
uniform float pixelPerCell;

uniform sampler2D velTex;

void main(){
    vec2 currentVel = texture(velTex, TexCoords).xy;
    fragColor = vec4(0.0, currentVel, 1.0);
}
