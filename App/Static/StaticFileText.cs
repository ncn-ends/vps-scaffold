namespace App.Static;

public static class StaticFileText
{
    public static string NginxServerBlock(string domainName, string portNumber = "3000")
    {
        return $@"server {{
    listen 80;
    listen [::]:80;

    root /var/www/{domainName}/html;
    index index.html index.htm index.nginx-debian.html;

    server_name {domainName} www.{domainName};

    location / {{
        proxy_pass http://localhost:{portNumber};
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }}
}}";
    }

    public static string DefaultHtmlFile()
    {
        return @"<html>
    <head>
        <title>Welcome to your_domain!</title>
    </head>
    <body>
        <h1>Success!  The your_domain server block is working!</h1>
    </body>
</html>";
    }
}