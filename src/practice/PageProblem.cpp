#include "practice/PageProblem.hpp"
#include "custom_events.hpp"
#include <wx/wx.h>
#include <wx/textfile.h>
#include <string>

PageProblem::PageProblem(wxWindow* parent, const std::string& path)
    : wxPanel(parent) {
    wxBoxSizer* sizer = new wxBoxSizer(wxVERTICAL);

    wxWebView* m_web = wxWebView::New(this, wxID_ANY, "", wxDefaultPosition, wxDefaultSize);

    wxString templateHTML = R"(
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="utf-8">
            <meta http-equiv="x-ua-compatible" content="ie=edge">
            <meta name="viewport" content="width=device-width, initial-scale=1">
        </head>
        <body>
            <div id="user-generated-content"></div>
            <div id="container"></div>
            <script>
            document.addEventListener("DOMContentLoaded", function () {

    let firstInputFocused = false;

    window.add_variable = function (varName, varValue) {
        var paragraphs = document.querySelectorAll('p');
        paragraphs.forEach(function (paragraph) {
            var regex = new RegExp(`\\[\\[${varName}\\]\\]`, 'g');
            paragraph.innerHTML = paragraph.innerHTML.replace(regex, varValue);
        });
    };

    window.add_question = function (questionString, answerString) {
        var container = document.getElementById('container');

        var questionContainer = document.createElement('div');
        questionContainer.classList.add('question-container');

        var questionParagraph = document.createElement('p');
        questionParagraph.textContent = questionString;
        questionContainer.appendChild(questionParagraph);

        var answerInput = document.createElement('input');
        answerInput.setAttribute('type', 'text');
        questionContainer.appendChild(answerInput);

        var checkAnswerButton = document.createElement('button');
        checkAnswerButton.textContent = 'Check Answer';
        questionContainer.appendChild(checkAnswerButton);

        var resultText = document.createElement('span');
        questionContainer.appendChild(resultText);

        container.appendChild(questionContainer);

        if (!firstInputFocused) {
            answerInput.focus();
            firstInputFocused = true;
        }

        function checkAnswer() {
            if (answerInput.value === answerString) {
                resultText.textContent = 'Correct';
                resultText.classList.add('indicator');
            } else {
                resultText.textContent = 'Incorrect';
                resultText.classList.add('indicator');
            }

            var nextInput = answerInput.closest('.question-container').nextElementSibling?.querySelector('input');
            if (nextInput) {
                nextInput.focus();
            }

            if (answerInput === container.querySelector('.question-container:last-child input')) {
                let allCorrect = true;
                container.querySelectorAll('.question-container').forEach(container => {
                    if (container.querySelector('span').textContent === 'Incorrect') {
                        allCorrect = false;
                        container.querySelector('input').classList.add('indicator');
                    }
                });

                if (allCorrect) {
                    window.wx_msg.postMessage("true");
                } else {
                    window.wx_msg.postMessage("false");
                }
            }
        }

        checkAnswerButton.addEventListener('click', checkAnswer);

        answerInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                checkAnswer();
            }
        });

    };
});
            </script>
            <script></script>
        </body>
        </html>
    )";

    wxString user_html;
    std::string path_html = path + "\\problem.html";
    read_to_wxstring(path_html, user_html);

    wxString user_script;
    std::string path_js = path + "\\user-script.js";
    read_to_wxstring(path_js, user_script);

    templateHTML.Replace("<div id=\"user-generated-content\"></div>", "<div id=\"user-generated-content\">" + user_html + "</div>");

    m_web->SetPage(templateHTML, "");
    sizer->Add(m_web, 1, wxEXPAND);


    m_web->AddScriptMessageHandler("wx_msg");
    m_web->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, &PageProblem::OnScriptMessage, this);
    m_web->Bind(wxEVT_WEBVIEW_LOADED, [this, m_web, user_script](wxWebViewEvent& event) {
        m_web->RunScript(user_script);
    });

    SetSizer(sizer);
}

void PageProblem::OnScriptMessage(wxWebViewEvent& event) {
    wxCommandEvent evt(EVT_CHANGE_PROBLEM, GetId());
    evt.SetString(event.GetString());
    wxPostEvent(GetParent(), evt);
}

void PageProblem::read_to_wxstring(const std::string& path, wxString& dest) {
    wxTextFile textfile;
    if (!textfile.Open(path)) {
        wxLogError("Cannot open file '%s'.", path);
        return;
    }

    wxString str;
    wxString line = textfile.GetFirstLine();
    str += line + "\n";

    while (!textfile.Eof()) {
        line = textfile.GetNextLine();
        str += line + "\n";
    }
    dest = str;
}