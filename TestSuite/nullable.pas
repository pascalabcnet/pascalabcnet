type TRec = record
a: integer;
b: real;
end;

type TClass = class
v1: System.Nullable<integer>;
v2: System.Nullable<TRec>;
constructor;
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

procedure Test(v1 : System.Nullable<integer>; v2: System.Nullable<TRec>);
begin
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;
end;

type TClass2 = class
class v1: System.Nullable<integer>;
class v2: System.Nullable<TRec>;
class procedure Test;
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;
class procedure Test2(v1 : System.Nullable<integer>; v2: System.Nullable<TRec>);
begin
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;
end;

procedure Test3(v1 : System.Nullable<integer>; v2: System.Nullable<TRec>);
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

procedure Test4(var v1 : System.Nullable<integer>; var v2: System.Nullable<TRec>);
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

procedure Test2;
begin
  var v1 : System.Nullable<integer>;
  var v2 : System.Nullable<TRec>;
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

procedure Test;
var 
  v1 : System.Nullable<integer>;
  v2 : System.Nullable<TRec>;
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

procedure Test6;
var 
  v1 : System.Nullable<integer>;
  v2 : System.Nullable<TRec>;

procedure Nested;
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;
begin
  Nested;
end;

procedure Test5(v1 : System.Nullable<integer>; v2: System.Nullable<TRec>);
begin
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
end;

var 
  v1 : System.Nullable<integer>;
  v2 : System.Nullable<TRec>;
  
begin
  v1 := nil;
  assert(v1 = nil);
  v1 := 4;
  assert(v1 <> nil);
  assert(not (v1 = nil));
  v2 := nil;
  assert(v2 = nil);
  v2 := new TRec;
  assert(v2 <> nil);
  Test;
  Test2;
  var obj := new TClass;
  obj.Test(nil, nil);
  TClass2.Test;
  Test3(v1, v2);
  Test4(v1, v2);
  v1 := nil;
  v2 := nil;
  Test5(v1, v2);
  Test6;
  Test5(nil, nil);
  TClass2.Test2(nil, nil);
  var arr: array of System.Nullable<integer>;
  SetLength(arr,3);
  arr[0] := nil;
  assert(arr[0] = nil);
  arr[0] := 2;
  assert(arr[0] <> nil);
  var lst: List<integer?> := new List<integer?>;
  lst.Add(nil);
  assert(lst[0] = nil);
end.