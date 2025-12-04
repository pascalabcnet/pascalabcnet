begin
  var a := 3.1;
  var s := $'{a,6:F2}';
  var s2 := string.Format('{0,6:F2}',a);
  assert(s = s2);
  s := $'{a,-10}';
  s2 := string.Format('{0,-10}', a);
  assert(s = s2);
  s := $'a is {a,6:N2}';
  s2 := string.Format('a is {0,6:N2}',a);
  assert(s = s2);
  s := $'{Min(2,3),-5:F2}';
  s2 := string.Format('{0,-5:F2}', Min(2,3));
  assert(s = s2);
  s := $'{Min(2,3):F2}';
  s2 := string.Format('{0:F2}', Min(2,3));
  assert(s = s2);
  var arr1 := Arr(1,2,3);
  s := $'{arr1[2],-5:F2}';
  s2 := string.Format('{0,-5:F2}', arr1[2]);
  var arr2 := new integer[2,2];
  s := $'{arr2[0,0],-5:F2}';
  s2 := string.Format('{0,-5:F2}', arr2[0,0]);
  assert(s = s2);
  s := $'{arr2[0,0]}';
  s2 := string.Format('{0}', arr2[0,0]);
  assert(s = s2);
end.