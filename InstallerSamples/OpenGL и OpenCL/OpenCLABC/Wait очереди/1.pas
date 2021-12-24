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
  var B := NamedQ('B').ThenMarkerSignal;
  var C := NamedQ('C').ThenMarkerSignal;
  var D := NamedQ('D');
  var E := NamedQ('E');
  var F := NamedQ('F');
  
  // Можно попробовать расписать как "A + B*C + D*E*F", но это будет не то же самое:
  // К примеру D будет ожидать окончание выполнения C перед тем как начать выполняться
  // (когда все очереди выполняются за ровно одну секунду - это не заметно, но в реальной ситуации будет существенно)
  Context.Default.SyncInvoke(
    A +
    (B+D) *
    (C+F) *
    (WaitFor(B and C) + E)
  );
  
end.