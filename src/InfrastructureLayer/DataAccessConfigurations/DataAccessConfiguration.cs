﻿using System.ComponentModel.DataAnnotations;

namespace InfrastructureLayer.DataAccessConfigurations
{
    public class DataAccessConfig : IValidatableObject
    {
        public string DBProvider { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;


        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(DBProvider))
            {
                yield return new ValidationResult(
                    $"{nameof(DataAccessConfig)}" +
                    $".{nameof(DBProvider)} is not configured",
                    new[] { nameof(DBProvider) });
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                yield return new ValidationResult(
                    $"{nameof(DataAccessConfig)}" +
                    $".{nameof(ConnectionString)} is not configured",
                    new[] { nameof(ConnectionString) });
            }
        }
    }
}
