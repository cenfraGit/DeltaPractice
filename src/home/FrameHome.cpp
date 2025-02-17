#include "home/FrameHome.hpp"
#include "ids_enum.hpp"

#include "practice/FramePractice.hpp"

#include <wx/filedlg.h>
#include <wx/textdlg.h>
#include <wx/wx.h>

FrameHome::FrameHome()
  : wxFrame(nullptr, wxID_ANY, "Delta Practice") {

  this->SetMinClientSize(this->FromDIP(wxSize(500, 500)));
  
  wxMenu *menuFile = new wxMenu;
  // menuFile->Append(ID_MENUBAR_FILE_PREFERENCES, "&Preferences...\tCtrl-H",
  // 		   "");
  // menuFile->AppendSeparator();
  menuFile->Append(ID_MENUBAR_FILE_EXIT, "&Exit\tCtrl-Q",
		   "Exit the program.");
  
  wxMenu *menuPractice = new wxMenu;
  menuPractice->Append(ID_MENUBAR_PRACTICE_START, "&Start practice session...",
		       "");
  menuPractice->Append(ID_MENUBAR_PRACTICE_EDIT, "&Edit practice files...",
		       "");
 
  wxMenuBar *menuBar = new wxMenuBar;
  menuBar->Append(menuFile, "&File");
  menuBar->Append(menuPractice, "&Practice");
 
  SetMenuBar(menuBar);
 
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

	wxTextEntryDialog dlg_amount(this, "Amount:", "Enter problem amount:", "");

	if (dlg_amount.ShowModal() == wxID_CANCEL)
		return;

	//wxLogMessage("%s %s", dlg_directory.GetPath(), dlg_amount.GetValue());

	int amount = wxAtoi(dlg_amount.GetValue());

	wxFrame* frame_practice = new FramePractice(this, 
												dlg_directory.GetPath().ToStdString(),
												amount);
	frame_practice->Show(true);


}

