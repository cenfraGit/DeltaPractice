#include "FramePractice.hpp"
#include "custom_events.hpp"
#include <wx/wx.h>
#include <filesystem>
#include <vector>
#include <random>
#include <wx/textfile.h>
#include <string>

FramePractice::FramePractice(wxFrame* parent, const std::string& path_practice) :
  wxFrame(parent, wxID_ANY, "Practice", wxDefaultPosition, wxDefaultSize),
  timer_colour_panel_buttons(this),
  m_path_practice(path_practice) {

  this->SetClientSize(this->FromDIP(wxSize(600, 400)));

  // contains two panels, problem panel and buttons panel
  panel_main = new wxPanel(this);
  sizer_main = new wxBoxSizer(wxVERTICAL);
  panel_main->SetSizer(sizer_main);

  /* ------------ problem view ------------ */

  m_web = wxWebView::New(panel_main, wxID_ANY, "", wxDefaultPosition, wxDefaultSize);
  wxString test = "<h1>test</h1>";
  m_web->SetPage(test, "");
  sizer_main->Add(m_web, 1, wxEXPAND);

  m_web->AddScriptMessageHandler("wx_msg");
  m_web->Bind(wxEVT_WEBVIEW_SCRIPT_MESSAGE_RECEIVED, &FramePractice::OnScriptMessage, this);
  m_web->Bind(wxEVT_WEBVIEW_LOADED, &FramePractice::OnWebviewLoaded, this);

  /* ----- get problems in directory ----- */
  
  for (const auto& entry : std::filesystem::directory_iterator(m_path_practice)) {
    if (entry.is_directory()) {
      directories.push_back(entry.path().string());
    }
  }
  if (directories.empty()) {
    throw std::runtime_error("no directories found");
  }

  gen = std::mt19937(rd());
  dis = std::uniform_int_distribution<>(0, directories.size() - 1);

  // buttons view
  panel_buttons = new wxPanel(panel_main);
  colour_default_panel_buttons = panel_buttons->GetBackgroundColour();
  wxBoxSizer* sizer_buttons = new wxBoxSizer(wxHORIZONTAL);
  wxButton* button_next = new wxButton(panel_buttons, wxID_ANY, "Next");
  button_next->Bind(wxEVT_BUTTON, &FramePractice::OnButtonNext, this);
  sizer_buttons->Add(button_next, 0, wxALL, 5);
  panel_buttons->SetSizer(sizer_buttons);

  sizer_main->Add(panel_buttons, 0, wxEXPAND);
  sizer_main->Layout();

  Bind(EVT_CHANGE_PROBLEM, &FramePractice::OnChangeProblem, this);
  Bind(wxEVT_TIMER, &FramePractice::OnTimer, this);
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

void FramePractice::GoToNextProblem() {

  wxString templateHTML = R"(
         <!DOCTYPE html>
         <html>
         <head>
             <meta charset="utf-8">
             <meta http-equiv="x-ua-compatible" content="ie=edge">
             <meta name="viewport" content="width=device-width, initial-scale=1">
<script src=" https://cdn.jsdelivr.net/npm/katex@0.16.21/dist/katex.min.js "></script>
<link href=" https://cdn.jsdelivr.net/npm/katex@0.16.21/dist/katex.min.css " rel="stylesheet">
         </head>
         <body>
             <div id="user-generated-content"></div>
             <div id="container"></div>
             <script>
document.addEventListener("DOMContentLoaded", function () {

     window.randint = function(min, max) {
       return Math.floor(Math.random() * (max - min + 1)) + min;
     }

     window.randfloat = function(min, max) {
       return Math.random() * (max - min) + min;
     }

     window.choice = function(array) {
       const index = Math.floor(Math.random() * array.length);
       return array[index];
     }

     let firstInputFocused = false;

     window.add_variable = function (varName, varValue) {
    var paragraphs = document.querySelectorAll('p');
    var katexSpans = document.querySelectorAll('.__se__katex.katex[data-exp]');

    paragraphs.forEach(function (paragraph) {
        var regex = new RegExp(`\\[\\[${varName}\\]\\]`, 'g');
        paragraph.innerHTML = paragraph.innerHTML.replace(regex, varValue);
    });

    katexSpans.forEach(function (span) {
	//alert(span.getAttribute("data-exp"));
        var regex = new RegExp(`\\[\\[${varName}\\]\\]`, 'g');
        span.setAttribute('data-exp', span.getAttribute('data-exp').replace(regex, varValue));
	//alert(span.getAttribute("data-exp"));
	katex.render(span.getAttribute('data-exp'), span);

	//renderMathInElement(document.getElementById('katex'))
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
             let input = answerInput.value.trim().toLowerCase();
             let answer = answerString.trim().toLowerCase();
             
             if (input === answer) {
                 resultText.textContent = 'Correct';
                 resultText.classList.add('indicator');
                 window.wx_msg.postMessage("true");
             } else {
                 resultText.textContent = 'Incorrect';
                 resultText.classList.add('indicator');
                 window.wx_msg.postMessage("false");
                 alert("Answer: " + answerString);
                 return;
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
                     window.wx_msg.postMessage("ok");
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

   
   // prevent same problem twice, needs at least two problems.
   int randomIndex;
   do { 
     randomIndex = dis(gen);
   } while (randomIndex == last_directory_index);
   last_directory_index = randomIndex;
   std::string name_problem = directories[randomIndex];

   wxString user_html;
   //std::string path_html = name_problem + "/problem.html";
   
   std::filesystem::path path_name_problem(name_problem);
   
   std::filesystem::path path_name_html("problem.html");
   std::filesystem::path path_html = path_name_problem / path_name_html;
   std::string path_html_str = path_html.string();
   read_to_wxstring(path_html_str, user_html);


   //wxString user_script;
   std::filesystem::path path_name_js("user-script.js");
   std::filesystem::path path_js = path_name_problem / path_name_js;
   std::string path_js_str = path_js.string();
   //std::string path_js = name_problem + "/user-script.js";
   read_to_wxstring(path_js_str, user_script);

   templateHTML.Replace("<div id=\"user-generated-content\"></div>", "<div id=\"user-generated-content\">" + user_html + "</div>");

   m_web->SetPage(templateHTML, "");
}

void FramePractice::OnWebviewLoaded(wxWebViewEvent& event) {
  m_web->RunScript(user_script);
}

void FramePractice::OnButtonNext(wxCommandEvent& event) {
  GoToNextProblem();
}

void FramePractice::OnExit(wxCloseEvent& event) {
  this->Destroy();
}

void FramePractice::OnTimer(wxTimerEvent& event) {
  // sets the panel_buttons background colour to default
  panel_buttons->SetBackgroundColour(colour_default_panel_buttons);
  panel_buttons->Refresh();
  timer_colour_panel_buttons.Stop();
}

 void FramePractice::OnScriptMessage(wxWebViewEvent& event) {
   if (event.GetString() == "ok") {
     panel_buttons->SetBackgroundColour(colour_correct_panel_buttons);
     GoToNextProblem();
   }
   else if (event.GetString() == "true") {
     panel_buttons->SetBackgroundColour(colour_correct_panel_buttons);
   }
   else if (event.GetString() == "false") {
     panel_buttons->SetBackgroundColour(colour_incorrect_panel_buttons);
   }
   panel_buttons->Refresh();
   timer_colour_panel_buttons.Start(timer_colour_interval_ms);
 }

void FramePractice::read_to_wxstring(const std::string& path, wxString& dest) {
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
