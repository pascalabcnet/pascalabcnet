unit procimplunit;

interface

procedure p0;

procedure p1(b: byte);
function f1(i: integer): integer;
function f2(i: integer): integer;

implementation

procedure p0 := exit;

procedure p1{@procedure procimplunit.p1(b: byte);@};
begin
  
end;

function f1{@function procimplunit.f1(i: integer): integer;@};
begin
  
end;

function f2{@function procimplunit.f2(i: integer): integer;@}(i: integer): integer;
begin
  
end;


end.