using System.Collections.Generic;
using Manect.Interfaces;
using Manect.Services.StringFormats;

namespace Manect.Services
{
    public class StringFormatService : IStringFormatService
    {
        private readonly List<StringFormat> _stringFormats;

        public StringFormatService()
        {
            _stringFormats = new List<StringFormat>
            {
                new StageStringFormat(),
                new FileStringFormat(),
                new ProjectStringFormat()
            };
        }

        public List<StringFormat> GetStringFormats() => _stringFormats;
    }
}