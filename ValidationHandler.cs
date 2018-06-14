using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Serialization
{
    public class ValidationHandler
    {
        public List<ValidationErrors> MyValidationErrors
        {
            get;
            private set;
        } = new List<ValidationErrors>();

        public void HandleValidationError(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error || args.Severity == XmlSeverityType.Warning)
            {
                MyValidationErrors.Add(
                    new ValidationErrors(
                        args.Severity,
                        args.Exception.Message,
                        args.Exception.LineNumber,
                        args.Exception.LinePosition
                    )
                );
            }
        }
    }

    public class ValidationErrors
    {
        public XmlSeverityType Severity { get; private set; }
        public string Message { get; private set; }
        public int Line { get; private set; }
        public int Position { get; private set; }

        public ValidationErrors(XmlSeverityType Severity, string Message, int Line, int Position)
        {
            this.Severity = Severity;
            this.Message = Message;
            this.Line = Line;
            this.Position = Position;
        }
    }
}
