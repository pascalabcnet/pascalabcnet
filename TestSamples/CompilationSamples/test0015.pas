type TRec1 = record a, b : integer; end;
     TRec2 = record
              a : TRec1;
              b : array[1..5] of integer; 
             end;
      TRec3 = record a : array[1..3] of TRec2; end;
      TRec4 = record
              a : set of char;
              end;

var rec1: TRec1;
    rec2 : TRec2;
    rec3 : TRec3;
    rec4 : TRec4;
    arr : array[1..5] of TRec3;
    arr2 : array of TRec3;
    arr3 : array[1..4] of integer;
begin
 arr3[2] := 3;
 writeln(arr3[2]);
 rec1.a := 2; rec1.b := 4;
 rec2.b[2] := 12;
 rec3.a[1].b[3] := 23;
 writeln(rec3.a[1].b[3]);
 rec4.a := ['w','e','p'];
 arr[2].a[2].b[3] := 111;
 writeln(arr[2].a[2].b[3]);
 SetLength(arr2,5);
 arr2[2].a[2].b[3] := 111;
 writeln(arr2[2].a[2].b[3]);
end.