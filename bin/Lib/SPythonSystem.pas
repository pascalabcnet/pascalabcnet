{$HiddenIdents}
unit SPythonSystem;

interface

uses PABCSystem;

// Basic IO methods

function input(): string;

type 
    `Print = record
    public
        sep: string;
        &end:string;
        static function Get(sep:string := ' '; &end: string := #10): `Print;
        begin
          Result.sep := sep;
          Result.&end := &end;
        end;
        procedure Print(params args: array of object);
        begin
          for var i := 0 to args.length - 2 do
            Write(args[i], sep);
          if args.length <> 0 then 
            Write(args[^1]);
          Write(&end);
        end;
    end;

procedure Print(params args: array of object);

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

//Standard functions with Lists

function len<T>(list: List<T>): integer;

function sorted<T>(list: List<T>): PABCSystem.List<T>;

function sum(list: List<integer>): integer;

function sum(list: List<real>): real;

implementation

function input(): string;
begin
  PABCSystem.Print();
  Result := PABCSystem.ReadlnString();
end;
  
procedure Print(params args: array of object);
begin
  `Print.Get().Print(args);
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

function len<T>(list: List<T>): integer := list.Count;

function sorted<T>(list: List<T>): PABCSystem.List<T>;
begin
  var new_list := new PABCSystem.List<T>(list);
  new_list.sort();
  Result := new_list;
end;

function sum(list: List<integer>): integer := list.sum();
function sum(list: List<real>): real := list.sum();

end.