using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // Все клеенты которые подключаются к хосту, будут работать с сервисом (один хост - много подключений)
    public class ServiceChat : IServiceChat
    {
        // Реализация сервиса
        // Когда клиент обращается к нашему сервису, то сервер (хост) для каждого подключения создает свой экземпляр сервиса
        // Чат - сервис один (сервис должен снать о всех клиентах) и посылать всем сообщение одновременно

        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)         // Подключение к сервису (Обязательно должны дождаться ответа сервера)
        {
            ServerUser user = new ServerUser()  // Создание пользователя
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMSG(": "+user.Name + " подключился(ась) к чату!",0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)      // Отключение от сервиса
        {
            var user = users.FirstOrDefault(i => i.ID == id);

            if (user != null)
            {
                users.Remove(user);
                SendMSG(": " + user.Name + " покинул(а) чат!",0);
            }
        }
        // Работа wcf с клиентом
        // Например мы вызываем SendMSG у сервиса, то клиент посылает нашему сервису сообщение и ждет ответа от сервиса о получении ответа
        // Если метод не возвращает никагого значения, то наш клиент будет заблокирован до ответа сервера
        public void SendMSG(string msg, int id) // Клиент посылает ответ
        {
            // Перебор всех пользователей
            // Формирование ответа всем пользователям
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);

                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }
                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);    // Получение ответа от пользователя
            }
        }
    }
}
