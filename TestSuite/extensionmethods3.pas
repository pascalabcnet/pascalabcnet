{$reference 'System.Core.dll'}

uses System,System.IO, System.Collections.Generic;

function Len(s: string): integer;
begin
  Result := s.Length
end;

function Cond(s: string): boolean;
begin
  Result := s.Contains('ab');
end;

function FindFirst(s: string): boolean;
begin
  if s.StartsWith('12') then
    Result := true;
end;

function MoreOne(i: integer): boolean;
begin
  Result := i > 1;
end;

type TRec = record
  a: integer;
end;

var lines : array of string := ('abcd','ab','123456','12345678');
    
begin
  var avgLength :=lines
  .Take(4)
  .Select&<string, integer>(Len)
  .Average();
  lines.Take(4);
  assert(avgLength = 5);
  var arr : array of integer := (1,2,3);
  assert(arr.Sum() = 6);
  var lst: List<integer> := new List<integer>();
  lst.Add(2); lst.Add(4);
  assert(lst.Sum() = 6);
  var arr2: array of object;
  arr2 := lst.Cast&<object>().ToArray&<object>();
  assert(integer(arr2[0])=2);assert(integer(arr2[1])=4);
  assert(lines.Count(Cond) = 2);
  assert(lines.First(FindFirst)='123456'); 
  var lst2: List<TRec> := new List<TRec>;
  
end.