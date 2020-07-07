unit SF;

procedure Pr(params a: array of object) := Print(a);
procedure Pr(o: object) := Print(o);
procedure Pr(s: string) := Print(s);
procedure Prln(params a: array of object) := Println(a);

function RI := ReadInteger;
function RBI := ReadBigInteger;
function RI64 := ReadInt64;
function RR := ReadReal;
function RC := ReadChar;
function RS := ReadString;

function RlnI := ReadlnInteger;
function RlnBI := ReadlnBigInteger;
function RlnI64 := ReadlnInt64;
function RlnR := ReadlnReal;
function RlnC := ReadlnChar;
function RlnS := ReadlnString;

function RI2 := ReadInteger2;
function RR2 := ReadReal2;
function RC2 := ReadChar2;
function RS2 := ReadString2;

function RlnI2 := ReadlnInteger2;
function RlnR2 := ReadlnReal2;
function RlnC2 := ReadlnChar2;
function RlnS2 := ReadlnString2;

function RI3 := ReadInteger3;
function RR3 := ReadReal3;
function RC3 := ReadChar3;
function RS3 := ReadString3;

function RlnI3 := ReadInteger3;
function RlnR3 := ReadlnReal3;
function RlnC3 := ReadlnChar3;
function RlnS3 := ReadlnString3;

function RI4 := ReadInteger4;
function RR4 := ReadReal4;
function RC4 := ReadChar4;
function RS4 := ReadString4;

function RlnI4 := ReadInteger4;
function RlnR4 := ReadlnReal4;
function RlnC4 := ReadlnChar4;
function RlnS4 := ReadlnString4;

function RAI(n: integer) := ReadArrInteger(n);
function RAR(n: integer) := ReadArrReal(n);

function Pr(Self: integer): integer; extensionmethod := Self.Print;
function Pr(Self: real): real; extensionmethod := Self.Print;
function Pr(Self: Biginteger): Biginteger; extensionmethod := Self.Print;
function Pr(Self: char): char; extensionmethod := Self.Print;
function Pr(Self: boolean): boolean; extensionmethod := Self.Print;
function Pr(Self: string): string; extensionmethod := Self.Print;

function Pr<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod := Self.Print(w);

function Prln(Self: integer): integer; extensionmethod := Self.Println;
function Prln(Self: real): real; extensionmethod := Self.Println;
function Prln(Self: Biginteger): Biginteger; extensionmethod := Self.Println;
function Prln(Self: char): char; extensionmethod := Self.Println;
function Prln(Self: boolean): boolean; extensionmethod := Self.Println;
function Prln(Self: string): string; extensionmethod := Self.Println;

procedure ReMin(var min: integer; x: integer);
begin
  if x < min then 
    min := x
end;

procedure ReMax(var max: integer; x: integer);
begin
  if x > max then 
    max := x
end;

procedure ReMin(var min: real; x: real);
begin
  if x < min then 
    min := x
end;

procedure ReMax(var max: real; x: real);
begin
  if x > max then 
    max := x
end;

function ToI(Self: string); extensionmethod := Self.ToInteger;

function ToR(Self: string); extensionmethod := Self.ToReal;

function operator-(c,c1: char): integer; extensionmethod := Ord(c) - Ord(c1);

function Len(Self: string): integer; extensionmethod := Self.Length;

end.