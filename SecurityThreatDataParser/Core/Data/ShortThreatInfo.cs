using System;

namespace SecurityThreatDataParser.Core.Data
{
    public class ShortThreatInfo : ICloneable
    {
        private String _id;

        public String Id
        {
            get => $"УБИ.{_id}";
            set => _id = value;
        }

        public String Name { get; set; }


        public Object Clone() =>
            new ShortThreatInfo()
            {
                Id = Id?.Split('.')[1],
                Name = Name
            };
    }
}