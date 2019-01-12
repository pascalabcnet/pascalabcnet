procedure p1(s1, s2, s3: string) := writeln('перегрузка 1');

procedure p1(i: integer; b: byte := 1) := writeln('перегрузка 2');

procedure p1(s: string) := writeln('перегрузка 3');

procedure p1(params a: array of string) := writeln('перегрузка 4');

procedure p1(b1,b2: boolean; params a: array of integer) := writeln('перегрузка 5');

procedure p1(b1: boolean; params a: array of integer) := writeln('перегрузка 6');

procedure p1(b1: boolean; params a: array of real) := writeln('перегрузка 7');

begin
  p1{@procedure p1(i: integer; b: byte:=1);@}(1);
  p1{@procedure p1(s: string);@}('aa');
  p1{@procedure p1(s1: string; s2: string; s3: string);@}('aa','bb','cc');
  p1{@procedure p1(params a: array of string);@}('aa','bb','cc','dd');
  p1{@procedure p1(b1: boolean; b2: boolean; params a: array of integer);@}(true, false);
  p1{@procedure p1(b1: boolean; b2: boolean; params a: array of integer);@}(true, false, 2, 3, 4);
  p1{@procedure p1(b1: boolean; params a: array of integer);@}(true);
  p1{@procedure p1(b1: boolean; params a: array of integer);@}(true, 2, 4);
  p1{@procedure p1(b1: boolean; params a: array of real);@}(true, 2.3, 4.2);
  System.Console.WriteLine{@static procedure Console.WriteLine(value: char);@}('2');
  System.Console.WriteLine{@static procedure Console.WriteLine(value: integer);@}(2);
  System.Console.WriteLine{@static procedure Console.WriteLine(value: real);@}(2.3);
  System.Console.WriteLine{@static procedure Console.WriteLine(value: boolean);@}(true);
  System.Console.WriteLine{@static procedure Console.WriteLine(format: string; arg0: Object; arg1: Object);@}('', nil, nil);
  System.Console.WriteLine{@static procedure Console.WriteLine(format: string; params arg: array of Object);@}('', 2, 2, 2, 2, 2);
end.