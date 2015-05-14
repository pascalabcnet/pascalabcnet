unit u_setlength1;
type TRec = record
s : string;
end;

var arr : array of TRec;

begin
SetLength(arr,Length(arr)+1);
assert(Length(arr)=1);
assert(arr[0].s='');  
end.