﻿type
  t1 = class end;
  t2 = class end;
  i1 = interface end;

begin
  var a1: t1;
  var a2: t2;
  var a3: i1;
  
  if a1 is t2(var o) then;
end.