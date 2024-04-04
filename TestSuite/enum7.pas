type 
  enum1 = (enval1 = 1);
  enum2 = (enval2);

begin
  var e := enum1.enval1;
  assert(integer(e) = 1);
end.