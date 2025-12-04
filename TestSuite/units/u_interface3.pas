unit u_interface3;
type TClass<T> = class(IEnumerable<T>)
public function System.Collections.Generic.IEnumerable<T>.GetEnumerator: IEnumerator<T>;
begin
  
end;

public function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator;
begin
  
end;
end;
begin
  
end.