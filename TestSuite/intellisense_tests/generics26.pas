begin
  var x0{@var x0: array[,] of string;@} := Matr&<string>(1, 1, 'abc');
 
  var x1{@var x1: sequence of string;@} := x0.ColSeq(0); // sequence of integer
  var x2{@var x2: sequence of string;@} := x0.RowSeq(0); // sequence of integer
  
  var x3{@var x3: array of array of string;@} := x0.Cols(); // sequence of IEnumerable<char>
  var x4{@var x4: array of array of string;@} := x0.Rows(); // sequence of IEnumerable<char>
  
  var x5{@var x5: sequence of string;@} := x0.ElementsByCol(); // sequence of string [правильно]
  var x6{@var x6: sequence of string;@} := x0.ElementsByRow(); // sequence of string [правильно]
  
  var x8{@var x8: sequence of byte;@} := x0.OfType&<byte>();
  
  var x9{@var x9: array[,] of string;@} := x0.Println();
  
end.