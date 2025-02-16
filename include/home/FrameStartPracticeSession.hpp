#pragma once

#include <wx/wx.h>
#include <wx/listctrl.h>


class FrameStartPracticeSession : public wxFrame {
  wxListCtrl* listctrl_practice_files;
public:
  FrameStartPracticeSession(wxFrame* parent);
private:
  void OnStart(wxCommandEvent& event);
  void OnExit(wxCommandEvent& event);
};
