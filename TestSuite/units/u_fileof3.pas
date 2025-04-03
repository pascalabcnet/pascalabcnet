unit u_fileof3;
type 
  TRec2 = record
            a : integer;
            b : string[6];
          end;
          
  TRec = record
          b : byte;
          sh : shortint;
          sm : smallint;
          w : word;
          lw : longword;
          i : integer;
          li : int64;
          ui : uint64;
          c : char;
          arr : array[1..3] of integer;
          arr2 : array[1..3] of set of byte;
          arr4 : array[1..3,1..4] of real;
          str1 : string[6];
          s : set of byte;
          arr3 : array[1..2] of TRec2;
          str2 : ShortString;
          rec1 : TRec2;
          r : real;
          f : single;
          dp : 1..8;
         end;

var rec, rec2 : TRec; 
    //f : file of TRec;
    
begin
  {Assign(f,'test5.dat');
  Rewrite(f);
  rec2.b := 1;
  rec2.sh := 2;
  rec2.sm := 3;
  rec2.w := 4;
  rec2.lw := 5;
  rec2.i := -100;
  rec2.li := 23;
  rec2.ui := 25;
  rec2.str1 := 'Privet';
  rec2.str2 := '1234567890';
  rec2.rec1.b := 'Privet';
  rec2.arr[2] := 111;
  rec2.arr2[1] := [1,3,45,78,99];
  rec2.arr4[1,3] := 2.6;
  rec2.c := 'h';
  rec2.arr3[1].b := 'Privet';
  rec2.s := [1,5,7,11];
  rec2.r := 3.14;
  rec2.f := 2.71;
  Write(f, rec2);
  Write(f, rec2);
  Close(f);
  Reset(f);
  Seek(f,1);
  Read(f,rec);
  assert(rec.b = 1);
  assert(rec.sh = 2);
  assert(rec.sm = 3);
  assert(rec.w = 4);
  assert(rec.lw = 5);
  assert(rec.i = -100);
  assert(rec.li = 23);
  assert(rec.ui = 25);
  assert(rec.str1 = 'Privet');
  assert(rec.str2 = '1234567890');
  assert(rec.rec1.b = 'Privet');
  assert(rec.arr[2] = 111);
  assert(rec.arr2[1] = [1,3,45,78,99]);
  assert(rec.arr4[1,3] = 2.6);
  assert(rec.c = 'h');
  assert(rec.arr3[1].b = 'Privet');
  assert(rec.s = [1,5,7,11]);
  assert(rec.r = 3.14);
  assert(rec.f = single(2.71));
  
  Close(f);}
end.