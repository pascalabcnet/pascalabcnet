begin
  var name := 'Иван';
  var age := 18;
  var text := $'''
    Меня зовут {name}.
    Мне {age} лет.
    ''';
  var nl := System.Environment.NewLine;
  assert(text = 'Меня зовут Иван.' + nl + 'Мне 18 лет.');

  var formatted := $'''
{{{age}}}: {age:000}

  {'quoted'}: {age + 1,4}
''';
  assert(formatted = '{18}: 018' + nl + nl + '  quoted:   19');

  var plain := '''
    {name}
    ''';
  assert(plain = '{name}');
  var interpolatedWithoutHoles := $'''
    a
    ''';
  assert(interpolatedWithoutHoles.GetType = typeof(string));
end.
