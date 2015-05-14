type 
     TRec = record
              a : array[1..4] of integer;
            end;
            
     TBaseClass = class
      a : array[1..6] of real;
      b : set of char;
      c : TRec;
      
      constructor(i : integer);
      begin
        a[2] := 2.4; assert(a[2] = 2.4);
        Include(b,'f'); assert(b=['f']);
        c.a[3] := 34; assert(c.a[3] = 34);
      end;
     end;
     
     TDerClass = class(TBaseClass)
     constructor;
     begin
      a[2] := 2.4; assert(a[2] = 2.4);
      Include(b,'f'); assert(b=['f']);
      c.a[3] := 34; assert(c.a[3] = 34);
     end;
     end;
 
var b : TBaseClass;
    b2 : TDerClass;
    
begin
  b := new TBaseClass(3);
  b2 := new TDerClass();
end.