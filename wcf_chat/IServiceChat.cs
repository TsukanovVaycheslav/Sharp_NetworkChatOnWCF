using System.ServiceModel;

namespace wcf_chat
{
    // Спомощью интерфейса мы описали, как будет работать наш сервис
    // Интерфейс вызывается на стороне клиента
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IServiceChat
    {
        // Реализация интерфеса
        [OperationContract]
        int Connect(string name);   // Подключение к сервису (Обязательно должны дождаться ответа сервера)

        [OperationContract]
        void Disconnect(int id);    // Отключение от сервиса

        [OperationContract(IsOneWay = true)]
        void SendMSG(string msg, int id);   // Клиент посылает ответ
    }

    public interface IServerChatCallback    // Функция обратного вызова (Получение сообщение от сервера)
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}