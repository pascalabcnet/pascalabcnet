procedure Test;
begin
end;

function Func2(a : integer) : integer;
begin
end;

function Func2(var a : integer) : integer;
begin
end;

const cnst1 = 23;
      
var a : integer;
    s : string;
begin
 case a of
 1,2,1 : a := 2;
 end;
end.