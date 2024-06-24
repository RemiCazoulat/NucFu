cmake_minimum_required(VERSION 3.28)
project(GLNucFu)

set(CMAKE_CXX_STANDARD 17)

add_executable(GLNucFu main.cpp)

add_subdirectory(thirdparty/glfw)
add_subdirectory(thirdparty/glm)


target_link_libraries(GLNucFu glfw glm)

