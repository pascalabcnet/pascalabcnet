var i: integer;
    j: integer;
    
type
  TRec = record
  end;
  
  TExample<T> = class
    event f: Action<T>;
    event f2: Action<List<T>>;
    constructor Create();
    begin
      f += procedure (x) -> begin i := 1; end;
      f2 += procedure (x) -> begin j := 1; end;
    end;
    procedure raiseevent;
    begin
      f(default(T));
      f2(new List<T>);
    end;
  end;
 
begin
  var o: TExample<integer>;
  o := new TExample<integer>();
  o.raiseevent;
  assert(i = 1);
  assert(j = 1);
  i := 0;
  j := 0;
  var o2 := new TExample<TRec>;
  o2.raiseevent;
  assert(i = 1);
  assert(j = 1);
end.