#include "practice/FramePractice.hpp"
#include "custom_events.hpp"
#include <wx/wx.h>

FramePractice::FramePractice(wxFrame* parent) : 
    wxFrame(parent, wxID_ANY, "Practice", wxDefaultPosition, wxDefaultSize),
    timer_colour_panel_buttons(this) {

    this->SetClientSize(this->FromDIP(wxSize(600, 400)));

    // contains two panels, problem panel and buttons panel
    panel_main = new wxPanel(this);
    sizer_main = new wxBoxSizer(wxVERTICAL);
    panel_main->SetSizer(sizer_main);

    // problem view
    m_sizer_page = new wxBoxSizer(wxVERTICAL);
    sizer_main->Add(m_sizer_page, 1, wxEXPAND);

    m_pages_problems.push_back(new PageProblem(panel_main, "Page 1"));
    m_pages_problems.push_back(new PageProblem(panel_main, "Page 2"));
    m_pages_problems.push_back(new PageProblem(panel_main, "Page 3"));

    for (auto page : m_pages_problems) {
        m_sizer_page->Add(page, 1, wxEXPAND);
        page->Hide();
    }

    m_current_page_index = 0;
    m_pages_problems[m_current_page_index]->Show();

    // buttons view
    panel_buttons = new wxPanel(panel_main);
    colour_default_panel_buttons = panel_buttons->GetBackgroundColour();
    wxBoxSizer* sizer_buttons = new wxBoxSizer(wxHORIZONTAL);
    wxButton* button_previous = new wxButton(panel_buttons, wxID_ANY, "Previous");
    wxButton* button_next = new wxButton(panel_buttons, wxID_ANY, "Next");

    button_previous->Bind(wxEVT_BUTTON, &FramePractice::OnButtonPrevious, this);
    button_next->Bind(wxEVT_BUTTON, &FramePractice::OnButtonNext, this);

    sizer_buttons->Add(button_previous, 0, wxALL, 5);
    sizer_buttons->Add(button_next, 0, wxALL, 5);
    panel_buttons->SetSizer(sizer_buttons);

    sizer_main->Add(panel_buttons, 0, wxEXPAND);
    sizer_main->Layout();

    Bind(EVT_CHANGE_PROBLEM, &FramePractice::OnChangeProblem, this);

    Bind(wxEVT_TIMER, &FramePractice::OnTimer, this);
    //Bind(wxEVT_CLOSE_WINDOW, &FramePractice::OnExit, this);
}

void FramePractice::OnChangeProblem(wxCommandEvent& event) {
    if (event.GetString() == "true") {
        panel_buttons->SetBackgroundColour(colour_correct_panel_buttons);
        GoToNextProblem();
    }
    else {
        panel_buttons->SetBackgroundColour(colour_incorrect_panel_buttons);
    }
    panel_buttons->Refresh();
    timer_colour_panel_buttons.Start(timer_colour_interval_ms);
}

void FramePractice::GoToPreviousProblem() {
    if (m_current_page_index > 0) {
        m_pages_problems[m_current_page_index]->Hide();
        m_current_page_index--;
        m_pages_problems[m_current_page_index]->Show();
        m_pages_problems[m_current_page_index]->Refresh();
        sizer_main->Layout();
        m_pages_problems[m_current_page_index]->SetFocus();
    }
}

void FramePractice::GoToNextProblem() {
    if (m_current_page_index < m_pages_problems.size() - 1) {
        m_pages_problems[m_current_page_index]->Hide();
        m_current_page_index++;
        m_pages_problems[m_current_page_index]->Show();
        m_pages_problems[m_current_page_index]->Refresh();
        sizer_main->Layout();
        m_pages_problems[m_current_page_index]->SetFocus();
    }
}

void FramePractice::OnButtonPrevious(wxCommandEvent& event) {
    GoToPreviousProblem();
}

void FramePractice::OnButtonNext(wxCommandEvent& event) {
    GoToNextProblem();
}

void FramePractice::OnExit(wxCloseEvent& event) {
    m_pages_problems.clear();
    this->Destroy();
}

void FramePractice::OnTimer(wxTimerEvent& event) {
    // sets the panel_buttons background colour to default
    panel_buttons->SetBackgroundColour(colour_default_panel_buttons);
    panel_buttons->Refresh();
    timer_colour_panel_buttons.Stop();
}