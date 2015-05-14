unit ArrayListInherit_unit;

uses System.Collections;

type
  c2 = class(ArrayList)
    constructor(r: integer);
    begin
      inherited Create(r);
    end;
  end;
  
end.