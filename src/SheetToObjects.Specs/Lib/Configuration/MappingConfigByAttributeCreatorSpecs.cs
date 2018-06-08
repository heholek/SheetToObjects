﻿using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.Configuration
{
    public class MappingConfigByAttributeCreatorSpecs
    {
        [Fact]
        void WhenCreatingFromModelWithoutSheetToObjectConfigAttribute_ItFailsToCreateMappingConfig()
        {
            var result = new MappingConfigByAttributeCreator<WithoutSheetToObjectConfigModel>().CreateMappingConfig();

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        void WhenCreatingFromModelWithAttributes_AttributesAreSetInConfig()
        {
            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();

            result.Value.HasHeaders.Should().BeTrue();
            result.Value.ColumnMappings.Single().Should().BeOfType<IndexColumnMapping>()
                .Which.ColumnIndex.Should().Be(3);

            result.Value.ColumnMappings.Single().Rules.OfType<RequiredRule>().Should().NotBeNull();
            result.Value.ColumnMappings.Single().Rules.OfType<RegexRule>().Should().NotBeNull();
        }

        [Fact]
        void WhenCreatingFromModelWithMappingByLetterAttribute_LetterAttributeIsSetInConfig()
        {
            var result = new MappingConfigByAttributeCreator<LetterAttributeTestModel>().CreateMappingConfig();

            result.Value.ColumnMappings.Single().Should().BeOfType<LetterColumnMapping>()
                .Which.ColumnIndex.Should().Be(2);
        }

        [Fact]
        void WhenCreatingFromModelWithMappingByNameAttributeAndRequiredSettingWithWhitespace_NameAttributeIsSetInConfig()
        {
            var columnName = "StringColumn";

            var result = new MappingConfigByAttributeCreator<ColumnNameAttributeTestModel>().CreateMappingConfig();

            result.Value.ColumnMappings.Single().Should().BeOfType<NameColumnMapping>()
                .Which.ColumnName.Should().Be(columnName);

            result.Value.ColumnMappings.Single().ParsingRules.OfType<RequiredRule>().Single().WhiteSpaceAllowed.Should().BeTrue();
        }

        [Fact]
        void WhenCreatingFromModelWithoutAttributes_CheckIfPropertyIsAutoMapped()
        {
            var result = new MappingConfigByAttributeCreator<AutoMapTestModel>().CreateMappingConfig();

            result.Value.HasHeaders.Should().BeTrue();
            result.Value.ColumnMappings.Single().Should().BeOfType<PropertyColumnMapping>()
                .Which.ColumnName.Should().Be("AutoMap");
        }
    }
}