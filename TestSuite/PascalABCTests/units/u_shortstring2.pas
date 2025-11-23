unit u_shortstring2;
procedure Test(s : string[5]);
begin
end;

type TString = string[3];

procedure Test2(s : set of String);
begin
  assert(s<>['aaa','bbb']);
end;

var s : string[6];
    i := 5;
    set2 : set of 1..3 := [1,2,i];
    s2 : string[2]:='kk';
    s3 : TString := 'ss';
    set1 : set of String := [s2,'bg','cdfg'];
       
begin
set2 := [1,i];
assert(set2=[1,i]);
assert(set1=['kk','bg','cdfg']);
set1 := ['pppp','rst'];
assert(set1=['pppp','rst']);
Test2(['aaad','bbbc']);
Test('a');
s := 'aa';
s += 'bb';
assert(s>'aa');
assert(s ='aabb');
s += 'c';
assert(s = 'aabbc');
s += s2;
assert(s='aabbck');
end.