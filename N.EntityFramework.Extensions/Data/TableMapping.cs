﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace N.EntityFramework.Extensions
{
    public class TableMapping
    {
        public EntitySetMapping Mapping { get; set; }
        public EntitySet EntitySet { get; set; }
        public EntityType EntityType { get; set; }
        public List<ScalarPropertyMapping> Columns { get; set; }
        public List<ConditionPropertyMapping> Conditions { get; set; }
        public string Schema { get; }
        public string TableName { get; }
        public string FullQualifedTableName
        {
            get { return string.Format("[{0}].[{1}]", this.Schema, this.TableName);  }
        }

        public bool HasIdentity => this.Columns.Any(o => o.Column.IsStoreGeneratedIdentity);

        public TableMapping(EntitySet entitySet, EntityType entityType, EntitySetMapping mapping, 
            List<ScalarPropertyMapping> columns, List<ConditionPropertyMapping> conditions)
        {
            var storeEntitySet = mapping.EntityTypeMappings.First(o => o.EntityType != null && o.EntityType.Name == entityType.Name).Fragments.Single().StoreEntitySet;
            
            EntitySet = entitySet;
            EntityType = entityType;
            Mapping = mapping;
            Columns = columns;
            Conditions = conditions;
            Schema = (string)storeEntitySet.MetadataProperties["Schema"].Value ?? storeEntitySet.Schema; 
            TableName = (string)storeEntitySet.MetadataProperties["Table"].Value ?? storeEntitySet.Name;
        }

        public IEnumerable<string> GetColumns(bool keepIdentity = false)
        {
            var columns = new List<string>();
            columns.AddRange(this.Columns.Where(o => keepIdentity || !o.Column.IsStoreGeneratedIdentity).Select(o => o.Column.Name));
            columns.AddRange(this.Conditions.Select(o => o.Column.Name));
            return columns;
        }
        public IEnumerable<string> GetPrimaryKeyColumns()
        {
            return EntitySet.ElementType.KeyMembers.Select(o => o.Name);
            //return Columns.Where(o => o.Column.IsStoreGeneratedIdentity).Select(o => o.Column.Name);
        }
    }
}

