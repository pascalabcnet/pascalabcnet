unit u_adder_u1;

interface

uses
  u_adder_u2;

var
  iadder: adder<integer>;
  sadder: adder<string>;
  
procedure init;
procedure iprint;
procedure sprint;
function iget: integer;
function sget: string;
procedure iprn(i: integer);
procedure sprn(i: integer);

implementation

procedure init;
begin
  iadder := new adder<integer>(3, 5);
  sadder := new adder<string>('abc', 'def');
end;

procedure iprint;
begin
  iadder.WriteResult;
end;

procedure sprint;
begin
  sadder.WriteResult;
end;

function iget: integer;
begin
  result := iadder.GetResult;
end;

function sget: string;
begin
  result := sadder.GetResult;
end;

procedure iprn(i: integer);
begin
  writeln(iadder.print_imp(i));
end;

procedure sprn(i: integer);
begin
  writeln(sadder.print_imp(i));
end;


end.
