namespace Business.DynamicModelReflector.Models
{
    public class PrimaryKeyInfo
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public bool DataType { get; set; }

        public bool IsIdentity { get; set; }

        public object InsertedValue { get; set; }
    }
}