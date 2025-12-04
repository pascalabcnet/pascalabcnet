// Это холостой тест. В оригинале было where G: integer но это мы запретили

type cl<T> = class where T: constructor;
  public procedure pr<G>(a: IEnumerable<G>); where G: record;
  begin
    var tt := a.OrderBy(x -> begin var d: T := new T(); result := x end).ToList;
    //assert(tt[0] = 1);
    //assert(tt[1] = 2);
    //assert(tt[2] = 3);
  end;
end;

begin
  var t := new cl<integer>();
  t.pr(Seq(3,2,1));
end.