type TSet = set of char;
     TArr = array[1..4] of TSet;
     TMatr = array[1..3,1..4] of TSet;
     
procedure Test;
var arr : TArr;
    arr2 : TMatr;
    
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
end;

procedure Test2(arr : TArr; arr2 : TMatr);
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
end;

procedure Test3(var arr : TArr; var arr2 : TMatr);
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
end;

procedure Test4;
var arr : TArr;
    arr2 : TMatr;
 
procedure Nested;
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
end;

begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
 Nested;
end;

procedure Test5(var arr : TArr; var arr2 : TMatr);
 
procedure Nested;
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
end;

begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
 Nested;
end;

var arr : TArr;
    arr2 : TMatr;
    
begin
 arr[2] := ['a','d','m'];
 arr[3] := arr[2];
 Include(arr[3],'s');
 assert(arr[3] = ['a','d','m','s']);
 assert(arr[2] = ['a','d','m']);
 Exclude(arr[2],'d'); assert(arr[2]=['a','m']);
 
 arr2[1,1] := ['a','d','m'];
 arr2[2,2] := arr2[1,1];
 Include(arr2[2,2],'s');
 assert(arr2[2,2] = ['a','d','m','s']);
 assert(arr2[1,1] = ['a','d','m']);
 Exclude(arr2[1,1],'d'); assert(arr2[1,1]=['a','m']);
 Test;
 Test2(arr,arr2);
 Test3(arr,arr2);
 Test4;
 Test5(arr,arr2);
end.