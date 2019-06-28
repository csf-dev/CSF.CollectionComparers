//
// TestInt32Extensions.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using NUnit.Framework;
using CSF;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF
{
  [TestFixture]
  public class TestInt32Extensions
  {
    #region test code for generating references
    
    [Test]
    public void TestGenerateAlphabeticReferenceZeroBased()
    {
      Dictionary<int, string> expectedValues = GetExpectedReferencesZeroBased();
      
      foreach(int integer in expectedValues.Keys)
      {
        Assert.AreEqual(expectedValues[integer],
                        Int32Extensions.GenerateAlphabeticReference(integer, true),
                        String.Format("Test generation of reference for {0}", integer));
      }
    }
    
    [Test]
    public void TestGenerateAlphabeticReferenceNonZeroBased()
    {
      Dictionary<int, string> expectedValues = GetExpectedReferencesNonZeroBased();
      
      foreach(int integer in expectedValues.Keys)
      {
        Assert.AreEqual(expectedValues[integer],
                        Int32Extensions.GenerateAlphabeticReference(integer, false),
                        String.Format("Test generation of reference for {0}", integer));
      }
    }
    
    [Test]
    public void TestGenerateAlphabeticReferenceException()
    {
      Assert.That(() => Int32Extensions.GenerateAlphabeticReference(-1, true), Throws.InstanceOf<ArgumentException>());
    }
    
    #endregion
    
    #region test code for parsing references
    
    [Test]
    public void TestParseAlphabeticReferenceZeroBased()
    {
      Dictionary<int, string> expectedValues = GetExpectedReferencesZeroBased();
      
      foreach(int integer in expectedValues.Keys)
      {
        Assert.AreEqual(integer,
                        Int32Extensions.ParseAlphabeticReference(expectedValues[integer], true),
                        String.Format("Test parsing of reference for {0}", integer));
      }
    }
    
    [Test]
    public void TestParseAlphabeticReferenceNonZeroBased()
    {
      Dictionary<int, string> expectedValues = GetExpectedReferencesNonZeroBased();
      
      foreach(int integer in expectedValues.Keys)
      {
        Assert.AreEqual(integer,
                        Int32Extensions.ParseAlphabeticReference(expectedValues[integer], false),
                        String.Format("Test parsing of reference for {0}", integer));
      }
    }
    
    [Test]
    public void TestParseAlphabeticReferenceNull()
    {
      Assert.That(() => Int32Extensions.ParseAlphabeticReference(null, true), Throws.InstanceOf<ArgumentNullException>());
    }
    
    [Test]
    public void TestParseAlphabeticReferenceEmptyString()
    {
      Assert.That(() => Int32Extensions.ParseAlphabeticReference(String.Empty, true), Throws.InstanceOf<FormatException>());
    }
    
    [Test]
    public void TestParseAlphabeticReferenceInvalid()
    {
      ;
      Assert.That(() => Int32Extensions.ParseAlphabeticReference("a6c", true), Throws.InstanceOf<FormatException>());
    }
    
    [Test]
    public void TestToAlphabeticReference()
    {
      Dictionary<int, string>
        zeroBased = this.GetExpectedReferencesZeroBased(),
        nonZeroBased = this.GetExpectedReferencesNonZeroBased();
      
      foreach(int number in zeroBased.Keys)
      {
        Assert.AreEqual(zeroBased[number], number.ToAlphabeticReference(), "Correct reference");
      }
      
      foreach(int number in nonZeroBased.Keys)
      {
        Assert.AreEqual(nonZeroBased[number], number.ToAlphabeticReference(false), "Correct reference");
      }
    }
    
    #endregion
    
    #region other tests
    
    [Test]
    public void TestToBitNumbers()
    {
      IList<int> result;
      result = (45).ToBitNumbers();
      
      Assert.IsTrue(result.Contains(1),  "Contains value 1");
      Assert.IsTrue(result.Contains(4),  "Contains value 4");
      Assert.IsTrue(result.Contains(8),  "Contains value 8");
      Assert.IsTrue(result.Contains(32), "Contains value 32");
      Assert.AreEqual(4, result.Count, "Correct count");
      
      result = (-46).ToBitNumbers();
      
      Assert.IsTrue(result.Contains(-2),  "Contains value -2");
      Assert.IsTrue(result.Contains(-4),  "Contains value -4");
      Assert.IsTrue(result.Contains(-8),  "Contains value -8");
      Assert.IsTrue(result.Contains(-32), "Contains value -32");
      Assert.AreEqual(4, result.Count, "Correct count");
    }
    
    #endregion
    
    #region test values
    
    private Dictionary<int, string> GetExpectedReferencesZeroBased()
    {
      Dictionary<int, string> output = new Dictionary<int, string>();
      
      output.Add(   0,   "a");
      output.Add(   2,   "c");
      output.Add(  20,   "u");
      output.Add(  25,   "z");
      output.Add(  26,  "aa");
      output.Add(  27,  "ab");
      output.Add(  51,  "az");
      output.Add(  52,  "ba");
      output.Add(  53,  "bb");
      output.Add( 500,  "sg");
      output.Add( 600,  "wc");
      
      return output;
    }
    
    private Dictionary<int, string> GetExpectedReferencesNonZeroBased()
    {
      Dictionary<int, string> output = new Dictionary<int, string>();
      
      output.Add(   0,    "");
      output.Add(   2,   "b");
      output.Add(  20,   "t");
      output.Add(  26,   "z");
      output.Add(  27,  "aa");
      output.Add(  28,  "ab");
      output.Add(  52,  "az");
      output.Add(  53,  "ba");
      output.Add(  54,  "bb");
      output.Add( 500,  "sf");
      output.Add( 600,  "wb");
      output.Add(  -2,  "-b");
      output.Add( -20,  "-t");
      output.Add( -26,  "-z");
      output.Add( -27, "-aa");
      output.Add( -28, "-ab");
      output.Add( -52, "-az");
      output.Add( -53, "-ba");
      output.Add( -54, "-bb");
      output.Add(-500, "-sf");
      output.Add(-600, "-wb");
      
      return output;
    }
    
    #endregion
  }
}

