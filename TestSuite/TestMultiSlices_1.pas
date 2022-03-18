procedure AssertEq<T>(m: array [,] of T; a: array of array of T);
begin
  Assert(m.MatrEqual(MatrByRow(a)));
end;

procedure AssertEq<T>(m,a: array [,] of T);
begin
  Assert(m.MatrEqual(a));
end;

procedure AssertEq<T>(m: array of T; a: array of T);
begin
  Assert(m.SequenceEqual(a));
end;

begin
  var d := [[6,7,8],[10,11,12]];
  var m := MatrByRow(||1,2,3,4|,|5,6,7,8|,|9,10,11,12||);
  AssertEq(m[:,:],m);
  AssertEq(m[::1,::1],m);
  AssertEq(m[1:3,1:4],||6,7,8|,|10,11,12||);
  AssertEq(m[::2,::3],||1,4|,|9,12||);
  AssertEq(m[::-2,::-1],||12,11,10,9|,|4,3,2,1||);
  AssertEq(m[^2::-1,^2::-1],||7,6,5|,|3,2,1||);
  AssertEq(m[:^1,:^1],||1,2,3|,|5,6,7||);
  AssertEq(m[1,:],|5,6,7,8|);
  AssertEq(m[^1,:],|9,10,11,12|);
  AssertEq(m[:,^1],|4,8,12|);
  //Print(m[:,^1]);
end.