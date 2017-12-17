type
  t1 = class
    i: integer;
    function f1: array[,] of byte;
  
  end;

function t1.f1: array[,] of byte;
begin
self.i{@var t1.i: integer;@} := 2;
end;

begin 

end.