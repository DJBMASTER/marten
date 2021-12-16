using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Marten.WeaselExport;
using Weasel.Postgresql;

#nullable enable

namespace Marten.Storage
{

    #region sample_IFeatureSchema
    /// <summary>
    /// Defines the database objects for a named feature within your
    /// Marten application
    /// </summary>
    public interface IFeatureSchema : ISchemaObjectGroup
    {
        /// <summary>
        /// Any document or feature types that this feature depends on. Used
        /// to intelligently order the creation and scripting of database
        /// schema objects
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> DependentTypes();

        /// <summary>
        /// Identifier by type for this feature. Used along with the DependentTypes()
        /// collection to control the proper ordering of object creation or scripting
        /// </summary>
        Type StorageType { get; }

        /// <summary>
        /// Write any permission SQL when this feature is exported to a SQL
        /// file
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="writer"></param>
        void WritePermissions(DdlRules rules, TextWriter writer);
    }

    #endregion

    /// <summary>
    /// Base class for easier creation of custom IFeatureSchema objects
    /// </summary>
    public abstract class FeatureSchemaBase: IFeatureSchema
    {
        public string Identifier { get; }

        protected FeatureSchemaBase(string identifier)
        {
            Identifier = identifier;
        }

        public virtual IEnumerable<Type> DependentTypes()
        {
            return new Type[0];
        }

        protected abstract IEnumerable<ISchemaObject> schemaObjects();

        public ISchemaObject[] Objects => schemaObjects().ToArray();

        public virtual Type StorageType => GetType();

        public virtual void WritePermissions(DdlRules rules, TextWriter writer)
        {
            // Nothing
        }
    }
}
