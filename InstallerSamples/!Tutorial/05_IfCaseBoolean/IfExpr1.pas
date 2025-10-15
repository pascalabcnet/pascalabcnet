begin
  var t := ReadInteger; // температура
  
  var color := 
    if t.Between(-100, 0) then 'blue' else
    if t.Between(1, 15) then 'green' else
    if t.Between(16, 30) then 'yellow' else
    if t.Between(31, 100) then 'red' else
      'black';
  
  Println($'Temperature color: {color}');
end.