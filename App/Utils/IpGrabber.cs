using System.Net;
using CliWrap;
using CliWrap.Buffered;

namespace App.Utils;

public class IpGrabber
{
    public static string GrabIp()
    {
        string externalIpString = new WebClient().DownloadString("http://ipinfo.io/ip").Replace("\\r\\n", "")
            .Replace("\\n", "").Trim();

        var externalIp = IPAddress.Parse(externalIpString);

        return externalIp.ToString();
    }

    public static async Task<string> GrabIpNoHttp()
    {
        var passwdCmd = Cli.Wrap("dig")
            .WithArguments(new[] {"-4", "+short", "myip.opendns.com", "@resolver1.opendns.com"});
        
        var res = await passwdCmd.ExecuteBufferedAsync();
        if (res.StandardError != "")
        {
            Console.WriteLine(res.StandardError);
            throw new Exception();
        }

        return res.StandardOutput.Trim();
    }
}