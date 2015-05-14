const cp : ^integer = nil;
      cptr : pointer = nil;
      
var p : ^integer := nil;
    ptr : pointer := nil;
    
begin
  assert(cp=nil);
  assert(cptr=nil);
  assert(p=nil);
  assert(ptr=nil);
  assert(sizeof(pointer)=sizeof(integer));
  assert(sizeof(pinteger)=sizeof(integer));
end.