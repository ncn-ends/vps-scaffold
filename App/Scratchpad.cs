//Create object of FileInfo for specified path            

using System.Text;
using App.Utils;

// var configPath = @"code/CliWrapper/App/example_file";
// var asd = FileSystem.FetchFileContents(configPath);
// Console.WriteLine(asd);
// try
// {
//     var sshd_config = FileSystem.FetchFileContents("/home/one/code/CliWrapper/App/example_file");
//
//     var linesAfterEdit = FileSystem.EditLine(
//         sshd_config,
//         "PasswordAuthentication yes",
//         "PasswordAuthentication no");
//
//     await FileSystem.OverwriteFile("example_file_output_refactored", linesAfterEdit);
// }
// catch (Exception e)
// {
//     Console.WriteLine(e.ToString());
// }