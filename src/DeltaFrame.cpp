#include "../include/DeltaFrame.hpp"
#include "../include/ids_enum.hpp"


DeltaFrame::DeltaFrame()
  : wxFrame(nullptr, wxID_ANY, "Delta Practice") {

  this->SetMinClientSize(wxSize(500, 500));
  
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
 
  Bind(wxEVT_MENU, &DeltaFrame::OnExit, this, ID_MENUBAR_FILE_EXIT);
}


void DeltaFrame::OnExit(wxCommandEvent& event) {
    Close(true);
}
