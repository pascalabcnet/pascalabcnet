begin
  Assign(output,'_GenerateLinuxVersion.bat');
  var a := EnumerateAllFiles('LinuxInstallerPABC');
  var b := a.Select(s -> 'copy bin\'+s[20:]+' Release\PascalABCNETLinux\'+s[20:]);//.OrderBy(s->ExtractFileExt(s));
  b := b.Append(#13#10'cd Release');
  b := b.Append(#13#10'..\Utils\pkzipc\pkzipc.exe -add -dir=current PascalABCNETLinux.zip PascalABCNETLinux\*.*');
  b := b.Append(#13#10'cd ..');
  b.PrintLines;
end.