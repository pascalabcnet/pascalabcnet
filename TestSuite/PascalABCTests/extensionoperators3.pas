function operator*(s1:string;s2:single):byte; extensionmethod := 1;

function GetSingle := single(0);

begin
  var a := '' * GetSingle();
  assert(a=1);
end.