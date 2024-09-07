using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DBAccessPluginNamespace
{
    public enum UserTypeEnum {None, Student, Teacher, Admin };
    
    //  Информация о пользователе для доступа к сайту, а также для работы с сайтом
    //  Это можно сохранять между запусками программы
    public class SiteAccessProvider
    {
        //  Данные о пользователе
        public string ShortFIO { get; set; } = "";
        public string FullFIO { get; set; } = "";
        public string Password { get; set; } = "";
        public string Group { get; set; } = "";
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.None;

        //  Базовый адрес папки со скриптами (адрес сервера)
        public string ServAddr = "";

        //  Клиент для обработки запросов http
        private HttpClient client = null;

        /// <summary>
        /// Запрос на добавление информации об активности в БДИм
        /// </summary>
        /// <param name="TaskName">Имя задания</param>
        /// <param name="LessonName">Имя урока</param>
        /// <param name="TypeString">Тип записи - например, выполнено или нет задание</param>
        /// <param name="ContentInfo">Дополнительные данные - например, текст программы</param>
        /// <returns></returns>
        async public Task<string> SendPostRequest(string TaskName, string LessonName, string TypeString, string ContentInfo)
        {
            //  Если не выполнен вход, то выход с ошибкой
            if (UserType == UserTypeEnum.None) return "Error";

            //  Если не выполнен вход, то выход с ошибкой
            if (UserType != UserTypeEnum.Student) return "Cannot write activity, because you're teacher!";

            //  Если клиент ещё не создан был, пересоздаём
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
            }
            //  Словарик для параметров запроса
            var values = new Dictionary<string, string>
                {
                    //`id`, `pupilId`, `taskName`, `lessonName`, `type`, `content`, `time`, `IP`
                    { "shortFIO", ShortFIO },  //  shortFIO или FIO, и они должны совпадать с такими же в базе
                    {"FIO", FullFIO },
                    { "taskName", TaskName },
                    { "lessonName", LessonName },
                    { "type", TypeString },
                    { "content", ContentInfo },
                    { "password", Password }
                };
            //  Отправка запроса на сервер
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "//add.php", content);
            return await response.Content.ReadAsStringAsync();
        }

        public void Clear()
        {
            ShortFIO  = "";
            FullFIO = "";
            Password = "";
            Group = "";
            UserType = UserTypeEnum.None;
        }

        /// <summary>
        /// Получение начальной информации (начальная страница)
        /// </summary>
        /// <returns></returns>
        async public Task<string> GetContents()
        {
            //  Если клиент ещё не создан был, пересоздаём
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
            }
            //  Тут не столь важно что отправлять, можно и пустой список параметров
            var values = new Dictionary<string, string>
                {
                    { "password", "123" },
                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "/index.php", content);
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// Получить из БД список групп
        /// </summary>
        /// <returns></returns>
        async public Task<string> GetGroupsNames(string ServerAddr = "")
        {
            //  Этот метод может быть вызван без предварительного входа
            if (ServerAddr != "")
                ServAddr = ServerAddr;
            if (ServAddr == "") return "Error";
            
            //  Если клиент ещё не создан был, пересоздаём
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
            }
            var values = new Dictionary<string, string>();
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "/groupslist.php", content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Получить из БД список учеников определенной группы
        /// </summary>
        /// <returns></returns>
        async public Task<string> GetUsersNames(string GroupName, string ServerAddr = "")
        {
            //  Этот метод может быть вызван без предварительного входа
            if (ServerAddr != "" || GroupName == "")
                ServAddr = ServerAddr;
            if (ServAddr == "") return "Error";
            //  Если клиент ещё не создан был, пересоздаём
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
            }
            Group = GroupName;
            var values = new Dictionary<string, string>
                {
                    //`id`, `pupilId`, `taskName`, `lessonName`, `type`, `content`, `time`, `IP`
                    { "userGroup", Group }  //  shortFIO или FIO, и они должны совпадать с такими же в базе
                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "/userslist.php", content);
            return await response.Content.ReadAsStringAsync();
        }

        async public Task<string> Login(string shortFIO, string fullFIO, string password, string ServerAddr = "")
        {
            //  Если передана строка адреса - то и хорошо, а если нет, то используем предыдущее значение
            Clear();
            if (ServerAddr != "")
                ServAddr = ServerAddr;
            if (ServAddr == "" && ServerAddr == "") return "Error";

            if (shortFIO == "" && fullFIO == "") return "Error";
            ShortFIO = shortFIO;
            FullFIO = fullFIO;
            Password = password;
            if (client == null)
            {
                client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
            }

            //  Словарик для параметров запроса
            var values = new Dictionary<string, string>
                {
                    //`id`, `pupilId`, `taskName`, `lessonName`, `type`, `content`, `time`, `IP`
                    { "shortFIO", ShortFIO },  //  shortFIO или FIO, и они должны совпадать с такими же в базе
                    { "FIO", FullFIO },
                    { "password", Password}
                };
            //  Отправка запроса на сервер
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "/login.php", content);
            string responseText = await response.Content.ReadAsStringAsync();

            //  Разобрать ответ от сервера - как пары key:value;
            //  Это сложно, т.к. тут могут быть сообщения об ошибках


            //return responseText;
            var pieces = responseText.Split(';');

            //  Сбрасываем тип пользователя, по нему и будем проверять успешность ответа
            //  В сообщениях об ошибках его не должно быть

            foreach(var s in pieces)
            {
                if (s.IndexOf(':') < 1) continue;
                var pair = s.Split(':');
                switch(pair[0])
                {
                    case "FIO" :  FullFIO = pair[1]; break;
                    case "ShortFIO": ShortFIO = pair[1]; break;
                    case "type":
                        {
                            if (pair[1] == "prepod") UserType = UserTypeEnum.Teacher;
                            else if (pair[1] == "student") UserType = UserTypeEnum.Student;
                            break;
                        }
                    case "group":
                        {
                            Group = pair[1];
                            break;
                        }
                }
            }

            if(UserType == UserTypeEnum.None)
            {
                Clear();
                return responseText;
            }
            return "Success";
        }

        async public Task<string> GetRating(string shortFIO, string fullFIO, string password)
        {
            var values = new Dictionary<string, string>
                {
                    //`id`, `pupilId`, `taskName`, `lessonName`, `type`, `content`, `time`, `IP`
                    //{ "shortFIO", ShortFIO },  //  shortFIO или FIO, и они должны совпадать с такими же в базе
                    { "FIO", FullFIO },
                    //{ "password", Password}
                };
            //  Отправка запроса на сервер
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(ServAddr + "/getRating.php", content);
            string responseText = await response.Content.ReadAsStringAsync();
            var pieces = responseText.Split(';');

            var res = "";
            var ClassTasksSolved = 0;
            var ControlTasksSolved = 0;
            var HomeworkTasksSolved = 0;
            var AdditionalTasksSolved = 0;
            var totalRating = 0;
            try 
            { 
                foreach (var s in pieces)
                {
                    if (s.IndexOf(':') < 1) continue;
                    var pair = s.Split(':');
                    switch (pair[0])
                    {
                        /*case "ClassTasks":
                            ClassTasksSolved = int.Parse(pair[2]); 
                            break;
                        case "ControlTasks":
                            ControlTasksSolved = int.Parse(pair[2]);
                            break;
                        case "HomeworkTasks":
                            HomeworkTasksSolved = int.Parse(pair[2]);
                            break;
                        case "AdditionalTasks":
                            AdditionalTasksSolved = int.Parse(pair[2]);
                            break;*/
                        case "Total":
                            totalRating = int.Parse(pair[1]);
                            break;
                    }
                }
                // великая формула !!!
                //var Rating = ClassTasksSolved * 1 + ControlTasksSolved * 5 + HomeworkTasksSolved * 3 + AdditionalTasksSolved * 2;
                var Rating = totalRating;
                //var Details = $" (Осн: {ClassTasksSolved}×1, Доп: {AdditionalTasksSolved}×2, ДЗ: {HomeworkTasksSolved}×3, КР: {ControlTasksSolved}×5)";
                res = Rating.ToString(); // + '|' + Details;
            }
            catch (Exception e)
            {
                res = e.Message;
            }

            return res;
        }

        //  Так как мы на сервере пока ничего не запоминаем, то процесс выхода - просто стереть всё
        public void Logout()
        {
            //  Данные о пользователе
            Clear();

            if (client != null)
                client.Dispose();
            client = null;
        }
    }
}
