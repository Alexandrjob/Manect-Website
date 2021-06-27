using Manect.Interfaces;
using Manect.Services.StringFormats;
using System.Collections.Generic;

namespace Manect.Services
{
    public class StringFormatService: IStringFormatService
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
