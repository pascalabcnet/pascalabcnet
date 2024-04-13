unit SPythonSystem;

interface

uses PABCSystem;

// Basic IO methods

function input(): string;

procedure print(item: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5, item6: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5, item6, item7: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5, item6, item7, item8: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5, item6, item7, item8, item9: object; sep: string := ' '; &end: string := '\n');
procedure print(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10: object; sep: string := ' '; &end: string := '\n');

// Basic type conversion methods

function int(val: string): integer;
function &type(obj: object): System.Type;

//function int(val: string): integer;
function int(b: boolean): integer;

function str(val: object): string;

function float(val: string): real;

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

implementation

function input(): string;
begin
  PABCSystem.Print();
  Result := PABCSystem.ReadlnString();
end;

procedure implPrint(sep: string; params items: array of object);
begin
  PABCSystem.Write(items[0]);
  for var i := 1 to items.Length do
  begin
    PABCSystem.Write(sep);
    PABCSystem.Write(items[i]);
  end;
end;

procedure print(item: object; sep: string; &end: string);
begin
  implPrint(sep, item);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5, item6: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5, item6);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5, item6, item7: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5, item6, item7);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5, item6, item7, item8: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5, item6, item7, item8);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5, item6, item7, item8, item9: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5, item6, item7, item8, item9);
  PABCSystem.Write(&end);
end;

procedure print(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10: object; sep: string; &end: string);
begin
  implPrint(sep, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10);
  PABCSystem.Write(&end);
end;

{
procedure print(params lst: array of object);
begin
  foreach var elem in lst do
    PABCSystem.Print(elem);
end;

procedure println(params lst: array of object);
begin
  foreach var elem in lst do
    PABCSystem.Print(elem);
  PABCSystem.Println(); 
end;
}

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

end.