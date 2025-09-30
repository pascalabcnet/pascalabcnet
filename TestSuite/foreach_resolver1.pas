{$reference 'foreach_resolver_lib.dll'}

uses System;

type
  c0 = class
    procedure pr:= exit;
  end;

begin
  var v1:= new c1<integer, c0>;
  v1._element:= new c0;
  
  foreach var item in v1 do
    item.pr;
  
  Assert(check);
  check:= false;
  
  var v2:= new c2;
  v2._element:= 'myStr';
  foreach var item in v2 do
    item.Replace('my', 'your');
  
  Assert(check);
  check:= false;
  
  var v3:= new c3;
  v3._element:= 'myStr';
  foreach var item in v3 do 
    item.GetHashCode;
  
  Assert(check);
  check:= false;
  
  var v4:= new c4;
  v4._element:= 'myStr';
  foreach var item in v4 do
    item.Replace('my', 'your');
  
  Assert(check);
  check:= false;
  
  var v5:= new c5;
  v5._element:= 4;
  foreach var item in v5 do
    Assert(item = 4);
  
  Assert(check);
  check:= false;
  
  var v6:= new c6<integer, Dictionary<integer, c0>>;
  v6._element:= new List<Tuple<Dictionary<Dictionary<integer, c0>, integer>, Dictionary<Dictionary<integer, c0>, Dictionary<integer, c0>>, integer>>;
  foreach var item in v6 do
    item.FirstOrDefault;
  
  Assert(check);
end.