using App.State;
using App.Terminal;
using App.Utils;

Speaker.SayAsHeader("VPS Scaffolder", "Created by ncn-ends");

new FlagStore(args);
await StepSequence.Begin();

Speaker.SayAsHeader("VPS Setup Complete", "Thanks for using VPS Scaffolder!");
ColorPrinter.CallToAction("It's recommended you close this ssh connection and login with your new user created account. Also, consider turning off root access to the server.");