#ifndef H_FRAMESTARTSESSION
#define H_FRAMESTARTSESSION

#include <wx/wx.h>
#include <wx/listctrl.h>


class FrameStartSession : public wxFrame {
  wxListCtrl* listctrl_practice_files;
public:
  FrameStartSession(wxFrame* parent);
private:
  void OnExit(wxCommandEvent& event);
};


#endif
