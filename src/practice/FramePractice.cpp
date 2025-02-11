#include "practice/FramePractice.hpp"
#include <wx/event.h>
#include <wx/wx.h>
#include <wx/scrolwin.h>

class ScrolledPanel : public wxScrolled<wxPanel> {
public:
  ScrolledPanel(wxWindow* parent)
    : wxScrolled<wxPanel>(parent, wxID_ANY) {
      
    wxBoxSizer* sizer = new wxBoxSizer(wxVERTICAL);

    for (int i = 0; i < 50; ++i) {
      sizer->Add(new wxButton(this, wxID_ANY, wxString::Format("Button %d", i)), 0, wxALL, 5);
    }

    this->SetSizer(sizer);
    this->FitInside();
    this->SetScrollRate(5, 5);
  }
};


FramePractice::FramePractice(wxFrame *parent)
  : wxFrame(parent, wxID_ANY, "Practice Session", wxDefaultPosition, wxDefaultSize) {

  this->SetMinClientSize(wxSize(900, 600));

  ScrolledPanel* scrolledPanel = new ScrolledPanel(this);
  wxBoxSizer* sizer = new wxBoxSizer(wxVERTICAL);
  sizer->Add(scrolledPanel, 1, wxEXPAND);
  this->SetSizer(sizer);
  
}

void FramePractice::OnExit(wxCommandEvent& event) {
  Close(true);
}
