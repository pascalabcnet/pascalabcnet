const
  s1 = 'abc';
  s2 = 'def';
  s3 = s1+s2;
  s4 = s1+'d';
  s5 = s1+6;
  i = 6;
  s6 = s1 + i;
  s7 = s1 + s2 + 'gh';
  
begin
  writeln(s3{@const s3: string = 'abcdef';@});
  writeln(s4{@const s4: string = 'abcd';@});
  writeln(s5{@const s5: string = 'abc6';@});
  writeln(s6{@const s6: string = 'abc6';@});
  writeln(s7{@const s7: string = 'abcdefgh';@});
end.