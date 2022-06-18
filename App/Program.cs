using App.State;
using App.Terminal;
using App.Utils;

Speaker.SayAsHeader("VPS Scaffolder", "Created by ncn-ends");

new FlagStore(args);
await StepSequence.Begin();

Speaker.SayAsHeader("VPS Setup Complete", "Thanks for using VPS Scaffolder!");