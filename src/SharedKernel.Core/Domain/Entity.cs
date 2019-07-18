using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Domain
{
    /// <summary>
    /// Base entity for domain classes.
    /// </summary>
    /// <typeparam name="TId">Type of entity id.</typeparam>
    public abstract class Entity<TId> : IEntity
    {
        /// <summary>
        /// Gets or sets entity id.
        /// </summary>
        /// <value>
        /// Entity id.
        /// </value>
        public virtual TId Id { get; set; }

        /// <summary>
        /// Gets or sets entity version for MongoDb.
        /// </summary>
        /// <value>
        /// Entity version.
        /// </value>
        [NotMapped]
        public virtual int? Version { get; set; }

        /// <summary>
        /// Equals operator comparing entity ids.
        /// </summary>
        /// <param name="a">First entity for comparison.</param>
        /// <param name="b">Second entity for comparison.</param>
        /// <returns>True if first entity id is equals to second entity id. Otherwise false.</returns>
        public static bool operator ==(Entity<TId> a, Entity<TId> b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Different operator comparing entity ids.
        /// </summary>
        /// <param name="a">First entity for comparison.</param>
        /// <param name="b">Second entity for comparison.</param>
        /// <returns>True if first entity id is different to second entity id. Otherwise false.</returns>
        public static bool operator !=(Entity<TId> a, Entity<TId> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Equals overrides comparing entity ids.
        /// </summary>
        /// <param name="obj">Second entity for comparison.</param>
        /// <returns>True if first entity id is equals to second entity id. Otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TId>;

            if (ReferenceEquals(this, compareTo))
            {
                return true;
            }

            if (ReferenceEquals(null, compareTo))
            {
                return false;
            }

            return Id.Equals(compareTo.Id);
        }

        /// <summary>
        /// To string everrides to show entity id.
        /// </summary>
        /// <returns>Entity with id.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        /// <summary>
        /// Generate hash code with entity type hash plus entity id hash.
        /// </summary>
        /// <returns>Hash code generated.</returns>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        /// <summary>
        /// Clone an entity to another with type.
        /// </summary>
        /// <typeparam name="T">Type of target entity.</typeparam>
        /// <returns>Entity cloned.</returns>
        public virtual T Clone<T>()
        {
            return (T)Clone();
        }

        /// <summary>
        /// Clone an entity.
        /// </summary>
        /// <returns>Return entity cloned object.</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
