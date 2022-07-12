begin
  Assign(output,'_GenerateLinuxVersion.bat');
  var a := EnumerateAllFiles('LinuxInstallerPABC');
  var b := a.Select(s -> 'copy bin\'+s[20:]+' Release\PascalABCNETLinux\'+s[20:]);//.OrderBy(s->ExtractFileExt(s));
  b := b.Append(#13#10'powershell Compress-Archive -Force Release\PascalABCNETLinux Release\PascalABCNETLinux.zip');
  b.PrintLines;
end.