using System.Drawing;
using App.Static;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using Pastel;
using static App.Utils.CLI;

namespace App.Steps;
/* --- setting up nodejs --- */
/*
 * cd to user home
 *   `cd ~`
 * use one of the install scripts:
 *   `wget -qO- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash`
 * restart bashrc
 *   `source ~/.bashrc`
 * ensure everything is working
 *   `nvm -v`
 * install latest node lts
 *   `nvm install --lts`
 * ensure node was installed
 *   `node -v`
 * ensure npm was installed
 *   `npm -v`
 * install essential dependencies for some npm packages after updating apt
 *   `sudo apt update`
 *   `sudo apt install build-essential`
 * install pm2
 *   `npm install pm2@latest -g`
 * add pm2 to startup list
 * 
 * ---
 *   this is where you get the node application on the machine somehow
 * ---
 * add node app to process
 *   `pm2 start /path/to/nodeapp.js`
 * 
 * reminder:
 *   - save pm2 processes with pm2 save (important for server restarts)
 *      `pm2 save`
*/

public static class NodeSteps
{
    public static async Task PerformAll()
    {
        var password = Data.Password;

        Console.WriteLine("Setting up NodeJS related things...".Pastel(Color.Teal));

        await Execute("cd ~");
        await Execute("wget -qO- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash");
        await Execute("source ~/.bashrc");
        await Execute("nvm install --lts");

        await APT.InstallPackage("build-essential");
        await Execute("npm install pm2@latest -g");

        Console.WriteLine("NodeJS set up complete.".Pastel(Color.Chartreuse));
    }
}