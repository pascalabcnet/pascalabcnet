Uses unit_example;

const N: integer = 10;

begin
    println('Последовательность чисел Фибоначчи:');
    for var i := 1 to N do
        println('F(' + i + ')=', fibonacci(i));
    println();
    
    println('Последовательность факториалов:');
    for var i := 1 to N do
        println(i + '!=', factorial(i));
    println();
end.
