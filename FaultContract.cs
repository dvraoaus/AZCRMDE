namespace Arizona.Courts.Extensions.v20
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34209")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schema.azcourts.az.gov/courts/efiling/ecf/extension/2.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://schema.azcourts.az.gov/courts/efiling/ecf/extension/2.0", IsNullable=true)]
    public partial class OperationExceptionType
    {
        
        private string operationField;
        
        private string exceptionDetailField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ExceptionDetail
        {
            get
            {
                return this.exceptionDetailField;
            }
            set
            {
                this.exceptionDetailField = value;
            }
        }
    }
}
