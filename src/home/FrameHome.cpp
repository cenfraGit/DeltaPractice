#include "home/FrameHome.hpp"
#include "practice/FramePractice.hpp"
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
  menu_practice->Append(ID_MENUBAR_PRACTICE_EDIT, "&Create...", "");
 
  wxMenuBar* menubar = new wxMenuBar;
  menubar->Append(menu_file, "&File");
  menubar->Append(menu_practice, "&Practice");
 
  SetMenuBar(menubar);
 
  CreateStatusBar();
  SetStatusText("Welcome to Delta Practice.");
 
  Bind(wxEVT_MENU, &FrameHome::OnExit, this, ID_MENUBAR_FILE_EXIT);
  Bind(wxEVT_MENU, &FrameHome::OnStartSession, this, ID_MENUBAR_PRACTICE_START);
}

void FrameHome::OnExit(wxCommandEvent &event) {
  Close(true);
}

void FrameHome::OnStartSession(wxCommandEvent& event) {

  wxDirDialog dlg_directory(NULL, "Choose input directory", "",
			    wxDD_DEFAULT_STYLE | wxDD_DIR_MUST_EXIST);

  if (dlg_directory.ShowModal() == wxID_CANCEL)
    return;

  //wxTextEntryDialog dlg_amount(this, "Amount:", "Enter problem amount:", "");

  //if (dlg_amount.ShowModal() == wxID_CANCEL)
  //  return;

  //wxLogMessage("%s %s", dlg_directory.GetPath(), dlg_amount.GetValue());

  //int amount = wxAtoi(dlg_amount.GetValue());

  wxFrame* frame_practice = new FramePractice(this, 
					      dlg_directory.GetPath().ToStdString());
  frame_practice->Show(true);
}

void FrameHome::OnCreate(wxCommandEvent& event) {

  
  
}

