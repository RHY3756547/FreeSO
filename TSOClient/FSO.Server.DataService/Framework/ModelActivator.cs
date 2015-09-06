﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSO.Common.DataService.Framework
{
    public class ModelActivator
    {
        public static T NewInstance<T>() where T : IModel
        {
            return (T)NewInstance(typeof(T));
        }

        public static object NewInstance(Type type)
        {
            var instance = Activator.CreateInstance(type);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            
            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Length > 0) { continue; }
                var propertyType = property.PropertyType;

                if (!IsBasicType(propertyType))
                {
                    //Need to activate child members too
                    property.SetValue(instance, NewInstance(propertyType), null);
                }
            }

            return instance;
        }

        public static bool IsBasicType(Type type)
        {
            return type.IsPrimitive ||
                   type.IsAssignableFrom(typeof(string)) ||
                   type.IsEnum;
        }
    }
}
