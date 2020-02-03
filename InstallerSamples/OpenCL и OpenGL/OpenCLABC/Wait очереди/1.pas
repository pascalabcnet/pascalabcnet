uses OpenCLABC;
uses NamedQData;

begin
  // График выполнения очередей:
  //
  //     D
  //    /
  //   B
  //  / \
  // A   E
  //  \ /
  //   C
  //    \
  //     F
  //
  
  var A := NamedQ('A');
  var B := NamedQ('B');
  var C := NamedQ('C');
  var D := NamedQ('D');
  var E := NamedQ('E');
  var F := NamedQ('F');
  
  Context.Default.SyncInvoke(
    (A + (B+D)*(C+F) ) *
    (WaitForAll(B,C) + E)
  );
  
end.