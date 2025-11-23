type
  T = class 
    c1: integer := 1;
    procedure c2;
    begin
      
    end;
  end;
  
function c1(self: T); extensionmethod := 2;

begin
  var a := new T;
  writeln(a.c1);
end.