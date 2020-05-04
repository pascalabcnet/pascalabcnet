uses OpenCLABC;
uses NamedQData;

begin
  // График выполнения очередей:
  //
  // A1--B1--C1------D1
  //       \        /
  //        \      /
  // A2------B2--C2--D2
  //
  
  var A1 := NamedQ('A1');
  var B1 := NamedQ('B1');
  var C1 := NamedQ('C1');
  var D1 := NamedQ('D1');
  var A2 := NamedQ('A2');
  var B2 := NamedQ('B2');
  var C2 := NamedQ('C2');
  var D2 := NamedQ('D2');
  
  Context.Default.SyncInvoke(
    ( A1 + B1          + C1 + WaitFor(C2) + D1 ) *
    ( A2 + WaitFor(B1) + B2 + C2          + D2 )
  );
  
end.