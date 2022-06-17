using App.State;
using App.Terminal;
using App.Utils;

Speaker.SayAsHeader("VPS Scaffolder", "Created by ncn-ends");

var flagStore = new FlagStore(args);
await StepSequence.Begin(flagStore);

Speaker.SayAsHeader("VPS Setup Complete", "Thanks for using VPS Scaffolder!");

// var asd = args.Contains("--minimal");
// var asd = args.Contains("--http-only");
// var asd = args.Contains("--https-only");
// Console.WriteLine(asd);