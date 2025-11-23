{$zerobasedstrings}
begin
  var s := 'ABCD'; 
  Assert(s[^1]='D');
  Assert(s[^2]='C');
  Assert(s[^3]='B');
  Assert(s[^4]='A');
end.
