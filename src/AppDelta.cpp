#include "AppDelta.hpp"
#include "home/FrameHome.hpp"


bool AppDelta::OnInit() {
  FrameHome* frame_home = new FrameHome();
  frame_home->Show(true);
  return true;
}


wxIMPLEMENT_APP(AppDelta);


