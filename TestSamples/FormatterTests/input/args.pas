begin
  Writeln('CommandLineArgs.Length=',CommandLineArgs.Length);
  foreach CommandLineArg: string in CommandLineArgs do
    Writeln(CommandLineArg);
end.