﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using Test.Cryptography;
using Xunit;

namespace System.Security.Cryptography.Encoding.Tests.Cbor
{
    public partial class CborReaderTests
    {
        [Fact]
        public static void Peek_EmptyBuffer_ShouldReturnEof()
        {
            var reader = new CborReader(ReadOnlyMemory<byte>.Empty);
            Assert.Equal(CborReaderState.EndOfData, reader.Peek());
        }

        [Fact]
        public static void BytesRead_NoReads_ShouldReturnZero()
        {
            var reader = new CborReader(new byte[10]);
            Assert.Equal(0, reader.BytesRead);
        }

        [Fact]
        public static void BytesRemaining_NoReads_ShouldReturnBufferSize()
        {
            var reader = new CborReader(new byte[10]);
            Assert.Equal(10, reader.BytesRemaining);
        }


        [Fact]
        public static void BytesRead_SingleRead_ShouldReturnConsumedBytes()
        {
            var reader = new CborReader(new byte[] { 24, 24 });
            reader.ReadInt64();
            Assert.Equal(2, reader.BytesRead);
        }

        [Fact]
        public static void BytesRemaining_SingleRead_ShouldReturnRemainingBytes()
        {
            var reader = new CborReader(new byte[] { 24, 24 });
            reader.ReadInt64();
            Assert.Equal(0, reader.BytesRemaining);
        }

        [Theory]
        [InlineData(CborMajorType.UnsignedInteger, CborReaderState.UnsignedInteger)]
        [InlineData(CborMajorType.NegativeInteger, CborReaderState.NegativeInteger)]
        [InlineData(CborMajorType.ByteString, CborReaderState.ByteString)]
        [InlineData(CborMajorType.TextString, CborReaderState.TextString)]
        [InlineData(CborMajorType.Array, CborReaderState.StartArray)]
        [InlineData(CborMajorType.Map, CborReaderState.StartMap)]
        [InlineData(CborMajorType.Tag, CborReaderState.Tag)]
        [InlineData(CborMajorType.Special, CborReaderState.Special)]
        internal static void Peek_SingleByteBuffer_ShouldReturnExpectedState(CborMajorType majorType, CborReaderState expectedResult)
        {
            ReadOnlyMemory<byte> buffer = new byte[] { (byte)((byte)majorType << 5) };
            var reader = new CborReader(buffer);
            Assert.Equal(expectedResult, reader.Peek());
        }

        [Fact]
        public static void CborReader_ReadingTwoPrimitiveValues_ShouldThrowInvalidOperationException()
        {
            ReadOnlyMemory<byte> buffer = new byte[] { 0, 0 };
            var reader = new CborReader(buffer);
            reader.ReadInt64();
            Assert.Equal(CborReaderState.Finished, reader.Peek());
            Assert.Throws<InvalidOperationException>(() => reader.ReadInt64());
        }
    }
}
