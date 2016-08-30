function Test(a: integer): integer;
begin
  
end;

function Test(a: real): integer;
begin
  
end;

function Test(a: string): integer;
begin
  
end;

function Test(a: boolean): integer;
begin
    
end;

function Test(a: List<integer>): integer;
begin
  
end;

begin
  Test{@function Test(a: integer): integer;@}(23);
  Test{@function Test(a: real): integer;@}(2.3);
  Test{@function Test(a: string): integer;@}('333');
  Test{@function Test(a: boolean): integer;@}(true);
  Test{@function Test(a: List<integer>): integer;@}(Lst(2,3,4));
end.