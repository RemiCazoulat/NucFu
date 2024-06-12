cmake_minimum_required(VERSION 3.25)
project(VkNucFu)
# -------{ INCLUDING GLFW }--------
include(FetchContent)
FETCHCONTENT_DECLARE(GLFW GIT_REPOSITORY https://github.com/glfw/glfw.git)
FETCHCONTENT_MAKEAVAILABLE(GLFW)

# -------{ INCLUDING GLM }--------
find_path(GLM_INCLUDE_DIR NAMES glm/glm.hpp)

include(FindPackageHandleStandardArgs)
find_package_handle_standard_args(GLM DEFAULT_MSG GLM_INCLUDE_DIR)

mark_as_advanced(FORCE GLM_INCLUDE_DIR)

if(NOT TARGET GLM::GLM)
    add_library(GLM::GLM INTERFACE IMPORTED)
    set_target_properties(GLM::GLM PROPERTIES
            INTERFACE_INCLUDE_DIRECTORIES ${GLM_INCLUDE_DIR})
endif()

# -------{ INCLUDING VULKAN SDK }--------

#find_package(Vulkan REQUIRED HINTS $ENV{VULKAN_SDK})
#if (NOT Vulkan_FOUND)
    set(Vulkan_INCLUDE_DIR "$ENV{VULKAN_SDK}/Include")
    set(Vulkan_LIBRARY "$ENV{VULKAN_SDK}/Lib/vulkan-1.lib")
#else ()
    include_directories(${Vulkan_INCLUDE_DIR})
    link_directories(${Vulkan_LIBRARY_DIR})
#endif ()

set(CMAKE_CXX_STANDARD 17)

add_executable(VkNucFu main.cpp)

target_link_libraries(VkNucFu glfw glm ${Vulkan_LIBRARY})
