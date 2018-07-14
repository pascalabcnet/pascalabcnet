type
  t1=class
  public
    i1 := 55;
    function f1 := 1;
  end;
  t2=class
  public
    i2 := 66;
    function f2 := 2;
  end;

begin
  
  var d := new SortedDictionary<t1,t2>;
  d.Add(new t1,new t2);
  
  {foreach var a in d do writeln(a.GetType);//KeyValuePair<t1,t2>
  foreach var a in d.Keys do writeln(a.GetType);//t1
  foreach var a in d.Values do writeln(a.GetType);//t2}
  
  foreach var a in d.Values do 
    Assert(a.i2=66);
 
end.