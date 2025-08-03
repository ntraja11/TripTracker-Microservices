using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripTracker.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(Object message, string queueName, string connectionString);
    }
}
