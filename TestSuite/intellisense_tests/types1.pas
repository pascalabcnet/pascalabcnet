type TRec = record
end;

type 
  myint{@type myint = integer@} = integer;
  myptr{@type myptr = ^real@} = ^real;
  mylst{@type mylst = List<integer>@} = List<integer>;
  mylst2{@type mylst2 = List<integer>@} = List<integer>;
  myset{@type myset = set of char@} = set of char;
  myarr{@type myarr = array of (integer,integer)@} = array of (integer, integer);
  myarr2{@type myarr2 = array[,] of List<string>@} = array[,] of List<string>;
  myrec = TRec;
  
begin
  var l: mylst;
  var a: myarr;
  l.Add{@procedure List<>.Add(item: integer);@}(2);
  foreach var el{@var el: char;@} in myset do
  begin
    
  end;
  foreach var el{@var el: integer;@} in l do
  begin
    
  end;
  foreach var el{@var el: (integer,integer);@} in a do
  begin
    
  end;
  var ptr: myptr;
  var r{@var r: real;@} := ptr^;
  var i: myint;
  var ptr2{@var ptr2: ^myint;@} := @i;
  var i2{@var i2: myint;@} := ptr2^;
end.