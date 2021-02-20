begin
  var s := '23 6 72 * +';
  var st := new Stack<integer>;
  foreach var w in s.ToWords do
    case w[1] of
  '0'..'9': st.Push(w.ToInteger);
  '+': st.Push(st.Pop+st.Pop);
  '*': st.Push(st.Pop*st.Pop);
    end;
   writeln(st.Pop); 
end.
