##
var r := 2..3;
Assert(
  r.Cartesian(2)
//  .Println
  .ZipTuple(||2,2|,|2,3|,|3,2|,|3,3||)
  .All(\(a,b)->a.SequenceEqual(b))
);