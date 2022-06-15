﻿using App.Static;
using App.Steps;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;

await StepSequence.Begin();

/* --- NOTES --- */
/*
 * some domains are not going to work with http
 *   - e.g. .dev TLDS
 *   - based on HSTS preload list
*/