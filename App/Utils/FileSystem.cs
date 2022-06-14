using System.Text;
using System;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using Pastel;

namespace App.Utils;

public static class FileSystem
{
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
        /* split by lines, platform independent */
        var lines = content.Split(
            new[] {"\r\n", "\r", "\n"},
            StringSplitOptions.None
        );

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
}