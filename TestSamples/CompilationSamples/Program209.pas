 	 type TRec = record
 	 a : integer;
 	 end;
 	 var arr : array[1..3] of TRec;
 	 const i = 5;
 	 
 	 begin
 	 arr[2+i*2-abs(3)-6*(i-4)].a := 23;
with typeof(integer) do
begin
writeln(Assembly);
end;
readln;
end. 