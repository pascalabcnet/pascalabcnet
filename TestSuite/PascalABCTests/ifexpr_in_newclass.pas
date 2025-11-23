##
var a := new class(x := true?1:2); //Ошибка: Встречено '?', а ожидалось '='
var b := new class(x := if true then 1 else 2); //Ошибка: Встречено 'if', а ожидалось выражение
Assert(a.x = 1);
Assert(b.x = 1);