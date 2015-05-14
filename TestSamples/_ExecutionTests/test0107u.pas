unit test0107u;

type 
  TRec = record a : integer; end; 
  TRec2 = record b : array of real; end;
  TRec3 = record a : TRec2; end;
  TMas = array[1..3] of TRec;
  TRec4 = record
    i := 1;
    j := i;
    rec : TRec := (a:i+3);
    arr : TMas := ((a:i),(a:i+1),(a:i+2));
    arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));
  end;
  
procedure Test;
var i := 1;
    rec : TRec := (a:i+3);
    arr : TMas := ((a:i),(a:i+1),(a:i+2));
    arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));
    
begin
assert(rec.a=4);
assert(arr[2].a=2);
assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
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
    rec : TRec := (a:i+3);
    arr : TMas := ((a:i),(a:i+1),(a:i+2));
    arr2 : array of TRec := ((a:i),(a:i+1),(a:i+2));
    rec2 : TRec2 := (b:(i,i,i));
    rec4 : TRec3 := (a:(b:(i,i,i)));
    rec5 : record a : TRec3 := (a:(b:(i,i,i))) end;
    rec6 : TRec4;
    
const j=1;
      jrec : TRec = (a:j+3);
      jrec5 : TRec = jrec;
      jarr : TMas = ((a:j),(a:j+1),(a:j+2));
      jarr2 : array of TRec = ((a:j),(a:j+1),(a:j+2));
      jrec2 : TRec2 = (b:(j,j,j));
      jrec4 : TRec3 = (a:(b:(j,j,j)));
       
begin
assert(rec.a=4);
assert(arr[2].a=2);
assert(arr2[2].a=3);
assert(rec2.b[0]=i);
assert(rec4.a.b[0]=i);
assert(rec5.a.a.b[0]=i);

assert(rec6.j = 1);
assert(rec6.rec.a=4);
assert(rec6.arr[2].a=2);
assert(rec6.arr2[2].a=3);
assert(rec6.rec2.b[0]=rec6.i);
assert(rec6.rec4.a.b[0]=rec6.i);

assert(jrec5=jrec);
assert(jrec.a=4);
assert(jarr[2].a=2);
assert(jarr2[2].a=3);
assert(jrec2.b[0]=j);
assert(jrec4.a.b[0]=j);

Test;
Test2;
end.