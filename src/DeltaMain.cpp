#include "../include/DeltaMain.hpp"
#include "../include/DeltaFrame.hpp"

bool DeltaApp::OnInit() {
  DeltaFrame* delta_frame = new DeltaFrame();
  delta_frame->Show(true);
  return true;
}

wxIMPLEMENT_APP(DeltaApp);

