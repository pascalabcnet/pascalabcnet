uses OpenCLABC;

begin
  var q := HFQ(()->123);
  
  Context.Default.SyncInvoke(
    q.ThenConvert(i -> $'"{i}"' )
  ).Println;
  
end.