type
  t1=class
    o: object;
    
    property p1: System.Type read o?.GetType;
  end;

begin 
  var t := new t1;
  Assert(t.p1=nil);
end.