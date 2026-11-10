unit ReexportOrigin;

interface

function F1(n: integer): integer;
function F1(s: string): integer;
procedure F2(var n: integer);
function Hidden: integer;

type
  TOriginPoint = record
    X: integer;
  end;
  TOriginAlias = TOriginPoint;
  TOriginBox<T> = class
  public
    Value: T;
  end;

const
  OriginCount: byte = 7;
  UnchangedConstant = 11;

var OriginCounter := 10;

procedure BumpPoint(var p: TOriginPoint);
function BoxValue(b: TOriginBox<integer>): integer;
function ReadOriginCounter: integer;
function Identity<T>(value: T): T;
function Identity(value: integer): integer;

implementation

var PrivateCounter := 100;

function F1(n: integer): integer := n + 1;
function F1(s: string): integer := s.Length;
procedure F2(var n: integer) := n += 2;
function Hidden: integer := 42;

procedure BumpPoint(var p: TOriginPoint) := p.X += 1;
function BoxValue(b: TOriginBox<integer>): integer := b.Value;
function ReadOriginCounter: integer := OriginCounter;
function Identity<T>(value: T): T := value;
function Identity(value: integer): integer := value + 1;

end.
