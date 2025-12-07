using MG.JsonNet.Internal;
using MG.JsonNet.Internal.Buffers;

namespace MG.JsonNet.Naming;

[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay(@"\{HasPolicy = {HasPolicy}, IsSpanPolicy = {IsSpanPolicy}, Policy = {Policy}\}")]
public readonly ref struct JsonNamingPolicyOverride
{
	private const int MAX_STACKALLOC = 256;
	private readonly SpanJsonNamingPolicy? _spanPolicy;

	/// <summary>
	/// Gets a value indicating whether a naming policy is present.
	/// </summary>
	/// <value>
	/// <see langword="true"/> if a naming policy is present; otherwise, <see langword="false"/>.
	/// </value>
	[MemberNotNullWhen(true, nameof(Policy), nameof(Options))]
	public readonly bool HasPolicy { get; }

	/// <summary>
	/// Gets the JSON serializer options.
	/// </summary>
	public JsonSerializerOptions? Options { get; }

	/// <summary>
	/// Gets a value indicating whether the naming policy is a span policy.
	/// </summary>
	/// <value>
	/// <see langword="true"/> if the naming policy is a span policy; otherwise, <see langword="false"/>.
	/// </value>
	[MemberNotNullWhen(true, nameof(Policy), nameof(_spanPolicy), nameof(Options))]
	public readonly bool IsSpanPolicy { get; }

	/// <summary>
	/// Gets the JSON naming policy.
	/// </summary>
	public JsonNamingPolicy? Policy { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="WorkingNamingPolicy"/> struct with the specified JSON serializer options.
	/// </summary>
	/// <param name="options">The JSON serializer options.</param>
	public JsonNamingPolicyOverride(JsonSerializerOptions? options, bool @override)
	{
		this.Options = options;
		JsonNamingPolicy? pol = !@override ? options?.PropertyNamingPolicy : null;
		bool hasPol = pol is not null;
		if (hasPol && pol is SpanJsonNamingPolicy spanPolicy)
		{
			_spanPolicy = spanPolicy;
			this.IsSpanPolicy = true;
		}

		this.HasPolicy = hasPol;
		this.Policy = pol;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="JsonNamingPolicyOverride"/> struct using the specified naming policy and override
	/// flag.
	/// </summary>
	/// <param name="policy">The WorkingNamingPolicy instance that provides the naming options and policy to apply.</param>
	/// <param name="override">A value indicating whether to override the provided naming policy. If <see langword="true"/>, the override is
	/// applied and the original policy is ignored.</param>
	internal JsonNamingPolicyOverride(WorkingNamingPolicy policy, bool @override)
	{
		this.Options = policy.Options;
		policy.SetOverride(ref _spanPolicy);
		this.HasPolicy = !@override && policy.HasPolicy;
		this.IsSpanPolicy = !@override && policy.IsSpanPolicy;
		this.Policy = policy.Policy;
	}

	/// <summary>
	/// Converts the specified UTF-8 property name to a new name using the naming policy.
	/// </summary>
	/// <param name="utf8PropertyName">The UTF-8 property name.</param>
	/// <param name="buffer">The buffer to store the converted name.</param>
	/// <returns>The converted name as a read-only span of bytes.</returns>
	public readonly ReadOnlySpan<byte> ConvertName(ReadOnlySpan<byte> utf8PropertyName, Span<byte> buffer)
	{
		if (!this.HasPolicy)
		{
			return utf8PropertyName;
		}

		if (this.IsSpanPolicy && utf8PropertyName.TryCopyTo(buffer))
		{
			_spanPolicy.ConvertSpan(buffer);
			return buffer;
		}

		int max = Encoding.UTF8.GetMaxCharCount(utf8PropertyName.Length);

		char[]? array = null;
		Span<char> chars = max <= MAX_STACKALLOC
			? stackalloc char[max]
			: Rent.Array(ref array, max);

		try
		{
			int written = Utf8Helper.GetChars(utf8PropertyName, chars);
			string name = this.Policy.ConvertName(StringHelper.Create(chars.Slice(0, written)));
			if (Encoding.UTF8.GetMaxByteCount(name.Length) > buffer.Length)
			{
				Debug.Fail("An allocation happened AND the destination was too small anyway.");
				return Encoding.UTF8.GetBytes(name);
			}

			Debug.Fail("An allocation happened here ^");
			written = Utf8Helper.GetBytes(name, buffer);
			return buffer.Slice(0, written);

		}
		finally
		{
			Rent.Return(array);
		}
	}

	/// <summary>
	/// Tries to convert the specified property name to a new name using the naming policy.
	/// </summary>
	/// <param name="propertyName">The property name.</param>
	/// <param name="destination">The destination buffer to store the converted name.</param>
	/// <returns>
	/// <see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.
	/// </returns>
	public readonly bool TryConvertName([NotNullWhen(true)] string? propertyName, Span<char> destination)
	{
		if (!this.IsSpanPolicy || string.IsNullOrWhiteSpace(propertyName) || !propertyName.AsSpan().TryCopyTo(destination))
			return false;

		_spanPolicy.ConvertSpan(destination);
		return true;
	}

	/// <summary>
	/// Tries to convert the specified property name to a new name using the naming policy.
	/// </summary>
	/// <param name="propertyName">The property name.</param>
	/// <param name="destination">The destination buffer to store the converted name.</param>
	/// <returns>
	/// <see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.
	/// </returns>
	public readonly bool TryConvertName(ReadOnlySpan<char> propertyName, Span<char> destination)
	{
		if (!this.IsSpanPolicy || !propertyName.TryCopyTo(destination))
			return false;

		_spanPolicy.ConvertSpan(destination);
		return true;
	}

	/// <summary>
	/// Tries to convert the specified UTF-8 property name to a new name using the naming policy.
	/// </summary>
	/// <param name="utf8PropertyName">The UTF-8 property name.</param>
	/// <param name="destination">The destination buffer to store the converted name.</param>
	/// <returns>
	/// <see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.
	/// </returns>
	public readonly bool TryConvertName(ReadOnlySpan<byte> utf8PropertyName, Span<byte> destination)
	{
		if (!this.IsSpanPolicy || !utf8PropertyName.TryCopyTo(destination))
			return false;

		_spanPolicy.ConvertSpan(destination);
		return true;
	}

	/// <summary>
	/// Writes the property name to the specified <see cref="Utf8JsonWriter"/>.
	/// </summary>
	/// <param name="writer">The JSON writer.</param>
	/// <param name="propertyName">The property name.</param>
	public readonly void WritePropertyName(Utf8JsonWriter writer, string propertyName)
	{
		Guard.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
		if (this.HasPolicy)
		{
			if (this.IsSpanPolicy)
			{
				WorkingNamingPolicy.WriteCharSpan(writer, _spanPolicy, propertyName);
				return;
			}

			propertyName = this.Policy.ConvertName(propertyName);
		}

		writer.WritePropertyName(propertyName);
	}

	/// <summary>
	/// Writes the property name to the specified <see cref="Utf8JsonWriter"/>.
	/// </summary>
	/// <param name="writer">The JSON writer.</param>
	/// <param name="propertyNameSpan">The property name as a read-only span of characters.</param>
	public readonly void WritePropertyName(Utf8JsonWriter writer, ReadOnlySpan<char> propertyNameSpan)
	{
		if (this.HasPolicy)
		{
			if (this.IsSpanPolicy)
			{
				WorkingNamingPolicy.WriteCharSpan(writer, _spanPolicy, propertyNameSpan);
				return;
			}

			propertyNameSpan = this.Policy.ConvertName(StringHelper.Create(propertyNameSpan));
			Debug.Fail("An allocation happened here ^");
		}

		writer.WritePropertyName(propertyNameSpan);
	}

	/// <summary>
	/// Writes the UTF-8 encoded property name to the specified <see cref="Utf8JsonWriter"/>.
	/// </summary>
	/// <param name="writer">The JSON writer.</param>
	/// <param name="propertyName">The property name as a read-only span of bytes.</param>
	public readonly void WritePropertyName(Utf8JsonWriter writer, ReadOnlySpan<byte> propertyName)
	{
		//EmptyStructException.ThrowIf(propertyName.IsEmpty, propertyName);
		if (!this.HasPolicy)
		{
			writer.WritePropertyName(propertyName);
			return;
		}
		else if (this.IsSpanPolicy)
		{
			WorkingNamingPolicy.WritePropertyName(writer, propertyName, _spanPolicy);
			return;
		}

		int length = Encoding.UTF8.GetMaxCharCount(propertyName.Length);

		char[]? array = null;
		Span<char> chars = length <= MAX_STACKALLOC
			? stackalloc char[length]
			: Rent.Array(ref array, length);

		try
		{
			int written = Utf8Helper.GetChars(propertyName, chars);

			string name = this.Policy.ConvertName(StringHelper.Create(chars.Slice(0, written)));
			Debug.Fail("An allocation happened here ^");
			writer.WritePropertyName(name);
		}
		finally
		{
            Rent.Return(array);
        }
	}
}
