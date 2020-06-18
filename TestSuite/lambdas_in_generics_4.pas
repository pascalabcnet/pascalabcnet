// Это холостой тест. В оригинале было where G: integer но это мы запретили
procedure p<T>(e: IEnumerable<T>); where T:record, constructor;
begin
   var kkk := 5;
   var rr := new List<integer>;
   {for var i := 1 to 4 do
     rr.Add(e.Select(x->begin 
                          var tt: T := new T(); 
                          var jj := e is IEnumerable<T>; 
                          result := x + kkk + i; 
                        end).ToList()[0]);
   assert(rr.Count = 4);
   assert(rr[0] = 11);
   assert(rr[1] = 12);
   assert(rr[2] = 13);
   assert(rr[3] = 14);}
end;

begin
  p(Seq(5));
end.