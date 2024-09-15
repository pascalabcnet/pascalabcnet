uses System.Collections, System.Collections.Generic;

type
  c1 = class (IEnumerable<integer>)
  public 
  
    function IEnumerable<integer>.GetEnumerator: IEnumerator<integer>;	
    begin
      Assert(false);
    end;
    
    function GetEnumerator: IEnumerator;
    begin
      result:= Seq&<object>().GetEnumerator;
    end;
  end;
  
  c2<T1, T2> = class (IEnumerable<T2>)
  public 
    function GetEnumerator: IEnumerator<T2>;	
    begin
      result:= Seq&<T2>().GetEnumerator;
    end;
    
    function IEnumerable.GetEnumerator: IEnumerator;
    begin
      Assert(false);
    end;
  end;
  
  c3 = class (IEnumerable<integer>, IEnumerable<string>)
  public
    function GetEnumerator: IEnumerator<string>;
    begin
      result:= Seq&<string>().GetEnumerator;
    end;
    
    function IEnumerable<integer>.GetEnumerator: IEnumerator<integer>;
    begin
      Assert(false);
    end;
    
    function IEnumerable.GetEnumerator: IEnumerator;
    begin
      Assert(false);
    end;
  end;
  
  c4 = class (c2<integer, int64>)
  
  end;
  
begin
  
  var v1:= new c1;
  foreach var item in v1 do ;
  
  var v2:= new c2<byte, string>;
  foreach var item in v2 do ;
  
  var v3:= new c3;
  foreach var item in v3 do ;
  
  var v4:= new c4;
  foreach var item in v4 do ;
  
end.