# CMAKE generated file: DO NOT EDIT!
# Generated by "MinGW Makefiles" Generator, CMake Version 3.28

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Disable VCS-based implicit rules.
% : %,v

# Disable VCS-based implicit rules.
% : RCS/%

# Disable VCS-based implicit rules.
% : RCS/%,v

# Disable VCS-based implicit rules.
% : SCCS/s.%

# Disable VCS-based implicit rules.
% : s.%

.SUFFIXES: .hpux_make_needs_suffix_list

# Command-line flag to silence nested $(MAKE).
$(VERBOSE)MAKESILENT = -s

#Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

SHELL = cmd.exe

# The CMake executable.
CMAKE_COMMAND = C:\Users\remi.cazoulat\AppData\Local\Programs\CLion\bin\cmake\win\x64\bin\cmake.exe

# The command to remove a file.
RM = C:\Users\remi.cazoulat\AppData\Local\Programs\CLion\bin\cmake\win\x64\bin\cmake.exe -E rm -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug

# Include any dependencies generated for this target.
include CMakeFiles/GLNucFu.dir/depend.make
# Include any dependencies generated by the compiler for this target.
include CMakeFiles/GLNucFu.dir/compiler_depend.make

# Include the progress variables for this target.
include CMakeFiles/GLNucFu.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/GLNucFu.dir/flags.make

CMakeFiles/GLNucFu.dir/main.cpp.obj: CMakeFiles/GLNucFu.dir/flags.make
CMakeFiles/GLNucFu.dir/main.cpp.obj: C:/Users/remi.cazoulat/Documents/GitHub/NucFu/GLNucFu/main.cpp
CMakeFiles/GLNucFu.dir/main.cpp.obj: CMakeFiles/GLNucFu.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color "--switch=$(COLOR)" --green --progress-dir=C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building CXX object CMakeFiles/GLNucFu.dir/main.cpp.obj"
	C:\Users\remi.cazoulat\AppData\Local\Programs\CLion\bin\mingw\bin\g++.exe $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT CMakeFiles/GLNucFu.dir/main.cpp.obj -MF CMakeFiles\GLNucFu.dir\main.cpp.obj.d -o CMakeFiles\GLNucFu.dir\main.cpp.obj -c C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\main.cpp

CMakeFiles/GLNucFu.dir/main.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color "--switch=$(COLOR)" --green "Preprocessing CXX source to CMakeFiles/GLNucFu.dir/main.cpp.i"
	C:\Users\remi.cazoulat\AppData\Local\Programs\CLion\bin\mingw\bin\g++.exe $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\main.cpp > CMakeFiles\GLNucFu.dir\main.cpp.i

CMakeFiles/GLNucFu.dir/main.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color "--switch=$(COLOR)" --green "Compiling CXX source to assembly CMakeFiles/GLNucFu.dir/main.cpp.s"
	C:\Users\remi.cazoulat\AppData\Local\Programs\CLion\bin\mingw\bin\g++.exe $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\main.cpp -o CMakeFiles\GLNucFu.dir\main.cpp.s

# Object files for target GLNucFu
GLNucFu_OBJECTS = \
"CMakeFiles/GLNucFu.dir/main.cpp.obj"

# External object files for target GLNucFu
GLNucFu_EXTERNAL_OBJECTS =

GLNucFu.exe: CMakeFiles/GLNucFu.dir/main.cpp.obj
GLNucFu.exe: CMakeFiles/GLNucFu.dir/build.make
GLNucFu.exe: CMakeFiles/GLNucFu.dir/linkLibs.rsp
GLNucFu.exe: CMakeFiles/GLNucFu.dir/objects1.rsp
GLNucFu.exe: CMakeFiles/GLNucFu.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color "--switch=$(COLOR)" --green --bold --progress-dir=C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug\CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Linking CXX executable GLNucFu.exe"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles\GLNucFu.dir\link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/GLNucFu.dir/build: GLNucFu.exe
.PHONY : CMakeFiles/GLNucFu.dir/build

CMakeFiles/GLNucFu.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles\GLNucFu.dir\cmake_clean.cmake
.PHONY : CMakeFiles/GLNucFu.dir/clean

CMakeFiles/GLNucFu.dir/depend:
	$(CMAKE_COMMAND) -E cmake_depends "MinGW Makefiles" C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug C:\Users\remi.cazoulat\Documents\GitHub\NucFu\GLNucFu\cmake-build-debug\CMakeFiles\GLNucFu.dir\DependInfo.cmake "--color=$(COLOR)"
.PHONY : CMakeFiles/GLNucFu.dir/depend

