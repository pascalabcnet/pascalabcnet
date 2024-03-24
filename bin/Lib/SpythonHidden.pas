unit SpythonHidden;

uses PABCSystem;

procedure TestProcedure();
begin
  println('test procedure called');
end;

function Floor(x : real) : integer := PABCSystem.Floor(x); 

end.