#include "shader.h"
#include "compute.h"


GLFWwindow* window;
double mouseX, mouseY;
bool leftButtonPressed = false;
bool rightButtonPressed = false;


void mouse_button_callback(const int button,const int action, int mods) {
    if (button == GLFW_MOUSE_BUTTON_LEFT) {
        leftButtonPressed = (action == GLFW_PRESS);
    }
    if (button == GLFW_MOUSE_BUTTON_RIGHT) {
        rightButtonPressed = (action == GLFW_PRESS);
    }
}

void cursor_position_callback(const  double xpos,const double ypos) {
    mouseX = xpos;
    mouseY = ypos;
}

glm::vec2 screenToTextureCoordinates(const double x,const double y) {
    int width, height;
    glfwGetWindowSize(window, &width, &height);
    // Convertir les coordonnées de l'écran (en pixels) en coordonnées de texture (de 0 à 1)
    return {x / width, 1.0 - y / height};
}

void addVelocity(const GLuint computeShader, const double x,const double y) {
    // Convertir les coordonnées de la souris en coordonnées de texture
    const glm::vec2 texCoord = screenToTextureCoordinates(x, y);

    // Envoyer les nouvelles données au compute shader
    glUseProgram(computeShader);
    glUniform2f(glGetUniformLocation(computeShader, "mousePos"), texCoord.x, texCoord.y);
    glUniform1i(glGetUniformLocation(computeShader, "addVelocity"), 1);

    // Lancez le compute shader
    glUseProgram(0);
}

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

GLuint createTextureVec2(const GLfloat * data, const int width, const int height) {
    GLuint texture;
    glGenTextures(1, &texture);
    //glActiveTexture( GL_TEXTURE0 );
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RG32F, width, height, 0, GL_RG, GL_FLOAT, data);
    //glBindImageTexture( 0, texture, 0, GL_FALSE, 0, GL_READ_WRITE, GL_R32F );
    //glBindTexture(GL_TEXTURE_2D, 0);
    return texture;
}
GLuint createTextureVec1(const GLfloat * data, const int width, const int height) {
    GLuint texture;
    glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_R32F, width, height, 0, GL_RED, GL_FLOAT, data);
    glGenerateMipmap(GL_TEXTURE_2D);
    glBindTexture(GL_TEXTURE_2D, 0);
    return texture;
}
// ------------------------------------------------------
// ------------------------------------------------------
// ------------------------------------------------------
// -----------{ Main function }--------------------------
// ------------------------------------------------------
// ------------------------------------------------------
// ------------------------------------------------------
// ---{ M }---
// ---{ A }---
// ---{ I }---
// ---{ N }---
int main() {
    // Init grid
    constexpr int gridWidth = 128 * 2;
    constexpr int gridHeight = 72 * 2;
    constexpr int pixelPerCell = 8;
    constexpr int gridSize = gridWidth * gridHeight;
    constexpr int gridSizex2 = gridSize * 2;
    GLfloat vel[gridSizex2] {0};
    GLfloat density[gridSize] {0};
    printf("[DEBUG] init arrays \n");
    /*
    constexpr auto circleCoord = glm::vec2(128 / 2, 72 / 8);
    constexpr float radius = 10.0;
    for(int j = 0; j < gridHeight ; j ++) {
        for(int i = 0; i < gridWidth; i ++) {
            const float distance = glm::distance(glm::vec2(i, j), circleCoord);
            density[(i + j * gridWidth)] = distance < radius ? 1.0 : 0.3;

            if( j > gridHeight / 2 - 10 && j < gridHeight / 2 + 10) {
                vel[(i + j * gridWidth) * 2] = 1.0;
                vel[(i + j * gridWidth) * 2 + 1] = 0.0;
            } else {
                vel[(i + j * gridWidth) * 2] = 0.0;
                vel[(i + j * gridWidth) * 2 + 1] = 0.0;
            }
        }
    }
    */
    printf("[DEBUG] init arrays values \n");


    // ---------- { Init Window }----------
    constexpr int windowWidth = pixelPerCell * gridWidth;
    constexpr int windowHeight = pixelPerCell * gridHeight;
    printf("[DEBUG] init window size :  %i %i", windowWidth, windowHeight);
    GLFWwindow* window = initWindow(windowWidth, windowHeight);
    printf("[DEBUG] init window done \n");

    // ---------- { Init Textures }----------
    const GLuint velTex = createTextureVec2(vel, gridWidth, gridHeight);
    const GLuint densTex = createTextureVec1(density, gridWidth, gridHeight);
    printf("[DEBUG] init textures done \n");

    // ---------- { Compute program }----------
    const GLuint computeProgram = createComputeProgram("../glsl/cshader.glsl");
    printf("[DEBUG] init compute done \n");


    //glUseProgram(computeProgram);
    //bindingUniformTex(computeProgram, velTex, "velTex", 0);
    //bindingUniformTex(computeProgram, densTex, "densTex", 1);
    // ---------- { Shader program }----------
    createGeometry();
    const GLuint shaderProgram = createShaderProgram("../glsl/vshader.glsl","../glsl/fshader.glsl");
    glUseProgram(shaderProgram);
    createUniform1f(shaderProgram, "gridWidth", gridWidth);
    createUniform1f(shaderProgram, "gridHeight", gridHeight);
    createUniform1f(shaderProgram, "pixelPerCell", pixelPerCell);
    bindingUniformTex(shaderProgram, "velTex", 0);
    bindingUniformTex(shaderProgram, "densTex", 1);
    printf("[DEBUG] init shader done \n");

    // ---------- { Main render loop }----------
    while (!glfwWindowShouldClose(window)) {

        execute(computeProgram, velTex, densTex, gridWidth, gridHeight);
        render(shaderProgram, velTex, densTex);

        // Swap buffers and poll for events
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
    // Clean up
    cleanShader(shaderProgram);
    cleanCompute(computeProgram);
    glfwDestroyWindow(window);
    glfwTerminate();
    return 0;
}