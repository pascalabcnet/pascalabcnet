uses System.Collections.Generic,System;

type AllFun<T> = class
  class function Identity(x: T): T;
  begin
    Result := x;
  end;
end;

function IEnumerable<T>.Sort1: IEnumerable<T>;
begin
  var f: Func<T,T>;
  f := AllFun&<T>.Identity;
  Self.OrderBy(f);
  Result := Self;  
end;

procedure IEnumerable<T>.For_Each1(p: System.Action<T>);
begin
  foreach x: T in Self do
    p(x);
end;

procedure p(x: integer);
begin
  write(x);
end;

{function IEnumerable<T>.Write(delim: string := ' '): IEnumerable<T>;
begin
  if Self.Count=0 then
    exit;
  write(Self.First);  
  foreach x: T in Self.Skip(1) do
    write(delim,x);
  Result := Self;  
end;

function IEnumerable<T>.Writeln(delim: string := ' '): IEnumerable<T>;
begin
  Self.Write(delim);
  writeln;
  Result := Self;  
end;}

function IEnumerable<T>.ZipClassic(b: IEnumerable<T>): IEnumerable<System.Tuple<T,T>>;
begin
  //Result := Self.Zip(b,(x,y)->x*y);
end;

begin
  var a := new integer[4](3,7,2,6); 
  a.Println(',');//.OrderBy(x->x).Println;
  //a.Sort1().Writeln();
  a.OrderBy(AllFun&<integer>.Identity).Println;
  
//  var pp: System.Action<integer> := procedure(x) -> write(x);
//  a.For_Each(pp);
  //a.For_Each(procedure(x) -> write(x));

  
end.

