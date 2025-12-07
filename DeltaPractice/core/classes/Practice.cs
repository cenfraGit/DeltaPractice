using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.classes;

public class Practice
{

  Dictionary<string, Problem> Problems = new();

  public void AddProblem(string name, Problem problem)
  {
    Problems.Add(name, problem);
  }

  public void GetProblem()
  {

  }
}
