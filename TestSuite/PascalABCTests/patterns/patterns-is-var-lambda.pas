begin
  var f: object->string;
  f := o->
  begin
    if o is string(var s) then
      Result := s else
      Result := o=nil?'nil':o.ToString;
  end;
  
  assert(f('str').Equals('str'));
  assert(f(1).Equals('1'));
  assert(f(new object()).Equals('System.Object'));
end.