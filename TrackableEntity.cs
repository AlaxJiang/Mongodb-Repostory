using AdeSAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdeSAAS.DataService.Tracking
{
    public class TrackableEntity<TVersion,TKey> : EntityModel<TKey>, ITrackableEntity<TVersion>
        where TVersion : IComparable
    {
        public RevisionRecord<TVersion> Revision { get; set; }
    }
    public class TrackableEntity<TVersion> : TrackableEntity<TVersion,string> where TVersion : IComparable
    {

    }
    public interface ITrackableEntity<TVersion,TKey> : IEntityModel<TKey>
        where TVersion : IComparable
    {
        RevisionRecord<TVersion> Revision { get; set; }
    }

    public interface ITrackableEntity<TVersion>: ITrackableEntity<TVersion, string> where TVersion : IComparable
    {

    }
}
