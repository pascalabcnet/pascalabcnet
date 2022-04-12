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
  
  var B1 := NamedQ('B1').ThenMarkerSignal;
  var Bw := WaitFor(B1);
  
  var C1 := NamedQ('C1');
  var C2 := NamedQ('C2');
  
  var D2 := NamedQ('D2').ThenMarkerSignal;
  var Dw := WaitFor(D2);
  
  var E1 := NamedQ('E1');
  var E2 := NamedQ('E2');
  
  Context.Default.SyncInvoke(
    ( A1 + B1 + C1 + Dw + E1 ) *
    ( A2 + Bw + C2 + D2 + E2 )
  );
  Writeln('='*30);
  // Дерево выполнения из данной программы может решать задачу такого типа:
  // > Два парралельных процесса выполняются последовательно и иногда взаимодействуют
  // Его можно расписать без Wait, к примеру:
  var B1s := (A1+B1).Multiusable;
  var D2s := ((B1s()*A2)+C2+D2).Multiusable;
  Context.Default.SyncInvoke(
    ( (B1s()+C1) * D2s() + E1 ) *
    ( D2s() + E2 )
  );
  // И результат будет тот же, но такое дерево уже не похоже по форме на изначальную задачу
  // Если между деревом и задачей сложно провести параллели - программу будет сложно улучшать
  // (то есть будет сложно понять что делает что в очереди)
  
end.