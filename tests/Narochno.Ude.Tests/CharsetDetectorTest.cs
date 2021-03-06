// CharsetDetectorTest.cs created with MonoDevelop
//
// Author:
//   Rudi Pettazzi <rudi.pettazzi@gmail.com>
//

using System.IO;
using System.Text;
using Narochno.Ude.Core;
using Xunit;

namespace Narochno.Ude.Tests
{
    public class CharsetDetectorTest
    {
        ICharsetDetector detector;
        
        public CharsetDetectorTest()
        {
            detector = new CharsetDetector();
        }

        [Fact]
        public void TestASCII()
        {
            string s =  
                "The Documentation of the libraries is not complete " +
                "and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and " + 
                "click on the [Edit] link to start writing";
            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(s))) {
                detector.Feed(ms);
                detector.DataEnd();
                Assert.Equal(Charsets.ASCII, detector.Charset);
                Assert.Equal(1.0f, detector.Confidence);
            }
        }
            
        [Fact]
        public void TestUTF8_1()
        {
            string s = "ウィキペディアはオープンコンテントの百科事典です。基本方針に賛同し" + 
                       "ていただけるなら、誰でも記事を編集したり新しく作成したりできます。" +
                       "ガイドブックを読んでから、サンドボックスで練習してみましょう。質問は" + 
                       "利用案内でどうぞ。";
            byte[] buf = Encoding.UTF8.GetBytes(s);
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF8, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }


        [Fact]
        public void TestBomUTF8()
        {
            byte[] buf = { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x21 };
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF8, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }

        [Fact]
        public void TestBomUTF16_BE()
        {
            byte[] buf = { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65 };
            detector = new CharsetDetector();
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF16_BE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }
        
        [Fact]
        public void TestBomUTF16_LE()
        {
            byte[] buf = { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00 };
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF16_LE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }
        
        [Fact]
        public void TestBomUTF32_BE()
        {
            byte[] buf = { 0x00, 0x00, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x68 };
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF32_BE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }
        
        [Fact]
        public void TestBomUTF32_LE()
        {
            byte[] buf = { 0xFF, 0xFE, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00 };
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.UTF32_LE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }
		
		[Fact]
        public void TestIssue3()
        {
            byte[] buf = Encoding.UTF8.GetBytes("3");
            detector.Feed(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.ASCII, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);      
        }
    }
}

