// 3.11.1. Целочисленный квадратный корень
begin
  Println(ISqrt(15),ISqrt(16));
  var bi: BigInteger := 6125476524;
  bi := bi*bi;
  Print(ISqrt(bi),ISqrt(bi-1));
end.