using App.Utils;

namespace App.Steps;

public static class SSLSteps
{
    public static async Task PerformAll()
    {
        await APT.InstallPackage("certbot");
        await APT.InstallPackage("python3-certbox-nginx");
        // sudo certbot --nginx -d example.com -d www.example.com
        
    }
}