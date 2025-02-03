#ifndef H_FRAMEPRACTICE
#define H_FRAMEPRACTICE

#include <wx/event.h>
#include <wx/wx.h>


class FramePractice : public wxFrame {
public:
  FramePractice(wxFrame* parent);
private:
  void OnExit(wxCommandEvent& event);
};


#endif
