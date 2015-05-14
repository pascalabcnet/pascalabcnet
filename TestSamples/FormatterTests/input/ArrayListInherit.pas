uses //ArrayListInherit_unit,
     System.Collections;

type
  c2 = class(ArrayList)
    constructor(r: integer);
    begin
      inherited Create(r);
    end;
  end;
  
var
  arr: c2;
  //arr2: ArrayListInherit_unit.c2;
  i: integer;
  
begin
  arr := new c2(10);
  arr.Add(7);
  arr.Add(9);
  arr.Add(11);
  writeln(arr.IndexOf(9));
  writeln(arr[1]);
  
  {arr2 := new ArrayListInherit_unit.c2(10);
  arr2.Add(7);
  arr2.Add(9);
  arr2.Add(11);
  writeln(arr2.IndexOf(9));
  writeln(arr2[1]);}

  readln;
end.