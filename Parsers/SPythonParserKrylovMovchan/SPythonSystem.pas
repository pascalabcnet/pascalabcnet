
unit SPythonSystem;

interface

uses PABCSystem;

// Basic IO methods

function input(prompt: string := ''): string;

procedure print(params lst: array of object);

// Basic type conversion methods

function int(val: string): integer;

// function int(val: object): integer;

function str(val: object): string;

function float(val: string): single;

// function float(val: object): single;

// Sequence generating methods

function range(s: integer; e: integer; step: integer): sequence of integer;

function range(e: integer): sequence of integer;

function range(s: integer; e: integer): sequence of integer;

implementation

function input(prompt: string): string;
begin
  PABCSystem.Print(prompt);
  Result := ReadlnString();
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

function float(val: string): single := single.Parse(val);

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

end.