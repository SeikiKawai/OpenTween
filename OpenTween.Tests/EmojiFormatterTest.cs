﻿// OpenTween - Client of Twitter
// Copyright (c) 2015 kim_upsilon (@kim_upsilon) <https://upsilo.net/~upsilon/>
// All rights reserved.
//
// This file is part of OpenTween.
//
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
// for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenTween
{
    public class EmojiFormatterTest
    {
        [Fact]
        public void ReplaceEmojiToImg_Test()
        {
            var origText = "©"; // U+00A9

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/a9.png\" alt=\"©\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_SurrogatePairTest()
        {
            var origText = "🍣"; // U+1F363

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f363.png\" alt=\"🍣\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_CombiningCharacterTest()
        {
            var origText = "#⃣"; // U+0023 U+20E3 (合字)

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/23-20e3.png\" alt=\"#⃣\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_Unicode8Test()
        {
            // Unicode 8.0 で追加された絵文字
            var origText = "🌭"; // U+1F32D (HOT DOG)

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f32d.png\" alt=\"🌭\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_EmojiModifiers_CombiningTest()
        {
            // Emoji modifiers を使用した合字 (リガチャー)
            var origText = "👦\U0001F3FF"; // U+1F466 (BOY) + U+1F3FF (EMOJI MODIFIER FITZPATRICK TYPE-6)

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f466-1f3ff.png\" alt=\"👦\U0001F3FF\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_EmojiModifiers_SingleTest()
        {
            // Emoji modifiers は単体でも絵文字として表示される
            var origText = "\U0001F3FF"; // U+1F3FB (EMOJI MODIFIER FITZPATRICK TYPE-6)

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f3ff.png\" alt=\"\U0001F3FF\" />";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_NotEmojiTest()
        {
            var origText = "123ABC";

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "123ABC";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceEmojiToImg_HtmlTest()
        {
            // 属性内の絵文字は変換しない
            var origText = "🐟<a href='http://xn--7c9bw4k.jp/' title='🍣.jp'>🍣.jp</a>🐡";

            var result = EmojiFormatter.ReplaceEmojiToImg(origText);
            var expected = "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f41f.png\" alt=\"🐟\" />" +
                "<a href='http://xn--7c9bw4k.jp/' title='🍣.jp'>" +
                "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f363.png\" alt=\"🍣\" />.jp" +
                "</a>" +
                "<img class=\"emoji\" src=\"https://twemoji.maxcdn.com/2/72x72/1f421.png\" alt=\"🐡\" />";

            Assert.Equal(expected, result);
        }
    }
}
