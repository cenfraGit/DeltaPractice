using Xunit;
using Xunit.Abstractions;
using core.classes;
using core.utils.files;

namespace tests;

public class FileUtilsTests
{
  // [Fact]
  // public void File_CreateTemp()
  // {
  //     // should be created here
  //     string tempAppPath = Path.Combine(Path.GetTempPath(), "DeltaPractice");

  //     // delete if already exists
  //     if (Directory.Exists(tempAppPath))
  //         Directory.Delete(tempAppPath, true);

  //     FileUtils.CreateTempFolderForDeltaIfDontExist();
  //     Assert.True(Directory.Exists(tempAppPath));

  //     // make sure creating twice doesn't break app
  //     FileUtils.CreateTempFolderForDeltaIfDontExist();
  //     Assert.True(Directory.Exists(tempAppPath));
  // }

  // [Fact]
  // public void File_TestUnpacking()
  // {
  //     FileUtils.UnpackProbFile(@"C:\Users\cenfr\OneDrive\Desktop\testProb.prob");
  // }

  [Fact]
  public void Test_Parsing()
  {
    string xmlContents = """
<?xml version="1.0" encoding="UTF-8"?>

<Problem Name="testProblem">
  <Variables>

    <Variable Name="myVar" Type="Integer" LimitLower="1" LimitUpper="5" />
    <Variable Name="myVar2" Type="Decimal" LimitLower="1" LimitUpper="5" />
    <Variable Name="myChoice" Type="Choice">
      <Choice Value="Value1" />
      <Choice Value="5.0" />
      <Choice Value="hi" />
      <Choice Value="-2" />
    </Variable>
  </Variables>

  <Context>
    <Text Value="This is an example paragraph for the context. [myVar] and my choice is [myChoice]"/>
  </Context>

  <Questions>

    <Question Name="question1" Type="TextBox" Text="what is the value of 2*[myVar] is _Ans1_ and 3*[myVar] is _Ans2_">
      <Answer Name="Ans1">
      result = [myVar] * 2;
      </Answer>
      <Answer Name="Ans2">
      result = [myVar] * 3;
      </Answer>
    </Question>

  </Questions>

</Problem>
""";

    Problem p = FileUtils.ParseXMLProb(xmlContents);



  }

  [Fact]
  public void Test_Compression()
  {
    FileUtils.CompressToPrac(
      "C:\\Users\\cenfr\\OneDrive\\Desktop\\PracFile",
      "C:\\Users\\cenfr\\OneDrive\\Desktop\\myFirstPrac.zip");
  }

  [Fact]
  public void Test_Decompression()
  {
    string pracFile = "C:\\Users\\cenfr\\OneDrive\\Desktop\\myFirstPrac.prac";
    FileUtils.DecompressPrac(pracFile);
  }

  [Fact]
  public void Test_ReadPracFile()
  {
    string pracFile = "C:\\Users\\cenfr\\OneDrive\\Desktop\\myFirstPrac.prac";
    FileUtils.ReadPracFile(pracFile);
  }
}
