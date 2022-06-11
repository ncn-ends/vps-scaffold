using System.Net;

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
}