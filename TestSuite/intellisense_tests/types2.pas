type
  // set of
  T1{@type T1 = set of procedure@} = set of procedure; // set of
  T2{@type T2 = set of function: byte@} = set of function: byte; // set of
  
  // sequence of
  T7{@type T7 = sequence of procedure@} = sequence of procedure;
  T8{@type T8 = sequence of function: byte@} = sequence of function: byte;
  
  // array of
  T9{@type T9 = array of procedure@} = array of procedure;
  T10{@type T10 = array of function: byte@} = array of function: byte;
  T3{@type T3 = file of integer@} = file of integer;
  T4{@type T4 = array[1..5] of procedure@} = array[1..5] of procedure;
  T5{@class T5@} = class
  end;
  T6{@record T6@} = record
  end;
  T11{@type T11 = List<integer>@} = List<integer>;
  T12{@type T12 = T5@} = T5;
  T13{@type T13 = 1..10@} = 1..10;
  T14{@type T14 = sequence of Tuple<procedure,procedure>@} = sequence of (procedure, procedure);
  
begin
end.