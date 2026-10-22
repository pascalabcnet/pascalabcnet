//nopabcrtl
// LightPT is a standalone standard unit, not part of PABCRtl.dll.
uses LightPT;

begin
  var values := ObjectList.New.AddRange(Arr(1, 2, 3));
  values.Add('LightPT');
  assert(values <> nil);
  assert(LightPT.EncryptString('LightPT .NET') <> '');
end.
