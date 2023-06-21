using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;

namespace TableParser
{
    public static class MapperCreator
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<object[], Primary>()
                        .ForAllMembers(MapTableProperty<Primary>);
                    cfg.CreateMap<Table, List<Primary>>()
                        .ConvertUsing(MapTable<Primary>);
                }
            );

            return config.CreateMapper();
        }

        private static void MapTableProperty<TResult>(IMemberConfigurationExpression<object[], TResult, object> configurationExpression)
        {
            var lowerCaseName = configurationExpression.DestinationMember.Name.ToLowerInvariant();
            var type = ((PropertyInfo)configurationExpression.DestinationMember).PropertyType;
            if (type == null)
                return;

            configurationExpression.MapFrom((src, _, _, ctx) =>
            {
                var columns = (Dictionary<string, ColumnInfo>)ctx.Items["columns"];
                var matchingColumn = columns.GetValueOrDefault(lowerCaseName);
                if (matchingColumn != null)
                {
                    var rawValue = src[matchingColumn.Index];
                    if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        if (rawValue == null)
                        {
                            return null;
                        }

                        type = Nullable.GetUnderlyingType(type);
                    }

                    return (rawValue == null) ? null : Convert.ChangeType(rawValue, type!);
                }

                return type.GetDefault();
            });
        }

        private static List<TResult> MapTable<TResult>(Table table, List<TResult> _, ResolutionContext ctx)
        {
            var columnsDict = table.Columns
                .Select((c, ix) => new { Name = c.Name.ToLowerInvariant(), c.Type, ix })
                .ToDictionary(c => c.Name, c => new ColumnInfo
                {
                    Name = c.Name,
                    Index = c.ix,
                    Type = c.Type
                });

            var result = new List<TResult>();
            ctx.Items.Add("columns", columnsDict);

            foreach (var row in table.Rows)
            {
                result.Add(ctx.Mapper.Map<object[], TResult>(row));
            }

            return result;
        }

        public static object? GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private class ColumnInfo
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }
    }
}