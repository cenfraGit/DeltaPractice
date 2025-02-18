#pragma once

#include <wx/event.h>
#include <wx/wx.h>

#if !wxUSE_WEBVIEW_WEBKIT && !wxUSE_WEBBIEW_WEBKIT2 && !wxUSE_WEBVIEW && !wxUSE_WEBVIEW_EDGE
#error "A wxWebView backend is required"
#endif
#include <wx/webview.h>

class FrameCreateProblem : public wxFrame {
public:
  FrameCreateProblem(wxFrame* parent);
private:
  wxWebView* m_web_editor_sun;
  wxWebView* m_web_editor_js;
  void OnExit(wxCommandEvent& event);
  void OnSave(wxCommandEvent& event);
};

