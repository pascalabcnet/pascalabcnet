Uses System,System.Net,System.Threading.Tasks;

begin
  Parallel.Invoke (
    procedure -> begin (new WebClient()).DownloadFile ('http://yandex.ru', 'yandex.html') end,
    procedure -> begin (new WebClient()).DownloadFile ('http://pascalabc.net', 'pabc.html') end
  );
end.