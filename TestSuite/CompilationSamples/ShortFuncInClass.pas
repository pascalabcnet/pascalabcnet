type
  arrType = array of System.Type;
  someClass = class
    function ArrTest(arr: arrType) := (arr = nil) or (arr.Length = 0);
  end;
begin
end.