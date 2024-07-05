#version 460 core

layout (local_size_x = 64, local_size_y = 1) in;

layout (rg32f, binding = 0) uniform image2D vel;
layout (r32f, binding = 1) uniform image2D density;

// Variables à passer en uniform
float k = 0.0005;
float timeStep = 0.1;


vec2 difVel(ivec2 coord) {
    vec2 dif = (
    imageLoad(vel, ivec2(coord.x + 1, coord.y)).xy -
    imageLoad(vel, ivec2(coord.x - 1, coord.y)).xy +
    imageLoad(vel, ivec2(coord.x, coord.y + 1)).xy -
    imageLoad(vel, ivec2(coord.x, coord.y - 1)).xy) / 2.0;
    return dif;
}

float lerp(float a, float b, float k) {
    return a + (b - a) * k;
}

float advection(ivec2 coord) {
    float nextDens = 0.0;

    vec2 nextDensCoord = vec2(coord) - imageLoad(density, coord).xy * timeStep;
    vec2 flooredDensCoord = floor(nextDensCoord);
    vec2 fractedDensCoord = fract(nextDensCoord);
    float z1 = lerp(imageLoad(density, ivec2(flooredDensCoord)).x, imageLoad(density, ivec2(flooredDensCoord) + ivec2(1, 0)).x, fractedDensCoord.x);
    float z2 = lerp(imageLoad(density, ivec2(flooredDensCoord) + ivec2(0, 1)).x, imageLoad(density, ivec2(flooredDensCoord) + ivec2(1, 1)).x, fractedDensCoord.x);
    nextDens = lerp(z1, z2, fractedDensCoord.y);

    return nextDens;
}

void inversedDiffusion(ivec2 coord) {
    ivec2 size = imageSize(density);
    ivec2 leftCoord  = clamp(ivec2(coord.x - 1, coord.y), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 rightCoord = clamp(ivec2(coord.x + 1, coord.y), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 upCoord    = clamp(ivec2(coord.x, coord.y - 1), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 downCoord  = clamp(ivec2(coord.x, coord.y + 1), ivec2(0, 0), size - ivec2(1, 1));
    float currentDens = imageLoad(density, coord).x;
    float nextDens = 0.0;
    float nextLeftDens = 0.0;
    float nextRightDens = 0.0;
    float nextUpDens = 0.0;
    float nextDownDens = 0.0;
    for(int i = 0; i < 5; i ++) {
        float nexDensConst = ( nextDens * (1.0 + 4 * k) - currentDens) / k;
        nextDens = (currentDens + k * (nextLeftDens + nextRightDens + nextUpDens + nextDownDens)) / ( 1.0 + 4 * k);
        nextLeftDens = nexDensConst - (nextRightDens + nextUpDens + nextDownDens);
        nextRightDens = nexDensConst - (nextLeftDens + nextUpDens + nextDownDens);
        nextUpDens = nexDensConst - (nextLeftDens + nextRightDens + nextDownDens);
        nextDownDens = nexDensConst - (nextLeftDens + nextRightDens + nextUpDens);
    }
    imageStore(density, coord, vec4(nextDens, 0.0, 0.0, 0.0));
    imageStore(density, leftCoord, vec4(nextLeftDens, 0.0, 0.0, 0.0));
    imageStore(density, rightCoord, vec4(nextRightDens, 0.0, 0.0, 0.0));
    imageStore(density, upCoord, vec4(nextUpDens, 0.0, 0.0, 0.0));
    imageStore(density, downCoord, vec4(nextDownDens, 0.0, 0.0, 0.0));
}

float diffusion(ivec2 coord) {
    ivec2 size = imageSize(density);

    ivec2 leftCoord  = clamp(ivec2(coord.x - 1, coord.y), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 rightCoord = clamp(ivec2(coord.x + 1, coord.y), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 upCoord    = clamp(ivec2(coord.x, coord.y - 1), ivec2(0, 0), size - ivec2(1, 1));
    ivec2 downCoord  = clamp(ivec2(coord.x, coord.y + 1), ivec2(0, 0), size - ivec2(1, 1));

    float currentDens = imageLoad(density, coord).x;

    float densLeftValue = imageLoad(density, leftCoord).x;
    float densRightValue = imageLoad(density, rightCoord).x;
    float densUpValue = imageLoad(density, upCoord).x;
    float densDownValue = imageLoad(density, downCoord).x;

    return (currentDens + k * (densLeftValue + densRightValue + densUpValue + densDownValue)) / ( 1.0 + 4 * k);
}
void main() {

    ivec2 coord = ivec2(gl_GlobalInvocationID.xy);
    float diffusedDens = diffusion(coord);
    float advectedDens = advection(coord);
    float nextDens = diffusedDens;
    imageStore(density, coord, vec4(nextDens, 0.0, 0.0, 0.0));
    //inversedDiffusion(coord);

}
