#pragma once

#include <wx/event.h>
#include <wx/wx.h>
#include "custom_events.hpp"
#include <wx/timer.h>
#include <string>
#include <iostream>
#include <random>

#if !wxUSE_WEBVIEW_WEBKIT && !wxUSE_WEBBIEW_WEBKIT2 && !wxUSE_WEBVIEW && !wxUSE_WEBVIEW_EDGE
#error "A wxWebView backend is required"
#endif
#include <wx/webview.h>

class FramePractice : public wxFrame {
public:
  FramePractice(wxFrame* parent, const std::string& path_practicet);
private:
  wxWebView* m_web;
  int last_directory_index;
  std::vector<std::string> directories;
  std::string m_path_practice;
  wxString user_script = "";
  std::random_device rd;
  std::mt19937 gen;
  std::uniform_int_distribution<> dis;
  wxPanel* panel_main;
  wxBoxSizer* sizer_main;
  wxPanel* panel_buttons;
  wxColour colour_default_panel_buttons;
  wxColour colour_correct_panel_buttons = wxColour(0, 150, 0);
  wxColour colour_incorrect_panel_buttons = wxColour(150, 0, 0);
  wxTimer timer_colour_panel_buttons;
  unsigned int timer_colour_interval_ms = 600;
  void OnChangeProblem(wxCommandEvent& event);
  void GoToNextProblem();
  void OnButtonNext(wxCommandEvent& event);
  void OnExit(wxCloseEvent& event);
  void OnTimer(wxTimerEvent& event);
  void OnScriptMessage(wxWebViewEvent& event);
  void OnWebviewLoaded(wxWebViewEvent& event);
  void read_to_wxstring(const std::string& path, wxString& dest);
};
