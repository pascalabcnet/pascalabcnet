function fff <T1>(a: System.Tuple<T1,T1>): boolean; where T1:IComparable<T1>; 
begin
  Result := a.Item1.CompareTo(a.Item2)<0;
end;

begin
  var date1 := (23,2);
  assert(fff(date1) = false);
end.