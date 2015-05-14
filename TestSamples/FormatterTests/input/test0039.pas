type TRec = record 
             a : array[1..3] of real;
             b : set of byte;
            end;
            
var arr : array of TRec;
    arr2 : array of set of byte;
    parr : ^array[1..4] of set of byte;
    pset : ^set of byte;
    
begin
 SetLength(arr,3);
 arr[1].a[2] := 10;
 Include(arr[1].b,43); 
 SetLength(arr,7);
 assert(arr[1].a[2] = 10);
 arr[6].a[2] := 13;
 assert(arr[6].a[2] = 13);
 assert(arr[1].b = [43]);
 SetLength(arr2,5);
 Include(arr2[1],15);
 assert(arr2[1]=[15]);
 SetLength(arr2,7);
 assert(arr2[1]=[15]);
 Include(arr2[1],345);
 assert(arr2[1]=[15]);
 
 New(pset);
 Include(pset^,50); assert(pset^ = [50]);
 Include(pset^,400); assert(pset^ = [50]);
 pset^ := [1,40,400]; assert(pset^ = [1,40]);
 
 New(parr);
 Include(parr^[1],50); assert(parr^[1] = [50]);
 Include(parr^[1],400); assert(parr^[1] = [50]);
 parr^[1] := [20,30,300]; assert(parr^[1] =[20,30]);
end.