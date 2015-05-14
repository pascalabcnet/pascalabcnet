procedure Test;
begin
end;

function Func(a : integer) : integer;
begin
end;

function Func2(var a : integer) : integer;
begin
end;

const cnst1 = 23;

var a : integer;
begin
Func2(cnst1);
end.