using System.Text;
using System;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using CliWrap;
using CliWrap.Buffered;
using Pastel;

namespace App.Utils;

public static class FileSystem
{
    public static string[] CrossPlatformSplit(string content)
    {
        var lines = content.Split(
            new[] {"\r\n", "\r", "\n"},
            StringSplitOptions.None
        );
        return lines;
    }

    /**
     * Reads file contents and parses them into a string, respecting formatting.
     * Expects path to be from user directory
     */
    public static string FetchFileContents(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);

        VerifyFileIsWriteable(fi, filePath);


        // Open file for Read\Write
        FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        //create byte array of same size as FileStream length
        byte[] fileBytes = new byte[fs.Length];

        //define counter to check how much bytes to read. Decrease the counter as you read each byte
        int numBytesToRead = fileBytes.Length;

        //Counter to indicate number of bytes already read
        int numBytesRead = 0;

        //iterate till all the bytes read from FileStream
        while (numBytesToRead > 0)
        {
            int n = fs.Read(fileBytes, numBytesRead, numBytesToRead);

            if (n == 0)
                break;

            numBytesRead += n;
            numBytesToRead -= n;
        }

        //Once you read all the bytes from FileStream, you can convert it into string using UTF8 encoding
        string filestring = Encoding.UTF8.GetString(fileBytes);

        return filestring;
    }

    public static async Task OverwriteFile(string path, string content)
    {
        var lines = CrossPlatformSplit(content);
        await OverwriteFile(path, lines);
    }

    public static async Task OverwriteFile(string path, string[] content)
    {
        await using StreamWriter file = new(path);
        foreach (string line in content)
        {
            await file.WriteLineAsync(line);
        }
    }

    public static async Task EditLine(string path, string signifier, string edit)
    {
        var content = FetchFileContents(path);
        var lines = CrossPlatformSplit(content);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Contains(signifier))
            {
                lines[i] = edit;
            }
        }

        await OverwriteFile(path, lines);
    }

    private static void VerifyFileIsWriteable(FileInfo fi, string filePath)
    {
        if (fi.IsReadOnly)
        {
            Console.WriteLine("File is read-only. Attempting to change permissions.");
            Console.WriteLine($"\t{filePath}");
            File.SetAttributes(filePath, File.GetAttributes(filePath) & ~FileAttributes.ReadOnly);
        }

        if (fi.IsReadOnly)
        {
            throw new UnauthorizedAccessException(
                "File is read-only despite attempt to change it. Are you running the program as sudo?".Pastel(
                    Color.Crimson));
        }
    }

    /*
     * Example: 
     *   var content = StaticFileText.DefaultHtmlFile();
     *   const string path = "somewhere/deeply/nested";
     *   const string filename = "index.html";
     *   await FileSystem.WriteFile(content, path, filename);
     */
    public static async Task WriteFile(string content, string pathToFile, string fileName)
    {
        var lines = CrossPlatformSplit(content);
        await WriteFile(lines, pathToFile, fileName);
    }

    /*
     * Example: 
     *   var content = StaticFileText.DefaultHtmlFile();
     *   const string path = "somewhere/deeply/nested";
     *   const string filename = "index.html";
     *   await FileSystem.WriteFile(content, path, filename);
     */
    public static async Task WriteFile(string[] content, string pathToFile, string fileName)
    {
        if (!Directory.Exists(pathToFile))
        {
            Directory.CreateDirectory(pathToFile);
        }

        var fullPath = Path.Combine(pathToFile, fileName);

        await OverwriteFile(fullPath, content);
    }

    private static async Task GrantFilePermissions(string fullPath)
    {
        var cmd = Cli.Wrap("sudo").WithArguments(new[] {"chmod", "+x", fullPath});
        await cmd.ExecuteBufferedAsync();
    }

    /*
     * This method should be used sparingly, only as a workaround to commands
     * that can't be executed normally through CliWrap due to shell conflicts.
     *
     * Examples of good use cases: `source ~/.bashrc` or using nvm commands
     */
    public static async Task CreateScriptAndRun(string content, string pathToFile, string fileName,
        string? stdin = null, 
        bool cleanup = false)
    {
        var fullPath = Path.Combine(pathToFile, fileName);
        await WriteFile(content, pathToFile, fileName);
        await GrantFilePermissions(fullPath);
        if (stdin is not null)
        {
            await (stdin | Cli.Wrap(fullPath)).ExecuteBufferedAsync();
        }
        else
        {
            await Cli.Wrap(fullPath).ExecuteBufferedAsync();
        }

        if (cleanup)
        {
            File.Delete(fullPath);
        }
    }
}