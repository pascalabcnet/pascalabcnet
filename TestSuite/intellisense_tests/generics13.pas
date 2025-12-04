type
  TProcedure = procedure;
  
begin
  var pa1: procedure; // procedure
  var pa2{@var pa2: (procedure,procedure);@}: (procedure, procedure);
  var pa3{@var pa3: (byte,procedure);@}: (byte, procedure);
  
  var pb1: TProcedure; // TProcedure
  var pb2{@var pb2: (TProcedure,TProcedure);@}: (TProcedure, TProcedure);
  var pb3{@var pb3: (byte,TProcedure);@}: (byte, TProcedure);
end.