using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class Pairs
        {
            public sealed class AmbiguousSettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                [PairDeconstructor(typeof(StringIntDeconstructor))]
                [TypeConverter(typeof(CatAgilityConverter))]
                public ILookup<string, string> Values { get; set; }
            }

            public sealed class NotDeconstructableSettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                [PairDeconstructor(typeof(StringIntDeconstructor))]
                public string Values { get; set; }
            }

            public sealed class DefaultPairDeconstructorSettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                public IDictionary<string, int> Values { get; set; }
            }

            public sealed class LookupSettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                [PairDeconstructor(typeof(StringIntDeconstructor))]
                public ILookup<string, string> Values { get; set; }
            }

            public sealed class DictionarySettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                [PairDeconstructor(typeof(StringIntDeconstructor))]
                public IDictionary<string, string> Values { get; set; }
            }

            public sealed class ReadOnlyDictionarySettings : CommandSettings
            {
                [CommandOption("--var <VALUE>")]
                [PairDeconstructor(typeof(StringIntDeconstructor))]
                public IReadOnlyDictionary<string, string> Values { get; set; }
            }

            public sealed class StringIntDeconstructor : PairDeconstuctor<string, string>
            {
                protected override (string Key, string Value) Deconstruct(string value)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    var parts = value.Split(new[] { '=' });
                    if (parts.Length != 2)
                    {
                        throw new InvalidOperationException("Could not parse pair");
                    }

                    return (parts[0], parts[1]);
                }
            }

            [Fact]
            public void Should_Throw_If_Option_Has_Pair_Deconstructor_And_Type_Converter()
            {
                // Given
                var app = new CommandApp<GenericCommand<AmbiguousSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = Record.Exception(() => app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "foo=qux",
                }));

                // Then
                result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The option 'var' is both marked as pair deconstructable and convertable.");
                });
            }

            [Fact]
            public void Should_Throw_If_Option_Has_Pair_Deconstructor_But_Type_Is_Not_Deconstructable()
            {
                // Given
                var app = new CommandApp<GenericCommand<NotDeconstructableSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = Record.Exception(() => app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "foo=qux",
                }));

                // Then
                result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The option 'var' is marked as pair deconstructable, but the underlying type does not support that.");
                });
            }

            [Fact]
            public void Should_Map_Pairs_To_Pair_Deconstructable_Collection_Using_Default_Deconstructort()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<DefaultPairDeconstructorSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--var", "foo=1",
                    "--var", "foo=3",
                    "--var", "bar=4",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DefaultPairDeconstructorSettings>().And(pair =>
                {
                    pair.Values.ShouldNotBeNull();
                    pair.Values.Count.ShouldBe(2);
                    pair.Values["foo"].ShouldBe(3);
                    pair.Values["bar"].ShouldBe(4);
                });
            }

            [Theory]
            [InlineData("foo=1=2", "Error: The value 'foo=1=2' is not in a correct format")]
            [InlineData("foo=1=2=3", "Error: The value 'foo=1=2=3' is not in a correct format")]
            public void Should_Throw_If_Value_Is_Not_In_A_Valid_Format_Using_Default_Deconstructor(
                string input, string expected)
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<DefaultPairDeconstructorSettings>>();

                // When
                var result = app.Run(new[]
                {
                    "--var", input,
                });

                // Then
                result.ExitCode.ShouldBe(-1);
                result.Output.ShouldBe(expected);
            }

            [Fact]
            public void Should_Map_Lookup_Values()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<LookupSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "foo=qux",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<LookupSettings>().And(pair =>
                {
                    pair.Values.ShouldNotBeNull();
                    pair.Values.Count.ShouldBe(1);
                    pair.Values["foo"].ToList().Count.ShouldBe(2);
                });
            }

            [Fact]
            public void Should_Map_Dictionary_Values()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<DictionarySettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "baz=qux",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DictionarySettings>().And(pair =>
                {
                    pair.Values.ShouldNotBeNull();
                    pair.Values.Count.ShouldBe(2);
                    pair.Values["foo"].ShouldBe("bar");
                    pair.Values["baz"].ShouldBe("qux");
                });
            }

            [Fact]
            public void Should_Map_Latest_Value_Of_Same_Key_When_Mapping_To_Dictionary()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<DictionarySettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "foo=qux",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DictionarySettings>().And(pair =>
                {
                    pair.Values.ShouldNotBeNull();
                    pair.Values.Count.ShouldBe(1);
                    pair.Values["foo"].ShouldBe("qux");
                });
            }

            [Fact]
            public void Should_Map_ReadOnly_Dictionary_Values()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<ReadOnlyDictionarySettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run(new[]
                {
                    "--var", "foo=bar",
                    "--var", "baz=qux",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<ReadOnlyDictionarySettings>().And(pair =>
                {
                    pair.Values.ShouldNotBeNull();
                    pair.Values.Count.ShouldBe(2);
                    pair.Values["foo"].ShouldBe("bar");
                    pair.Values["baz"].ShouldBe("qux");
                });
            }
        }
    }
}
