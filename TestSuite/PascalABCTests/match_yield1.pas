function f1: sequence of byte;
begin
  match new Object with
    integer(var i): yield i;
    byte(var i): yield i;
    object(var i): yield 66;
  end;
end;

begin 
  Assert(f1.First = 66);
end.