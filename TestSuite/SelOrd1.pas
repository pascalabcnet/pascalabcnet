###
function Ord1(a: char): integer := 2;
function Ord1(a: integer): boolean := False;

Assert(|1,2,3,4|.Sel(Ord1).First = False)
  