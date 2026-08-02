type
  Day = (Mon, Tue, Wed, Thu, Fri, Sat, Sun);

begin
  var days := Arr(Mon, Wed, Fri);
  foreach var day in days do
    Println(day);
end.
