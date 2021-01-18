using Manect.Interfaces;
using Manect.Services.MessageFormats;
using System.Collections.Generic;

namespace Manect.Services
{
    public class MessageFormatService: IMessageFormatService
    {
        private readonly List<MessageFormat> _messageFormats;

        public MessageFormatService()
        {
            _messageFormats = new List<MessageFormat>
            {
                new StageMessageFormat(),
                new FileMessageFormat(),
                new ProjectMessageFormat()
            };
        }

        public List<MessageFormat> GetMessageFormats() => _messageFormats;
    }
}
