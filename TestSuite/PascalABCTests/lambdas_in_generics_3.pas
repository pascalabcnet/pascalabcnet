// Это холостой тест. В оригинале было where G: integer но это мы запретили
type cl1<T>  = class where T: record, constructor;
  private 
    k: T:= new T();
  public   
  function r<G>(gg: G): T; where G: record;
  begin
    begin
      var l := Seq(1,2,3);
      var fff := 5;
      //var tttt := l.Select(x -> begin fff := fff + 1; result := gg + k + fff + x end).ToList();
      //assert(tttt[0] = 72);
      //assert(tttt[1] = 74);
      //assert(tttt[2] = 76);
    end;
    begin
      var l := Seq(1,2,3);
      var fff := 5;
      //var tttt := l.Select(x -> gg + k + fff).ToList();
      //assert(tttt[0] = 70);
      //assert(tttt[1] = 70);
      //assert(tttt[2] = 70);
    end;
  end;
  public constructor(a:T);
  begin
  end;
end;

procedure pr<T>(a: T); where T: record, constructor;
begin
  var g: cl1<T> := new cl1<T>(a);
  g.r(65);
end;

begin
  pr(2);
end.

