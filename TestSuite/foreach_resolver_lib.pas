library foreach_resolver_lib;
  uses System.Collections, System.Collections.Generic;
  
  var check:= false;
  
  type
    c1<T1, T2> = class (IEnumerable<T1>, IEnumerable<T2>)
    public
      _element: T2;
    
      function GetEnumerator: IEnumerator<T2>;
      begin 
        check:= true;
        result:= lst( _element ).GetEnumerator;
      end;
      
      function IEnumerable<T1>.GetEnumerator: IEnumerator<T1>;
      begin end;
      
      function IEnumerable.GetEnumerator: IEnumerator;
      begin end;
    end;
    
    c2 = class (IEnumerable<integer>, IEnumerable<string>)
    public
      _element: string;
    
      function GetEnumerator: IEnumerator<string>;
      begin
        check:= true;
        result:= lst(_element).GetEnumerator;
      end;
      
      function IEnumerable<integer>.GetEnumerator: IEnumerator<integer>;
      begin end;
      
      function IEnumerable.GetEnumerator: IEnumerator;
      begin end;
    end;

    c3 = class (IEnumerable<string>)
    public
      _element: object;
    
      function IEnumerable<string>.GetEnumerator: IEnumerator<string>;
      begin end;
      
      function GetEnumerator: IEnumerator;
      begin
        check:= true;
        result:= |_element|.GetEnumerator;
      end;
    end;
    
    c4 = class (c2) end;
    
    c5 = class (c1<string, integer>) end;
end.