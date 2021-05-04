using System.ServiceModel;

namespace wcf_chat
{
    class ServerUser
    {
        public int ID { get; set; }                             // ID текущего пользователя
        public string Name { get; set; }                        // Его имя
        public OperationContext operationContext { get; set; }  // Подключение к нашему сервису
    }
}
