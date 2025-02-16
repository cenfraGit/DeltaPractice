#pragma once

#include <wx/event.h>
#include <wx/wx.h>
#include "custom_events.hpp"
#include "practice/PageProblem.hpp"
#include <wx/timer.h>

class FramePractice : public wxFrame {
public:
    FramePractice(wxFrame* parent);
private:
    wxPanel* panel_main;
    wxBoxSizer* sizer_main;
    wxPanel* panel_buttons;
    wxColour colour_default_panel_buttons;
    wxColour colour_correct_panel_buttons = wxColour(0, 150, 0);
    wxColour colour_incorrect_panel_buttons = wxColour(150, 0, 0);
    wxTimer timer_colour_panel_buttons;
    unsigned int timer_colour_interval_ms = 600;
    std::vector<PageProblem*> m_pages_problems;
    int m_current_page_index;
    wxBoxSizer* m_sizer_page;
    void OnChangeProblem(wxCommandEvent& event);
    void GoToPreviousProblem();
    void GoToNextProblem();
    void OnButtonPrevious(wxCommandEvent& event);
    void OnButtonNext(wxCommandEvent& event);
    void OnExit(wxCloseEvent& event);
    void OnTimer(wxTimerEvent& event);
};