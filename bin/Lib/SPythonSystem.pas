{$HiddenIdents}
unit SPythonSystem;

interface

uses PABCSystem;

// Basic IO methods

function input(): string;

function input(s: string): string;

type 
    !print = record
    public
        sep: string;
        &end:string;
        static function Get(sep:string := ' '; &end: string := #10): !Print;
        begin
          Result.sep := sep;
          Result.&end := &end;
        end;
        procedure print(params args: array of object);
        begin
          for var i := 0 to args.length - 2 do
            Write(args[i], sep);
          if args.length <> 0 then 
            Write(args[^1]);
          Write(&end);
        end;
    end;

procedure print(params args: array of object);

// Basic type conversion methods

function int(val: string): integer;
function &type(obj: object): System.Type;

//function int(val: string): integer;
function int(b: boolean): integer;

function str(val: object): string;

function float(val: string): real;

function float(x: integer): real;

// Basic sequence functions

function range(s: integer; e: integer; step: integer): sequence of integer;

function range(e: integer): sequence of integer;

function range(s: integer; e: integer): sequence of integer;

//function all<T>(seq: sequence of T): boolean;

//function any<T>(seq: sequence of T): boolean;

// Standard Math functions

function abs(x: integer): integer;

function abs(x: real): real;

// function floor(x: real): real;

//Standard functions with Lists

function len<T>(lst: PABCSystem.List<T>): integer;

function sorted<T>(lst: PABCSystem.List<T>): PABCSystem.List<T>;

function sum(lst: PABCSystem.List<integer>): integer;

function sum(lst: PABCSystem.List<real>): real;

function !assign<T>(var a: T; b: T): T;

function !pow(x, n: integer): integer;

function !pow(x, n: biginteger): biginteger;

function !pow(x: integer; y: real): real;

function !pow(x: real; y: real): real;

function bigint(x: integer): biginteger;

function !pow_recursion(x, n: integer): integer;

function !pow_recursion(x, n: biginteger): biginteger;

type 
    list<T> = PABCSystem.List<T>;
    biginteger = PABCSystem.BigInteger;

implementation

function input(): string;
begin
  PABCSystem.Print();
  Result := PABCSystem.ReadlnString();
end;

function input(s: string): string;
begin
  PABCSystem.Print(s);
  Result := PABCSystem.ReadlnString();
end;
  
procedure print(params args: array of object);
begin
  !Print.Get().Print(args);
end;

function int(val: string): integer := integer.Parse(val);

function int(b: boolean): integer;
begin
  if b then
    Result := 1
  else
    Result := 0;
end;

function &type(obj: object): System.Type := obj.GetType();

function int(obj: object): integer := Convert.ToInt32(obj);

function str(val: object): string := val.ToString(); 

function float(val: string): real := real.Parse(val);

function float(x: integer): real := PABCSystem.Floor(x);

function range(s: integer; e: integer; step: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - 1, step);
end;

function range(s: integer; e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - 1);
end;

function range(e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(0, e - 1);
end;

//function all<T>(seq: sequence of T): boolean := seq.All(x -> x);

//function any<T>(seq: sequence of T): boolean := seq.Any(x -> x);

function abs(x: integer): integer := if x >= 0 then x else -x;

function abs(x: real): real := PABCSystem.Abs(x);

function len<T>(lst: PABCSystem.List<T>): integer := lst.Count;

function sorted<T>(lst: PABCSystem.List<T>): PABCSystem.List<T>;
begin
  var new_list := new PABCSystem.List<T>(lst);
  new_list.sort();
  Result := new_list;
end;

function sum(lst: PABCSystem.List<integer>): integer := lst.sum();
function sum(lst: PABCSystem.List<real>): real := lst.sum();

function !assign<T>(var a: T; b: T): T;
begin
  a := b;
  Result := a;
end;

function !pow(x, n: biginteger): biginteger;
begin
  if (n < 0) then
    raise new System.ArgumentException('возведение в степень не работает для целой отрицательной степени типа bigint.');
  Result := !pow_recursion(x, n);
end;

function !pow_recursion(x, n: biginteger): biginteger;
begin
  
  if (n = 0) then
    Result := 1
  else begin
    Result := !pow_recursion(x, n div 2);
    Result *= Result;
    if ((n mod 2) = 1) then
      Result *= x;
  end;
end;

function !pow(x, n: integer): integer;
begin
  if (n < 0) then
    raise new System.ArgumentException('возведение в степень не работает для целой отрицательной степени, используйте привидение к типу с плавающей точкой.');
  Result := !pow_recursion(x, n);
end;

function !pow(x: integer; y: real): real := Power(x, y);

function !pow(x: real; y: real): real := Power(x, y);

function !pow_recursion(x, n: integer): integer;
begin
  
  if (n = 0) then
    Result := 1
  else begin
    Result := !pow_recursion(x, n div 2);
    Result *= Result;
    if ((n mod 2) = 1) then
      Result *= x;
  end;
end;

function bigint(x: integer): biginteger;
begin
  Result := x;
end;

function !count<T>(Self: list<T>; val: T): integer; extensionmethod;
begin
  Result := 0;
  for var i := 0 to Self.Count - 1 do
       if val = Self[i] then
         Result += 1;
end;

function pop<T>(Self: list<T>; ind: integer): T; extensionmethod;
begin
  Result := Self[ind];
  Self.RemoveAt(ind);
end;

end.