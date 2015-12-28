// Исправьте имя SMTP-сервера SMTPServerName и адрес получателя toReceiver! 
uses System.Net.Mail;

const 
  SMTPServerName = 'mail.spark-mail.ru';

begin
  var fromSender := 'ivanov@mail.ru';
  var toReceiver := 'petrov@yandex.ru';
  var subject := 'Proba';
  var body := 'Hello!' + NewLine + 'I am robot!';
  var message := new MailMessage(fromSender, toReceiver, subject, body);
  
  var mailClient := new SmtpClient(SMTPServerName);

  mailClient.Send(message);
end.