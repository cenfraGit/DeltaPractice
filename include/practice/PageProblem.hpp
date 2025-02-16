#pragma once

#include <wx/wx.h>

#if !wxUSE_WEBVIEW_WEBKIT && !wxUSE_WEBBIEW_WEBKIT2 && !wxUSE_WEBVIEW && !wxUSE_WEBVIEW_EDGE
#error "A wxWebView backend is required"
#endif
#include <wx/webview.h>

class PageProblem : public wxPanel {
public:
    PageProblem(wxWindow* parent, const wxString& content);
private:
    void OnScriptMessage(wxWebViewEvent& event);
};