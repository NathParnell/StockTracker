using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IMessageService
    {
        List<string> GetContactIds(string userId);
        List<Message> GetMessageThreads(string userId, string contactId);
        bool SendMessage(string receiverId, string messageBody);
        bool SendMessage(string receiverId, string messageBody, string messageSubject);
        bool SendMessage(List<string> receiverIds, string messageBody);
    }
}
