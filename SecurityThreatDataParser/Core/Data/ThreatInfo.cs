using System;

namespace SecurityThreatDataParser.Core.Data
{
    public class ThreatInfo : ICloneable
    {
        private SByte _violationOfConfidentiality = -1;
        private SByte _integrityViolation = -1;
        private SByte _violationOfAvailability = -1;
        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String SourceOfThreat { get; set; }
        public String InteractionObject { get; set; }

        public String ViolationOfConfidentiality
        {
            get =>
                _violationOfConfidentiality == 1
                    ? "да"
                    : _violationOfConfidentiality == 0
                        ? "нет"
                        : "";
            set => _violationOfConfidentiality = (SByte) (value == "0" || value == "1" 
                ? Convert.ToSByte(value) 
                : -1);
        }

        public String IntegrityViolation
        {
            get =>
                _integrityViolation == 1
                    ? "да"
                    : _integrityViolation == 0
                        ? "нет"
                        : "";
            set => _integrityViolation = (SByte) (value == "0" || value == "1" 
                ? Convert.ToSByte(value) 
                : -1);
        }

        public String ViolationOfAvailability
        {
            get => _violationOfAvailability == 1 
                ? "да" 
                : _violationOfAvailability == 0 
                    ? "нет" 
                    : "";
            set => _violationOfAvailability = (SByte) (value == "0" || value == "1" 
                ? Convert.ToSByte(value) 
                : -1);
        }

        public override Boolean Equals(Object obj) =>
            obj is ThreatInfo threat && threat.Id.Equals(Id) && threat.Name.Equals(Name) &&
            threat.Description.Equals(Description) && threat.SourceOfThreat.Equals(SourceOfThreat) &&
            threat.InteractionObject.Equals(InteractionObject) &&
            threat._violationOfConfidentiality.Equals(_violationOfConfidentiality) &&
            threat._integrityViolation.Equals(_integrityViolation) &&
            threat._violationOfAvailability.Equals(_violationOfAvailability);

        public Object Clone() =>
            new ThreatInfo()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                SourceOfThreat = SourceOfThreat,
                InteractionObject = InteractionObject,
                _violationOfConfidentiality = _violationOfConfidentiality,
                _integrityViolation = _integrityViolation,
                _violationOfAvailability = _violationOfAvailability
            };

        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hashCode = _violationOfConfidentiality.GetHashCode();
                hashCode = (hashCode * 601) ^ _integrityViolation.GetHashCode();
                hashCode = (hashCode * 601) ^ _violationOfAvailability.GetHashCode();
                hashCode = (hashCode * 601) ^ (Id?.GetHashCode() ?? 0);
                hashCode = (hashCode * 601) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 601) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 601) ^ (SourceOfThreat?.GetHashCode() ?? 0);
                hashCode = (hashCode * 601) ^ (InteractionObject?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}