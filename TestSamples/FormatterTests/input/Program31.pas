type TRec = record
            a, b : array[1..4] of integer;
            end;
 var arr : array of TRec;
 
begin
//SetLength(arr,10);
arr:=new trec[10];
write(arr[0].a[1]);
writeln('xxx');
end.