#ifndef H_DELTAFRAME
#define H_DELTAFRAME

#include "wx/wx.h"

class DeltaFrame : public wxFrame {
public:
  DeltaFrame();
private:
  void OnExit(wxCommandEvent& event);
};

#endif
