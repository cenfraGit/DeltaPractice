#include "FrameCreateProblem.hpp"
#include "ids_enum.hpp"
#include <wx/wx.h>
#include <wx/textdlg.h>
#include <filesystem>
#include <iostream>
#include <fstream>

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
        window.editor = SUNEDITOR.create(document.getElementById('editor'), {
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
<html>
    <style>
    body {
        overflow: hidden;
    }
    .monaco-ed {
        height: 100%
    }
    </style>
    <body>
        <div class="monaco-ed" id="container"></div>
    <script src="https://www.matrixlead.com/monaco-editor/min/vs/loader.js"></script>
    <script>
        require.config({ paths: { 'vs': 'https://www.matrixlead.com/monaco-editor/min/vs' }}) 
        require(["vs/editor/editor.main"], function () {
          window.editor = monaco.editor.create(document.getElementById('container'), {
            value: '',
            language: 'javascript',
            theme: 'vs-dark',
            minimap: { enabled: false },
            automaticLayout: true,
            scrollBeyondLastLine: false
          });
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

  /* ---------------- dir ---------------- */

  wxDirDialog dlg_directory(NULL, "Where to save?", "",
			    wxDD_DEFAULT_STYLE);

  if (dlg_directory.ShowModal() == wxID_CANCEL)
    return;

  std::filesystem::path path_dir = dlg_directory.GetPath().ToStdString();

  /* ------------ problem name ------------ */

  wxTextEntryDialog dlg_name_problem(this, "Name of problem", "Name:");

  if (dlg_name_problem.ShowModal() == wxID_CANCEL)
    return;

  /* ------------ build paths ------------ */

  std::filesystem::path path_new = path_dir / dlg_name_problem.GetValue().ToStdString();

  /* -------- change current path -------- */

  bool t = std::filesystem::create_directory(path_new);
  std::filesystem::current_path(path_new);

  /* -------------- get data -------------- */

  wxString result_suneditor;
  t = m_web_editor_sun->RunScript("window.editor.getContents();", &result_suneditor);

  wxString result_monaco;
  m_web_editor_js->RunScript("window.editor.getValue();", &result_monaco);

  /* ------------- save data ------------- */

  std::ofstream out_problem_html("problem.html");
  out_problem_html << result_suneditor << std::endl;
  out_problem_html.close();

  std::ofstream out_userscript_js("user-script.js");
  out_userscript_js << result_monaco.ToStdString() << std::endl;
  out_userscript_js.close();

  wxLogMessage("saved");

  Close(true);
  
}
