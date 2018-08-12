begin
  var i0: shortint;
  var i1: byte;
  var i2: smallint;
  var i3: word;
  var i4: integer;
  var i5: longword;
  var i6: int64;
  var i7: uint64;
  var i8: BigInteger;
  var i9: single;
  var ia: real;
  var ib: decimal;
  
  var n := 12;
  
  var b := new string[12,12];
  
  b[0,0] := (True ? i0 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,1] := (True ? i0 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,2] := (True ? i0 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,3] := (True ? i0 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,4] := (True ? i0 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,5] := (True ? i0 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,6] := (True ? i0 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,7] := (True ? i0 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,8] := (True ? i0 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,9] := (True ? i0 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,10] := (True ? i0 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[0,11] := (True ? i0 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);

  b[1,0] := (True ? i1 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,1] := (True ? i1 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,2] := (True ? i1 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,3] := (True ? i1 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,4] := (True ? i1 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,5] := (True ? i1 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,6] := (True ? i1 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,7] := (True ? i1 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,8] := (True ? i1 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,9] := (True ? i1 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,10] := (True ? i1 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[1,11] := (True ? i1 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[2,0] := (True ? i2 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,1] := (True ? i2 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,2] := (True ? i2 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,3] := (True ? i2 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,4] := (True ? i2 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,5] := (True ? i2 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,6] := (True ? i2 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,7] := (True ? i2 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,8] := (True ? i2 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,9] := (True ? i2 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,10] := (True ? i2 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[2,11] := (True ? i2 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[3,0] := (True ? i3 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,1] := (True ? i3 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,2] := (True ? i3 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,3] := (True ? i3 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,4] := (True ? i3 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,5] := (True ? i3 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,6] := (True ? i3 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,7] := (True ? i3 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,8] := (True ? i3 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,9] := (True ? i3 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,10] := (True ? i3 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[3,11] := (True ? i3 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[4,0] := (True ? i4 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,1] := (True ? i4 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,2] := (True ? i4 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,3] := (True ? i4 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,4] := (True ? i4 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,5] := (True ? i4 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,6] := (True ? i4 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,7] := (True ? i4 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,8] := (True ? i4 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,9] := (True ? i4 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,10] := (True ? i4 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[4,11] := (True ? i4 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[5,0] := (True ? i5 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,1] := (True ? i5 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,2] := (True ? i5 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,3] := (True ? i5 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,4] := (True ? i5 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,5] := (True ? i5 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,6] := (True ? i5 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,7] := (True ? i5 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,8] := (True ? i5 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,9] := (True ? i5 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,10] := (True ? i5 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[5,11] := (True ? i5 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[6,0] := (True ? i6 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,1] := (True ? i6 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,2] := (True ? i6 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,3] := (True ? i6 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,4] := (True ? i6 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,5] := (True ? i6 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,6] := (True ? i6 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,7] := (True ? i6 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,8] := (True ? i6 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,9] := (True ? i6 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,10] := (True ? i6 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[6,11] := (True ? i6 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[7,0] := (True ? i7 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,1] := (True ? i7 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,2] := (True ? i7 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,3] := (True ? i7 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,4] := (True ? i7 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,5] := (True ? i7 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,6] := (True ? i7 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,7] := (True ? i7 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,8] := (True ? i7 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,9] := (True ? i7 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,10] := (True ? i7 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[7,11] := (True ? i7 : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  
  b[8,0] := (True ? i8 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,1] := (True ? i8 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,2] := (True ? i8 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,3] := (True ? i8 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,4] := (True ? i8 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,5] := (True ? i8 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,6] := (True ? i8 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,7] := (True ? i8 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,8] := (True ? i8 : i8).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[8,9] := '--'.PadLeft(n);
  b[8,10] := '--'.PadLeft(n);
  b[8,11] := '--'.PadLeft(n);

  b[9,0] := (True ? i9 : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,1] := (True ? i9 : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,2] := (True ? i9 : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,3] := (True ? i9 : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,4] := (True ? i9 : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,5] := (True ? i9 : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,6] := (True ? i9 : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,7] := (True ? i9 : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,8] := '--'.PadLeft(n);
  b[9,9] := (True ? i9 : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,10] := (True ? i9 : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[9,11] := '--'.PadLeft(n);

  b[10,0] := (True ? ia : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,1] := (True ? ia : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,2] := (True ? ia : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,3] := (True ? ia : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,4] := (True ? ia : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,5] := (True ? ia : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,6] := (True ? ia : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,7] := (True ? ia : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,8] := '--'.PadLeft(n);
  b[10,9] := (True ? ia : i9).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,10] := (True ? ia : ia).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[10,11] := '--'.PadLeft(n);

  b[11,0] := (True ? ib : i0).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,1] := (True ? ib : i1).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,2] := (True ? ib : i2).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,3] := (True ? ib : i3).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,4] := (True ? ib : i4).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,5] := (True ? ib : i5).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,6] := (True ? ib : i6).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,7] := (True ? ib : i7).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);
  b[11,8] := '--'.PadLeft(n);
  b[11,9] := '--'.PadLeft(n);
  b[11,10] := '--'.PadLeft(n);
  b[11,11] := (True ? ib : ib).GetType.ToString.Substring(7).ToWords('.').Last.PadLeft(n);

  b.Println(8);
end.