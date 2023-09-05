namespace MCS.HomeSite.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuditingAttribute : Attribute
    {
        private bool AuditDbSet { get; set; }
        private string Fields { get; set; }
        private string ParentEntityKeyField { get; set; }
        private bool RecordAdded { get; set; }
        private string RecordFieldOnDelete { get; set; }
        private bool RecordDeleted { get; set; }
        private string AdditionalDataField { get; set; }

        public AuditingAttribute(bool auditDbSet, string parentEntityKeyField = null, string fields = null, 
            bool recordAdded = true, bool recordDeleted = true, string recordFieldOnDelete = null, string additionalDataField = null)
        {
            AuditDbSet = auditDbSet;
            Fields = fields;
            ParentEntityKeyField = parentEntityKeyField;
            RecordAdded = recordAdded;
            RecordFieldOnDelete = recordFieldOnDelete;
            RecordDeleted = recordDeleted;
            AdditionalDataField = additionalDataField;
        }

        public static AuditingAttributeResponse CanAuditDbSet(Type controllerType)
        {
            if (controllerType is null)
                return EmptySet();

            var type = controllerType.GetType();
            if (!type.IsClass)
                throw new ArgumentException($"{nameof(controllerType)} must be of Class type", nameof(controllerType));

            var attrs = controllerType.GetCustomAttributes(typeof(AuditingAttribute), false);

            if (attrs.Length <= 0) return EmptySet();

            var auditingAttribute = ((AuditingAttribute)attrs[0]);

            if (string.IsNullOrEmpty(auditingAttribute.Fields))
                return EmptySet();

            var fieldList = auditingAttribute.Fields.Split(",", StringSplitOptions.RemoveEmptyEntries);

            return new AuditingAttributeResponse
            {
                AuditDbSet = auditingAttribute.AuditDbSet,
                ParentEntityKeyField = auditingAttribute.ParentEntityKeyField,
                Fields = fieldList,
                RecordAdded = auditingAttribute.RecordAdded,
                RecordFieldOnDelete = auditingAttribute.RecordFieldOnDelete,
                RecordDeleted = auditingAttribute.RecordDeleted,
                AdditionalDataField = auditingAttribute.AdditionalDataField
            };

        }

        private static AuditingAttributeResponse EmptySet()
        {
            return new AuditingAttributeResponse { AuditDbSet = false, Fields = Array.Empty<string>(), 
                ParentEntityKeyField = null, RecordAdded = false, RecordDeleted = false, RecordFieldOnDelete = null, 
                AdditionalDataField = null };
        }
    }

    public class AuditingAttributeResponse
    {
        public bool AuditDbSet { get; set; }
        public string ParentEntityKeyField { get; set; }
        public string[] Fields { get; set; }
        public bool RecordAdded { get; set; }
        public bool RecordDeleted { get; set; }
        public string RecordFieldOnDelete { get; set; }
        public string AdditionalDataField { get; set; }
    }
}
