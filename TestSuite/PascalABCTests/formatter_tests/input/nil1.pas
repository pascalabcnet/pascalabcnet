
type TRec = record
a : object;
arr : array of object;
end;

const c : object = nil;
      arr : array[1..3] of object = (nil,nil,nil);
      arr2 : array of object = (nil,nil);
      arr3 : array[,] of object = ((nil,nil),(nil,nil));
      rec : TRec=(a:nil;arr:(nil,nil));
      
var v : object := nil;
    arr4 : array[1..3] of object := (nil,nil,nil);
    arr5 : array of object := (nil,nil);
    arr6 : array[,] of object := ((nil,nil),(nil,nil));    
    rec2 : TRec:=(a:nil;arr:(nil,nil));
    
begin

end.