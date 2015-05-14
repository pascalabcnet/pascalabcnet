type TClass = class
public function ToString: string; override;
begin
  inherited ToString();
  Result := 'tt'+inherited ToString;
end;
end;

var obj : TClass;
begin
  obj := new TClass;
  assert(obj.ToString()='ttinherited1.TClass');
end.