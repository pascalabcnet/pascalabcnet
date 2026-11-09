unit ReexportOrigin;

interface

function F1(n: integer): integer;
function F1(s: string): integer;
procedure F2(var n: integer);
function Hidden: integer;

implementation

function F1(n: integer): integer := n + 1;
function F1(s: string): integer := s.Length;
procedure F2(var n: integer) := n += 2;
function Hidden: integer := 42;

end.
