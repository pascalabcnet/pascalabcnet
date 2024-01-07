type
  // Обязательно запись
  r1 = record(IEnumerable<Exception>)
    res := new Exception[](new System.InvalidOperationException('OK'));
    
    function GetEnumerator: IEnumerator<Exception> := res.AsEnumerable.GetEnumerator;
    function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator := self.GetEnumerator;
    
  end;
  
begin
  var ex: Exception;
  foreach var e in new r1 do
  begin
    ex := e;
  end;
  assert(ex.Message = 'OK');
end.