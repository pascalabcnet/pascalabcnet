//!Невозможно выполнить оператор foreach или yield sequence по выражению типа c1

uses System.Collections, System.Collections.Generic;

type
  c1 = class (IEnumerable<integer>, IEnumerable<string>)
  public
    function IEnumerable<string>.GetEnumerator: IEnumerator<string>;
    begin end;
    
    function IEnumerable<integer>.GetEnumerator: IEnumerator<integer>;
    begin end;
    
    function IEnumerable.GetEnumerator: IEnumerator;
    begin end;
  end;
  
begin
  foreach var item in new c1 do ;
end.