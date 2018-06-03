using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace CodeSharp.Parser.Tests
{
	

	public static class StringExtensions
	{
		public static string RemoveComments (this string sender)
		{
			int commentStartIndex;
			while ((commentStartIndex = sender.IndexOf ("/*")) != -1) {
				commentStartIndex += "/*".Length;
				var tmpString = sender.Substring (commentStartIndex);
				var nextClosedComment = tmpString.IndexOf ("*/") + "*/".Length;
				var firstPart = sender.Substring (0, commentStartIndex - 2);
				var secondPart = sender.Substring (commentStartIndex + nextClosedComment);
				sender = firstPart + secondPart;
			}
			return sender;
		}

		public static string RemoveTabSpaces (this string sender)
		{
			int startIndex = 0;
			bool hasEmptyOrTabChar;
			while (startIndex < sender.Length && (hasEmptyOrTabChar = (sender[startIndex] == ' ' || sender [startIndex] == '\t')) == true) {
				startIndex++;
			}

			if (startIndex == sender.Length - 1) 
				return "";

			int endIndex = sender.Length - 1;
			while (endIndex > startIndex + 1 && (hasEmptyOrTabChar = (sender [endIndex] == ' ' || sender [endIndex] == '\t')) == true) {
				if (hasEmptyOrTabChar) {
					endIndex--;
				}
			}
			return sender.Substring (startIndex, endIndex - startIndex);
		}

		public static string RemoveFormat (this string sender)
		{
			

			StringBuilder builder = new StringBuilder ();
			foreach (var item in sender.Split (new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)) {
				builder.AppendLine (item.Trim ().Trim ('\t'));
			}
			return builder.ToString ();
		}
	}

	///Users/josemedranojimenez/CodeSharp/CodeSharp.VSCode/node_modules/vscode/vscode.d.ts
	/// 
	/// 
	[TestFixture ()]
	public class MarshallingTest
	{
		[Test ()]
		public void RemoveComments ()
		{
			var comments = "/* ******** */this /* eeeeeee */is /**/a test".RemoveComments ();
			Assert.AreEqual ("this is a test", comments);
		}

		[Test ()]
		public void RemoveFormat ()
		{
			var comments = string.Format ("    1{0}\t2{0}\t\t3", Environment.NewLine);
			Assert.AreEqual (string.Format ("1{0}2{0}3", Environment.NewLine), comments.RemoveFormat ());
		}


		[Test ()]
		public void RemoveTabSpacs ()
		{
			var comments = "	 	e 	        dddd		  ".RemoveTabSpaces ();	
			Assert.AreEqual ("e 	        dddd", comments);
		}

	/**
	 * The version of the editor.
	 */
	

	}
}
