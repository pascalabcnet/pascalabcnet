type
  T2 = class
    i: integer;
  end;
  
  T = class
    function Get(i: byte) := default(byte);
    
    property X[i: T2]: T2{@class T2@} read Get;
    property X2: T2{@class T2@} read Get;
    property X3[i{@parameter i: T2;@}: T2]: T2 read Get;
    property X4[i, j{@parameter j: integer;@}: integer; 
                z{@parameter z: T2;@}: T2]: T2 read Get;
    property X5{@property T.X5[List<integer>]: T2; readonly;@}[l: List<integer>]: T2 read Get;
  end;
  
begin
  var o:= new T;
end.