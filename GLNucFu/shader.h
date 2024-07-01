//
// Created by remi.cazoulat on 28/06/2024.
//

#ifndef SHADER_H
#define SHADER_H

#include "includes.h"
std::string readShaderCode(const char* filePath);
GLuint compileShader(const char* shaderCode, GLenum shaderType);
GLuint createShaderProgram(const char* vertexPath, const char* fragmentPath);
void createGeometry();
void createUniform1f(const GLchar* name, const float & value);
void createUniformTexVec2(const GLchar* name, const int & width, const int & height, const GLfloat* data, const int & texture);
void render();
void cleanShader();

#endif //SHADER_H
