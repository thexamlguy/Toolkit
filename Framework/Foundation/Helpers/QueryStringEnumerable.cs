using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

readonly struct QueryStringEnumerable
{
    private readonly ReadOnlyMemory<char> queryString;

    public QueryStringEnumerable(string? queryString) : this(queryString.AsMemory())
    {

    }

    public QueryStringEnumerable(ReadOnlyMemory<char> queryString)
    {
        this.queryString = queryString;
    }

    public Enumerator GetEnumerator() => new(queryString);

    public readonly struct EncodedNameValuePair
    {
        internal EncodedNameValuePair(ReadOnlyMemory<char> encodedName, ReadOnlyMemory<char> encodedValue)
        {
            EncodedName = encodedName;
            EncodedValue = encodedValue;
        }

        public readonly ReadOnlyMemory<char> EncodedName { get; }

        public readonly ReadOnlyMemory<char> EncodedValue { get; }

        public ReadOnlyMemory<char> DecodeName() => Decode(EncodedName);

        public ReadOnlyMemory<char> DecodeValue() => Decode(EncodedValue);

        private static ReadOnlyMemory<char> Decode(ReadOnlyMemory<char> chars)
        {
            return chars.Length < 16 && chars.Span.IndexOfAny('%', '+') < 0 ? chars : Uri.UnescapeDataString(SpanHelper.ReplacePlusWithSpace(chars.Span)).AsMemory();
        }
    }

    public struct Enumerator
    {
        private ReadOnlyMemory<char> query;

        internal Enumerator(ReadOnlyMemory<char> query)
        {
            Current = default;
            this.query = query.IsEmpty || query.Span[0] != '?' ? query : query[1..];
        }

        public EncodedNameValuePair Current { get; private set; }

        public bool MoveNext()
        {
            while (!query.IsEmpty)
            {
                ReadOnlyMemory<char> segment;
                var delimiterIndex = query.Span.IndexOf('&');
                if (delimiterIndex >= 0)
                {
                    segment = query[..delimiterIndex];
                    query = query[(delimiterIndex + 1)..];
                }
                else
                {
                    segment = query;
                    query = default;
                }

                int equalIndex = segment.Span.IndexOf('=');
                if (equalIndex >= 0)
                {
                    Current = new EncodedNameValuePair(segment[..equalIndex], segment[(equalIndex + 1)..]);
                    return true;
                }
                else if (!segment.IsEmpty)
                {
                    Current = new EncodedNameValuePair(segment, default);
                    return true;
                }
            }

            Current = default;
            return false;
        }
    }

    private static class SpanHelper
    {
        private static readonly SpanAction<char, IntPtr> replacePlusWithSpace = ReplacePlusWithSpaceCore;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string ReplacePlusWithSpace(ReadOnlySpan<char> span)
        {
            fixed (char* ptr = &MemoryMarshal.GetReference(span))
            {
                return string.Create(span.Length, (IntPtr)ptr, replacePlusWithSpace);
            }
        }

        private static unsafe void ReplacePlusWithSpaceCore(Span<char> buffer, IntPtr state)
        {
            fixed (char* ptr = &MemoryMarshal.GetReference(buffer))
            {
                ushort* input = (ushort*)state.ToPointer();
                ushort* output = (ushort*)ptr;

                nint i = 0;
                nint n = (nint)(uint)buffer.Length;

                if (Vector256.IsHardwareAccelerated && n >= Vector256<ushort>.Count)
                {
                    Vector256<ushort> vecPlus = Vector256.Create((ushort)'+');
                    Vector256<ushort> vecSpace = Vector256.Create((ushort)' ');

                    do
                    {
                        Vector256<ushort> vec = Vector256.Load(input + i);
                        Vector256<ushort> mask = Vector256.Equals(vec, vecPlus);
                        Vector256<ushort> res = Vector256.ConditionalSelect(mask, vecSpace, vec);
                        res.Store(output + i);
                        i += Vector256<ushort>.Count;
                    } while (i <= n - Vector256<ushort>.Count);
                }

                if (Vector128.IsHardwareAccelerated && n - i >= Vector128<ushort>.Count)
                {
                    do
                    {
                        Vector128<ushort> vec = Vector128.Load(input + i);
                        Vector128<ushort> vecPlus = Vector128.Create((ushort)'+');
                        Vector128<ushort> mask = Vector128.Equals(vec, vecPlus);
                        Vector128<ushort> vecSpace = Vector128.Create((ushort)' ');
                        Vector128<ushort> res = Vector128.ConditionalSelect(mask, vecSpace, vec);
                        res.Store(output + i);
                        i += Vector128<ushort>.Count;
                    } while (i <= n - Vector128<ushort>.Count);
                }

                for (; i < n; ++i)
                {
                    if (input[i] != '+')
                    {
                        output[i] = input[i];
                    }
                    else
                    {
                        output[i] = ' ';
                    }
                }
            }
        }
    }
}