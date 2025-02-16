#pragma once

#include "wx/wx.h"


class FrameHome : public wxFrame {
public:
  FrameHome();
private:
  void OnExit(wxCommandEvent& event);
  void OnStartSession(wxCommandEvent& event);
};
