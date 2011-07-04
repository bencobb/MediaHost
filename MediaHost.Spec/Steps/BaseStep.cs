using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace MediaHost.Spec.Steps
{
    public class BaseStep
    {
        public List<string> ErrorMessages { get; set; }

        public BaseStep()
        {
            ErrorMessages = new List<string>();
        }

        [Then(@"I should receive error message:")]
        public void ThenIShouldReceiveErrorMessageEntityNameIsRequired(Table table)
        {
            for( int i = 0; i < table.RowCount; i++)
            {
                var exception = table.Rows[i]["Exception"].Trim();
                Assert.IsTrue(ErrorMessages.Contains(exception), exception + " - not found");
            }
        }
    }
}
