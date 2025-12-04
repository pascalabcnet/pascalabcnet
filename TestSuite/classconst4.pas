
type
  TArrStr = array of string;
  t1 = class
    
    public const arr: array of string = ('abc', 'def');
    
  end;
  
begin
  var arr := TArrStr(typeof(t1).GetField('arr').GetValue(nil));
  assert(arr[0] = 'abc');
  assert(arr[1] = 'def');
end.