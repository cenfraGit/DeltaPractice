#include "practice/PageProblem.hpp"
#include "custom_events.hpp"
#include <wx/wx.h>

PageProblem::PageProblem(wxWindow* parent, const wxString& content)
    : wxPanel(parent) {
    wxBoxSizer* sizer = new wxBoxSizer(wxVERTICAL);

    wxWebView* m_web = wxWebView::New(this, wxID_ANY, "", wxDefaultPosition, wxDefaultSize);
    m_web->LoadURL("");
    sizer->Add(m_web, 1, wxEXPAND);

    m_web->AddScriptMessageHandler("wx_msg");
    m_web->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, &PageProblem::OnScriptMessage, this);

    SetSizer(sizer);
}

void PageProblem::OnScriptMessage(wxWebViewEvent& event) {
    wxCommandEvent evt(EVT_CHANGE_PROBLEM, GetId());
    evt.SetString(event.GetString());
    wxPostEvent(GetParent(), evt);
}
