type TDiap = 1..7;
     TDiap2 = byte(1)..byte(7);
     TDiap3 = smallint(1)..smallint(7);
     TDiap4 = shortint(1)..shortint(7);
     TDiap5 = word(1)..word(7);
     TDiap6 = longword(1)..longword(7);
     TDiap7 = int64(1)..int64(7);
     TDiap8 = uint64(1)..uint64(7);

var a : TDiap;
    a2 : TDiap2;
    a3 : TDiap3;
    a4 : TDiap4;
    a5 : TDiap5;
    a6 : TDiap6;
    a7 : TDiap7;
    a8 : TDiap8;

begin
  a := 1; a2 := 1; a3 := 1; a4 := 1; a5 := 1; a6 := 1; a7 := 1; a8 := 1;
  assert(a+a2+a3+a4+a5+a6+a7+a8=8);
  assert(a*a2*a3*a4*a5*a6*a7*a8=1);
  assert(a-a2-a3-a4-a5-a6-a7-a8=-6);
  
  assert(a+a = 2);
  assert(a+a2 = 2);
  assert(a+a3 = 2);
  assert(a+a4 = 2);
  assert(a+a5 = 2);
  assert(a+a6 = 2);
  assert(a+a7 = 2);
  assert(a+a8 = 2);
  
  assert(a*a = 1);
  assert(a*a2 = 1);
  assert(a*a3 = 1);
  assert(a*a4 = 1);
  assert(a*a5 = 1);
  assert(a*a6 = 1);
  assert(a*a7 = 1);
  assert(a*a8 = 1);
  
  assert(a div a = 1);
  assert(a div a2 = 1);
  assert(a div a3 = 1);
  assert(a div a4 = 1);
  assert(a div a5 = 1);
  assert(a div a6 = 1);
  assert(a div a7 = 1);
  assert(a div a8 = 1);
  
  assert(a mod a = 0);
  assert(a mod a2 = 0);
  assert(a mod a3 = 0);
  assert(a mod a4 = 0);
  assert(a mod a5 = 0);
  assert(a mod a6 = 0);
  assert(a mod a7 = 0);
  assert(a mod a8 = 0);
  
  assert(a and a = 1);
  assert(a and a2 = 1);
  assert(a and a3 = 1);
  assert(a and a4 = 1);
  assert(a and a5 = 1);
  assert(a and a6 = 1);
  assert(a and a7 = 1);
  assert(a and a8 = 1);

  assert(a or a = 1);
  assert(a or a2 = 1);
  assert(a or a3 = 1);
  assert(a or a4 = 1);
  assert(a or a5 = 1);
  assert(a or a6 = 1);
  assert(a or a7 = 1);
  assert(a or a8 = 1);
  
  assert(a xor a = 0);
  assert(a xor a2 = 0);
  assert(a xor a3 = 0);
  assert(a xor a4 = 0);
  assert(a xor a5 = 0);
  assert(a xor a6 = 0);
  assert(a xor a7 = 0);
  assert(a xor a8 = 0);
  
  assert(a shl a = 2);
  assert(a shl a2 = 2);
  assert(a shl a3 = 2);
  assert(a shl a4 = 2);
  assert(a shl a5 = 2);
  assert(a shl a6 = 2);
  assert(a shl a7 = 2);
  assert(a shl a8 = 2);
  
  assert(a shr a = 0);
  assert(a shr a2 = 0);
  assert(a shr a3 = 0);
  assert(a shr a4 = 0);
  assert(a shr a5 = 0);
  assert(a shr a6 = 0);
  assert(a shr a7 = 0);
  assert(a shr a8 = 0);
  
  
end.