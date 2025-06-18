type TSet<T> = record
str: string;
set1: set of char;
set2: set of T;
end;

begin
  var set1: TSet<char>;
  var set2: TSet<char>;
  assert(set1.str = '');
  assert(set1.set1 = []);
  assert(set1.set2 = []);
  set2 := set1;
  Include(set1.set1,'b');
  Include(set1.set2,'b');
  Include(set2.set1,'b');
  Include(set2.set2,'b');
  assert(set1.set1 = ['b']); // увы - при присваивании не вызывается operator := для компонент
  assert(set1.set2 = ['b']);
end.