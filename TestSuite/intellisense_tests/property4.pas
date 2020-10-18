type
  t1=class
    static function getitem(i: string): integer := nil;
    static property item[i: string]: integer read getitem; default;
  end;
  
  t2=class
    function getitem(i: string): integer := nil;
    property item[i: string]: integer read getitem; default;
  end;
  
begin
  var a := new t1;
  var o1 :=  a{[]}[''];
  var o2 := t1{[t1[string] : integer]}[''];
  var b := new t2;
  o1 := b{[this[string] : integer]};
  o1 := t2{[]};
end.