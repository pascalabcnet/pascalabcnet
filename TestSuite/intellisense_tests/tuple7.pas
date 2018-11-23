begin
  var t := (1,1.2);
  var s{@var s: string;@} := t[0].ToString;
  t[1].ToString{@function real.ToString(): string; virtual;@};
end.