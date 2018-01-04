using System;
using System.Collections.Generic;
using System.Text;

namespace AdeSAAS.DataService.Tracking
{
    public interface IRevisionRecord<TVersion>
    {
        TVersion Version { get; set; }
        DateTime Date { get; set; }
    }

    public class RevisionRecord<TVersion> : IRevisionRecord<TVersion>
    {
        public RevisionRecord()
        {
            Date = DateTime.UtcNow;
        }

        public TVersion Version { get; set; }
        public DateTime Date { get; set; }
    }
}
