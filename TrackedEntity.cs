using AdeSAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdeSAAS.DataService.Tracking
{
    public interface ITrackedEntity<TEntity, TVersion,TKey> : IEntityModel<TKey>
       where TVersion : IComparable
       where TEntity : ITrackableEntity<TVersion>, new()
    {
        List<RevisionRecord<TVersion>> Revisions { get; set; }
        TEntity GetRevision(TVersion version = default(TVersion));
        void Initialize(TEntity entity);
    }
    public interface ITrackedEntity<TEntity, TVersion> : ITrackedEntity<TEntity, TVersion, string>
         where TVersion : IComparable
       where TEntity : ITrackableEntity<TVersion>, new()
    {

    }

    public class TrackedEntity<TEntity, TVersion,TKey> : EntityModel<TKey>, ITrackedEntity<TEntity, TVersion>
        where TEntity : TrackableEntity<TVersion>, new()
        where TVersion : IComparable


    {
        public TrackedEntity()
        {
            Revisions = new List<RevisionRecord<TVersion>>();
        }

        public TrackedEntity(TEntity initialValue)
        {
            Revisions = new List<RevisionRecord<TVersion>>();
            Initialize(initialValue);
        }

        public List<RevisionRecord<TVersion>> Revisions { get; set; }


        public TEntity GetRevision(TVersion version = default(TVersion))
        {
            var versionRecord = version.Equals(default(TVersion))
                ? Revisions.First(x => x.Version.Equals(Revisions.Max(a => a.Version)))
                : Revisions.FirstOrDefault(x => x.Version.CompareTo(version) <= 0);

            var allSourceProperties = GetType()
                .GetProperties();

            var versionedSourceProperties = allSourceProperties
                .Where(x => typeof(ITrackedProperty).IsAssignableFrom(x.PropertyType)).ToList();
            var standardSourceProperties = allSourceProperties
                .Where(x => !versionedSourceProperties.Contains(x));

            var result = new TEntity { Revision = versionRecord };

            var allTargetProperties = result
                .GetType()
                .GetProperties()
                .Where(x => x.SetMethod?.IsPublic == true)
                .ToList();

            foreach (var prop in versionedSourceProperties)
            {
                var value = (ITrackedProperty)prop.GetValue(this);
                result.GetType().GetProperties().First(x => x.Name == prop.Name)
                    .SetValue(result, value.GetVersionValue(version));
            }

            foreach (var prop in standardSourceProperties)
                allTargetProperties
                    .FirstOrDefault(x => x.Name == prop.Name)?
                    .SetValue(result, prop.GetValue(this));

            return result;
        }

        public void Initialize(TEntity entity)
        {
            AddRevision(entity);
        }


        public void AddRevision(TEntity entity, DateTime versionDateTime = default(DateTime))
        {
            if (Revisions.Any())
                if (Revisions.Max(x => x.Version).CompareTo(entity.Revision.Version) >= 0)
                    throw new OldVersionInsertionException();

            Revisions.Add(new RevisionRecord<TVersion>
            {
                Version = entity.Revision.Version,
                Date = versionDateTime == default(DateTime) ? DateTime.UtcNow : versionDateTime
            });

            var targetProperties = GetType()
                .GetProperties().Where(
                    x => x.SetMethod?.IsPublic == true && x.Name != "Revisions").ToArray();

            var sourceProperties = entity.GetType().GetProperties();

            foreach (var prop in targetProperties)
            {
                var sourceValue = sourceProperties
                    .FirstOrDefault(x => x.Name == prop.Name)?
                    .GetValue(entity);

                if (typeof(ITrackedProperty).IsAssignableFrom(prop.PropertyType))
                {
                    var propVal = (ITrackedProperty)prop.GetValue(this);
                    if (propVal == null)
                    {
                        propVal = (ITrackedProperty)Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(this, propVal);
                    }

                    propVal.AddVersion(sourceValue, entity.Revision.Version);
                }
                else
                {
                    prop.SetValue(this, sourceValue);
                }
            }
        }
    }
    public class TrackedEntity<TEntity, TVersion> : TrackedEntity<TEntity, TVersion,string>, ITrackedEntity<TEntity, TVersion>
        where TEntity : TrackableEntity<TVersion>, new()
        where TVersion : IComparable
    {

    }
}
