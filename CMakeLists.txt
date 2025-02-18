cmake_minimum_required(VERSION 3.12)

project(DeltaPractice)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED True)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(wxWidgets_USE_STATIC ON)

add_definitions(-DwxUSE_GUI=1)

find_package(wxWidgets REQUIRED COMPONENTS core base webview)

include_directories(include)
include(${wxWidgets_USE_FILE})

file(GLOB_RECURSE SOURCES "src/*.cpp")

if (WIN32)
    set(WEBVIEW2_LIBRARY "C:/wx/3rdparty/webview2/build/native/x64/WebView2LoaderStatic.lib")
    add_executable(DeltaPractice WIN32 ${SOURCES} delta.manifest)
    target_link_libraries(DeltaPractice PRIVATE ${wxWidgets_LIBRARIES} ${WEBVIEW2_LIBRARY})
elseif (UNIX)
    add_executable(DeltaPractice ${SOURCES})
    target_link_libraries(DeltaPractice PRIVATE ${wxWidgets_LIBRARIES})
endif()

target_include_directories(DeltaPractice PRIVATE)
