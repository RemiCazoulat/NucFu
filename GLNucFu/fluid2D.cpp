#include "shader.h"
#include "compute.h"


// Init Window, GLFW and GLAD
GLFWwindow* initWindow(const int & windowWidth, const int & windowHeight) {
    // Initialize GLFW
    if (!glfwInit()) {
        std::cerr << "Failed to initialize GLFW" << std::endl;
        exit(-1);
    }
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    // Create variables

    // Create a GLFW window
    GLFWwindow* window = glfwCreateWindow(windowWidth, windowHeight, "OpenGL 2D Fluid", nullptr, nullptr);
    if (!window) {
        std::cerr << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
    }
    glfwMakeContextCurrent(window);
    // Load OpenGL functions using glfwGetProcAddress
    if (!gladLoadGLLoader(reinterpret_cast<GLADloadproc>(glfwGetProcAddress))) {
        std::cerr << "Failed to initialize OpenGL context" << std::endl;
    }
    return window;
}

// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// -----------{ Main function }-----------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// ---{ M }---
// ---{ A }---
// ---{ I }---
// ---{ N }---

int main() {
    // Init grid
    constexpr int gridWidth = 160 / 4;
    constexpr int gridHeight = 90 / 4;
    constexpr int pixelPerCell = 50;
    constexpr int gridSize = gridWidth * gridHeight * 2;
    GLfloat vel[gridSize];
    for(int j = 0; j < gridHeight ; j ++) {
        for(int i = 0; i < gridWidth; i ++) {
            vel[(i + j * gridWidth) * 2    ] = (float) i / gridWidth;
            vel[(i + j * gridWidth) * 2 + 1] = (float) j / gridHeight;
        }
    }
    // ---------- { Init Window }----------
    constexpr int windowWidth = pixelPerCell * gridWidth;
    constexpr int windowHeight = pixelPerCell * gridHeight;
    GLFWwindow* window = initWindow(windowWidth, windowHeight);

    // ---------- { Compute program }----------
    const GLuint computeProgram = createComputeProgram("../glsl/cshader.glsl");
    glUseProgram(computeProgram);

    // ---------- { Shader program }----------
    createGeometry();
    const GLuint shaderProgram = createShaderProgram("../glsl/vshader.glsl","../glsl/fshader.glsl");
    glUseProgram(shaderProgram);
    createUniform1f( "gridWidth", gridWidth);
    createUniform1f( "gridHeight", gridHeight);
    createUniform1f( "pixelPerCell", pixelPerCell);
    createUniformTexVec2("velTex", gridWidth, gridHeight, vel, 0);

    // ---------- { Main render loop }----------
    while (!glfwWindowShouldClose(window)) {

        glUseProgram(shaderProgram);
        render();
        // Swap buffers and poll for events
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
    // Clean up

    glDeleteProgram(shaderProgram);
    //glDeleteProgram(computeProgram);
    glfwDestroyWindow(window);
    glfwTerminate();
    return 0;
}