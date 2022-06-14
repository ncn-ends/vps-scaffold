using System.Drawing;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using Pastel;
using static App.Utils.Executor;

namespace App.Steps;

// Console.WriteLine("Configuring Nginx server block...".Pastel(Color.Teal));
// su to new user
// var domainName = await IpGrabber.GrabIpNoHttp();
// sudo mkdir -p /var/www/{domainName}/html (stdin password)
// sudo chown -R $USER:$USER /var/www/{domainName}/html
// sudo chmod -R 755 /var/www/{domainName}
// CopyFile(Files.SampleIndexHtml, /var/www/{domainName}/html/index.html)
// CopyFile(Files.ServerBlockSampleIndexHtml, /etc/nginx/sites-available/{domainName}
// sudo ln -s /etc/nginx/sites-available/{domainName} /etc/nginx/sites-enabled/
// EditLine(/etc/nginx/nginx.conf, "server_names_hash_bucket_size", "server_names_hash_bucket_size 64;")
// sudo nginx -t
// sudo systemctl restart nginx

public static class NginxSteps
{
    public static async Task PerformAll(string password)
    {
        Console.WriteLine("Installing Nginx...".Pastel(Color.Teal));
        await Execute(new[] {"sudo", "apt", "update"}, silently: true);
        await Execute(new[] {"sudo", "apt", "install", "nginx", "-y"}, silently: true);

        Console.WriteLine("Configuring firewall for HTTP requests...".Pastel(Color.Teal));
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx HTTP"});

        var currentIp = await IpGrabber.GrabIpNoHttp();
        Console.WriteLine("Verify that the server is accessible by navigating to it via your client machine's browser."
            .Pastel(Color.Gold));
        Console.WriteLine($"\tGo to: http://{currentIp}/".Pastel(Color.Gold));
        Console.WriteLine("You should be greeted by the default Nginx greeting page.");
        Speaker.SayPressAnyKey();
    }
}