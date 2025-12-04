function p(a: IEnumerable<integer>): IEnumerable<integer>;
begin
   Result := a.Where(x->x<=a.First())
end;

begin
   var a := Seq(2,3,5,1);
   var b := p(a).ToList;
   
   assert(b.Count = 2);
   assert(b[0] = 2);
   assert(b[1] = 1);
end.