uses typedef1u;

var i : myint;
    pi : myptrint;
    ppi : myptrintptr;
    arr : myarr;
    dynarr : mydynarr;
    dynarr2 : mydynarr2;
    set1 : myset;
    set2 : myset2;
    enum1 : myenum;
    set3 : myset3;
    set4 : myset4;
    diap : mydiap;
    diap2 : mydiap2;
    ptrdiap : myptrdiap;
    ptrdiap2 : myptrdiap2;
    ptrenum : myptrenum;
    rec : myrec;
    ptrrec :  myptrrec;
    ptrrec2 : myptrrec2;
    rec2 : myrec2;
    tst : array of integer;
    diap1 : 1..4;
    
begin
  i := 2; assert(i=2);
  pi := @i; assert(pi^=2);
  ppi := @pi; assert(ppi^^=2);
  New(ppi); New(ppi^); ppi^^ := 4; assert(ppi^^=4);
  Dispose(ppi^); Dispose(ppi);
  arr[2] := 2; assert(arr[2]=2);
  SetLength(dynarr,5);
  dynarr[2][3] := 4; assert(dynarr[2][3] = 4);
  SetLength(dynarr2,5);
  dynarr2[2] := 34; assert(dynarr2[2] = 34);
  set1 := ['a','d']; 
  assert(set1=['a','d']);
  Exclude(set1,'a'); assert(set1=['d']);
  Include(set1,'k'); assert('k' in set1);
  assert(set1+['f']=['d','k','f']);
  assert(set1-['d']=['k']);
  assert(set1*['k']=['k']);
  Include(set2,3);
  diap1 := 2;
  Include(set2,diap1);
  assert(set2=[2,3]);
  enum1 := two; assert(enum1 = two);
  enum1 := myenum(2); assert(enum1 = three);
  assert(integer(enum1)=2);
  assert(integer(four)=3);
  assert(two<three);
  assert(four>three);
  assert(one <= four); assert(three>=two);
  
  set3 := [one,two]; assert(set3=[one,two]);
  Include(set3,three); 
  assert(three in set3);
  Exclude(set3,three); 
  assert(set3=[one,two]);
  enum1 := two;
  assert(set3<[one,two,three]);
  assert(set3>[enum1]);
  
  set4 := [one..four]; assert(set4=[one..three]);
  Exclude(set4,one); 
  assert(set4=[two,three]);
  diap2 := one; Include(set4,diap2);
  assert(one in set4);
  
  diap := 4; assert(diap=4);
  assert(diap > 2); assert(diap < 6);
  assert(diap <> 3);
  
  diap2 := two;
  assert(diap2 = two);
  assert(diap2 > one);
  assert(diap2 < three);
  assert(diap2 <= two);
  assert(diap2 >= one);
  assert(diap2 <> one);
  
  diap := 3; ptrdiap := @diap;
  assert(ptrdiap^=3);
  New(ptrdiap); ptrdiap^:=5; assert(ptrdiap^=5);
  Dispose(ptrdiap);
  diap2 := three;
  ptrdiap2 := @diap2;
  assert(ptrdiap2^=three);
  New(ptrdiap2); ptrdiap2^ := four;
  assert(ptrdiap2^=four);
  Dispose(ptrdiap2);
 
end.