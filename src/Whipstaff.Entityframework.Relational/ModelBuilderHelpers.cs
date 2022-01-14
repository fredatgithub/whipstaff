﻿// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Whipstaff.EntityFramework
{
    /// <summary>
    /// Entity Framework Model Builder Helpers for SQL Lite.
    /// </summary>
    public static class ModelBuilderHelpers
    {
        /// <summary>
        /// Converts all DBSet entities that contain a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework Model Builder being configured.</param>
        public static void ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(
                    modelBuilder,
                    mutableEntityType);
            }
        }

        /// <summary>
        /// Converts all properties on a DBSet entity that are a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework Model Builder being configured.</param>
        /// <param name="mutableEntityType">Mutable Entity Type Representing the DBSet to check.</param>
        public static void ConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(
            ModelBuilder modelBuilder,
            IMutableEntityType mutableEntityType)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (mutableEntityType == null)
            {
                throw new ArgumentNullException(nameof(mutableEntityType));
            }

            InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(
                modelBuilder,
                mutableEntityType);
        }

        /// <summary>
        /// Converts a property properties on a DBSet entity that are a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework Model Builder being configured.</param>
        /// <param name="entityClrType">The CLR type of the Entity represented as a DBSet.</param>
        /// <param name="propertyName">The name of the property that's to be converted from DateTimeOffset.</param>
        public static void ConvertDateTimeOffSetPropertyToLong(
            ModelBuilder modelBuilder,
            Type entityClrType,
            string propertyName)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (entityClrType == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            InternalConvertDateTimeOffSetPropertyToLong(
                modelBuilder,
                entityClrType,
                propertyName);
        }

        /// <summary>
        /// Gets a value convertor for converting a date time offset to unix time milliseconds as a long.
        /// </summary>
        /// <returns>Value convertor.</returns>
        public static ValueConverter<DateTimeOffset, long> GetDateTimeOffSetToUnixTimeMillisecondsLongValueConverter() =>
            new ValueConverter<DateTimeOffset, long>(
                offset => offset.ToUnixTimeMilliseconds(),
                milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(milliseconds));

        private static void InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        {
            foreach (var p in mutableEntityType.GetProperties())
            {
                if (p.ClrType == typeof(DateTimeOffset))
                {
                    ConvertDateTimeOffSetPropertyToLong(
                        modelBuilder,
                        mutableEntityType.ClrType,
                        p.Name);
                }
            }
        }

        private static void InternalConvertDateTimeOffSetPropertyToLong(
            ModelBuilder modelBuilder,
            Type entityClrType,
            string propertyName)
        {
            modelBuilder.Entity(entityClrType)
                .Property(propertyName)
                .HasColumnType("INTEGER")
                .HasConversion(GetDateTimeOffSetToUnixTimeMillisecondsLongValueConverter());
        }
    }
}
