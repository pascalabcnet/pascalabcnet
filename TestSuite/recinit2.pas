
type 
  TRec = record a : integer; end; 
  TRec2 = record b : array of real; end;
  TRec3 = record a : TRec2; end;
  TMas = array[1..3] of TRec;
  TRec4 = record a : string[4]; end;
  
procedure Test;
var i := 1;
    rec : TRec := (a:i+3);
    arr : TMas := ((a:i),(a:i+1),(a:i+2));
    arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));
    rec6 : TRec4 := (a:'abcde');
    
begin
assert(rec.a=4);
assert(arr[2].a=2);
assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
assert(rec6.a='abcd');
end;

procedure Test2;
var i := 1;
    rec : TRec := (a:i+3);
    //arr : TMas := ((a:i),(a:i+1),(a:i+2));
    //arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));

procedure Nested;
begin
assert(rec.a=4);
//assert(arr[2].a=2);
//assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
end;
    
begin
assert(rec.a=4);
//assert(arr[2].a=2);
//assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
Nested;
end;

var i := 1;
procedure Test3;

begin
  
  var rec : TRec := (a:i+3);
  var arr : TMas := ((a:i),(a:i+1),(a:i+2));
  var arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
  var rec2 : TRec2 := (b:(i,i,i));
  var rec4 : TRec3 := (a:(b:(i,i,i)));
  var rec6 : TRec4 := (a:'abcde');
  assert(rec.a=4);
  assert(arr[2].a=2);
  assert(arr2[2].a=3);
  assert(rec2.b[0]=i);
  assert(rec4.a.b[0]=i);
  assert(rec6.a='abcd');
end;

var
    rec : TRec := (a:i+3);
    arr : TMas := ((a:i),(a:i+1),(a:i+2));
    arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));
    rec5 : record a : TRec3 := (a:(b:(i,i,i))) end;
    rec6 : TRec4 := (a:'abcde');
    
begin
assert(rec.a=4);
assert(arr[2].a=2);
assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
assert(rec5.a.a.b[0]=i);
assert(rec6.a='abcd');
Test;
Test2;
Test3;
end.