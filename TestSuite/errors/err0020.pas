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
begin
 Func2(a);
end.