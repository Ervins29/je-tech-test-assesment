using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestAssesment.Integrations.Omdb.Converters;

public class StringToBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            {
                var stringValue = reader.GetString();

                if (bool.TryParse(stringValue, out var result)) return result;

                break;
            }
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
        }

        throw new JsonException($"Unable to convert {reader.GetString()} to bool");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}