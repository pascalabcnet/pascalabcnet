  	 type TRec = record
              a : integer;
              b : real;
            end;
            
type
  Node = class<T>
  a : T;
  procedure Test;
  begin
    a := nil;//?????????
  end;
  end;

var p : Node<TRec> := nil;
  
begin
p := new Node<TRec>();
p.a.a := 23;
p.Test;
end.