Кодовое имя проекта: CodeCompletion
Название: CodeCompletion                     
Автор:  Бондарев Иван                        
EMail, ICQ автора: ibond84@freemail.ru		
Логин на SVN: ibond			
Краткое описание проекта: Движок интеллисенса	
Комментарии:
CodeCompletion.cs содержит класс CodeCompletionController - контроллер для парсинга файлов
DomConverter.cs содержит класс DomConverter, использующийся для получения необходимой интеллисенсу информации
DomSyntaxTreeVisitor.cs содержит класс DomSyntaxTreeVisitor, строящий по синтаксическому дереву внутреннее представление 
CodeCompletionPCUReader содержит класс IntellisensePCUReader, строящий по PCU-файлу внутреннее представление
ExpressionVisitor.cs содержит класс ExpressionVisitor, который позволяет получить по выражению его scope (например для выражения до точки)
SymTable.cs содержит классы для внутреннего представления
ExpressionEvaluator.cs содержит класс  ExpressionEvaluator, предназначенный для вычисления константных выражений 
FindReferences.cs содержит класс ReferenceFinder, предназначенный для поиска использований символов
XmlDocs.cs содержит классы, ответственные за получение документации к сущностям