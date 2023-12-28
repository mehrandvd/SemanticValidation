using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SemanticValidation.Utils;

namespace SemanticValidation.Tests
{
    public class PowerParseJsonTests
    {
        [Fact]
        public void ParseJson_SimpleObject()
        {
            var text = """
                {
                    "name": "mehran",
                    "desc": "software"
                }
                """;

            var json = SemanticUtils.PowerParseJson<JsonObject>(text);

            Assert.NotNull(json);
        }

        [Fact]
        public void ParseJson_SimpleArray()
        {
            var text = """
                [
                    {
                        "name": "mehran"
                    }  
                ]
                """;

            var json = SemanticUtils.PowerParseJson<JsonArray>(text);

            Assert.NotNull(json);
        }

        [Theory]
        [InlineData("\"")]
        [InlineData("'")]
        [InlineData("`")]
        [InlineData("```")]
        public void ParseJson_Quotes(string quote)
        {
            var text = $$"""
                {{quote}}
                {
                    "name": "mehran",
                    "desc": "software"
                }
                {{quote}}
                """;

            var json = SemanticUtils.PowerParseJson<JsonObject>(text);

            Assert.NotNull(json);
        }

        [Fact]
        public void ParseJson_JsonPrefix()
        {
            var text = """
                ```json
                {
                    "name": "mehran",
                    "desc": "software"
                }
                ```
                """;

            var json = SemanticUtils.PowerParseJson<JsonObject>(text);

            Assert.NotNull(json);
        }

        [Fact]
        public void ParseJson_DoubleQuote()
        {
            var text = """
                "
                {
                    "name": "mehran",
                    "desc": "software"
                }
                ```
                """;

            var json = SemanticUtils.PowerParseJson<JsonObject>(text);

            Assert.NotNull(json);
        }

    }
}
