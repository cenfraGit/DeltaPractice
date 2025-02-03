#ifndef H_FRAMEHOME
#define H_FRAMEHOME

#include "wx/wx.h"


class FrameHome : public wxFrame {
public:
  FrameHome();
private:
  void OnExit(wxCommandEvent& event);
  void OnStartSession(wxCommandEvent& event);
};


#endif
