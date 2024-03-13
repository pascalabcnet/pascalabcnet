unit SPythonSystem;

interface

uses PABCSystem;

// Basic IO methods

function input(): string;

procedure print(params lst: array of object);

// Basic type conversion methods

function int(val: string): integer;

// function int(val: object): integer;

function str(val: object): string;

function float(val: string): real;

// function float(val: object): single;

// Basic sequence functions

function range(s: integer; e: integer; step: integer): sequence of integer;

function range(e: integer): sequence of integer;

function range(s: integer; e: integer): sequence of integer;

//function all<T>(seq: sequence of T): boolean;

//function any<T>(seq: sequence of T): boolean;

// Standard Math functions

function abs(x: integer): integer;

function abs(x: real): real;

function floor(x: real): real;

implementation

function input(): string;
begin
  PABCSystem.Print();
  Result := PABCSystem.ReadlnString();
end;

procedure print(params lst: array of object);
begin
  foreach var elem in lst do
    PABCSystem.Print(elem);
end;

function int(val: string): integer := integer.Parse(val);
{
function int(val: object): integer;
begin
  Result := val.GetHashCode();
end;
}
function str(val: object): string := val.ToString(); 

function float(val: string): real := real.Parse(val);

function range(s: integer; e: integer; step: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e-1, step);
end;

function range(s: integer; e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e-1);
end;

function range(e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(0, e-1);
end;

//function all<T>(seq: sequence of T): boolean := seq.All(x -> x);

//function any<T>(seq: sequence of T): boolean := seq.Any(x -> x);

function abs(x: integer): integer := if x >= 0 then x else -x;

function abs(x: real): real := PABCSystem.Abs(x);

function floor(x: real): real := PABCSystem.Floor(x);

end.