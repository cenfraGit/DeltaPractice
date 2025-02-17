cmake_minimum_required(VERSION 3.12)

project(DeltaPractice)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED True)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(wxWidgets_USE_STATIC ON)
set(WEBVIEW2_LIBRARY "C:/wx/3rdparty/webview2/build/native/x64/WebView2LoaderStatic.lib")

add_definitions(-DwxUSE_GUI=1)

find_package(wxWidgets REQUIRED COMPONENTS core base webview)

include_directories(include)
include(${wxWidgets_USE_FILE})

file(GLOB_RECURSE SOURCES "src/*.cpp")

if (WIN32)
    add_executable(DeltaPractice WIN32 ${SOURCES} delta.manifest)
elseif (UNIX)
    add_executable(DeltaPractice ${SOURCES})
endif()

target_include_directories(DeltaPractice PRIVATE)
target_link_libraries(DeltaPractice PRIVATE ${wxWidgets_LIBRARIES} ${WEBVIEW2_LIBRARY})
