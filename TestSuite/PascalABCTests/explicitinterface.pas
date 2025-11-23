type
  IEnr = System.Collections.IEnumerator;
  IEnb = System.Collections.IEnumerable;
  TRec = record
    a: integer;
  end;
  
  var a : integer;
  type My = class(IEnumerable<integer>)
  public
    function IEnb.GetEnumerator(): IEnr;
    begin
      a := 1;
    end;
    function GetEnumerator(): IEnumerator<integer>;
    begin
      a := 2;
    end;
    
  end;
  My2 = class(IEnumerable<TRec>)
  public
    function IEnb.GetEnumerator(): IEnr;
    begin
      a := 1;
    end;
    function GetEnumerator(): IEnumerator<TRec>;
    begin
      a := 2;
    end;
    
  end;
begin
var ien: IEnb := new My;
ien.GetEnumerator;
assert(a=1);
var ien2: IEnumerable<integer> := new My;
ien2.GetEnumerator;
assert(a=2);
a := 0;
var mc := new My;
mc.GetEnumerator();
assert(a=2);
var ien3: IEnumerable<TRec> := new My2;
a := 0;
ien3.GetEnumerator();
assert(a=2);
end.