#include "FrameHome.hpp"
#include "FramePractice.hpp"
#include "FrameCreateProblem.hpp"
#include "ids_enum.hpp"

#include <wx/filedlg.h>
#include <wx/textdlg.h>
#include <wx/wx.h>

FrameHome::FrameHome()
  : wxFrame(nullptr, wxID_ANY, "Delta Practice") {

  this->SetMinClientSize(this->FromDIP(wxSize(500, 500)));
  
  wxMenu* menu_file = new wxMenu;
  menu_file->Append(ID_MENUBAR_FILE_EXIT, "&Exit\tCtrl-Q", "Exit the program.");
  
  wxMenu* menu_practice = new wxMenu;
  menu_practice->Append(ID_MENUBAR_PRACTICE_START, "&Load practice files...", "");
  menu_practice->Append(ID_MENUBAR_PRACTICE_CREATE, "&Create problem...", "");
 
  wxMenuBar* menubar = new wxMenuBar;
  menubar->Append(menu_file, "&File");
  menubar->Append(menu_practice, "&Practice");
 
  SetMenuBar(menubar);
 
  CreateStatusBar();
  SetStatusText("Welcome to Delta Practice.");
 
  Bind(wxEVT_MENU, &FrameHome::OnExit, this, ID_MENUBAR_FILE_EXIT);
  Bind(wxEVT_MENU, &FrameHome::OnStartSession, this, ID_MENUBAR_PRACTICE_START);
  Bind(wxEVT_MENU, &FrameHome::OnCreateProblem, this, ID_MENUBAR_PRACTICE_CREATE);
}

void FrameHome::OnExit(wxCommandEvent &event) {
  Close(true);
}

void FrameHome::OnStartSession(wxCommandEvent& event) {

  wxDirDialog dlg_directory(NULL, "Choose input directory", "",
			    wxDD_DEFAULT_STYLE | wxDD_DIR_MUST_EXIST);

  if (dlg_directory.ShowModal() == wxID_CANCEL)
    return;

  wxFrame* frame_practice = new FramePractice(this, 
					      dlg_directory.GetPath().ToStdString());
  frame_practice->Show(true);
}

void FrameHome::OnCreateProblem(wxCommandEvent& event) {
  wxFrame* frame_create_problem = new FrameCreateProblem(this);
  frame_create_problem->Show();
}

