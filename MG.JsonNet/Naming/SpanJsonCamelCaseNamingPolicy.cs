namespace MG.JsonNet.Naming;

internal sealed class SpanJsonCamelCaseNamingPolicy : SpanJsonNamingPolicy
{
	protected override JsonNamingPolicy BackingPolicy => CamelCase;

	public override Span<char> ConvertSpan(Span<char> chars)
	{
		for (int i = 0; i < chars.Length; i++)
		{
			if (!FixCharCase(chars, i))
				break;
		}

		return chars;
    }
	public override Span<byte> ConvertSpan(Span<byte> utf8Text)
	{
		if (utf8Text.IsEmpty)
		{
			return utf8Text;
		}

		// Decode the first rune from the span.
		var status = Rune.DecodeFromUtf8(utf8Text, out Rune firstRune, out int bytesConsumed);
		Debug.Assert(status == OperationStatus.Done);
		if (status != OperationStatus.Done)
		{
			throw new JsonException("Invalid UTF-8 sequence.");
		}

		// Encode the lowercase rune back into the span.
		Span<byte> tempSlice = utf8Text.Slice(0, bytesConsumed);

		// Convert the first rune to lowercase.
		Rune lowerRune = Rune.ToLowerInvariant(firstRune);

		// Check if a change is needed
		if (firstRune != lowerRune)
		{
			// Re-encode the lowercase rune back into the span.
			// Note: Encoding might not change byte count since TitleCase generally implies simple capital letters.
			int written = lowerRune.EncodeToUtf8(tempSlice);
			if (bytesConsumed != written)
			{
				throw new JsonException("Unexpected change in byte length when converting to lowercase");
			}
		}

		return utf8Text;
    }

	private static bool FixCharCase(Span<char> chars, int i)
	{
		ref char current = ref chars[i];
		if (i == 1 && !char.IsUpper(current))
		{
			return false;
		}

		bool hasNext = i + 1 < chars.Length;

		// Stop when next char is already lowercase.
		if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
		{
			// If the next char is a space, lowercase current char before exiting.
			if (chars[i + 1] == ' ')
			{
				current = char.ToLowerInvariant(current);
			}

			return false;
		}

		current = char.ToLowerInvariant(current);
		return true;
	}
}