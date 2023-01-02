using System;

using Xunit;
using TECHIS.Core.Text;

namespace TECHIS.EndApps.App.Tests
{
    public class StringUtilTests
    {
        private const string alphabets = "abcdefghijjklmnopqrstuvwxyz";
        [Fact]
        public void StartsWith_DifferentChars_NotEqual()
        {
            string first = nameof(first);
            string second = nameof(second);

            Assert.False( first.StartsWith(second, 0, first.Length, 0, StringComparison.Ordinal));
        }

        [Fact]
        public void StartsWith_SameChars_DifferentCase_NotEqual()
        {
            string first = nameof(first);
            string second = first.ToUpper();

            Assert.False(first.StartsWith(second, 0, first.Length, 0, StringComparison.Ordinal));
        }


        [Fact]
        public void StartsWith_SameChars_Equal()
        {
            int length = 5;
            int start1 = 0;
            int start2 = 0;
            string first = new(alphabets.AsSpan().Slice(start1,length));
            string second = new(alphabets.AsSpan().Slice(start2, length));

            Assert.True(first.StartsWith(second, start1, length, start2, StringComparison.Ordinal));
            start2 = 1;
            Assert.False(first.StartsWith(second, start1, length-1, start2, StringComparison.Ordinal));
        }
        [Fact]
        public void RemoveAccents_WithAccents()
        {
            var sample1 = @"Is there an encoding where an accented character like á or ä is treated as a single character?";
            var output1 = @"Is there an encoding where an accented character like a or a is treated as a single character?";

            var result = StringUtil.RemoveAccents(sample1, null);

            Assert.Equal(output1, result);
        }
        [Fact]
        public void RemoveAccents_Slugify2()
        {
            var sample1 = @"Is there an encoding where an accented character like á or ä is treated as a single character?";
            var output1 = @"is-there-an-encoding-where-an-accented-character-like-a-or-a-is-treated-as-a-single-character";

            var result = StringUtil.Slugify2(sample1,true,true);

            Assert.Equal(output1, result);
        }
        [Fact]
        public void RemoveAccents_Slugify2_AllowUnderscore()
        {
            var sample1 = @"Is there an encoding where an accented character like á or ä is treated as a single character?";
            var output1 = @"Is-there-an-encoding-where-an-accented-character-like-or-is-treated-as-a-single-character";

            var result = StringUtil.Slugify2(sample1,false,false, @"[^A-Za-z0-9\s_-]");

            Assert.Equal(output1, result);
        }
        [Fact]
        public void RemoveAccents_SlugifyTitle_Length100()
        {
            var sample1 = @"Is There the - top - of - an encoding where an accented character like á or ä is treated?";
            var output1 = @"is-there-the-top-of-an-encoding-where-an-accented-character-like-a-or-a-is-treated";

            var result = StringUtil.SlugifyTitle(sample1,100);

            Assert.Equal(output1, result);
        }
        [Fact]
        public void RemoveAccents_SlugifyTitle_Length50()
        {
            var sample1 = @"Is There the - top - of - an encoding where an accented character like á or ä is treated?";
            var output1 = @"is-there-the-top-of-an-encoding-where-an-accented-character";

            var result = StringUtil.SlugifyTitle(sample1, 60);

            Assert.Equal(output1, result);
        }
    }
}
