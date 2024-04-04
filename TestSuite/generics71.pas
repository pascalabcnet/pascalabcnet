var InputList := new List<object>;

procedure GenerateTests<T>(a: array of T);
begin
  InputList.Add(object(a[0]));
  Print(1);
end;

begin
  GenerateTests(Arr('a','b'));
  Print(InputList);
  assert(char(InputList[0]) = 'a');
end.