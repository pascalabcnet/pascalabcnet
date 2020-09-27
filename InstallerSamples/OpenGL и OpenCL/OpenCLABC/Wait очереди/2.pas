uses OpenCLABC;
uses NamedQData;

begin
  // График выполнения очередей:
  //
  // A1--B1--C1------E1
  //       \        /
  //        \      /
  // A2------C2--D2--E2
  //
  
  var A1 := NamedQ('A1');
  var A2 := NamedQ('A2');
  
  var B1 := NamedQ('B1');
  
  var C1 := NamedQ('C1');
  var C2 := NamedQ('C2');
  
  var D2 := NamedQ('D2');
  
  var E1 := NamedQ('E1');
  var E2 := NamedQ('E2');
  
  Context.Default.SyncInvoke(
    ( A1 + B1          + C1 + WaitFor(D2) + E1 ) *
    ( A2 + WaitFor(B1) + C2 + D2          + E2 )
  );
  
end.