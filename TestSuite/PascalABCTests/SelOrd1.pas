function Ord1(a: char): integer := 2;
function Ord1(a: integer): boolean := False;

begin
  Assert(|1,2,3,4|.Select(Ord1).First = False)
end.  
  