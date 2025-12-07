namespace MG.JsonNet.Naming;

#nullable enable

/// <summary>
/// A naming policy that converts property names to a specific standard.
/// </summary>
/// <remarks>
/// The names can either be concrete <see cref="string"/> instances -or- <see cref="Span{T}"/> of 
/// <see cref="char"/> or <see cref="byte"/> values.
/// </remarks>
[DebuggerStepThrough]
public abstract class SpanJsonNamingPolicy : JsonNamingPolicy
{
	/// <summary>
	/// The backing <see cref="JsonNamingPolicy"/> that this policy wraps when <see cref="string"/>-based conversions are requested.
	/// </summary>
	protected abstract JsonNamingPolicy BackingPolicy { get; }

	/// <inheritdoc cref="JsonNamingPolicy.ConvertName(string)" path="/*[not(self::remarks)]"/>
	/// <remarks>
	/// <inheritdoc cref="JsonNamingPolicy.ConvertName(string)" path="/remarks"/>
	/// <para>
	/// The default <see cref="SpanJsonNamingPolicy"/> implementation calls <see cref="JsonNamingPolicy.ConvertName(string)"/> on the backing policy.
	/// </para>
	/// </remarks>
	public override string ConvertName(string name)
	{
		return !ReferenceEquals(this, this.BackingPolicy) // Avoid possible stack overflow
			? this.BackingPolicy.ConvertName(name)
			: name;
	}
	/// <summary>
	/// Converts the provided <see cref="Span{T}"/> of <see cref="char"/> instances to this policy's format using <paramref name="chars"/> 
	/// in-place.
	/// </summary>
	/// <param name="chars">The span of characters to format.</param>
	public abstract Span<char> ConvertSpan(Span<char> chars);
	/// <summary>
	/// Converts the provided <see cref="Span{T}"/> of <see cref="byte"/> instances to this policy's format using <paramref name="utf8Text"/>
	/// in-place.
	/// </summary>
	/// <param name="utf8Text">The utf-8 encoded text to format.</param>
	public abstract Span<byte> ConvertSpan(Span<byte> utf8Text);

	/// <summary>
	/// Gets the span naming policy for camel-case.
	/// </summary>
	public static readonly SpanJsonNamingPolicy SpanCamelCase = new SpanJsonCamelCaseNamingPolicy();
	/// <summary>
	/// Gets the span naming policy for default casing.
	/// </summary>
	public static readonly SpanJsonNamingPolicy SpanDefault = new DefaultSpanNamingPolicy();

	[DebuggerStepThrough, SuppressMessage("Style", "IDE0022:Use block body for method", Justification = "Private class.")]
	private sealed class DefaultSpanNamingPolicy : SpanJsonNamingPolicy
	{
		protected override JsonNamingPolicy BackingPolicy => this;

		public override string ConvertName(string name) => name;
		public override Span<char> ConvertSpan(Span<char> chars) => chars;
		public override Span<byte> ConvertSpan(Span<byte> utf8Text) => utf8Text;
	}
}
