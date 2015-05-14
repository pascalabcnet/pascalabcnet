unit u_dynarrayofset1;
type TSet = set of byte;

var arr : array of TSet;

begin
 SetLength(arr,5);
 Include(arr[1],3);
 Include(arr[1],4);
 assert(arr[1]=[3,4]);
 SetLength(arr,7);
 assert(arr[1]=[3,4]);
 arr := new TSet[4];
 Include(arr[1],3);
 Include(arr[1],4);
 assert(arr[1]=[3,4]);
end.