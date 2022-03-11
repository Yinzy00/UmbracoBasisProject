using BasisProjectUmbraco.models;
using System;
using System.Globalization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace BasisProjectUmbraco.ValueConverters
{
    public class MetaTagsValueConverter : IPropertyValueConverter
    {
        public MetaTagsValueConverter()
        {
        }
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        public MetaTagsValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
        }
        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null)
                return null;

            if ((propertyType.Alias != null) == false)
            {
                IPublishedContent content;
                if (inter is int id)
                {
                    content = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Content.GetById(id);
                    if (content != null)
                        return content;
                }
                else
                {
                    var udi = inter as GuidUdi;
                    if (udi == null)
                        return null;
                    content = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Content.GetById(udi.Guid);
                    if (content != null && content.ContentType.ItemType == PublishedItemType.Content)
                        return content;
                }
            }

            return inter;
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null) return null;
            return inter.ToString();
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return null;

            var attemptConvertInt = source.TryConvertTo<int>();
            if (attemptConvertInt.Success)
                return attemptConvertInt.Result;

            var attemptConvertUdi = source.TryConvertTo<Udi>();
            if (attemptConvertUdi.Success)
                return attemptConvertUdi.Result;

            return null;
            //if (source == null)
            //    return null;

            //try
            //{
            //    var convertedObj = (source as MetaTagsDataType);
            //    return convertedObj;
            //}
            //catch (Exception) { return null; }
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Elements;
        }

        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(MetaTagsDataType);
        }

        public bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals("MetaTag");
        }

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            switch (level)
            {
                case PropertyValueLevel.Source:
                    return value != null && (!(value is string) || string.IsNullOrWhiteSpace((string)value) == false);
                default:
                    throw new NotSupportedException($"Invalid level: {level}.");
            }
        }
    }


    //    public class MetaTagsValueConverter : IPropertyValueConverter
    //    {
    //        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
    //        public MetaTagsValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor)
    //        {
    //            _publishedSnapshotAccessor = publishedSnapshotAccessor;
    //        }
    //        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
    //        {
    //            if (inter == null)
    //                return null;

    //            if ((propertyType.Alias != null) == false)
    //            {
    //                IPublishedContent content;
    //                if (inter is int id)
    //                {
    //                    content = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Content.GetById(id);
    //                    if (content != null)
    //                        return content;
    //                }
    //                else
    //                {
    //                    var udi = inter as GuidUdi;
    //                    if (udi == null)
    //                        return null;
    //                    content = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Content.GetById(udi.Guid);
    //                    if (content != null && content.ContentType.ItemType == PublishedItemType.Content)
    //                        return content;
    //                }
    //            }

    //            return inter;
    //        }

    //        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
    //        {
    //            if (inter == null) return null;
    //            return inter.ToString();
    //        }

    //        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
    //        {
    //            if (source == null) return null;

    //            var attemptConvertInt = source.TryConvertTo<int>();
    //            if (attemptConvertInt.Success)
    //                return attemptConvertInt.Result;

    //            var attemptConvertUdi = source.TryConvertTo<Udi>();
    //            if (attemptConvertUdi.Success)
    //                return attemptConvertUdi.Result;

    //            return null;
    //            //if (source == null)
    //            //    return null;

    //            //try
    //            //{
    //            //    var convertedObj = (source as MetaTagsDataType);
    //            //    return convertedObj;
    //            //}
    //            //catch (Exception) { return null; }
    //        }

    //        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
    //        {
    //            return PropertyCacheLevel.Elements;
    //        }

    //        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
    //        {
    //            return typeof(MetaTagsDataType);
    //        }

    //        public bool IsConverter(IPublishedPropertyType propertyType)
    //        {
    //            return propertyType.EditorAlias.Equals("MetaTag");
    //        }

    //        public bool? IsValue(object value, PropertyValueLevel level)
    //        {
    //            switch (level)
    //            {
    //                case PropertyValueLevel.Source:
    //                    return value != null && (!(value is string) || string.IsNullOrWhiteSpace((string)value) == false);
    //                default:
    //                    throw new NotSupportedException($"Invalid level: {level}.");
    //            }
    //        }
    //    }
}
