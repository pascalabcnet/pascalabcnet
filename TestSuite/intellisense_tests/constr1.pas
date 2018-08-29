type
  t1 = class
    constructor(b: integer) := exit;
  end;

begin
  new t1{@constructor t1();@};
end.