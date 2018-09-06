// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.StringExtensions;

namespace LavaData.StringExtensions.Test
{
    public class ChangeCapitalizationTest
    {
        [Fact]
        public void ToCamelCaseTest()
        {
            Assert.Null(((string)null).ToCamelCase());
            Assert.Equal(string.Empty, "    ".ToCamelCase());
            Assert.Equal("a", "A".ToCamelCase());
            Assert.Equal("helloThere", "hello-there".ToCamelCase());
            Assert.Equal("helloThere", "hello_there".ToCamelCase());
            Assert.Equal("helloThere", "hello there".ToCamelCase());
            Assert.Equal("helloThere", "helloThere".ToCamelCase());
            Assert.Equal("helloThere", "HelloThere".ToCamelCase());
            Assert.Equal("helloThereA", "HelloThereA".ToCamelCase());
            Assert.Equal("mimeText", "MIMEText".ToCamelCase());
            Assert.Equal("_helloThere", "_hello_there".ToCamelCase());
            Assert.Equal("_helloThere", "_HelloThere".ToCamelCase());
            Assert.Equal("_helloThere", "_helloThere".ToCamelCase());
            Assert.Equal("_helloThere", "_hello_there".ToCamelCase());
        }


        [Fact]
        void ToPascalCaseTest()
        {
            Assert.Null(((string)null).ToPascalCase());
            Assert.Equal(string.Empty, "    ".ToPascalCase());
            Assert.Equal("A", "A".ToPascalCase());
            Assert.Equal("HelloThere", "hello-there".ToPascalCase());
            Assert.Equal("HelloThere", "hello_there".ToPascalCase());
            Assert.Equal("HelloThere", "hello there".ToPascalCase());
            Assert.Equal("HelloThere", "HelloThere".ToPascalCase());
            Assert.Equal("HelloThereA", "HelloThereA".ToPascalCase());
            Assert.Equal("MimeText", "MIMEText".ToPascalCase());
            Assert.Equal("_HelloThere", "_hello_there".ToPascalCase());
            Assert.Equal("_HelloThere", "_HelloThere".ToPascalCase());
            Assert.Equal("_HelloThere", "_hello_there".ToPascalCase());
        }

        [Fact]
        void ToSnakeCaseTest()
        {
            Assert.Null(((string)null).ToSnakeCase());
            Assert.Equal(string.Empty, "    ".ToSnakeCase());
            Assert.Equal("a", "A".ToSnakeCase());
            Assert.Equal("hello_there", "hello-there".ToSnakeCase());
            Assert.Equal("hello_there", "hello_there".ToSnakeCase());
            Assert.Equal("hello_there", "hello__there".ToSnakeCase());
            Assert.Equal("hello_there", "hello there".ToSnakeCase());
            Assert.Equal("hello_there", "HelloThere".ToSnakeCase());
            Assert.Equal("mime_text", "MIMEText".ToSnakeCase());
            Assert.Equal("_hello_there", "_hello_there".ToSnakeCase());
            Assert.Equal("_hello_there", "_HelloThere".ToSnakeCase());
            Assert.Equal("_hello_there", "_hello_there".ToSnakeCase());
        }

        [Fact]
        void ToHtmlIdCaseTest()
        {
            Assert.Null(((string)null).ToKebabCase());
            Assert.Equal(string.Empty, "    ".ToKebabCase());
            Assert.Equal("a", "A".ToKebabCase());
            Assert.Equal("hello-there", "hello-there".ToKebabCase());
            Assert.Equal("hello-there", "hello_there".ToKebabCase());
            Assert.Equal("hello-there", "hello__there".ToKebabCase());
            Assert.Equal("hello-there", "hello there".ToKebabCase());
            Assert.Equal("hello-there", "HelloThere".ToKebabCase());
            Assert.Equal("mime-text", "MIMEText".ToKebabCase());
            Assert.Equal("-hello-there", "_hello_there".ToKebabCase());
            Assert.Equal("-hello-there", "_HelloThere".ToKebabCase());
            Assert.Equal("-hello-there", "_hello_there".ToKebabCase());
        }

    }
}
