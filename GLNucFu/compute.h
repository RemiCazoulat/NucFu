//
// Created by remi.cazoulat on 28/06/2024.
//

#ifndef COMPUTE_H
#define COMPUTE_H

#include "includes.h"

void printWorkGroupsCapabilities();
GLuint createComputeProgram(const char* computePath);

#endif //COMPUTE_H
