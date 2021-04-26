using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMSG(": "+user.Name + " подключился к чату!",0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);

            if (user != null)
            {
                users.Remove(user);
                SendMSG(": " + user.Name + " покинул чат!",0);
            }
        }

        public void SendMSG(string msg, int id)
        {
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);

                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }
                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
        }
    }
}
