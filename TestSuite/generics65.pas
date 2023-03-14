type
  // Не важно запись или класс, но обязательно пользовательский тип
  t1 = class end;
  
procedure p1<TList>(l: TList); where TList: IList<t1>;
begin
  assert(l.Count = 0);
  assert(not l.Contains(new t1));
end;

begin
  p1(new t1[0]);
end.