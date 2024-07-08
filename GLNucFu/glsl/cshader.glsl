#version 460 core

layout (local_size_x = 64, local_size_y = 1) in;

layout (rg32f, binding = 0) uniform image2D vel;
layout (r32f, binding = 1) uniform image2D density0;
layout (r32f, binding = 2) uniform image2D densityTransi;


// Variables à passer en uniform
float k = 0.5;
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

    vec2 nextDensCoord = vec2(coord) - imageLoad(density0, coord).xy * timeStep;
    vec2 flooredDensCoord = floor(nextDensCoord);
    vec2 fractedDensCoord = fract(nextDensCoord);
    float z1 = lerp(imageLoad(density0, ivec2(flooredDensCoord)).x, imageLoad(density0, ivec2(flooredDensCoord) + ivec2(1, 0)).x, fractedDensCoord.x);
    float z2 = lerp(imageLoad(density0, ivec2(flooredDensCoord) + ivec2(0, 1)).x, imageLoad(density0, ivec2(flooredDensCoord) + ivec2(1, 1)).x, fractedDensCoord.x);
    nextDens = lerp(z1, z2, fractedDensCoord.y);

    return nextDens;
}

bool checkBoundaries(ivec2 coord) {
    ivec2 size = imageSize(density0);
    if (coord.x < 0 || coord.x >= size.x || coord.y < 0 || coord.y >= size.y) {
        return false;
    }
    return true;
}



float inversedDiffusion(ivec2 coord) {
    ivec2 size = imageSize(density0);
    ivec2 leftCoord  = ivec2(coord.x - 1, coord.y);
    ivec2 rightCoord = ivec2(coord.x + 1, coord.y);
    ivec2 upCoord    = ivec2(coord.x, coord.y - 1);
    ivec2 downCoord  = ivec2(coord.x, coord.y + 1);
    float dens = imageLoad(density0, coord).x;
    float nextLeftDens = imageLoad(densityTransi, leftCoord).x;
    float nextRightDens = imageLoad(densityTransi, rightCoord).x;
    float nextUpDens = imageLoad(densityTransi, upCoord).x;
    float nextDownDens = imageLoad(densityTransi, downCoord).x;

    float nextDens = (dens + k * (nextLeftDens + nextRightDens + nextUpDens + nextDownDens)) / ( 1.0 + 4 * k);
    return nextDens;
}

float diffusion(ivec2 coord) {
    ivec2 size = imageSize(density0);

    ivec2 leftCoord  = ivec2(coord.x - 1, coord.y);
    ivec2 rightCoord = ivec2(coord.x + 1, coord.y);
    ivec2 upCoord    = ivec2(coord.x, coord.y - 1);
    ivec2 downCoord  = ivec2(coord.x, coord.y + 1);

    float currentDens = imageLoad(density0, coord).x;

    float densLeftValue = imageLoad(density0, leftCoord).x;
    float densRightValue = imageLoad(density0, rightCoord).x;
    float densUpValue = imageLoad(density0, upCoord).x;
    float densDownValue = imageLoad(density0, downCoord).x;

    return (currentDens + k * (densLeftValue + densRightValue + densUpValue + densDownValue)) / ( 1.0 + 4 * k);
}


void main() {

    ivec2 coord = ivec2(gl_GlobalInvocationID.xy);
    //float diffusedDens = diffusion(coord);
    //float advectedDens = advection(coord);
    //float nextDens = diffusedDens;
    //imageStore(density, coord, vec4(nextDens, 0.0, 0.0, 0.0));



    bool isBound = checkBoundaries(coord);
    float density = 0.0;
    if (!isBound) {
        density = diffusion(coord);
    }
    imageStore(density0, coord, vec4(density, 0.0, 0.0, 0.0));

}
