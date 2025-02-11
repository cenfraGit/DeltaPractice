#include "home/FrameStartPracticeSession.hpp"
#include "practice/FramePractice.hpp"
#include <wx/gdicmn.h>
#include <wx/textctrl.h>
#include <wx/wx.h>
#include <wx/gbsizer.h>
#include <wx/statbox.h>


FrameStartPracticeSession::FrameStartPracticeSession(wxFrame* parent)
  : wxFrame(parent, wxID_ANY, "Start Practice Session", wxPoint(100, 100), wxDefaultSize) {

  this->SetMinClientSize(wxSize(600, 400));

  wxPanel* panel_main = new wxPanel(this);
  wxGridBagSizer* sizer_main = new wxGridBagSizer();

  /* ---------------------- left side ---------------------- */

  listctrl_practice_files = new wxListCtrl(panel_main, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLC_REPORT);
  
  wxListItem col0;
  col0.SetId(0);
  col0.SetText("Available practice files");
  col0.SetWidth(100);
  
  listctrl_practice_files->InsertColumn(0, col0);
  listctrl_practice_files->SetColumnWidth(0, wxLIST_AUTOSIZE_USEHEADER);

  sizer_main->Add(listctrl_practice_files,
		  wxGBPosition(0, 0),
		  wxDefaultSpan,
		  wxEXPAND|wxLEFT,
		  10);

  /* ---------------------- right side ---------------------- */

  // create panel on right side
  wxPanel* panel_right = new wxPanel(panel_main);
  wxBoxSizer* sizer_panel_right = new wxBoxSizer(wxVERTICAL);
  panel_right->SetSizer(sizer_panel_right);

  // create staticbox inside it
  wxStaticBox* sb_setup = new wxStaticBox(panel_right, wxID_ANY, "Setup");
  wxBoxSizer* sizer_sb_setup = new wxBoxSizer(wxVERTICAL);
  sb_setup->SetSizer(sizer_sb_setup);

  // create panel inside staticbox (for easier alignment)
  wxPanel* panel_setup_data = new wxPanel(sb_setup);
  wxGridBagSizer* gbsizer_panel_setup_data = new wxGridBagSizer();
  panel_setup_data->SetSizer(gbsizer_panel_setup_data);

  // create input objects
  wxTextCtrl* textctrl_problem_amount = new wxTextCtrl(panel_setup_data, wxID_ANY, "");

  // add input objects to setup data panel
  gbsizer_panel_setup_data->Add(new wxStaticText(panel_setup_data, wxID_ANY, "Problem amount:"),
				wxGBPosition(0, 0),
				wxGBSpan(1, 1),
				wxALIGN_CENTER_VERTICAL,
				0);
  gbsizer_panel_setup_data->Add(textctrl_problem_amount,
				wxGBPosition(0, 1),
				wxGBSpan(1, 1),
				wxEXPAND,
				0);

  // add setup data panel to staticbox sizer
  sizer_sb_setup->Add(panel_setup_data, 0, wxTOP|wxALIGN_CENTER_HORIZONTAL, 10);
  
  // add staticbox to right side panel
  sizer_panel_right->Add(sb_setup, 1, wxEXPAND|wxLEFT|wxRIGHT, 10);

  // add right panel to main panel
  sizer_main->Add(panel_right,
		  wxGBPosition(0, 1),
		  wxDefaultSpan,
		  wxEXPAND);

  /* ------------------------ bottom ------------------------ */

  wxPanel* panel_bottom = new wxPanel(panel_main);
  wxBoxSizer* sizer_panel_bottom = new wxBoxSizer(wxHORIZONTAL);

  wxButton* button_start = new wxButton(panel_bottom, wxID_ANY, "Start");
  wxButton* button_cancel = new wxButton(panel_bottom, wxID_ANY, "Cancel");

  sizer_panel_bottom->Add(button_cancel, 0);
  sizer_panel_bottom->Add(button_start, 0, wxLEFT, 10);

  panel_bottom->SetSizer(sizer_panel_bottom);

  sizer_main->Add(panel_bottom,
		  wxGBPosition(1, 0),
		  wxGBSpan(1, 2),
		  wxALIGN_RIGHT|wxALL,
		  10);

  /* --------------------- sizer config --------------------- */

  sizer_main->AddGrowableCol(0, 1);
  sizer_main->AddGrowableCol(1, 1);
  sizer_main->AddGrowableRow(0, 1);

  panel_main->SetSizer(sizer_main);

  /* ------------------------ events ------------------------ */

  button_start->Bind(wxEVT_BUTTON, &FrameStartPracticeSession::OnStart, this);
  button_cancel->Bind(wxEVT_BUTTON, &FrameStartPracticeSession::OnExit, this);
}


void FrameStartPracticeSession::OnStart(wxCommandEvent& event) {
  wxFrame* frame_practice = new FramePractice(this);
  frame_practice->Show(true);
}


void FrameStartPracticeSession::OnExit(wxCommandEvent& event) {
    Close(true);
}

