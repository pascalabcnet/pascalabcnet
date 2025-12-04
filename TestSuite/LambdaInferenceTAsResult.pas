function Inv<T>(p: ()->T): T := p();

begin
  Assert(Inv(()->1)=1)
end.