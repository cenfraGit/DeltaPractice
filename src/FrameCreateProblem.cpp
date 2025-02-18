#include "FrameCreateProblem.hpp"
#include "ids_enum.hpp"
#include <wx/wx.h>

FrameCreateProblem::FrameCreateProblem(wxFrame* parent)
  : wxFrame(parent, wxID_ANY, "Create problem...") {

  this->SetClientSize(this->FromDIP(wxSize(500, 500)));

  // ------------------------------------------------------------
  // menubar
  // ------------------------------------------------------------

  wxMenu* menu_file = new wxMenu;
  menu_file->Append(ID_MENUBAR_FILE_EXIT, "&Exit\tCtrl-Q", "");
  
  wxMenu* menu_practice = new wxMenu;
  menu_practice->Append(ID_MENUBAR_PROBLEM_SAVE, "&Save as...", "");
 
  wxMenuBar* menubar = new wxMenuBar;
  menubar->Append(menu_file, "&File");
  menubar->Append(menu_practice, "&Practice");
 
  SetMenuBar(menubar);
 
  Bind(wxEVT_MENU, &FrameCreateProblem::OnExit, this, ID_MENUBAR_FILE_EXIT);
  Bind(wxEVT_MENU, &FrameCreateProblem::OnSave, this, ID_MENUBAR_PROBLEM_SAVE);

  // ------------------------------------------------------------
  // html
  // ------------------------------------------------------------

  wxString html_suneditor = R"(
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

<link href="https://cdn.jsdelivr.net/npm/suneditor@latest/dist/css/suneditor.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/suneditor@latest/dist/suneditor.min.js"></script>
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/katex@0.16.21/dist/katex.min.css" integrity="sha384-zh0CIslj+VczCZtlzBcjt5ppRcsAmDnRem7ESsYwWwg3m/OaJ2l4x7YBZl9Kxxib" crossorigin="anonymous">

    <script src="https://cdn.jsdelivr.net/npm/katex@0.16.21/dist/katex.min.js" integrity="sha384-Rma6DA2IPUwhNxmrB/7S3Tno0YY7sFu9WSYMCuulLhIqYSGZ2gKCJWIqhBWqMQfh" crossorigin="anonymous"></script>

    <!--<script defer src="https://cdn.jsdelivr.net/npm/katex@0.16.21/dist/contrib/auto-render.min.js" integrity="sha384-hCXGrW6PitJEwbkoStFjeJxv+fSOOQKOPbJxSfM6G5sWZjAyWhXiTIIAmQqnlLlh" crossorigin="anonymous"
        onload="renderMathInElement(document.body);"></script>-->
    <style>
        html, body {
            height: 100%;
            margin: 0;
            padding: 0;
        }
        .editor-container {
            height: 100%;
            width: 100%;
        }
        .sun-editor {
            height: 100%;
        }
    </style>
</head>
<body>
    <div class="editor-container">
        <textarea id="editor" placeholder="Type something..."></textarea>
    </div>
    <script>
        
        // Initialize SunEditor
        const editor = SUNEDITOR.create(document.getElementById('editor'), {
            plugins: [
                'font',
                'fontSize',
                'formatBlock',
                'image',
                'link',
                "math"
            ],
            buttonList: [
                ['undo', 'redo'],
                ['bold', 'italic', 'underline'],
                ['font', 'fontSize', 'formatBlock'],
                ['image', "math"],
                ["codeView"]
            ],
            katex: katex,
            width: '100%',
            height: '100%'
        });

    </script>
</body>
</html>
)";

  wxString html_monaco_editor = R"(
<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<title>Monaco editor</title>
<link rel="stylesheet" data-name="vs/editor/editor.main" href="https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.20.0/min/vs/editor/editor.main.min.css">
</head>
<body>
<div id="container" style="height:400px;border:1px solid black;"></div>
<script src="https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.26.1/min/vs/loader.min.js"></script>
<script>
require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.26.1/min/vs' }});
require(["vs/editor/editor.main"], () => {
  window.editor = monaco.editor.create(document.getElementById('container'), {
    value: `placeholder`,
    language: 'javascript',
    theme: 'vs-dark',
  });
});

document.addEventListener("DOMContentLoaded", (event) => {
  alert(window.editor.getValue());
});

</script>
</body>
</html>

)";

  /* ----- sizer holding two webviews ----- */

  wxBoxSizer* sizer_main = new wxBoxSizer(wxHORIZONTAL);

  /* ------------- left side ------------- */
  

  m_web_editor_sun = wxWebView::New(this, wxID_ANY, "", wxDefaultPosition, wxDefaultSize);
  m_web_editor_sun->SetPage(html_suneditor, "");

  sizer_main->Add(m_web_editor_sun, 1, wxEXPAND);

  /* ------------- right side ------------- */

  m_web_editor_js = wxWebView::New(this, wxID_ANY, "", wxDefaultPosition, wxDefaultSize);
  m_web_editor_js->SetPage(html_monaco_editor, "");

  sizer_main->Add(m_web_editor_js, 1, wxEXPAND);

  /* ----------- set main sizer ----------- */

  this->SetSizer(sizer_main);

}

void FrameCreateProblem::OnExit(wxCommandEvent& event) {
  Close(true);
}

void FrameCreateProblem::OnSave(wxCommandEvent &event) {

  // get save directory with name

  // save suneditor contents to problem.html

  // save monaco editor contents to user-script.js
  
}
