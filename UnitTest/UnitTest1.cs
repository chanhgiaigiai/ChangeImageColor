using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var list = new List<int> { 1, 2, 3, 4, 5, 6 };
			var evenNumberList = list
				.Where(item => item % 2 == 0);
			foreach (var item in list)
			{
				Debug.WriteLine(item);
			}
		}
	}
}
