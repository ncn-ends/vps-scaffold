//Create object of FileInfo for specified path            

using System.Text;
using App.Utils;

// var configPath = @"code/CliWrapper/App/example_file";
// var asd = FileSystem.FetchFileContents(configPath);
// Console.WriteLine(asd);
try
{
    var sshd_config = FileSystem.FetchFileContents("/home/one/code/CliWrapper/App/example_file");
    Console.WriteLine(sshd_config);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
