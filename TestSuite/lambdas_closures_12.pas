var i: integer;
type
  t2 = class
    procedure Redraw;
    begin
      i := 1;
    end;
    
    constructor(Redraw: boolean);
    begin
      var p:procedure := ()->
      begin
        if Redraw then
          self.Redraw;
      end;
      p;
    end;
  
  end;

begin
new t2(true);
assert(i = 1);
end.