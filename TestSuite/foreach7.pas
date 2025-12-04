type 
  TB = record
    a: integer;
    constructor(a: integer);
    begin
      self.a := a;
    end;
  end;
  
  TA = class(IEnumerable<TB>)
    l := new List<TB>;
  public  
    function GetEnumerator: IEnumerator<TB> := l.GetEnumerator;
    function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator := l.GetEnumerator;
  end;

begin
  var a := new TA;
  a.l.Add(new TB(2));
  a.l.Add(new TB(3));
  var i := 2;
  foreach var x in a do
  begin
    var o: TB := x;;
    assert(o.a = i);
    Inc(i);
  end;
end.