type
  t1=class
    constructor(i: byte) := exit;
  end;

begin
  new t1{@constructor t1(i: byte);@}(true?0:1);
end.